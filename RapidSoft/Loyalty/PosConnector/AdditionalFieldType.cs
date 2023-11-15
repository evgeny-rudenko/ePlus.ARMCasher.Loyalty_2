// Decompiled with JetBrains decompiler
// Type: RapidSoft.Loyalty.PosConnector.AdditionalFieldType
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace RapidSoft.Loyalty.PosConnector
{
  [DataContract(Name = "AdditionalFieldType", Namespace = "RapidSoft.Loyalty.PosConnector")]
  [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
  public enum AdditionalFieldType
  {
    [EnumMember] Byte,
    [EnumMember] DateTime,
    [EnumMember] Double,
    [EnumMember] Float,
    [EnumMember] Int32,
    [EnumMember] Int64,
    [EnumMember] String,
  }
}
