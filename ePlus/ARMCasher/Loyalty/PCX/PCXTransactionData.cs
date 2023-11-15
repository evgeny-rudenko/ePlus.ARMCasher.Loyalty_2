// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.PCXTransactionData
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using winpcxLib;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  internal class PCXTransactionData : LpTransactionData
  {
    public winpcxTransaction Transaction { get; private set; }

    public PCXTransactionData(
      Guid chequeID,
      Guid chequeOperationType,
      winpcxTransaction pcxTransaction)
      : base(chequeID, chequeOperationType)
    {
      this.Transaction = pcxTransaction;
    }
  }
}
