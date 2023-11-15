// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.GoldenMiddle.GoldenMiddleLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessLogic.Events;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon;
using ePlus.ARMCommon.Config;
using ePlus.ARMCommon.Log;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.Loyalty.GoldenMiddle;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wscardax;

namespace ePlus.ARMCasher.Loyalty.GoldenMiddle
{
  internal class GoldenMiddleLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private const string NumericFormat = "###0.##";
    private static readonly object _syncLock = new object();
    private static readonly Guid _chequeOperTypeCharge = new Guid("EC6238C3-3630-42F2-89FF-678F77D4F074");
    private static readonly Guid _chequeOperTypeDebit = new Guid("24FB0197-1E6D-4F52-BE2E-BFE880139C12");
    private static readonly Guid _chequeOperTypeRefundCharge = new Guid("92622C53-F13B-4B83-8D90-0DD9D8A25519");
    private static readonly Guid _chequeOperTypeRefundDebit = new Guid("6F3B3F66-0087-4E1E-82E1-D139EFC458B5");
    private static Guid _id = new Guid("9DA22AE8-94D9-4665-A208-0609A53EFCAE");
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();
    private static POSProcessAX _posAx;
    private static readonly GoldenMiddle_Bl _bl = new GoldenMiddle_Bl();

    private Guid ChequeOperTypeCharge => GoldenMiddleLoyaltyProgram._chequeOperTypeCharge;

    private Guid ChequeOperTypeDebit => GoldenMiddleLoyaltyProgram._chequeOperTypeDebit;

    private Guid ChequeOperTypeRefundCharge => GoldenMiddleLoyaltyProgram._chequeOperTypeRefundCharge;

    private Guid ChequeOperTypeRefundDebit => GoldenMiddleLoyaltyProgram._chequeOperTypeRefundDebit;

    private static bool IscompatibilityEnabled { get; set; }

    private static Settings Settings { get; set; }

    public POSProcessAX PosAx
    {
      get
      {
        lock (GoldenMiddleLoyaltyProgram._syncLock)
          return GoldenMiddleLoyaltyProgram._posAx;
      }
    }

    public override string Name => "Золотая середина";

    public override Guid IdGlobal => GoldenMiddleLoyaltyProgram._id;

