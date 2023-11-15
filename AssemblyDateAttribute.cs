// Decompiled with JetBrains decompiler
// Type: AssemblyDateAttribute
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.Runtime.InteropServices;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
[ComVisible(true)]
public sealed class AssemblyDateAttribute : Attribute
{
  private static readonly DateTime dt = new DateTime(2021, 12, 1);

  public DateTime AssemblyDate => AssemblyDateAttribute.dt;
}
