// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.RapidSoft.RapidSoftLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon.Config;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.Loyalty.RapidSoft;
using ePlus.MetaData.Client;
using RapidSoft.Loyalty.PosConnector;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.RapidSoft
{
  internal sealed class RapidSoftLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private const string Currency = "RUR";
    private readonly Guid ID_DISCOUNT2_CARD_GLOBAL = new Guid("D04FD3C9-82D9-46B1-BDA8-9A728AB5E7C1");
    private static bool _isSettingsInit;
    private static Dictionary<Guid, DataRowItem> _excludedPrograms = new Dictionary<Guid, DataRowItem>();
    private static string _name;
    private static Guid _idGlobal;
    private Guid? _lastTranRequestId;
    private Guid? _lastTranChequeId;
    private GetBalanceResponse _balance;

    private static string PosId { get; set; }

    private static Settings Settings { get; set; }

    private static Certificate Certificate { get; set; }

    private static bool IscompatibilityEnabled { get; set; }

    protected string CardNumber { get; set; }

    public override string Name => RapidSoftLoyaltyProgram._name;

    public override Guid IdGlobal => RapidSoftLoyaltyProgram._idGlobal;

    private bool IsSettingsInit
    {
      get => RapidSoftLoyaltyProgram._isSettingsInit;
      set => RapidSoftLoyaltyProgram._isSettingsInit = value;
    }

    public RapidSoftLoyaltyProgram(string PublicId, string cardNumber)
      : base(LoyaltyType.RapidSoft, PublicId, cardNumber, "")
    {
      this.CardNumber = cardNumber;
      this.LoadSettings();
    }

    protected override bool DoIsCompatibleTo(Guid discountId)
    {
      this.LoadSettings();
      return RapidSoftLoyaltyProgram.IscompatibilityEnabled && !RapidSoftLoyaltyProgram._excludedPrograms.ContainsKey(discountId);
    }

    protected override void OnInitInternal() => throw new NotImplementedException();

    protected override void OnInitSettings() => throw new NotImplementedException();

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      PosConnectorClient posConnectorClient = this.Connect();
      if (posConnectorClient == null)
        throw new Exception("Не удалось получить доступ к сервису RapidSoft");
      this._balance = new GetBalanceResponse();
      GetBalanceRequest request = new GetBalanceRequest();
      request.CardData = this.ClientId;
      request.PartnerId = RapidSoftLoyaltyProgram.Settings.PartnerId;
      request.PosId = RapidSoftLoyaltyProgram.PosId;
      request.RequestDateTime = DateTime.Now;
      request.TerminalId = RapidSoftLoyaltyProgram.Settings.Terminal;
      request.RequestId = Guid.NewGuid().ToString();
      this._balance = posConnectorClient.GetBalance(request);
      string str = string.Empty;
      switch (this._balance.CardStatus)
      {
        case 0:
          str = "Не найдена";
          break;
        case 1:
          str = "Активна";
          break;
        case 20:
          str = "Лимитирована";
          break;
        case 30:
          str = "Заблокирована";
          break;
        case 40:
          str = "Не активирована";
          break;
        case 50:
          str = "Истёк срок действия";
          break;
      }
      return new LoyaltyCardInfo()
      {
        Balance = this._balance.MoneyBalance,
        Points = this._balance.PointsBalance,
        CardNumber = this.CardNumber,
        CardStatus = str,
        ClientId = this.ClientId
      };
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque) => 0M;

    private void LoadSettings()
    {
      if (this.IsSettingsInit)
        return;
      try
      {
        using (SqlConnection connection = new SqlConnection(MultiServerBL.ClientConnectionString))
        {
          using (SqlCommand sqlCommand = new SqlCommand("SELECT TOP 1 VALUE FROM SYS_OPTION WHERE CODE = 'A_COD_CONTRACTOR_SELF'", connection))
          {
            connection.Open();
            RapidSoftLoyaltyProgram.PosId = (string) sqlCommand.ExecuteScalar();
          }
        }
      }
      catch (Exception ex)
      {
        throw new Exception("Не удалось получить код контрагента из базы АРМ", ex);
      }
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      RapidSoftLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      RapidSoftLoyaltyProgram.Settings.Terminal = Convert.ToString(AppConfigManager.IdCashRegister);
      RapidSoftLoyaltyProgram.Certificate = settingsModel.Deserialize<Certificate>(loyaltySettings.SETTINGS, "Certificate");
      RapidSoftLoyaltyProgram._idGlobal = loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL;
      RapidSoftLoyaltyProgram._name = loyaltySettings.NAME;
      RapidSoftLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (RapidSoftLoyaltyProgram.IscompatibilityEnabled)
      {
        RapidSoftLoyaltyProgram._excludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
        foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
          RapidSoftLoyaltyProgram._excludedPrograms.Add(exclude.Guid, exclude);
        foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
          RapidSoftLoyaltyProgram._excludedPrograms.Add(exclude.Guid, exclude);
        foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
          RapidSoftLoyaltyProgram._excludedPrograms.Add(exclude.Guid, exclude);
      }
      this.IsSettingsInit = true;
    }

    private BasicHttpBinding CreateBasicHttpBinding()
    {
      BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
      basicHttpBinding.Name = "BasicHttpsBinding_PosConnector";
      basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
      basicHttpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Certificate;
      return basicHttpBinding;
    }

    private PosConnectorClient Connect()
    {
      this.LoadSettings();
      EndpointAddress remoteAddress = RapidSoftLoyaltyProgram.Settings != null ? new EndpointAddress(RapidSoftLoyaltyProgram.Settings.Url) : throw new Exception("Не задана конфигурация модуля RapidSoft.");
      PosConnectorClient posConnectorClient = new PosConnectorClient((System.ServiceModel.Channels.Binding) this.CreateBasicHttpBinding(), remoteAddress);
      BasicHttpBinding binding = posConnectorClient.Endpoint.Binding as BasicHttpBinding;
      binding.UseDefaultWebProxy = false;
      binding.BypassProxyOnLocal = false;
      if (RapidSoftLoyaltyProgram.Settings.Proxy.Use)
      {
        binding.Security.Mode = BasicHttpSecurityMode.Transport;
        binding.ProxyAddress = new Uri(RapidSoftLoyaltyProgram.Settings.Proxy.Address + ":" + (object) RapidSoftLoyaltyProgram.Settings.Proxy.Port);
        binding.UseDefaultWebProxy = false;
        binding.BypassProxyOnLocal = false;
        binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.Basic;
        posConnectorClient.ClientCredentials.UserName.UserName = RapidSoftLoyaltyProgram.Settings.Proxy.User;
        posConnectorClient.ClientCredentials.UserName.Password = RapidSoftLoyaltyProgram.Settings.Proxy.Password;
      }
      X509Store x509Store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
      x509Store.Open(OpenFlags.ReadOnly);
      X509Certificate2Collection certificate2Collection = x509Store.Certificates.Find(X509FindType.FindByIssuerName, (object) RapidSoftLoyaltyProgram.Certificate.CenterName, false);
      x509Store.Close();
      if (certificate2Collection.Count > 1)
        throw new Exception("Обнаружено более одного сертификата RapidSoft. Исправьте конфигурацию.");
      posConnectorClient.ClientCredentials.ClientCertificate.Certificate = certificate2Collection != null && certificate2Collection.Count > 0 ? certificate2Collection[0] : throw new Exception("Не установлен сертификат клиента для работы с RapidSoft.");
      return posConnectorClient;
    }

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => throw new NotImplementedException();

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => throw new NotImplementedException();

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      throw new NotImplementedException();
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      throw new NotImplementedException();
    }

    private void CreateAndSavePCXChequeItemList(
      IEnumerable<CHEQUE_ITEM> chequeItemList,
      Decimal totalSum,
      Decimal pcxSumMoney,
      Guid transactionId,
      string operType)
    {
      Dictionary<Guid, Decimal> chequeItems = new Dictionary<Guid, Decimal>();
      foreach (CHEQUE_ITEM chequeItem in chequeItemList)
      {
        Decimal summ = chequeItem.SUMM;
        foreach (DISCOUNT2_MAKE_ITEM discount2MakeItem in chequeItem.Discount2MakeItemList)
        {
          if (discount2MakeItem.TYPE == "RAPID")
            summ += discount2MakeItem.AMOUNT;
        }
        chequeItems.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, summ);
      }
      IDictionary<Guid, Decimal> dictionary = LoyaltyProgManager.Distribute((IEnumerable<KeyValuePair<Guid, Decimal>>) chequeItems, totalSum, Math.Abs(pcxSumMoney));
      List<PCX_CHEQUE_ITEM> chequeItemList1 = new List<PCX_CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem in chequeItemList)
      {
        PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM()
        {
          TRANSACTION_ID = transactionId.ToString(),
          CLIENT_ID = this.ClientId,
          CLIENT_ID_TYPE = (int) this.LoyaltyType,
          OPER_TYPE = operType
        };
        chequeItemList1.Add(pcxChequeItem);
        pcxChequeItem.SUMM = dictionary[chequeItem.ID_CHEQUE_ITEM_GLOBAL];
        pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL;
      }
      if (chequeItemList1.Count <= 0)
        return;
      new PCX_CHEQUE_ITEM_BL().Save(chequeItemList1);
    }

    private void RollbackLastTransaction(out string slipCheque)
    {
      if (!this._lastTranRequestId.HasValue || !this._lastTranChequeId.HasValue)
        throw new Exception("Нет данных о последней транзакции");
      slipCheque = string.Empty;
      DateTime now = DateTime.Now;
      Guid guid = Guid.NewGuid();
      PosConnectorClient posConnectorClient = this.Connect();
      if (posConnectorClient == null)
        throw new Exception("Не удалось получить доступ к сервису RapidSoft");
      RollbackRequest request = new RollbackRequest();
      request.PartnerId = RapidSoftLoyaltyProgram.Settings.PartnerId;
      request.PosId = RapidSoftLoyaltyProgram.PosId;
      request.TerminalId = RapidSoftLoyaltyProgram.Settings.Terminal;
      request.RequestDateTime = now;
      request.RequestId = guid.ToString();
      request.OriginalRequestId = this._lastTranRequestId.Value.ToString();
      RollbackResponse rollbackResponse = (RollbackResponse) null;
      try
      {
        rollbackResponse = posConnectorClient.Rollback(request);
      }
      catch (Exception ex)
      {
        if (ex is FaultException && ex.Message == "Forbidden")
          throw new Exception("Процессинговый центр отклонил запрос по причине ошибки аутентификации.");
        throw ex;
      }
      finally
      {
        posConnectorClient.Close();
      }
      if (rollbackResponse.Status == 0)
      {
        StringBuilder stringBuilder = new StringBuilder("ОТМЕНА");
        stringBuilder.AppendLine("Точка обслуживания ").Append(RapidSoftLoyaltyProgram.PosId);
        stringBuilder.AppendLine("Дата и время ").Append((object) now);
        stringBuilder.AppendLine(RapidSoftHelper.OperationStatusText((RollbackStatus) rollbackResponse.Status));
        stringBuilder.AppendLine();
        slipCheque = stringBuilder.ToString();
      }
      string str = RapidSoftHelper.OperationStatusText((RollbackStatus) rollbackResponse.Status);
      if (rollbackResponse.Status != 0)
        throw new Exception("Ошибка удаления скидки: " + str);
      BaseLoyaltyProgramEx.QueryLogBl.ReverseQuery(this._lastTranChequeId.Value.ToString());
      string whereExpression = string.Format("TRANSACTION_ID = '{0}'", (object) this._lastTranChequeId.Value);
      BaseLoyaltyProgramEx.PCXChequeItemLoader.Delete(whereExpression);
      BaseLoyaltyProgramEx.PCXChequeLoader.Delete(whereExpression);
    }

    protected override void DoRollback(out string slipCheque) => throw new NotImplementedException();
  }
}
