// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyException
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.Loyalty;
using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class LoyaltyException : ApplicationException
  {
    public ILoyaltyProgram LoyaltyProgram { get; private set; }

    public LoyaltyException(ILoyaltyProgram loyaltyProgram) => this.LoyaltyProgram = loyaltyProgram;

    public LoyaltyException(ILoyaltyProgram loyaltyProgram, string message)
      : base(message)
    {
      this.LoyaltyProgram = loyaltyProgram;
    }

    public LoyaltyException(
      ILoyaltyProgram loyaltyProgram,
      string message,
      Exception innerException)
      : base(message, innerException)
    {
      this.LoyaltyProgram = loyaltyProgram;
    }

    public override string Message => string.Format("Ошибка программы лояльности {0} {1}:{2}", (object) this.LoyaltyProgram.LoyaltyType, (object) this.LoyaltyProgram.Name, (object) base.Message);
  }
}
