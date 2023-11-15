// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Domestic.DomesticLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon.Config;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.Loyalty.Domestic;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Domestic
{
  internal class DomesticLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private static bool _isInitiliazed;
    private static Dictionary<Guid, DataRowItem> _excludedPrograms = new Dictionary<Guid, DataRowItem>();
    private static string _name;
    private static Guid _idGlobal;
    private PCX_CHEQUE_BL _pcxChequeBl = new PCX_CHEQUE_BL();
    private PCX_QUERY_LOG_BL _pcxQueryLogBl = new PCX_QUERY_LOG_BL();
    private PCX_CHEQUE_ITEM_BL _pcxChequeItemBl = new PCX_CHEQUE_ITEM_BL();
    private bool _isTransaction;
    private Stack<Guid> _transactions = new Stack<Guid>();
    private LoyaltyDomesticWebApi _api;

    private static Settings Settings { get; set; }

    private static Params Parameters { get; set; }

    private static bool IscompatibilityEnabled { get; set; }

    public override string Name => DomesticLoyaltyProgram._name;

    public override Guid IdGlobal => DomesticLoyaltyProgram._idGlobal;

    public DomesticLoyaltyProgram(string PublicId)
      : base(LoyaltyType.Domestic, PublicId, PublicId, "LP_DOMESTIC")
    {
    }

    public new void BeginTransaction() => this._isTransaction = true;

    public new void Commit()
    {
      this._isTransaction = false;
      this._transactions.Clear();
    }

    private void ClearTransations()
    {
      if (this._isTransaction)
        return;
      this._transactions.Clear();
    }

    protected override bool DoIsCompatibleTo(Guid discountId) => true;

    private ILpTransResult DoRefund(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      OperationType operationType)
    {
      this.ClearTransations();
      ApiRefundResponce apiRefundResponce = this._api.Refund(new ApiRefundRequest(this.ClientId, returnCheque.ID_CHEQUE_GLOBAL, baseCheque.ID_CHEQUE_GLOBAL, operationType));
      if (apiRefundResponce.IsCanceled)
        return (ILpTransResult) null;
      this._transactions.Push(apiRefundResponce.TransactionId);
      this.CreateCheque(baseCheque, 0M, operationType);
      Decimal chargedSum = operationType == OperationType.RefundCharge ? apiRefundResponce.RefundPoints / DomesticLoyaltyProgram.Parameters.PointsPerRub : 0M;
      Decimal refundPoints1 = operationType == OperationType.RefundDebit ? apiRefundResponce.RefundPoints : 0M;
      this.SavePcxCheque(returnCheque, refundPoints1, chargedSum, operationType, apiRefundResponce.TransactionId);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("Отмена ").Append(operationType == OperationType.RefundCharge ? "начисления " : "списания ").AppendLine(DomesticLoyaltyProgram._name);
      stringBuilder.Append("ШК карты: ").AppendLine(this.ClientPublicId);
      stringBuilder.Append("Дата/время: ").AppendLine(DateTime.Now.ToString("dd.MM.yy HH:mm:ss"));
      stringBuilder.AppendFormat(operationType == OperationType.RefundCharge ? "Списано: " : "Начислено ").Append(apiRefundResponce.RefundPoints.ToString("N2")).AppendLine(" баллов");
      stringBuilder.AppendFormat("Баланс: ").Append(apiRefundResponce.Balance.ToString("N2")).AppendLine(" баллов");
      Decimal refundPoints2 = operationType == OperationType.RefundCharge ? 0M : apiRefundResponce.RefundPoints;
      Decimal refundPoints3 = operationType == OperationType.RefundCharge ? apiRefundResponce.RefundPoints : 0M;
      LpTransResult lpTransResult = new LpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, refundPoints2, refundPoints3, apiRefundResponce.Balance, " баллов", true, true);
      lpTransResult.RoundingInCheque = DomesticLoyaltyProgram.Parameters.RoundingInCheque;
      return (ILpTransResult) lpTransResult;
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = this.DoRefund(baseCheque, returnCheque, OperationType.RefundCharge);
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = this.DoRefund(baseCheque, returnCheque, OperationType.RefundDebit);
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      this.ClearTransations();
      if (discountSum == 0M)
        return;
      ApiDebetResponce apiDebetResponce = this._api.Debet(new ApiDebetRequest(this.CreateCheque(cheque, discountSum, OperationType.Debit), this.ClientId));
      if (!(apiDebetResponce.DebitPoints > 0M))
        return;
      this._transactions.Push(apiDebetResponce.TransactionId);
      this.SavePcxCheque(cheque, discountSum, 0M, OperationType.Debit, apiDebetResponce.TransactionId);
      ref ILpTransResult local = ref result;
      LpTransResult lpTransResult1 = new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, 0M, apiDebetResponce.DebitPoints, apiDebetResponce.Balance, "баллов", false, true);
      lpTransResult1.RoundingInCheque = DomesticLoyaltyProgram.Parameters.RoundingInCheque;
      LpTransResult lpTransResult2 = lpTransResult1;
      local = (ILpTransResult) lpTransResult2;
    }

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (cheque.SUMM + discountSum < DomesticLoyaltyProgram.Parameters.MinChequeSumForCharge)
        return;
      this.ClearTransations();
      ApiChargeResponce apiChargeResponce = this._api.Charge(new ApiChargeRequest(this.CreateCheque(cheque, discountSum, OperationType.Charge), this.ClientId));
      if (!(apiChargeResponce.ChargedPoints > 0M))
        return;
      this._transactions.Push(apiChargeResponce.TransactionId);
      this.SavePcxCheque(cheque, discountSum, apiChargeResponce.ChargedPoints / DomesticLoyaltyProgram.Parameters.PointsPerRub, OperationType.Charge, apiChargeResponce.TransactionId);
      ref ILpTransResult local = ref result;
      LpTransResult lpTransResult1 = new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, apiChargeResponce.ChargedPoints, 0M, apiChargeResponce.Balance, "баллов", false, true);
      lpTransResult1.RoundingInCheque = DomesticLoyaltyProgram.Parameters.RoundingInCheque;
      LpTransResult lpTransResult2 = lpTransResult1;
      local = (ILpTransResult) lpTransResult2;
    }

    private Cheque CreateCheque(CHEQUE cheque, Decimal discountSum, OperationType operationType)
    {
      Cheque cheque1 = new Cheque()
      {
        OperationType = operationType,
        ChequeId = cheque.ID_CHEQUE_GLOBAL,
        DiscountSum = discountSum
      };
      Decimal summ = cheque.SUMM;
      foreach (CHEQUE_ITEM chequeItem1 in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        Decimal num = chequeItem1.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (discountItem => !(discountItem.TYPE == "LP_DOMESTIC") ? 0M : discountItem.AMOUNT));
        summ += num;
        ChequeItem chequeItem2 = new ChequeItem()
        {
          Id = chequeItem1.ID_CHEQUE_ITEM_GLOBAL,
          LotId = chequeItem1.ID_LOT_GLOBAL,
          Quantity = chequeItem1.QUANTITY,
          Sum = chequeItem1.SUMM,
          DiscountSum = num
        };
        cheque1.Items.Add(chequeItem2);
      }
      cheque1.Sum = summ;
      return cheque1;
    }

    private void SavePcxCheque(
      CHEQUE cheque,
      Decimal discountSum,
      Decimal chargedSum,
      OperationType operationType,
      Guid transactionId)
    {
      List<PrepaymentInfo> prepaymentInfoList = cheque.GetPrepaymentInfoList();
      bool flag = DomesticLoyaltyProgram.Parameters != null && DomesticLoyaltyProgram.Parameters.ApplyDiscountAsPrepayment;
      discountSum = flag ? prepaymentInfoList.Where<PrepaymentInfo>((Func<PrepaymentInfo, bool>) (item => item.ApplyDiscountAsPrepayment = true)).Sum<PrepaymentInfo>((Func<PrepaymentInfo, Decimal>) (item => item.MaxSumm)) : cheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => ci.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => !(mi.TYPE == "LP_DOMESTIC") ? 0M : mi.AMOUNT))));
      string str = string.Empty;
      int num1 = 0;
      Decimal num2 = 0M;
      switch (operationType)
      {
        case OperationType.Charge:
          str = "CHARGE";
          num1 = 3;
          num2 = chargedSum;
          break;
        case OperationType.Debit:
          str = "DEBIT";
          num1 = 2;
          num2 = discountSum;
          break;
        case OperationType.RefundCharge:
          str = "CHARGE_REFUND";
          num1 = 5;
          num2 = chargedSum;
          break;
        case OperationType.RefundDebit:
          str = "DEBIT_REFUND";
          num1 = 4;
          num2 = discountSum;
          break;
      }
      PCX_QUERY_LOG logRecord = new PCX_QUERY_LOG()
      {
        ID_USER_GLOBAL = SecurityContextEx.USER_GUID,
        ID_QUERY_GLOBAL = Guid.NewGuid(),
        STATE = 4,
        ID_CASH_REGISTER = AppConfigManager.IdCashRegister,
        DATE_REQUEST = DateTime.Now,
        ID_CHEQUE_GLOBAL = cheque.ID_CHEQUE_GLOBAL,
        SUMM = num2,
        TYPE = num1,
        CLIENT_ID_TYPE = (int) this.LoyaltyType,
        CLIENT_ID = this.ClientId,
        TRANSACTION_ID = transactionId.ToString()
      };
      PCX_CHEQUE cheque1 = new PCX_CHEQUE()
      {
        CLIENT_ID = this.ClientId,
        CLIENT_ID_TYPE = (int) this.LoyaltyType,
        SUMM = num2,
        SUMM_MONEY = num2,
        SCORE = num2,
        SUMM_SCORE = num2,
        PARTNER_ID = string.Empty,
        LOCATION = string.Empty,
        TERMINAL = string.Empty,
        ID_CHEQUE_GLOBAL = cheque.ID_CHEQUE_GLOBAL,
        OPER_TYPE = str,
        CARD_NUMBER = this.ClientPublicId,
        TRANSACTION_ID = transactionId.ToString()
      };
      Dictionary<Guid, Decimal> chequeItems = new Dictionary<Guid, Decimal>();
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        Decimal num3 = flag ? 0M : chequeItem.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.TYPE == "LP_DOMESTIC")).Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (dmi => dmi.AMOUNT));
        Decimal num4 = chequeItem.SUMM + num3;
        chequeItems.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, num4);
      }
      Decimal chequeSum = cheque.SUMM + (flag ? 0M : discountSum);
      IDictionary<Guid, Decimal> dictionary = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, chequeSum, Math.Abs(discountSum));
      List<PCX_CHEQUE_ITEM> chequeItemList = new List<PCX_CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM()
        {
          ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL,
          TRANSACTION_ID = transactionId.ToString(),
          CLIENT_ID = this.ClientId,
          CLIENT_ID_TYPE = (int) this.LoyaltyType,
          SUMM_SCORE = 0M,
          SUMM = dictionary[chequeItem.ID_CHEQUE_ITEM_GLOBAL],
          STATUS = pcxOperationStatus.Online.ToString(),
          OPER_TYPE = str
        };
        chequeItemList.Add(pcxChequeItem);
      }
      this._pcxQueryLogBl.Save(logRecord);
      this._pcxChequeBl.Save(cheque1);
      if (chequeItemList.Count <= 0)
        return;
      this._pcxChequeItemBl.Save(chequeItemList);
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
      Decimal num1 = cheque.SUMM + this.GetDiscountSum(cheque);
      LoyaltyCardInfo loyaltyCardInfo = this.GetLoyaltyCardInfo(false);
      Decimal num2 = num1 - (Decimal) (DomesticLoyaltyProgram.Parameters == null || !DomesticLoyaltyProgram.Parameters.ApplyDiscountAsPrepayment ? 1 : 0);
      if (DomesticLoyaltyProgram.Parameters.MinPayPercent > 0M)
        num2 = Math.Truncate(Math.Min(num1 - num1 * (DomesticLoyaltyProgram.Parameters.MinPayPercent / 100M), num2));
      if (num1 < DomesticLoyaltyProgram.Parameters.MinChequeSumForCharge)
        num2 = 0M;
      return Math.Min(num2, loyaltyCardInfo.Balance);
    }

    protected override void DoRollback(out string slipCheque)
    {
      StringBuilder stringBuilder = new StringBuilder();
      while (this._transactions.Count > 0)
      {
        Guid transactionId = this._transactions.Pop();
        ApiRollbackResponce rollbackResponce = this._api.Rollback(transactionId);
        if (stringBuilder.Length > 0)
          stringBuilder.AppendLine();
        stringBuilder.Append("Отмена транзакции ").AppendLine(this.Name);
        stringBuilder.Append("ШК карты: ").AppendLine(this.ClientPublicId);
        stringBuilder.Append("Баланс карты: ").AppendLine(rollbackResponce.Balance.ToString("N2"));
        string whereExpression = string.Format("TRANSACTION_ID = '{0}'", (object) transactionId);
        BaseLoyaltyProgramEx.PCXChequeItemLoader.Delete(whereExpression, ServerType.Local);
        BaseLoyaltyProgramEx.PCXChequeLoader.Delete(whereExpression, ServerType.Local);
        new PCX_QUERY_LOG_BL().ReverseQuery(transactionId.ToString());
      }
      slipCheque = stringBuilder.ToString();
    }

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      Card clientCardInfo = this._api.GetClientCardInfo(this.ClientId);
      LoyaltyCardInfo cardInfoFromService = new LoyaltyCardInfo()
      {
        ClientId = this.ClientId,
        CardStatusId = clientCardInfo.CARD_STATUS
      };
      cardInfoFromService.CardStatus = SettingsModel.GetLoyaltyCardStatusName(cardInfoFromService.CardStatusId);
      cardInfoFromService.CardNumber = cardInfoFromService.ClientId;
      cardInfoFromService.Points = clientCardInfo.BALANCE;
      cardInfoFromService.Balance = clientCardInfo.BALANCE;
      cardInfoFromService.TransactionsCountInDay = clientCardInfo.TRANSACTIONS_COUNT_IN_DAY;
      cardInfoFromService.TransactionsCountInMonth = clientCardInfo.TRANSACTIONS_COUNT_IN_MONTH;
      return cardInfoFromService;
    }

    protected override void OnInitInternal()
    {
    }

    protected override void OnInitSettings()
    {
      if (!DomesticLoyaltyProgram._isInitiliazed)
      {
        SettingsModel settingsModel = new SettingsModel();
        LoyaltySettings loyaltySettings = settingsModel.Load(LoyaltyType.Domestic, Guid.Empty);
        DomesticLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
        DomesticLoyaltyProgram.Parameters = settingsModel.Deserialize<Params>(loyaltySettings.PARAMS, "Params");
        DomesticLoyaltyProgram.Parameters.PointsPerRub = DomesticLoyaltyProgram.Parameters.PointsPerRub == 0M ? 1M : DomesticLoyaltyProgram.Parameters.PointsPerRub;
        DomesticLoyaltyProgram._name = DomesticLoyaltyProgram.Settings.Name;
        DomesticLoyaltyProgram._idGlobal = loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL;
      }
      DomesticLoyaltyProgram._isInitiliazed = true;
      if (this._api != null)
        return;
      this._api = new LoyaltyDomesticWebApi(DomesticLoyaltyProgram.Settings, SecurityContextEx.Context.User.Password_hash);
    }

    public override LoyaltyParams GetLoyaltyParams() => (LoyaltyParams) DomesticLoyaltyProgram.Parameters;
  }
}
