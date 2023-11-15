// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.ChequeRefundInfo
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [DebuggerStepThrough]
  [DataContract(Name = "ChequeRefundInfo", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [KnownType(typeof (ChequeRefundInfoFull))]
  public class ChequeRefundInfo : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private string CurrencyField;
    private ChequeItemRefundInfo[] ItemsField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string Currency
    {
      get => this.CurrencyField;
      set => this.CurrencyField = value;
    }

    [DataMember]
    public ChequeItemRefundInfo[] Items
    {
      get => this.ItemsField;
      set => this.ItemsField = value;
    }
  }
}
