// Decompiled with JetBrains decompiler
// Type: PosConnector
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using RapidSoft.Loyalty.PosConnector;
using System.CodeDom.Compiler;
using System.ServiceModel;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "RapidSoft.Loyalty.PosConnector.Service", ConfigurationName = "PosConnector")]
public interface PosConnector
{
  [OperationContract(Action = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/ApplyDiscount", ReplyAction = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/ApplyDiscountResponse")]
  ApplyDiscountResponse ApplyDiscount(ApplyDiscountRequest request);

  [OperationContract(Action = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/GetBalance", ReplyAction = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/GetBalanceResponse")]
  GetBalanceResponse GetBalance(GetBalanceRequest request);

  [OperationContract(Action = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/RefundByCheque", ReplyAction = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/RefundByChequeResponse")]
  RefundByChequeResponse RefundByCheque(RefundByChequeRequest request);

  [OperationContract(Action = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/Refund", ReplyAction = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/RefundResponse")]
  RefundResponse Refund(RefundRequest request);

  [OperationContract(Action = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/Rollback", ReplyAction = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/RollbackResponse")]
  RollbackResponse Rollback(RollbackRequest request);

  [OperationContract(Action = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/FindLastTransactions", ReplyAction = "RapidSoft.Loyalty.PosConnector.Service/PosConnector/FindLastTransactionsResponse")]
  FindTransactionsResponse FindLastTransactions(FindTransactionsRequest request);
}
