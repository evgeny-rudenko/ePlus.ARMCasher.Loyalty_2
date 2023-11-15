// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.PCXLoyaltyProgramEx
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessLogic.Events;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon;
using ePlus.ARMCommon.Log;
using ePlus.ARMUtils;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.MetaData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using winpcxLib;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  internal abstract class PCXLoyaltyProgramEx : BaseLoyaltyProgramEx
  {
    protected const string ServiceCode = "-100";
    protected const int CurrencyRu = 643;
    private static readonly object _pcxLock = new object();
    private static readonly object _settingsLock = new object();
    private static winpcxClass _pcx;

    protected abstract Guid ChequeOperTypeCharge { get; }

    protected abstract Guid ChequeOperTypeDebit { get; }

    protected abstract Guid ChequeOperTypeRefundCharge { get; }

    protected abstract Guid ChequeOperTypeRefundDebit { get; }

    private bool IsOffline { get; set; }

    protected virtual Decimal ScorePerRub => 1M;

    protected virtual Decimal Devider { get; set; }

    protected virtual string UnitName { get; set; }

    protected virtual Decimal MinPayPercent { get; set; }

    protected virtual Decimal MinChequeSumForCharge { get; set; }

    protected abstract Decimal OfflineChargePercent { get; }

    protected winpcxClass PcxObject
    {
      get
      {
        lock (PCXLoyaltyProgramEx._pcxLock)
          return PCXLoyaltyProgramEx._pcx;
      }
    }

    protected int ClientIDType => this.LoyaltyType != LoyaltyType.Sberbank ? 4 : 6;

    protected virtual bool OnIsCompatibleTo(Guid discountId) => throw new NotImplementedException();

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeCharge);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Начисление баллов на карту лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторное начисление произведено не будет.", (object) this.Name, (object) cheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        this.Log(OperTypeEnum.Charge, discountSum, cheque);
        long scoreAmount = (long) (discountSum * 100M);
        long cashAmount = (long) (cheque.SUM_CASH * 100M);
        long cardAmount = (long) (cheque.SUM_CARD * 100M);
        long num1 = scoreAmount + cashAmount + cardAmount;
        PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
        PCX_CHEQUE pcxCheque = this.CreatePCXCheque(cheque.ID_CHEQUE_GLOBAL, cheque.SUM_CASH + cheque.SUM_CARD, (Decimal) num1, "CHARGE", 0M);
        pcxCheque.STATUS = this.GetPCXStatus();
        List<winpcxPaymentItem> paymentListForCahrge = this.GetPaymentListForCahrge(scoreAmount, cashAmount, cardAmount);
        PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(cheque.ID_CHEQUE_GLOBAL, (Decimal) (scoreAmount + cashAmount + cardAmount), 3);
        winpcxTransaction transaction;
        winpcxAuthResponseData response;
        int num2 = this.AuthPoints(num1, discountSum, (IEnumerable<winpcxPaymentItem>) paymentListForCahrge, cheque, logQueryLog, OperTypeEnum.Charge, out transaction, out response);
        this.Log(OperTypeEnum.Debit, discountSum, cheque, new int?(num2), transaction.ID);
        pcxChequeBl.Save(pcxCheque);
        BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
        try
        {
          if (transaction != null)
          {
            pcxCheque.TRANSACTION_ID = transaction.ID;
            this.FillLocationInformation(pcxCheque, transaction.Location, transaction.Terminal, transaction.PartnerID);
          }
          else
            this.FillLocationInformation(pcxCheque, this.PcxObject.Location, this.PcxObject.Terminal, this.PcxObject.PartnerID);
          Decimal num3 = this.ScoreDeltaBalance(response);
          Decimal balance = this.ScoreBalance(response);
          if (num2 == 1)
          {
            Decimal discountSum1 = this.GetDiscountSum(cheque);
            Decimal num4 = (cheque.SUMM + discountSum1) * (this.OfflineChargePercent / 100M);
            pcxCheque.SCORE = Math.Abs(num4);
          }
          else
          {
            pcxCheque.CARD_SCORE = balance;
            pcxCheque.SCORE = Math.Abs(num3);
          }
          result = (ILpTransResult) new PcxLpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, Math.Abs(num3), 0M, balance, this.UnitName);
          pcxCheque.STATUS = this.GetPCXStatus();
        }
        catch (Exception ex)
        {
          this.LogError(OperTypeEnum.Charge, ex);
          throw ex;
        }
        finally
        {
          pcxChequeBl.Save(pcxCheque);
        }
      }
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeDebit);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Списание баллов с карты лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторное списание произведено не будет.", (object) this.Name, (object) cheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        this.Log(OperTypeEnum.Debit, discountSum, cheque);
        if (!(discountSum > 0M))
          return;
        long amount = (long) (discountSum * 100M);
        PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
        PCX_CHEQUE pcxCheque = new PCX_CHEQUE()
        {
          CLIENT_ID = this.ClientId,
          CLIENT_ID_TYPE = (int) this.LoyaltyType,
          SUMM = (Decimal) amount,
          SUMM_MONEY = 0M,
          SCORE = 0M,
          SUMM_SCORE = discountSum,
          PARTNER_ID = string.Empty,
          LOCATION = string.Empty,
          TERMINAL = string.Empty,
          ID_CHEQUE_GLOBAL = cheque.ID_CHEQUE_GLOBAL,
          OPER_TYPE = "DEBIT",
          CARD_NUMBER = this.ClientPublicId
        };
        cheque.PcxCheque = pcxCheque;
        pcxCheque.STATUS = this.GetPCXStatus();
        if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
          throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        paymentItem.PayMeans = "P";
        paymentItem.Amount = amount.ToString();
        List<winpcxPaymentItem> paymentItemList = new List<winpcxPaymentItem>();
        paymentItemList.Add(paymentItem);
        PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(cheque.ID_CHEQUE_GLOBAL, discountSum * 100M, 2);
        winpcxTransaction transaction;
        winpcxAuthResponseData response;
        int num = this.AuthPoints(amount, discountSum, (IEnumerable<winpcxPaymentItem>) paymentItemList, cheque, logQueryLog, OperTypeEnum.Debit, out transaction, out response);
        this.Log(OperTypeEnum.Debit, discountSum, cheque, new int?(num), transaction.ID);
        pcxChequeBl.Save(pcxCheque);
        BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
        try
        {
          if (transaction != null)
          {
            pcxCheque.TRANSACTION_ID = transaction.ID;
            this.FillLocationInformation(pcxCheque, transaction.Location, transaction.Terminal, transaction.PartnerID);
            pcxCheque.CARD_SCORE = this.ScoreBalance(response);
            pcxCheque.SCORE = this.ScoreDeltaBalance(response);
          }
          pcxCheque.STATUS = this.GetPCXStatus();
          result = (ILpTransResult) new PcxLpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, 0M, pcxCheque.SCORE, pcxCheque.CARD_SCORE, this.UnitName);
        }
        catch (Exception ex)
        {
          this.LogError(OperTypeEnum.Debit, ex);
          throw;
        }
        finally
        {
          pcxChequeBl.Save(pcxCheque);
        }
      }
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(returnCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeRefundCharge);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Возврат начисления на карту лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторного возврата начисления произведено не будет.", (object) this.Name, (object) baseCheque.ID_CHEQUE_GLOBAL));
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
              PCX_QUERY_LOG_BL pcxQueryLogBl = new PCX_QUERY_LOG_BL();
              PCX_QUERY_LOG pcxQuerylog = pcxQueryLogBl.Load(baseCheque.ID_CHEQUE_GLOBAL, pcxOperation.Charge, (int) this.LoyaltyType);
              if (pcxQuerylog == null)
                return;
              if (!(this.PcxObject.CreateRefundRequest() is winpcxRefundRequest refundRequest))
                throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundRequest. Вернулся пустой объект.");
              if (!(this.PcxObject.CreateRefundResponse() is winpcxRefundResponse refundResponse))
                throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundResponse. Вернулся пустой объект.");
              IDictionary<Guid, Decimal> chequeItems = (IDictionary<Guid, Decimal>) new Dictionary<Guid, Decimal>();
              List<PCX_CHEQUE_ITEM> chequeItemList = new List<PCX_CHEQUE_ITEM>();
              Decimal summ1 = returnCheque.SUMM;
              Decimal summ2 = 0M;
              foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) returnCheque.CHEQUE_ITEMS)
              {
                Decimal summ3 = chequeItem.SUMM;
                Decimal num = chequeItem.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.TYPE == this.DiscountType)).Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (x => x.AMOUNT));
                Decimal itemSum = summ3 + num;
                summ2 += num;
                PCX_CHEQUE_ITEM pcxChequeItem = this.CreatePcxChequeItem(chequeItem.ID_CHEQUE_ITEM_GLOBAL, chequeItem.QUANTITY, itemSum);
                chequeItemList.Add(pcxChequeItem);
                chequeItems.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, itemSum);
                winpcxChequeItem winPcxChequeItem = this.CreateWinPcxChequeItem((int) (itemSum * 100M), chequeItem.CODE, chequeItem.QUANTITY);
                this.AddChequeItem(refundRequest, winPcxChequeItem);
              }
              Decimal num1 = 0M;
              if (summ2 > 0M)
                num1 = this.AddPaymentItem("N", summ2, num1, refundRequest);
              switch (chequePaymentType)
              {
                case PaymentType.Cash:
                  num1 = this.AddPaymentItem("C", summ1, num1, refundRequest);
                  break;
                case PaymentType.Card:
                  num1 = this.AddPaymentItem("I", summ1, num1, refundRequest);
                  break;
                case PaymentType.Mixed:
                  if (baseCheque.CHEQUE_PAYMENTS.Any<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (cp => cp.SEPARATE_TYPE_ENUM == PaymentType.Card)))
                    num1 = this.AddPaymentItem("I", baseCheque.SUM_CARD, num1, refundRequest);
                  if (baseCheque.CHEQUE_PAYMENTS.Any<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (cp => cp.SEPARATE_TYPE_ENUM == PaymentType.Cash)))
                  {
                    num1 = this.AddPaymentItem("C", baseCheque.SUM_CASH, num1, refundRequest);
                    break;
                  }
                  break;
              }
              PCX_CHEQUE pcxCheque = this.CreatePCXCheque(returnCheque.ID_CHEQUE_GLOBAL, returnCheque.SUM_CASH + returnCheque.SUM_CARD, num1, "CHARGE_REFUND", 0M);
              pcxCheque.TRANSACTION_ID_PARENT = pcxQuerylog.TRANSACTION_ID;
              pcxCheque.STATUS = this.GetPCXStatus();
              Decimal num2 = (summ1 + summ2) * 100M;
              this.Log(OperTypeEnum.ChargeRefund, num2, baseCheque);
              this.FillRequest(refundRequest, num2, pcxQuerylog);
              int num3 = this.PcxObject.Refund((object) refundRequest, (object) refundResponse);
              this.Log(OperTypeEnum.ChargeRefund, num2, baseCheque, new int?(num3), refundResponse.TransactionID);
              winpcxTransaction transaction = refundResponse.Transaction as winpcxTransaction;
              PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(returnCheque.ID_CHEQUE_GLOBAL, num2, 5);
              try
              {
                if (transaction != null)
                  PCXLoyaltyProgramEx.FillPcxCheque(pcxCheque, transaction.ID, transaction.Location, transaction.PartnerID, transaction.Terminal);
                else
                  PCXLoyaltyProgramEx.FillPcxCheque(pcxCheque, string.Empty, this.PcxObject.Location, this.PcxObject.PartnerID, this.PcxObject.Terminal);
                this.FillPcxQueryLog(pcxCheque, logQueryLog, pcxQuerylog.ID_QUERY_GLOBAL, num3);
                pcxQueryLogBl.Save(logQueryLog);
                if (num3 != 0 && num3 != 1)
                {
                  if (num3 == -991)
                    throw new PCXInternalException((ILoyaltyProgram) this);
                  if (num3 == -212)
                    throw new LoyaltyException((ILoyaltyProgram) this, string.Format("Возврата {0} произведено не будет, т.к. родительская транзакция не была проведена.", (object) this.UnitName));
                  throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода Refund\r\n" + Environment.NewLine + ErrorMessage.GetErrorMessage(num3, this.PcxObject.GetErrorMessage(num3)));
                }
                PCXTransactionData transactionData = new PCXTransactionData(baseCheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeRefundCharge, transaction);
                this.SaveTransaction(OperTypeEnum.ChargeRefund, num2, (LpTransactionData) transactionData);
                if (this.IsOffline)
                {
                  pcxCheque.SCORE = Math.Abs(baseCheque.SumWithoutDiscount * this.ScorePerRub);
                  pcxCheque.CARD_SCORE = 0M;
                }
                else
                {
                  pcxCheque.SCORE = this.ScoreDeltaBalance(refundResponse);
                  pcxCheque.CARD_SCORE = this.ScoreBalance(refundResponse);
                }
                pcxCheque.STATUS = this.GetPCXStatus();
              }
              catch (Exception ex)
              {
                this.LogError(OperTypeEnum.ChargeRefund, ex);
                throw;
              }
              finally
              {
                new PCX_CHEQUE_BL().Save(pcxCheque);
              }
              IDictionary<Guid, Decimal> dictionary = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, summ1 + summ2, Math.Round(Math.Abs(pcxCheque.SCORE), 2), true);
              foreach (PCX_CHEQUE_ITEM pcxChequeItem in chequeItemList)
              {
                if (this.IsOffline)
                {
                  pcxChequeItem.SUMM_SCORE = Math.Abs(pcxChequeItem.SUMM * this.ScorePerRub);
                  pcxChequeItem.SUMM = pcxChequeItem.SUMM_SCORE / this.Devider;
                }
                else
                {
                  pcxChequeItem.SUMM_SCORE = dictionary[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL] * this.Devider;
                  pcxChequeItem.SUMM = dictionary[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL];
                }
                pcxChequeItem.OPER_TYPE = "CHARGE_REFUND";
                if (transaction != null)
                {
                  pcxChequeItem.TRANSACTION_ID = transaction.ID;
                  pcxChequeItem.CLIENT_ID = transaction.ClientID;
                  pcxChequeItem.CLIENT_ID_TYPE = (int) this.LoyaltyType;
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
              this.CreateRefundChargeSlipCheque(baseCheque, logQueryLog, transaction, pcxCheque, num3, pcxCheque.SCORE);
              Decimal chargedSum = 0M;
              Decimal debitSum = Math.Abs(this.ScoreDeltaBalance(refundResponse));
              Decimal balance = this.ScoreBalance(refundResponse);
              result = (ILpTransResult) new PcxLpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, balance, this.UnitName, true);
              return;
            }
            break;
        }
        throw new LoyaltyException((ILoyaltyProgram) this, string.Format("Невозможно выполнить возврат {0} при оплате отличной от Наличных или Картой", (object) this.UnitName));
      }
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
            /*
              string slipCheque = (string) null;
            result = (ILpTransResult) null;
            BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(returnCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, this.ChequeOperTypeRefundDebit);
            BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
            if (e.IsOperationExists)
              return;
            PCX_QUERY_LOG_BL pcxQueryLogBl = new PCX_QUERY_LOG_BL();
            PCX_QUERY_LOG pcxQuerylog = pcxQueryLogBl.Load(baseCheque.ID_CHEQUE_GLOBAL, pcxOperation.Debit, (int) this.LoyaltyType);
            if (pcxQuerylog == null)
              return;
            Decimal num1 = 0M;
            if (!(this.PcxObject.CreateRefundRequest() is winpcxRefundRequest refundRequest))
              throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundRequest. Вернулся пустой объект.");
            if (!(this.PcxObject.CreateRefundResponse() is winpcxRefundResponse refundResponse))
              throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateRefundResponse. Вернулся пустой объект.");
            Dictionary<Guid, Decimal> chequeItems = new Dictionary<Guid, Decimal>();
            List<PCX_CHEQUE_ITEM> chequeItemList = new List<PCX_CHEQUE_ITEM>();
            foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) returnCheque.CHEQUE_ITEMS)
            {
              Decimal itemSum = chequeItem.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.TYPE == this.DiscountType)).Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (x => x.AMOUNT));
              num1 += itemSum;
              chequeItems.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, itemSum);
              PCX_CHEQUE_ITEM pcxChequeItem = this.CreatePcxChequeItem(chequeItem.ID_CHEQUE_ITEM_GLOBAL, chequeItem.QUANTITY, itemSum);
              chequeItemList.Add(pcxChequeItem);
              winpcxChequeItem winPcxChequeItem = this.CreateWinPcxChequeItem((int) (itemSum * 100M), chequeItem.CODE, chequeItem.QUANTITY);
              this.AddChequeItem(refundRequest, winPcxChequeItem);
            }
            Decimal num2 = num1 * 100M;
            this.Log(OperTypeEnum.DebitRefund, num2, baseCheque);
            PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
            PCX_CHEQUE pcxCheque = this.CreatePCXCheque(returnCheque.ID_CHEQUE_GLOBAL, 0M, num2, "DEBIT_REFUND", num1);
            pcxCheque.TRANSACTION_ID_PARENT = pcxQuerylog.TRANSACTION_ID;
            if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
              throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
            paymentItem.PayMeans = "P";
            paymentItem.Amount = num2.ToString();
            refundRequest.AddPaymentItem((object) paymentItem);
            this.FillRequest(refundRequest, num2, pcxQuerylog);
            PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog(returnCheque.ID_CHEQUE_GLOBAL, num1 * 100M, 4);
            pcxCheque.STATUS = this.GetPCXStatus();
            int num3 = this.PcxObject.Refund((object) refundRequest, (object) refundResponse);
            this.Log(OperTypeEnum.DebitRefund, num2, baseCheque, new int?(num3), refundResponse.TransactionID);
            try
            {
              if (refundResponse.Transaction is winpcxTransaction transaction)
                PCXLoyaltyProgramEx.FillPcxCheque(pcxCheque, transaction.ID, transaction.Location, transaction.PartnerID, transaction.Terminal);
              else
                PCXLoyaltyProgramEx.FillPcxCheque(pcxCheque, string.Empty, this.PcxObject.Location, this.PcxObject.PartnerID, this.PcxObject.Location);
              this.FillPcxQueryLog(pcxCheque, logQueryLog, pcxQuerylog.ID_QUERY_GLOBAL, num3);
              pcxQueryLogBl.Save(logQueryLog);
              if (num3 != 0 && num3 != 1)
              {
                if (num3 == -212)
                  throw new LoyaltyException((ILoyaltyProgram) this, string.Format("Возврата {0} произведено не будет, т.к. родительская транзакция не была проведена.", (object) this.UnitName));
                if (num3 == -991)
                  throw new PCXInternalException((ILoyaltyProgram) this);
                throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода Refund\r\n" + Environment.NewLine + ErrorMessage.GetErrorMessage(num3, this.PcxObject.GetErrorMessage(num3)));
              }
              PCXTransactionData transactionData = new PCXTransactionData(baseCheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeRefundDebit, transaction);
              this.SaveTransaction(OperTypeEnum.DebitRefund, num2, (LpTransactionData) transactionData);
              pcxChequeBl.Save(pcxCheque);
              IDictionary<Guid, Decimal> dictionary = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, num1, Math.Abs(this.ScoreDeltaBalance(refundResponse)));
              foreach (PCX_CHEQUE_ITEM pcxChequeItem in chequeItemList)
              {
                if (this.IsOffline)
                {
                  pcxChequeItem.SUMM_SCORE = Math.Abs(pcxChequeItem.SUMM * this.Devider);
                  pcxChequeItem.SUMM = pcxChequeItem.SUMM_SCORE / this.Devider;
                }
                else
                {
                  pcxChequeItem.SUMM_SCORE = dictionary[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL] * this.Devider;
                  pcxChequeItem.SUMM = dictionary[pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL];
                }
                pcxChequeItem.OPER_TYPE = PCX_CHEQUE_ITEM.operTypeArr[2];
                if (refundResponse.Transaction != null)
                {
                  pcxChequeItem.TRANSACTION_ID = ((_DwinpcxTransaction) refundResponse.Transaction).ID;
                  pcxChequeItem.CLIENT_ID = ((_DwinpcxTransaction) refundResponse.Transaction).ClientID;
                  pcxChequeItem.CLIENT_ID_TYPE = (int) this.LoyaltyType;
                }
              }
              if (this.IsOffline)
              {
                pcxCheque.SCORE = Math.Abs(num1 * this.Devider);
                pcxCheque.CARD_SCORE = 0M;
              }
              else
              {
                pcxCheque.SCORE = this.ScoreDeltaBalance(refundResponse);
                pcxCheque.CARD_SCORE = this.ScoreBalance(refundResponse);
              }
            }
            catch (Exception ex)
            {
              this.LogError(OperTypeEnum.DebitRefund, ex);
              throw;
            }
            finally
            {
              pcxChequeBl.Save(pcxCheque);
              if (chequeItemList.Count > 0)
                new PCX_CHEQUE_ITEM_BL().Save(chequeItemList);
            }
            this.CreateRefundDebitCheque(out slipCheque, num3, pcxCheque, (winpcxTransaction) refundResponse.Transaction, logQueryLog.DATE_REQUEST, baseCheque.SUM_DISCOUNT, this.ScoreDeltaBalance(refundResponse), num1);
            BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
            Decimal chargedSum = Math.Abs(this.ScoreDeltaBalance(refundResponse));
            Decimal debitSum = 0M;
            Decimal balance = this.ScoreBalance(refundResponse);
            result = (ILpTransResult) new PcxLpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, balance, this.UnitName, true);
            */
            string str = null;
            result = null;
            BeginChequeTransactionEvent beginChequeTransactionEvent = new BeginChequeTransactionEvent(returnCheque.ID_CHEQUE_GLOBAL, base.GetType().Name, this.ChequeOperTypeRefundDebit);
            BusinessLogicEvents.Instance.OnBeginChequeTransaction(this, beginChequeTransactionEvent);
            if (beginChequeTransactionEvent.IsOperationExists)
            {
                return;
            }
            PCX_QUERY_LOG_BL pCXQUERYLOGBL = new PCX_QUERY_LOG_BL();
            PCX_QUERY_LOG pCXQUERYLOG = pCXQUERYLOGBL.Load(baseCheque.ID_CHEQUE_GLOBAL, pcxOperation.Debit, (int)base.LoyaltyType);
            if (pCXQUERYLOG == null)
            {
                return;
            }
            decimal num = new decimal(0);
            winpcxRefundRequest _winpcxRefundRequest = this.PcxObject.CreateRefundRequest() as winpcxRefundRequest;
            if (_winpcxRefundRequest == null)
            {
                throw new LoyaltyException(this, "Ошибка при вызове метода CreateRefundRequest. Вернулся пустой объект.");
            }
            winpcxRefundResponse _winpcxRefundResponse = this.PcxObject.CreateRefundResponse() as winpcxRefundResponse;
            if (_winpcxRefundResponse == null)
            {
                throw new LoyaltyException(this, "Ошибка при вызове метода CreateRefundResponse. Вернулся пустой объект.");
            }
            Dictionary<Guid, decimal> guids = new Dictionary<Guid, decimal>();
            List<PCX_CHEQUE_ITEM> pCXCHEQUEITEMs = new List<PCX_CHEQUE_ITEM>();
            foreach (CHEQUE_ITEM cHEQUEITEM in returnCheque.CHEQUE_ITEMS)
            {
                decimal num1 = (
                    from dmi in cHEQUEITEM.Discount2MakeItemList
                    where dmi.TYPE == base.DiscountType
                    select dmi).Sum<DISCOUNT2_MAKE_ITEM>((DISCOUNT2_MAKE_ITEM x) => x.AMOUNT);
                num += num1;
                guids.Add(cHEQUEITEM.ID_CHEQUE_ITEM_GLOBAL, num1);
                PCX_CHEQUE_ITEM pCXCHEQUEITEM = this.CreatePcxChequeItem(cHEQUEITEM.ID_CHEQUE_ITEM_GLOBAL, cHEQUEITEM.QUANTITY, num1);
                pCXCHEQUEITEMs.Add(pCXCHEQUEITEM);
                winpcxChequeItem _winpcxChequeItem = this.CreateWinPcxChequeItem((int)(num1 * new decimal(100)), cHEQUEITEM.CODE, cHEQUEITEM.QUANTITY);
                this.AddChequeItem(_winpcxRefundRequest, _winpcxChequeItem);
            }
            decimal num2 = num * new decimal(100);
            base.Log(OperTypeEnum.DebitRefund, num2, baseCheque, null, null);
            PCX_CHEQUE_BL pCXCHEQUEBL = new PCX_CHEQUE_BL();
            PCX_CHEQUE tRANSACTIONID = base.CreatePCXCheque(returnCheque.ID_CHEQUE_GLOBAL, new decimal(0), num2, "DEBIT_REFUND", num);
            tRANSACTIONID.TRANSACTION_ID_PARENT = pCXQUERYLOG.TRANSACTION_ID;
            winpcxPaymentItem _winpcxPaymentItem = this.PcxObject.CreatePaymentItem() as winpcxPaymentItem;
            if (_winpcxPaymentItem == null)
            {
                throw new LoyaltyException(this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
            }
            _winpcxPaymentItem.PayMeans = "P";
            _winpcxPaymentItem.Amount = num2.ToString();
            _winpcxRefundRequest.AddPaymentItem(_winpcxPaymentItem);
            this.FillRequest(_winpcxRefundRequest, num2, pCXQUERYLOG);
            PCX_QUERY_LOG pCXQUERYLOG1 = base.CreateLogQueryLog(returnCheque.ID_CHEQUE_GLOBAL, num * new decimal(100), 4);
            tRANSACTIONID.STATUS = this.GetPCXStatus();
            int num3 = this.PcxObject.Refund(_winpcxRefundRequest, _winpcxRefundResponse);
            base.Log(OperTypeEnum.DebitRefund, num2, baseCheque, new int?(num3), _winpcxRefundResponse.TransactionID);
            try
            {
                try
                {
                    winpcxTransaction transaction = _winpcxRefundResponse.Transaction as winpcxTransaction;
                    if (transaction == null)
                    {
                        PCXLoyaltyProgramEx.FillPcxCheque(tRANSACTIONID, string.Empty, this.PcxObject.Location, this.PcxObject.PartnerID, this.PcxObject.Location);
                    }
                    else
                    {
                        PCXLoyaltyProgramEx.FillPcxCheque(tRANSACTIONID, transaction.ID, transaction.Location, transaction.PartnerID, transaction.Terminal);
                    }
                    this.FillPcxQueryLog(tRANSACTIONID, pCXQUERYLOG1, pCXQUERYLOG.ID_QUERY_GLOBAL, num3);
                    pCXQUERYLOGBL.Save(pCXQUERYLOG1);
                    if (num3 != 0 && num3 != 1)
                    {
                        if (num3 != -212)
                        {
                            if (num3 != -991)
                            {
                                throw new LoyaltyException(this, string.Concat("Ошибка при вызове метода Refund\r\n", Environment.NewLine, ePlus.ARMCasher.Loyalty.PCX.ErrorMessage.GetErrorMessage(num3, this.PcxObject.GetErrorMessage(num3))));
                            }
                            throw new PCXInternalException(this);
                        }
                        throw new LoyaltyException(this, string.Format("Возврата {0} произведено не будет, т.к. родительская транзакция не была проведена.", this.UnitName));
                    }
                    PCXTransactionData pCXTransactionDatum = new PCXTransactionData(baseCheque.ID_CHEQUE_GLOBAL, this.ChequeOperTypeRefundDebit, transaction);
                    base.SaveTransaction(OperTypeEnum.DebitRefund, num2, pCXTransactionDatum);
                    pCXCHEQUEBL.Save(tRANSACTIONID);
                    IDictionary<Guid, decimal> guids1 = LoyaltyProgManager.Distribute(guids, num, Math.Abs(this.ScoreDeltaBalance(_winpcxRefundResponse)), false);
                    foreach (PCX_CHEQUE_ITEM item in pCXCHEQUEITEMs)
                    {
                        if (!this.IsOffline)
                        {
                            item.SUMM_SCORE = guids1[item.ID_CHEQUE_ITEM_GLOBAL] * this.Devider;
                            item.SUMM = guids1[item.ID_CHEQUE_ITEM_GLOBAL];
                        }
                        else
                        {
                            item.SUMM_SCORE = Math.Abs(item.SUMM * this.Devider);
                            item.SUMM = item.SUMM_SCORE / this.Devider;
                        }
                        item.OPER_TYPE = PCX_CHEQUE_ITEM.operTypeArr[2];
                        if (_winpcxRefundResponse.Transaction == null)
                        {
                            continue;
                        }
                        item.TRANSACTION_ID = ((winpcxTransaction)_winpcxRefundResponse.Transaction).ID;
                        item.CLIENT_ID = ((winpcxTransaction)_winpcxRefundResponse.Transaction).ClientID;
                        item.CLIENT_ID_TYPE = (int)base.LoyaltyType;
                    }
                    if (!this.IsOffline)
                    {
                        tRANSACTIONID.SCORE = this.ScoreDeltaBalance(_winpcxRefundResponse);
                        tRANSACTIONID.CARD_SCORE = this.ScoreBalance(_winpcxRefundResponse);
                    }
                    else
                    {
                        tRANSACTIONID.SCORE = Math.Abs(num * this.Devider);
                        tRANSACTIONID.CARD_SCORE = new decimal(0);
                    }
                }
                catch (Exception exception)
                {
                    base.LogError(OperTypeEnum.DebitRefund, exception);
                    throw;
                }
            }
            finally
            {
                pCXCHEQUEBL.Save(tRANSACTIONID);
                if (pCXCHEQUEITEMs.Count > 0)
                {
                    (new PCX_CHEQUE_ITEM_BL()).Save(pCXCHEQUEITEMs);
                }
            }
            this.CreateRefundDebitCheque(out str, num3, tRANSACTIONID, (winpcxTransaction)_winpcxRefundResponse.Transaction, pCXQUERYLOG1.DATE_REQUEST, baseCheque.SUM_DISCOUNT, this.ScoreDeltaBalance(_winpcxRefundResponse), num);
            BusinessLogicEvents.Instance.OnChequeTransaction(this, beginChequeTransactionEvent);
            decimal num4 = Math.Abs(this.ScoreDeltaBalance(_winpcxRefundResponse));
            decimal num5 = new decimal(0);
            decimal num6 = this.ScoreBalance(_winpcxRefundResponse);
            result = new PcxLpTransResult(returnCheque.ID_CHEQUE_GLOBAL, base.ClientPublicId, num4, num5, num6, this.UnitName, true);

        }

        protected void CreateRefundDebitCheque(
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
      stringBuilder.AppendFormat("ШК {0}", (object) this.ClientPublicId).AppendLine();
      if (transaction == null)
        this.FillLocationInformation(pcxCheque, this.PcxObject.Location, this.PcxObject.Terminal, this.PcxObject.PartnerID);
      if (res == 1)
        stringBuilder.AppendFormat("{1} к начислению {0}", (object) PCXUtils.TruncateNonZero(Math.Abs(sumTotalPCX)), (object) this.UnitName);
      else
        stringBuilder.AppendFormat("Начислено {0}: {1}", (object) this.UnitName, (object) PCXUtils.TruncateNonZero(Math.Abs(scoreDeltaBalance)));
      slipCheque = stringBuilder.ToString();
    }

    private Decimal GetScoreValue(string scoreValue) => Math.Abs(Decimal.Parse(scoreValue) * this.ScorePerRub / 100M / this.Devider);

    private Decimal ScoreDeltaBalance(winpcxAuthResponseData response)
    {
      for (int nIndex = 0; nIndex < response.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = response.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "BNS":
            if (cardInfoItemAt.Type == "C")
              return this.GetScoreValue(cardInfoItemAt.Value);
            break;
        }
      }
      return 0M;
    }

    private Decimal ScoreDeltaBalance(winpcxRefundResponse response)
    {
      for (int nIndex = 0; nIndex < response.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = response.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "BNS":
            if (cardInfoItemAt.Type == "C")
              return this.GetScoreValue(cardInfoItemAt.Value);
            break;
        }
      }
      return 0M;
    }

    private Decimal ScoreBalance(winpcxAuthResponseData response)
    {
      for (int nIndex = 0; nIndex < response.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = response.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "BNS":
            if (cardInfoItemAt.Type == "S")
              return this.GetScoreValue(cardInfoItemAt.Value);
            break;
        }
      }
      return 0M;
    }

    private Decimal ScoreBalance(winpcxRefundResponse response)
    {
      for (int nIndex = 0; nIndex < response.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = response.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
        switch (cardInfoItemAt.Name)
        {
          case "BNS":
            if (cardInfoItemAt.Type == "S")
              return this.GetScoreValue(cardInfoItemAt.Value);
            break;
        }
      }
      return 0M;
    }

    private string CreateRefundChargeSlipCheque(
      CHEQUE baseCheque,
      PCX_QUERY_LOG logRecord,
      winpcxTransaction trans,
      PCX_CHEQUE pcxCheque,
      int res,
      Decimal scoreDeltaBalance)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendFormat("ШК {0}", (object) this.ClientPublicId).AppendLine();
      if (trans != null)
        this.FillLocationInformation(pcxCheque, trans.Location, trans.Terminal, trans.PartnerID);
      else
        this.FillLocationInformation(pcxCheque, this.PcxObject.Location, this.PcxObject.Terminal, this.PcxObject.PartnerID);
      if (res == 1)
        stringBuilder.AppendFormat("{1} к списанию {0}", (object) PCXUtils.TruncateNonZero((Decimal) Math.Abs(Utils.GetLong((object) (baseCheque.SumWithoutDiscount * this.ScorePerRub)))), (object) this.UnitName);
      else
        stringBuilder.AppendFormat("Списано {0}: {1}", (object) this.UnitName, (object) PCXUtils.TruncateNonZero(Math.Abs(scoreDeltaBalance)));
      return stringBuilder.ToString();
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
            /*Decimal num = cheque.SUMM + this.GetDiscountSum(cheque);
            LoyaltyCardInfo loyaltyCardInfo = this.GetLoyaltyCardInfo(false);
            Decimal val1 = Decimal.op_Decrement(num);
            if (this.MinPayPercent > 0M)
              val1 = Math.Truncate(Math.Min(num - num * (this.MinPayPercent / 100M), num));
            if (num < this.MinChequeSumForCharge)
              val1 = 0M;
            return Math.Min(val1, loyaltyCardInfo.Balance);
            */
            decimal sUMM = cheque.SUMM + base.GetDiscountSum(cheque);
            LoyaltyCardInfo loyaltyCardInfo = base.GetLoyaltyCardInfo(false);
            decimal num = sUMM--;
            if (this.MinPayPercent > new decimal(0))
            {
                num = Math.Truncate(Math.Min(sUMM - (sUMM * (this.MinPayPercent / new decimal(100))), sUMM));
            }
            if (sUMM < this.MinChequeSumForCharge)
            {
                num = new decimal(0);
            }
            return Math.Min(num, loyaltyCardInfo.Balance);

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
          PCXTransactionData data = (PCXTransactionData) transaction.Data;
          this.LogMsg(OperTypeEnum.Rollback, data.Transaction.ID);
          int num = this.PcxObject.Reverse((object) data.Transaction);
          this.LogMsg("Транзакция успешно отменена");
          if (num != 0 && num != 1)
            throw new LoyaltyRollbackException(ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
          string whereExpression = string.Format("TRANSACTION_ID = '{0}'", (object) data.Transaction.ID);
          BaseLoyaltyProgramEx.PCXChequeItemLoader.Delete(whereExpression, ServerType.Local);
          BaseLoyaltyProgramEx.PCXChequeLoader.Delete(whereExpression, ServerType.Local);
          new PCX_QUERY_LOG_BL().ReverseQuery(data.Transaction.ID);
          BusinessLogicEvents.Instance.OnRollbackChequeTransaction((object) this, new ChequeTransactionEvent(data.ChequeID, string.Empty, data.ChequeOperationType));
          stringBuilder.Append(this.GetRollbackSlipCheque(transaction)).AppendLine();
        }
        slipCheque = stringBuilder.ToString();
        this.Commit();
      }
    }

    private int AuthPoints(
      long amount,
      Decimal discountSum,
      IEnumerable<winpcxPaymentItem> paymentItemList,
      CHEQUE cheque,
      PCX_QUERY_LOG logRecord,
      OperTypeEnum operType,
      out winpcxTransaction transaction,
      out winpcxAuthResponseData response)
    {
      Guid chequeOperationType = operType == OperTypeEnum.Debit ? this.ChequeOperTypeDebit : this.ChequeOperTypeCharge;
      response = this.PcxObject.CreateAuthRequest() is winpcxAuthRequestData authRequest ? this.PcxObject.CreateAuthResponse() as winpcxAuthResponseData : throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateAuthRequest. Вернулся пустой объект.");
      if (response == null)
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreateAuthResponse. Вернулся пустой объект.");
      authRequest.Amount = amount.ToString();
      authRequest.ClientID = this.ClientId;
      authRequest.ClientIDType = this.ClientIDType;
      authRequest.Currency = 643.ToString();
      foreach (winpcxPaymentItem paymentItem in paymentItemList)
        authRequest.AddPaymentItem((object) paymentItem);
      foreach (CHEQUE_ITEM chequeItem1 in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        if (!(this.PcxObject.CreateChequeItem() is winpcxChequeItem chequeItem2))
          throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        Decimal num1 = chequeItem1.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (discountItem => !(discountItem.TYPE == this.DiscountType) ? 0M : discountItem.AMOUNT));
        Decimal num2 = chequeItem1.SUMM + (discountSum == 0M ? 0M : num1);
        chequeItem2.Amount = Convert.ToInt64(num2 * 100M).ToString();
        chequeItem2.Product = PCXLoyaltyProgramEx.GetProductCode(chequeItem1.CODE);
        chequeItem2.Quantity = Convert.ToDouble(chequeItem1.QUANTITY);
        this.AddChequeItem(authRequest, chequeItem2);
      }
      BaseLoyaltyProgramEx.QueryLogBl.Save(logRecord);
      int num = this.PcxObject.AuthPoints((object) authRequest, (object) response);
      logRecord.DATE_RESPONSE = DateTime.Now;
      if (num != 0 && num != 1 && !this.IsOffline)
        throw new LoyaltyException((ILoyaltyProgram) this, ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
      switch (num)
      {
        case 0:
          logRecord.STATE = 4;
          break;
        case 1:
          logRecord.STATE = 5;
          this.IsOffline = true;
          this.SetMinRecvTimeout(true);
          break;
        default:
          logRecord.STATE = 2;
          break;
      }
      transaction = response.Transaction as winpcxTransaction;
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
        logRecord.TRANSACTION_LOCATION = this.PcxObject.Location;
        logRecord.TRANSACTION_PARTNER_ID = this.PcxObject.PartnerID;
        logRecord.TRANSACTION_TERMINAL = this.PcxObject.Terminal;
      }
      logRecord.STATUS = this.GetPCXStatus();
      BaseLoyaltyProgramEx.QueryLogBl.Save(logRecord);
      if ((num != 1 || operType != OperTypeEnum.Charge) && num != 0)
      {
        foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
        {
          foreach (DISCOUNT2_MAKE_ITEM discount2MakeItem in chequeItem.Discount2MakeItemList)
          {
            if (discount2MakeItem.TYPE == "SVYAZ")
            {
              discount2MakeItem.AMOUNT = 0M;
              discountSum = 0M;
            }
          }
        }
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода AuthPoints\r\n" + ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
      }
      PCXTransactionData transactionData = new PCXTransactionData(cheque.ID_CHEQUE_GLOBAL, chequeOperationType, transaction);
      this.SaveTransaction(operType, discountSum, (LpTransactionData) transactionData);
      Decimal pcxSumMoney = 0M;
      int pcxSumScore = 0;
      for (int nIndex = 0; nIndex < response.GetCardInfoItemCount(); ++nIndex)
      {
        winpcxCardInfoItem cardInfoItemAt = response.GetCardInfoItemAt(nIndex) as winpcxCardInfoItem;
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
              pcxSumScore = (int) (Utils.GetDecimal((object) cardInfoItemAt.Value) / this.Devider);
              break;
            }
            break;
        }
      }
      this.CreateAndSavePCXChequeItemList((IEnumerable<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS, cheque.SUMM + discountSum, pcxSumMoney, pcxSumScore, transaction.ID);
      return num;
    }

    protected string GetRollbackSlipCheque(LoyaltyTransaction transaction)
    {
      StringBuilder stringBuilder = new StringBuilder();
      LpTransactionData data = transaction.Data;
      switch (transaction.Operation)
      {
        case OperTypeEnum.Debit:
          stringBuilder.AppendLine("Отмена списания");
          break;
        case OperTypeEnum.Charge:
          stringBuilder.AppendLine("Отмена начисления");
          break;
        default:
          stringBuilder.AppendLine("Отмена");
          break;
      }
      stringBuilder.AppendFormat("ШК {0}", (object) this.ClientPublicId).AppendLine();
      stringBuilder.AppendFormat("Сумма операции: {0}", (object) PCXUtils.TruncateNonZero(transaction.OperationSum));
      return stringBuilder.ToString();
    }

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      LoyaltyCardInfo cardInfoFromService = new LoyaltyCardInfo();
      cardInfoFromService.ClientId = this.ClientId;
      object authResponse = this.PcxObject.CreateAuthResponse();
      if (authResponse == null)
        throw new Exception("Ошибка при вызове метода CreateAuthResponse. Вернулся пустой объект.");
      int info = this.PcxObject.GetInfo(this.ClientId, this.ClientIDType, authResponse);
      switch (info)
      {
        case -162:
          cardInfoFromService.CardNumber = this.ClientPublicId;
          cardInfoFromService.CardStatus = "Заблокирована";
          cardInfoFromService.CardStatusId = LoyaltyCardStatus.Blocked;
          break;
        case 0:
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
              case "EAN13":
                cardInfoFromService.CardNumber = cardInfoItemAt.Value;
                break;
              case "ID_DATA":
                if (string.IsNullOrEmpty(cardInfoFromService.CardNumber) && cardInfoItemAt.Type == "S")
                {
                  cardInfoFromService.CardNumber = cardInfoItemAt.Value;
                  break;
                }
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
          break;
        default:
          throw new Exception("Ошибка при вызове метода CreateAuthResponse\r\n" + ErrorMessage.GetErrorMessage(info, this.PcxObject.GetErrorMessage(info)));
      }
      return cardInfoFromService;
    }

    private string GetPCXStatus() => !this.IsOffline ? pcxOperationStatus.Online.ToString() : pcxOperationStatus.Offline.ToString();

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
        SUMM = UtilsArm.RoundDown(itemSum),
        STATUS = this.GetPCXStatus()
      };
    }

    private winpcxChequeItem CreateWinPcxChequeItem(int amount, string product, Decimal quantity)
    {
      if (!(this.PcxObject.CreateChequeItem() is winpcxChequeItem chequeItem))
        throw new LoyaltyException((ILoyaltyProgram) this, "Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
      chequeItem.Amount = amount.ToString();
      chequeItem.Product = PCXLoyaltyProgramEx.GetProductCode(product);
      chequeItem.Quantity = Convert.ToDouble(quantity);
      return chequeItem;
    }

    protected static string GetProductCode(string productCode) => !string.IsNullOrEmpty(productCode) ? productCode : "-100";

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

    private void FillRequest(
      winpcxRefundRequest request,
      Decimal discountSum,
      PCX_QUERY_LOG pcxQuerylog)
    {
      request.Amount = discountSum.ToString();
      request.ClientID = this.ClientId;
      request.ClientIDType = this.ClientIDType;
      request.Currency = 643.ToString();
      request.OrigID = pcxQuerylog.TRANSACTION_ID;
      request.OrigPartnerID = pcxQuerylog.TRANSACTION_PARTNER_ID;
      request.OrigLocation = pcxQuerylog.TRANSACTION_LOCATION;
      request.OrigTerminal = pcxQuerylog.TRANSACTION_TERMINAL;
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

    protected int GetResultState(int result)
    {
      switch (result)
      {
        case 0:
          return 4;
        case 1:
          return 5;
        default:
          return 2;
      }
    }

    protected void FillLocationInformation(
      PCX_CHEQUE pcxCheque,
      string location,
      string terminal,
      string partnerId)
    {
      pcxCheque.LOCATION = location;
      pcxCheque.TERMINAL = terminal;
      pcxCheque.PARTNER_ID = partnerId;
    }

    protected virtual void OnPCXSettings() => throw new NotImplementedException();

    protected override void OnInitInternal()
    {
      if (PCXLoyaltyProgramEx._pcx != null)
        return;
      try
      {
        PCXLoyaltyProgramEx._pcx = new winpcxClass();
        this.OnPCXSettings();
        int num = PCXLoyaltyProgramEx._pcx.EnableBkgndFlush(1);
        if (num != 0)
          throw new NonCriticalInitializationException("Невозможно задействовать механизм выполнения сеансов связи в автоматическом режиме" + Environment.NewLine + ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
      }
      catch (NonCriticalInitializationException ex)
      {
        ARMLogger.Error(ex.ToString());
      }
      catch (Exception ex)
      {
        throw new Exception("Ошибка инициализации ПЦ." + Environment.NewLine + "Работа с программами лояльности невозможна.", ex);
      }
    }

    private void CreateAndSavePCXChequeItemList(
      IEnumerable<CHEQUE_ITEM> chequeItemList,
      Decimal totalSum,
      Decimal pcxSumMoney,
      int pcxSumScore,
      string transactionId)
    {
      List<CHEQUE_ITEM> list = chequeItemList.ToList<CHEQUE_ITEM>();
      Dictionary<Guid, Decimal> chequeItems = new Dictionary<Guid, Decimal>();
      foreach (CHEQUE_ITEM chequeItem in list)
      {
        Decimal num = chequeItem.SUMM + chequeItem.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.TYPE == this.DiscountType)).Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (dmi => dmi.AMOUNT));
        chequeItems.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, num);
      }
      IDictionary<Guid, Decimal> dictionary1 = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, totalSum, Math.Abs(pcxSumMoney), true);
      IDictionary<Guid, Decimal> dictionary2 = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, totalSum, (Decimal) Math.Abs(pcxSumScore), true);
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
        if (this.IsOffline)
        {
          pcxChequeItem.SUMM_SCORE = Math.Abs(chequeItem.SUMM * this.ScorePerRub);
          pcxChequeItem.SUMM = pcxChequeItem.SUMM_SCORE * (this.OfflineChargePercent / 100M);
          pcxChequeItem.STATUS = pcxOperationStatus.Offline.ToString();
        }
        else
        {
          pcxChequeItem.SUMM_SCORE = dictionary2[chequeItem.ID_CHEQUE_ITEM_GLOBAL];
          pcxChequeItem.SUMM = dictionary1[chequeItem.ID_CHEQUE_ITEM_GLOBAL];
          pcxChequeItem.STATUS = pcxOperationStatus.Online.ToString();
        }
        pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL;
        OperTypeEnum index = pcxSumMoney >= 0M ? OperTypeEnum.Charge : OperTypeEnum.Debit;
        pcxChequeItem.OPER_TYPE = PCX_CHEQUE_ITEM.operTypeArr[(int) index];
      }
      if (chequeItemList1.Count <= 0)
        return;
      new PCX_CHEQUE_ITEM_BL().Save(chequeItemList1);
    }

    private void SetMinRecvTimeout(bool flag)
    {
      if (flag)
      {
        this.PcxObject.SendRecvTimeout = 5;
        this.PcxObject.ConnectTimeout = 5;
      }
      else
      {
        this.PcxObject.SendRecvTimeout = this.PcxObject.SendRecvTimeout;
        this.PcxObject.ConnectTimeout = this.PcxObject.ConnectTimeout;
      }
    }

    protected List<winpcxPaymentItem> GetPaymentListForCahrge(
      long scoreAmount,
      long cashAmount,
      long cardAmount)
    {
      List<winpcxPaymentItem> paymentListForCahrge = new List<winpcxPaymentItem>();
      if (scoreAmount > 0L)
      {
        if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
          throw new Exception("Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        paymentItem.PayMeans = "N";
        paymentItem.Amount = scoreAmount.ToString();
        paymentListForCahrge.Add(paymentItem);
      }
      if (cashAmount > 0L)
      {
        if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
          throw new Exception("Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        paymentItem.PayMeans = "C";
        paymentItem.Amount = cashAmount.ToString();
        paymentListForCahrge.Add(paymentItem);
      }
      if (cardAmount > 0L)
      {
        if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
          throw new Exception("Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
        paymentItem.PayMeans = "I";
        paymentItem.Amount = cardAmount.ToString();
        paymentListForCahrge.Add(paymentItem);
      }
      return paymentListForCahrge;
    }

    protected void AddChequeItem(winpcxRefundRequest request, winpcxChequeItem item)
    {
      if (string.IsNullOrEmpty(item.Product))
        item.Product = "0";
      request.AddChequeItem((object) item);
    }

    protected void AddChequeItem(winpcxAuthRequestData request, winpcxChequeItem item)
    {
      if (string.IsNullOrEmpty(item.Product))
        item.Product = "0";
      request.AddChequeItem((object) item);
    }

    protected Decimal AddPaymentItem(
      string payMeans,
      Decimal summ,
      Decimal processingSendSum,
      winpcxRefundRequest request)
    {
      if (!(this.PcxObject.CreatePaymentItem() is winpcxPaymentItem paymentItem))
        throw new Exception("Ошибка при вызове метода CreatePaymentItem. Вернулся пустой объект.");
      paymentItem.PayMeans = payMeans;
      Decimal num = summ * 100M;
      processingSendSum += num;
      paymentItem.Amount = num.ToString();
      request.AddPaymentItem((object) paymentItem);
      return processingSendSum;
    }

    public PCXLoyaltyProgramEx(
      LoyaltyType loyaltyType,
      string clientId,
      string clientPublicId,
      string discountType)
      : base(loyaltyType, clientId, clientPublicId, discountType)
    {
    }
  }
}
