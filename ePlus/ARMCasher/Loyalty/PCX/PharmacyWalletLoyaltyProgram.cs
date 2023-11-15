// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.PharmacyWalletLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessLogic;
using ePlus.Loyalty;
using ePlus.Loyalty.PharmacyWallet;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;

namespace ePlus.ARMCasher.Loyalty.PCX
{
  internal class PharmacyWalletLoyaltyProgram : PCXLoyaltyProgramEx
  {
    private static readonly Guid _chequeOperTypeCharge = new Guid("C65D562E-F17E-4A5A-A851-966DE23BA7D6");
    private static readonly Guid _chequeOperTypeDebit = new Guid("05F3045F-7F74-48D4-AA79-EF276C8A5A21");
    private static readonly Guid _chequeOperTypeRefundCharge = new Guid("CDA64C3B-A3FC-4271-B706-5105F4003DA2");
    private static readonly Guid _chequeOperTypeRefundDebit = new Guid("942CCDC8-3578-445A-8659-1F5FB67D2D53");
    private static string _name;
    private static Guid _id;
    private static bool _isSettingsInitialized;
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();

    protected override Guid ChequeOperTypeCharge => PharmacyWalletLoyaltyProgram._chequeOperTypeCharge;

    protected override Guid ChequeOperTypeDebit => PharmacyWalletLoyaltyProgram._chequeOperTypeDebit;

    protected override Guid ChequeOperTypeRefundCharge => PharmacyWalletLoyaltyProgram._chequeOperTypeRefundCharge;

    protected override Guid ChequeOperTypeRefundDebit => PharmacyWalletLoyaltyProgram._chequeOperTypeRefundDebit;

    private static Settings Settings { get; set; }

    private static Certificate Certificate { get; set; }

    private static Params Params { get; set; }

    protected override bool IsSettingsInitialized
    {
      get => PharmacyWalletLoyaltyProgram._isSettingsInitialized;
      set => PharmacyWalletLoyaltyProgram._isSettingsInitialized = value;
    }

    private static bool IscompatibilityEnabled { get; set; }

    public override string Name => PharmacyWalletLoyaltyProgram._name;

    public override Guid IdGlobal => PharmacyWalletLoyaltyProgram._id;

    protected override Decimal Devider
    {
      get => PharmacyWalletLoyaltyProgram.Params.Devider;
      set => PharmacyWalletLoyaltyProgram.Params.Devider = value;
    }

    protected override Decimal MinChequeSumForCharge
    {
      get => PharmacyWalletLoyaltyProgram.Params.MinChequeSumForCharge;
      set => PharmacyWalletLoyaltyProgram.Params.MinChequeSumForCharge = value;
    }

    protected override Decimal MinPayPercent
    {
      get => PharmacyWalletLoyaltyProgram.Params.MinPayPercent;
      set => PharmacyWalletLoyaltyProgram.Params.MinPayPercent = value;
    }

    protected override string UnitName
    {
      get => PharmacyWalletLoyaltyProgram.Params.UnitName;
      set => PharmacyWalletLoyaltyProgram.Params.UnitName = value;
    }

    protected override Decimal OfflineChargePercent => 2.0M;

    protected override bool DoIsCompatibleTo(Guid discountId) => PharmacyWalletLoyaltyProgram.IscompatibilityEnabled && !PharmacyWalletLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void OnInitSettings()
    {
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      PharmacyWalletLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      PharmacyWalletLoyaltyProgram.Certificate = settingsModel.Deserialize<Certificate>(loyaltySettings.SETTINGS, "Certificate");
      PharmacyWalletLoyaltyProgram.Params = settingsModel.Deserialize<Params>(loyaltySettings.PARAMS, "Params");
      PharmacyWalletLoyaltyProgram.Params.Devider = PharmacyWalletLoyaltyProgram.Params.Devider == 0M ? 1M : PharmacyWalletLoyaltyProgram.Params.Devider;
      PharmacyWalletLoyaltyProgram.Params.ScorePerRub = PharmacyWalletLoyaltyProgram.Params.ScorePerRub == 0M ? 1M : PharmacyWalletLoyaltyProgram.Params.ScorePerRub;
      this.SendRecvTimeout = PharmacyWalletLoyaltyProgram.Settings.SendReciveTimeout == 0 ? 30 : PharmacyWalletLoyaltyProgram.Settings.SendReciveTimeout;
      PharmacyWalletLoyaltyProgram._id = loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL;
      PharmacyWalletLoyaltyProgram._name = loyaltySettings.NAME;
      PharmacyWalletLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (!PharmacyWalletLoyaltyProgram.IscompatibilityEnabled)
        return;
      PharmacyWalletLoyaltyProgram.ExcludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
        PharmacyWalletLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
        PharmacyWalletLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
        PharmacyWalletLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
    }

    protected override void OnPCXSettings()
    {
      this.PcxObject.ConnectionString = PharmacyWalletLoyaltyProgram.Settings.Url;
      this.PcxObject.ConnectTimeout = PharmacyWalletLoyaltyProgram.Settings.ConnectionTimeout;
      this.PcxObject.SendRecvTimeout = PharmacyWalletLoyaltyProgram.Settings.SendReciveTimeout;
      this.PcxObject.Location = PharmacyWalletLoyaltyProgram.Settings.Location;
      this.PcxObject.PartnerID = PharmacyWalletLoyaltyProgram.Settings.PartnerId;
      this.PcxObject.BackgndFlushPeriod = PharmacyWalletLoyaltyProgram.Settings.BkgndFlushPeriod;
      if (PharmacyWalletLoyaltyProgram.Settings.Proxy.Use)
      {
        this.PcxObject.ProxyHost = PharmacyWalletLoyaltyProgram.Settings.Proxy.Address;
        this.PcxObject.ProxyPort = PharmacyWalletLoyaltyProgram.Settings.Proxy.Port;
        this.PcxObject.ProxyUserId = PharmacyWalletLoyaltyProgram.Settings.Proxy.User;
        this.PcxObject.ProxyUserPass = PharmacyWalletLoyaltyProgram.Settings.Proxy.Password;
      }
      this.PcxObject.Terminal = PharmacyWalletLoyaltyProgram.Settings.Terminal;
      if (PharmacyWalletLoyaltyProgram.Certificate.SertInStorage)
      {
        this.PcxObject.CertSubjectName = PharmacyWalletLoyaltyProgram.Certificate.SertName;
      }
      else
      {
        this.PcxObject.CertFilePath = PharmacyWalletLoyaltyProgram.Certificate.SertFilePath;
        this.PcxObject.KeyFilePath = PharmacyWalletLoyaltyProgram.Certificate.KeyFilePath;
        this.PcxObject.KeyPassword = PharmacyWalletLoyaltyProgram.Certificate.CertPassword;
      }
      if (!LoyaltyProgManager.IsLoyalityProgramEnabled(LoyaltyType.Svyaznoy) && !AppConfigurator.EnableSberbank)
        return;
      int num = this.PcxObject.Init();
      if (num != 0)
        throw new LoyaltyException((ILoyaltyProgram) this, "Объект PCX, не создан  возможно ActiveX компонент не установлен" + Environment.NewLine + ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
    }

    public PharmacyWalletLoyaltyProgram(string clientId)
      : base(LoyaltyType.PharmacyWallet, clientId, clientId, "PH_WL")
    {
    }
  }
}
