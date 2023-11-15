// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Xml.DiscountMobileCouponList
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ePlus.ARMCasher.Loyalty.Xml
{
  [XmlType("root", Namespace = "")]
  [Serializable]
  public class DiscountMobileCouponList
  {
    [XmlElement("results")]
    public DiscountMobileCouponList.CouponListResults CouponList;
    [XmlElement("page")]
    public int Page;
    [XmlElement("pages")]
    public int Pages;
    [XmlElement("next")]
    public string NextPage;

    public DiscountMobileCouponList()
    {
      this.CouponList = new DiscountMobileCouponList.CouponListResults();
      this.CouponList.Items = new List<DiscountMobileCouponItem>();
    }

    public class CouponListResults
    {
      [XmlElement("list-item")]
      public List<DiscountMobileCouponItem> Items;
    }
  }
}
