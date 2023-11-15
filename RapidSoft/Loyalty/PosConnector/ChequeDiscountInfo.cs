// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.ChequeDiscountInfo
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
  [DataContract(Name = "ChequeDiscountInfo", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [DebuggerStepThrough]
  public class ChequeDiscountInfo : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private AdditionalField[] CustomFieldsField;
    private Decimal? FinalChequeDiscountField;
    private ChequeItemDiscountInfo[] ItemsField;
    private Decimal? MinSumField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public AdditionalField[] CustomFields
    {
      get => this.CustomFieldsField;
      set => this.CustomFieldsField = value;
    }

    [DataMember]
    public Decimal? FinalChequeDiscount
    {
      get => this.FinalChequeDiscountField;
      set => this.FinalChequeDiscountField = value;
    }

    [DataMember]
    public ChequeItemDiscountInfo[] Items
    {
      get => this.ItemsField;
      set => this.ItemsField = value;
    }

    [DataMember]
    public Decimal? MinSum
    {
      get => this.MinSumField;
      set => this.MinSumField = value;
    }
  }
}