    protected override bool DoIsCompatibleTo(Guid discountId) => GoldenMiddleLoyaltyProgram.IscompatibilityEnabled && !GoldenMiddleLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (discountSum > 0M)
        return;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeCharge);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Начисление баллов на карту лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторное начисление произведено не будет.", (object) this.Name, (object) cheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        this.Log(OperTypeEnum.Charge, discountSum, cheque);
        this.FillCheque(cheque);
        string str = cheque.SUMM.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture);
        int ErrCode;
        string ErrText;
        ((IPOSProcessAX33) this.PosAx).MakeChequeWithBonusCount(this.ClientId, out ErrCode, out ErrText, cheque.ID_CHEQUE_GLOBAL.ToString(), str, str);
        if (ErrCode == 0)
        {
          this.LogMsg(OperTypeEnum.Charge, ErrText);
          GMTransactionData transactionData = new GMTransactionData(cheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeCharge, ((IPOSProcessAX33) this.PosAx).TransactionID);
          this.SaveTransaction(OperTypeEnum.Charge, discountSum, (LpTransactionData) transactionData);
          BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
        }
        else
          this.OnException(OperTypeEnum.Charge, ErrText);
        this.SaveQueryLog(cheque.ID_CHEQUE_GLOBAL, ((IPOSProcessAX33) this.PosAx).ProcessedDT, discountSum, pcxOperation.Charge, ((IPOSProcessAX33) this.PosAx).TransactionID);
        this.SavePcxCheque(cheque, discountSum, "CHARGE", ((IPOSProcessAX33) this.PosAx).TransactionID);
        Decimal result1;
        Decimal result2;
        if (!Decimal.TryParse(((IPOSProcessAX33) this.PosAx).ChargedBonus, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result1) || !Decimal.TryParse(((IPOSProcessAX33) this.PosAx).Balance, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2))
          return;
        result = (ILpTransResult) new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, result1, 0M, result2, string.Empty, false, true);
      }
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (discountSum == 0M)
        return;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeDebit);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Списание баллов с карты лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторное списание произведено не будет.", (object) this.Name, (object) cheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        this.Log(OperTypeEnum.Debit, discountSum, cheque);
        this.FillCheque(cheque);
        Decimal discountSum1 = this.GetDiscountSum(cheque);
        Decimal num1 = cheque.SUMM + discountSum1;
        Decimal num2 = discountSum1 / num1 * 100M;
        ((IPOSProcessAX33) this.PosAx).ChequeIsSoft = false;
        int ErrCode;
        string ErrText;
        ((IPOSProcessAX33) this.PosAx).MakeChequeWithBonusDiscount(this.ClientId, out ErrCode, out ErrText, cheque.ID_CHEQUE_GLOBAL.ToString(), num1.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture), num1.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture), discountSum1.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture));
        if (ErrCode == 0)
        {
          this.LogMsg(OperTypeEnum.Debit, ErrText);
          GMTransactionData transactionData = new GMTransactionData(cheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeDebit, ((IPOSProcessAX33) this.PosAx).TransactionID);
          this.SaveTransaction(OperTypeEnum.Debit, discountSum, (LpTransactionData) transactionData);
          BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
        }
        else
          this.OnException(OperTypeEnum.Debit, ErrText);
        this.SaveQueryLog(cheque.ID_CHEQUE_GLOBAL, ((IPOSProcessAX33) this.PosAx).ProcessedDT, discountSum, pcxOperation.Debit, ((IPOSProcessAX33) this.PosAx).TransactionID);
        this.SavePcxCheque(cheque, discountSum, "DEBIT", ((IPOSProcessAX33) this.PosAx).TransactionID);
        Decimal result1;
        Decimal result2;
        Decimal result3;
        if (!Decimal.TryParse(((IPOSProcessAX33) this.PosAx).DiscountedBonus, out result1) || !Decimal.TryParse(((IPOSProcessAX33) this.PosAx).ChargedBonus, out result2) || !Decimal.TryParse(((IPOSProcessAX33) this.PosAx).Balance, out result3))
          return;
        result = (ILpTransResult) new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, result2 > 0M ? result2 : 0M, result1, result3, string.Empty, false, true);
      }
    }

    private void SaveQueryLog(
      Guid idChequeGlobal,
      DateTime responseDate,
      Decimal sum,
      pcxOperation operType,
      string transactionID)
    {
      PCX_QUERY_LOG logRecord = new PCX_QUERY_LOG()
      {
        ID_USER_GLOBAL = SecurityContextEx.USER_GUID,
        ID_QUERY_GLOBAL = Guid.NewGuid(),
        STATE = 4,
        ID_CASH_REGISTER = AppConfigManager.IdCashRegister,
        DATE_REQUEST = DateTime.Now,
        DATE_RESPONSE = responseDate,
        ID_CHEQUE_GLOBAL = idChequeGlobal,
        SUMM = sum,
        TRANSACTION_ID = transactionID,
        TYPE = (int) operType,
        CLIENT_ID_TYPE = (int) this.LoyaltyType,
        CLIENT_ID = this.ClientId
      };
      BaseLoyaltyProgramEx.QueryLogBl.Save(logRecord);
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      Decimal discountSum = returnCheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => ci.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => !(mi.TYPE == "LP_GM") ? 0M : mi.AMOUNT))));
      this.DoRefund(baseCheque, returnCheque, discountSum, out result);
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      Decimal discountSum = returnCheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => ci.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => !(mi.TYPE == "LP_GM") ? 0M : mi.AMOUNT))));
      this.DoRefund(baseCheque, returnCheque, discountSum, out result);
    }

    private void DoRefund(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      Decimal discountSum,
      out ILpTransResult result)
    {
      string empty = string.Empty;
      result = (ILpTransResult) null;
      pcxOperation pcxOperation1 = discountSum > 0M ? pcxOperation.Debit : pcxOperation.Charge;
      pcxOperation pcxOperation2 = discountSum > 0M ? pcxOperation.RefundDebit : pcxOperation.RefundCharge;
      OperTypeEnum operTypeEnum = discountSum > 0M ? OperTypeEnum.DebitRefund : OperTypeEnum.ChargeRefund;
      Guid guid = discountSum > 0M ? this.ChequeOperTypeRefundDebit : this.ChequeOperTypeRefundCharge;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(baseCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, guid);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Возврат по карте лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторный возврат произведен не будет.", (object) this.Name, (object) baseCheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        PaymentType chequePaymentType = baseCheque.CHEQUE_PAYMENT_TYPE;
        switch (chequePaymentType)
        {
          case PaymentType.Cash:
          case PaymentType.Card:
            if (chequePaymentType != PaymentType.Mixed || !baseCheque.CHEQUE_PAYMENTS.All<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (p => p.SEPARATE_TYPE_ENUM == PaymentType.Card || p.SEPARATE_TYPE_ENUM == PaymentType.Cash)))
            {
              PCX_QUERY_LOG pcxQueryLog = BaseLoyaltyProgramEx.QueryLogBl.Load(baseCheque.ID_CHEQUE_GLOBAL, pcxOperation1, (int) this.LoyaltyType);
              if (pcxQueryLog == null)
                return;
              this.Log(operTypeEnum, 0M, baseCheque);
              this.FillCheque(returnCheque);
              Decimal num1 = baseCheque.SUMM + discountSum;
              int ErrCode;
              string ErrText;
              ((IPOSProcessAX33) this.PosAx).MakeReturnRRNCheque(this.ClientId, out ErrCode, out ErrText, returnCheque.ID_CHEQUE_GLOBAL.ToString(), baseCheque.ID_CHEQUE_GLOBAL.ToString(), pcxQueryLog.DATE_RESPONSE, (double) num1, (double) num1, 0.0);
              if (ErrCode == 0)
              {
                this.LogMsg(operTypeEnum, ErrText);
                GMTransactionData transactionData = new GMTransactionData(baseCheque.ID_CHEQUE_GLOBAL, guid, ((IPOSProcessAX33) this.PosAx).TransactionID);
                this.SaveTransaction(operTypeEnum, discountSum, (LpTransactionData) transactionData);
                BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
              }
              else
                this.OnException(operTypeEnum, ErrText);
              Decimal num2 = Decimal.Parse(((IPOSProcessAX33) this.PosAx).ChargedBonus, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
              Decimal num3 = Decimal.Parse(((IPOSProcessAX33) this.PosAx).DiscountedBonus, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture);
              this.SavePcxCheque(returnCheque, discountSum, pcxOperation2 == pcxOperation.RefundDebit ? "DEBIT_REFUND" : "CHARGE_REFUND", ((IPOSProcessAX33) this.PosAx).TransactionID);
              StringBuilder stringBuilder = new StringBuilder();
              stringBuilder.Append('"').Append(this.Name).Append('"').AppendLine();
              if (num2 > 0M)
              {
                stringBuilder.AppendLine("Возврат списания");
                stringBuilder.Append("Начислено: ").Append(Math.Abs(num2)).AppendLine();
              }
              if (num3 > 0M)
              {
                stringBuilder.AppendLine("Возврат начисления");
                stringBuilder.Append("Списано: ").Append(Math.Abs(num3)).AppendLine();
              }
              stringBuilder.Append("Баланс: ").Append(((IPOSProcessAX33) this.PosAx).Balance).AppendLine();
              stringBuilder.AppendLine(" ");
              stringBuilder.AppendLine(" ");
              stringBuilder.ToString();
              Decimal chargedSum = Math.Abs(num2);
              Decimal debitSum = Math.Abs(num3);
              Decimal result1 = 0M;
              if (!Decimal.TryParse(((IPOSProcessAX33) this.PosAx).Balance, out result1))
                this.LogMsg(string.Format("Не удалось привести строку {0} к decimal", (object) ((IPOSProcessAX33) this.PosAx).Balance));
              result = (ILpTransResult) new LpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, result1, string.Empty, true, true);
              return;
            }
            break;
        }
        throw new LoyaltyException((ILoyaltyProgram) this, "Невозможно выполнить возврат бонусов при оплате отличной от Наличных или Картой");
      }
    }

    private void FillCheque(CHEQUE cheque)
    {
      List<PCX_CHEQUE_ITEM> pcxChequeItemList = new List<PCX_CHEQUE_ITEM>();
      IChequeItems items = ((IPOSProcessAX33) this.PosAx).Items as IChequeItems;
      items.Clear();
      Dictionary<long, string> goodsGroups = GoldenMiddleLoyaltyProgram._bl.GetGoodsGroups(cheque.CHEQUE_ITEMS.Select<CHEQUE_ITEM, long>((Func<CHEQUE_ITEM, long>) (ci => ci.ID_GOODS)));
      foreach (CHEQUE_ITEM chequeItem1 in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        Decimal num = this.GetChequeItemSideDiscountSum(chequeItem1) / (chequeItem1.SUMM + chequeItem1.SUMM_DISCOUNT) * 100M;
        Decimal discountSum = this.GetDiscountSum(chequeItem1);
        IChequeItem chequeItem2 = items.AddItem() as IChequeItem;
        string empty;
        if (!goodsGroups.TryGetValue(chequeItem1.ID_GOODS, out empty))
        {
          empty = string.Empty;
          LoyaltyLogger.Info(string.Format("Для товара {0} не найдена группа Золотой Середины", (object) chequeItem1.GOODS_NAME));
        }
        chequeItem2.Article = empty;
        chequeItem2.ArticleName = chequeItem1.GOODS_NAME;
        chequeItem2.Price = (double) chequeItem1.PRICE;
        chequeItem2.Quantity = (double) chequeItem1.QUANTITY;
        chequeItem2.Summ = (double) (chequeItem1.SUMM + discountSum);
      }
    }

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      LoyaltyCardInfo cardInfoFromService = new LoyaltyCardInfo();
      cardInfoFromService.ClientId = this.ClientId;
      cardInfoFromService.CardNumber = this.ClientPublicId;
      int ErrCode;
      string ErrText;
      string Balance;
      ((IPOSProcessAX33) this.PosAx).GetBalance(this.ClientId, out ErrCode, out ErrText, out Balance);
      switch (ErrCode)
      {
        case 0:
          Decimal result;
          if (!Decimal.TryParse(Balance, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result))
            throw new LoyaltyException((ILoyaltyProgram) this, "Баланс карты, полученный от сервиса, не удалось преобразовать в числовое значение.");
          cardInfoFromService.CardStatusId = LoyaltyCardStatus.Active;
          cardInfoFromService.CardStatus = "Активна";
          cardInfoFromService.Balance = result;
          break;
        case 57052:
          cardInfoFromService.CardStatusId = LoyaltyCardStatus.Blocked;
          cardInfoFromService.CardStatus = "Заблокирована";
          cardInfoFromService.Balance = 0M;
          break;
        case 59001:
        case 80241:
          cardInfoFromService.CardStatusId = LoyaltyCardStatus.NotFound;
          cardInfoFromService.CardStatus = "Не найдена";
          cardInfoFromService.Balance = 0M;
          break;
        default:
          throw new LoyaltyException((ILoyaltyProgram) this, ErrText);
      }
      return cardInfoFromService;
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
      Decimal num = cheque.SUMM + this.GetDiscountSum(cheque);
      this.FillCheque(cheque);
      string ErrText;
      try
      {
        ((IPOSProcessAX33) this.PosAx).ChequeIsSoft = true;
        int ErrCode;
        ((IPOSProcessAX33) this.PosAx).MakeChequeWithBonusDiscount(this.ClientId, out ErrCode, out ErrText, Guid.NewGuid().ToString(), num.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture), num.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture), num.ToString("###0.##", (IFormatProvider) CultureInfo.InvariantCulture));
        if (ErrCode != 0)
        {
          if (ErrCode != 81400)
            goto label_5;
        }
        return Convert.ToDecimal(((IPOSProcessAX33) this.PosAx).AvailablePayment);
      }
      finally
      {
        ((IPOSProcessAX33) this.PosAx).ChequeIsSoft = false;
      }
