// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.SberbankLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCasher.BusinessLogic.Events;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon;
using ePlus.ARMCommon.Log;
using ePlus.ARMUtils;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Discount2.Server;
using ePlus.Loyalty;
using ePlus.Loyalty.Sber;
using ePlus.MetaData.Client;
using ePlus.MetaData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using winpcxLib;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  internal sealed class SberbankLoyaltyProgram : PCXLoyaltyProgramEx
  {
    private const int _devider = 100;
    private static readonly Guid _chequeOperTypeCharge = new Guid("781CD2C7-EFAD-4411-A752-961024ED95E4");
    private static readonly Guid _chequeOperTypeDebit = new Guid("C44A8A3E-69FE-49AE-8A6C-62FB569F5D9E");
    private static readonly Guid _chequeOperTypeRefundCharge = new Guid("B75C3136-2790-47A5-95AB-698FB41423AF");
    private static readonly Guid _chequeOperTypeRefundDebit = new Guid("5955BE0A-118D-4C55-B93F-86FD1739BA7A");
    private static bool _isSettingsInit;
    private static Guid _idGlobal;
    private static string _name;
    private static readonly Dictionary<Guid, DataRowItem> _excludedPrograms = new Dictionary<Guid, DataRowItem>();

    protected override Guid ChequeOperTypeCharge => SberbankLoyaltyProgram._chequeOperTypeCharge;

    protected override Guid ChequeOperTypeDebit => SberbankLoyaltyProgram._chequeOperTypeDebit;

    protected override Guid ChequeOperTypeRefundCharge => SberbankLoyaltyProgram._chequeOperTypeRefundCharge;

    protected override Guid ChequeOperTypeRefundDebit => SberbankLoyaltyProgram._chequeOperTypeRefundDebit;

    protected override string UnitName
    {
      get => "спасибо";
      set => throw new NotImplementedException();
    }

    private static Settings Settings { get; set; }

    private static Certificate Certificate { get; set; }

    private static Params Params { get; set; }

    private static bool IscompatibilityEnabled { get; set; }

    private static bool IsSettingsInit { get; set; }

    private winpcxAuthResponseData AuthPointsAuthResponse { get; set; }

    protected DISCOUNT2_MEMBER DiscountMember { get; set; }

    protected DISCOUNT2_CARD_TYPE DiscountCardType { get; set; }

    public override string Name => SberbankLoyaltyProgram._name;

    public override Guid IdGlobal => SberbankLoyaltyProgram._idGlobal;

    protected override Decimal MinChequeSumForCharge
    {
      get => SberbankLoyaltyProgram.Params.MinChequeSumForCharge;
      set => SberbankLoyaltyProgram.Params.MinChequeSumForCharge = value;
    }

    protected override Decimal MinPayPercent
    {
      get => SberbankLoyaltyProgram.Params.MinPayPercent;
      set => SberbankLoyaltyProgram.Params.MinPayPercent = value;
    }

    protected override Decimal OfflineChargePercent => 0.5M;

    protected override bool DoIsCompatibleTo(Guid discountId) => SberbankLoyaltyProgram.IscompatibilityEnabled && !SberbankLoyaltyProgram._excludedPrograms.ContainsKey(discountId);

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
            /*
              Decimal d = cheque.SUMM + this.GetDiscountSum(cheque);
            LoyaltyCardInfo loyaltyCardInfo = this.GetLoyaltyCardInfo(false);
            Decimal num = Decimal.op_Decrement(d);
            if (this.MinPayPercent > 0M)
              num = Math.Truncate(Math.Min(d - d * (this.MinPayPercent / 100M), num));
            if (d < SberbankLoyaltyProgram.Params.MinChequeSumForCharge)
              num = 0M;
            return Math.Min(num, loyaltyCardInfo.Balance);*/
            decimal sUMM = cheque.SUMM + base.GetDiscountSum(cheque);
            LoyaltyCardInfo loyaltyCardInfo = base.GetLoyaltyCardInfo(false);
            decimal num = sUMM--;
            if (this.MinPayPercent > new decimal(0))
            {
                num = Math.Truncate(Math.Min(sUMM - (sUMM * (this.MinPayPercent / new decimal(100))), num));
            }
            if (sUMM < SberbankLoyaltyProgram.Params.MinChequeSumForCharge)
            {
                num = new decimal(0);
            }
            return Math.Min(num, loyaltyCardInfo.Balance);

        }

        protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      LoyaltyCardInfo cardInfoFromService = new LoyaltyCardInfo();
      cardInfoFromService.ClientId = this.ClientId;
      cardInfoFromService.CardNumber = this.ClientPublicId;
      cardInfoFromService.ClientIdType = PublicIdType.CardNumber;
      object authResponse = this.PcxObject.CreateAuthResponse();
      if (authResponse == null)
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateAuthResponse. Вернулся пустой объект.");
      int info = this.PcxObject.GetInfo(this.ClientId, (int) this.LoyaltyType, authResponse);
      if (info != 0)
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateAuthResponse\r\n" + ErrorMessage.GetErrorMessage(info, this.PcxObject.GetErrorMessage(info)));
      winpcxAuthResponseData authResponseData = authResponse as winpcxAuthResponseData;
      int cardInfoItemCount = authResponseData.GetCardInfoItemCount();
      for (int nIndex = 0; nIndex < cardInfoItemCount; ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = authResponseData.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "BNS":
            cardInfoFromService.Points = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
            break;
          case "AB":
            cardInfoFromService.Balance = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
            break;
          case "CS":
            switch (cardInfoItemAt.Value)
            {
              case "A":
                cardInfoFromService.CardStatus = "Активна";
                cardInfoFromService.CardStatusId = LoyaltyCardStatus.Active;
                continue;
              case "R":
                cardInfoFromService.CardStatus = "Ограничена";
                cardInfoFromService.CardStatusId = LoyaltyCardStatus.Limited;
                continue;
              default:
                cardInfoFromService.CardStatus = "Заблокирована";
                cardInfoFromService.CardStatusId = LoyaltyCardStatus.Blocked;
                continue;
            }
        }
      }
      return cardInfoFromService;
    }

    protected override void OnInitInternal()
    {
      base.OnInitInternal();
      this.SendRecvTimeout = SberbankLoyaltyProgram.Settings.SendReciveTimeout;
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(baseCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeRefundCharge);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
        return;
      Guid idChequeGlobal1 = baseCheque.ID_CHEQUE_GLOBAL;
      Guid idChequeGlobal2 = returnCheque.ID_CHEQUE_GLOBAL;
      PaymentType chequePaymentType = baseCheque.CHEQUE_PAYMENT_TYPE;
      Dictionary<Guid, CHEQUE_ITEM> dictionary1 = new Dictionary<Guid, CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem1 in (List<CHEQUE_ITEM>) returnCheque.CHEQUE_ITEMS)
      {
        CHEQUE_ITEM chequeItem2 = (CHEQUE_ITEM) chequeItem1.Clone();
        dictionary1.Add(chequeItem1.ID_CHEQUE_ITEM_GLOBAL, chequeItem2);
      }
      string str = (string) null;
      int num1;
      switch (chequePaymentType)
      {
        case PaymentType.Cash:
        case PaymentType.Card:
          num1 = 1;
          break;
        case PaymentType.Mixed:
          num1 = baseCheque.CHEQUE_PAYMENTS.All<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (p => p.SEPARATE_TYPE_ENUM == PaymentType.Card || p.SEPARATE_TYPE_ENUM == PaymentType.Cash)) ? 1 : 0;
          break;
        default:
          num1 = 0;
          break;
      }
      if (num1 == 0)
      {
        int num2 = (int) UtilsArm.ShowMessageErrorOK("Невозможен возврат бонусов Спасибо при оплате отличной от Наличных или Картой");
      }
      PCX_QUERY_LOG_BL pcxQueryLogBl = new PCX_QUERY_LOG_BL();
      PCX_QUERY_LOG pcxQueryLog = pcxQueryLogBl.Load(idChequeGlobal1, pcxOperation.Charge, (int) this.LoyaltyType);
      if (pcxQueryLog == null)
        return;
      str = (string) null;
      if (!(this.PcxObject.CreateRefundRequest() is winpcxRefundRequest refundRequest))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundRequest. Вернулся пустой объект.");
      if (!(this.PcxObject.CreateRefundResponse() is winpcxRefundResponse refundResponse))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundResponse. Вернулся пустой объект.");
      Dictionary<Guid, Decimal> chequeItems = new Dictionary<Guid, Decimal>();
      List<PCX_CHEQUE_ITEM> chequeItemList = new List<PCX_CHEQUE_ITEM>();
      Decimal summ1 = dictionary1.Values.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (item => item.SUMM));
      Decimal summ2 = 0M;
      foreach (CHEQUE_ITEM chequeItem3 in dictionary1.Values)
      {
        PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM();
        Decimal summ3 = chequeItem3.SUMM;
        foreach (DISCOUNT2_MAKE_ITEM discount2MakeItem in chequeItem3.Discount2MakeItemList)
        {
          if (discount2MakeItem.TYPE == "SBER")
          {
            summ3 += discount2MakeItem.AMOUNT;
            summ2 += discount2MakeItem.AMOUNT;
          }
        }
        pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL = chequeItem3.ID_CHEQUE_ITEM_GLOBAL;
        pcxChequeItem.QUANTITY = chequeItem3.QUANTITY;
        pcxChequeItem.PRICE = UtilsArm.Round(summ3 / chequeItem3.QUANTITY);
        pcxChequeItem.SUMM = UtilsArm.RoundDown(summ3 * pcxChequeItem.QUANTITY / chequeItem3.QUANTITY);
        chequeItemList.Add(pcxChequeItem);
        chequeItems.Add(chequeItem3.ID_CHEQUE_ITEM_GLOBAL, summ3);
        if (!(this.PcxObject.CreateChequeItem() is winpcxChequeItem chequeItem4))
          throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        chequeItem4.Amount = ((int) (summ3 * 100M)).ToString();
        chequeItem4.Product = PCXLoyaltyProgramEx.GetProductCode(dictionary1[chequeItem3.ID_CHEQUE_ITEM_GLOBAL].CODE);
        chequeItem4.Quantity = Convert.ToDouble(chequeItem3.QUANTITY);
        this.AddChequeItem(refundRequest, chequeItem4);
      }
      Decimal num3 = 0M;
      if (summ2 > 0M)
        num3 = this.AddPaymentItem("N", summ2, num3, refundRequest);
      if (chequePaymentType == PaymentType.Cash)
        num3 = this.AddPaymentItem("C", summ1, num3, refundRequest);
      if (chequePaymentType == PaymentType.Card)
        num3 = this.AddPaymentItem("I", summ1, num3, refundRequest);
      if (chequePaymentType == PaymentType.Mixed)
      {
        if (baseCheque.CHEQUE_PAYMENTS.Any<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (cp => cp.SEPARATE_TYPE_ENUM == PaymentType.Card)))
          num3 = this.AddPaymentItem("I", baseCheque.SUM_CARD, num3, refundRequest);
        if (baseCheque.CHEQUE_PAYMENTS.Any<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (cp => cp.SEPARATE_TYPE_ENUM == PaymentType.Cash)))
          num3 = this.AddPaymentItem("C", baseCheque.SUM_CASH, num3, refundRequest);
      }
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = this.CreatePCXCheque(idChequeGlobal2, returnCheque.SUM_CASH + returnCheque.SUM_CARD, num3, "CHARGE_REFUND", 0M);
      pcxCheque.TRANSACTION_ID_PARENT = pcxQueryLog.TRANSACTION_ID;
      Decimal num4 = (summ1 + summ2) * 100M;
      this.Log(OperTypeEnum.ChargeRefund, num4, baseCheque);
      refundRequest.Amount = num4.ToString();
      refundRequest.ClientID = this.ClientId;
      refundRequest.ClientIDType = (int) this.LoyaltyType;
      refundRequest.Currency = 643.ToString();
      refundRequest.OrigID = pcxQueryLog.TRANSACTION_ID;
      refundRequest.OrigPartnerID = pcxQueryLog.TRANSACTION_PARTNER_ID;
      refundRequest.OrigLocation = pcxQueryLog.TRANSACTION_LOCATION;
      refundRequest.OrigTerminal = pcxQueryLog.TRANSACTION_TERMINAL;
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(idChequeGlobal2, (summ1 + summ2) * 100M, 5);
      logQueryLog.ID_CANCELED_QUERY_GLOBAL = pcxQueryLog.ID_QUERY_GLOBAL;
      logQueryLog.CLIENT_ID = this.ClientId;
      int num5 = this.PcxObject.Refund((object) refundRequest, (object) refundResponse);
      this.Log(OperTypeEnum.ChargeRefund, num4, baseCheque, new int?(num5), refundResponse.TransactionID);
      winpcxTransaction transaction = refundResponse.Transaction as winpcxTransaction;
      Decimal scoreDeltaBalance;
      Decimal scoreBalance;
      try
      {
        logQueryLog.DATE_RESPONSE = DateTime.Now;
        switch (num5)
        {
          case 0:
            logQueryLog.STATE = 4;
            break;
          case 1:
            logQueryLog.STATE = 5;
            break;
          default:
            logQueryLog.STATE = 2;
            break;
        }
        if (transaction != null)
        {
          pcxCheque.TRANSACTION_ID = logQueryLog.TRANSACTION_ID = transaction.ID;
          pcxCheque.LOCATION = logQueryLog.TRANSACTION_LOCATION = transaction.Location;
          pcxCheque.PARTNER_ID = logQueryLog.TRANSACTION_PARTNER_ID = transaction.PartnerID;
          pcxCheque.TERMINAL = logQueryLog.TRANSACTION_TERMINAL = transaction.Terminal;
        }
        else
        {
          pcxCheque.TRANSACTION_ID = logQueryLog.TRANSACTION_ID = string.Empty;
          pcxCheque.LOCATION = logQueryLog.TRANSACTION_LOCATION = SberbankLoyaltyProgram.Settings.Location;
          pcxCheque.PARTNER_ID = logQueryLog.TRANSACTION_PARTNER_ID = SberbankLoyaltyProgram.Settings.PartnerId;
          pcxCheque.TERMINAL = logQueryLog.TRANSACTION_TERMINAL = SberbankLoyaltyProgram.Settings.Terminal;
        }
        pcxQueryLogBl.Save(logQueryLog);
        if (num5 != 0 && num5 != 1)
        {
          if (num5 == -991)
            throw new PCXInternalException((ILoyaltyProgram) this);
          if (num5 == -212)
            throw new LoyaltyException((ILoyaltyProgram) this, "Возврата баллов произведено не будет, т.к. родительская транзакция не была проведена.");
          throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода Refund\r\n" + Environment.NewLine + ErrorMessage.GetErrorMessage(num5, this.PcxObject.GetErrorMessage(num5)));
        }
        PCXTransactionData transactionData = new PCXTransactionData(baseCheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeRefundCharge, transaction);
        this.SaveTransaction(OperTypeEnum.ChargeRefund, num4, (LpTransactionData) transactionData);
        this.GetScoreDeltaBalance(refundResponse, out scoreDeltaBalance, out scoreBalance);
        pcxCheque.SCORE = Math.Abs(scoreDeltaBalance);
        pcxCheque.CARD_SCORE = scoreBalance;
      }
      catch (Exception ex)
      {
        this.LogError(OperTypeEnum.ChargeRefund, ex);
        throw;
      }
      finally
      {
        pcxChequeBl.Save(pcxCheque);
      }
      IDictionary<Guid, Decimal> dictionary2 = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, summ1 + summ2, Math.Round(Math.Abs(scoreDeltaBalance), 2));
      foreach (PCX_CHEQUE_ITEM pcxChequeItem in chequeItemList)
      {
        pcxChequeItem.SUMM_SCORE = dictionary2[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL] * 100M;
        pcxChequeItem.SUMM = dictionary2[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL];
        pcxChequeItem.OPER_TYPE = PCX_CHEQUE_ITEM.operTypeArr[3];
        if (transaction != null)
        {
          pcxChequeItem.TRANSACTION_ID = transaction.ID;
          pcxChequeItem.CLIENT_ID = transaction.ClientID;
          pcxChequeItem.CLIENT_ID_TYPE = transaction.ClientIDType;
        }
        else
        {
          pcxChequeItem.CLIENT_ID = this.ClientId;
          pcxChequeItem.CLIENT_ID_TYPE = (int) this.LoyaltyType;
        }
      }
      if (chequeItemList.Count > 0)
        new PCX_CHEQUE_ITEM_BL().Save(chequeItemList);
      BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("НОМЕР КАРТЫ: {0}", (object) PCXUtils.GetCardNumberMasked(this.ClientPublicId)).AppendLine();
      if (transaction != null)
        this.FillLocationInformation(pcxCheque, transaction.Location, transaction.Terminal, transaction.PartnerID);
      if (num5 == 1)
        stringBuilder.AppendFormat("К списанию {0} СПАСИБО", (object) PCXUtils.TruncateNonZero(Math.Abs(Utils.GetDecimal((object) (summ1 * SberbankLoyaltyProgram.Params.ScorePerRub)))));
      else
        stringBuilder.AppendFormat("Списано {0} СПАСИБО", (object) PCXUtils.TruncateNonZero(Math.Abs(scoreDeltaBalance)));
      str = stringBuilder.ToString();
      Decimal chargedSum = 0M;
      Decimal debitSum = Math.Abs(scoreDeltaBalance);
      Decimal balance = scoreBalance;
      result = (ILpTransResult) new PcxLpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, balance, this.UnitName, true);
    }

    private void FillPcxQueryLog(
      PCX_CHEQUE pcxCheque,
      PCX_QUERY_LOG logRecord,
      Guid idQueryGlobal,
      int result)
    {
      logRecord.TRANSACTION_ID = pcxCheque.TRANSACTION_ID;
      logRecord.TRANSACTION_LOCATION = pcxCheque.LOCATION;
      logRecord.TRANSACTION_PARTNER_ID = pcxCheque.PARTNER_ID;
      logRecord.TRANSACTION_TERMINAL = pcxCheque.TERMINAL;
      logRecord.ID_CANCELED_QUERY_GLOBAL = idQueryGlobal;
      logRecord.CLIENT_ID = this.ClientId;
      logRecord.STATUS = pcxCheque.STATUS;
      logRecord.DATE_RESPONSE = DateTime.Now;
      logRecord.STATE = this.GetResultState(result);
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(baseCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeRefundDebit);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
        return;
      Guid idChequeGlobal1 = baseCheque.ID_CHEQUE_GLOBAL;
      Guid idChequeGlobal2 = returnCheque.ID_CHEQUE_GLOBAL;
      string slipCheque = (string) null;
      PCX_QUERY_LOG_BL pcxQueryLogBl = new PCX_QUERY_LOG_BL();
      PCX_QUERY_LOG logDebit = pcxQueryLogBl.Load(idChequeGlobal1, pcxOperation.Debit, (int) this.LoyaltyType);
      if (logDebit == null)
        return;
      if (!(this.PcxObject.CreateRefundRequest() is winpcxRefundRequest refundRequest))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundRequest. Вернулся пустой объект.");
      if (!(this.PcxObject.CreateRefundResponse() is winpcxRefundResponse refundResponse))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundResponse. Вернулся пустой объект.");
      List<PCX_CHEQUE_ITEM> list = returnCheque.CHEQUE_ITEMS.Select<CHEQUE_ITEM, PCX_CHEQUE_ITEM>(new Func<CHEQUE_ITEM, PCX_CHEQUE_ITEM>(SberbankLoyaltyProgram.CreatePcxItem)).ToList<PCX_CHEQUE_ITEM>();
      Dictionary<Guid, Decimal> dictionary1 = list.ToDictionary<PCX_CHEQUE_ITEM, Guid, Decimal>((Func<PCX_CHEQUE_ITEM, Guid>) (item => item.ID_CHEQUE_ITEM_GLOBAL), (Func<PCX_CHEQUE_ITEM, Decimal>) (item => item.SUMM_SCORE));
      Decimal num1 = list.Sum<PCX_CHEQUE_ITEM>((Func<PCX_CHEQUE_ITEM, Decimal>) (item => item.SUMM_SCORE));
      Decimal num2 = num1;
      this.Log(OperTypeEnum.DebitRefund, num2, baseCheque);
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = this.CreatePCXCheque(idChequeGlobal2, 0M, num2, "DEBIT_REFUND", num1);
      pcxCheque.TRANSACTION_ID_PARENT = logDebit.TRANSACTION_ID;
      this.FillPcxRequest(refundRequest, num2, logDebit);
      int num3 = this.PcxObject.Refund((object) refundRequest, (object) refundResponse);
      this.Log(OperTypeEnum.DebitRefund, num2, baseCheque, new int?(num3), refundResponse.TransactionID);
      winpcxTransaction transaction = refundResponse.Transaction as winpcxTransaction;
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(idChequeGlobal2, num1 * 100M, 4);
      Decimal scoreDeltaBalance = 0M;
      Decimal num4 = 0M;
      try
      {
        if (transaction != null)
          SberbankLoyaltyProgram.FillPcxCheque(pcxCheque, transaction.ID, transaction.Location, transaction.PartnerID, transaction.Terminal);
        else
          SberbankLoyaltyProgram.FillPcxCheque(pcxCheque, string.Empty, SberbankLoyaltyProgram.Settings.Location, SberbankLoyaltyProgram.Settings.PartnerId, SberbankLoyaltyProgram.Settings.Terminal);
        this.FillPcxQueryLog(pcxCheque, logQueryLog, logQueryLog.ID_QUERY_GLOBAL, num3);
        pcxQueryLogBl.Save(logQueryLog);
        if (num3 != 0 && num3 != 1)
        {
          if (num3 == -991)
            throw new PCXInternalException((ILoyaltyProgram) this);
          if (num3 == -212)
            throw new LoyaltyException((ILoyaltyProgram) this, "Возврата баллов произведено не будет, т.к. родительская транзакция не была проведена.");
          throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода Refund\r\n" + Environment.NewLine + ErrorMessage.GetErrorMessage(num3, this.PcxObject.GetErrorMessage(num3)));
        }
        PCXTransactionData transactionData = new PCXTransactionData(baseCheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeRefundDebit, transaction);
        this.SaveTransaction(OperTypeEnum.DebitRefund, num2, (LpTransactionData) transactionData);
        pcxChequeBl.Save(pcxCheque);
        for (int nIndex = 0; nIndex < refundResponse.GetCardInfoItemCount(); ++nIndex)
        {
          winpcxCardInfoItem cardInfoItemAt = refundResponse.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
          switch (cardInfoItemAt.Name)
          {
            case "BNS":
              if (cardInfoItemAt.Type == "C")
              {
                scoreDeltaBalance = Utils.GetDecimal((object) cardInfoItemAt.Value);
                break;
              }
              if (cardInfoItemAt.Type == "S")
              {
                num4 = Utils.GetDecimal((object) cardInfoItemAt.Value);
                break;
              }
              break;
          }
        }
        IDictionary<Guid, Decimal> dictionary2 = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) dictionary1, num1, Math.Round(Math.Abs(scoreDeltaBalance), 2));
        foreach (PCX_CHEQUE_ITEM pcxChequeItem in list)
        {
          pcxChequeItem.SUMM_SCORE = dictionary2[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL];
          pcxChequeItem.SUMM = pcxChequeItem.SUMM_SCORE / 100M;
          pcxChequeItem.OPER_TYPE = PCX_CHEQUE_ITEM.operTypeArr[2];
          if (transaction != null)
          {
            pcxChequeItem.TRANSACTION_ID = transaction.ID;
            pcxChequeItem.CLIENT_ID = transaction.ClientID;
            pcxChequeItem.CLIENT_ID_TYPE = transaction.ClientIDType;
          }
        }
        pcxCheque.SCORE = Math.Abs(scoreDeltaBalance);
        pcxCheque.CARD_SCORE = num4;
      }
      catch (Exception ex)
      {
        ARMLogger.Error(ex.ToString());
        throw;
      }
      finally
      {
        pcxChequeBl.Save(pcxCheque);
        if (list.Count > 0)
          new PCX_CHEQUE_ITEM_BL().Save(list);
      }
      this.CreateRefundDebitCheque(out slipCheque, num3, pcxCheque, transaction, DateTime.Now, baseCheque.SUM_DISCOUNT, scoreDeltaBalance, num1);
      BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
      this.Log(OperTypeEnum.DebitRefund, Math.Abs(scoreDeltaBalance), baseCheque);
      Decimal chargedSum = Math.Abs(scoreDeltaBalance) / 100M;
      Decimal debitSum = 0M;
      Decimal balance = num4 / 100M;
      result = (ILpTransResult) new PcxLpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, balance, this.UnitName, true);
    }

    private static void FillPcxCheque(
      PCX_CHEQUE pcxCheque,
      string transactionID,
      string transactionLocation,
      string transactionPartnerID,
      string transactionTerminal)
    {
      pcxCheque.TRANSACTION_ID = transactionID;
      pcxCheque.LOCATION = transactionLocation;
      pcxCheque.PARTNER_ID = transactionPartnerID;
      pcxCheque.TERMINAL = transactionTerminal;
    }

    private void FillPcxRequest(
      winpcxRefundRequest request,
      Decimal requestAmount,
      PCX_QUERY_LOG logDebit)
    {
      request.AddPaymentItem((object) this.CreatePcxPayment(requestAmount));
      request.Amount = requestAmount.ToString();
      request.ClientID = this.ClientId;
      request.ClientIDType = (int) this.LoyaltyType;
      request.Currency = 643.ToString();
      request.OrigID = logDebit.TRANSACTION_ID;
      request.OrigPartnerID = logDebit.TRANSACTION_PARTNER_ID;
      request.OrigLocation = logDebit.TRANSACTION_LOCATION;
      request.OrigTerminal = logDebit.TRANSACTION_TERMINAL;
    }

    private winpcxPaymentItem CreatePcxPayment(Decimal requestAmount)
    {
      if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
      paymentItem.PayMeans = "P";
      paymentItem.Amount = requestAmount.ToString();
      return paymentItem;
    }

    private static PCX_CHEQUE_ITEM CreatePcxItem(CHEQUE_ITEM item)
    {
      PCX_CHEQUE_ITEM pcxItem = new PCX_CHEQUE_ITEM();
      Decimal num = 0M;
      foreach (DISCOUNT2_MAKE_ITEM discounT2MakeItem in item.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.TYPE == "SBER")))
        num = discounT2MakeItem.AMOUNT;
      pcxItem.ID_CHEQUE_ITEM_GLOBAL = item.ID_CHEQUE_ITEM_GLOBAL;
      pcxItem.QUANTITY = item.QUANTITY;
      pcxItem.PRICE = UtilsArm.Round(num / item.QUANTITY);
      pcxItem.SUMM = UtilsArm.RoundDown(num);
      pcxItem.SUMM_SCORE = pcxItem.SUMM * 100M;
      return pcxItem;
    }

    protected new string GetRollbackSlipCheque(LoyaltyTransaction transaction)
    {
      StringBuilder stringBuilder = new StringBuilder();
      LpTransactionData data = transaction.Data;
      switch (transaction.Operation)
      {
        case OperTypeEnum.Debit:
          stringBuilder.AppendLine("Отмена списания СПАСИБО");
          break;
        case OperTypeEnum.Charge:
          stringBuilder.AppendLine("Отмена начисления СПАСИБО");
          break;
        default:
          stringBuilder.AppendLine("Отмена операции СПАСИБО");
          break;
      }
      stringBuilder.AppendFormat("НОМЕР КАРТЫ: {0}", (object) PCXUtils.GetCardNumberMasked(this.ClientPublicId)).AppendLine();
      stringBuilder.AppendFormat("Сумма операции: {0}", (object) PCXUtils.TruncateNonZero(transaction.OperationSum));
      return stringBuilder.ToString();
    }

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      this.Log(OperTypeEnum.Charge, discountSum, cheque);
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeCharge);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
        return;
      int scoreAmount = (int) (discountSum * 100M);
      int cashAmount = (int) (cheque.SUM_CASH * 100M);
      int cardAmount = (int) (cheque.SUM_CARD * 100M);
      if (cardAmount <= 0)
        return;
      int num1 = scoreAmount + cashAmount + cardAmount;
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = this.CreatePCXCheque(cheque.ID_CHEQUE_GLOBAL, cheque.SUM_CASH + cheque.SUM_CARD, (Decimal) num1, "CHARGE", 0M);
      List<winpcxPaymentItem> paymentListForCahrge = this.GetPaymentListForCahrge((long) scoreAmount, (long) cashAmount, (long) cardAmount);
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(cheque.ID_CHEQUE_GLOBAL, (Decimal) (scoreAmount + cashAmount + cardAmount), 3);
      winpcxTransaction transaction;
      int num2 = this.AuthPoints(num1, discountSum, (IEnumerable<winpcxPaymentItem>) paymentListForCahrge, cheque, logQueryLog, OperTypeEnum.Charge, out transaction);
      this.Log(OperTypeEnum.Charge, discountSum, cheque, new int?(num2), transaction.ID);
      pcxChequeBl.Save(pcxCheque);
      BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
      try
      {
        if (transaction != null)
          this.FillLocationInformation(pcxCheque, transaction.Location, transaction.Terminal, transaction.PartnerID);
        Decimal num3 = 0M;
        Decimal balance = 0M;
        if (num2 == 1)
        {
          Decimal discountSum1 = this.GetDiscountSum(cheque);
          Decimal num4 = (cheque.SUMM + discountSum1) * (this.OfflineChargePercent / 100M);
          pcxCheque.SCORE = Math.Abs(num4);
        }
        else
        {
          for (int nIndex = 0; nIndex < this.AuthPointsAuthResponse.GetCardInfoItemCount(); ++nIndex)
          {
            winpcxCardInfoItem cardInfoItemAt = this.AuthPointsAuthResponse.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
            switch (cardInfoItemAt.Name)
            {
              case "BNS":
                if (cardInfoItemAt.Type == "C")
                {
                  num3 = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
                  break;
                }
                if (cardInfoItemAt.Type == "S")
                {
                  balance = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
                  break;
                }
                break;
            }
          }
          pcxCheque.CARD_SCORE = balance;
          pcxCheque.SCORE = Math.Abs(num3);
        }
        result = (ILpTransResult) new PcxLpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, Math.Abs(num3), 0M, balance, this.UnitName);
      }
      catch (Exception ex)
      {
        this.LogError(OperTypeEnum.Charge, ex);
        throw;
      }
      finally
      {
        pcxChequeBl.Save(pcxCheque);
      }
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      this.Log(OperTypeEnum.Debit, discountSum, cheque);
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeDebit);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists || !(discountSum > 0M))
        return;
      int num1 = (int) (discountSum * 100M);
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = this.CreatePCXCheque(cheque.ID_CHEQUE_GLOBAL, 0M, (Decimal) num1, "DEBIT", discountSum);
      cheque.PcxCheque = pcxCheque;
      if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
      paymentItem.PayMeans = "P";
      paymentItem.Amount = num1.ToString();
      List<winpcxPaymentItem> paymentItemList = new List<winpcxPaymentItem>();
      paymentItemList.Add(paymentItem);
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(cheque.ID_CHEQUE_GLOBAL, discountSum * 100M, 2);
      winpcxTransaction transaction;
      int num2 = this.AuthPoints(num1, discountSum, (IEnumerable<winpcxPaymentItem>) paymentItemList, cheque, logQueryLog, OperTypeEnum.Debit, out transaction);
      this.Log(OperTypeEnum.Debit, discountSum, cheque, new int?(num2), transaction.ID);
      pcxChequeBl.Save(pcxCheque);
      BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
      try
      {
        if (transaction == null)
          return;
        this.FillLocationInformation(pcxCheque, transaction.Location, transaction.Terminal, transaction.PartnerID);
        Decimal balance = 0M;
        Decimal num3 = 0M;
        for (int nIndex = 0; nIndex < this.AuthPointsAuthResponse.GetCardInfoItemCount(); ++nIndex)
        {
          winpcxCardInfoItem cardInfoItemAt = this.AuthPointsAuthResponse.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
          switch (cardInfoItemAt.Name)
          {
            case "BNS":
              if (cardInfoItemAt.Type == "C")
              {
                num3 = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
                break;
              }
              if (cardInfoItemAt.Type == "S")
              {
                balance = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
                break;
              }
              break;
          }
        }
        pcxCheque.CARD_SCORE = balance;
        pcxCheque.SCORE = Math.Abs(num3);
        result = (ILpTransResult) new PcxLpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, 0M, Math.Abs(num3), balance, this.UnitName);
      }
      catch (Exception ex)
      {
        ARMLogger.Error(ex.ToString());
        throw;
      }
      finally
      {
        pcxChequeBl.Save(pcxCheque);
      }
    }

    private int AuthPoints(
      int amount,
      Decimal discountSum,
      IEnumerable<winpcxPaymentItem> paymentItemList,
      CHEQUE cheque,
      PCX_QUERY_LOG logRecord,
      OperTypeEnum operType,
      out winpcxTransaction transaction)
    {
      Guid chequeOperationType = operType == OperTypeEnum.Debit ? this.ChequeOperTypeDebit : this.ChequeOperTypeCharge;
      this.AuthPointsAuthResponse = this.PcxObject.CreateAuthRequest() is winpcxAuthRequestData authRequest ? this.PcxObject.CreateAuthResponse() as winpcxAuthResponseData : throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateAuthRequest. Вернулся пустой объект.");
      if (this.AuthPointsAuthResponse == null)
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateAuthResponse. Вернулся пустой объект.");
      authRequest.Amount = amount.ToString();
      authRequest.ClientID = this.ClientId;
      authRequest.ClientIDType = (int) this.LoyaltyType;
      authRequest.Currency = 643.ToString();
      foreach (winpcxPaymentItem paymentItem in paymentItemList)
        authRequest.AddPaymentItem((object) paymentItem);
      foreach (CHEQUE_ITEM chequeItem1 in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        if (!(this.PcxObject.CreateChequeItem() is winpcxChequeItem chequeItem2))
          throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        Decimal num = chequeItem1.SUMM + (discountSum == 0M ? 0M : chequeItem1.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (discountItem =>
        {
          if (string.IsNullOrEmpty(discountItem.TYPE))
            return 0M;
          return !discountItem.TYPE.Equals("SBER") ? 0M : discountItem.AMOUNT;
        })));
        chequeItem2.Amount = Convert.ToInt32(num * 100M).ToString();
        chequeItem2.Product = PCXLoyaltyProgramEx.GetProductCode(chequeItem1.CODE);
        chequeItem2.Quantity = Convert.ToDouble(chequeItem1.QUANTITY);
        this.AddChequeItem(authRequest, chequeItem2);
      }
      BaseLoyaltyProgramEx.QueryLogBl.Save(logRecord);
      int num1 = this.PcxObject.AuthPoints((object) authRequest, (object) this.AuthPointsAuthResponse);
      transaction = this.AuthPointsAuthResponse.Transaction as winpcxTransaction;
      logRecord.DATE_RESPONSE = DateTime.Now;
      logRecord.STATE = num1 == 0 || num1 == 1 ? this.GetResultState(num1) : throw new LoyaltyException((ILoyaltyProgram) this, ErrorMessage.GetErrorMessage(num1, this.PcxObject.GetErrorMessage(num1)));
      if (transaction != null)
      {
        logRecord.TRANSACTION_ID = transaction.ID;
        logRecord.TRANSACTION_LOCATION = transaction.Location;
        logRecord.TRANSACTION_PARTNER_ID = transaction.PartnerID;
        logRecord.TRANSACTION_TERMINAL = transaction.Terminal;
      }
      else
      {
        logRecord.TRANSACTION_ID = string.Empty;
        logRecord.TRANSACTION_LOCATION = SberbankLoyaltyProgram.Settings.Location;
        logRecord.TRANSACTION_PARTNER_ID = SberbankLoyaltyProgram.Settings.PartnerId;
        logRecord.TRANSACTION_TERMINAL = SberbankLoyaltyProgram.Settings.Terminal;
      }
      BaseLoyaltyProgramEx.QueryLogBl.Save(logRecord);
      if ((num1 != 1 || operType != OperTypeEnum.Charge) && num1 != 0)
      {
        foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
        {
          foreach (DISCOUNT2_MAKE_ITEM discount2MakeItem in chequeItem.Discount2MakeItemList)
          {
            if (discount2MakeItem.TYPE == "SBER")
            {
              discount2MakeItem.AMOUNT = 0M;
              discountSum = 0M;
            }
          }
        }
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода AuthPoints\r\n" + ErrorMessage.GetErrorMessage(num1, this.PcxObject.GetErrorMessage(num1)));
      }
      PCXTransactionData transactionData = new PCXTransactionData(cheque.ID_CHEQUE_GLOBAL, chequeOperationType, transaction);
      this.SaveTransaction(operType, discountSum, (LpTransactionData) transactionData);
      Decimal pcxSumMoney = 0M;
      Decimal pcxSumScore = 0M;
      for (int nIndex = 0; nIndex < this.AuthPointsAuthResponse.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = this.AuthPointsAuthResponse.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "AB":
            if (cardInfoItemAt.Type == "C")
            {
              pcxSumMoney = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
              break;
            }
            break;
          case "BNS":
            if (cardInfoItemAt.Type == "C")
            {
              pcxSumScore = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
              break;
            }
            break;
        }
      }
      transaction = (winpcxTransaction) this.AuthPointsAuthResponse.Transaction;
      this.CreateAndSavePCXChequeItemList((IEnumerable<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS, cheque.SUMM + discountSum, pcxSumMoney, pcxSumScore, transaction.ID);
      return num1;
    }

    private void CreateAndSavePCXChequeItemList(
      IEnumerable<CHEQUE_ITEM> chequeItemList,
      Decimal totalSum,
      Decimal pcxSumMoney,
      Decimal pcxSumScore,
      string transactionId)
    {
      List<CHEQUE_ITEM> list = chequeItemList.ToList<CHEQUE_ITEM>();
      Dictionary<Guid, Decimal> chequeItems = new Dictionary<Guid, Decimal>();
      foreach (CHEQUE_ITEM chequeItem in list)
      {
        Decimal num = chequeItem.SUMM + chequeItem.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.TYPE == "SBER")).Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (dmi => dmi.AMOUNT));
        chequeItems.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, num);
      }
      if (!(Math.Abs(pcxSumMoney) > 0M) || !(Math.Abs(pcxSumMoney) < totalSum) || !(Math.Abs(pcxSumScore) > 0M))
        return;
      IDictionary<Guid, Decimal> dictionary1 = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, totalSum, Math.Abs(pcxSumMoney), true);
      IDictionary<Guid, Decimal> dictionary2 = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, totalSum, Math.Abs(pcxSumScore), true);
      List<PCX_CHEQUE_ITEM> chequeItemList1 = new List<PCX_CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem in list)
      {
        PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM()
        {
          TRANSACTION_ID = transactionId,
          CLIENT_ID = this.ClientId,
          CLIENT_ID_TYPE = (int) this.LoyaltyType
        };
        chequeItemList1.Add(pcxChequeItem);
        pcxChequeItem.SUMM_SCORE = dictionary2[chequeItem.ID_CHEQUE_ITEM_GLOBAL];
        pcxChequeItem.SUMM = dictionary1[chequeItem.ID_CHEQUE_ITEM_GLOBAL];
        pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL;
        OperTypeEnum index = pcxSumMoney >= 0M ? OperTypeEnum.Charge : OperTypeEnum.Debit;
        pcxChequeItem.OPER_TYPE = PCX_CHEQUE_ITEM.operTypeArr[(int) index];
      }
      if (chequeItemList1.Count <= 0)
        return;
      new PCX_CHEQUE_ITEM_BL().Save(chequeItemList1);
    }

    protected Decimal CalculateSumTotalPCX(
      Dictionary<Guid, CHEQUE_ITEM> returnedChequeItemList)
    {
      Decimal sumTotalPcx = 0M;
      foreach (CHEQUE_ITEM chequeItem in returnedChequeItemList.Values)
      {
        Decimal num = 0M;
        foreach (DISCOUNT2_MAKE_ITEM discount2MakeItem in chequeItem.Discount2MakeItemList)
        {
          if (discount2MakeItem.TYPE == "SBER")
            num = discount2MakeItem.AMOUNT;
        }
        sumTotalPcx += num;
      }
      return sumTotalPcx;
    }

    protected new void CreateRefundDebitCheque(
      out string slipCheque,
      int res,
      PCX_CHEQUE pcxCheque,
      winpcxTransaction transaction,
      DateTime dateRequest,
      Decimal discountSum,
      Decimal scoreDeltaBalance,
      Decimal sumTotalPCX)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (this.DiscountMember != null)
        new DISCOUNT2_MEMBER_BL(MultiServerBL.ServerConnectionString).Save(this.DiscountMember);
      stringBuilder.AppendFormat("НОМЕР КАРТЫ: {0}", (object) PCXUtils.GetCardNumberMasked(this.ClientPublicId)).AppendLine();
      if (res == 1)
        stringBuilder.AppendFormat("К начислению {0} СПАСИБО", (object) PCXUtils.TruncateNonZero(Utils.GetDecimal((object) sumTotalPCX)));
      else
        stringBuilder.AppendFormat("Начислено {0} СПАСИБО", (object) PCXUtils.TruncateNonZero(Math.Abs(scoreDeltaBalance) / 100M));
      slipCheque = stringBuilder.ToString();
    }

    protected override void OnInitSettings()
    {
      if (SberbankLoyaltyProgram.IsSettingsInit)
        return;
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      SberbankLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      SberbankLoyaltyProgram.Certificate = settingsModel.Deserialize<Certificate>(loyaltySettings.SETTINGS, "Certificate");
      SberbankLoyaltyProgram.Params = settingsModel.Deserialize<Params>(loyaltySettings.PARAMS, "Params");
      this.SendRecvTimeout = SberbankLoyaltyProgram.Settings.SendReciveTimeout == 0 ? 30 : SberbankLoyaltyProgram.Settings.SendReciveTimeout;
      SberbankLoyaltyProgram._idGlobal = loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL;
      SberbankLoyaltyProgram._name = loyaltySettings.NAME;
      this.MinPayPercent = SberbankLoyaltyProgram.Params.MinPayPercent;
      SberbankLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (SberbankLoyaltyProgram.IscompatibilityEnabled)
      {
        SberbankLoyaltyProgram._excludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
        foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
          SberbankLoyaltyProgram._excludedPrograms.Add(exclude.Guid, exclude);
        foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
          SberbankLoyaltyProgram._excludedPrograms.Add(exclude.Guid, exclude);
        foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
          SberbankLoyaltyProgram._excludedPrograms.Add(exclude.Guid, exclude);
      }
      SberbankLoyaltyProgram.IsSettingsInit = true;
    }

    protected override void OnPCXSettings()
    {
      this.PcxObject.ConnectionString = SberbankLoyaltyProgram.Settings.Url;
      this.PcxObject.ConnectTimeout = SberbankLoyaltyProgram.Settings.ConnectionTimeout;
      this.PcxObject.SendRecvTimeout = SberbankLoyaltyProgram.Settings.SendReciveTimeout;
      this.PcxObject.Location = SberbankLoyaltyProgram.Settings.Location;
      this.PcxObject.PartnerID = SberbankLoyaltyProgram.Settings.PartnerId;
      this.PcxObject.BackgndFlushPeriod = SberbankLoyaltyProgram.Settings.BkgndFlushPeriod;
      if (SberbankLoyaltyProgram.Settings.Proxy.Use)
      {
        this.PcxObject.ProxyHost = SberbankLoyaltyProgram.Settings.Proxy.Address;
        this.PcxObject.ProxyPort = SberbankLoyaltyProgram.Settings.Proxy.Port;
        this.PcxObject.ProxyUserId = SberbankLoyaltyProgram.Settings.Proxy.User;
        this.PcxObject.ProxyUserPass = SberbankLoyaltyProgram.Settings.Proxy.Password;
      }
      this.PcxObject.Terminal = SberbankLoyaltyProgram.Settings.Terminal;
      if (SberbankLoyaltyProgram.Certificate.SertInStorage)
      {
        this.PcxObject.CertSubjectName = SberbankLoyaltyProgram.Certificate.SertName;
      }
      else
      {
        this.PcxObject.CertFilePath = SberbankLoyaltyProgram.Certificate.SertFilePath;
        this.PcxObject.KeyFilePath = SberbankLoyaltyProgram.Certificate.KeyFilePath;
        this.PcxObject.KeyPassword = SberbankLoyaltyProgram.Certificate.CertPassword;
      }
      if (!LoyaltyProgManager.IsLoyalityProgramEnabled(LoyaltyType.Svyaznoy) && !AppConfigurator.EnableSberbank)
        return;
      int num = this.PcxObject.Init();
      if (num != 0)
        throw new LoyaltyException((ILoyaltyProgram) this, "Объект PCX, не создан  возможно ActiveX компонент не установлен" + Environment.NewLine + ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
    }

    public SberbankLoyaltyProgram(string PublicId, string cardNumber)
      : base(LoyaltyType.Sberbank, PublicId, cardNumber, "SBER")
    {
    }

    public bool ValidateBin(string cardNo)
    {
      this.OnInitSettings();
      return SberbankLoyaltyProgram.Params.Bins.Any<Bin>((Func<Bin, bool>) (item => item.DateDeleted == DateTime.MinValue && cardNo.StartsWith(item.Value)));
    }

    protected void GetScoreDeltaBalance(
      winpcxRefundResponse response,
      out Decimal scoreDeltaBalance,
      out Decimal scoreBalance)
    {
      scoreDeltaBalance = 0M;
      scoreBalance = 0M;
      for (int nIndex = 0; nIndex < response.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = response.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "BNS":
            if (cardInfoItemAt.Type == "C")
            {
              scoreDeltaBalance = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
              break;
            }
            if (cardInfoItemAt.Type == "S")
            {
              scoreBalance = Utils.GetDecimal((object) cardInfoItemAt.Value) / 100M;
              break;
            }
            break;
        }
      }
    }
  }
}
