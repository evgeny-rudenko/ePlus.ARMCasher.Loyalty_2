// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.SailPlay.SailPlayLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMBusinessLogic.Caches.ContractorGroupsCaches;
using ePlus.ARMCacheManager;
using ePlus.ARMCacheManager.Interfaces;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCasher.BusinessLogic.Events;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon;
using ePlus.ARMCommon.Log;
using ePlus.Discount2.BusinessObjects;
using ePlus.Loyalty;
using ePlus.Loyalty.Interfaces;
using ePlus.Loyalty.SailPlay;
using ePlus.Loyalty.SailPlay.Forms;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.SailPlay
{
  internal class SailPlayLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private static readonly Guid _chequeOperTypeCharge = new Guid("BE257268-1307-4F25-A4A1-1459B6176347");
    private static readonly Guid _chequeOperTypeDebit = new Guid("8BC86099-9DB6-46A5-9277-2115330F3FF7");
    private static readonly Guid _chequeOperTypeRefundCharge = new Guid("44F6E9F6-72BA-487B-8A2B-EBB52902FEF6");
    private static readonly Guid _chequeOperTypeRefundDebit = new Guid("C6DC7F28-8430-4519-A368-8C23F6BDC98C");
    private static Guid _id = new Guid("3C6BCC91-3F6D-4E6B-A7B0-D1DCFBEF8F00");
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();
    private SailPlayWebApi _api;
    private long? _lastCartId;
    private string codeConfirmation;
    private Guid? lastChequeIdGlobal;

    private Guid ChequeOperTypeCharge => SailPlayLoyaltyProgram._chequeOperTypeCharge;

    private Guid ChequeOperTypeDebit => SailPlayLoyaltyProgram._chequeOperTypeDebit;

    private Guid ChequeOperTypeRefundCharge => SailPlayLoyaltyProgram._chequeOperTypeRefundCharge;

    private Guid ChequeOperTypeRefundDebit => SailPlayLoyaltyProgram._chequeOperTypeRefundDebit;

    private static bool IscompatibilityEnabled { get; set; }

    private static Settings Settings { get; set; }

    private static Params Params { get; set; }

    public override event EventHandler CheckCodeConfirmationEvent;

    public override bool SuccessCodeConfirmation { get; set; }

    protected override bool OnIsExplicitDiscount => false;

    public SailPlayLoyaltyProgram(string publicId)
      : base(LoyaltyType.SailPlay, publicId, publicId, "LP_SPLAY")
    {
      this.SendRecvTimeout = 30;
    }

    public override string Name => "Мое здоровье";

    public override Guid IdGlobal => SailPlayLoyaltyProgram._id;

    public override IEnumerable<string> PersonalAdditionsSale { get; set; }

    protected override bool DoIsCompatibleTo(Guid discountId) => SailPlayLoyaltyProgram.IscompatibilityEnabled && !SailPlayLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => result = (ILpTransResult) null;

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      this.DoProcess(cheque, discountSum, out result);
    }

    protected override void CheckCodeConfirmation(CHEQUE cheque, Decimal discountSum)
    {
      this.SuccessCodeConfirmation = true;
      if (!SailPlayLoyaltyProgram.Params.DebitPermitBySms || !(cheque.SUMM + discountSum >= (Decimal) SailPlayLoyaltyProgram.Params.MinSumForDebit) || this.CheckCodeConfirmationEvent == null)
        return;
      this.CheckCodeConfirmationEvent((object) this, (EventArgs) null);
    }

    private void DoProcess(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (!this._lastCartId.HasValue)
        throw new LoyaltyException((ILoyaltyProgram) this, "Необходимо произвести расчёт корзины, до совершения покупки.");
      OperTypeEnum operTypeEnum;
      Guid guid;
      if (discountSum > 0M)
      {
        operTypeEnum = OperTypeEnum.Debit;
        guid = this.ChequeOperTypeDebit;
      }
      else
      {
        operTypeEnum = OperTypeEnum.Charge;
        guid = this.ChequeOperTypeCharge;
      }
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(cheque.ID_CHEQUE_GLOBAL, this.GetType().Name, guid);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Списание баллов с карты лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторное списание произведено не будет.", (object) this.Name, (object) cheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        this.Log(operTypeEnum, discountSum, cheque);
        this.GetDiscountSum(cheque);
        PurchasesNewResult purchasesNewResult = this._api.PurchasesNew(this._lastCartId.Value, cheque.ID_CHEQUE_GLOBAL);
        Decimal operationSum = operTypeEnum == OperTypeEnum.Debit ? discountSum : purchasesNewResult.Purchase.PointsDelta;
        if (LoyaltyProgManager.InitMarketingActions() && LoyaltyProgManager.MarketingActions != null)
        {
          List<MarketingActionResult> all = ((IEnumerable<MarketingActionResult>) purchasesNewResult.Cart.MarketingActionsApplied).ToList<MarketingActionResult>().FindAll((Predicate<MarketingActionResult>) (x => LoyaltyProgManager.MarketingActions.Any<MarketingAction>((Func<MarketingAction, bool>) (y => y.alias == x.Alias))));
          if (cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is SailPlayCard)) as LoyaltyCard is ILoyaltyMessageList loyaltyMessageList)
          {
            foreach (MarketingActionResult marketingActionResult in all)
            {
              if (marketingActionResult.ClientMessage != null && loyaltyMessageList != null)
              {
                LoyaltyMessage message = new LoyaltyMessage(LoyaltyMessageType.Cheque, marketingActionResult.ClientMessage);
                loyaltyMessageList.Add((ILoyaltyMessage) message);
              }
            }
          }
        }
        this.LogMsg(operTypeEnum, purchasesNewResult.Message);
        LpTransactionData transactionData = new LpTransactionData(cheque.ID_CHEQUE_GLOBAL, guid);
        this.SaveTransaction(operTypeEnum, operationSum, transactionData);
        BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
        this.lastChequeIdGlobal = new Guid?(cheque.ID_CHEQUE_GLOBAL);
        ref ILpTransResult local = ref result;
        LpTransResult lpTransResult1 = new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, purchasesNewResult.Purchase.PointsDelta > 0M ? purchasesNewResult.Purchase.PointsDelta : 0M, discountSum, this.GetLoyaltyCardInfo(true).Balance, string.Empty);
        lpTransResult1.LpType = this.LoyaltyType;
        LpTransResult lpTransResult2 = lpTransResult1;
        local = (ILpTransResult) lpTransResult2;
        this.Log(result.ToSlipCheque());
      }
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult lpResult)
    {
      string empty = string.Empty;
      lpResult = (ILpTransResult) null;
      Decimal operationSum = returnCheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => ci.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => !(mi.TYPE == this.DiscountType) ? 0M : mi.AMOUNT))));
      int num = operationSum > 0M ? 1 : 0;
      OperTypeEnum operTypeEnum = operationSum > 0M ? OperTypeEnum.DebitRefund : OperTypeEnum.ChargeRefund;
      Guid guid = operationSum > 0M ? this.ChequeOperTypeRefundDebit : this.ChequeOperTypeRefundCharge;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(baseCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, guid);
      BusinessLogicEvents.Instance.OnBeginChequeTransaction((object) this, e);
      if (e.IsOperationExists)
      {
        ARMLogger.Trace(string.Format("Возврат по карте лояльности {0} по чеку ID: {1}. Операция найдена в логе транзакций чека, повторный возврат произведен не будет.", (object) this.Name, (object) baseCheque.ID_CHEQUE_GLOBAL));
      }
      else
      {
        PaymentType chequePaymentType = baseCheque.CHEQUE_PAYMENT_TYPE;
        switch (chequePaymentType)
        {
          case PaymentType.Cash:
          case PaymentType.Card:
            if (chequePaymentType != PaymentType.Mixed || !baseCheque.CHEQUE_PAYMENTS.All<CHEQUE_PAYMENT>((Func<CHEQUE_PAYMENT, bool>) (p => p.SEPARATE_TYPE_ENUM == PaymentType.Card || p.SEPARATE_TYPE_ENUM == PaymentType.Cash)))
            {
              PurchaseInfoResult purchaseInfoResult = this._api.PurchaseInfo(baseCheque.ID_CHEQUE_GLOBAL);
              this._api.PurchaseDelete(baseCheque.ID_CHEQUE_GLOBAL);
              this.LogMsg(operTypeEnum, "Информация о продаже успешно удалена из SailPlay");
              LpTransactionData transactionData = new LpTransactionData(baseCheque.ID_CHEQUE_GLOBAL, guid);
              this.SaveTransaction(operTypeEnum, operationSum, transactionData);
              BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
              int chargedSum = purchaseInfoResult.Cart.Cart.Positions.Sum<PurchaseInfoPosition>((Func<PurchaseInfoPosition, int>) (p => p.DiscountPoints));
              int totalPoints = purchaseInfoResult.Cart.Cart.TotalPoints;
              StringBuilder stringBuilder = new StringBuilder();
              stringBuilder.Append('"').Append(this.Name).Append('"').AppendLine();
              if (chargedSum > 0)
              {
                stringBuilder.AppendLine("Возврат списания");
                stringBuilder.Append("Начислено: ").Append(Math.Abs(chargedSum)).AppendLine();
              }
              if (totalPoints > 0)
              {
                stringBuilder.AppendLine("Возврат начисления");
                stringBuilder.Append("Списано: ").Append(Math.Abs(totalPoints)).AppendLine();
              }
              UserInfoResult userInfo = this._api.GetUserInfo(this.ClientPublicId);
              stringBuilder.Append("Баланс: ").Append(userInfo.Points.Total).AppendLine();
              stringBuilder.AppendLine(" ");
              stringBuilder.AppendLine(" ");
              string message = stringBuilder.ToString();
              ref ILpTransResult local = ref lpResult;
              LpTransResult lpTransResult1 = new LpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, (Decimal) chargedSum, (Decimal) totalPoints, userInfo.Points.Confirmed, string.Empty, true);
              lpTransResult1.LpType = this.LoyaltyType;
              LpTransResult lpTransResult2 = lpTransResult1;
              local = (ILpTransResult) lpTransResult2;
              this.Log(message);
              return;
            }
            break;
        }
        throw new LoyaltyException((ILoyaltyProgram) this, "Невозможно выполнить возврат бонусов при оплате отличной от Наличных или Картой");
      }
    }

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
            /*UserInfoResult userInfo = (UserInfoResult) null;
            try
            {
              userInfo = this._api.GetUserInfo(this.ClientPublicIdType, this.ClientPublicId);
            }
            catch (SailPlayUserNotFoundException ex)
            {
              this.Log(string.Format("Пользователь SailPlay ID: {0} не найден в системе.", (object) this.ClientPublicId));
            }
            if (userInfo == null)
            {
              form.Invoke((Delegate) (() =>
              {
                if (MessageBox.Show((IWin32Window) form, "Дисконтная карта не зарегистрирована. Продолжить регистрацию?", "Мое здоровье - Регистрация карты", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                  this.Log("Пользователь отказался от регистрации");
                  throw new LoyaltyException((ILoyaltyProgram) this, "Карта не зарегистрирована");
                }
              }));
              form.Invoke((Delegate) (() =>
              {
                try
                {
                  form.Hide();
                  userInfo = this.FillNewUserInfo(form);
                }
                finally
                {
                  form.Show();
                }
                if (userInfo == null)
                {
                  this.Log("Пользователь отказался от ввода данных");
                  throw new LoyaltyException((ILoyaltyProgram) this, "Новый пользователь не был зарегистрирован");
                }
              }));
              UserInfoResult userInfo1;
              if (this.ClientPublicIdType != PublicIdType.Phone && this._api.TryGetUserInfo(PublicIdType.Phone, userInfo.Phone, out userInfo1))
              {
                this.ShowCodeValidationForm(userInfo1.Phone, false, form);
                UserSex sex = userInfo.Sex == "1" ? UserSex.Male : UserSex.Female;
                this._api.UserAdd(this.ClientPublicId, this.ClientPublicIdType, userInfo.FirstName, userInfo.LastName, userInfo.MiddleName, userInfo.BirthDate, sex, string.Empty);
                this.Log(string.Format("Создан новый пользовательский аккаунт. ID: {0}", (object) this.ClientPublicId));
                this.Log(string.Format("Будет выполнена попытка переноса аккаунта {0} на {1}", (object) userInfo1.ID, (object) this.ClientPublicId));
                if (!string.IsNullOrWhiteSpace(userInfo.EMail))
                  this._api.UserUpdateAddEmail(userInfo.EMail);
                this._api.UsersMerge(PublicIdType.Phone, userInfo1.Phone);
                this.Log("Перенос данных успешно завершён");
                userInfo = this._api.GetUserInfo(this.ClientPublicId);
              }
              else
              {
                this.ShowCodeValidationForm(userInfo.Phone, true, form);
                UserSex sex = userInfo.Sex == "1" ? UserSex.Male : UserSex.Female;
                this._api.UserAdd(this.ClientPublicId, this.ClientPublicIdType, userInfo.FirstName, userInfo.LastName, userInfo.MiddleName, userInfo.BirthDate, sex, this.ClientPublicIdType == PublicIdType.Phone ? string.Empty : userInfo.Phone);
                if (!string.IsNullOrWhiteSpace(userInfo.EMail))
                  this._api.UserUpdateAddEmail(userInfo.EMail);
                try
                {
                  this._api.UserTagAdd((IEnumerable<string>) new List<string>()
                  {
                    userInfo.AgeTag
                  }, (IEnumerable<string>) new List<string>()
                  {
                    "Возраст"
                  });
                }
                catch (Exception ex)
                {
                  this.Log("Не удалось присвоить тег зарегестрированному пользователю SailPlay. Ошибка: " + ex.Message);
                }
                userInfo = this._api.GetUserInfo(this.ClientPublicId);
              }
              if (new ContractorAttributesCache((ICacheManager) new MemoryCacheManager(MemoryCache.Default)).GetCache().BONUS_MZ_ACTIVE && userInfo != null)
              {
                new SailPlay_Bl().SaveRegisterCardMyHealth(ArmSecurityManager.CurrentUserId);
                DataSyncBL.Instance.Sync(new ArmDbSyncDelegate(DataSyncBL.Instance.SyncRegistryMZData));
                BusinessLogicEvents.Instance.OnUpdatePurseBonusEvent((object) null, (EventArgs) null);
              }
            }
            LoyaltyCardInfo cardInfoFromService = new LoyaltyCardInfo()
            {
              ClientId = this.ClientPublicId,
              ClientIdType = this.ClientPublicIdType,
              CardNumber = this.ClientPublicId,
              Points = userInfo.Points.Confirmed
            };
            cardInfoFromService.Balance = cardInfoFromService.Points;
            cardInfoFromService.CardStatus = "Активна";
            cardInfoFromService.ClientEmail = userInfo.EMail;
            cardInfoFromService.ClientPhone = userInfo.Phone;
            cardInfoFromService.UserInfo = (IUserInfo) userInfo;
            return cardInfoFromService;
            */
            UserInfoResult userInfoResult;
            UserInfoResult userInfo = null;
            try
            {
                userInfo = this._api.GetUserInfo(base.ClientPublicIdType, base.ClientPublicId);
            }
            catch (SailPlayUserNotFoundException sailPlayUserNotFoundException)
            {
                base.Log(string.Format("Пользователь SailPlay ID: {0} не найден в системе.", base.ClientPublicId));
            }
            if (userInfo == null)
            {
                form.Invoke(new MethodInvoker(() => {
                    if (MessageBox.Show(form, "Дисконтная карта не зарегистрирована. Продолжить регистрацию?", "Мое здоровье - Регистрация карты", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        base.Log("Пользователь отказался от регистрации");
                        throw new LoyaltyException(this, "Карта не зарегистрирована");
                    }
                }));
                form.Invoke(new MethodInvoker(() => {
                    try
                    {
                        form.Hide();
                        userInfo = this.FillNewUserInfo(form);
                    }
                    finally
                    {
                        form.Show();
                    }
                    if (userInfo == null)
                    {
                        base.Log("Пользователь отказался от ввода данных");
                        throw new LoyaltyException(this, "Новый пользователь не был зарегистрирован");
                    }
                }));
                if (base.ClientPublicIdType == PublicIdType.Phone || !this._api.TryGetUserInfo(PublicIdType.Phone, userInfo.Phone, out userInfoResult))
                {
                    this.ShowCodeValidationForm(userInfo.Phone, true, form);
                    UserSex userSex = (userInfo.Sex == "1" ? UserSex.Male : UserSex.Female);
                    this._api.UserAdd(base.ClientPublicId, base.ClientPublicIdType, userInfo.FirstName, userInfo.LastName, userInfo.MiddleName, userInfo.BirthDate, userSex, (base.ClientPublicIdType == PublicIdType.Phone ? string.Empty : userInfo.Phone));
                    if (!string.IsNullOrWhiteSpace(userInfo.EMail))
                    {
                        this._api.UserUpdateAddEmail(userInfo.EMail);
                    }
                    try
                    {
                        SailPlayWebApi sailPlayWebApi = this._api;
                        List<string> strs = new List<string>()
                        {
                            userInfo.AgeTag
                        };
                        sailPlayWebApi.UserTagAdd(strs, new List<string>()
                        {
                            "Возраст"
                        });
                    }
                    catch (Exception exception)
                    {
                        base.Log(string.Concat("Не удалось присвоить тег зарегестрированному пользователю SailPlay. Ошибка: ", exception.Message));
                    }
                    userInfo = this._api.GetUserInfo(base.ClientPublicId);
                }
                else
                {
                    this.ShowCodeValidationForm(userInfoResult.Phone, false, form);
                    UserSex userSex1 = (userInfo.Sex == "1" ? UserSex.Male : UserSex.Female);
                    this._api.UserAdd(base.ClientPublicId, base.ClientPublicIdType, userInfo.FirstName, userInfo.LastName, userInfo.MiddleName, userInfo.BirthDate, userSex1, string.Empty);
                    base.Log(string.Format("Создан новый пользовательский аккаунт. ID: {0}", base.ClientPublicId));
                    base.Log(string.Format("Будет выполнена попытка переноса аккаунта {0} на {1}", userInfoResult.ID, base.ClientPublicId));
                    if (!string.IsNullOrWhiteSpace(userInfo.EMail))
                    {
                        this._api.UserUpdateAddEmail(userInfo.EMail);
                    }
                    this._api.UsersMerge(PublicIdType.Phone, userInfoResult.Phone);
                    base.Log("Перенос данных успешно завершён");
                    userInfo = this._api.GetUserInfo(base.ClientPublicId);
                }
                if ((new ContractorAttributesCache(new MemoryCacheManager(MemoryCache.Default))).GetCache().BONUS_MZ_ACTIVE && userInfo != null)
                {
                    (new SailPlay_Bl()).SaveRegisterCardMyHealth(ArmSecurityManager.CurrentUserId);
                    DataSyncBL instance = DataSyncBL.Instance;
                    ArmDbSyncDelegate[] armDbSyncDelegate = new ArmDbSyncDelegate[] { new ArmDbSyncDelegate(DataSyncBL.Instance.SyncRegistryMZData) };
                    instance.Sync(armDbSyncDelegate);
                    BusinessLogicEvents.Instance.OnUpdatePurseBonusEvent(null, null);
                }
            }
            LoyaltyCardInfo loyaltyCardInfo = new LoyaltyCardInfo()
            {
                ClientId = base.ClientPublicId,
                ClientIdType = base.ClientPublicIdType,
                CardNumber = base.ClientPublicId,
                Points = userInfo.Points.Confirmed,
                //Balance = loyaltyCardInfo.Points,
                CardStatus = "Активна",
                ClientEmail = userInfo.EMail,
                ClientPhone = userInfo.Phone,
                UserInfo = userInfo
            };
            return loyaltyCardInfo;

        }
        /*
        private void ShowCodeValidationForm(string phone, bool isNewUser, Form owner) => owner.Invoke((Delegate) (() =>
    {
      using (FormCodeValidation formCodeValidation = new FormCodeValidation(this._api, isNewUser, SailPlayLoyaltyProgram.Params)
      {
        Owner = owner
      })
      {
        try
        {
          owner.Hide();
          if (formCodeValidation.ShowDialog(phone) != DialogResult.OK)
          {
            this.Log("Пользователь отказался от ввода кода подтверждения");
            throw new LoyaltyException((ILoyaltyProgram) this, "Новый пользователь не был зарегистрирован");
          }
        }
        finally
        {
          formCodeValidation.Close();
          owner.Show();
        }
      }
    }));
        */
        private void ShowCodeValidationForm(string phone, bool isNewUser, Form owner)
        {
            owner.Invoke(new MethodInvoker(() => {
                using (FormCodeValidation formCodeValidation = new FormCodeValidation(this._api, isNewUser, SailPlayLoyaltyProgram.Params)
                       {
                           Owner = owner
                       })
                {
                    try
                    {
                        owner.Hide();
                        if (formCodeValidation.ShowDialog(phone) != DialogResult.OK)
                        {
                            base.Log("Пользователь отказался от ввода кода подтверждения");
                            throw new LoyaltyException(this, "Новый пользователь не был зарегистрирован");
                        }
                    }
                    finally
                    {
                        formCodeValidation.Close();
                        owner.Show();
                    }
                }
            }));
        }

        private UserInfoResult FillNewUserInfo(Form form)
    {
      this.Log("Вызыван метод регистрации нового пользователя");
      FormSailPlayUserRegister view = new FormSailPlayUserRegister();
      view.Owner = form;
      using (UserRegisterPresenter registerPresenter = new UserRegisterPresenter(view))
        return registerPresenter.ShowView(this.ClientPublicId, this.ClientPublicIdType);
    }

    protected override void OnInitInternal()
    {
      if (this._api != null)
        return;
      this._api = new SailPlayWebApi(SailPlayLoyaltyProgram.Settings, this.ClientPublicIdType, this.ClientPublicId);
    }

    protected override void OnInitSettings()
    {
      if (SailPlayLoyaltyProgram.Settings != null)
        return;
      SettingsModel settingsModel = new SettingsModel();
      LoyaltySettings loyaltySettings = settingsModel.Load(this.LoyaltyType, Guid.Empty);
      SailPlayLoyaltyProgram.Settings = settingsModel.Deserialize<Settings>(loyaltySettings.SETTINGS, "Settings");
      SailPlayLoyaltyProgram.Params = settingsModel.Deserialize<Params>(loyaltySettings.PARAMS, "Params");
      SailPlayLoyaltyProgram.IscompatibilityEnabled = loyaltySettings.COMPATIBILITY;
      if (!SailPlayLoyaltyProgram.IscompatibilityEnabled)
        return;
      SailPlayLoyaltyProgram.ExcludedPrograms.Add(this.IdGlobal, (DataRowItem) null);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
        SailPlayLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
        SailPlayLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
        SailPlayLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
            /*this.ClearSelfAndRoundingDiscounts(cheque);
            LoyaltyCard card = cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is SailPlayCard)) as LoyaltyCard;
            Cart sailPlayCart = LoyaltyProgManager.CreateSailPlayCart(cheque);
            LoyaltyCardInfo loyaltyCardInfo = this.GetLoyaltyCardInfo(false);
            CalcResult calcResult = this._api.CalcMaxDiscount(sailPlayCart, (int) cheque.SUMM);
            Decimal val2 = Math.Min(card == null ? 0.0M : card.DiscountSum, (Decimal) calcResult.Cart.TotalDiscountPointsMax);
            Decimal num = Math.Floor(Decimal.op_Decrement(calcResult.Cart.TotalPrice));
            Decimal discountPoints = Math.Min(num, val2);
            if (discountPoints > 0M)
              calcResult = this._api.CalcMaxDiscount(this.Distribute(calcResult, discountPoints, cheque), (int) cheque.SUMM);
            this._lastCartId = new long?(calcResult.Cart.Id);
            int val1 = ((IEnumerable<CartPositionResult>) calcResult.Cart.Positions).Sum<CartPositionResult>((Func<CartPositionResult, int>) (p => p.DiscountPointsMax));
            card.DiscountSumMax = new Decimal?((Decimal) ((IEnumerable<CartPositionResult>) calcResult.Cart.Positions).Sum<CartPositionResult>((Func<CartPositionResult, int>) (p => p.DiscountPoints)));
            if (card != null)
              this.AddSailPlayExtraDiscounts(calcResult, cheque, card);
            return Math.Min(Math.Min((Decimal) val1, loyaltyCardInfo.Balance), num);*/
            this.ClearSelfAndRoundingDiscounts(cheque);
            LoyaltyCard nullable = cheque.DiscountCardPolicyList.Find((DISCOUNT2_CARD_POLICY c) => c is SailPlayCard) as LoyaltyCard;
            Cart carts = LoyaltyProgManager.CreateSailPlayCart(cheque);
            LoyaltyCardInfo loyaltyCardInfo = base.GetLoyaltyCardInfo(false);
            CalcResult calcResult = this._api.CalcMaxDiscount(carts, (int)cheque.SUMM);
            decimal num = (nullable == null ? new decimal(0, 0, 0, false, 1) : nullable.DiscountSum);
            num = Math.Min(num, calcResult.Cart.TotalDiscountPointsMax);
            decimal totalPrice = calcResult.Cart.TotalPrice;
            totalPrice = Math.Floor(totalPrice--);
            num = Math.Min(totalPrice, num);
            if (num > new decimal(0))
            {
                carts = this.Distribute(calcResult, num, cheque);
                calcResult = this._api.CalcMaxDiscount(carts, (int)cheque.SUMM);
            }
            this._lastCartId = new long?(calcResult.Cart.Id);
            int num1 = ((IEnumerable<CartPositionResult>)calcResult.Cart.Positions).Sum<CartPositionResult>((CartPositionResult p) => p.DiscountPointsMax);
            nullable.DiscountSumMax = new decimal?(((IEnumerable<CartPositionResult>)calcResult.Cart.Positions).Sum<CartPositionResult>((CartPositionResult p) => p.DiscountPoints));
            if (nullable != null)
            {
                this.AddSailPlayExtraDiscounts(calcResult, cheque, nullable);
            }
            decimal num2 = Math.Min(num1, loyaltyCardInfo.Balance);
            return Math.Min(num2, totalPrice);

        }

        private void AddSailPlayExtraDiscounts(CalcResult calcResult, CHEQUE cheque, LoyaltyCard card)
    {
      card.ExtraDiscounts.Clear();
      cheque.CHEQUE_ITEMS.ForEach((Action<CHEQUE_ITEM>) (ch => ch.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.TYPE == "SP_EX"))));
      Dictionary<int, CHEQUE_ITEM> dictionary = cheque.CHEQUE_ITEMS.ToDictionary<CHEQUE_ITEM, int>((Func<CHEQUE_ITEM, int>) (c => Math.Abs(c.ID_LOT_GLOBAL.GetHashCode())));
      Dictionary<string, string> actions = ((IEnumerable<MarketingActionResult>) calcResult.MarketingActionsApplied).ToDictionary<MarketingActionResult, string, string>((Func<MarketingActionResult, string>) (a => a.Alias), (Func<MarketingActionResult, string>) (a => a.Name));
      if (card is ILoyaltyMessageList loyaltyMessageList)
      {
        loyaltyMessageList.Clear();
        foreach (MarketingActionResult possibleMarketingAction in calcResult.PossibleMarketingActions)
        {
          if (possibleMarketingAction.ServiceMessage != null && loyaltyMessageList != null)
          {
            LoyaltyMessage message = new LoyaltyMessage(LoyaltyMessageType.Service, possibleMarketingAction.ServiceMessage);
            loyaltyMessageList.Add((ILoyaltyMessage) message);
          }
        }
      }
      foreach (CartPositionResult position in calcResult.Cart.Positions)
      {
        CHEQUE_ITEM chequeItem;
        if (position.NewPrice.HasValue && position.MarketingActions != null && position.MarketingActions.Length > 0 && dictionary.TryGetValue(position.Num, out chequeItem))
        {
          DISCOUNT_VALUE_INFO discountValueInfo = new DISCOUNT_VALUE_INFO()
          {
            BARCODE = card.BARCODE,
            ID_DISCOUNT2_GLOBAL = ARM_DISCOUNT2_PROGRAM.SailPlayDiscountGUID,
            DISCOUNT2_NAME = string.Join(";", ((IEnumerable<string>) position.MarketingActions).Select<string, string>((Func<string, string>) (a => actions[a]))),
            TYPE = "SP_EX",
            ID_LOT_GLOBAL = chequeItem.ID_LOT_GLOBAL,
            VALUE = position.Price - position.NewPrice.Value
          };
          card.ExtraDiscounts.Add(discountValueInfo);
        }
      }
      cheque.CalculateFields();
    }

    private void ClearSelfAndRoundingDiscounts(CHEQUE cheque)
    {
      cheque.CHEQUE_ITEMS.ForEach((Action<CHEQUE_ITEM>) (ch =>
      {
        ch.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.ID_DISCOUNT2_PROGRAM_GLOBAL == ARM_DISCOUNT2_PROGRAM.SailPlayDiscountGUID));
        ch.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.ID_DISCOUNT2_PROGRAM_GLOBAL == ARM_DISCOUNT2_PROGRAM.RoundingDiscountGUID));
      }));
      (cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is SailPlayCard)) as LoyaltyCard).ExtraDiscounts.RemoveAll((Predicate<DISCOUNT_VALUE_INFO>) (e => e.TYPE.Equals("SP_EX")));
      cheque.CalculateFields();
    }

    private Cart Distribute(CalcResult calc, Decimal discountPoints, CHEQUE cheque)
    {
      Decimal discountPointsMax = (Decimal) calc.Cart.TotalDiscountPointsMax;
      Cart cart = new Cart();
      Dictionary<int, CHEQUE_ITEM> dictionary = cheque.CHEQUE_ITEMS.ToDictionary<CHEQUE_ITEM, int>((Func<CHEQUE_ITEM, int>) (c => Math.Abs(c.ID_LOT_GLOBAL.GetHashCode())));
      IOrderedEnumerable<CartPositionResult> source = ((IEnumerable<CartPositionResult>) calc.Cart.Positions).OrderByDescending<CartPositionResult, int>((Func<CartPositionResult, int>) (p => p.DiscountPointsMax));
      foreach (CartPositionResult cartPositionResult in (IEnumerable<CartPositionResult>) source)
      {
        Decimal num = 0M;
        if (discountPoints > 0M)
        {
          CHEQUE_ITEM chequeItem = dictionary[cartPositionResult.Num];
          num = Math.Min(Math.Min(Math.Min(Math.Ceiling((Decimal) cartPositionResult.DiscountPointsMax * discountPoints / discountPointsMax), discountPoints), (Decimal) cartPositionResult.DiscountPointsMax), Math.Floor(cartPositionResult.NewPrice ?? cartPositionResult.Price));
          discountPoints -= num;
          discountPointsMax -= (Decimal) cartPositionResult.DiscountPointsMax;
        }
        PurchaseItem purchaseItem = new PurchaseItem()
        {
          Sku = cartPositionResult.Product.Sku,
          Price = cartPositionResult.Price,
          Qantity = cartPositionResult.Quantity,
          DiscountPoints = (int) num
        };
        cart.AddPurchase(purchaseItem, cartPositionResult.Num);
      }
      CartPositionResult cartPositionResult1 = source.First<CartPositionResult>();
      CHEQUE_ITEM chequeItem1 = dictionary[cartPositionResult1.Num];
      if (discountPoints > 0M)
      {
        if (!(discountPoints <= Math.Floor(cartPositionResult1.NewPrice ?? cartPositionResult1.Price) - (Decimal) cartPositionResult1.DiscountPoints))
          throw new NotImplementedException("Не удалось корректно распределить бальную скидку Моё здоровье");
        source.Last<CartPositionResult>().DiscountPoints += (int) discountPoints;
      }
      return cart;
    }

    private Decimal CalculateChequeItemDiscountWithoutRoundingDiscount(CHEQUE_ITEM item) => item.Discount2MakeItemList.Where<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, bool>) (dmi => dmi.ID_DISCOUNT2_PROGRAM_GLOBAL != ARM_DISCOUNT2_PROGRAM.SailPlayDiscountGUID && dmi.ID_DISCOUNT2_PROGRAM_GLOBAL != ARM_DISCOUNT2_PROGRAM.RoundingDiscountGUID)).Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (d => d.AMOUNT));

    protected override void DoRollback(out string slipCheque)
    {
      slipCheque = string.Empty;
      ARMLogger.Trace(string.Format("SailPlay: DoRollback"));
      if (!this.lastChequeIdGlobal.HasValue)
        return;
      ARMLogger.Info(string.Format("SailPlay: отмена списания/начисления  по чеку {0}", (object) this.lastChequeIdGlobal.Value));
      if (this._api.PurchaseDelete(this.lastChequeIdGlobal.Value).IsOk)
        slipCheque = "SailPlay: Списание/начисление балов отменено";
      BusinessLogicEvents.Instance.OnRollbackChequeTransaction((object) this, new ChequeTransactionEvent(this.lastChequeIdGlobal.Value, this.GetType().Name, SailPlayLoyaltyProgram._chequeOperTypeDebit));
      BusinessLogicEvents.Instance.OnRollbackChequeTransaction((object) this, new ChequeTransactionEvent(this.lastChequeIdGlobal.Value, this.GetType().Name, SailPlayLoyaltyProgram._chequeOperTypeCharge));
      this.lastChequeIdGlobal = new Guid?();
      ARMLogger.Info(slipCheque);
    }

    private string GetClientPhone()
    {
      LoyaltyCardInfo loyaltyCardInfo = this.GetLoyaltyCardInfo(false);
      string clientPhone = loyaltyCardInfo.ClientPhone;
      if (clientPhone == null && loyaltyCardInfo.ClientIdType == PublicIdType.Phone)
        clientPhone = loyaltyCardInfo.CardNumber;
      return clientPhone;
    }

    public override bool RequestCodeConfirmation(CHEQUE cheque)
    {
      bool flag = false;
      this.codeConfirmation = (string) null;
      this.SuccessCodeConfirmation = false;
      string clientPhone = this.GetClientPhone();
      try
      {
        if (clientPhone != null)
        {
          this.Log("SailPlay: Запрашиваем код подтверждения по SMS");
          SmsCodeResult smsCodeResult = this._api.SmsCode(clientPhone, SailPlayLoyaltyProgram.Params.SmsDebitTemplate);
          this.codeConfirmation = smsCodeResult != null ? smsCodeResult.Code : string.Empty;
          if (!string.IsNullOrEmpty(this.codeConfirmation))
          {
            this.Log("SailPlay: Код подтверждения по SMS успешно получен");
            flag = true;
          }
          else
          {
            string message = "SailPlay: Получен неверный код подтверждения по SMS";
            this.Log(message);
            throw new Exception(message);
          }
        }
      }
      catch (Exception ex)
      {
        this.Log(string.Format("SailPlay: Ошибка получения кода подтверждения по SMS\n{0}", (object) ex.Message));
        throw;
      }
      return flag;
    }

    public override void CodeConfirmation(CHEQUE cheque, string code) => this.SuccessCodeConfirmation = code == this.codeConfirmation;

    public override void RequestPersonalAdditionSales()
    {
      this.PersonalAdditionsSale = (IEnumerable<string>) null;
      if (SailPlayLoyaltyProgram.Params == null || !SailPlayLoyaltyProgram.Params.UsePersonalAdditionSale)
        return;
      this.Log("SailPlay: Запрашиваем список персональных доппродаж");
      string clientPhone = this.GetClientPhone();
      CustomVarsGetResult customVarsGetResult = (CustomVarsGetResult) null;
      try
      {
        customVarsGetResult = this._api.GetCustomVars(clientPhone, "products");
      }
      catch (Exception ex)
      {
        this.Log(string.Format("SailPlay: Ошибка получения списка персональных доппродаж\n{0}", (object) ex.Message));
      }
      if (customVarsGetResult != null && customVarsGetResult.IsOk)
      {
        this.Log("SailPlay: Список персональных доппродаж успешно получен");
        if (string.IsNullOrEmpty(customVarsGetResult.value))
          return;
        IEnumerable<string> source = ((IEnumerable<string>) customVarsGetResult.value.Trim('[', ']').Split(',')).Select<string, string>((Func<string, string>) (v => v.Trim('u', '\'', ' ')));
        if (!source.Any<string>())
          return;
        this.PersonalAdditionsSale = source.ToList<string>().Where<string>((Func<string, bool>) (x => x != null)).Select<string, string>((Func<string, string>) (x => x.ToString()));
      }
      else
        this.Log("SailPlay: Ошибка получения списка персональных доппродаж");
    }
  }
}
