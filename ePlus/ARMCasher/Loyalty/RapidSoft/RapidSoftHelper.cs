// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.RapidSoft.RapidSoftHelper
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.Security.Cryptography;
using System.Text;

namespace ePlus.ARMCasher.Loyalty.RapidSoft
{
  internal static class RapidSoftHelper
  {
    private static string HexStringFromBytes(byte[] bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in bytes)
      {
        string str = num.ToString("x2");
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }

    public static string NumberToHash(string number) => RapidSoftHelper.HexStringFromBytes(new SHA1Managed().ComputeHash(Encoding.ASCII.GetBytes(number))).ToUpper();

    internal static string OperationStatusText(OperationStatus status)
    {
      switch (status)
      {
        case OperationStatus.Success:
          return "Операция успешно выполнена";
        case OperationStatus.UnknownError:
          return "Произошла неизвестная ошибка";
        case OperationStatus.Denied:
          return "Отказ процессинга";
        case OperationStatus.NoLoyalty:
          return "Программа лояльности не найдена";
        case OperationStatus.AnotherLoyalty:
          return "Переданная карта принадлежит сторонней программе лояльности";
        default:
          return string.Empty;
      }
    }

    internal static string OperationStatusText(RollbackStatus status)
    {
      switch (status)
      {
        case RollbackStatus.Success:
          return "Операция успешно выполнена";
        case RollbackStatus.UnknownError:
          return "Произошла неизвестная ошибка";
        case RollbackStatus.Denied:
          return "Отказ процессинга";
        case RollbackStatus.Impossible:
          return "Отмена невозможна";
        default:
          return string.Empty;
      }
    }
  }
}
