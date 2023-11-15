// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Database.RapidCheque
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;

namespace ePlus.ARMCasher.Loyalty.Database
{
  internal class RapidCheque
  {
    private Guid _chequeId;
    private string _operation;
    private Decimal _summ;
    private DateTime _date;
    private string _clientId;
    private Guid _requestId;

    public Guid ChequeId
    {
      get => this._chequeId;
      set => this._chequeId = value;
    }

    public Guid RequestId
    {
      get => this._requestId;
      set => this._requestId = value;
    }

    public string ClientId
    {
      get => this._clientId;
      set => this._clientId = value;
    }

    public string Operation
    {
      get => this._operation;
      set => this._operation = value;
    }

    public Decimal Summ
    {
      get => this._summ;
      set => this._summ = value;
    }

    public DateTime ChequeDate
    {
      get => this._date;
      set => this._date = value;
    }

    public long Date
    {
      get => this._date.Ticks;
      set => this._date = new DateTime(value, DateTimeKind.Local);
    }

    public enum RapidChequeOperation
    {
      MakeDiscount,
      CancelDiscount,
    }
  }
}
