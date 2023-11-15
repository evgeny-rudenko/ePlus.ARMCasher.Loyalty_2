// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyProgramDebitArgs
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.Loyalty;
using System;

namespace ePlus.ARMCasher.Loyalty
{
  public class LoyaltyProgramDebitArgs
  {
    public Func<LoyaltyType, ILoyaltyProgram> DelegateGetContainsLoyaltyProgram { get; set; }

    public LoyaltyType LoyaltyType { get; set; }

    public Guid LoyaltyInstance { get; set; }

    public CardReader DelegateCardReader { get; set; }

    public Func<ILoyaltyProgram, LoyaltyCard> DelegateCreateLoyaltyCard { get; set; }

    public Func<LoyaltyCard, ILoyaltyProgram, bool> DelegateAddLoyaltyCard { get; set; }

    public Func<LoyaltyType, LoyaltyCard> DelegateGetContainsLoyaltyCard { get; set; }

    public bool UseMaxAllowScoresOnCancel { get; set; }

    public bool ResetDiscount { get; set; }

    public Func<ILoyaltyProgram, bool> DelegateRemoveLoyaltyProgram { get; set; }

    public Func<Decimal> DelegateChequeSum { get; set; }

    public CHEQUE Cheque { get; set; }

    public string ClientId { get; set; }

    public string Promocode { get; set; }
  }
}
