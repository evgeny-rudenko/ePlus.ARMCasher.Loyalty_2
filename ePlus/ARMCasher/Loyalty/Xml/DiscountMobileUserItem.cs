// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserItem
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.Xml.Serialization;

namespace ePlus.ARMCasher.Loyalty.Xml
{
  public class DiscountMobileUserItem
  {
    [XmlElement("purchases")]
    public int Purchases;
    [XmlElement("first_name")]
    public string FirstName;
    [XmlElement("last_name")]
    public string LastName;
    [XmlElement("middle_name")]
    public string MiddleName;
    [XmlElement("bonus")]
    public int Bonus;
    [XmlElement("purchases_url")]
    public string PurchasesUrl;
    [XmlElement("discount")]
    public int Discounts;
    [XmlElement("amount")]
    public Decimal Amount;
    [XmlElement("url")]
    public string Url;
    [XmlElement("loyalty_url")]
    public string LoyaltyUrl;
    [XmlElement("coupons_url")]
    public string CouponsUrl;
    [XmlElement("id")]
    public int Id;
    [XmlElement("card")]
    public string Card;
  }
}
