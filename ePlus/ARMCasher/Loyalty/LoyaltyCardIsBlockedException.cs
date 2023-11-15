// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyCardIsBlockedException
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.Loyalty;
using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class LoyaltyCardIsBlockedException : LoyaltyException
  {
    public LoyaltyCardIsBlockedException(
      ILoyaltyProgram lp,
      string message,
      Exception innerException)
      : base(lp, message, innerException)
    {
    }

    public LoyaltyCardIsBlockedException(ILoyaltyProgram lp, Exception innerException)
      : this(lp, "Карта \"Заблокирована\". Использование для списания/начисления невозможно.", innerException)
    {
    }

    public LoyaltyCardIsBlockedException(ILoyaltyProgram lp)
      : this(lp, (Exception) null)
    {
    }
  }
}
