// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.RollbackRequest
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DebuggerStepThrough]
  [DataContract(Name = "RollbackRequest", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  public class RollbackRequest : RequestBase
  {
    private string OriginalRequestIdField;

    [DataMember]
    public string OriginalRequestId
    {
      get => this.OriginalRequestIdField;
      set => this.OriginalRequestIdField = value;
    }
  }
}
