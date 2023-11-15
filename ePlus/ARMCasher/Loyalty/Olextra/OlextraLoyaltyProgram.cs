// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Olextra.OlextraLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using Dapper;
using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.Loyalty.Olextra;
using ePlus.Loyalty.Olextra.API;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Olextra
{
  internal class OlextraLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private static Guid _id = new Guid("B98825C2-926E-4E65-BFE2-421D3265ABE1");
    private static string DiscountType = "OL_EX";
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();
    private OlextraWebApi olextraWebApi;
    private string name = "Олекстра";
    private Dictionary<string, List<long>> extBarcodeCache;
    private RequestGetDiscount latestRequest;
    private ResponseGetDiscount latestResponse;
    private List<AllowedBarcode> barcodeCache;

    private static bool IscompatibilityEnabled { get; set; }

    private static ePlus.Loyalty.Olextra.Settings Settings { get; set; }

    private string PosId { get; set; }

    protected override bool OnIsExplicitDiscount => false;

    public OlextraLoyaltyProgram(string publicId, string posId)
      : base(LoyaltyType.Olextra, publicId, publicId, "LP_OL")
    {
      this.SendRecvTimeout = 30;
      this.PosId = posId;
    }

    public override string Name => this.name;

    public override Guid IdGlobal => OlextraLoyaltyProgram._id;

    protected override bool DoIsCompatibleTo(Guid discountId) => OlextraLoyaltyProgram.IscompatibilityEnabled && !OlextraLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      this.OnTraceMessage("Подтверждение транзакций Олекстра");
      List<CHEQUE_ITEM_TRANSACTION> list = cheque.CHEQUE_ITEMS.Where<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => ci.Transaction != null)).Select<CHEQUE_ITEM, CHEQUE_ITEM_TRANSACTION>((Func<CHEQUE_ITEM, CHEQUE_ITEM_TRANSACTION>) (ci => ci.Transaction)).ToList<CHEQUE_ITEM_TRANSACTION>();
      if (list.Any<CHEQUE_ITEM_TRANSACTION>())
        this.ConfirmOlextraTransactions(this.ClientPublicId, (IEnumerable<CHEQUE_ITEM_TRANSACTION>) list);
      this.OnTraceMessage("Подтверждение транзакций Олекстра завершено");
      this.ConfirmStroredOlextraTransactionsAsync();
    }

    public Task ConfirmStroredOlextraTransactionsAsync() => Task.Factory.StartNew(new Action(this.ConfirmStoredOlextraTransactions));

    public void ConfirmStoredOlextraTransactions()
    {
            /*
              this.InitInternal();
            Dictionary<\u003C\u003Ef__AnonymousType0<Guid, string>, List<CHEQUE_ITEM_TRANSACTION>> dictionary = new OlextraTransactionsBl().GetListUnconfirmed().GroupBy(t => new
            {
              ID_CHEQUE_GLOBAL = t.ID_CHEQUE_GLOBAL,
              CLIENT_ID = t.CLIENT_ID
            }).ToDictionary(g => g.Key, g => g.ToList<CHEQUE_ITEM_TRANSACTION>());
            foreach (var key in dictionary.Keys)
              this.ConfirmOlextraTransactions(key.CLIENT_ID, (IEnumerable<CHEQUE_ITEM_TRANSACTION>) dictionary[key]);
            */
            base.InitInternal();
            var dictionary = (
                from t in (new OlextraTransactionsBl()).GetListUnconfirmed()
                group t by new { ID_CHEQUE_GLOBAL = t.ID_CHEQUE_GLOBAL, CLIENT_ID = t.CLIENT_ID }).ToDictionary((g) => g.Key, (g) => g.ToList<CHEQUE_ITEM_TRANSACTION>());
            foreach (var key in dictionary.Keys)
            {
                this.ConfirmOlextraTransactions(key.CLIENT_ID, dictionary[key]);
            }

        }

        private void SaveOlextraTransactions(IEnumerable<CHEQUE_ITEM_TRANSACTION> transactions) => new OlextraTransactionsBl().SaveEx(transactions);

    private void ConfirmOlextraTransactions(
      string clientId,
      IEnumerable<CHEQUE_ITEM_TRANSACTION> transactions)
    {
      PublicIdType publicIdType = this.GetPublicIdType(clientId);
      RequestConfirmPurchase request = this.olextraWebApi.CreateRequest<RequestConfirmPurchase>(this.PosId);
      request.CardNumber = publicIdType == PublicIdType.CardNumber ? clientId : (string) null;
      request.PhoneNumber = publicIdType == PublicIdType.Phone ? "+" + clientId : (string) null;
      request.Transactions = transactions.Select<CHEQUE_ITEM_TRANSACTION, string>((Func<CHEQUE_ITEM_TRANSACTION, string>) (t => t.TRANSACTION_ID));
      try
      {
        Response response = this.olextraWebApi.ConfirmPurchase(request);
        if (response.IsSuccess)
        {
          this.OnTraceMessage("обновляем статус транзакций в БД");
          foreach (CHEQUE_ITEM_TRANSACTION transaction in transactions)
          {
            transaction.TransactionStatus = ChequeItemTransactionStatus.Confirmed;
            transaction.ERROR_CODE = response.ErrorCode;
            transaction.MESSAGE = response.Message;
            ++transaction.ATTEMPTS_NUMBER;
          }
        }
        else
        {
          this.OnErrorMessage("ошибка при подтверждении транзакций на сервере API");
          this.OnErrorMessage(response.Message);
          foreach (CHEQUE_ITEM_TRANSACTION transaction in transactions)
          {
            transaction.TransactionStatus = ChequeItemTransactionStatus.Error;
            transaction.ERROR_CODE = response.ErrorCode;
            transaction.MESSAGE = response.Message;
            ++transaction.ATTEMPTS_NUMBER;
          }
        }
      }
      catch (Exception ex)
      {
        this.OnErrorMessage("ошибка при подтверждении транзакций на сервере API");
        this.OnErrorMessage(ex.ToString());
        foreach (CHEQUE_ITEM_TRANSACTION transaction in transactions)
        {
          transaction.MESSAGE = ex.Message;
          ++transaction.ATTEMPTS_NUMBER;
        }
      }
      finally
      {
        this.SaveOlextraTransactions(transactions);
      }
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => this.DoProcess(cheque, discountSum, out result);

    private void DoProcess(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (this.latestRequest == null)
        return;
      List<OrderResponse> list = this.olextraWebApi.GetDiscount(this.latestRequest).Orders.ToList<OrderResponse>();
      foreach (CHEQUE_ITEM chequeItem in cheque.CHEQUE_ITEMS.Where<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => ci.Transaction != null)))
      {
        CHEQUE_ITEM item = chequeItem;
        string barcode = this.GetBarcode(item);
        OrderResponse orderResponse = list.FirstOrDefault<OrderResponse>((Func<OrderResponse, bool>) (o => o.Barcode == barcode && o.Price == item.PRICE));
        list.Remove(orderResponse);
        if (orderResponse == null)
        {
          item.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.TYPE == OlextraLoyaltyProgram.DiscountType));
          item.Transaction = (CHEQUE_ITEM_TRANSACTION) null;
        }
        else
          item.Transaction.TRANSACTION_ID = orderResponse.Transaction;
      }
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
    }

    private ResponseGetDiscount GetDiscount(RequestGetDiscount request)
    {
      ResponseGetDiscount discount = this.olextraWebApi.GetDiscount(request);
      if (discount.IsSuccess)
      {
        this.OnInfoMessage("status - {2}; error_code - {0}; message - {1}", (object) discount.ErrorCode, (object) discount.Message, (object) discount.Status);
        return discount;
      }
      throw new LoyaltyException((ILoyaltyProgram) this, this.FormatMessage("status - {2} error_code - {0}; message - {1}", (object) discount.ErrorCode, (object) discount.Message, (object) discount.Status));
    }

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      this.UpdateClientPublicId(this.ClientPublicId);
      return new LoyaltyCardInfo()
      {
        ClientId = this.ClientPublicId,
        ClientIdType = this.ClientPublicIdType,
        CardNumber = this.ClientPublicId,
        Balance = 0M,
        CardStatus = "Активна",
        CardStatusId = LoyaltyCardStatus.Active
      };
    }

    protected override void OnInitInternal()
    {
      if (this.olextraWebApi != null)
        return;
      this.olextraWebApi = new OlextraWebApi(OlextraLoyaltyProgram.Settings);
    }

    protected override void OnInitSettings()
    {
      if (OlextraLoyaltyProgram.Settings != null)
        return;
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      OlextraLoyaltyProgram.Settings = settingsModel.Deserialize<ePlus.Loyalty.Olextra.Settings>(loyaltySettings.SETTINGS, "Settings");
      this.name = OlextraLoyaltyProgram.Settings.Name;
      OlextraLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (!OlextraLoyaltyProgram.IscompatibilityEnabled)
        return;
      OlextraLoyaltyProgram.ExcludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
        OlextraLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
      {
        if (!(exclude.Guid == ARM_DISCOUNT2_PROGRAM.OlextraDiscountGUID))
          OlextraLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      }
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
        OlextraLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
    }

    private IEnumerable<Order> CreateOrderItems(CHEQUE cheque)
    {
      List<Order> orderItems = new List<Order>();
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        string barcode = this.GetBarcode(chequeItem);
        if (!string.IsNullOrEmpty(barcode))
        {
          Order order = new Order()
          {
            Barcode = barcode,
            Price = chequeItem.SUMM / chequeItem.QUANTITY,
            Count = Convert.ToInt32(chequeItem.QUANTITY),
            QrCode = chequeItem.IS_KIZ ? chequeItem.KIZ : ""
          };
          orderItems.Add(order);
        }
      }
      return (IEnumerable<Order>) orderItems;
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
      this.CalculateDiscount(cheque);
      return 0M;
    }

    private void CalculateDiscount(CHEQUE cheque)
    {
      this.latestRequest = (RequestGetDiscount) null;
      if (cheque.CHEQUE_ITEMS == null || cheque.CHEQUE_ITEMS.Count <= 0)
        return;
      LoyaltyCard loyaltyCard = cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is OlextraCard)) as LoyaltyCard;
      RequestGetDiscount request = this.olextraWebApi.CreateRequest<RequestGetDiscount>(this.PosId);
      request.CardNumber = this.ClientPublicId;
      request.Orders = this.CreateOrderItems(cheque);
      if (!request.Orders.Any<Order>())
        return;
      this.OnTraceMessage("отправляем запрос на получение скидки.");
      ResponseGetDiscount responseGetDiscount = (ResponseGetDiscount) null;
      if (request.Orders.Any<Order>())
      {
        this.latestRequest = request;
        responseGetDiscount = this.olextraWebApi.GetDiscount(request);
        this.latestResponse = responseGetDiscount;
      }
      this.OnTraceMessage("обрабатываем результат.");
      loyaltyCard.ExtraDiscounts.Clear();
      cheque.CHEQUE_ITEMS.ForEach((Action<CHEQUE_ITEM>) (ci =>
      {
        ci.Transaction = (CHEQUE_ITEM_TRANSACTION) null;
        ci.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.TYPE == OlextraLoyaltyProgram.DiscountType));
      }));
      if (responseGetDiscount != null)
      {
        if (loyaltyCard is ILoyaltyMessageList loyaltyMessageList && responseGetDiscount.Description != null)
        {
          LoyaltyMessage message = new LoyaltyMessage(LoyaltyMessageType.Service, string.Format("{0}{2}{1}", (object) responseGetDiscount.Description, (object) responseGetDiscount.Message, (object) Environment.NewLine));
          loyaltyMessageList.Add((ILoyaltyMessage) message);
        }
        foreach (OrderResponse order1 in responseGetDiscount.Orders)
        {
          OrderResponse order = order1;
          if (order.Discount > 0M)
          {
            CHEQUE_ITEM chequeItem = cheque.CHEQUE_ITEMS.FirstOrDefault<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => this.GetBarcode(ci) != null && this.GetBarcode(ci).Equals(order.Barcode) && ci.Transaction == null));
            if (chequeItem != null)
            {
              this.OnTraceMessage("найдена позиция чека [BARCODE={0}, LOT_ID={1}]", (object) this.GetBarcode(chequeItem), (object) chequeItem.ID_LOT_GLOBAL);
              this.OnTraceMessage("номер транзакции [Transaction={0}]", (object) order.Transaction);
              Decimal num = chequeItem.SUMM - order.Value;
              chequeItem.Transaction = new CHEQUE_ITEM_TRANSACTION()
              {
                TRANSACTION_ID = order.Transaction,
                ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL,
                SUM_DISCOUNT = num,
                BARCODE = order.Barcode,
                CLIENT_ID = order.CardNumber
              };
              DISCOUNT_VALUE_INFO discountValueInfo = new DISCOUNT_VALUE_INFO()
              {
                BARCODE = loyaltyCard.BARCODE,
                ID_DISCOUNT2_GLOBAL = ARM_DISCOUNT2_PROGRAM.OlextraDiscountGUID,
                DISCOUNT2_NAME = order.Message,
                TYPE = OlextraLoyaltyProgram.DiscountType,
                ID_LOT_GLOBAL = chequeItem.ID_LOT_GLOBAL,
                VALUE = num
              };
              if (discountValueInfo.VALUE < 0M)
                this.OnErrorMessage("Ошибка! Рассчетная скидка меньше нуля: LOT_PRICE_VAT_ORIGINAL={0}, QUANTITY={1}, AZ_ORDER_VALUE={2}", (object) chequeItem.LOT_PRICE_VAT_ORIGINAL, (object) chequeItem.QUANTITY, (object) order.Value);
              else
                loyaltyCard.ExtraDiscounts.Add(discountValueInfo);
            }
          }
        }
      }
      cheque.CalculateFields();
    }

    protected override PublicIdType GetPublicIdType(string clientId)
    {
      if (string.IsNullOrWhiteSpace(clientId))
        return PublicIdType.Unknown;
      return OlextraLoyaltyProgram.Settings != null && !string.IsNullOrWhiteSpace(OlextraLoyaltyProgram.Settings.CardPrefix) && this.CardPrefixFound(clientId) ? PublicIdType.CardNumber : PublicIdType.Phone;
    }

    private bool CardPrefixFound(string clientId)
    {
      if (OlextraLoyaltyProgram.Settings.CardPrefix.Contains<char>(';'))
      {
        string cardPrefix = OlextraLoyaltyProgram.Settings.CardPrefix;
        char[] chArray = new char[1]{ ';' };
        foreach (string str in cardPrefix.Split(chArray))
        {
          if (clientId.StartsWith(str))
            return true;
        }
      }
      else if (clientId.StartsWith(OlextraLoyaltyProgram.Settings.CardPrefix) && clientId.Length >= OlextraLoyaltyProgram.Settings.CardPrefix.Length)
        return true;
      return false;
    }

    protected override void DoRollback(out string slipCheque) => slipCheque = string.Empty;

    private void FillBarcodeCache()
    {
      this.barcodeCache = new List<AllowedBarcode>();
      using (SqlConnection cnn = new SqlConnection(MultiServerBL.ClientConnectionString))
      {
        string sql = " \r\n                    select \r\n\t                    ID_GOODS_GLOBAL,\r\n                        BARCODE\r\n                            FROM ALLOWED_BARCODE_OLEXTRA ab                            \r\n                        where DATE_DELETED IS NULL";
        List<AllowedBarcode> list = cnn.Query<AllowedBarcode>(sql).ToList<AllowedBarcode>();
        if (list == null)
          return;
        this.barcodeCache = list;
      }
    }

    protected string GetBarcode(CHEQUE_ITEM item)
    {
      if (this.barcodeCache == null)
        this.FillBarcodeCache();
      AllowedBarcode allowedBarcode = this.barcodeCache.FirstOrDefault<AllowedBarcode>((Func<AllowedBarcode, bool>) (ab => ab.ID_GOODS_GLOBAL == item.idGoodsGlobal));
      return allowedBarcode == null ? (string) null : allowedBarcode.BARCODE;
    }
  }
}
