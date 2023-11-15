// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.RapidSoft.OperationStatus
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

namespace ePlus.ARMCasher.Loyalty.RapidSoft
{
  internal enum OperationStatus
  {
    Success = 0,
    UnknownError = 1,
    Denied = 2,
    NoLoyalty = 3,
    AnotherLoyalty = 99, // 0x00000063
  }
}
