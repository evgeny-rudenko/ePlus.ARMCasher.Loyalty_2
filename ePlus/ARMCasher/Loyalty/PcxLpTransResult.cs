// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PcxLpTransResult
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.Loyalty.PCX;
using ePlus.Loyalty;
using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class PcxLpTransResult : LpTransResultBase
  {
    public PcxLpTransResult() => this.IsRegistered = true;

    public PcxLpTransResult(
      Guid idChequeGlobal,
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle,
      bool isRefund)
      : this()
    {
      this.IdChequeGlobal = idChequeGlobal;
      this.ID = id;
      this.ChargedSum = chargedSum;
      this.DebitSum = debitSum;
      this.Balance = balance;
      this.PointsTitle = pointsTitle;
      this.IsRefund = isRefund;
    }

    public PcxLpTransResult(
      Guid idChequeGlobal,
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle)
      : this(idChequeGlobal, id, chargedSum, debitSum, balance, pointsTitle, false)
    {
    }

    protected override ILpTransResult Create(
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle,
      bool isRefund)
    {
      PcxLpTransResult pcxLpTransResult = new PcxLpTransResult();
      pcxLpTransResult.IdChequeGlobal = this.IdChequeGlobal;
      pcxLpTransResult.ID = id;
      pcxLpTransResult.ChargedSum = chargedSum;
      pcxLpTransResult.DebitSum = debitSum;
      pcxLpTransResult.Balance = balance;
      pcxLpTransResult.PointsTitle = pointsTitle;
      pcxLpTransResult.IsRefund = isRefund;
      return (ILpTransResult) pcxLpTransResult;
    }

    protected override string Format(Decimal value) => PCXUtils.TruncateNonZero(value).ToString();
  }
}
