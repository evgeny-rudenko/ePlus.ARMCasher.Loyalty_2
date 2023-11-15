// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.RequestBase
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
  [KnownType(typeof (ApplyDiscountRequest))]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "RequestBase", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [KnownType(typeof (GetBalanceRequest))]
  [KnownType(typeof (RefundByChequeRequest))]
  [KnownType(typeof (RefundRequest))]
  [KnownType(typeof (RollbackRequest))]
  [KnownType(typeof (FindTransactionsRequest))]
  public class RequestBase : PointRequest
  {
    private DateTime RequestDateTimeField;
    private string RequestIdField;

    [DataMember(IsRequired = true)]
    public DateTime RequestDateTime
    {
      get => this.RequestDateTimeField;
      set => this.RequestDateTimeField = value;
    }

    [DataMember]
    public string RequestId
    {
      get => this.RequestIdField;
      set => this.RequestIdField = value;
    }
  }
}
