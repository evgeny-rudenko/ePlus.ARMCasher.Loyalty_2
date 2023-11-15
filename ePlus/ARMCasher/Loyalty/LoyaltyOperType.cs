// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyOperType
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.Runtime.InteropServices;

namespace ePlus.ARMCasher.Loyalty
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  internal struct LoyaltyOperType
  {
    public const string Charge = "CHARGE";
    public const string Debit = "DEBIT";
    public const string RefundDebit = "DEBIT_REFUND";
    public const string CargeRefund = "CHARGE_REFUND";
  }
}
