// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LpTransResultBase
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.Loyalty;
using System;
using System.Text;

namespace ePlus.ARMCasher.Loyalty
{
  public abstract class LpTransResultBase : ILpTransResult
  {
    public Guid IdChequeGlobal { get; set; }

    public string PointsTitle { get; set; }

    public bool IsRefund { get; set; }

    public LoyaltyType LpType { get; set; }

    public string ID { get; set; }

    public Decimal ChargedSum { get; set; }

    public Decimal DebitSum { get; set; }

    public Decimal Balance { get; set; }

    public bool IsRegistered { get; set; }

    public string TransactionId { get; set; }

    public bool RoundingInCheque { get; set; }

    protected abstract ILpTransResult Create(
      string id,
      Decimal chargedSum,
      Decimal debitSum,
      Decimal balance,
      string pointsTitle,
      bool isRefund);

    protected virtual string Format(Decimal value)
    {
      string empty = string.Empty;
      return this.RoundingInCheque ? Math.Truncate(value).ToString("") : value.ToString();
    }

    public virtual ILpTransResult Merge(ILpTransResult result) => this.Create(this.ID, result.ChargedSum + this.ChargedSum, result.DebitSum + this.DebitSum, result.Balance, this.PointsTitle, this.IsRefund);

    public virtual string ToSlipCheque(string header = null, string footer = null)
    {
      StringBuilder stringBuilder = new StringBuilder();
      if (!string.IsNullOrEmpty(header))
        stringBuilder.AppendLine(header);
      stringBuilder.Append("ШК ").Append(this.ID).AppendLine();
      stringBuilder.AppendFormat("Списано {0}: {1}", (object) this.PointsTitle, (object) this.Format(this.DebitSum)).AppendLine();
      stringBuilder.AppendFormat("Начислено {0}: {1}", (object) this.PointsTitle, (object) this.Format(this.ChargedSum)).AppendLine();
      stringBuilder.AppendFormat("Баланс {0}: {1}", (object) this.PointsTitle, (object) this.Format(this.Balance)).AppendLine();
      if (!string.IsNullOrEmpty(footer))
        stringBuilder.AppendLine(footer);
      return stringBuilder.ToString();
    }
  }
}
