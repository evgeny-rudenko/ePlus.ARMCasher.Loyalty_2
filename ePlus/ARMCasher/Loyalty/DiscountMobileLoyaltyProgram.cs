// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.DiscountMobileLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.DiscountMobile.Forms;
using ePlus.ARMCasher.Loyalty.Xml;
using ePlus.ARMCommon.Config;
using ePlus.ARMUtils;
using ePlus.CommonEx;
using ePlus.Discount2.BusinessObjects;
using ePlus.Discount2.Server;
using ePlus.DiscountMobile.Client;
using ePlus.DiscountMobile.Client.Database;
using ePlus.Loyalty;
using ePlus.MetaData.Client;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ePlus.ARMCasher.Loyalty
{
  internal class DiscountMobileLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private Decimal _dmScorePerSum;
    private Decimal _dmMinPayPercent;
    private DiscountMobileCard _discountCard;
    private bool _isInit;
    private readonly SettingsDatabase _settingsDatabase = new SettingsDatabase();
    private readonly PosSettingsDatabase _posSettingsDatabase = new PosSettingsDatabase();
    private SettingsItem _settingsItem;
    private PosSettingsItem _posSettingsItem;
    private string _clientId;
    private ePlus.ARMCasher.Loyalty.Xml.DiscountMobilePosTokenStatus _discountMobilePosTokenStatus;
    private DiscountMobileLoyalty _discountMobileLoyalty;
    private DiscountMobilePurchaseResponse _discountMobilePurchaseResponse;
    protected ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserResponse DiscountMobileUserResponse;
    private static Guid _id = new Guid("96F37FA8-0CD9-45A4-8DF2-5BACB8158F96");
    private Dictionary<long, DiscountMobileCouponItem> _couponDict;
    private static readonly Logger logger = LogManager.GetLogger("DiscountMobileLogger");
    private DISCOUNT2_MEMBER _discountMember;
    private string _discountMobileCouponStatus;

    public override string Name => "Dinect";

    public override Guid IdGlobal => DiscountMobileLoyaltyProgram._id;

    protected override bool OnIsExplicitDiscount => false;

    protected string CardNumber { get; set; }

    private List<long> Coupons { get; set; }

    public static void LoadGlobalConfigFromDinectSettings()
    {
      DiscountMobileLoyaltyProgram mobileLoyaltyProgram = new DiscountMobileLoyaltyProgram("", "");
    }

    private IEnumerable<DISCOUNT_VALUE_INFO> ApplyDiscountMobile(
      CHEQUE cheque,
      DISCOUNT2_CARD_POLICY sDiscountCard)
    {
      if (!(sDiscountCard is DiscountMobileCard discountMobileCard))
        return (IEnumerable<DISCOUNT_VALUE_INFO>) null;
      if (discountMobileCard.LoyaltyType != LoyaltyType.DiscountMobile)
        return (IEnumerable<DISCOUNT_VALUE_INFO>) null;
      if (discountMobileCard.State == DiscountMobileCard.CardStates.Active)
      {
        DiscountMobilePurchaseResponse purchaseResponse = LoyaltyProgManager.DiscountMobileApplyCoupon(cheque, false, out ILpTransResult _);
        if (purchaseResponse != null)
        {
          Decimal sumDiscount = purchaseResponse.SumDiscount;
          discountMobileCard.SumDiscount = sumDiscount;
          SpecialDiscountParamDecimal discountParamDecimal = new SpecialDiscountParamDecimal();
          discountParamDecimal.Value = discountMobileCard.SumDiscount;
          discountMobileCard.SpecialDiscountParam = new SpecialDiscountParam();
          discountMobileCard.SpecialDiscountParam.Value = (ISpecialDiscountParamValue) discountParamDecimal;
        }
        else
        {
          discountMobileCard.SumDiscount = 0M;
          discountMobileCard.DiscountSum = 0M;
          discountMobileCard.SpecialDiscountParam = (SpecialDiscountParam) null;
          discountMobileCard.ChequeItems.Clear();
        }
      }
      List<DISCOUNT_VALUE_INFO> discountValueInfoList = new List<DISCOUNT_VALUE_INFO>();
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        DISCOUNT_VALUE_INFO discountValueInfo = new DISCOUNT_VALUE_INFO();
        discountValueInfo.ID_DISCOUNT2_GLOBAL = ARM_DISCOUNT2_PROGRAM.DiscountMobileDiscountGUID;
        discountValueInfo.TYPE = this.DiscountType;
        discountValueInfo.BARCODE = sDiscountCard.BARCODE;
        Decimal sumDiscount = discountMobileCard.SumDiscount;
        if (discountMobileCard.ChequeItems.Count > 0)
        {
          long barcode;
          long.TryParse(chequeItem.INTERNAL_BARCODE, out barcode);
          DiscountMobileCard.DiscountItem discountItem = discountMobileCard.ChequeItems.Find((Predicate<DiscountMobileCard.DiscountItem>) (item1 => item1.Id == barcode));
          discountValueInfo.VALUE = discountItem.Price - discountItem.PriceDiscounted;
          Decimal num = sumDiscount - discountValueInfo.VALUE;
        }
        discountValueInfo.ID_LOT_GLOBAL = chequeItem.ID_LOT_GLOBAL;
        discountValueInfoList.Add(discountValueInfo);
      }
      return (IEnumerable<DISCOUNT_VALUE_INFO>) discountValueInfoList;
    }

    public DiscountMobileLoyaltyProgram(string clientId, string cardNumber)
      : base(LoyaltyType.DiscountMobile, clientId, cardNumber, "DISCOUNT_MOBILE")
    {
      DiscountMobileLoyaltyProgram.logger.Info("Инициализация модуля DM.");
      ServicePointManager.Expect100Continue = false;
      this._clientId = clientId;
      this.Coupons = new List<long>();
      this.CardNumber = cardNumber;
      try
      {
        if (AppConfigurator.EnableDiscountMobile)
        {
          if (!this._isInit)
            this.InitInternal();
        }
      }
      catch (Exception ex)
      {
        this._isInit = false;
        int num = (int) UtilsArm.ShowMessageErrorOK("Ошибка инициализации ПЦ." + Environment.NewLine + "Работа с программами лояльности невозможна.");
      }
      long result;
      if (!this.CardNumber.StartsWith("+") || !long.TryParse(this.CardNumber.Substring(1), out result))
        return;
      this.CardNumber = this.DiscountMobileLoadPhone(result);
      if (string.IsNullOrEmpty(this.CardNumber))
        throw new LoyaltyException((ILoyaltyProgram) this, "Пользователь по телефону " + clientId + " не найден в системе Dinnect");
    }

    private void AddCoupon(long couponId)
    {
      if (!this._discountCard.Coupons.Contains(couponId))
        this._discountCard.Coupons.Add(couponId);
      this._discountCard.SumDiscount = 0M;
    }

    private void AddCoupons(IEnumerable<long> list)
    {
      foreach (long num in list)
      {
        if (!this._discountCard.Coupons.Contains(num))
          this._discountCard.Coupons.Add(num);
      }
      this._discountCard.SumDiscount = 0M;
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque) => Math.Min(cheque.SUMM + (this._discountCard == null ? 0M : this._discountCard.SumDiscount), this.GetLoyaltyCardInfo(false).Balance);

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => result = (ILpTransResult) null;

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
    }

    public bool ChequeItemsEquals(CHEQUE_ITEM c, CHEQUE_ITEM cc) => c.ID_LOT_GLOBAL == cc.ID_LOT_GLOBAL && c.QUANTITY == cc.QUANTITY;

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      if (!baseCheque.CHEQUE_ITEMS.TrueForAll((Predicate<CHEQUE_ITEM>) (c => returnCheque.CHEQUE_ITEMS.Find((Predicate<CHEQUE_ITEM>) (rc => this.ChequeItemsEquals(c, rc))) != null)))
      {
        int num = (int) MessageBox.Show("Для программы лояльности Динект частичный возврат невозможен", "Ошибка");
        throw new Exception("Для программы лояльности Динект частичный возврат невозможен");
      }
      Dictionary<Guid, CHEQUE_ITEM> returnedChequeItemList = new Dictionary<Guid, CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem1 in (List<CHEQUE_ITEM>) returnCheque.CHEQUE_ITEMS)
      {
        CHEQUE_ITEM chequeItem2 = (CHEQUE_ITEM) chequeItem1.Clone();
        returnedChequeItemList.Add(chequeItem1.ID_CHEQUE_ITEM_GLOBAL, chequeItem2);
      }
      result = this.RefundDebitDiscountMobile(this._clientId, returnedChequeItemList, baseCheque, returnCheque);
    }

    protected LoyaltyType GetTypeDiscountCard(int clientIdType)
    {
      LoyaltyType typeDiscountCard = LoyaltyType.Unknown;
      if (System.Enum.IsDefined(typeof (LoyaltyType), (object) clientIdType))
        typeDiscountCard = (LoyaltyType) System.Enum.Parse(typeof (LoyaltyType), clientIdType.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      return typeDiscountCard;
    }

    protected override void DoPreOrderCalculation(CHEQUE cheque)
    {
      if (!(cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is DiscountMobileCard)) is DiscountMobileCard sDiscountCard) || sDiscountCard.LoyaltyType != LoyaltyType.DiscountMobile)
        return;
      sDiscountCard.ExtraDiscounts.Clear();
      sDiscountCard.ExtraDiscounts.AddRange(this.ApplyDiscountMobile(cheque, (DISCOUNT2_CARD_POLICY) sDiscountCard));
    }

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      bool submit = true;
      if (!(cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is DiscountMobileCard)) is DiscountMobileCard discountMobileCard) || discountMobileCard.LoyaltyType != LoyaltyType.DiscountMobile)
        throw new Exception("Code, that was depricated");
      this.DiscountMobileApplyCoupon(cheque, submit, out result);
    }

    private new void InitInternal()
    {
      try
      {
        this._settingsItem = this._settingsDatabase.Load();
        this._posSettingsItem = this._posSettingsDatabase.Load(AppConfigManager.IdCashRegister);
        if (this._settingsItem != null)
        {
          if (this._posSettingsItem != null)
            goto label_4;
        }
        throw new Exception("Ошибка при загрузке настроек ПЦ из БД");
      }
      catch (Exception ex)
      {
        DiscountMobileLoyaltyProgram.logger.Error(string.Format("Ошибка при загрузке настроек ПЦ из БД", (object) ex.Message));
        return;
      }
label_4:
      AppConfigurator.EnableDiscountMobile = true;
      AppConfigurator.DiscountMobilePrefix = this._settingsItem.CardPrefix;
      AppConfigurator.DiscountMobileCouponPrefix = this._settingsItem.CouponPrefix;
      if (!AppConfigurator.EnableDiscountMobile)
        return;
      this._isInit = true;
    }

    protected bool IsDiscountMobilePosTokenStatusActive
    {
      get
      {
        this.DiscountMobileCheckPosTokenStatus();
        return this._discountMobilePosTokenStatus.IsActive == "True";
      }
    }

    protected bool SetPaymentSumDiscountMobile(Decimal maxSum, Decimal chequeSum, bool blockForm)
    {
      Decimal num1 = chequeSum * (100M - this._dmMinPayPercent) / 100M;
      if (maxSum > num1)
        maxSum = num1;
      maxSum *= this._dmScorePerSum;
      chequeSum *= this._dmScorePerSum;
      if (this._dmScorePerSum <= 0M)
      {
        int num2 = (int) MessageBox.Show("Неправильно задан курс баллов в конфигурации DiscountMobile.\nСписание баллов невозможно.", "Ошибка");
        return false;
      }
      using (FrmAddPaymentDiscountMobile paymentDiscountMobile = new FrmAddPaymentDiscountMobile())
      {
        paymentDiscountMobile.BlockForm = blockForm;
        paymentDiscountMobile.MaxSum = maxSum;
        paymentDiscountMobile.ChequeSum = chequeSum;
        paymentDiscountMobile.ScorePerSum = this._dmScorePerSum;
        paymentDiscountMobile.Sum = 0M;
        if (paymentDiscountMobile.ShowDialog() != DialogResult.OK)
          return false;
        this._discountCard.SumDiscount += paymentDiscountMobile.Sum / this._dmScorePerSum;
        this._discountCard.BonusDiscount = paymentDiscountMobile.Sum;
        SpecialDiscountParamDecimal discountParamDecimal = new SpecialDiscountParamDecimal();
        discountParamDecimal.Value = this._discountCard.SumDiscount;
        if (this._discountCard.SpecialDiscountParam == null)
          this._discountCard.SpecialDiscountParam = new SpecialDiscountParam();
        this._discountCard.SpecialDiscountParam.Value = (ISpecialDiscountParamValue) discountParamDecimal;
        return true;
      }
    }

    private void AddCommonRequestParams(ref HttpWebRequest req, bool dmToken)
    {
      req.Headers.Add("DM-Authorization", "dmapptoken " + this._settingsItem.AppToken);
      req.UserAgent = "eFarma2.ARMCacher";
      req.KeepAlive = true;
      req.Timeout = 10000;
      req.ReadWriteTimeout = 10000;
      req.Accept = "application/xml";
      if (dmToken)
        req.Headers.Add("Authorization", "dmtoken " + this._posSettingsItem.PosToken);
      if (!this._settingsItem.UseProxy)
        return;
      req.Proxy = (IWebProxy) new WebProxy()
      {
        Address = new Uri(this._settingsItem.ProxyUrl + ":" + this._settingsItem.ProxyPort),
        Credentials = (ICredentials) new NetworkCredential(this._settingsItem.ProxyUser, this._settingsItem.ProxyPass)
      };
      req.ProtocolVersion = HttpVersion.Version10;
    }

    private DiscountMobileCouponItem DiscountMobileLoadCoupon(string cardCode, string url)
    {
      Uri requestUri = new Uri(url);
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      this.AddCommonRequestParams(ref req, true);
      HttpWebResponse response = (HttpWebResponse) req.GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (DiscountMobileCouponList));
      string end = streamReader.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end);
      DiscountMobileCouponList mobileCouponList = (DiscountMobileCouponList) null;
      using (StringReader stringReader = new StringReader(end))
        mobileCouponList = (DiscountMobileCouponList) xmlSerializer.Deserialize((TextReader) stringReader);
      if (mobileCouponList != null && mobileCouponList.CouponList != null && mobileCouponList.CouponList.Items != null && mobileCouponList.CouponList.Items.Count > 0)
      {
        foreach (DiscountMobileCouponItem mobileCouponItem in mobileCouponList.CouponList.Items)
        {
          if (mobileCouponItem.Number == cardCode)
          {
            response.Close();
            return mobileCouponItem;
          }
        }
      }
      response.Close();
      if (mobileCouponList != null && mobileCouponList.Page < mobileCouponList.Pages)
      {
        DiscountMobileCouponItem mobileCouponItem = this.DiscountMobileLoadCoupon(cardCode, mobileCouponList.NextPage);
        if (mobileCouponItem != null)
          return mobileCouponItem;
      }
      return (DiscountMobileCouponItem) null;
    }

    protected string DiscountMobileLoadPhone(long phone)
    {
      Uri requestUri = new Uri(this._settingsItem.Url + "users/?phone=" + (object) phone);
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      this.AddCommonRequestParams(ref req, true);
      HttpWebResponse response = (HttpWebResponse) req.GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList));
      string end = streamReader.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end);
      ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList discountMobileUserList = (ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList) null;
      using (StringReader stringReader = new StringReader(end))
        discountMobileUserList = (ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList) xmlSerializer.Deserialize((TextReader) stringReader);
      if (discountMobileUserList != null && discountMobileUserList.Items != null && discountMobileUserList.Items.Count > 0)
      {
        response.Close();
        return discountMobileUserList.Items[0].Card;
      }
      response.Close();
      return (string) null;
    }

    private bool IsCoupon(string cardCode) => !string.IsNullOrWhiteSpace(AppConfigurator.DiscountMobileCouponPrefix) && cardCode.StartsWith(AppConfigurator.DiscountMobileCouponPrefix);

    protected bool IsCard(string cardCode) => this._settingsItem != null && this._settingsItem.CardPrefix != null && cardCode.StartsWith(this._settingsItem.CardPrefix);

    protected void FindUser(string cardCode, CHEQUE cheque)
    {
      ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList result;
      if (!this.MakeApiRequest<ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList>(!this.IsCoupon(cardCode) ? this._settingsItem.Url + "users/?auto=" + cardCode : this._settingsItem.Url + "users/?coupon=" + cardCode, true, out result) || result.Items.Count != 1)
        return;
      if (this._discountCard == null)
      {
        this._discountCard = cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is DiscountMobileCard)) as DiscountMobileCard;
        if (this._discountCard == null)
          this._discountCard = new DiscountMobileCard();
        this.DiscountMobileCheckCoupons(result.Items[0].CouponsUrl, 1, cheque);
      }
      List<DISCOUNT2_CARD> discounT2CardList = new DISCOUNT2_CARD_BL(MultiServerBL.ServerConnectionString).List(string.Format("BARCODE = '{0}'", (object) cardCode));
      if (discounT2CardList.Count > 0)
        this._discountCard.ID_DISCOUNT2_CARD_GLOBAL = discounT2CardList[0].ID_DISCOUNT2_CARD_GLOBAL;
      else
        this._discountCard.ID_DISCOUNT2_CARD_GLOBAL = Guid.NewGuid();
      this._discountCard.NUMBER = result.Items[0].Card;
      this._discountCard.BARCODE = result.Items[0].Card;
      if (!string.IsNullOrEmpty(result.Items[0].LoyaltyUrl))
        this.DiscountMobileLoadLoyalty(result.Items[0].LoyaltyUrl);
      this._discountCard.DiscountPercent = result.Items[0].Discounts;
      this._discountCard.SumScore = (Decimal) result.Items[0].Bonus;
      this._discountCard.Recived = true;
      this._discountCard.State = DiscountMobileCard.CardStates.Active;
      this._discountCard.MEMBER_FULLNAME = string.Format("{0} {1} {2}", (object) result.Items[0].FirstName, (object) result.Items[0].MiddleName, (object) result.Items[0].LastName);
      if (!string.IsNullOrEmpty(result.Items[0].FirstName))
      {
        this._discountMember = new DISCOUNT2_MEMBER();
        this._discountMember.LASTNAME = result.Items[0].FirstName;
        if (!string.IsNullOrEmpty(result.Items[0].LastName))
          this._discountMember.MIDDLENAME = result.Items[0].LastName.Substring(0, 1).ToUpper() + ".";
        if (!string.IsNullOrEmpty(result.Items[0].MiddleName))
          this._discountMember.FIRSTNAME = result.Items[0].MiddleName.Substring(0, 1).ToUpper() + ".";
        this._discountMember.ID_DISCOUNT2_MEMBER_GLOBAL = Guid.NewGuid();
        this._discountCard.ID_DISCOUNT2_MEMBER_GLOBAL = this._discountMember.ID_DISCOUNT2_MEMBER_GLOBAL;
        this.DiscountMobileLoadUser(result.Items[0].Id);
      }
      if (string.IsNullOrEmpty(result.Items[0].LoyaltyUrl))
        return;
      this.DiscountMobileLoadLoyalty(result.Items[0].LoyaltyUrl);
    }

    private bool MakeApiRequest<T>(string url, bool sendToken, out T result)
    {
      result = default (T);
      Uri requestUri = new Uri(url);
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      this.AddCommonRequestParams(ref req, sendToken);
      HttpWebResponse response;
      try
      {
        response = (HttpWebResponse) req.GetResponse();
      }
      catch (WebException ex)
      {
        if (ex.ToString().Contains("401"))
        {
          int num = (int) MessageBox.Show("Ошибка авторизации, проверьте токен для работы с DiscountMobile.");
          return false;
        }
        throw;
      }
      StreamReader streamReader = new StreamReader(response.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (T));
      string end = streamReader.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end);
      using (StringReader stringReader = new StringReader(end))
        result = (T) xmlSerializer.Deserialize((TextReader) stringReader);
      response.Close();
      return true;
    }

    public bool IsBonusProgram() => this.GetProgramType() == DiscountMobileLoyalty.LoyalityProgramType.Bonus;

    public DiscountMobileLoyalty.LoyalityProgramType GetProgramType() => this._discountMobileLoyalty != null ? this._discountMobileLoyalty.Type : DiscountMobileLoyalty.LoyalityProgramType.Nothing;

    private void DiscountMobileLoadLoyalty(string loyaltyUrl)
    {
      if (!this.MakeApiRequest<DiscountMobileLoyalty>(loyaltyUrl, true, out this._discountMobileLoyalty) || this._discountMobileLoyalty == null)
        return;
      if (this._discountMobileLoyalty.Bonus2Amount != null && this._discountMobileLoyalty.Bonus2Amount.Items != null && this._discountMobileLoyalty.Bonus2Amount.Items.Count > 0)
        this._dmScorePerSum = this._discountMobileLoyalty.Bonus2Amount.Items[0];
      this._dmMinPayPercent = (Decimal) (100 - this._discountMobileLoyalty.MaxPurchasePercentage);
    }

    protected bool DiscountMobileDeleteToken()
    {
      Uri requestUri = new Uri(this._settingsItem.Url + "tokens/" + this._posSettingsItem.PosToken);
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      this.AddCommonRequestParams(ref req, false);
      req.Method = "DELETE";
      try
      {
        if (((HttpWebResponse) req.GetResponse()).StatusCode == HttpStatusCode.NoContent)
        {
          this._discountMobilePosTokenStatus.IsActive = "False";
          return true;
        }
      }
      catch (WebException ex)
      {
        if (ex.ToString().Contains("404"))
        {
          this.DiscountMobileCheckPosTokenStatus();
          return false;
        }
        throw;
      }
      return false;
    }

    protected void DiscountMobileCheckPosTokenStatus()
    {
      if (this.MakeApiRequest<ePlus.ARMCasher.Loyalty.Xml.DiscountMobilePosTokenStatus>(this._settingsItem.Url + "tokens/" + this._posSettingsItem.PosToken, false, out this._discountMobilePosTokenStatus) && this._discountMobilePosTokenStatus.IsActive != "True")
        throw new ApplicationException("Токен DiscountMobile для ККМ не активен.");
    }

    private DiscountMobilePurchaseList DiscountMobileFindPurchases(string chequeId)
    {
      DiscountMobilePurchaseList result;
      if (!this.MakeApiRequest<DiscountMobilePurchaseList>(this._settingsItem.Url + "users/" + (object) this.DiscountMobileUserResponse.Id + "/purchases/?doc_id=" + chequeId, true, out result))
        throw new ApplicationException("Не удалось получить данные от ПЦ.");
      return result;
    }

    private bool DiscountMobileDeletePurchase(string purchaseUrl)
    {
      Uri requestUri = new Uri(purchaseUrl);
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      req.ContentType = "application/x-www-form-urlencoded";
      this.AddCommonRequestParams(ref req, true);
      req.Method = "DELETE";
      HttpWebResponse response = (HttpWebResponse) req.GetResponse();
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        response.Close();
        return true;
      }
      response.Close();
      return false;
    }

    private ILpTransResult RefundDebitDiscountMobile(
      string clientId,
      Dictionary<Guid, CHEQUE_ITEM> returnedChequeItemList,
      CHEQUE baseCheque,
      CHEQUE returnCheque)
    {
      this.FindUser(clientId, baseCheque);
      DiscountMobilePurchaseList purchases = this.DiscountMobileFindPurchases(baseCheque.ID_CHEQUE_GLOBAL.ToString());
      DiscountMobilePurchase discountMobilePurchase = (DiscountMobilePurchase) null;
      if (purchases != null && purchases.Results != null && purchases.Results.Items != null && purchases.Results.Items.Count > 0)
        discountMobilePurchase = purchases.Results.Items[0];
      if (discountMobilePurchase == null)
        return (ILpTransResult) null;
      if (!this.DiscountMobileDeletePurchase(discountMobilePurchase.Url))
        return (ILpTransResult) null;
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog();
      logQueryLog.DATE_REQUEST = DateTime.Now;
      logQueryLog.TYPE = 5;
      logQueryLog.CLIENT_ID = clientId;
      logQueryLog.ID_CHEQUE_GLOBAL = returnCheque.ID_CHEQUE_GLOBAL;
      new PCX_QUERY_LOG_BL().Save(logQueryLog);
      this.CreateAndSavePCXChequeItemList((IEnumerable<CHEQUE_ITEM>) returnCheque.CHEQUE_ITEMS, baseCheque.SUMM + discountMobilePurchase.SumDiscount, discountMobilePurchase.SumBonus, discountMobilePurchase.SumBonus, discountMobilePurchase.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), clientId, true);
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = this.CreatePCXCheque();
      pcxCheque.ID_CHEQUE_GLOBAL = returnCheque.ID_CHEQUE_GLOBAL;
      pcxCheque.SUMM_SCORE = Math.Abs(discountMobilePurchase.SumBonus);
      pcxCheque.SUMM = Math.Abs(discountMobilePurchase.SumBonus);
      pcxCheque.SUMM_MONEY = Math.Abs(discountMobilePurchase.SumBonus);
      pcxCheque.OPER_TYPE = discountMobilePurchase.SumBonus >= 0M ? PCX_CHEQUE_ITEM.operTypeArr[3] : PCX_CHEQUE_ITEM.operTypeArr[2];
      pcxChequeBl.Save(pcxCheque);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Отмена списания баллов");
      stringBuilder.AppendLine("Номер карты Dinect:");
      stringBuilder.AppendLine(clientId);
      stringBuilder.AppendLine("Дата/время:");
      stringBuilder.AppendLine(logQueryLog.DATE_REQUEST.ToString("dd.MM.yy HH:mm:ss"));
      stringBuilder.AppendLine("Транзакция:" + discountMobilePurchase.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.AppendLine("Торг. точка:" + this._settingsItem.ShopDin);
      stringBuilder.AppendLine("Касса:" + discountMobilePurchase.Pos);
      stringBuilder.AppendLine("Начислено:" + (discountMobilePurchase.SumBonus * -1M).ToString((IFormatProvider) CultureInfo.InvariantCulture) + " баллов");
      this.DiscountMobileLoadUser(this.DiscountMobileUserResponse.Id);
      stringBuilder.AppendLine("Баланс:" + this.DiscountMobileUserResponse.Bonus.ToString((IFormatProvider) CultureInfo.InvariantCulture) + " баллов");
      stringBuilder.AppendLine();
      return (ILpTransResult) new LpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this._discountCard.NUMBER, discountMobilePurchase.SumBonus < 0M ? Math.Abs(discountMobilePurchase.SumBonus) : 0M, discountMobilePurchase.SumBonus > 0M ? discountMobilePurchase.SumBonus : 0M, (Decimal) this.DiscountMobileUserResponse.Bonus, "баллов", true, true);
    }

    public string DiscountMobileGetCouponStatus() => this._discountMobileCouponStatus;

    private void ParseChequeItems()
    {
      this._discountCard.ChequeItems.Clear();
      foreach (DiscountMobilePurchaseItem mobilePurchaseItem in this._discountMobilePurchaseResponse.Items.Items)
      {
        DiscountMobileCard.DiscountItem discountItem = new DiscountMobileCard.DiscountItem();
        long result1;
        long.TryParse(mobilePurchaseItem.ItemGtin, out result1);
        discountItem.Id = result1;
        Decimal result2;
        Decimal.TryParse(mobilePurchaseItem.SumTotal, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result2);
        discountItem.Price = result2;
        Decimal result3;
        Decimal.TryParse(mobilePurchaseItem.SumWithDiscount, NumberStyles.Any, (IFormatProvider) CultureInfo.InvariantCulture, out result3);
        discountItem.PriceDiscounted = result3;
        Decimal result4;
        Decimal.TryParse(mobilePurchaseItem.Quantity, out result4);
        discountItem.Quantity = result4;
        this._discountCard.ChequeItems.Add(discountItem);
      }
    }

    private string MakeChequeItemsString(
      CHEQUE cheque,
      ref Decimal sumTotal,
      bool submit,
      bool useGTIN)
    {
      string empty = string.Empty;
      int num = 0;
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        if (!string.IsNullOrEmpty(empty))
          empty += "&";
        empty += string.Format("item_{0}_gtin={1}&item_{0}_q={2}&item_{0}_sum={3}", (object) num, (object) chequeItem.INTERNAL_BARCODE, (object) chequeItem.QUANTITY.ToString("0.000", (IFormatProvider) CultureInfo.InvariantCulture), (object) (chequeItem.PRICE * chequeItem.QUANTITY).ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture));
        sumTotal += chequeItem.PRICE * chequeItem.QUANTITY;
        ++num;
      }
      return empty;
    }

    public DiscountMobilePurchaseResponse DiscountMobileApplyCoupon(
      CHEQUE cheque,
      bool submit,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      if (cheque == null || cheque.CHEQUE_ITEMS == null || cheque.CHEQUE_ITEMS.Count == 0)
        return (DiscountMobilePurchaseResponse) null;
      this.FindUser(this.CardNumber, cheque);
      Uri requestUri = new Uri(this._settingsItem.Url + "users/" + (object) this.DiscountMobileUserResponse.Id + "/purchases/");
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      req.ContentType = "application/x-www-form-urlencoded";
      this.AddCommonRequestParams(ref req, true);
      req.Method = "POST";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("doc_id", cheque.ID_CHEQUE_GLOBAL.ToString());
      string empty = string.Empty;
      foreach (long coupon in this._discountCard.Coupons)
      {
        if (!string.IsNullOrEmpty(empty))
          empty += ",";
        empty += coupon.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      }
      if (!string.IsNullOrEmpty(empty))
        dictionary.Add("coupons", empty);
      Decimal sumTotal = 0M;
      string str1 = this.MakeChequeItemsString(cheque, ref sumTotal, submit, false);
      dictionary.Add("sum_total", sumTotal.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture));
      if (submit)
        dictionary.Add("commit", "true");
      if (this._settingsItem.RedeemAuto)
        dictionary.Add("redeem_auto", "true");
      dictionary.Add("curr_iso_name", "RUB");
      dictionary.Add("curr_iso_code", "643");
      if (this._discountCard.DiscountSum > 0M)
        dictionary.Add("bonus_payment", this._discountCard.DiscountSum.ToString("0.00", (IFormatProvider) CultureInfo.InvariantCulture));
      string str2 = "";
      foreach (string key in dictionary.Keys)
        str2 = str2 + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(dictionary[key]) + "&";
      string s1 = str2.Substring(0, str2.Length - 1);
      if (!string.IsNullOrEmpty(str1))
        s1 = s1 + "&" + str1;
      byte[] bytes = Encoding.ASCII.GetBytes(s1);
      DiscountMobileLoyaltyProgram.logger.Info(string.Format("{0}\nPOST DATA: {1}", (object) requestUri.AbsoluteUri, (object) s1));
      Stream requestStream = req.GetRequestStream();
      requestStream.Write(bytes, 0, bytes.Length);
      requestStream.Close();
      this._discountMobileCouponStatus = string.Empty;
      HttpWebResponse response1;
      try
      {
        response1 = (HttpWebResponse) req.GetResponse();
      }
      catch (WebException ex)
      {
        if (ex.ToString().Contains("400"))
        {
          WebResponse response2 = ex.Response;
          StreamReader streamReader = new StreamReader(response2.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
          XmlSerializer xmlSerializer = new XmlSerializer(typeof (DiscountMobileUtil.DiscountMobileErrorList));
          string end = streamReader.ReadToEnd();
          DiscountMobileLoyaltyProgram.logger.Info(end);
          DiscountMobileUtil.DiscountMobileErrorList discountMobileErrorList = (DiscountMobileUtil.DiscountMobileErrorList) null;
          using (StringReader stringReader = new StringReader(end))
            discountMobileErrorList = (DiscountMobileUtil.DiscountMobileErrorList) xmlSerializer.Deserialize((TextReader) stringReader);
          string str3 = string.Empty;
          if (discountMobileErrorList != null && discountMobileErrorList.Errors != null && discountMobileErrorList.Errors.Errors != null && discountMobileErrorList.Errors.Errors.Count > 0)
          {
            foreach (string error in discountMobileErrorList.Errors.Errors)
              str3 = str3 + error + "\n";
          }
          int num = (int) MessageBox.Show("Процессинговый центр не принял запрос.\n" + str3, "Ошибка расчёта скидки");
          response2.Close();
          this._discountMobileCouponStatus = "(Сертификат не может быть использован для данной покупки.)";
          return (DiscountMobilePurchaseResponse) null;
        }
        if (ex.ToString().Contains("417"))
        {
          int num = (int) MessageBox.Show("Процессинговый центр не принял запрос.\nПрокси-сервер скрыл код ошибки.", "Ошибка расчёта скидки");
          return (DiscountMobilePurchaseResponse) null;
        }
        throw;
      }
      StreamReader streamReader1 = new StreamReader(response1.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer1 = new XmlSerializer(typeof (DiscountMobilePurchaseResponse));
      string end1 = streamReader1.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end1);
      using (StringReader stringReader = new StringReader(end1))
        this._discountMobilePurchaseResponse = (DiscountMobilePurchaseResponse) xmlSerializer1.Deserialize((TextReader) stringReader);
      this.ParseChequeItems();
      response1.Close();
      if (!submit)
        return this._discountMobilePurchaseResponse;
      this.DiscountMobileLoadUser();
      Decimal sumBonus1 = this._discountMobilePurchaseResponse.SumBonus;
      Decimal sumBonus2 = this._discountMobilePurchaseResponse.SumBonus;
      this.CreateAndSavePCXChequeItemList((IEnumerable<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS, cheque.SUMM + this._discountCard.DiscountSum, sumBonus1, sumBonus2, this._discountMobilePurchaseResponse.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), this.DiscountMobileUserResponse.Card.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      PCX_QUERY_LOG logQueryLog = this.CreateLogQueryLog();
      logQueryLog.DATE_REQUEST = DateTime.Now;
      logQueryLog.ID_CHEQUE_GLOBAL = cheque.ID_CHEQUE_GLOBAL;
      logQueryLog.SUMM = Math.Abs(this._discountMobilePurchaseResponse.SumBonus);
      logQueryLog.TYPE = 2;
      logQueryLog.STATE = 4;
      logQueryLog.TRANSACTION_TERMINAL = this._discountMobilePurchaseResponse.Pos;
      logQueryLog.TRANSACTION_LOCATION = this._discountMobilePurchaseResponse.Pos;
      logQueryLog.CLIENT_ID = this._discountCard.BARCODE;
      new PCX_QUERY_LOG_BL().Save(logQueryLog);
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = this.CreatePCXCheque();
      pcxCheque.ID_CHEQUE_GLOBAL = cheque.ID_CHEQUE_GLOBAL;
      pcxCheque.SUMM_SCORE = Math.Abs(this._discountMobilePurchaseResponse.SumBonus);
      pcxCheque.SUMM = Math.Abs(this._discountMobilePurchaseResponse.SumBonus);
      pcxCheque.SUMM_MONEY = 0M;
      pcxCheque.OPER_TYPE = this._discountMobilePurchaseResponse.SumBonus >= 0M ? PCX_CHEQUE_ITEM.operTypeArr[1] : PCX_CHEQUE_ITEM.operTypeArr[0];
      pcxChequeBl.Save(pcxCheque);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("Номер карты Dinect:");
      stringBuilder.AppendLine(this._discountCard.NUMBER);
      if (!string.IsNullOrEmpty(this._discountMobilePurchaseResponse.Coupons))
      {
        stringBuilder.AppendLine("Автоматически применены купоны:");
        string coupons = this._discountMobilePurchaseResponse.Coupons;
        char[] chArray = new char[1]{ ',' };
        foreach (string s2 in coupons.Split(chArray))
        {
          long result1;
          long.TryParse(s2, out result1);
          if (this._couponDict.ContainsKey(result1))
          {
            DiscountMobileCouponItem mobileCouponItem = this._couponDict[result1];
            stringBuilder.AppendLine(this._settingsItem.CouponPrefix + mobileCouponItem.Number);
          }
        }
      }
      if (this._discountCard.Coupons.Count > 0)
      {
        stringBuilder.AppendLine("Использованы купоны:");
        foreach (long coupon in this._discountCard.Coupons)
        {
          if (this._couponDict.ContainsKey(coupon))
          {
            DiscountMobileCouponItem mobileCouponItem = this._couponDict[coupon];
            stringBuilder.AppendLine(this._settingsItem.CouponPrefix + mobileCouponItem.Number);
          }
        }
      }
      result = (ILpTransResult) new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this._discountCard.NUMBER, this._discountMobilePurchaseResponse.SumBonus > 0M ? this._discountMobilePurchaseResponse.SumBonus : 0M, this._discountMobilePurchaseResponse.SumBonus < 0M ? this._discountMobilePurchaseResponse.SumBonus * -1M : 0M, (Decimal) this.DiscountMobileUserResponse.Bonus, "баллов", false, true);
      return (DiscountMobilePurchaseResponse) null;
    }

    protected PCX_QUERY_LOG CreateLogQueryLog()
    {
      PCX_QUERY_LOG logQueryLog = new PCX_QUERY_LOG();
      logQueryLog.ID_USER_GLOBAL = SecurityContextEx.USER_GUID;
      logQueryLog.ID_QUERY_GLOBAL = Guid.NewGuid();
      logQueryLog.STATE = 1;
      logQueryLog.ID_CASH_REGISTER = AppConfigManager.IdCashRegister;
      logQueryLog.CLIENT_ID_TYPE = 8;
      if (this._discountCard != null)
        logQueryLog.CLIENT_ID = this._discountCard.BARCODE;
      return logQueryLog;
    }

    protected PCX_CHEQUE CreatePCXCheque() => new PCX_CHEQUE()
    {
      CLIENT_ID = this._discountCard.BARCODE,
      CLIENT_ID_TYPE = 8,
      SUMM = 0M,
      SUMM_MONEY = 0M,
      SCORE = 0M,
      SUMM_SCORE = 0M,
      PARTNER_ID = string.Empty,
      LOCATION = string.Empty,
      TERMINAL = string.Empty
    };

    private void AuthPointsDiscountMobile(
      CHEQUE cheque,
      PCX_QUERY_LOG logRecord,
      OperTypeEnum operType)
    {
      Uri requestUri = new Uri(this._settingsItem.Url + "users/" + (object) this.DiscountMobileUserResponse.Id + "/purchases/");
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      req.ContentType = "application/x-www-form-urlencoded";
      this.AddCommonRequestParams(ref req, true);
      req.Method = "POST";
      req.ContentType = "application/x-www-form-urlencoded";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      dictionary.Add("doc_id", cheque.ID_CHEQUE_GLOBAL.ToString());
      dictionary.Add("sum_total", cheque.SumWithoutDiscount.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      Decimal num = cheque.SUM_DISCOUNT * this._dmScorePerSum;
      if (operType != OperTypeEnum.Charge && this._discountCard.DiscountPercent == 0)
        dictionary.Add("bonus_payment", ((int) num).ToString((IFormatProvider) CultureInfo.InvariantCulture));
      dictionary.Add("commit", "on");
      if (this._settingsItem.RedeemAuto)
        dictionary.Add("redeem_auto", "true");
      string str = "";
      foreach (string key in dictionary.Keys)
        str = str + HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(dictionary[key]) + "&";
      string s = str.Substring(0, str.Length - 1);
      byte[] bytes = Encoding.ASCII.GetBytes(s);
      Stream requestStream = req.GetRequestStream();
      requestStream.Write(bytes, 0, bytes.Length);
      requestStream.Close();
      DiscountMobileLoyaltyProgram.logger.Info(string.Format("{0}\nPOST DATA: {1}", (object) requestUri.AbsoluteUri, (object) s));
      HttpWebResponse response = (HttpWebResponse) req.GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (DiscountMobilePurchaseResponse));
      string end = streamReader.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end);
      using (StringReader stringReader = new StringReader(end))
        this._discountMobilePurchaseResponse = (DiscountMobilePurchaseResponse) xmlSerializer.Deserialize((TextReader) stringReader);
      this._discountMobilePurchaseResponse = (DiscountMobilePurchaseResponse) xmlSerializer.Deserialize((TextReader) streamReader);
      response.Close();
      this.DiscountMobileLoadUser();
      this.CreateAndSavePCXChequeItemList((IEnumerable<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS, cheque.SUMM + this._discountCard.SumDiscount, 0M, 0M, this._discountMobilePurchaseResponse.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture), this.DiscountMobileUserResponse.Card.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      logRecord.DATE_REQUEST = DateTime.Now;
      logRecord.TYPE = 2;
      logRecord.CLIENT_ID = this._discountCard.BARCODE;
      new PCX_QUERY_LOG_BL().Save(logRecord);
    }

    protected Dictionary<Guid, Decimal> CalculatePCXDiscount(
      Dictionary<Guid, Decimal> itemSumDict,
      Decimal totalSum,
      Decimal pcxPaymentSum)
    {
      Dictionary<Guid, Decimal> pcxDiscount = new Dictionary<Guid, Decimal>();
      IEnumerator<Guid> enumerator = (IEnumerator<Guid>) itemSumDict.Keys.GetEnumerator();
      enumerator.Reset();
      Decimal num1 = 0M;
      for (int index = 0; index < itemSumDict.Count; ++index)
      {
        enumerator.MoveNext();
        Decimal num2;
        if (index == itemSumDict.Count - 1)
        {
          num2 = pcxPaymentSum - num1;
        }
        else
        {
          num2 = UtilsArm.Round(pcxPaymentSum * itemSumDict[enumerator.Current] / totalSum);
          num1 += num2;
        }
        pcxDiscount.Add(enumerator.Current, num2);
      }
      return pcxDiscount;
    }

    private void CreateAndSavePCXChequeItemList(
      IEnumerable<CHEQUE_ITEM> chequeItemList,
      Decimal totalSum,
      Decimal pcxSumMoney,
      Decimal pcxSumScore,
      string transactionId,
      string clientId,
      bool isRefund = false)
    {
      List<CHEQUE_ITEM> chequeItemList1 = new List<CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem in chequeItemList)
        chequeItemList1.Add(chequeItem);
      Dictionary<Guid, Decimal> itemSumDict = new Dictionary<Guid, Decimal>();
      for (int index = 0; index < chequeItemList1.Count; ++index)
      {
        CHEQUE_ITEM chequeItem = chequeItemList1[index];
        Decimal summ = chequeItem.SUMM;
        foreach (DISCOUNT2_MAKE_ITEM discount2MakeItem in chequeItem.Discount2MakeItemList)
        {
          if (discount2MakeItem.TYPE != null)
            summ += discount2MakeItem.AMOUNT;
        }
        itemSumDict.Add(chequeItem.ID_CHEQUE_ITEM_GLOBAL, summ);
      }
      Dictionary<Guid, Decimal> pcxDiscount1 = this.CalculatePCXDiscount(itemSumDict, totalSum, pcxSumMoney);
      Dictionary<Guid, Decimal> pcxDiscount2 = this.CalculatePCXDiscount(itemSumDict, totalSum, pcxSumScore);
      List<PCX_CHEQUE_ITEM> chequeItemList2 = new List<PCX_CHEQUE_ITEM>();
      foreach (CHEQUE_ITEM chequeItem in chequeItemList1)
      {
        PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM();
        pcxChequeItem.TRANSACTION_ID = transactionId;
        pcxChequeItem.CLIENT_ID = clientId;
        pcxChequeItem.CLIENT_ID_TYPE = 8;
        chequeItemList2.Add(pcxChequeItem);
        pcxChequeItem.SUMM_SCORE = Math.Abs(pcxDiscount2[chequeItem.ID_CHEQUE_ITEM_GLOBAL]);
        pcxChequeItem.SUMM = Math.Abs(pcxDiscount1[chequeItem.ID_CHEQUE_ITEM_GLOBAL]);
        pcxChequeItem.ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL;
        pcxChequeItem.OPER_TYPE = isRefund ? (pcxSumMoney >= 0M ? PCX_CHEQUE_ITEM.operTypeArr[3] : PCX_CHEQUE_ITEM.operTypeArr[2]) : (pcxSumMoney >= 0M ? PCX_CHEQUE_ITEM.operTypeArr[1] : PCX_CHEQUE_ITEM.operTypeArr[0]);
      }
      if (chequeItemList2.Count <= 0)
        return;
      new PCX_CHEQUE_ITEM_BL().Save(chequeItemList2);
    }

    private void DiscountMobileLoadUser() => this.DiscountMobileLoadUser(0);

    protected List<DiscountMobileCouponItem> DiscountMobileCheckCoupons(
      string couponsUrl,
      int pageNumber,
      CHEQUE cheque)
    {
      Decimal sumTotal = 0M;
      string str = this.MakeChequeItemsString(cheque, ref sumTotal, false, true);
      Uri requestUri = couponsUrl.Contains("?id=") || couponsUrl.Contains("items=") && couponsUrl.Contains("status=ACTIVE") ? new Uri(couponsUrl) : new Uri(couponsUrl + "?items=" + str + "&status=ACTIVE");
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      req.ContentType = "application/x-www-form-urlencoded";
      this.AddCommonRequestParams(ref req, true);
      req.Method = "GET";
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      HttpWebResponse response;
      try
      {
        response = (HttpWebResponse) req.GetResponse();
      }
      catch (WebException ex)
      {
        DiscountMobileLoyaltyProgram.logger.Error(string.Format("HTTP Exception: {0}", (object) ex.Message));
        return (List<DiscountMobileCouponItem>) null;
      }
      StreamReader streamReader = new StreamReader(response.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (DiscountMobileCouponList));
      string end = streamReader.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end);
      DiscountMobileCouponList mobileCouponList = (DiscountMobileCouponList) null;
      using (StringReader stringReader = new StringReader(end))
        mobileCouponList = (DiscountMobileCouponList) xmlSerializer.Deserialize((TextReader) stringReader);
      response.Close();
      List<DiscountMobileCouponItem> mobileCouponItemList = new List<DiscountMobileCouponItem>();
      foreach (DiscountMobileCouponItem mobileCouponItem in mobileCouponList.CouponList.Items)
        mobileCouponItemList.Add(mobileCouponItem);
      if (pageNumber == 1)
        this._couponDict = new Dictionary<long, DiscountMobileCouponItem>();
      foreach (DiscountMobileCouponItem mobileCouponItem in mobileCouponItemList)
        this._couponDict.Add((long) mobileCouponItem.Id, mobileCouponItem);
      if (mobileCouponList.Page < mobileCouponList.Pages)
        mobileCouponItemList.AddRange((IEnumerable<DiscountMobileCouponItem>) this.DiscountMobileCheckCoupons(mobileCouponList.NextPage, mobileCouponList.Page + 1, cheque));
      return mobileCouponItemList;
    }

    protected override void DoRollback(out string slipCheque) => throw new NotImplementedException();

    protected override void OnInitSettings()
    {
    }

    protected override void OnInitInternal()
    {
    }

    protected override bool DoIsCompatibleTo(Guid discountId) => true;

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      LoyaltyCardInfo cardInfoFromService = new LoyaltyCardInfo();
      cardInfoFromService.ClientId = this.ClientId;
      cardInfoFromService.CardNumber = this.ClientPublicId;
      string str;
      ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList result;
      if (!this.MakeApiRequest<ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserList>(str = this._settingsItem.Url + "users/?auto=" + this.CardNumber, true, out result))
        return cardInfoFromService;
      if (result.Items.Count <= 0)
        throw new Exception("Карта или телефон не найдена");
      cardInfoFromService.Balance = (Decimal) result.Items[0].Bonus;
      this.DiscountMobileLoadLoyalty(result.Items[0].LoyaltyUrl);
      return cardInfoFromService;
    }

    private void DiscountMobileLoadUser(int id)
    {
      Uri requestUri = id == 0 ? new Uri(this._settingsItem.Url + "users/" + (object) this.DiscountMobileUserResponse.Id) : new Uri(this._settingsItem.Url + "users/" + (object) id);
      DiscountMobileLoyaltyProgram.logger.Info(requestUri.AbsoluteUri);
      HttpWebRequest req = (HttpWebRequest) WebRequest.Create(requestUri);
      this.AddCommonRequestParams(ref req, true);
      HttpWebResponse response = (HttpWebResponse) req.GetResponse();
      StreamReader streamReader = new StreamReader(response.GetResponseStream() ?? throw new ApplicationException("Не удалось получить данные от ПЦ."));
      XmlSerializer xmlSerializer = new XmlSerializer(typeof (ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserResponse));
      string end = streamReader.ReadToEnd();
      DiscountMobileLoyaltyProgram.logger.Info(end);
      using (StringReader stringReader = new StringReader(end))
        this.DiscountMobileUserResponse = (ePlus.ARMCasher.Loyalty.Xml.DiscountMobileUserResponse) xmlSerializer.Deserialize((TextReader) stringReader);
      response.Close();
    }
  }
}
