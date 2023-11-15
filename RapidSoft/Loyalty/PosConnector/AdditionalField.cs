// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.AdditionalField
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DataContract(Name = "AdditionalField", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DebuggerStepThrough]
  public class AdditionalField : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private string NameField;
    private AdditionalFieldType TypeField;
    private string ValueField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string Name
    {
      get => this.NameField;
      set => this.NameField = value;
    }

    [DataMember]
    public AdditionalFieldType Type
    {
      get => this.TypeField;
      set => this.TypeField = value;
    }

    [DataMember]
    public string Value
    {
      get => this.ValueField;
      set => this.ValueField = value;
    }
  }
}
