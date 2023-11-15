// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Xml.DiscountMobileLoyalty
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
  public class DiscountMobileLoyalty
  {
    [XmlElement("thresholds")]
    public ThresholdsList Thresholds;
    [XmlElement("amount_to_bonus")]
    public DiscountMobileLoyalty.Amount2BonusList Amount2Bonus;
    [XmlElement("bonus_to_amount")]
    public DiscountMobileLoyalty.Amount2BonusList Bonus2Amount;
    [XmlElement("max_purchase_percentage")]
    public int MaxPurchasePercentage;
    [XmlElement("min_purchase_amount")]
    public int MinPurchaseAmount;
    [XmlElement("type")]
    public string TypeAsString;
    public static readonly List<string> LoyalityProgramTypeAsString = new List<string>()
    {
      "amount",
      "count",
      "bonus",
      "slave",
      "nothing"
    };

    public DiscountMobileLoyalty.LoyalityProgramType Type
    {
      get
      {
        int index = DiscountMobileLoyalty.LoyalityProgramTypeAsString.FindIndex((Predicate<string>) (lt => lt == this.TypeAsString.ToLower()));
        return index >= 0 ? (DiscountMobileLoyalty.LoyalityProgramType) index : DiscountMobileLoyalty.LoyalityProgramType.Nothing;
      }
    }

    public enum LoyalityProgramType
    {
      Amount,
      Count,
      Bonus,
      Slave,
      Nothing,
    }

    public class DiscountMobileLoyaltyList
    {
      [XmlElement("list-item")]
      public List<Decimal> Items;
    }

    public class Amount2BonusList
    {
      [XmlElement("list-item")]
      public List<Decimal> Items;
    }

    public class Bonus2AmountList
    {
      [XmlElement("list-item")]
      public List<Decimal> Items;
    }
  }
}
