// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.BaseLoyaltyProgramEx
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.Forms;
using ePlus.ARMCommon.Config;
using ePlus.ARMCommon.Log;
using ePlus.ARMUtils;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty
{
  public abstract class BaseLoyaltyProgramEx : ILoyaltyProgram
  {
    protected static readonly object SyncObj = new object();
    protected static readonly ConnectionStateSqlLoader<PCX_CHEQUE> PCXChequeLoader = new ConnectionStateSqlLoader<PCX_CHEQUE>("PCX_CHEQUE");
    protected static readonly ConnectionStateSqlLoader<PCX_CHEQUE_ITEM> PCXChequeItemLoader = new ConnectionStateSqlLoader<PCX_CHEQUE_ITEM>("PCX_CHEQUE_ITEM");
    protected static readonly PCX_QUERY_LOG_BL QueryLogBl = new PCX_QUERY_LOG_BL();
    protected static readonly PCX_CHEQUE_BL PcxChequeBl = new PCX_CHEQUE_BL();
    protected static readonly PCX_CHEQUE_ITEM_BL PcxChequeItemBl = new PCX_CHEQUE_ITEM_BL();
    private Stack<LoyaltyTransaction> _transactionStack = new Stack<LoyaltyTransaction>();
    private LoyaltyCardInfo? _loyaltyCardInfo;
    private Stack<LoyaltyTransaction> TransactionStack = new Stack<LoyaltyTransaction>();
    private List<ILpTransResult> operations = new List<ILpTransResult>();
    protected FrmWaiting waitingForm;
    private Regex phoneRegex = new Regex("^[+]{0,1}7[0-9]{10}$");

    protected string DiscountType { get; private set; }

    protected bool IsTransactionProcessing { get; private set; }

    protected int SendRecvTimeout { get; set; }

    public abstract string Name { get; }

    public abstract Guid IdGlobal { get; }

    public LoyaltyType LoyaltyType { get; private set; }

    public Guid LoyaltyInstance { get; set; }

    protected string ClientId { get; private set; }

    protected string ClientPublicId { get; private set; }

    protected PublicIdType ClientPublicIdType { get; private set; }

    protected virtual bool IsInitialized { get; set; }

    protected virtual bool IsSettingsInitialized { get; set; }

    private bool IscompatibilityEnabled { get; set; }

    public virtual event EventHandler CheckCodeConfirmationEvent;

    public virtual bool SuccessCodeConfirmation { get; set; }

    public virtual IEnumerable<string> PersonalAdditionsSale { get; set; }

    public virtual int SortOrder => 0;

    public virtual string GetDebitOperationDescription() => string.Format("Списание баллов по ПЛ \"{0}\"", (object) this.Name);

    protected void UpdateClientPublicId(string newClientPublicId)
    {
      this.ClientPublicId = newClientPublicId;
      this.ClientPublicIdType = this.GetPublicIdType(this.ClientPublicId);
    }

    protected abstract bool DoIsCompatibleTo(Guid discountId);

    protected abstract void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result);

    protected abstract void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result);

    [Obsolete]
    protected abstract void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result);

    [Obsolete]
    protected abstract void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result);

    protected virtual void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundedCheques,
      out ILpTransResult result)
    {
      this.DoRefundCharge(baseCheque, returnCheque, out result);
    }

    protected virtual void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundedCheques,
      out ILpTransResult result)
    {
      this.DoRefundDebit(baseCheque, returnCheque, out result);
    }

    protected abstract LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form);

    protected abstract void OnInitInternal();

    protected abstract void OnInitSettings();

    public abstract Decimal CalculateMaxSumBonus(CHEQUE cheque);

    public virtual void Rollback(out string slipCheque)
    {
      this.ClearOperations();
      this.DoRollback(out slipCheque);
    }

    protected abstract void DoRollback(out string slipCheque);

    public Decimal CalculateDiscountSum(CHEQUE cheque) => cheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => ci.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => !(mi.TYPE == this.DiscountType) ? 0.0M : mi.AMOUNT))));

    public void PreOrderCalculation(CHEQUE cheque)
    {
      if (this.IsExplicitDiscount)
        return;
      this.InitInternal();
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Предварительный расчет заказа";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((sender, e) => this.DoPreOrderCalculation(cheque));
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
    }

    protected virtual void DoPreOrderCalculation(CHEQUE cheque) => this.CalculateMaxSumBonus(cheque);

    public virtual bool IsPreOrderCalculationRequired => false;

    public bool IsCompatibleTo(Guid discountId)
    {
      this.InitInternal();
      return this.LoyaltyType.IsUsedAsDiscount() || this.DoIsCompatibleTo(discountId);
    }

    protected virtual void RegisterInDatabase(ILpTransResult result)
    {
      if (result == null || result.IsRegistered)
        return;
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(result.IdChequeGlobal, 0M, 0);
      logQueryLog.DATE_REQUEST = DateTime.Now;
      logQueryLog.DATE_RESPONSE = logQueryLog.DATE_REQUEST;
      logQueryLog.STATE = 4;
      logQueryLog.STATUS = "Online";
      if (result.ChargedSum > 0M)
      {
        this.SaveLoyaltyInfo(result.IdChequeGlobal, result.ChargedSum, result.IsRefund ? "DEBIT_REFUND" : "CHARGE", result.TransactionId);
        logQueryLog.SUMM = result.ChargedSum;
        logQueryLog.TYPE = result.IsRefund ? 4 : 3;
        this.SaveQueryLog(logQueryLog);
      }
      if (!(result.DebitSum > 0M))
        return;
      PCX_QUERY_LOG log = (PCX_QUERY_LOG) logQueryLog.Clone();
      log.ID_QUERY_GLOBAL = Guid.NewGuid();
      this.SaveLoyaltyInfo(result.IdChequeGlobal, result.DebitSum, result.IsRefund ? "CHARGE_REFUND" : "DEBIT", result.TransactionId);
      log.SUMM = result.DebitSum;
      log.TYPE = result.IsRefund ? 5 : 2;
      this.SaveQueryLog(log);
    }

    private void AddOperation(ILpTransResult operation)
    {
      if (operation == null)
        return;
      this.operations.Add(operation);
    }

    private void RegisterOperations() => this.operations.ForEach((Action<ILpTransResult>) (op => this.RegisterInDatabase(op)));

    private void ClearOperations() => this.operations.Clear();

    public ILpTransResult Charge(CHEQUE cheque, Decimal discountSum)
    {
      this.InitInternal();
      ILpTransResult result = (ILpTransResult) null;
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Начисление";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((sender, e) => this.DoCharge(cheque, discountSum, out result));
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      this.AddOperation(result);
      return result;
    }

    public ILpTransResult Debit(CHEQUE cheque, Decimal discountSum, bool submit)
    {
      this.InitInternal();
      ILpTransResult result = (ILpTransResult) null;
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        this.waitingForm = frmWaiting;
        frmWaiting.Text = "Списание";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        if (discountSum > 0M)
        {
          this.CheckCodeConfirmation(cheque, discountSum);
          if (!this.SuccessCodeConfirmation)
            discountSum = 0M;
        }
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((sender, e) => this.DoDebit(cheque, discountSum, out result));
        DialogResult dialogResult = frmWaiting.ShowDialog();
        this.waitingForm = (FrmWaiting) null;
        if (dialogResult != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      this.AddOperation(result);
      return result;
    }

    protected virtual void CheckCodeConfirmation(CHEQUE cheque, Decimal discountSum) => this.SuccessCodeConfirmation = true;

    public ILpTransResult RefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundCheques)
    {
      this.InitInternal();
      ILpTransResult result = (ILpTransResult) null;
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Возврат начисления";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((sender, e) => this.DoRefundCharge(baseCheque, returnCheque, refundCheques, out result));
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      this.AddOperation(result);
      return result;
    }

    public ILpTransResult RefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundedCheques)
    {
      this.InitInternal();
      ILpTransResult result = (ILpTransResult) null;
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Возврат списания";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((sender, e) => this.DoRefundDebit(baseCheque, returnCheque, refundedCheques, out result));
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      this.AddOperation(result);
      return result;
    }

    public LoyaltyCardInfo GetLoyaltyCardInfo(bool resetCache = false)
    {
      this.InitInternal();
      if (!this._loyaltyCardInfo.HasValue || resetCache)
      {
        using (FrmWaiting frm = new FrmWaiting())
        {
          frm.Text = "Получение данных по карте";
          frm.WaitingTimeout = this.SendRecvTimeout;
          frm.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) => this._loyaltyCardInfo = new LoyaltyCardInfo?(this.DoGetLoyaltyCardInfoFromService((Form) frm)));
          if (frm.ShowDialog() != DialogResult.Yes)
            throw frm.Exception;
        }
      }
      return this._loyaltyCardInfo.Value;
    }

    public void BeginTransaction()
    {
      this.TransactionStack.Clear();
      this.IsTransactionProcessing = true;
    }

    public void Commit()
    {
      this.IsTransactionProcessing = false;
      this.TransactionStack.Clear();
      this.RegisterOperations();
    }

    protected LoyaltyTransaction SaveTransaction(
      OperTypeEnum operation,
      Decimal operationSum,
      LpTransactionData transactionData)
    {
      if (!this.IsTransactionProcessing)
        this._transactionStack.Clear();
      LoyaltyTransaction loyaltyTransaction = new LoyaltyTransaction(operation)
      {
        OperationSum = operationSum,
        Data = transactionData
      };
      this._transactionStack.Push(loyaltyTransaction);
      return loyaltyTransaction;
    }

    protected bool PopLastTransaction(out LoyaltyTransaction transaction)
    {
      if (this._transactionStack.Count > 0)
      {
        transaction = this._transactionStack.Pop();
        return true;
      }
      transaction = (LoyaltyTransaction) null;
      return false;
    }

    protected void InitInternal()
    {
      if (!this.IsSettingsInitialized)
      {
        if (this.ClientPublicIdType == PublicIdType.Unknown)
        {
          this.ClientPublicIdType = this.GetPublicIdType(this.ClientPublicId);
          if (this.ClientPublicIdType == PublicIdType.Phone)
            this.ClientPublicId = this.ClientPublicId.Replace("+", "");
        }
        this.OnInitSettings();
        this.IsSettingsInitialized = true;
      }
      if (this.IsInitialized)
        return;
      lock (BaseLoyaltyProgramEx.SyncObj)
      {
        if (this.IsInitialized)
          return;
        try
        {
          this.OnInitInternal();
        }
        catch (NonCriticalInitializationException ex)
        {
          ARMLogger.Error(ex.ToString());
        }
        catch (Exception ex)
        {
          StringBuilder stringBuilder = new StringBuilder();
          stringBuilder.AppendLine("Ошибка инициализации объекта");
          stringBuilder.Append("Работа с программой лояльности \"").Append(this.Name).AppendLine("\" невозможна");
          stringBuilder.AppendLine(ex.Message);
          throw new Exception(stringBuilder.ToString());
        }
        this.IsInitialized = true;
      }
    }

    protected PCX_CHEQUE CreatePCXCheque(
      Guid idChequeGlobal,
      Decimal sumMoney,
      Decimal chequeSum,
      string operType,
      Decimal sumScore)
    {
      return new PCX_CHEQUE()
      {
        CLIENT_ID = this.ClientId,
        CLIENT_ID_TYPE = (int) this.LoyaltyType,
        SUMM = chequeSum,
        SUMM_MONEY = sumMoney,
        SCORE = 0M,
        SUMM_SCORE = sumScore,
        PARTNER_ID = string.Empty,
        LOCATION = string.Empty,
        TERMINAL = string.Empty,
        ID_CHEQUE_GLOBAL = idChequeGlobal,
        OPER_TYPE = operType,
        CARD_NUMBER = this.GetLoyaltyCardInfo(false).CardNumber
      };
    }

    private PCX_CHEQUE_ITEM CreatePcxChequeItem(
      Guid idChequeItemGlobal,
      Decimal quantity,
      Decimal itemSum)
    {
      return new PCX_CHEQUE_ITEM()
      {
        ID_CHEQUE_ITEM_GLOBAL = idChequeItemGlobal,
        QUANTITY = quantity,
        PRICE = UtilsArm.Round(itemSum / quantity),
        SUMM = UtilsArm.RoundDown(itemSum)
      };
    }

    protected PCX_QUERY_LOG CreateLogQueryLog(Guid idChequeGlobal, Decimal sum, int type) => new PCX_QUERY_LOG()
    {
      ID_USER_GLOBAL = SecurityContextEx.USER_GUID,
      ID_QUERY_GLOBAL = Guid.NewGuid(),
      STATE = 1,
      ID_CASH_REGISTER = AppConfigManager.IdCashRegister,
      DATE_REQUEST = DateTime.Now,
      ID_CHEQUE_GLOBAL = idChequeGlobal,
      SUMM = sum,
      TYPE = type,
      CLIENT_ID_TYPE = (int) this.LoyaltyType,
      CLIENT_ID = this.ClientId
    };

    protected void SaveQueryLog(PCX_QUERY_LOG log) => BaseLoyaltyProgramEx.QueryLogBl.Save(log);

    protected void SaveQueryLog(
      Guid idChequeGlobal,
      Decimal summ,
      pcxOperation operationType,
      DateTime responseDate)
    {
      this.SaveQueryLog(this.CreateLogQueryLog(idChequeGlobal, summ, (int) operationType));
    }

    protected void Log(
      OperTypeEnum loyaltyOperation,
      Decimal sum,
      CHEQUE cheque,
      int? resultCode = null,
      string transactionId = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append('"').Append(this.Name).Append('"');
      stringBuilder.Append(" Номер карты: ").Append(this.ClientPublicId).Append(", ");
      stringBuilder.Append(this.GetLoyaltyOperationDescription(loyaltyOperation));
      stringBuilder.Append(",  Скидка: ").Append(sum.ToString("N2"));
      stringBuilder.Append(" Чек: ").Append((object) cheque.ID_CHEQUE_GLOBAL);
      if (!string.IsNullOrEmpty(transactionId))
        stringBuilder.Append(" ID транзакции: ").Append(transactionId);
      if (resultCode.HasValue)
        stringBuilder.Append(" Результат операции: ").Append(resultCode.Value < 0 ? "Ошибка: " : "OK: ").Append((object) resultCode);
      LoyaltyLogger.Info(stringBuilder.ToString());
    }

    protected void Log(string message) => LoyaltyLogger.Info(message);

    protected void LogError(OperTypeEnum loyaltyOperation, string message) => LoyaltyLogger.Error(string.Format("\"{0}\" Номер карты: {1}, Операция: {2}, Сообщение: {3}", (object) this.Name, (object) this.ClientPublicId, (object) this.GetLoyaltyOperationDescription(loyaltyOperation), (object) message));

    protected void LogMsg(OperTypeEnum loyaltyOperation, string message) => LoyaltyLogger.Error(string.Format("\"{0}\" Номер карты: {1}, Операция: {2}, Сообщение: {3}", (object) this.Name, (object) this.ClientPublicId, (object) this.GetLoyaltyOperationDescription(loyaltyOperation), (object) message));

    protected void LogMsg(string message) => LoyaltyLogger.Error(string.Format("\"{0}\" Номер карты: {1}, Сообщение: {2}", (object) this.Name, (object) this.ClientPublicId, (object) message));

    protected void LogError(OperTypeEnum loyaltyOperation, Exception exception) => this.LogError(loyaltyOperation, exception.Message);

    protected string GetLoyaltyOperationDescription(OperTypeEnum loyaltyOperation)
    {
      switch (loyaltyOperation)
      {
        case OperTypeEnum.Debit:
          return "Списание";
        case OperTypeEnum.Charge:
          return "Начисление";
        case OperTypeEnum.DebitRefund:
          return "Возврат списания";
        case OperTypeEnum.ChargeRefund:
          return "Возврат начисления";
        case OperTypeEnum.Rollback:
          return "Откат транзакции";
        default:
          return "";
      }
    }

    protected Decimal GetDiscountSum(CHEQUE cheque) => cheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => this.GetDiscountSum(ci)));

    protected Decimal GetDiscountSum(CHEQUE_ITEM chequeItem) => chequeItem.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => mi.TYPE == null || !mi.TYPE.StartsWith(this.DiscountType) ? 0M : mi.AMOUNT));

    protected Decimal GetChequeItemSideDiscountSum(CHEQUE_ITEM chequeItem) => chequeItem.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => mi.TYPE == null || !mi.TYPE.StartsWith(this.DiscountType) ? mi.AMOUNT : 0.0M));

    protected void SaveLoyaltyInfo(
      Guid idChequeGlobal,
      Decimal sum,
      string operationType,
      string transactionId)
    {
      PCX_CHEQUE cheque = new PCX_CHEQUE()
      {
        ID_CHEQUE_GLOBAL = idChequeGlobal,
        CLIENT_ID = this.ClientPublicId,
        CLIENT_ID_TYPE = (int) this.LoyaltyType,
        SUMM = sum,
        CARD_NUMBER = this.ClientPublicId,
        OPER_TYPE = operationType,
        TRANSACTION_ID = transactionId
      };
      BaseLoyaltyProgramEx.PcxChequeBl.Save(cheque);
    }

    protected void SavePcxCheque(
      CHEQUE cheque,
      Decimal discountSum,
      string operationType,
      string transactionId)
    {
      PCX_CHEQUE cheque1 = new PCX_CHEQUE()
      {
        CLIENT_ID = this.ClientId,
        CLIENT_ID_TYPE = (int) this.LoyaltyType,
        SUMM = cheque.SUMM + discountSum,
        SUMM_MONEY = cheque.SUMM + discountSum,
        SCORE = 0M,
        SUMM_SCORE = 0M,
        PARTNER_ID = string.Empty,
        LOCATION = string.Empty,
        TERMINAL = string.Empty,
        ID_CHEQUE_GLOBAL = cheque.ID_CHEQUE_GLOBAL,
        OPER_TYPE = operationType,
        CARD_NUMBER = this.ClientPublicId
      };
      List<PCX_CHEQUE_ITEM> chequeItemList = new List<PCX_CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        Decimal num = chequeItem.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (i => !(i.TYPE == this.DiscountType) ? 0.0M : i.AMOUNT));
        PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM()
        {
          CLIENT_ID = this.ClientId,
          CLIENT_ID_TYPE = (int) this.LoyaltyType,
          ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL,
          ID_CHEQUE_ITEM_GLOBAL_NEW = chequeItem.ID_CHEQUE_ITEM_GLOBAL,
          OPER_TYPE = operationType,
          PRICE = chequeItem.PRICE,
          QUANTITY = chequeItem.QUANTITY,
          STATUS = string.Empty,
          SUMM = num,
          SUMM_SCORE = num,
          TRANSACTION_ID = transactionId
        };
        chequeItemList.Add(pcxChequeItem);
      }
      BaseLoyaltyProgramEx.PcxChequeBl.Save(cheque1);
      BaseLoyaltyProgramEx.PcxChequeItemBl.Save(chequeItemList);
    }

    public BaseLoyaltyProgramEx(
      LoyaltyType loyaltyType,
      string clientId,
      string clientPublicId,
      string discountType)
    {
      this.LoyaltyType = loyaltyType;
      this.ClientId = clientId;
      this.ClientPublicId = clientPublicId;
      this.DiscountType = discountType;
    }

    protected virtual bool OnIsExplicitDiscount => true;

    public bool IsExplicitDiscount => this.OnIsExplicitDiscount;

    protected virtual PublicIdType GetPublicIdType(string publicId)
    {
      if (this.phoneRegex.IsMatch(publicId))
        return PublicIdType.Phone;
      return publicId.Contains("@") ? PublicIdType.EMail : PublicIdType.CardNumber;
    }

    protected virtual bool DoUpdateLoyaltyCardInfo(LoyaltyCardInfo oldInfo, LoyaltyCardInfo newInfo) => false;

    public bool UpdateLoyaltyCardInfo(LoyaltyCardInfo oldInfo, LoyaltyCardInfo newInfo)
    {
      this.InitInternal();
      bool result = false;
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Замена/восстановление карты";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) => result = this.DoUpdateLoyaltyCardInfo(oldInfo, newInfo));
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      return result;
    }

    public virtual bool IsUpdateLoyaltyCardInfoSupported => false;

    public LoyaltyCardInfo? GetLoyaltyCardInfo(CHEQUE cheque)
    {
      this.InitInternal();
      LoyaltyCardInfo? result = new LoyaltyCardInfo?();
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Получение данных по карте";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) => result = this.DoGetLoyaltyCardInfoByCheque(cheque));
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      if (result.HasValue)
      {
        this._loyaltyCardInfo = result;
        this.ClientPublicId = result.Value.ClientId;
        this.ClientPublicIdType = result.Value.ClientIdType;
      }
      return result;
    }

    protected virtual LoyaltyCardInfo? DoGetLoyaltyCardInfoByCheque(CHEQUE cheque) => new LoyaltyCardInfo?();

    public bool SendAuthenticationCode()
    {
      this.InitInternal();
      bool result = false;
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.Text = "Отправка SMS кода подтверждения...";
        frmWaiting.WaitingTimeout = this.SendRecvTimeout;
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) => result = this.OnSendAuthenticationCode());
        if (frmWaiting.ShowDialog() != DialogResult.Yes)
          throw frmWaiting.Exception;
      }
      return result;
    }

    protected virtual bool OnSendAuthenticationCode() => false;

    public virtual LoyaltyCard GetLoyaltyCard() => (LoyaltyCard) null;

    public virtual LoyaltyParams GetLoyaltyParams() => (LoyaltyParams) null;

    protected virtual string FormatMessage(string message, params object[] obj) => string.Format(this.Name + ": " + message, obj);

    protected virtual void OnErrorMessage(string message, params object[] obj) => ARMLogger.Error(this.FormatMessage(message, obj));

    protected virtual void OnTraceMessage(string message, params object[] obj) => ARMLogger.Trace(this.FormatMessage(message, obj));

    protected virtual void OnInfoMessage(string message, params object[] obj) => ARMLogger.Info(this.FormatMessage(message, obj));

    public virtual bool RequestCodeConfirmation(CHEQUE cheque) => true;

    public virtual void CodeConfirmation(CHEQUE cheque, string code)
    {
    }

    public virtual void RequestPersonalAdditionSales()
    {
    }
  }
}
