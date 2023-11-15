// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.RefundByChequeRequest
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DebuggerStepThrough]
  [DataContract(Name = "RefundByChequeRequest", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  public class RefundByChequeRequest : RequestBase
  {
    private string CardDataField;
    private ChequeRefundInfoFull ChequeField;
    private Decimal RefundSumField;

    [DataMember]
    public string CardData
    {
      get => this.CardDataField;
      set => this.CardDataField = value;
    }

    [DataMember]
    public ChequeRefundInfoFull Cheque
    {
      get => this.ChequeField;
      set => this.ChequeField = value;
    }

    [DataMember]
    public Decimal RefundSum
    {
      get => this.RefundSumField;
      set => this.RefundSumField = value;
    }
  }
}
