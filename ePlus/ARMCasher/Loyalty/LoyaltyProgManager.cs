// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LoyaltyProgManager
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMBusinessLogic.Caches.StocksCaches;
using ePlus.ARMCacheManager;
using ePlus.ARMCacheManager.Interfaces;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCasher.BusinessLogic.Events;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.AstraZeneca;
using ePlus.ARMCasher.Loyalty.Domestic;
using ePlus.ARMCasher.Loyalty.Forms;
using ePlus.ARMCasher.Loyalty.GoldenMiddle;
using ePlus.ARMCasher.Loyalty.Mindbox;
using ePlus.ARMCasher.Loyalty.Olextra;
using ePlus.ARMCasher.Loyalty.PCX;
using ePlus.ARMCasher.Loyalty.RapidSoft;
using ePlus.ARMCasher.Loyalty.SailPlay;
using ePlus.ARMCasher.Loyalty.Xml;
using ePlus.ARMCommon.Log;
using ePlus.ARMUtils;
using ePlus.Loyalty;
using ePlus.Loyalty.Domestic;
using ePlus.Loyalty.Interfaces;
using ePlus.Loyalty.SailPlay;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty
{
  public static class LoyaltyProgManager
  {
    private static Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> _loyaltySettings;
    private static bool? _isAutoChargeSberBonusOnSale;
    private static Dictionary<LoyaltyType, bool> _isLpCompatibility = new Dictionary<LoyaltyType, bool>();
    private static Dictionary<LoyaltyType, Dictionary<Guid, DataRowItem>> _excludedPrograms = new Dictionary<LoyaltyType, Dictionary<Guid, DataRowItem>>();
    private static LoyaltyDomesticWebApi _domesticWebApi;
    private static string _sailPlayCardPrefix;
    private static SailPlayWebApi _spWebApi;
    private static ePlus.Loyalty.SailPlay.Settings _spSettings;
    private static List<MarketingAction> _marketingActions;
    private static DateTime dtEndMarketingActionDefault = new DateTime(2099, 12, 31);
    private static string pharmacyWalletCardPrefix = (string) null;
    private static Dictionary<string, Guid> m_mindboxCardPrefix = (Dictionary<string, Guid>) null;
    private static List<string> astraZenecaCardPrefix = (List<string>) null;
    private static string olextraCardPrefix = (string) null;

    public static ILoyaltyProgram GetAnonimousLoyaltyProgram(LoyaltyType type, object parameters = null)
    {
      if (!LoyaltyProgManager.LoyaltySettings.ContainsKey(type))
        return (ILoyaltyProgram) null;
      if (type == LoyaltyType.Mindbox)
        return (ILoyaltyProgram) MindboxLoyaltyProgram.Create(parameters);
      throw new NotImplementedException(string.Format("Method: {0}, LoyaltyType: {1}", (object) nameof (GetAnonimousLoyaltyProgram), (object) type));
    }

    public static List<MarketingAction> MarketingActions
    {
      set => LoyaltyProgManager._marketingActions = value;
      get => LoyaltyProgManager._marketingActions;
    }

    static LoyaltyProgManager() => LoyaltyProgManager.SupportPhoneAsIdentity = (IEnumerable<LoyaltyType>) new List<LoyaltyType>()
    {
      LoyaltyType.Domestic,
      LoyaltyType.SailPlay,
      LoyaltyType.Mindbox,
      LoyaltyType.AstraZeneca,
      LoyaltyType.Olextra,
      LoyaltyType.DiscountMobile
    };

    public static void Init() => BusinessLogicEvents.Instance.ZReportPrinting += new EventHandler<ZReportPrintingEventArgs>(LoyaltyProgManager.ZReportPrinting);

    private static void ZReportPrinting(object sender, EventArgs e)
    {
      try
      {
        string posId = DataSyncBL.Instance.CashRegisterSelf.ID_CASH_REGISTER_GLOBAL.ToString();
        new AstraZenecaLoyaltyProgram(string.Empty, posId).ConfirmStoredAzTransactions();
      }
      catch (Exception ex)
      {
        ARMLogger.ErrorException("Ошбика при попытке отправки подтверждения операций Астразенека", ex);
      }
    }

    public static IEnumerable<ILpTransResult> GetMergedLpResults(
      Dictionary<LoyaltyType, ILpTransResult> first,
      Dictionary<LoyaltyType, ILpTransResult> second)
    {
      foreach (LoyaltyType lpType in first.Keys.Union<LoyaltyType>((IEnumerable<LoyaltyType>) second.Keys))
      {
        ILpTransResult lp = !first.ContainsKey(lpType) || !second.ContainsKey(lpType) ? (!first.ContainsKey(lpType) ? second[lpType] : first[lpType]) : first[lpType].Merge(second[lpType]);
        yield return lp;
      }
    }

    public static void MakePreOrderRecalculation(CHEQUE cheque, LoyaltyType type)
    {
      if (!cheque.LoyaltyPrograms.ContainsKey(type))
        return;
      ILoyaltyProgram loyaltyProgram = cheque.LoyaltyPrograms[type];
      if (!loyaltyProgram.IsPreOrderCalculationRequired)
        return;
      loyaltyProgram.PreOrderCalculation(cheque);
    }

    public static Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> LoyaltySettings
    {
      get
      {
        if (LoyaltyProgManager._loyaltySettings == null)
        {
          List<ePlus.Loyalty.LoyaltySettings> source1 = new SettingsModel().List();
          Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> dictionary = new Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]>();
          foreach (IGrouping<LoyaltyType, ePlus.Loyalty.LoyaltySettings> source2 in source1.GroupBy<ePlus.Loyalty.LoyaltySettings, LoyaltyType>((Func<ePlus.Loyalty.LoyaltySettings, LoyaltyType>) (x => x.Type)))
            dictionary.Add(source2.Key, source2.ToArray<ePlus.Loyalty.LoyaltySettings>());
          LoyaltyProgManager._loyaltySettings = dictionary;
        }
        return LoyaltyProgManager._loyaltySettings;
      }
    }

    private static void InitLoyaltyDomesticWebApi()
    {
      if (LoyaltyProgManager._domesticWebApi != null)
        return;
      SettingsModel settingsModel = new SettingsModel();
      ePlus.Loyalty.LoyaltySettings loyaltySettings = settingsModel.Load(LoyaltyType.Domestic, Guid.Empty);
      LoyaltyProgManager._domesticWebApi = new LoyaltyDomesticWebApi(settingsModel.Deserialize<ePlus.Loyalty.Domestic.Settings>(loyaltySettings.SETTINGS, "Settings"), SecurityContextEx.Context.User.Password_hash);
    }

    public static IEnumerable<Card> GetLoyaltyDomesticCards(string phone)
    {
      LoyaltyProgManager.InitLoyaltyDomesticWebApi();
      return LoyaltyProgManager._domesticWebApi.GetClientCardsInfo(new ApiCardListRequest()
      {
        Phone = phone
      });
    }

    public static string GetLoyaltyProgramName(LoyaltyType loyaltyType, LoyaltyCard card = null)
    {
      if (card != null && card is MindboxCard)
        return (card as MindboxCard).loyaltyProgram.Name;
      return LoyaltyProgManager.LoyaltySettings.ContainsKey(loyaltyType) ? ((IEnumerable<ePlus.Loyalty.LoyaltySettings>) LoyaltyProgManager.LoyaltySettings[loyaltyType]).First<ePlus.Loyalty.LoyaltySettings>().NAME : "";
    }

    public static bool IsNeedToBeSentToSailPlay(CHEQUE cheque)
    {
      ARMLogger.Trace("Чтение настроек SailPlay");
      LoyaltyProgManager.LoadSailPlaySettings();
      ARMLogger.Trace(string.Format("Мое здоровье. Автоматическая отправка анонимных чеков: {0}", (object) LoyaltyProgManager._spSettings.IsAutoSendAllCheques));
      return LoyaltyProgManager._spSettings.IsAutoSendAllCheques && !cheque.LoyaltyPrograms.ContainsKey(LoyaltyType.SailPlay);
    }

    public static void SendChequeToSailPlay(CHEQUE cheque)
    {
      try
      {
        if (!LoyaltyProgManager.IsNeedToBeSentToSailPlay(cheque))
          return;
        if (LoyaltyProgManager._spWebApi == null)
          LoyaltyProgManager._spWebApi = new SailPlayWebApi(LoyaltyProgManager._spSettings, PublicIdType.Unknown, (string) null);
        Cart sailPlayCart = LoyaltyProgManager.CreateSailPlayCart(cheque);
        ARMLogger.Trace("Создана корзина для отпавки в SailPlay: " + sailPlayCart.ToString());
        LoyaltyProgManager._spWebApi.PurchasesNewAnonymous(sailPlayCart, cheque.ID_CHEQUE_GLOBAL);
        ARMLogger.Trace("Информация о чеке успешно отправлена в SailPlay");
      }
      catch (Exception ex)
      {
        ARMLogger.Trace("Не удалось отпавить информацию о чеке в SailPlay. Ошибка: " + ex.Message);
      }
    }

    public static bool InitMarketingActions()
    {
      if (LoyaltyProgManager._spSettings == null)
        LoyaltyProgManager.LoadSailPlaySettings();
      if (LoyaltyProgManager._marketingActions == null)
      {
        if (LoyaltyProgManager._spWebApi == null)
          LoyaltyProgManager._spWebApi = new SailPlayWebApi(LoyaltyProgManager._spSettings, PublicIdType.Unknown, (string) null);
        ActionsResult marketingActions = LoyaltyProgManager._spWebApi.GetMarketingActions();
        if (marketingActions != null && marketingActions.MarketingActions != null)
        {
          SettingsModel settingsModel = new SettingsModel();
          ePlus.Loyalty.LoyaltySettings loyaltySettings = settingsModel.Load(LoyaltyType.SailPlay, Guid.Empty);
          if (loyaltySettings != null)
          {
            ePlus.Loyalty.SailPlay.Params param = settingsModel.Deserialize<ePlus.Loyalty.SailPlay.Params>(loyaltySettings.PARAMS, "Params");
            if (param != null)
            {
              IEnumerable<MarketingAction> source = marketingActions.MarketingActions.Where<MarketingAction>((Func<MarketingAction, bool>) (x =>
              {
                int? groupId = x.group_id;
                long idActionGroup = param.IDActionGroup;
                return (long) groupId.GetValueOrDefault() == idActionGroup && groupId.HasValue;
              }));
              if (source.Any<MarketingAction>())
              {
                LoyaltyProgManager._marketingActions = source.ToList<MarketingAction>();
                LoyaltyProgManager._marketingActions.ForEach((Action<MarketingAction>) (a =>
                {
                  if (!a.dt_end.HasValue)
                    a.dt_end = new DateTime?(LoyaltyProgManager.dtEndMarketingActionDefault);
                  if (a.dt_start.HasValue)
                    return;
                  a.dt_start = new DateTime?(DateTime.Now);
                }));
              }
            }
          }
        }
      }
      return LoyaltyProgManager._marketingActions != null && LoyaltyProgManager._marketingActions.Count != 0;
    }

    public static string GetMarketingActionsText(CHEQUE cheque)
    {
      StringBuilder stringBuilder = new StringBuilder();
      try
      {
        if (LoyaltyProgManager._spWebApi == null)
        {
          if (LoyaltyProgManager._spSettings == null)
            LoyaltyProgManager.LoadSailPlaySettings();
          LoyaltyProgManager._spWebApi = new SailPlayWebApi(LoyaltyProgManager._spSettings, PublicIdType.Unknown, (string) null);
        }
        Cart sailPlayCart = LoyaltyProgManager.CreateSailPlayCart(cheque);
        CalcResult calcResult = LoyaltyProgManager._spWebApi.CalcMaxDiscount(sailPlayCart, (int) cheque.SUMM);
        if (LoyaltyProgManager.InitMarketingActions())
        {
          if (calcResult != null)
          {
            if (calcResult.MarketingActionsApplied != null)
            {
              if (((IEnumerable<MarketingActionResult>) calcResult.MarketingActionsApplied).Count<MarketingActionResult>() > 0)
              {
                foreach (MarketingActionResult marketingActionResult in ((IEnumerable<MarketingActionResult>) calcResult.MarketingActionsApplied).ToList<MarketingActionResult>().FindAll((Predicate<MarketingActionResult>) (x => LoyaltyProgManager._marketingActions.Any<MarketingAction>((Func<MarketingAction, bool>) (y => y.alias == x.Alias)))))
                {
                  if (marketingActionResult.ClientMessage != null)
                    stringBuilder.AppendLine(marketingActionResult.ClientMessage);
                }
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        ARMLogger.Trace("Не удалось получить информацию о примененных акциях в SailPlay. Ошибка: " + ex.Message);
      }
      return stringBuilder.ToString();
    }

    public static void SendChequeToSailPlayAsync(CHEQUE cheque)
    {
      if (!LoyaltyProgManager.IsLoyalityProgramEnabled(LoyaltyType.SailPlay))
        return;
      new Task((Action) (() => LoyaltyProgManager.SendChequeToSailPlay(cheque))).Start();
    }

    private static void LoadSailPlaySettings()
    {
      ePlus.Loyalty.LoyaltySettings[] source;
      if (!LoyaltyProgManager.LoyaltySettings.TryGetValue(LoyaltyType.SailPlay, out source))
        throw new ArgumentNullException("Настройки SailPlay не найдены");
      LoyaltyProgManager._spSettings = new SettingsModel().Deserialize<ePlus.Loyalty.SailPlay.Settings>(((IEnumerable<ePlus.Loyalty.LoyaltySettings>) source).First<ePlus.Loyalty.LoyaltySettings>().SETTINGS, "Settings");
    }

    public static Cart CreateSailPlayCart(CHEQUE cheque)
    {
      Cart sailPlayCart = new Cart();
      StocksCache stocksCache = new StocksCache((ICacheManager) new MemoryCacheManager(MemoryCache.Default));
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        STOCK_DETAIL stockDetail = stocksCache.GetStockDetail(chequeItem);
        PurchaseItem purchaseItem = new PurchaseItem()
        {
          Sku = LoyaltyProgManager.GetGoodCode(chequeItem),
          Price = chequeItem.SUMM,
          Qantity = chequeItem.QUANTITY * (Decimal) stockDetail.NUMERATOR / (Decimal) stockDetail.DENOMINATOR,
          PurchasePrice = chequeItem.PRICE_SUP,
          Markup = (float) chequeItem.MARGIN_PCT
        };
        sailPlayCart.AddPurchase(purchaseItem, Math.Abs(chequeItem.ID_LOT_GLOBAL.GetHashCode()));
      }
      return sailPlayCart;
    }

    private static string GetGoodCode(CHEQUE_ITEM chequeItem) => !string.IsNullOrEmpty(chequeItem.CODE) ? chequeItem.CODE : "-100";

    public static bool IsAutoChargeSberBonusOnSale
    {
      get
      {
        if (!LoyaltyProgManager._isAutoChargeSberBonusOnSale.HasValue)
        {
          try
          {
            LoyaltyProgManager._isAutoChargeSberBonusOnSale = new bool?(new SettingsModel().Deserialize<ePlus.Loyalty.Sber.Params>(((IEnumerable<ePlus.Loyalty.LoyaltySettings>) LoyaltyProgManager.LoyaltySettings[LoyaltyType.Sberbank]).First<ePlus.Loyalty.LoyaltySettings>().PARAMS, "Params").AutoCharge);
          }
          catch (Exception ex)
          {
            LoyaltyProgManager._isAutoChargeSberBonusOnSale = new bool?(false);
          }
        }
        return LoyaltyProgManager._isAutoChargeSberBonusOnSale.Value;
      }
    }

    public static IEnumerable<LoyaltyType> SupportPhoneAsIdentity { get; private set; }

    public static string SailPlayCardPrefix
    {
      get
      {
        if (string.IsNullOrEmpty(LoyaltyProgManager._sailPlayCardPrefix))
        {
          ePlus.Loyalty.SailPlay.Settings settings = new SettingsModel().Deserialize<ePlus.Loyalty.SailPlay.Settings>(((IEnumerable<ePlus.Loyalty.LoyaltySettings>) LoyaltyProgManager.LoyaltySettings[LoyaltyType.SailPlay]).First<ePlus.Loyalty.LoyaltySettings>().SETTINGS, "Settings");
          LoyaltyProgManager._sailPlayCardPrefix = string.IsNullOrEmpty(settings.CardPrefix) ? "#null" : settings.CardPrefix;
        }
        return !(LoyaltyProgManager._sailPlayCardPrefix == "#null") ? LoyaltyProgManager._sailPlayCardPrefix : (string) null;
      }
    }

    public static bool IsCompatible(LoyaltyType loyaltyType, Guid discountId)
    {
      if (loyaltyType.IsUsedAsDiscount())
        return true;
      if (!LoyaltyProgManager._isLpCompatibility.ContainsKey(loyaltyType))
      {
        ePlus.Loyalty.LoyaltySettings loyaltySettings = new SettingsModel().Load(loyaltyType, Guid.Empty);
        LoyaltyProgManager._isLpCompatibility.Add(loyaltyType, loyaltySettings.COMPATIBILITY);
        if (loyaltySettings.COMPATIBILITY)
        {
          Dictionary<Guid, DataRowItem> dictionary = new Dictionary<Guid, DataRowItem>();
          dictionary.Add(loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL, (DataRowItem) null);
          LoyaltyProgManager._excludedPrograms.Add(loyaltyType, dictionary);
          foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDCT.ExcludeList)
            dictionary.Add(exclude.Guid, exclude);
          foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesDP.ExcludeList)
            dictionary.Add(exclude.Guid, exclude);
          foreach (DataRowItem exclude in loyaltySettings.CompatibilitiesPL.ExcludeList)
            dictionary.Add(exclude.Guid, exclude);
        }
      }
      return LoyaltyProgManager._isLpCompatibility[loyaltyType] && !LoyaltyProgManager._excludedPrograms[loyaltyType].ContainsKey(discountId);
    }

    public static ILoyaltyProgram GetLoyaltyProgram(
      LoyaltyType loyaltyId,
      Guid loyaltyInstance,
      string clientId,
      string cardNumber = null)
    {
      switch (loyaltyId)
      {
        case LoyaltyType.RapidSoft:
          return LoyaltyProgManager.GetRapidSoftLoyaltyProgram(clientId, cardNumber);
        case LoyaltyType.Svyaznoy:
          return LoyaltyProgManager.GetSvyaznoyLoyaltyProgram(clientId);
        case LoyaltyType.Sberbank:
          return LoyaltyProgManager.GetSberLoyaltyProgram(clientId, cardNumber);
        case LoyaltyType.DiscountMobile:
          return LoyaltyProgManager.GetMobileDiscountLoyaltyProgram(clientId);
        case LoyaltyType.Domestic:
          return LoyaltyProgManager.GetDomesticLoyaltyProgram(clientId);
        case LoyaltyType.PharmacyWallet:
          return LoyaltyProgManager.GetPharmacyWalletLoyaltyProgram(clientId);
        case LoyaltyType.GoldenMiddle:
          return LoyaltyProgManager.GetGoldenMiddleLoyaltyProgram(clientId);
        case LoyaltyType.SailPlay:
          return LoyaltyProgManager.GetSailPlayLoyaltyProgram(clientId);
        case LoyaltyType.Mindbox:
          return LoyaltyProgManager.GetMindboxLoyaltyProgram(clientId, loyaltyInstance);
        case LoyaltyType.AstraZeneca:
          return LoyaltyProgManager.GetAstraZenecaLoyaltyProgram(clientId);
        case LoyaltyType.Olextra:
          return LoyaltyProgManager.GetOlextraLoyaltyProgram(clientId);
        default:
          throw new Exception("Поставщик программы лояльности не найден");
      }
    }

    public static string CardNumberToHash(string number, LoyaltyType loyaltyType = LoyaltyType.Unknown) => LoyaltyProgManager.HexStringFromBytes(new SHA1Managed().ComputeHash(Encoding.ASCII.GetBytes(number))).ToUpper();

    private static string HexStringFromBytes(byte[] bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (byte num in bytes)
      {
        string str = num.ToString("x2");
        stringBuilder.Append(str);
      }
      return stringBuilder.ToString();
    }

    private static ILoyaltyProgram GetDomesticLoyaltyProgram(string clientId) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new DomesticLoyaltyProgram(clientId) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Аптечка\" необходимо указать номер карты");

    private static ILoyaltyProgram GetSvyaznoyLoyaltyProgram(string clientId) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new SvyaznoyLoyaltyProgram(clientId) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Связной\" необходимо указать номер карты");

    private static ILoyaltyProgram GetPharmacyWalletLoyaltyProgram(string clientId) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new PharmacyWalletLoyaltyProgram(clientId) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Аптечный кошелёк\" необходимо указать номер карты");

    private static ILoyaltyProgram GetSberLoyaltyProgram(string clientId, string cardNumber) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new SberbankLoyaltyProgram(clientId, cardNumber) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Cпасибо от Сбербанка\" необходимо указать хэш значение карты");

    private static ILoyaltyProgram GetRapidSoftLoyaltyProgram(string clientId, string cardNumber) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new RapidSoftLoyaltyProgram(clientId, cardNumber) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Bonus Back\" необходимо указать хэш значение карты");

    private static ILoyaltyProgram GetMobileDiscountLoyaltyProgram(string clientId) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new DiscountMobileLoyaltyProgram(clientId, clientId) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Discount Mobile\" необходимо указать номер карты");

    private static ILoyaltyProgram GetGoldenMiddleLoyaltyProgram(string clientId) => !string.IsNullOrEmpty(clientId) ? (ILoyaltyProgram) new GoldenMiddleLoyaltyProgram(clientId) : throw new ArgumentNullException(nameof (clientId), "Для использования программы лояльности \"Золотая Середина\" необходимо указать номер карты");

    private static ILoyaltyProgram GetSailPlayLoyaltyProgram(string publicId) => !string.IsNullOrEmpty(publicId) ? (ILoyaltyProgram) new SailPlayLoyaltyProgram(publicId) : throw new ArgumentNullException("clientId", "Для использования программы лояльности \"Мое здоровье\" необходимо указать номер карты");

    private static ILoyaltyProgram GetMindboxLoyaltyProgram(string publicId, Guid loyaltyInstance)
    {
      string contactForMindbox = LoyaltyProgManager.GetPointOfContactForMindbox();
      return (ILoyaltyProgram) MindboxLoyaltyProgram.Create((object) new
      {
        publicId = publicId,
        pointOfContact = contactForMindbox,
        instance = loyaltyInstance
      });
    }

    public static string GetPointOfContactForMindbox()
    {
      string contactForMindbox;
      if (AppConfigurator.IsRiglaLic)
      {
        contactForMindbox = DataSyncBL.Instance.ContractorSelf.A_COD.ToString();
      }
      else
      {
        string farmDataCode = DataSyncBL.Instance.ContractorSelf.FarmDataCode;
        if (string.IsNullOrEmpty(farmDataCode))
          throw new ApplicationException("Не удалось найти адрес точки контакта: вероятно, не заполнены данные ФармаДата. Обмен с MindBox невозможен");
        contactForMindbox = "PA_" + farmDataCode;
      }
      return contactForMindbox;
    }

    private static ILoyaltyProgram GetAstraZenecaLoyaltyProgram(string publicId)
    {
      if (string.IsNullOrEmpty(publicId))
        throw new ArgumentNullException("clientId", "Для использования программы лояльности \"АстраЗенека\" необходимо указать номер карты");
      string posId = DataSyncBL.Instance.CashRegisterSelf.ID_CASH_REGISTER_GLOBAL.ToString();
      return AppConfigurator.IsRiglaLic ? (ILoyaltyProgram) new AstraZenecaLoyaltyProgramRigla(publicId, posId) : (ILoyaltyProgram) new AstraZenecaLoyaltyProgram(publicId, posId);
    }

    private static ILoyaltyProgram GetOlextraLoyaltyProgram(string publicId)
    {
      if (string.IsNullOrEmpty(publicId))
        throw new ArgumentNullException("clientId", "Для использования программы лояльности \"Олекстра\" необходимо указать номер карты");
      string posId = DataSyncBL.Instance.CashRegisterSelf.ID_CASH_REGISTER_GLOBAL.ToString();
      return (ILoyaltyProgram) new OlextraLoyaltyProgram(publicId, posId);
    }

    public static void ShowLoyaltyProgramBalance(
      CardReader cardReader,
      LoyaltyType loyaltyType,
      Guid LoyaltyInstance,
      string clientId = null)
    {
      string cardNumber = string.Empty;
      CustomerCardInfo customerInfo = (CustomerCardInfo) null;
      if (string.IsNullOrEmpty(clientId) && (cardReader == null || !cardReader(out customerInfo)))
        return;
      if (customerInfo != null)
      {
        clientId = customerInfo.ClientId;
        cardNumber = customerInfo.Last4Digit;
      }
      ILoyaltyProgram loyaltyProgram = LoyaltyProgManager.GetLoyaltyProgram(loyaltyType, LoyaltyInstance, clientId, cardNumber);
      using (FrmBallance frmBallance = new FrmBallance())
      {
        frmBallance.Bind(loyaltyProgram.GetLoyaltyCardInfo(), loyaltyProgram.Name);
        int num = (int) frmBallance.ShowDialog();
      }
    }

    public static LoyaltyCard CreateLoyaltyCard(ILoyaltyProgram lp)
    {
      LoyaltyCardInfo loyaltyCardInfo = lp.GetLoyaltyCardInfo();
      LoyaltyCard loyaltyCard = (LoyaltyCard) null;
      switch (lp.LoyaltyType)
      {
        case LoyaltyType.RapidSoft:
          RapidSoftCard rapidSoftCard = new RapidSoftCard();
          rapidSoftCard.ID_DISCOUNT2_CARD_GLOBAL = new Guid("D04FD3C9-82D9-46B1-BDA8-9A728AB5E7C1");
          rapidSoftCard.NUMBER = loyaltyCardInfo.CardNumber;
          rapidSoftCard.BARCODE = loyaltyCardInfo.ClientId;
          rapidSoftCard.RapidSoftName = lp.Name;
          loyaltyCard = (LoyaltyCard) rapidSoftCard;
          break;
        case LoyaltyType.Svyaznoy:
        case LoyaltyType.Sberbank:
        case LoyaltyType.PharmacyWallet:
          PCXDiscount2Card pcxDiscount2Card = new PCXDiscount2Card((int) lp.LoyaltyType);
          pcxDiscount2Card.BARCODE = loyaltyCardInfo.ClientId;
          pcxDiscount2Card.NUMBER = loyaltyCardInfo.CardNumber;
          pcxDiscount2Card.ACCUMULATE_SUM = loyaltyCardInfo.Balance;
          pcxDiscount2Card.SumScore = loyaltyCardInfo.Points;
          pcxDiscount2Card.Recived = true;
          loyaltyCard = (LoyaltyCard) pcxDiscount2Card;
          break;
        case LoyaltyType.DiscountMobile:
          DiscountMobileCard discountMobileCard = new DiscountMobileCard();
          discountMobileCard.NUMBER = loyaltyCardInfo.ClientId;
          discountMobileCard.BARCODE = loyaltyCardInfo.ClientId;
          loyaltyCard = (LoyaltyCard) discountMobileCard;
          break;
        case LoyaltyType.Domestic:
          LoyaltyDomestic loyaltyDomestic = new LoyaltyDomestic();
          loyaltyDomestic.NUMBER = loyaltyCardInfo.CardNumber;
          loyaltyDomestic.BARCODE = loyaltyCardInfo.ClientId;
          loyaltyCard = (LoyaltyCard) loyaltyDomestic;
          break;
        case LoyaltyType.GoldenMiddle:
          LoyaltyGoldenMiddleCard goldenMiddleCard = new LoyaltyGoldenMiddleCard();
          goldenMiddleCard.NUMBER = loyaltyCardInfo.CardNumber;
          goldenMiddleCard.BARCODE = loyaltyCardInfo.ClientId;
          loyaltyCard = (LoyaltyCard) goldenMiddleCard;
          break;
        case LoyaltyType.SailPlay:
          SailPlayCard sailPlayCard = new SailPlayCard();
          sailPlayCard.NUMBER = loyaltyCardInfo.ClientId;
          sailPlayCard.BARCODE = loyaltyCardInfo.ClientId;
          loyaltyCard = (LoyaltyCard) sailPlayCard;
          break;
        case LoyaltyType.Mindbox:
          loyaltyCard = lp.GetLoyaltyCard();
          loyaltyCard.NUMBER = loyaltyCardInfo.ClientId;
          loyaltyCard.BARCODE = loyaltyCardInfo.ClientId;
          break;
        case LoyaltyType.AstraZeneca:
          AstraZenecaCard astraZenecaCard = new AstraZenecaCard();
          astraZenecaCard.NUMBER = loyaltyCardInfo.ClientId;
          astraZenecaCard.BARCODE = loyaltyCardInfo.ClientId;
          loyaltyCard = (LoyaltyCard) astraZenecaCard;
          break;
        case LoyaltyType.Olextra:
          OlextraCard olextraCard = new OlextraCard();
          olextraCard.NUMBER = loyaltyCardInfo.ClientId;
          olextraCard.BARCODE = loyaltyCardInfo.ClientId;
          loyaltyCard = (LoyaltyCard) olextraCard;
          break;
      }
      return loyaltyCard;
    }

    private static bool IsValidEmail(string email)
    {
      string str = email.Trim();
      if (str.EndsWith("."))
        return false;
      try
      {
        return new MailAddress(email).Address == str;
      }
      catch
      {
        return false;
      }
    }

    public static bool SetLoyaltyProgramDebit(LoyaltyProgramDebitArgs args)
    {
      Func<LoyaltyCard, ILoyaltyProgram, bool> func = (Func<LoyaltyCard, ILoyaltyProgram, bool>) ((arg1, arg2) => true);
      ILoyaltyProgram lp = args.DelegateGetContainsLoyaltyProgram(args.LoyaltyType);
      bool flag = false;
      LoyaltyCard loyaltyCard;
      if (lp == null)
      {
        string empty = string.Empty;
        string clientId1 = args.ClientId;
        CustomerCardInfo customerInfo = new CustomerCardInfo()
        {
          ClientId = args.ClientId
        };
        if (string.IsNullOrEmpty(args.ClientId) && !args.DelegateCardReader(out customerInfo))
          return false;
        string last4Digit = customerInfo.Last4Digit;
        string clientId2 = customerInfo.ClientId;
        if (string.IsNullOrEmpty(clientId2))
          clientId2 = "$EmptyForPromocode$";
        lp = LoyaltyProgManager.GetLoyaltyProgram(args.LoyaltyType, args.LoyaltyInstance, clientId2, last4Digit);
        loyaltyCard = args.DelegateCreateLoyaltyCard(lp);
        if (loyaltyCard is ILoyaltyPromocodeList && !string.IsNullOrEmpty(customerInfo.Promocode))
          (loyaltyCard as ILoyaltyPromocodeList).AddPromocode(customerInfo.Promocode);
        if (lp is MindboxLoyaltyProgram)
        {
          MindboxLoyaltyProgram mindboxLoyaltyProgram = (MindboxLoyaltyProgram) lp;
          if (mindboxLoyaltyProgram.SendPersonalRecomendationRequests)
            mindboxLoyaltyProgram.GetClientRecomendations(args.Cheque);
        }
        func = args.DelegateAddLoyaltyCard;
        flag = true;
      }
      else
        loyaltyCard = args.DelegateGetContainsLoyaltyCard(args.LoyaltyType);
      if (!func(loyaltyCard, lp))
        return false;
      LoyaltyCardInfo cardInfo = lp.GetLoyaltyCardInfo();
      if (!AppConfigurator.IsRiglaLic)
      {
        if (!string.IsNullOrWhiteSpace(cardInfo.ClientEmail) && LoyaltyProgManager.IsValidEmail(cardInfo.ClientEmail))
          args.Cheque.SetDigitalChequeInfo(cardInfo.ClientEmail, cardInfo.ClientEmail);
        else if (!string.IsNullOrWhiteSpace(cardInfo.ClientPhone) && cardInfo.ClientPhone.All<char>((Func<char, bool>) (x => char.IsDigit(x) || x == '+')) && cardInfo.ClientPhone.Skip<char>(1).All<char>(new Func<char, bool>(char.IsDigit)))
          args.Cheque.SetDigitalChequeInfo(cardInfo.ClientPhone, cardInfo.ClientEmail);
        else
          args.Cheque.SetDigitalChequeInfo((string) null, (string) null);
      }
      if (args.Cheque.HasPrepaymentItems())
      {
        args.Cheque.CustomerInfo.CardNumber = cardInfo.CardNumber;
        args.Cheque.CustomerInfo.Name = cardInfo.ClientName;
        args.Cheque.CustomerInfo.Phone = cardInfo.ClientPhone;
        loyaltyCard.DiscountSum = 0M;
        return true;
      }
      if (lp.LoyaltyType == LoyaltyType.AstraZeneca || lp.LoyaltyType == LoyaltyType.Olextra)
      {
        loyaltyCard.DiscountSum = 0M;
        return true;
      }
      if (lp.LoyaltyType == LoyaltyType.DiscountMobile && (lp as DiscountMobileLoyaltyProgram).GetProgramType() != DiscountMobileLoyalty.LoyalityProgramType.Bonus)
      {
        loyaltyCard.DiscountSum = 0M;
        return true;
      }
      if (cardInfo.CardStatusId == LoyaltyCardStatus.Limited)
      {
        int num = (int) UtilsArm.ShowMessageInformationOK(string.Format("Статус карты {0} \"Ограничена\". Списание невозможно.", (object) cardInfo.CardNumber));
        loyaltyCard.DiscountSum = 0M;
        return true;
      }
      if (cardInfo.CardStatusId == LoyaltyCardStatus.Blocked && UtilsArm.ShowMessageQuestionYesNo("Карта \"Заблокирована\". Использование для списания/начисления невозможно. Желаете посмотреть подробную информацию по карте?") == DialogResult.No)
      {
        int num = args.DelegateRemoveLoyaltyProgram(lp) ? 1 : 0;
        throw new LoyaltyCardIsBlockedException(lp);
      }
      LoyaltyProgManager.TakePersonalAdditions(lp);
      using (FrmDebit frm = new FrmDebit())
      {
        Decimal num1 = args.DelegateChequeSum() + (args.ResetDiscount ? 0M : loyaltyCard.DiscountSum);
        frm.Bind(lp, loyaltyCard.DiscountSum, args.Cheque);
        frm.EmailEditEvent += (EventHandler) ((sender, e) => LoyaltyProgManager.EmailEditUserSailPlay(cardInfo, frm));
        DialogResult dialogResult = frm.ShowDialog();
        LoyaltyProgManager.UpdateLoyaltyDiscount(frm.DiscountSum, frm.MaxAllowSum);
        if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Cancel && (lp.LoyaltyType == LoyaltyType.Svyaznoy || lp.LoyaltyType == LoyaltyType.PharmacyWallet || lp.LoyaltyType == LoyaltyType.Mindbox))
        {
          loyaltyCard.DiscountSum = frm.DiscountSum;
          return true;
        }
        if (dialogResult == DialogResult.Cancel && args.UseMaxAllowScoresOnCancel)
        {
          Decimal maxSumBonus = lp.CalculateMaxSumBonus(args.Cheque);
          loyaltyCard.DiscountSum = maxSumBonus < loyaltyCard.DiscountSum ? maxSumBonus : loyaltyCard.DiscountSum;
          return true;
        }
        if (flag)
        {
          int num2 = args.DelegateRemoveLoyaltyProgram(lp) ? 1 : 0;
        }
        if (cardInfo.CardStatusId == LoyaltyCardStatus.Blocked)
          throw new LoyaltyCardIsBlockedException(lp);
      }
      return false;
    }

    private static void EmailEditUserSailPlay(LoyaltyCardInfo cardInfo, FrmDebit frm)
    {
      ARMLogger.Trace("Вызван метод редактирования Email пользователя SailPlay");
      try
      {
        FormSailPlayUserRegister view = new FormSailPlayUserRegister();
        view.Owner = (Form) frm;
        view.OnlyEmailEdit = true;
        using (UserRegisterPresenter registerPresenter = new UserRegisterPresenter(view))
        {
          UserInfoResult userInfo = (UserInfoResult) cardInfo.UserInfo;
          userInfo.OnlyEmailEdit = true;
          UserInfoResult userInfoResult = registerPresenter.ShowView((IUserInfo) userInfo);
          if (userInfoResult == null)
            return;
          if (LoyaltyProgManager._spSettings == null)
            LoyaltyProgManager.LoadSailPlaySettings();
          SailPlayWebApi sailPlayWebApi = new SailPlayWebApi(LoyaltyProgManager._spSettings, cardInfo.ClientIdType, cardInfo.ClientId);
          string clientEmail = cardInfo.ClientEmail;
          string email = userInfoResult.EMail;
          UserUpdateResult userUpdateResult;
          if (string.IsNullOrWhiteSpace(clientEmail))
          {
            userUpdateResult = sailPlayWebApi.UserUpdateAddEmail(email, false);
            ARMLogger.Trace("Вызван метод редактирования Email пользователя SailPlay: UserUpdateAddEmail");
          }
          else
          {
            userUpdateResult = sailPlayWebApi.UserUpdateNewEmail(email, false);
            ARMLogger.Trace("Вызван метод редактирования Email пользователя SailPlay: UserUpdateNewEmail");
          }
          if (userUpdateResult.IsOk)
          {
            frm.Email = email;
          }
          else
          {
            int num = (int) UtilsArm.ShowMessageExclamationOK(string.Format("Неуспешное изменение Email пользователя SailPlay. {0}", (object) userUpdateResult.Message));
          }
        }
      }
      catch (Exception ex)
      {
        ARMLogger.ErrorException("Ошибка при выполнении метода редактирования Email пользователя SailPlay", ex);
      }
    }

    private static void frmDebit_EditEmailEvent(object sender, EventArgs e)
    {
    }

    private static void TakePersonalAdditions(ILoyaltyProgram lp)
    {
      lp.RequestPersonalAdditionSales();
      List<string> list = lp.PersonalAdditionsSale != null ? lp.PersonalAdditionsSale.ToList<string>() : (List<string>) null;
      if (list == null)
        return;
      BusinessLogicEvents.Instance.OnTakePersonalAdditions(new TakePersonalAdditionsEventArgs(list));
    }

    private static void UpdateLoyaltyDiscount(Decimal discountSum, Decimal maxAllowSum) => BusinessLogicEvents.Instance.OnUpdateLoyaltyDiscount(new UpdateLoyaltyDiscountEventArgs(discountSum, maxAllowSum));

    public static bool ValidateCountTransactions(
      LoyaltyType loyaltyType,
      ILoyaltyProgram lp,
      LoyaltyCardInfo cardInfo)
    {
      bool flag = true;
      if (loyaltyType == LoyaltyType.Domestic && lp.GetLoyaltyParams() is ePlus.Loyalty.Domestic.Params loyaltyParams)
      {
        if (loyaltyParams.MaxCardTransactionsInDay != 0 && cardInfo.TransactionsCountInDay != 0 && cardInfo.TransactionsCountInDay >= loyaltyParams.MaxCardTransactionsInDay)
        {
          ARMLogger.Trace("Операция начисления ПЛ Аптечка не может быть выполнена. Превышено количество операций по карте в сутки.");
          flag = false;
        }
        else if (loyaltyParams.MaxCardTransactionsInMonth != 0 && cardInfo.TransactionsCountInMonth != 0 && cardInfo.TransactionsCountInMonth >= loyaltyParams.MaxCardTransactionsInMonth)
        {
          ARMLogger.Trace("Операция начисления ПЛ Аптечка не может быть выполнена. Превышено количество операций по карте в месяц.");
          flag = false;
        }
      }
      return flag;
    }

    public static LoyaltyType DetectLoyaltyTypeByBin(string track2)
    {
      ARMLogger.Trace("Определение ПЛ по БИНу карты");
      string numberFromTrack2 = LoyaltyProgManager.GetCardNumberFromTrack2(track2);
      string last4DigitFromTrack2 = LoyaltyProgManager.GetLast4DigitFromTrack2(track2);
      ARMLogger.Trace("Последние 4 цифры номера карты: {0}", (object) last4DigitFromTrack2);
      string hash = LoyaltyProgManager.CardNumberToHash(numberFromTrack2, LoyaltyType.Sberbank);
      List<ePlus.Loyalty.LoyaltySettings> source = new SettingsModel().List();
      if (source.Any<ePlus.Loyalty.LoyaltySettings>((Func<ePlus.Loyalty.LoyaltySettings, bool>) (x => x.Type == LoyaltyType.Sberbank)) && new SberbankLoyaltyProgram(hash, last4DigitFromTrack2).ValidateBin(numberFromTrack2))
        return LoyaltyType.Sberbank;
      return source.Any<ePlus.Loyalty.LoyaltySettings>((Func<ePlus.Loyalty.LoyaltySettings, bool>) (x => x.Type == LoyaltyType.RapidSoft)) ? LoyaltyType.RapidSoft : LoyaltyType.Unknown;
    }

    public static string GetLast4DigitFromTrack2(string track2) => track2.Substring(12, 4);

    public static string GetCardNumberFromTrack2(string track2) => track2.Substring(0, 16);

    public static IDictionary<Guid, Decimal> Distribute(
      IEnumerable<KeyValuePair<Guid, Decimal>> chequeItems,
      Decimal chequeSum,
      Decimal discount,
      bool isDiscountScore = false)
    {
      if (chequeItems == null)
        throw new ArgumentNullException(nameof (chequeItems));
      List<KeyValuePair<Guid, Decimal>> list = chequeItems.OrderBy<KeyValuePair<Guid, Decimal>, Decimal>((Func<KeyValuePair<Guid, Decimal>, Decimal>) (ci => ci.Value)).ToList<KeyValuePair<Guid, Decimal>>();
      if (list.Count == 0)
        throw new ArgumentException("Количество позиций равно нулю");
      if (chequeSum <= 0M)
        throw new ArgumentException("Сумма чека меньше или равна нулю");
      if (discount > chequeSum && !isDiscountScore)
        throw new InvalidLoyaltySumException("Сумма скидки больше суммы чека");
      if (chequeSum != list.Sum<KeyValuePair<Guid, Decimal>>((Func<KeyValuePair<Guid, Decimal>, Decimal>) (item => item.Value)))
        throw new ArgumentException("Сумма чека не равна сумме позиций");
      IDictionary<Guid, Decimal> dictionary = (IDictionary<Guid, Decimal>) new Dictionary<Guid, Decimal>();
      Decimal num1 = 0M;
      int count = list.Count;
      for (int index = 0; index < count; ++index)
      {
        KeyValuePair<Guid, Decimal> keyValuePair = list[index];
        Decimal num2 = count - 1 != index ? Math.Round(discount * keyValuePair.Value / chequeSum, 2) : discount - num1;
        dictionary[keyValuePair.Key] = num2;
        num1 += num2;
      }
      return dictionary;
    }

    public static IDictionary<Guid, Decimal> DistributeInteger(
      IEnumerable<KeyValuePair<Guid, Decimal>> chequeItems,
      Decimal chequeSum,
      Decimal discount,
      bool isDiscountScore = false)
    {
      List<KeyValuePair<Guid, Decimal>> list = chequeItems.OrderBy<KeyValuePair<Guid, Decimal>, Decimal>((Func<KeyValuePair<Guid, Decimal>, Decimal>) (ci => ci.Value)).ToList<KeyValuePair<Guid, Decimal>>();
      IDictionary<Guid, Decimal> dictionary = (IDictionary<Guid, Decimal>) new Dictionary<Guid, Decimal>();
      Decimal num1 = 0M;
      int count = list.Count;
      for (int index = 0; index < count; ++index)
      {
        KeyValuePair<Guid, Decimal> keyValuePair = list[index];
        Decimal num2 = count - 1 != index ? Math.Truncate(Math.Round(discount * keyValuePair.Value / chequeSum, 2)) : discount - num1;
        dictionary[keyValuePair.Key] = num2;
        num1 += num2;
      }
      return dictionary;
    }

    public static bool IsLoyalityProgramEnabled(LoyaltyType lpType)
    {
      Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> loyaltySettings = LoyaltyProgManager.LoyaltySettings;
      return loyaltySettings != null && loyaltySettings.ContainsKey(lpType) && ((IEnumerable<ePlus.Loyalty.LoyaltySettings>) loyaltySettings[lpType]).First<ePlus.Loyalty.LoyaltySettings>().IS_ENABLED;
    }

    public static string GetPublicIdTypeTitle(PublicIdType idType)
    {
      switch (idType)
      {
        case PublicIdType.CardNumber:
          return "Номер карты";
        case PublicIdType.Phone:
          return "Номер телефона";
        case PublicIdType.EMail:
          return "E-mail";
        default:
          return LoyaltyProgManager.GetPublicIdTypeTitle(PublicIdType.CardNumber);
      }
    }

    public static string FormatPublicId(string publicId, PublicIdType publicIdType)
    {
      try
      {
        return publicIdType == PublicIdType.Phone ? string.Format("+{0:# (###) ### ## ##}", (object) long.Parse(publicId)) : publicId;
      }
      catch
      {
        return publicId;
      }
    }

    public static string PharmacyWalletCardPrefix
    {
      get
      {
        if (string.IsNullOrWhiteSpace(LoyaltyProgManager.pharmacyWalletCardPrefix))
        {
          LoyaltyProgManager.pharmacyWalletCardPrefix = "31";
          Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> loyaltySettings = LoyaltyProgManager.LoyaltySettings;
          if (loyaltySettings != null)
          {
            if (loyaltySettings.ContainsKey(LoyaltyType.PharmacyWallet))
            {
              try
              {
                ePlus.Loyalty.PharmacyWallet.Settings settings = new SettingsModel().Deserialize<ePlus.Loyalty.PharmacyWallet.Settings>(((IEnumerable<ePlus.Loyalty.LoyaltySettings>) loyaltySettings[LoyaltyType.PharmacyWallet]).First<ePlus.Loyalty.LoyaltySettings>().SETTINGS, "Settings");
                if (settings != null)
                {
                  if (!string.IsNullOrWhiteSpace(settings.CardPrefix))
                  {
                    LoyaltyProgManager.pharmacyWalletCardPrefix = settings.CardPrefix.Trim();
                    ARMLogger.Info(string.Format("Используем префикс карты из настроек Аптечного Кошелька [CardPrefix={0}]", (object) LoyaltyProgManager.pharmacyWalletCardPrefix));
                  }
                }
              }
              catch (Exception ex)
              {
                ARMLogger.Error("Ошибка чтения параметров Аптечного Кошелька.");
                ARMLogger.Error(ex.ToString());
              }
            }
          }
        }
        return LoyaltyProgManager.pharmacyWalletCardPrefix;
      }
    }

    public static Dictionary<string, Guid> MindboxCardPrefix
    {
      get
      {
        if (LoyaltyProgManager.m_mindboxCardPrefix == null)
        {
          Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> loyaltySettings = LoyaltyProgManager.LoyaltySettings;
          LoyaltyProgManager.m_mindboxCardPrefix = new Dictionary<string, Guid>();
          if (loyaltySettings != null && loyaltySettings.ContainsKey(LoyaltyType.Mindbox))
          {
            foreach (string srcXml in ((IEnumerable<ePlus.Loyalty.LoyaltySettings>) loyaltySettings[LoyaltyType.Mindbox]).Select<ePlus.Loyalty.LoyaltySettings, string>((Func<ePlus.Loyalty.LoyaltySettings, string>) (x => x.PARAMS)))
            {
              try
              {
                ePlus.Loyalty.Mindbox.Params @params = new SettingsModel().Deserialize<ePlus.Loyalty.Mindbox.Params>(srcXml, "Params");
                if (@params != null)
                {
                  if (!string.IsNullOrWhiteSpace(@params.CardPrefix))
                  {
                    foreach (string key in ((IEnumerable<string>) @params.CardPrefix.Split(',')).Select<string, string>((Func<string, string>) (x => x.Trim())))
                    {
                      LoyaltyProgManager.m_mindboxCardPrefix.Add(key, @params.IdContractorGroupGlobal);
                      ARMLogger.Info(string.Format("Используем префикс карты из настроек Mindbox [CardPrefix={0}]", (object) key));
                    }
                  }
                }
              }
              catch (Exception ex)
              {
                ARMLogger.Error("Ошибка чтения настроек Mindbox.");
                ARMLogger.Error(ex.ToString());
              }
            }
          }
        }
        return LoyaltyProgManager.m_mindboxCardPrefix;
      }
    }

    public static List<string> AstraZenecaCardPrefix
    {
      get
      {
        if (LoyaltyProgManager.astraZenecaCardPrefix == null)
        {
          Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> loyaltySettings = LoyaltyProgManager.LoyaltySettings;
          if (loyaltySettings != null)
          {
            if (loyaltySettings.ContainsKey(LoyaltyType.AstraZeneca))
            {
              try
              {
                ePlus.Loyalty.AstraZeneca.Settings settings = new SettingsModel().Deserialize<ePlus.Loyalty.AstraZeneca.Settings>(((IEnumerable<ePlus.Loyalty.LoyaltySettings>) loyaltySettings[LoyaltyType.AstraZeneca]).First<ePlus.Loyalty.LoyaltySettings>().SETTINGS, "Settings");
                if (settings != null && !string.IsNullOrWhiteSpace(settings.CardPrefix))
                {
                  LoyaltyProgManager.astraZenecaCardPrefix = ((IEnumerable<string>) settings.CardPrefix.Trim().Split(',')).Select<string, string>((Func<string, string>) (x => x.Trim())).ToList<string>();
                  ARMLogger.Info(string.Format("Используем префиксы карты из настроек АстраЗенека [CardPrefix={0}]", (object) string.Join(", ", (IEnumerable<string>) LoyaltyProgManager.astraZenecaCardPrefix)));
                }
                else
                  LoyaltyProgManager.astraZenecaCardPrefix = new List<string>();
              }
              catch (Exception ex)
              {
                ARMLogger.Error("Ошибка чтения настроек АстраЗенека.");
                ARMLogger.Error(ex.ToString());
              }
            }
          }
        }
        return LoyaltyProgManager.astraZenecaCardPrefix;
      }
    }

    public static string OlextraCardPrefix
    {
      get
      {
        if (LoyaltyProgManager.olextraCardPrefix == null)
        {
          Dictionary<LoyaltyType, ePlus.Loyalty.LoyaltySettings[]> loyaltySettings = LoyaltyProgManager.LoyaltySettings;
          if (loyaltySettings != null)
          {
            if (loyaltySettings.ContainsKey(LoyaltyType.Olextra))
            {
              try
              {
                ePlus.Loyalty.Olextra.Settings settings = new SettingsModel().Deserialize<ePlus.Loyalty.Olextra.Settings>(((IEnumerable<ePlus.Loyalty.LoyaltySettings>) loyaltySettings[LoyaltyType.Olextra]).First<ePlus.Loyalty.LoyaltySettings>().SETTINGS, "Settings");
                if (settings != null && !string.IsNullOrWhiteSpace(settings.CardPrefix))
                {
                  LoyaltyProgManager.olextraCardPrefix = settings.CardPrefix.Trim();
                  ARMLogger.Info(string.Format("Используем префикс карты из настроек Олекстра [CardPrefix={0}]", (object) LoyaltyProgManager.olextraCardPrefix));
                }
                else
                  LoyaltyProgManager.olextraCardPrefix = string.Empty;
              }
              catch (Exception ex)
              {
                ARMLogger.Error("Ошибка чтения настроек Олекстры.");
                ARMLogger.Error(ex.ToString());
              }
            }
          }
        }
        return LoyaltyProgManager.olextraCardPrefix;
      }
    }

    public static bool IsUsedAsDiscount(this ILoyaltyProgram lp) => lp.LoyaltyType.IsUsedAsDiscount();

    public static bool IsUsedAsDiscount(this LoyaltyType type) => false;

    public static DiscountMobilePurchaseResponse DiscountMobileApplyCoupon(
      CHEQUE cheque,
      bool submit,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      return !cheque.LoyaltyPrograms.ContainsKey(LoyaltyType.DiscountMobile) ? (DiscountMobilePurchaseResponse) null : ((DiscountMobileLoyaltyProgram) cheque.LoyaltyPrograms[LoyaltyType.DiscountMobile]).DiscountMobileApplyCoupon(cheque, submit, out result);
    }

    public static void LoadGlobalConfigFromDinectSettings() => DiscountMobileLoyaltyProgram.LoadGlobalConfigFromDinectSettings();
  }
}
