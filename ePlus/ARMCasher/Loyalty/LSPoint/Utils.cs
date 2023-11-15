// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.Utils
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using Microsoft.VisualBasic;

namespace ePlus.ARMCasher.Loyalty.LSPoint
{
  internal static class Utils
  {
    public static string Str2Hex(string input)
    {
      string str = "";
      for (int Start = 1; Start <= Strings.Len(input); ++Start)
        str += Conversion.Hex(Strings.Asc(Strings.Mid(input, Start, 1)));
      return str;
    }

    public static string PrepareString0D0A(ref string strIn)
    {
      string str = strIn;
      int length = str.Length;
      for (int index = 0; index < length; ++index)
      {
        if ((int) str[index] == (int) Strings.Chr(10) | (int) str[index] == (int) Strings.Chr(13))
        {
          ++index;
          if (index < length)
          {
            if ((int) str[index - 1] == (int) Strings.Chr(10) & (int) str[index] == (int) Strings.Chr(13))
            {
              str = str.Remove(index - 1, 2).Insert(index - 1, ((int) Strings.Chr(13) + (int) Strings.Chr(10)).ToString());
              length = str.Length;
            }
            else if ((int) str[index - 1] == (int) Strings.Chr(13) & (int) str[index] == (int) Strings.Chr(10))
            {
              str = str.Remove(index - 1, 2).Insert(index - 1, ((int) Strings.Chr(13) + (int) Strings.Chr(10)).ToString());
              length = str.Length;
            }
            else if ((int) str[index - 1] == (int) str[index])
            {
              str = str.Remove(index - 1, 1).Insert(index - 1, ((int) Strings.Chr(13) + (int) Strings.Chr(10)).ToString());
              length = str.Length;
            }
            else if ((int) str[index] != (int) Strings.Chr(0) & (int) str[index] != (int) Strings.Chr(13) & (int) str[index] != (int) Strings.Chr(10))
            {
              str = str.Remove(index - 1, 1).Insert(index - 1, ((int) Strings.Chr(13) + (int) Strings.Chr(10)).ToString());
              length = str.Length;
            }
            else
            {
              str = str.Remove(index - 1, 1).Insert(index - 1, ((int) Strings.Chr(13) + (int) Strings.Chr(10)).ToString());
              length = str.Length;
              --index;
            }
          }
          else
            str = str.Remove(index - 1, 1).Insert(index - 1, ((int) Strings.Chr(13) + (int) Strings.Chr(10)).ToString());
        }
      }
      return str;
    }
  }
}
