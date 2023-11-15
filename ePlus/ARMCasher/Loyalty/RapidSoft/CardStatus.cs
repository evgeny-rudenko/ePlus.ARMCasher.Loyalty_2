// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.RapidSoft.CardStatus
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

namespace ePlus.ARMCasher.Loyalty.RapidSoft
{
  internal enum CardStatus
  {
    NotFound = 0,
    Active = 1,
    Limited = 20, // 0x00000014
    Locked = 30, // 0x0000001E
    NotActivated = 40, // 0x00000028
    Expired = 50, // 0x00000032
  }
}
