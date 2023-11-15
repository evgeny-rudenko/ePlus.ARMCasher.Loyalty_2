// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.DiscountMobileCard
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.Loyalty;
using System;
using System.Collections.Generic;

namespace ePlus.ARMCasher.Loyalty
{
  public class DiscountMobileCard : LoyaltyCard
  {
    private const int DiscountMobileType = 8;
    private Decimal _sumDiscount;
    private Decimal _sumScore;
    private DiscountMobileCard.CardStates _state;
    private bool _recived;
    private int _discountPercent;
    private int _couponId;
    private List<long> _coupons;
    private Decimal _bonusDiscount;
    public List<DiscountMobileCard.DiscountItem> ChequeItems;

    public override LoyaltyType LoyaltyType => LoyaltyType.DiscountMobile;

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

    public DiscountMobileCard.CardStates State
    {
      get => this._state;
      set => this._state = value;
    }

    private string CardStateString()
    {
      string str;
      switch (this.State)
      {
        case DiscountMobileCard.CardStates.Active:
          str = "АКТИВНА";
          break;
        case DiscountMobileCard.CardStates.Used:
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

    public string CouponStatusInfo => throw new NotImplementedException();

    public int CouponId
    {
      get => this._couponId;
      set => this._couponId = value;
    }

    public List<long> Coupons
    {
      get => this._coupons;
      set => this._coupons = value;
    }

    public DiscountMobileCard()
    {
      this.Coupons = new List<long>();
      this.ChequeItems = new List<DiscountMobileCard.DiscountItem>();
    }

    public Decimal BonusDiscount
    {
      set => this._bonusDiscount = value;
      get => this._bonusDiscount;
    }

    public override string ToString()
    {
      string str = "Покупатель: " + this.MEMBER_FULLNAME;
      return "Карта " + this.NUMBER + " " + this.CardStateString() + " " + (this.DiscountPercent <= 0 ? "Баланс: " + (object) this.SumScore + " " : "Скидка " + (object) this.DiscountPercent + "% ") + str;
    }

    public enum CardStates
    {
      Active,
      Used,
      Unknown,
    }

    public class DiscountItem
    {
      private long _id;
      private Decimal _quantity;
      private Decimal _price;
      private Decimal _priceDiscounted;

      public long Id
      {
        get => this._id;
        set => this._id = value;
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

      public Decimal PriceDiscounted
      {
        get => this._priceDiscounted;
        set => this._priceDiscounted = value;
      }
    }
  }
}
