// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.ApplyDiscountResponse
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DataContract(Name = "ApplyDiscountResponse", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  public class ApplyDiscountResponse : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private int CardStatusField;
    private ChequeDiscountInfo ChequeField;
    private string ChequeMessageField;
    private string ClientMessageField;
    private DateTime DateTimeField;
    private string ErrorField;
    private string ExternalCardNumberField;
    private int ExternalProgramIdField;
    private string OperatorMessageField;
    private int StatusField;
    private DateTime UtcDateTimeField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public int CardStatus
    {
      get => this.CardStatusField;
      set => this.CardStatusField = value;
    }

    [DataMember]
    public ChequeDiscountInfo Cheque
    {
      get => this.ChequeField;
      set => this.ChequeField = value;
    }

    [DataMember]
    public string ChequeMessage
    {
      get => this.ChequeMessageField;
      set => this.ChequeMessageField = value;
    }

    [DataMember]
    public string ClientMessage
    {
      get => this.ClientMessageField;
      set => this.ClientMessageField = value;
    }

    [DataMember]
    public DateTime DateTime
    {
      get => this.DateTimeField;
      set => this.DateTimeField = value;
    }

    [DataMember]
    public string Error
    {
      get => this.ErrorField;
      set => this.ErrorField = value;
    }

    [DataMember]
    public string ExternalCardNumber
    {
      get => this.ExternalCardNumberField;
      set => this.ExternalCardNumberField = value;
    }

    [DataMember]
    public int ExternalProgramId
    {
      get => this.ExternalProgramIdField;
      set => this.ExternalProgramIdField = value;
    }

    [DataMember]
    public string OperatorMessage
    {
      get => this.OperatorMessageField;
      set => this.OperatorMessageField = value;
    }

    [DataMember]
    public int Status
    {
      get => this.StatusField;
      set => this.StatusField = value;
    }

    [DataMember]
    public DateTime UtcDateTime
    {
      get => this.UtcDateTimeField;
      set => this.UtcDateTimeField = value;
    }
  }
}
