// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.ChequeItemDiscountInfo
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
  [DataContract(Name = "ChequeItemDiscountInfo", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [DebuggerStepThrough]
  public class ChequeItemDiscountInfo : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private string ArticleIdField;
    private AdditionalField[] CustomFieldsField;
    private Decimal DiscountField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string ArticleId
    {
      get => this.ArticleIdField;
      set => this.ArticleIdField = value;
    }

    [DataMember]
    public AdditionalField[] CustomFields
    {
      get => this.CustomFieldsField;
      set => this.CustomFieldsField = value;
    }

    [DataMember]
    public Decimal Discount
    {
      get => this.DiscountField;
      set => this.DiscountField = value;
    }
  }
}
