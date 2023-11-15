// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.PointRequest
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  [KnownType(typeof (FindTransactionsRequest))]
  [DebuggerStepThrough]
  [DataContract(Name = "PointRequest", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [KnownType(typeof (RequestBase))]
  [KnownType(typeof (GetBalanceRequest))]
  [KnownType(typeof (RefundByChequeRequest))]
  [KnownType(typeof (RefundRequest))]
  [KnownType(typeof (RollbackRequest))]
  [KnownType(typeof (ApplyDiscountRequest))]
  public class PointRequest : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private string PartnerIdField;
    private string PosIdField;
    private string TerminalIdField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public string PartnerId
    {
      get => this.PartnerIdField;
      set => this.PartnerIdField = value;
    }

    [DataMember]
    public string PosId
    {
      get => this.PosIdField;
      set => this.PosIdField = value;
    }

    [DataMember]
    public string TerminalId
    {
      get => this.TerminalIdField;
      set => this.TerminalIdField = value;
    }
  }
}
