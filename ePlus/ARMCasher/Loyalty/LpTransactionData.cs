﻿// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LpTransactionData
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class LpTransactionData
  {
    public Guid ChequeID { get; private set; }

    public Guid ChequeOperationType { get; private set; }

    public LpTransactionData(Guid chequeID, Guid chequeOperationType)
    {
      this.ChequeID = chequeID;
      this.ChequeOperationType = chequeOperationType;
    }
  }
}
