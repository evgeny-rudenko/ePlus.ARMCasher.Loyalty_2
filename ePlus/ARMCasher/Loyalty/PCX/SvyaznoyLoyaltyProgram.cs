// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.SvyaznoyLoyaltyProgram
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
  internal class SvyaznoyLoyaltyProgram : PCXLoyaltyProgramEx
  {
    private static readonly Guid _chequeOperTypeCharge = new Guid("D4D0A8BF-36E0-49EB-BE2D-E4EC6A64E1A9");
    private static readonly Guid _chequeOperTypeDebit = new Guid("52AE7BC7-A480-4B69-BA2F-D4AD7E60EAFC");
    private static readonly Guid _chequeOperTypeRefundCharge = new Guid("738AB360-B233-4719-A51D-E052E6ED1C70");
    private static readonly Guid _chequeOperTypeRefundDebit = new Guid("09432046-CAA6-4102-9FC6-241BCAF83B95");
    private static string _name;
    private static Guid _id;
    private static bool _isSettingsInitialized;
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();

    protected override Guid ChequeOperTypeCharge => SvyaznoyLoyaltyProgram._chequeOperTypeCharge;

    protected override Guid ChequeOperTypeDebit => SvyaznoyLoyaltyProgram._chequeOperTypeDebit;

    protected override Guid ChequeOperTypeRefundCharge => SvyaznoyLoyaltyProgram._chequeOperTypeRefundCharge;

    protected override Guid ChequeOperTypeRefundDebit => SvyaznoyLoyaltyProgram._chequeOperTypeRefundDebit;

    private static Settings Settings { get; set; }

    private static Certificate Certificate { get; set; }

    private static Params Params { get; set; }

    protected override bool IsSettingsInitialized
    {
      get => SvyaznoyLoyaltyProgram._isSettingsInitialized;
      set => SvyaznoyLoyaltyProgram._isSettingsInitialized = value;
    }

    private static bool IscompatibilityEnabled { get; set; }

    public override string Name => SvyaznoyLoyaltyProgram._name;

    public override Guid IdGlobal => SvyaznoyLoyaltyProgram._id;

    protected override Decimal Devider
    {
      get => SvyaznoyLoyaltyProgram.Params.Devider;
      set => SvyaznoyLoyaltyProgram.Params.Devider = value;
    }

    protected override Decimal MinChequeSumForCharge
    {
      get => SvyaznoyLoyaltyProgram.Params.MinChequeSumForCharge;
      set => SvyaznoyLoyaltyProgram.Params.MinChequeSumForCharge = value;
    }

    protected override Decimal MinPayPercent
    {
      get => SvyaznoyLoyaltyProgram.Params.MinPayPercent;
      set => SvyaznoyLoyaltyProgram.Params.MinPayPercent = value;
    }

    protected override string UnitName
    {
      get => SvyaznoyLoyaltyProgram.Params.UnitName;
      set => SvyaznoyLoyaltyProgram.Params.UnitName = value;
    }

    protected override Decimal OfflineChargePercent => 1.0M;

    protected override bool DoIsCompatibleTo(Guid discountId) => SvyaznoyLoyaltyProgram.IscompatibilityEnabled && !SvyaznoyLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void OnInitSettings()
    {
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      SvyaznoyLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      SvyaznoyLoyaltyProgram.Certificate = settingsModel.Deserialize<Certificate>(loyaltySettings.SETTINGS, "Certificate");
      SvyaznoyLoyaltyProgram.Params = settingsModel.Deserialize<Params>(loyaltySettings.PARAMS, "Params");
      SvyaznoyLoyaltyProgram.Params.Devider = SvyaznoyLoyaltyProgram.Params.Devider == 0M ? 1M : SvyaznoyLoyaltyProgram.Params.Devider;
      SvyaznoyLoyaltyProgram.Params.ScorePerRub = SvyaznoyLoyaltyProgram.Params.ScorePerRub == 0M ? 1M : SvyaznoyLoyaltyProgram.Params.ScorePerRub;
      this.SendRecvTimeout = SvyaznoyLoyaltyProgram.Settings.SendReciveTimeout == 0 ? 30 : SvyaznoyLoyaltyProgram.Settings.SendReciveTimeout;
      SvyaznoyLoyaltyProgram._id = loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL;
      SvyaznoyLoyaltyProgram._name = loyaltySettings.NAME;
      SvyaznoyLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (!SvyaznoyLoyaltyProgram.IscompatibilityEnabled)
        return;
      SvyaznoyLoyaltyProgram.ExcludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
        SvyaznoyLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
        SvyaznoyLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
        SvyaznoyLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
    }

    protected override void OnPCXSettings()
    {
      this.PcxObject.ConnectionString = SvyaznoyLoyaltyProgram.Settings.Url;
      this.PcxObject.ConnectTimeout = SvyaznoyLoyaltyProgram.Settings.ConnectionTimeout;
      this.PcxObject.SendRecvTimeout = SvyaznoyLoyaltyProgram.Settings.SendReciveTimeout;
      this.PcxObject.Location = SvyaznoyLoyaltyProgram.Settings.Location;
      this.PcxObject.PartnerID = SvyaznoyLoyaltyProgram.Settings.PartnerId;
      this.PcxObject.BackgndFlushPeriod = SvyaznoyLoyaltyProgram.Settings.BkgndFlushPeriod;
      if (SvyaznoyLoyaltyProgram.Settings.Proxy.Use)
      {
        this.PcxObject.ProxyHost = SvyaznoyLoyaltyProgram.Settings.Proxy.Address;
        this.PcxObject.ProxyPort = SvyaznoyLoyaltyProgram.Settings.Proxy.Port;
        this.PcxObject.ProxyUserId = SvyaznoyLoyaltyProgram.Settings.Proxy.User;
        this.PcxObject.ProxyUserPass = SvyaznoyLoyaltyProgram.Settings.Proxy.Password;
      }
      this.PcxObject.Terminal = SvyaznoyLoyaltyProgram.Settings.Terminal;
      if (SvyaznoyLoyaltyProgram.Certificate.SertInStorage)
      {
        this.PcxObject.CertSubjectName = SvyaznoyLoyaltyProgram.Certificate.SertName;
      }
      else
      {
        this.PcxObject.CertFilePath = SvyaznoyLoyaltyProgram.Certificate.SertFilePath;
        this.PcxObject.KeyFilePath = SvyaznoyLoyaltyProgram.Certificate.KeyFilePath;
        this.PcxObject.KeyPassword = SvyaznoyLoyaltyProgram.Certificate.CertPassword;
      }
      if (!LoyaltyProgManager.IsLoyalityProgramEnabled(LoyaltyType.Svyaznoy) && !AppConfigurator.EnableSberbank)
        return;
      int num = this.PcxObject.Init();
      if (num != 0)
        throw new LoyaltyException((ILoyaltyProgram) this, "Объект PCX, не создан  возможно ActiveX компонент не установлен" + Environment.NewLine + ErrorMessage.GetErrorMessage(num, this.PcxObject.GetErrorMessage(num)));
    }

    public SvyaznoyLoyaltyProgram(string clientId)
      : base(LoyaltyType.Svyaznoy, clientId, clientId, "SVYAZ")
    {
    }
  }
}
