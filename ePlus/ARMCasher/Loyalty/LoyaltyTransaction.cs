// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyTransaction
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessObjects;
using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class LoyaltyTransaction
  {
    public Guid Id { get; private set; }

    public OperTypeEnum Operation { get; private set; }

    public Decimal OperationSum { get; set; }

    public LpTransactionData Data { get; set; }

    public LoyaltyTransaction(OperTypeEnum operation)
    {
      this.Id = Guid.NewGuid();
      this.Operation = operation;
    }
  }
}
