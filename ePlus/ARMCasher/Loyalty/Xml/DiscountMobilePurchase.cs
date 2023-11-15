// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Xml.DiscountMobilePurchase
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.Xml.Serialization;

namespace ePlus.ARMCasher.Loyalty.Xml
{
  public class DiscountMobilePurchase
  {
    [XmlElement("coupons_url", Namespace = "")]
    public string CouponsUrl;
    [XmlElement("date", Namespace = "")]
    public string Date;
    [XmlElement("discount", Namespace = "")]
    public Decimal Discount;
    [XmlElement("doc_id", Namespace = "")]
    public string DocId;
    [XmlElement("id", Namespace = "")]
    public int Id;
    [XmlElement("pos", Namespace = "")]
    public string Pos;
    [XmlElement("sum_bonus", Namespace = "")]
    public Decimal SumBonus;
    [XmlElement("sum_discount", Namespace = "")]
    public Decimal SumDiscount;
    [XmlElement("sum_total", Namespace = "")]
    public Decimal SumTotal;
    [XmlElement("url", Namespace = "")]
    public string Url;
    [XmlElement("items", Namespace = "")]
    public DiscountMobilePurchaseItemList Items;
    [XmlElement("coupons", Namespace = "")]
    public string Coupons;
  }
}
