// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.LSPointCard
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using System;
using System.Collections.Generic;

namespace ePlus.ARMCasher.Loyalty.LSPoint
{
  public class LSPointCard : DISCOUNT2_CARD_POLICY
  {
    private const int LSPointCardType = 10;
    private Decimal _sumDiscount;
    private Decimal _sumScore;
    private LSPointCard.CardStates _state;
    private bool _recived;
    private int _discountPercent;
    private Decimal _bonusDiscount;
    public List<LSPointCard.DiscountItem> ChequeItems;

    public int ClientTypeId => 10;

    public Decimal SumDiscount
    {
      get => this._sumDiscount;
      set => this._sumDiscount = value;
    }

    public Decimal SumScore
    {
      get => this._sumScore;
      set => this._sumScore = value;
    }

    public int DiscountPercent
    {
      get => this._discountPercent;
      set => this._discountPercent = value;
    }

    public LSPointCard.CardStates State
    {
      get => this._state;
      set => this._state = value;
    }

    private string CardStateString()
    {
      string str;
      switch (this.State)
      {
        case LSPointCard.CardStates.Active:
          str = "АКТИВНА";
          break;
        case LSPointCard.CardStates.Used:
          str = "Использована";
          break;
        default:
          str = "Неопределённый статус";
          break;
      }
      return str;
    }

    public bool Recived
    {
      get => this._recived;
      set => this._recived = value;
    }

    public Decimal BonusDiscount
    {
      set => this._bonusDiscount = value;
      get => this._bonusDiscount;
    }

    public override string ToString() => "Карта LSPoint №" + this.NUMBER;

    public enum CardStates
    {
      Active,
      Used,
      Unknown,
    }

    public class DiscountItem
    {
      private long _id;
      private long _quantity;
      private Decimal _price;
      private Decimal _priceDiscounted;

      public long Id
      {
        get => this._id;
        set => this._id = value;
      }

      public long Quantity
      {
        get => this._quantity;
        set => this._quantity = value;
      }

      public Decimal Price
      {
        get => this._price;
        set => this._price = value;
      }

      public Decimal PriceDiscounted
      {
        get => this._priceDiscounted;
        set => this._priceDiscounted = value;
      }
    }
  }
}
