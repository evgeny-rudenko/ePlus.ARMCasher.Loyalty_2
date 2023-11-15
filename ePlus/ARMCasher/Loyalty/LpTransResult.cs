// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LpTransResult
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessObjects;
using ePlus.Loyalty;
using System;
using System.Collections.Generic;

namespace ePlus.ARMCasher.Loyalty
{
  public class LpTransResult : LpTransResultBase
  {
    public List<PCX_CHEQUE_ITEM> Details = new List<PCX_CHEQUE_ITEM>();

    public void AddDetail(
      Guid idChequeItemGlobal,
      Decimal summ,
      OperTypeEnum operType,
      string balanceName,
      string idPromoaction)
    {
      this.Details.Add(new PCX_CHEQUE_ITEM()
      {
        CLIENT_ID = this.ID,
        SUMM = summ,
        OPER_TYPE = operType.ToString().ToUpper(),
        BALANCE_NAME = balanceName,
        CLIENT_ID_TYPE = (int) this.LpType,
        ID_CHEQUE_ITEM_GLOBAL = idChequeItemGlobal,
        ID_PROMOACTION = idPromoaction,
        TRANSACTION_ID = this.TransactionId
      });
    }

    public void AddDetail(
      Guid idChequeItemGlobal,
      Decimal summ,
      OperTypeEnum operType,
      string balanceName)
    {
      this.AddDetail(idChequeItemGlobal, summ, operType, balanceName, (string) null);
    }

    protected override ILpTransResult Create(
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle,
      bool isRefund)
    {
      LpTransResult lpTransResult = new LpTransResult(this.IdChequeGlobal, id, chargedSum, debitSum, balance, pointsTitle, isRefund);
      lpTransResult.RoundingInCheque = this.RoundingInCheque;
      return (ILpTransResult) lpTransResult;
    }

    public LpTransResult(
      Guid idChequeGlobal,
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle,
      bool isRefund = false)
    {
      this.IdChequeGlobal = idChequeGlobal;
      this.ID = id;
      this.ChargedSum = chargedSum;
      this.DebitSum = debitSum;
      this.Balance = balance;
      this.PointsTitle = pointsTitle;
      this.IsRefund = isRefund;
      this.RoundingInCheque = false;
    }

    public LpTransResult(
      Guid idChequeGlobal,
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle,
      bool isRefund,
      bool isRegistered)
      : this(idChequeGlobal, id, chargedSum, debitSum, balance, pointsTitle, isRefund)
    {
      this.IsRegistered = isRegistered;
    }
  }
}
