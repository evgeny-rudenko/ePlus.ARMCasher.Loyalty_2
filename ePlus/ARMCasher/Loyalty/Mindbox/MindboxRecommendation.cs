// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Mindbox.MindboxRecommendation
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessObjects;
using ePlus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ePlus.ARMCasher.Loyalty.Mindbox
{
  public class MindboxRecommendation : IRecommendation, IObjectHoder<STOCK_DETAIL>
  {
    private HashSet<STOCK_DETAIL> items = new HashSet<STOCK_DETAIL>();

    public string GoodsName => !this.items.Any<STOCK_DETAIL>() ? string.Empty : this.items.First<STOCK_DETAIL>().GOODS_NAME;

    public Decimal Quantity => !this.items.Any<STOCK_DETAIL>() ? 0M : this.items.Sum<STOCK_DETAIL>((Func<STOCK_DETAIL, Decimal>) (i => i.ACCESSIBLE));

    public Decimal Price => !this.items.Any<STOCK_DETAIL>() ? 0M : this.items.Max<STOCK_DETAIL>((Func<STOCK_DETAIL, Decimal>) (i => i.LOT_PRICE_VAT));

    public Guid GoodsGuid
    {
      get
      {
        STOCK_DETAIL stockDetail = this.GetObject();
        return stockDetail == null ? Guid.Empty : stockDetail.ID_LOT_GLOBAL;
      }
    }

    public string StoragePlace => !this.items.Any<STOCK_DETAIL>() ? string.Empty : this.items.First<STOCK_DETAIL>().STORE_PLACE_NAME;

    public int Marginality => !this.items.Any<STOCK_DETAIL>() ? 0 : this.items.First<STOCK_DETAIL>().MarginInt ?? 0;

    public string Code => !this.items.Any<STOCK_DETAIL>() ? string.Empty : this.items.First<STOCK_DETAIL>().CODE;

    public RecommendationType RecommendationType { get; private set; }

    public string RecommendationTypeString { get; private set; }

    public string CodeTo { get; private set; }

    public bool Show { get; set; }

    public STOCK_DETAIL GetObject() => this.items.FirstOrDefault<STOCK_DETAIL>();

    public MindboxRecommendation(
      RecommendationType type,
      string typeString,
      string codeTo,
      IEnumerable<STOCK_DETAIL> itemList,
      bool show)
    {
      this.RecommendationType = type;
      this.CodeTo = codeTo;
      this.RecommendationTypeString = typeString;
      this.Show = show;
      foreach (STOCK_DETAIL stockDetail in itemList)
        this.items.Add(stockDetail);
    }

    public override int GetHashCode() => (this.Code ?? string.Empty).GetHashCode();

    public override bool Equals(object obj) => this.GetHashCode() == obj.GetHashCode();
  }
}
