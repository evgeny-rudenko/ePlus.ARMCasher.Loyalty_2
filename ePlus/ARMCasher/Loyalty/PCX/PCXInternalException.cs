// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.PCXInternalException
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.Loyalty;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  public class PCXInternalException : LoyaltyException
  {
    public PCXInternalException(ILoyaltyProgram loyaltyProgram)
      : base(loyaltyProgram, "Внутренняя ошибка ПЦ. Состояние счета не было изменено, откат не требуется")
    {
    }
  }
}
