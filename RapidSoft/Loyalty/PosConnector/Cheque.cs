// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.Cheque
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DataContract(Name = "Cheque", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [DebuggerStepThrough]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  public class Cheque : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private DateTime ChequeDateTimeField;
    private Decimal ChequeDiscountField;
    private string ChequeNumberField;
    private Decimal ChequeSumField;
    private string CurrencyField;
    private AdditionalField[] CustomFieldsField;
    private ChequeItem[] ItemsField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public DateTime ChequeDateTime
    {
      get => this.ChequeDateTimeField;
      set => this.ChequeDateTimeField = value;
    }

    [DataMember]
    public Decimal ChequeDiscount
    {
      get => this.ChequeDiscountField;
      set => this.ChequeDiscountField = value;
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

    [DataMember]
    public string Currency
    {
      get => this.CurrencyField;
      set => this.CurrencyField = value;
    }

    [DataMember]
    public AdditionalField[] CustomFields
    {
      get => this.CustomFieldsField;
      set => this.CustomFieldsField = value;
    }

    [DataMember]
    public ChequeItem[] Items
    {
      get => this.ItemsField;
      set => this.ItemsField = value;
    }
  }
}
