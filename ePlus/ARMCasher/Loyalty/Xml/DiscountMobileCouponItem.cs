// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Xml.DiscountMobileCouponItem
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.Xml.Serialization;

namespace ePlus.ARMCasher.Loyalty.Xml
{
  public class DiscountMobileCouponItem
  {
    [XmlElement("coupon_condition")]
    public string CouponCondition;
    [XmlElement("date_bought")]
    public string DateBought;
    [XmlElement("date_expiration")]
    public string DateExpiration;
    [XmlElement("date_used")]
    public string DateUsed;
    [XmlElement("id")]
    public int Id;
    [XmlElement("number")]
    public string Number;
    [XmlElement("offer_name")]
    public string OfferName;
    [XmlElement("status")]
    public string Status;
    [XmlElement("url")]
    public string Url;
  }
}
