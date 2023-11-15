// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.ChequeItem
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
  [DataContract(Name = "ChequeItem", Namespace = "RapidSoft.Loyalty.PosConnector")]
  public class ChequeItem : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private Decimal AmountField;
    private string ArticleIdField;
    private string ArticleNameField;
    private AdditionalField[] CustomFieldsField;
    private Decimal ItemSumField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public Decimal Amount
    {
      get => this.AmountField;
      set => this.AmountField = value;
    }

    [DataMember]
    public string ArticleId
    {
      get => this.ArticleIdField;
      set => this.ArticleIdField = value;
    }

    [DataMember]
    public string ArticleName
    {
      get => this.ArticleNameField;
      set => this.ArticleNameField = value;
    }

    [DataMember]
    public AdditionalField[] CustomFields
    {
      get => this.CustomFieldsField;
      set => this.CustomFieldsField = value;
    }

    [DataMember]
    public Decimal ItemSum
    {
      get => this.ItemSumField;
      set => this.ItemSumField = value;
    }
  }
}
