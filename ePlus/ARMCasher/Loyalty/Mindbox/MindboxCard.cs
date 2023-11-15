// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Mindbox.MindboxCard
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.Interfaces;
using ePlus.Loyalty;
using ePlus.Loyalty.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ePlus.ARMCasher.Loyalty.Mindbox
{
  public class MindboxCard : LoyaltyCard, ILoyaltyMessageList, ILoyaltyPromocodeList
  {
    public MindboxLoyaltyProgram loyaltyProgram;
    private HashSet<MindboxRecommendation> recommendations = new HashSet<MindboxRecommendation>();
    private HashSet<ILoyaltyMessage> messages = new HashSet<ILoyaltyMessage>();
    private List<IPromocode> promocodes = new List<IPromocode>();

    public string CustomerExternalId { get; set; }

    public override LoyaltyType LoyaltyType => LoyaltyType.Mindbox;

    private IEnumerable<IRecommendation> GetRecommendations()
    {
      List<string> recTo = this.recommendations.Select<MindboxRecommendation, string>((Func<MindboxRecommendation, string>) (r => r.CodeTo)).ToList<string>();
      return (IEnumerable<IRecommendation>) this.recommendations.Where<MindboxRecommendation>((Func<MindboxRecommendation, bool>) (r => !recTo.Contains(r.Code))).OrderByDescending<MindboxRecommendation, int>((Func<MindboxRecommendation, int>) (r => r.Marginality)).ThenByDescending<MindboxRecommendation, Decimal>((Func<MindboxRecommendation, Decimal>) (r => r.Price)).ThenBy<MindboxRecommendation, string>((Func<MindboxRecommendation, string>) (r => r.GoodsName)).ToList<MindboxRecommendation>();
    }

    void ILoyaltyMessageList.Add(ILoyaltyMessage message) => this.messages.Add(message);

    void ILoyaltyMessageList.Clear() => throw new NotImplementedException();

    IEnumerable<ILoyaltyMessage> ILoyaltyMessageList.GetMessages() => (IEnumerable<ILoyaltyMessage>) this.messages;

    public bool AddPromocode(string promocode)
    {
      if (this.promocodes.FirstOrDefault<IPromocode>((Func<IPromocode, bool>) (c => c.Id == promocode)) != null)
        return false;
      this.promocodes.Add((IPromocode) new DiscountPromocode()
      {
        Id = promocode
      });
      return true;
    }

    public IEnumerable<IPromocode> Promocodes => (IEnumerable<IPromocode>) this.promocodes;

    public bool ChangePromocodeStatus(string promocode, PromocodeStatus status)
    {
      IPromocode promocode1 = this.promocodes.FirstOrDefault<IPromocode>((Func<IPromocode, bool>) (c => c.Id == promocode));
      if (promocode1 == null)
        return false;
      promocode1.Status = status;
      return true;
    }
  }
}
