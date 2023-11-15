// Decompiled with JetBrains decompiler
// Type: PosConnectorClient
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using RapidSoft.Loyalty.PosConnector;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class PosConnectorClient : ClientBase<global::PosConnector>, global::PosConnector
{
  public PosConnectorClient()
  {
  }

  public PosConnectorClient(string endpointConfigurationName)
    : base(endpointConfigurationName)
  {
  }

  public PosConnectorClient(string endpointConfigurationName, string remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  public PosConnectorClient(string endpointConfigurationName, EndpointAddress remoteAddress)
    : base(endpointConfigurationName, remoteAddress)
  {
  }

  public PosConnectorClient(Binding binding, EndpointAddress remoteAddress)
    : base(binding, remoteAddress)
  {
  }

  public ApplyDiscountResponse ApplyDiscount(ApplyDiscountRequest request) => this.Channel.ApplyDiscount(request);

  public GetBalanceResponse GetBalance(GetBalanceRequest request) => this.Channel.GetBalance(request);

  public RefundByChequeResponse RefundByCheque(RefundByChequeRequest request) => this.Channel.RefundByCheque(request);

  public RefundResponse Refund(RefundRequest request) => this.Channel.Refund(request);

  public RollbackResponse Rollback(RollbackRequest request) => this.Channel.Rollback(request);

  public FindTransactionsResponse FindLastTransactions(FindTransactionsRequest request) => this.Channel.FindLastTransactions(request);
}
