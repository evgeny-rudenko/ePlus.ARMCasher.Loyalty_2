﻿// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.ChequeRefundInfoFull
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DebuggerStepThrough]
  [DataContract(Name = "ChequeRefundInfoFull", Namespace = "RapidSoft.Loyalty.PosConnector")]
  public class ChequeRefundInfoFull : ChequeRefundInfo
  {
    private DateTime ChequeDateTimeField;
    private string ChequeNumberField;
    private Decimal ChequeSumField;

    [DataMember]
    public DateTime ChequeDateTime
    {
      get => this.ChequeDateTimeField;
      set => this.ChequeDateTimeField = value;
    }

    [DataMember]
    public string ChequeNumber
    {
      get => this.ChequeNumberField;
      set => this.ChequeNumberField = value;
    }

    [DataMember]
    public Decimal ChequeSum
    {
      get => this.ChequeSumField;
      set => this.ChequeSumField = value;
    }
  }
}
