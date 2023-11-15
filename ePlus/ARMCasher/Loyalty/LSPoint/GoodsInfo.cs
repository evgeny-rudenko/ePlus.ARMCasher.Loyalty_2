// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.GoodsInfo
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;

namespace ePlus.ARMCasher.Loyalty.LSPoint
{
  internal class GoodsInfo
  {
    private string _barCode;
    private short _flags;
    private string _name;
    private Decimal _quantity;
    private Decimal _price;

    public string BarCode
    {
      get => this._barCode;
      set => this._barCode = value;
    }

    public short Flags
    {
      get => this._flags;
      set => this._flags = value;
    }

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public Decimal Quantity
    {
      get => this._quantity;
      set => this._quantity = value;
    }

    public Decimal Price
    {
      get => this._price;
      set => this._price = value;
    }
  }
}
