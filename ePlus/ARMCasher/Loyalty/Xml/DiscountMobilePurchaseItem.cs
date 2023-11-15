// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Xml.DiscountMobilePurchaseItem
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.Xml.Serialization;

namespace ePlus.ARMCasher.Loyalty.Xml
{
  public class DiscountMobilePurchaseItem
  {
    [XmlElement("group_code")]
    public string GroupCode { get; set; }

    [XmlElement("sum_total")]
    public string SumTotal { get; set; }

    [XmlElement("item_code")]
    public string ItemCode { get; set; }

    [XmlElement("sum_with_discount")]
    public string SumWithDiscount { get; set; }

    [XmlElement("item_gtin")]
    public string ItemGtin { get; set; }

    [XmlElement("quantity")]
    public string Quantity { get; set; }
  }
}
