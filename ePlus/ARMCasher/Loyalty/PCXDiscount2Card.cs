// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCXDiscount2Card
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.Loyalty;
using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class PCXDiscount2Card : LoyaltyCard
  {
    private bool recived;

    public PCXDiscount2Card(int type) => this.CLIENT_ID_TYPE = type;

    public int CLIENT_ID_TYPE { get; set; }

    public override LoyaltyType LoyaltyType => (LoyaltyType) this.CLIENT_ID_TYPE;

    public Decimal SumScore { get; set; }

    public int DiscountPercent { get; set; }

    public string State { get; set; }

    public bool Recived
    {
      get => this.recived;
      set => this.recived = value;
    }

    private string GetNameDiscountCard()
    {
      string nameDiscountCard;
      switch (this.LoyaltyType)
      {
        case LoyaltyType.Svyaznoy:
          nameDiscountCard = "Связной Клуб";
          break;
        case LoyaltyType.Sberbank:
          nameDiscountCard = "Сбербанк";
          break;
        default:
          nameDiscountCard = string.Empty;
          break;
      }
      return nameDiscountCard;
    }
  }
}
