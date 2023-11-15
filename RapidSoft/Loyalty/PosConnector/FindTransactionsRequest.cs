// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.FindTransactionsRequest
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DataContract(Name = "FindTransactionsRequest", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  public class FindTransactionsRequest : RequestBase
  {
    private Decimal ChequeSumField;
    private Decimal MoneySumField;
    private DateTime TransactionDateTimeField;

    [DataMember]
    public Decimal ChequeSum
    {
      get => this.ChequeSumField;
      set => this.ChequeSumField = value;
    }

    [DataMember]
    public Decimal MoneySum
    {
      get => this.MoneySumField;
      set => this.MoneySumField = value;
    }

    [DataMember]
    public DateTime TransactionDateTime
    {
      get => this.TransactionDateTimeField;
      set => this.TransactionDateTimeField = value;
    }
  }
}
