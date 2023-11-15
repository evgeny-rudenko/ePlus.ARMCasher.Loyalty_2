// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.PCXUtils
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  public static class PCXUtils
  {
    public static string GetCardNumberMasked(string cardNumber) => string.Format("** {0}", (object) PCXUtils.GetLast4Digit(cardNumber));

    private static string GetLast4Digit(string cardNumber)
    {
      string last4Digit = string.Empty;
      if (!string.IsNullOrWhiteSpace(cardNumber))
      {
        string str = cardNumber.Trim();
        int length = str.Length;
        if (length <= 4)
          last4Digit = str;
        else if (length > 4)
          last4Digit = str.Substring(length - 4);
      }
      return last4Digit;
    }

    public static int TruncateNonZero(Decimal value)
    {
      if (value == 0M)
        return 0;
      int int32 = Convert.ToInt32(Math.Truncate(value));
      if (int32 == 0)
        ++int32;
      return int32;
    }
  }
}
