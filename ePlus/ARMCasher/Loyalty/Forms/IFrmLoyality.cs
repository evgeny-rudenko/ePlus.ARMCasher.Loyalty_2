﻿// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.IFrmLoyality
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCommon;
using ePlus.Loyalty;
using System;
using System.Collections.Generic;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public interface IFrmLoyality : IBaseView
  {
    event Action<LoyaltySettings> LoyaltyTypeSelected;

    void Bind(Dictionary<LoyaltySettings, string> list);
  }
}
