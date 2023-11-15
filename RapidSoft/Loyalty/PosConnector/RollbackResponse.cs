// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.RollbackResponse
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
  [DataContract(Name = "RollbackResponse", Namespace = "RapidSoft.Loyalty.PosConnector")]
  public class RollbackResponse : IExtensibleDataObject
  {
    private ExtensionDataObject extensionDataField;
    private DateTime DateTimeField;
    private int StatusField;
    private DateTime UtcDateTimeField;

    public ExtensionDataObject ExtensionData
    {
      get => this.extensionDataField;
      set => this.extensionDataField = value;
    }

    [DataMember]
    public DateTime DateTime
    {
      get => this.DateTimeField;
      set => this.DateTimeField = value;
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
