// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.GetBalanceResponse
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
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DataContract(Name = "GetBalanceResponse", Namespace = "RapidSoft.Loyalty.PosConnector")]
  public class GetBalanceResponse : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private int CardStatusField;
    private string ClientMessageField;
    private AdditionalField[] CustomFieldsField;
    private DateTime DateTimeField;
    private string ErrorField;
    private string ExternalCardNumberField;
    private int ExternalProgramIdField;
    private Decimal MinSumField;
    private Decimal MoneyAuthLimitField;
    private Decimal MoneyBalanceField;
    private string MoneyCurrencyField;
    private Decimal PointsAuthLimitField;
    private Decimal PointsBalanceField;
    private string PointsCurrencyField;
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
    public string ClientMessage
    {
      get => this.ClientMessageField;
      set => this.ClientMessageField = value;
    }

    [DataMember]
    public AdditionalField[] CustomFields
    {
      get => this.CustomFieldsField;
      set => this.CustomFieldsField = value;
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
    public Decimal MinSum
    {
      get => this.MinSumField;
      set => this.MinSumField = value;
    }

    [DataMember]
    public Decimal MoneyAuthLimit
    {
      get => this.MoneyAuthLimitField;
      set => this.MoneyAuthLimitField = value;
    }

    [DataMember]
    public Decimal MoneyBalance
    {
      get => this.MoneyBalanceField;
      set => this.MoneyBalanceField = value;
    }

    [DataMember]
    public string MoneyCurrency
    {
      get => this.MoneyCurrencyField;
      set => this.MoneyCurrencyField = value;
    }

    [DataMember]
    public Decimal PointsAuthLimit
    {
      get => this.PointsAuthLimitField;
      set => this.PointsAuthLimitField = value;
    }

    [DataMember]
    public Decimal PointsBalance
    {
      get => this.PointsBalanceField;
      set => this.PointsBalanceField = value;
    }

    [DataMember]
    public string PointsCurrency
    {
      get => this.PointsCurrencyField;
      set => this.PointsCurrencyField = value;
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
