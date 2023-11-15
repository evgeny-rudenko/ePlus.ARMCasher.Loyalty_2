// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.AstraZeneca.AstraZenecaLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMBusinessLogic.Caches.BarcodesCaches;
using ePlus.ARMCacheManager;
using ePlus.ARMCacheManager.Interfaces;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.AstraZeneca.Forms;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.Loyalty.AstraZeneca;
using ePlus.Loyalty.AstraZeneca.API;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.AstraZeneca
{
  internal class AstraZenecaLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private static Guid _id = new Guid("647190D8-DC71-46FB-9112-5C3021256404");
    private static string DiscountType = "AZ_EX";
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();
    private AstraZenecaWebApi azAPI;
    private string name = "АстраЗенека";
    private Dictionary<string, List<long>> extBarcodeCache;
    private RequestGetDiscount latestRequest;
    private ResponseGetDiscount latestResponse;

    private static bool IscompatibilityEnabled { get; set; }

    private static Settings Settings { get; set; }

    private string PosId { get; set; }

    protected override bool OnIsExplicitDiscount => false;

    public AstraZenecaLoyaltyProgram(string publicId, string posId)
      : base(LoyaltyType.AstraZeneca, publicId, publicId, "LP_AZ")
    {
      this.SendRecvTimeout = 30;
      this.PosId = posId;
    }

    public override string Name => this.name;

    public override Guid IdGlobal => AstraZenecaLoyaltyProgram._id;

    protected override bool DoIsCompatibleTo(Guid discountId) => AstraZenecaLoyaltyProgram.IscompatibilityEnabled && !AstraZenecaLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      this.OnTraceMessage("Подтверждение транзакций Вместе онлайн");
      List<CHEQUE_ITEM_TRANSACTION> list = cheque.CHEQUE_ITEMS.Where<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => ci.Transaction != null)).Select<CHEQUE_ITEM, CHEQUE_ITEM_TRANSACTION>((Func<CHEQUE_ITEM, CHEQUE_ITEM_TRANSACTION>) (ci => ci.Transaction)).ToList<CHEQUE_ITEM_TRANSACTION>();
      if (list.Any<CHEQUE_ITEM_TRANSACTION>())
        this.ConfirmAzTransacrions(this.ClientPublicId, (IEnumerable<CHEQUE_ITEM_TRANSACTION>) list);
      this.OnTraceMessage("Подтверждение транзакций Вместе онлайн завершено");
      this.ConfirmStroredAzTransactionsAsync();
    }

    public Task ConfirmStroredAzTransactionsAsync() => Task.Factory.StartNew(new Action(this.ConfirmStoredAzTransactions));

    public void ConfirmStoredAzTransactions()
    {
            /*
            this.InitInternal();
          Dictionary<\u003C\u003Ef__AnonymousType0<Guid, string>, List<CHEQUE_ITEM_TRANSACTION>> dictionary = new AzTransactionsBl().GetListUnconfirmed().GroupBy(t => new
          {
            ID_CHEQUE_GLOBAL = t.ID_CHEQUE_GLOBAL,
            CLIENT_ID = t.CLIENT_ID
          }).ToDictionary(g => g.Key, g => g.ToList<CHEQUE_ITEM_TRANSACTION>());
          foreach (var key in dictionary.Keys)
            this.ConfirmAzTransacrions(key.CLIENT_ID, (IEnumerable<CHEQUE_ITEM_TRANSACTION>) dictionary[key]);
            */
            base.InitInternal();
            var dictionary = (
                from t in (new AzTransactionsBl()).GetListUnconfirmed()
                group t by new { ID_CHEQUE_GLOBAL = t.ID_CHEQUE_GLOBAL, CLIENT_ID = t.CLIENT_ID }).ToDictionary((g) => g.Key, (g) => g.ToList<CHEQUE_ITEM_TRANSACTION>());
            foreach (var key in dictionary.Keys)
            {
                this.ConfirmAzTransacrions(key.CLIENT_ID, dictionary[key]);
            }

    }

        private void SaveAzTransactions(IEnumerable<CHEQUE_ITEM_TRANSACTION> transactions) => new AzTransactionsBl().SaveEx(transactions);

    private void ConfirmAzTransacrions(
      string clientId,
      IEnumerable<CHEQUE_ITEM_TRANSACTION> transactions)
    {
      PublicIdType publicIdType = this.GetPublicIdType(clientId);
      RequestConfirmPurchase request = this.azAPI.CreateRequest<RequestConfirmPurchase>(this.PosId);
      request.CardNumber = publicIdType == PublicIdType.CardNumber ? clientId : (string) null;
      request.PhoneNumber = publicIdType == PublicIdType.Phone ? "+" + clientId : (string) null;
      request.Transactions = transactions.Select<CHEQUE_ITEM_TRANSACTION, string>((Func<CHEQUE_ITEM_TRANSACTION, string>) (t => t.TRANSACTION_ID));
      try
      {
        Response response = this.azAPI.ConfirmPurchase(request);
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
        this.SaveAzTransactions(transactions);
      }
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => this.DoProcess(cheque, discountSum, out result);

    private void DoProcess(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (this.latestRequest == null)
        return;
      List<OrderResponse> list = this.azAPI.GetDiscount(this.latestRequest).Orders.ToList<OrderResponse>();
      foreach (CHEQUE_ITEM chequeItem in cheque.CHEQUE_ITEMS.Where<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => ci.Transaction != null)))
      {
        CHEQUE_ITEM item = chequeItem;
        OrderResponse orderResponse = list.FirstOrDefault<OrderResponse>((Func<OrderResponse, bool>) (o => o.AnyData == item.ID_LOT_GLOBAL.ToString()));
        list.Remove(orderResponse);
        if (orderResponse == null)
        {
          item.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.TYPE == AstraZenecaLoyaltyProgram.DiscountType));
          item.Transaction = (CHEQUE_ITEM_TRANSACTION) null;
        }
        else
          item.Transaction.TRANSACTION_ID = orderResponse.Transaction;
      }
      if (list.Count > 0)
        throw new ApplicationException("Не удалось связать ответ программы лояльности со строками чека");
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
      ResponseGetDiscount discount = this.azAPI.GetDiscount(request);
      if (discount.IsSuccess)
      {
        if (discount.ErrorCode == 2)
        {
          this.OnTraceMessage("запрашиваем код подтверждения");
          using (FormConfirmationCode confirmationCode = new FormConfirmationCode())
          {
            int num = (int) confirmationCode.ShowDialog();
          }
        }
        else
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
      if (this.azAPI != null)
        return;
      this.azAPI = new AstraZenecaWebApi(AstraZenecaLoyaltyProgram.Settings);
    }

    protected override void OnInitSettings()
    {
      if (AstraZenecaLoyaltyProgram.Settings != null)
        return;
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      AstraZenecaLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      this.name = AstraZenecaLoyaltyProgram.Settings.Name;
      AstraZenecaLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (!AstraZenecaLoyaltyProgram.IscompatibilityEnabled)
        return;
      AstraZenecaLoyaltyProgram.ExcludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
        AstraZenecaLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
      {
        if (!(exclude.Guid == ARM_DISCOUNT2_PROGRAM.AstraZenecaDiscountGUID))
          AstraZenecaLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      }
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
        AstraZenecaLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
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
            AnyData = chequeItem.ID_LOT_GLOBAL.ToString()
          };
          orderItems.Add(order);
        }
      }
      return (IEnumerable<Order>) orderItems;
    }

    protected virtual string GetBarcode(CHEQUE_ITEM item)
    {
      if (this.extBarcodeCache == null)
        this.extBarcodeCache = new ExternalBarcodesCache((ICacheManager) new MemoryCacheManager(MemoryCache.Default)).GetCache();
      if (string.IsNullOrWhiteSpace(item.BARCODE))
      {
        string str = this.extBarcodeCache.Where<KeyValuePair<string, List<long>>>((Func<KeyValuePair<string, List<long>>, bool>) (cache => cache.Value.Contains(item.ID_GOODS))).Select<KeyValuePair<string, List<long>>, string>((Func<KeyValuePair<string, List<long>>, string>) (cache => cache.Key)).LastOrDefault<string>();
        item.BARCODE = str;
      }
      return item.BARCODE;
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
      LoyaltyCard loyaltyCard = cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is AstraZenecaCard)) as LoyaltyCard;
      this.GetLoyaltyCardInfo(false);
      RequestGetDiscount request = this.azAPI.CreateRequest<RequestGetDiscount>(this.PosId);
      PublicIdType publicIdType = this.GetPublicIdType(this.ClientPublicId);
      request.CardNumber = publicIdType == PublicIdType.CardNumber ? this.ClientPublicId : (string) null;
      string str = (string) null;
      if (!string.IsNullOrEmpty(this.ClientPublicId))
        str = this.ClientPublicId.Contains("+") ? this.ClientPublicId : "+" + this.ClientPublicId;
      request.PhoneNumber = publicIdType == PublicIdType.Phone ? str : (string) null;
      request.Orders = this.CreateOrderItems(cheque);
      if (!request.Orders.Any<Order>())
        return;
      this.OnTraceMessage("отправляем запрос на получение скидки.");
      ResponseGetDiscount responseGetDiscount = (ResponseGetDiscount) null;
      if (request.Orders.Any<Order>())
      {
        this.latestRequest = request;
        responseGetDiscount = this.azAPI.CheckDiscount(request);
        this.latestResponse = responseGetDiscount;
      }
      this.OnTraceMessage("обрабатываем результат.");
      loyaltyCard.ExtraDiscounts.Clear();
      cheque.CHEQUE_ITEMS.ForEach((Action<CHEQUE_ITEM>) (ci =>
      {
        ci.Transaction = (CHEQUE_ITEM_TRANSACTION) null;
        ci.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.TYPE == AstraZenecaLoyaltyProgram.DiscountType));
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
            CHEQUE_ITEM chequeItem = cheque.CHEQUE_ITEMS.FirstOrDefault<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => order.AnyData == ci.ID_LOT_GLOBAL.ToString() && ci.Transaction == null));
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
                ID_DISCOUNT2_GLOBAL = ARM_DISCOUNT2_PROGRAM.AstraZenecaDiscountGUID,
                DISCOUNT2_NAME = order.Message,
                TYPE = AstraZenecaLoyaltyProgram.DiscountType,
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
      this.OnInitSettings();
      List<string> list = ((IEnumerable<string>) AstraZenecaLoyaltyProgram.Settings.CardPrefix.Trim().Split(',')).Select<string, string>((Func<string, string>) (x => x.Trim())).ToList<string>();
      return AstraZenecaLoyaltyProgram.Settings != null && list.Any<string>((Func<string, bool>) (x => clientId.Length > x.Length && this.CardPrefixFound(clientId, x))) ? PublicIdType.CardNumber : PublicIdType.Phone;
    }

    private bool CardPrefixFound(string clientId, string prefixes = null)
    {
      if (prefixes == null)
        prefixes = AstraZenecaLoyaltyProgram.Settings.CardPrefix;
      if (prefixes.Contains<char>(';'))
      {
        string str1 = prefixes;
        char[] chArray = new char[1]{ ';' };
        foreach (string str2 in str1.Split(chArray))
        {
          if (clientId.StartsWith(str2))
            return true;
        }
      }
      else if (clientId.StartsWith(prefixes) && clientId.Length >= prefixes.Length)
        return true;
      return false;
    }

    protected override void DoRollback(out string slipCheque) => slipCheque = string.Empty;
  }
}