label_5:
      throw new LoyaltyException((ILoyaltyProgram) this, ErrText);
    }

    protected override void DoRollback(out string slipCheque)
    {
      if (!this.IsTransactionProcessing)
      {
        ARMLogger.Error("Транзакция не была начата, откат невозможен!");
        slipCheque = string.Empty;
      }
      else
      {
        StringBuilder stringBuilder = new StringBuilder();
        LoyaltyTransaction transaction;
        while (this.PopLastTransaction(out transaction))
        {
          this.LogMsg(transaction.Operation, "Откат транзакции");
          GMTransactionData data = transaction.Data as GMTransactionData;
          int ErrCode;
          string ErrText;
          ((IPOSProcessAX33) this.PosAx).MakeChequeCancel(this.ClientId, out ErrCode, out ErrText, Guid.NewGuid().ToString(), data.TransactionID);
          if (ErrCode != 0)
            throw new LoyaltyRollbackException(ErrText);
          string whereExpression = string.Format("TRANSACTION_ID = '{0}'", (object) data.TransactionID);
          BaseLoyaltyProgramEx.PCXChequeItemLoader.Delete(whereExpression);
          BaseLoyaltyProgramEx.PCXChequeLoader.Delete(whereExpression);
          BusinessLogicEvents.Instance.OnRollbackChequeTransaction((object) this, new ChequeTransactionEvent(data.ChequeID, string.Empty, data.ChequeOperationType));
          stringBuilder.Append(this.GetRollbackSlipCheque(transaction)).AppendLine();
          this.LogMsg(transaction.Operation, "Откат транзакции завершён");
        }
        slipCheque = stringBuilder.ToString();
        this.Commit();
      }
    }

    protected string GetRollbackSlipCheque(LoyaltyTransaction transaction)
    {
      StringBuilder stringBuilder = new StringBuilder();
      switch (transaction.Operation)
      {
        case OperTypeEnum.Debit:
          stringBuilder.AppendLine(string.Format("Отмена списания с карты {0}", (object) this.Name));
          break;
        case OperTypeEnum.Charge:
          stringBuilder.AppendLine(string.Format("Отмена начисления на карту {0}", (object) this.Name));
          break;
        default:
          stringBuilder.AppendLine("Отмена операции");
          break;
      }
      stringBuilder.AppendLine("Номер карты:").AppendLine(this.ClientPublicId);
      stringBuilder.AppendLine("Дата/время:").AppendLine(DateTime.Now.ToString("dd.MM.yy HH:mm:ss"));
      stringBuilder.Append("Сумма операции: ").Append(transaction.OperationSum.ToString("N2")).AppendLine();
      return stringBuilder.ToString();
    }

    protected override void OnInitInternal()
    {
      if (GoldenMiddleLoyaltyProgram._posAx != null)
        return;
      GoldenMiddleLoyaltyProgram._posAx = (POSProcessAX) new POSProcessAXClass();
      ((IPOSProcessAX33) GoldenMiddleLoyaltyProgram._posAx).SetIniFile(GoldenMiddleLoyaltyProgram.Settings.IniFileName, true);
    }

    protected override void OnInitSettings()
    {
      if (GoldenMiddleLoyaltyProgram.Settings != null)
        return;
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      GoldenMiddleLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      GoldenMiddleLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (!GoldenMiddleLoyaltyProgram.IscompatibilityEnabled)
        return;
      GoldenMiddleLoyaltyProgram.ExcludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
        GoldenMiddleLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
        GoldenMiddleLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
        GoldenMiddleLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
    }

    private void OnException(OperTypeEnum operType, string errorMessage)
    {
      errorMessage = "Сообщение от Золотой Середины: " + errorMessage;
      this.LogError(operType, errorMessage);
      throw new LoyaltyException((ILoyaltyProgram) this, errorMessage);
    }

    public GoldenMiddleLoyaltyProgram(string publicId)
      : base(LoyaltyType.GoldenMiddle, publicId, publicId, "LP_GM")
    {
    }
  }
}
