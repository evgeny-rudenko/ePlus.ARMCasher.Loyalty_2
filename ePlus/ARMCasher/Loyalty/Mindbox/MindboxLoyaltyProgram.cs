// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Mindbox.MindboxLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMBusinessLogic.Caches.StocksCaches;
using ePlus.ARMBusinessLogic.Interfaces;
using ePlus.ARMCacheManager;
using ePlus.ARMCacheManager.Interfaces;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCasher.BusinessLogic.Events;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.Forms;
using ePlus.ARMCasher.Loyalty.Properties;
using ePlus.ARMCommon;
using ePlus.ARMCommon.Log;
using ePlus.Discount2.BusinessObjects;
using ePlus.Interfaces;
using ePlus.Loyalty;
using ePlus.Loyalty.Interfaces;
using ePlus.Loyalty.Mindbox;
using ePlus.Loyalty.Mindbox.Enums;
using ePlus.MetaData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Mindbox
{
  public class MindboxLoyaltyProgram : BaseLoyaltyProgramEx
  {
    private const string IgnoreDiscountId = "IgnoreDiscountId";
    private const string DiscountTypeBalance = "balance";
    private const string DiscountTypeExternal = "externalPromoAction";
    private const int smsRepeateTimeoutMinutes = 0;
    private static readonly Guid m_chequeOperTypeCharge = new Guid("96E48109-532E-4FE7-8E8E-DF7C37248C58");
    private static readonly Guid m_chequeOperTypeDebit = new Guid("839AD96C-BA49-4645-AF78-A8F57C927668");
    private static readonly Guid m_chequeOperTypeRefundCharge = new Guid("94E3AE98-B869-4700-8276-A460D0B6AB5A");
    private static readonly Guid m_chequeOperTypeRefundDebit = new Guid("091FA7EC-568D-42E9-8808-23647FA2C9E3");
    private Settings m_settings;
    protected Params m_params;
    private static LoyaltySettings m_loyaltySettings;
    private static Dictionary<Guid, DataRowItem> ExcludedPrograms = new Dictionary<Guid, DataRowItem>();
    private readonly string m_typePrefix;
    private string[] activeCardStatuses = new string[1]
    {
      "Activated"
    };
    private string[] blockedCardStatuses = new string[1]
    {
      "Blocked"
    };
    private string[] restrictedCardStatuses = new string[3]
    {
      "Issued",
      "NotIssued",
      "Inactive"
    };
    private Dictionary<CHEQUE, Order> m_registeredOrders = new Dictionary<CHEQUE, Order>();
    private Dictionary<int, Decimal> MaxDiscountDictionary = new Dictionary<int, Decimal>();
    private bool m_isAuthenticationCodeSent;
    private DateTime? m_authenticationCodeSentDateTime = new DateTime?();

    private Guid ChequeOperTypeCharge => MindboxLoyaltyProgram.m_chequeOperTypeCharge;

    private Guid ChequeOperTypeDebit => MindboxLoyaltyProgram.m_chequeOperTypeDebit;

    private Guid ChequeOperTypeRefundCharge => MindboxLoyaltyProgram.m_chequeOperTypeRefundCharge;

    private Guid ChequeOperTypeRefundDebit => MindboxLoyaltyProgram.m_chequeOperTypeRefundDebit;

    protected IMindboxWebApi Api { get; private set; }

    private static bool IscompatibilityEnabled { get; set; }

    private static string PointOfContact { get; set; }

    public Guid IdChequeGlobal { get; set; }

    private Customer FullCustomerInfo { get; set; }

    public MindboxLoyaltyProgram(string publicId, string pointOfContact, Guid instance)
      : base(LoyaltyType.Mindbox, publicId, publicId, "MINDBOX_BALANCE")
    {
      this.SendRecvTimeout = 30;
      MindboxLoyaltyProgram.PointOfContact = pointOfContact;
      this.m_typePrefix = "MINDBOX";
      if (this.m_params == null)
        return;
      this.m_params.PointOfContact = pointOfContact;
    }

    public static MindboxLoyaltyProgram Create(object parameters)
    {
      Guid parameter1 = (Guid) MindboxLoyaltyProgram.ExtractParameter(parameters, "instance");
      string publicid = (string) MindboxLoyaltyProgram.ExtractParameter(parameters, "publicId") ?? string.Empty;
      if (publicid == "$EmptyForPromocode$")
        publicid = "";
      CHEQUE parameter2 = MindboxLoyaltyProgram.ExtractParameter(parameters, "cheque") as CHEQUE;
      Guid? nullable = new Guid?();
      if (parameter2 != null)
        nullable = new Guid?(parameter2.ID_CHEQUE_GLOBAL);
      string parameter3 = (string) MindboxLoyaltyProgram.ExtractParameter(parameters, "pointOfContact");
      MindboxLoyaltyProgram mindboxLoyaltyProgram = new MindboxLoyaltyProgram(publicid, parameter3, new MindboxCard(), parameter1);
      mindboxLoyaltyProgram.InitLoyaltySettings(parameter1);
      if (nullable.HasValue)
        mindboxLoyaltyProgram.IdChequeGlobal = nullable.Value;
      mindboxLoyaltyProgram.LoyaltyInstance = parameter1;
      return mindboxLoyaltyProgram;
    }

    private static object ExtractParameter(object parameters, string name)
    {
      Type type = parameters.GetType();
      return !((IEnumerable<PropertyInfo>) type.GetProperties()).Select<PropertyInfo, string>((Func<PropertyInfo, string>) (p => p.Name)).Contains<string>(name) ? (object) null : type.GetProperty(name).GetValue(parameters, (object[]) null);
    }

    public MindboxLoyaltyProgram(
      string publicid,
      string pointOfContact,
      MindboxCard lcard,
      Guid instance)
      : this(publicid, pointOfContact, instance)
    {
      this.loyaltyCard = lcard;
    }

    private string GetResourceString(string name) => this.GetResourceString(name, (string) null);

    private string GetResourceString(string name, string prefix, params object[] pList)
    {
      string resourceString = string.IsNullOrEmpty(prefix) ? name : string.Format("{1}_{0}", (object) name, (object) prefix);
      object obj = Resources.ResourceManager.GetObject(resourceString);
      if (obj != null)
      {
        resourceString = (string) obj;
        if (((IEnumerable<object>) pList).Any<object>())
          resourceString = string.Format(resourceString, pList);
      }
      return resourceString;
    }

    private DiscountCard FindDiscountCard(IEnumerable<DiscountCard> cards)
    {
      if (this.ClientPublicIdType == PublicIdType.CardNumber)
        return cards.FirstOrDefault<DiscountCard>((Func<DiscountCard, bool>) (c => c.Ids.Number.Equals(this.ClientPublicId))) ?? throw new LoyaltyException((ILoyaltyProgram) this, string.Format("Mindbox: Карта {0} не найдена...", (object) this.ClientPublicId));
      DiscountCard discountCard;
      if (this.m_params.CardPrefix != null)
      {
        List<string> list = ((IEnumerable<string>) this.m_params.CardPrefix.Split(',')).Select<string, string>((Func<string, string>) (x => x.Trim())).ToList<string>();
        discountCard = this.FindDiscountCard(cards, list);
      }
      else
        discountCard = this.FindDiscountCard(cards, new List<string>());
      return discountCard;
    }

    private DiscountCard FindDiscountCard(IEnumerable<DiscountCard> cards, List<string> prefix)
    {
      IEnumerable<DiscountCard> source = prefix == null ? cards : cards.Where<DiscountCard>((Func<DiscountCard, bool>) (c => prefix.Any<string>((Func<string, bool>) (x => c.Ids.Number.StartsWith(x)))));
      return ((source.FirstOrDefault<DiscountCard>((Func<DiscountCard, bool>) (c => c.Status.Ids.SystemName.Equals("Activated", StringComparison.InvariantCultureIgnoreCase))) ?? source.FirstOrDefault<DiscountCard>((Func<DiscountCard, bool>) (c => !c.Status.Ids.SystemName.Equals("Blocked", StringComparison.InvariantCultureIgnoreCase)))) ?? source.FirstOrDefault<DiscountCard>()) ?? throw new ApplicationException("У клиента нет ни одной активной карты!");
    }

    private void DoProcess(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
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
        Order mindboxOrderId = this.GetMindboxOrderId(cheque);
        if (this.Api.PaidAll(this.CreatePaidMindboxOrder(cheque, discountSum, mindboxOrderId.Ids.MindboxOrderId)) == null)
          throw new LoyaltyException((ILoyaltyProgram) this, "Не удалось завершить заказ в Mindbox");
        int operationSum = 0;
        LpTransactionData transactionData = new LpTransactionData(cheque.ID_CHEQUE_GLOBAL, guid);
        this.SaveTransaction(operTypeEnum, (Decimal) operationSum, transactionData);
        BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
        if (string.IsNullOrEmpty(this.ClientId))
          return;
        Decimal chargedSum = mindboxOrderId.TotalAcquiredBalanceChange ?? 0M;
        Decimal debitSum = ((IEnumerable<Line>) mindboxOrderId.Lines).Sum<Line>((Func<Line, Decimal>) (l => l.AppliedDiscounts != null ? ((IEnumerable<AppliedDiscount>) l.AppliedDiscounts).Where<AppliedDiscount>((Func<AppliedDiscount, bool>) (d => d.Type.Equals("balance"))).Sum<AppliedDiscount>((Func<AppliedDiscount, Decimal>) (ad => ad.Amount)) : 0M));
        LpTransResult lpTransResult1 = new LpTransResult(cheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, this.GetLoyaltyCardInfo(true).Balance, this.Name);
        lpTransResult1.LpType = this.LoyaltyType;
        lpTransResult1.TransactionId = (mindboxOrderId.Ids.MindboxOrderId ?? 0L).ToString();
        LpTransResult lpTransResult2 = lpTransResult1;
        foreach (Line line1 in mindboxOrderId.Lines)
        {
          Line line = line1;
          CHEQUE_ITEM chequeItem = cheque.CHEQUE_ITEMS.FirstOrDefault<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (i => i.ID_LOT_GLOBAL == (line.IdLotGlobal ?? Guid.Empty)));
          if (chequeItem != null)
          {
            if (line.AcquiredBalanceChanges != null)
            {
              foreach (AcquiredBalanceChange acquiredBalanceChange in line.AcquiredBalanceChanges)
                lpTransResult2.AddDetail(chequeItem.ID_CHEQUE_ITEM_GLOBAL, acquiredBalanceChange.Amount, OperTypeEnum.Charge, acquiredBalanceChange.BalanceType.Ids.SystemName, acquiredBalanceChange.PromoAction.Ids.ExternalId);
            }
            if (line.AppliedDiscounts != null)
            {
              foreach (AppliedDiscount appliedDiscount in line.AppliedDiscounts)
              {
                if (appliedDiscount.Type.Equals("balance", StringComparison.InvariantCultureIgnoreCase))
                  lpTransResult2.AddDetail(chequeItem.ID_CHEQUE_ITEM_GLOBAL, appliedDiscount.Amount, OperTypeEnum.Debit, appliedDiscount.BalanceType.Ids.SystemName, appliedDiscount.PromoAction.Ids.ExternalId);
              }
            }
          }
        }
        result = (ILpTransResult) lpTransResult2;
        this.Log(result.ToSlipCheque());
      }
    }

    private void RegisterOrder(CHEQUE cheque)
    {
      Decimal discountSum = this.GetDiscountSum(cheque);
      Order checkoutMindboxOrder = this.CreateCheckoutMindboxOrder(cheque, discountSum);
      LoyaltyCard loyaltyCard = this.GetLoyaltyCard(cheque);
      Order result;
      if (this.IsAuthenticationRequired(discountSum, cheque))
      {
        checkoutMindboxOrder.AuthenticationCode = loyaltyCard.AuthenticationCode;
        if (string.IsNullOrEmpty(checkoutMindboxOrder.AuthenticationCode))
        {
          using (SmsAuthenticationForm frm = new SmsAuthenticationForm())
          {
            do
            {
              this.SendAuthenticationCode();
            }
            while (this.waitingForm.ShowChildDialod((Form) frm) == DialogResult.Retry);
            checkoutMindboxOrder.AuthenticationCode = frm.Code;
          }
        }
        if (checkoutMindboxOrder.Customer == null || string.IsNullOrEmpty(checkoutMindboxOrder.Customer.MobilePhone))
          throw new MindboxApiExcepion("К заказу не привязан телефон - подтверждение операции невозможно");
        result = this.Api.CheckedOut(checkoutMindboxOrder, true);
        this.loyaltyCard = (MindboxCard) loyaltyCard;
        loyaltyCard.AcquiredBalanceChange = result.TotalAcquiredBalanceChange;
        loyaltyCard.AuthenticationCode = checkoutMindboxOrder.AuthenticationCode;
      }
      else
        result = this.Api.CheckedOut(checkoutMindboxOrder);
      if (this.loyaltyCard != null)
        this.AddMessages(result, (LoyaltyCard) this.loyaltyCard);
      if (this.m_registeredOrders.ContainsKey(cheque))
        this.m_registeredOrders[cheque] = result;
      else
        this.m_registeredOrders.Add(cheque, result);
    }

    private bool IsAuthenticationRequired(Decimal discountSum, CHEQUE cheque)
    {
      if (!this.m_params.CustomOptions.EnablePreOrederRegistrationSmsAuthentication || discountSum == 0M || !cheque.DiscountCardPolicyList.Any<DISCOUNT2_CARD_POLICY>((Func<DISCOUNT2_CARD_POLICY, bool>) (d => d is MindboxCard)))
        return false;
      this.GetLoyaltyCardInfo(false);
      return discountSum > 0M;
    }

    public override int SortOrder => 100;

    protected override void RegisterInDatabase(ILpTransResult result)
    {
      base.RegisterInDatabase(result);
      if (!(result is LpTransResult lpTransResult) || lpTransResult.Details == null || !lpTransResult.Details.Any<PCX_CHEQUE_ITEM>())
        return;
      BaseLoyaltyProgramEx.PcxChequeItemBl.Save(lpTransResult.Details);
    }

    public override bool IsUpdateLoyaltyCardInfoSupported => true;

    protected override bool DoUpdateLoyaltyCardInfo(
      LoyaltyCardInfo currentInfo,
      LoyaltyCardInfo newInfo)
    {
      DiscountCard newDiscountCard1 = new DiscountCard(newInfo.ClientId);
      Customer customer1 = this.GetCustomer();
      GetCustomerResult customer2 = this.Api.GetCustomer(customer1);
      if (customer2.Status != MindboxApiResult.ResultStatus.Success)
        throw new LoyaltyException((ILoyaltyProgram) this, "Потрбитель не найден");
      ProcessingStatusContainter customer3;
      DiscountCard newDiscountCard2;
      if (customer2.DiscountCards == null || customer2.DiscountCards.Length == 0)
      {
        CardRegistrationResult registrationResult = this.AppendCard(customer1, newInfo.ClientId);
        customer3 = registrationResult.Customer;
        newDiscountCard2 = registrationResult.NewDiscountCard;
      }
      else
      {
        DiscountCard oldDiscountCard;
        switch (currentInfo.ClientIdType)
        {
          case PublicIdType.CardNumber:
            oldDiscountCard = new DiscountCard(currentInfo.ClientId);
            break;
          case PublicIdType.Phone:
            oldDiscountCard = new DiscountCard(this.FindDiscountCard((IEnumerable<DiscountCard>) customer2.DiscountCards).Ids.Number);
            break;
          default:
            throw new NotImplementedException();
        }
        CardReplacementResult replacementResult = this.Api.CardReplacement(this.GetCustomer(currentInfo), oldDiscountCard, newDiscountCard1, MindboxLoyaltyProgram.PointOfContact);
        customer3 = replacementResult.Customer;
        newDiscountCard2 = replacementResult.NewDiscountCard;
      }
      if (customer3.ProcessingStatusValue != ProcessingStatusContainter.ProcessingStatus.Found)
        throw new ApplicationException("Потребитель не найден в БД Mindbox!");
      if (newDiscountCard2.ProcessingStatusValue == ProcessingStatus.ProcessingStatusType.Bound)
        return true;
      throw new ApplicationException(this.GetResourceString(newDiscountCard2.ProcessingStatusValue.ToString(), "DiscountCardProcessingStatus"));
    }

    protected override bool OnIsExplicitDiscount => false;

    public override string Name => MindboxLoyaltyProgram.m_loyaltySettings.NAME;

    public override Guid IdGlobal => MindboxLoyaltyProgram.m_loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL;

    protected override bool DoIsCompatibleTo(Guid discountId) => !(discountId == this.IdGlobal) && MindboxLoyaltyProgram.IscompatibilityEnabled && !MindboxLoyaltyProgram.ExcludedPrograms.ContainsKey(discountId);

    protected override void DoCharge(CHEQUE cheque, Decimal discountSum, out ILpTransResult result) => this.DoProcess(cheque, discountSum, out result);

    protected override void DoDebit(CHEQUE cheque, Decimal discountSum, out ILpTransResult result)
    {
      result = (ILpTransResult) null;
      try
      {
        this.RegisterOrder(cheque);
        if (!cheque.LoyaltyPrograms.Any<KeyValuePair<LoyaltyType, ILoyaltyProgram>>((Func<KeyValuePair<LoyaltyType, ILoyaltyProgram>, bool>) (x => x.Key == LoyaltyType.Mindbox)))
          return;
        LoyaltyCardInfo loyaltyCardInfo = cheque.LoyaltyPrograms.First<KeyValuePair<LoyaltyType, ILoyaltyProgram>>((Func<KeyValuePair<LoyaltyType, ILoyaltyProgram>, bool>) (x => x.Key == LoyaltyType.Mindbox)).Value.GetLoyaltyCardInfo();
        if (loyaltyCardInfo.CardStatusId != LoyaltyCardStatus.NotIssued || string.IsNullOrEmpty(loyaltyCardInfo.CardNumber))
          return;
        this.Api.DeployCard(loyaltyCardInfo.CardNumber);
      }
      catch (Exception ex)
      {
        throw new LoyaltyException((ILoyaltyProgram) this, "Произошла ошибка при регистрации заказа в Mindbox", ex);
      }
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      throw new NotImplementedException();
    }

    protected override void DoRefundCharge(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundedCheques,
      out ILpTransResult result)
    {
      result = (ILpTransResult) null;
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      out ILpTransResult result)
    {
      throw new NotImplementedException();
    }

    protected override void DoRefundDebit(
      CHEQUE baseCheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundedCheques,
      out ILpTransResult result)
    {
      baseCheque.CHEQUE_ITEMS.Sum<CHEQUE_ITEM>((Func<CHEQUE_ITEM, Decimal>) (ci => ci.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (mi => !(mi.TYPE == this.DiscountType) ? 0M : mi.AMOUNT))));
      string empty = string.Empty;
      result = (ILpTransResult) null;
      Guid guid = this.ChequeOperTypeRefundDebit;
      BeginChequeTransactionEvent e = new BeginChequeTransactionEvent(returnCheque.ID_CHEQUE_GLOBAL, this.GetType().Name, guid);
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
              Operation refundMindboxOrder = this.CreateRefundMindboxOrder(baseCheque, returnCheque, refundedCheques);
              LoyaltyCardInfo loyaltyCardInfo1 = this.GetLoyaltyCardInfo(this.ClientPublicId, this.ClientPublicIdType);
              this.Api.ReturnedV3(refundMindboxOrder);
              LoyaltyCardInfo loyaltyCardInfo2 = this.GetLoyaltyCardInfo(this.ClientPublicId, this.ClientPublicIdType);
              Decimal balance = loyaltyCardInfo2.Balance;
              Decimal num = loyaltyCardInfo2.Balance - loyaltyCardInfo1.Balance;
              OperTypeEnum operTypeEnum;
              if (num > 0M)
              {
                operTypeEnum = OperTypeEnum.DebitRefund;
              }
              else
              {
                operTypeEnum = OperTypeEnum.ChargeRefund;
                guid = this.ChequeOperTypeRefundCharge;
              }
              this.LogMsg(operTypeEnum, "Информация о возврате успешно отправлена в Mindbox");
              LpTransactionData transactionData = new LpTransactionData(returnCheque.ID_CHEQUE_GLOBAL, guid);
              this.SaveTransaction(operTypeEnum, Math.Abs(num), transactionData);
              BusinessLogicEvents.Instance.OnChequeTransaction((object) this, (ChequeTransactionEvent) e);
              Decimal chargedSum = num > 0M ? num : 0M;
              Decimal debitSum = num < 0M ? -num : 0M;
              StringBuilder stringBuilder = new StringBuilder();
              stringBuilder.Append('"').Append(this.Name).Append('"').AppendLine();
              if (debitSum > 0M)
              {
                stringBuilder.AppendLine("Возврат списания");
                stringBuilder.Append("Начислено: ").Append((int) chargedSum).AppendLine();
              }
              if (chargedSum > 0M)
              {
                stringBuilder.AppendLine("Возврат начисления");
                stringBuilder.Append("Списано: ").Append((int) debitSum).AppendLine();
              }
              stringBuilder.Append("Баланс: ").Append((int) balance).AppendLine();
              stringBuilder.AppendLine(" ");
              stringBuilder.AppendLine(" ");
              this.Log(stringBuilder.ToString());
              ref ILpTransResult local = ref result;
              LpTransResult lpTransResult1 = new LpTransResult(returnCheque.ID_CHEQUE_GLOBAL, this.ClientPublicId, chargedSum, debitSum, balance, string.Empty, true);
              lpTransResult1.LpType = this.LoyaltyType;
              LpTransResult lpTransResult2 = lpTransResult1;
              local = (ILpTransResult) lpTransResult2;
              return;
            }
            break;
        }
        throw new LoyaltyException((ILoyaltyProgram) this, "Невозможно выполнить возврат бонусов при оплате отличной от Наличных или Картой");
      }
    }

    private LoyaltyCardInfo? CardInfo { get; set; }

    private Task GetCardInfoTask { get; set; }

    private LoyaltyCardStatus ConvertStatus(string statusName) => !((IEnumerable<string>) this.activeCardStatuses).Contains<string>(statusName) ? (!((IEnumerable<string>) this.blockedCardStatuses).Contains<string>(statusName) ? LoyaltyCardStatus.Limited : LoyaltyCardStatus.Blocked) : LoyaltyCardStatus.Active;

    protected override LoyaltyCardInfo DoGetLoyaltyCardInfoFromService(Form form)
    {
      if (string.IsNullOrEmpty(this.ClientId))
        return new LoyaltyCardInfo();
      LoyaltyCardInfo? nullable = new LoyaltyCardInfo?();
      try
      {
        nullable = new LoyaltyCardInfo?(this.GetLoyaltyCardInfo(this.ClientPublicId, this.ClientPublicIdType));
      }
      catch (CustomerNotFoundException ex)
      {
        if (this.m_params.CustomOptions.EnableRegistration && this.ClientPublicIdType == PublicIdType.Phone && DialogResult.Yes == MessageBox.Show((IWin32Window) this.waitingForm, string.Format("Покупатель с номером телефона {0} не найден в системе!{1}Зарегистрировать нового покупателя?", (object) this.ClientPublicId, (object) Environment.NewLine), "Регистрация покупателя", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
          nullable = new LoyaltyCardInfo?(this.RegisterCustomer(this.ClientId));
        if (this.ClientPublicIdType == PublicIdType.CardNumber)
        {
          GetCardInfoResult cardInfoByBarcode = this.Api.GetLoyaltyCardInfoByBarcode(this.ClientId);
          if (cardInfoByBarcode.Card.Status == null)
            throw new LoyaltyException((ILoyaltyProgram) this, "Карта с таким номером не найдена в системе");
          if (cardInfoByBarcode.Card != null && cardInfoByBarcode.Card.Status.Ids.SystemName == "NotIssued")
            nullable = new LoyaltyCardInfo?(new LoyaltyCardInfo()
            {
              CardNumber = this.ClientId,
              CardStatusId = LoyaltyCardStatus.NotIssued,
              CardStatus = "Карта не выдана",
              ClientIdType = PublicIdType.CardNumber,
              ClientId = this.ClientId
            });
        }
        if (!nullable.HasValue)
          throw new LoyaltyException((ILoyaltyProgram) this, this.GetResourceString(ex.ProcessingStatus, "CustomerProcessingStatus"), (Exception) ex);
      }
      return nullable.Value;
    }

    protected virtual void GetRecommendations(IEnumerable<string> productCodes)
    {
    }

    private CardRegistrationResult CardRegistration(Customer customer)
    {
      string cardNumber = string.Empty;
      using (FrmScanBarcode frmScanBarcode = new FrmScanBarcode())
      {
        frmScanBarcode.StartPosition = FormStartPosition.CenterScreen;
        frmScanBarcode.Title = "ШК карты:";
        frmScanBarcode.Text = "Замена/восстановление карты";
        int num = (int) frmScanBarcode.ShowDialog((IWin32Window) this.waitingForm);
        if (frmScanBarcode.DialogResult != DialogResult.OK)
          return (CardRegistrationResult) null;
        cardNumber = frmScanBarcode.Barcode;
      }
      return this.AppendCard(customer, cardNumber);
    }

    private CardRegistrationResult AppendCard(Customer customer, string cardNumber) => this.Api.CardRegistration(customer, cardNumber, MindboxLoyaltyProgram.PointOfContact);

    private LoyaltyCardInfo RegisterCustomer(string phone)
    {
      Customer customer = new Customer()
      {
        MobilePhone = phone
      };
      if (this.Api.CustomerOfflineReg(customer, MindboxLoyaltyProgram.PointOfContact).Customer.ProcessingStatusValue != ProcessingStatus.ProcessingStatusType.Created)
        throw new LoyaltyException((ILoyaltyProgram) this, string.Format("Не удалось создать пользователя по номеру телефона: {0}", (object) phone));
      if (DialogResult.Yes == MessageBox.Show((IWin32Window) this.waitingForm, "Покупатель успешно зарегистрирован. Желаете выдать дисконтную карту?", "Карта покупателя", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
      {
        CardRegistrationResult registrationResult = this.CardRegistration(customer);
        if (registrationResult != null)
        {
          int num = (int) MessageBox.Show((IWin32Window) this.waitingForm, this.GetResourceString(registrationResult.NewDiscountCard.ProcessingStatusValue.ToString(), "DiscountCardProcessingStatus"), "Выдача новой карты", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
      }
      return this.GenerateLoyaltyCardInfo(this.Api.GetCustomer(customer).Customer);
    }

    private LoyaltyCardInfo GenerateLoyaltyCardInfo(Customer customer) => new LoyaltyCardInfo()
    {
      ClientName = customer.FirstName,
      ClientId = customer.MobilePhone,
      ClientIdType = PublicIdType.Phone
    };

    private LoyaltyCardInfo GetLoyaltyCardInfo(string id, PublicIdType idType)
    {
      GetCustomerResult getCustomerResult;
      switch (idType)
      {
        case PublicIdType.CardNumber:
          getCustomerResult = this.Api.GetCustomerByCardNubmer(id);
          break;
        case PublicIdType.Phone:
          getCustomerResult = this.Api.GetCustomerByPhone(id);
          break;
        default:
          throw new NotImplementedException();
      }
      LoyaltyCardInfo loyaltyCardInfo = new LoyaltyCardInfo();
      loyaltyCardInfo.ClientName = getCustomerResult.Customer.FirstName;
      loyaltyCardInfo.ClientId = id;
      loyaltyCardInfo.ClientIdType = idType;
      loyaltyCardInfo.ClientPhone = getCustomerResult.Customer.MobilePhone;
      loyaltyCardInfo.ClientEmail = getCustomerResult.Customer.Email;
      loyaltyCardInfo.BalanceDetails = new List<IBalanceInfoRow>();
      loyaltyCardInfo.Balance = 0M;
      if (getCustomerResult.Balances != null && getCustomerResult.Balances.Length > 0)
      {
        loyaltyCardInfo.Balance = ((IEnumerable<Balance>) getCustomerResult.Balances).Sum<Balance>((Func<Balance, Decimal>) (b => b.Available));
        loyaltyCardInfo.BalanceDetails.AddRange((IEnumerable<IBalanceInfoRow>) ((IEnumerable<Balance>) getCustomerResult.Balances).Select<Balance, BalanceInfoRow>((Func<Balance, BalanceInfoRow>) (b => new BalanceInfoRow()
        {
          Amount = b.Available,
          Name = b.Type == null ? string.Empty : b.Type.Name
        })));
      }
      if (getCustomerResult.DiscountCards != null && getCustomerResult.DiscountCards.Length > 0)
      {
        DiscountCard discountCard = this.FindDiscountCard((IEnumerable<DiscountCard>) getCustomerResult.DiscountCards);
        string systemName = discountCard.Status.Ids.SystemName;
        loyaltyCardInfo.CardStatus = this.GetResourceString(systemName, "MindboxCardStatus");
        loyaltyCardInfo.CardNumber = discountCard.Ids.Number;
        loyaltyCardInfo.CardStatusId = this.ConvertStatus(systemName);
      }
      this.loyaltyCard.CustomerExternalId = getCustomerResult.Customer.Ids.MindboxId;
      return loyaltyCardInfo;
    }

    protected override void OnInitInternal()
    {
      if (this.Api != null)
        return;
      this.Api = ApiHelper.GetApi(this.m_settings, this.m_params);
      this.InitCardStatuses(this.m_params.CustomOptions.ActiveCardStatuses, this.m_params.CustomOptions.BlockedCardStatuses, this.m_params.CustomOptions.RestrictedCardStatuses);
    }

    private void InitCardStatuses(string[] active, string[] blocked, string[] restricted)
    {
      string[] strArray1 = active;
      if (strArray1 == null)
        strArray1 = new string[1]{ "Activated" };
      this.activeCardStatuses = strArray1;
      string[] strArray2 = blocked;
      if (strArray2 == null)
        strArray2 = new string[1]{ "Blocked" };
      this.blockedCardStatuses = strArray2;
      string[] strArray3 = restricted;
      if (strArray3 == null)
        strArray3 = new string[3]
        {
          "Issued",
          "NotIssued",
          "Inactive"
        };
      this.restrictedCardStatuses = strArray3;
    }

    protected override void OnInitSettings()
    {
    }

    private void InitLoyaltySettings(Guid instance)
    {
      if (this.m_settings != null)
        return;
      SettingsModel settingsModel = new SettingsModel();
      MindboxLoyaltyProgram.m_loyaltySettings = settingsModel.Load(LoyaltyType.Mindbox, instance);
      this.m_settings = settingsModel.Deserialize<Settings>(MindboxLoyaltyProgram.m_loyaltySettings.SETTINGS, "Settings");
      this.m_params = settingsModel.Deserialize<Params>(MindboxLoyaltyProgram.m_loyaltySettings.PARAMS, "Params");
      if (!string.IsNullOrEmpty(MindboxLoyaltyProgram.PointOfContact))
        this.m_params.PointOfContact = MindboxLoyaltyProgram.PointOfContact;
      MindboxLoyaltyProgram.IscompatibilityEnabled = MindboxLoyaltyProgram.m_loyaltySettings.COMPATIBILITY;
      if (MindboxLoyaltyProgram.ExcludedPrograms.Count > 0 || !MindboxLoyaltyProgram.IscompatibilityEnabled)
        return;
      MindboxLoyaltyProgram.ExcludedPrograms.Add(MindboxLoyaltyProgram.m_loyaltySettings.ID_LOYALITY_PROGRAM_GLOBAL, (DataRowItem) null);
      foreach (DataRowItem exclude in MindboxLoyaltyProgram.m_loyaltySettings.CompatibilitiesDCT.ExcludeList)
        MindboxLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      foreach (DataRowItem exclude in MindboxLoyaltyProgram.m_loyaltySettings.CompatibilitiesDP.ExcludeList)
      {
        if (!(exclude.Guid == ARM_DISCOUNT2_PROGRAM.MindboxDiscountGUID))
          MindboxLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
      }
      foreach (DataRowItem exclude in MindboxLoyaltyProgram.m_loyaltySettings.CompatibilitiesPL.ExcludeList)
        MindboxLoyaltyProgram.ExcludedPrograms.Add(exclude.Guid, exclude);
    }

    private Customer GetCustomer(LoyaltyCardInfo cardInfo)
    {
      if (this.ClientPublicId == "0000000000")
        return (Customer) null;
      Customer customer = new Customer();
      switch (cardInfo.ClientIdType)
      {
        case PublicIdType.CardNumber:
          customer.DiscountCard = new DiscountCard(cardInfo.ClientId);
          customer.MobilePhone = cardInfo.ClientPhone;
          break;
        case PublicIdType.Phone:
          customer.MobilePhone = cardInfo.ClientId;
          break;
        default:
          throw new NotImplementedException();
      }
      return customer;
    }

    private void ClearSelfDiscounts(CHEQUE cheque)
    {
      cheque.CHEQUE_ITEMS.ForEach((Action<CHEQUE_ITEM>) (ch => ch.Discount2MakeItemList.RemoveAll((Predicate<DISCOUNT2_MAKE_ITEM>) (d => d.ID_DISCOUNT2_PROGRAM_GLOBAL == ARM_DISCOUNT2_PROGRAM.MindboxDiscountGUID))));
      this.GetLoyaltyCard(cheque).ExtraDiscounts.RemoveAll((Predicate<DISCOUNT_VALUE_INFO>) (e => e.TYPE.StartsWith(this.m_typePrefix)));
      cheque.CalculateFields();
    }

    private Order CreateSimpleMindboxOrder(CHEQUE cheque)
    {
      Order order = new Order();
      order.PointOfContact = MindboxLoyaltyProgram.PointOfContact;
      List<Line> lineList = new List<Line>();
      Decimal num1 = 0M;
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        Decimal num2 = chequeItem.SUMM + chequeItem.Discount2MakeItemList.Sum<DISCOUNT2_MAKE_ITEM>((Func<DISCOUNT2_MAKE_ITEM, Decimal>) (d => !(d.ID_DISCOUNT2_PROGRAM_GLOBAL == ARM_DISCOUNT2_PROGRAM.MindboxDiscountGUID) ? 0M : d.AMOUNT));
        Decimal num3 = Math.Ceiling(100M * num2 / chequeItem.QUANTITY) / 100M;
        num1 += num3 * chequeItem.QUANTITY - num2;
        lineList.Add(new Line()
        {
          Quantity = chequeItem.QUANTITY,
          IdLotGlobal = new Guid?(chequeItem.ID_LOT_GLOBAL),
          Sku = new Sku()
          {
            ProductId = string.IsNullOrEmpty(chequeItem.CODE) ? chequeItem.ID_LOT_GLOBAL.ToString() : chequeItem.CODE,
            BasePricePerItem = num3
          }
        });
      }
      order.Lines = lineList.ToArray();
      this.AppendOrderCustomFields(order, cheque);
      if (num1 > 0M)
        order.AddDiscount(new ePlus.Loyalty.Mindbox.Currency()
        {
          Amount = num1,
          Id = "IgnoreDiscountId",
          Type = "externalPromoAction"
        });
      return order;
    }

    private Order CreateSimpleMindboxOrderV3(CHEQUE cheque)
    {
      Order order = new Order();
      List<Line> lineList = new List<Line>();
      Decimal num1 = 0M;
      foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) cheque.CHEQUE_ITEMS)
      {
        Decimal summ = chequeItem.SUMM;
        Decimal num2 = Math.Ceiling(100M * summ / chequeItem.QUANTITY) / 100M;
        lineList.Add(new Line()
        {
          Quantity = chequeItem.QUANTITY,
          IdLotGlobal = new Guid?(chequeItem.ID_LOT_GLOBAL),
          Product = new ProductV3()
          {
            ids = new Ids()
            {
              InternalOrderId = string.IsNullOrEmpty(chequeItem.CODE) ? chequeItem.ID_LOT_GLOBAL.ToString() : chequeItem.CODE
            }
          },
          DiscountedPriceV3 = new Decimal?(summ)
        });
      }
      order.Lines = lineList.ToArray();
      this.AppendOrderCustomFields(order, cheque);
      if (num1 > 0M)
        order.AddDiscount(new ePlus.Loyalty.Mindbox.Currency()
        {
          Amount = num1,
          Id = "IgnoreDiscountId",
          Type = "externalPromoAction"
        });
      return order;
    }

    protected virtual void AppendOrderCustomFields(Order order, CHEQUE cheque)
    {
      if (AppConfigurator.LicensedToRigla)
      {
        OrderCustomFields orderCustomFields = order.CustomFields ?? new OrderCustomFields();
        orderCustomFields.IsOnlineOrder = cheque.IsInternetOrder;
        orderCustomFields.InternetOrderSource = cheque.INTERNET_ORDER_SOURCE_NAME;
        order.CustomFields = orderCustomFields;
      }
      else
      {
        ARMLogger.Trace("Mindbox: Проверка интернет заказа...");
        if (cheque.CHEQUE_ITEMS.Any<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (ci => ci.BOX != null && ci.BOX.Equals("internet", StringComparison.InvariantCultureIgnoreCase))))
        {
          ARMLogger.Trace("Mindbox: Заказ помечен как интернет заказ");
          order.CustomFields = new OrderCustomFields()
          {
            IsInternetOrder = true
          };
        }
        else
          ARMLogger.Trace("Mindbox: В заказе не найдены позиции интернет заказа");
      }
    }

    private Order CreateRecalculationMindboxOrder(CHEQUE cheque)
    {
      Order simpleMindboxOrder = this.CreateSimpleMindboxOrder(cheque);
      simpleMindboxOrder.Ids = new Ids()
      {
        InternalOrderId = cheque.ID_CHEQUE_GLOBAL.ToString()
      };
      return simpleMindboxOrder;
    }

    private Customer GetCustomer() => string.IsNullOrEmpty(this.ClientId) ? this.GetDefaultCustomer() : this.GetCustomer(this.GetLoyaltyCardInfo(false));

    private Customer GetDefaultCustomer()
    {
      Customer defaultCustomer = new Customer();
      if (this.m_params.DefaultLoyaltyCredentials.IsActive && !string.IsNullOrEmpty(this.m_params.DefaultLoyaltyCredentials.MindboxId))
        defaultCustomer.Ids = new CustomerIds()
        {
          Mindbox = this.m_params.DefaultLoyaltyCredentials.MindboxId
        };
      return defaultCustomer;
    }

    private Order CreateMindboxOrder(CHEQUE cheque, Decimal discountSum = 0.0M)
    {
      Order simpleMindboxOrder = this.CreateSimpleMindboxOrder(cheque);
      simpleMindboxOrder.Customer = this.GetCustomer();
      if (discountSum > 0M)
        simpleMindboxOrder.AddDiscount(new ePlus.Loyalty.Mindbox.Currency()
        {
          Type = "balance",
          Amount = discountSum
        });
      if (this.loyaltyCard != null && this.loyaltyCard != null)
      {
        foreach (IPromocode promocode in this.loyaltyCard.Promocodes)
          simpleMindboxOrder.AddDiscount(new ePlus.Loyalty.Mindbox.Currency()
          {
            Type = "promoCode",
            Id = promocode.Id
          });
      }
      return simpleMindboxOrder;
    }

    private Order CreateCheckoutMindboxOrder(CHEQUE cheque, Decimal discountSum)
    {
      Order mindboxOrder = this.CreateMindboxOrder(cheque, discountSum);
      mindboxOrder.PreOrderDiscountedTotalPrice = new Decimal?(cheque.SUMM);
      mindboxOrder.CreatedDateTimeUtc = new DateTime?(DateTime.UtcNow);
      return mindboxOrder;
    }

    private Order GetMindboxOrderId(CHEQUE cheque) => this.m_registeredOrders.ContainsKey(cheque) ? this.m_registeredOrders[cheque] : throw new ApplicationException("Заказ не зарегистрирован в системе Mindbox");

    private Order CreateCancelAllOrder(CHEQUE cheque, long mindboxOrderId)
    {
      Order mindboxOrder = this.CreateMindboxOrder(cheque);
      mindboxOrder.UpdatedDateTimeUtc = new DateTime?(DateTime.UtcNow);
      mindboxOrder.Ids = new Ids()
      {
        MindboxOrderId = new long?(mindboxOrderId)
      };
      mindboxOrder.TotalPrice = new Decimal?(0M);
      return mindboxOrder;
    }

    protected virtual Order CreatePaidMindboxOrder(
      CHEQUE cheque,
      Decimal discountSum,
      long? mindboxOrderId)
    {
      Order mindboxOrder = this.CreateMindboxOrder(cheque, discountSum);
      mindboxOrder.UpdatedDateTimeUtc = new DateTime?(DateTime.UtcNow);
      mindboxOrder.Ids = new Ids()
      {
        InternalOrderId = cheque.ID_CHEQUE_GLOBAL.ToString(),
        MindboxOrderId = mindboxOrderId
      };
      mindboxOrder.Payments = cheque.CHEQUE_PAYMENTS.Select<CHEQUE_PAYMENT, ePlus.Loyalty.Mindbox.Currency>((Func<CHEQUE_PAYMENT, ePlus.Loyalty.Mindbox.Currency>) (c => new ePlus.Loyalty.Mindbox.Currency()
      {
        Type = c.TYPE_PAYMENT_ENUM.ToString().ToLower(),
        Amount = c.SUMM
      })).ToArray<ePlus.Loyalty.Mindbox.Currency>();
      mindboxOrder.TotalPrice = new Decimal?(cheque.SUMM);
      if (AppConfigurator.LicensedToRigla)
        mindboxOrder.Ids.InternetOrderId = cheque.InternetOrderNumber;
      return mindboxOrder;
    }

    private Operation CreateRefundMindboxOrder(
      CHEQUE cheque,
      CHEQUE returnCheque,
      IEnumerable<CHEQUE> refundedCheques)
    {
      Order simpleMindboxOrderV3 = this.CreateSimpleMindboxOrderV3(returnCheque);
      foreach (Line line in simpleMindboxOrderV3.Lines)
        line.StatusString = "efReturn";
      simpleMindboxOrderV3.Ids = new Ids();
      if (this.m_params.ExchangeType == ExchangeTypes.ProApteka)
        simpleMindboxOrderV3.Ids.ExternalOrderCashdeskProapteka = cheque.ID_CHEQUE_GLOBAL.ToString();
      else
        simpleMindboxOrderV3.Ids.InternalOrderIdV3 = cheque.ID_CHEQUE_GLOBAL.ToString();
      return new Operation()
      {
        Order = simpleMindboxOrderV3,
        ExecutionDateTimeUtc = DateTime.UtcNow.ToString()
      };
    }

    private Order CreateRefundAllMindboxOrder(CHEQUE cheque)
    {
      Order simpleMindboxOrder = this.CreateSimpleMindboxOrder(cheque);
      simpleMindboxOrder.UpdatedDateTimeUtc = new DateTime?(DateTime.UtcNow);
      simpleMindboxOrder.Ids = new Ids()
      {
        InternalOrderId = cheque.ID_CHEQUE_GLOBAL.ToString()
      };
      simpleMindboxOrder.TotalPrice = new Decimal?(0M);
      return simpleMindboxOrder;
    }

    private void AddMindboxExtraDiscounts(Order result, CHEQUE cheque, LoyaltyCard lpCard)
    {
      List<CHEQUE_ITEM> second = new List<CHEQUE_ITEM>();
      foreach (Line line1 in result.Lines)
      {
        Line line = line1;
        if (line.AppliedDiscounts != null)
        {
          CHEQUE_ITEM chequeItem = cheque.CHEQUE_ITEMS.Except<CHEQUE_ITEM>((IEnumerable<CHEQUE_ITEM>) second).FirstOrDefault<CHEQUE_ITEM>((Func<CHEQUE_ITEM, bool>) (i =>
          {
            Guid idLotGlobal1 = i.ID_LOT_GLOBAL;
            Guid? idLotGlobal2 = line.IdLotGlobal;
            return idLotGlobal2.HasValue && idLotGlobal1 == idLotGlobal2.GetValueOrDefault();
          }));
          second.Add(chequeItem);
          Dictionary<string, DISCOUNT_VALUE_INFO> dictionary = new Dictionary<string, DISCOUNT_VALUE_INFO>();
          foreach (AppliedDiscount appliedDiscount in line.AppliedDiscounts)
          {
            if (appliedDiscount.Id == null || !appliedDiscount.Id.Equals("IgnoreDiscountId"))
            {
              string upper = string.Format("{1}_{0}", (object) appliedDiscount.GetCurrencyType(), (object) this.m_typePrefix).ToUpper();
              if (dictionary.ContainsKey(upper))
              {
                dictionary[upper].VALUE += appliedDiscount.Amount;
              }
              else
              {
                DISCOUNT_VALUE_INFO discountValueInfo = new DISCOUNT_VALUE_INFO()
                {
                  BARCODE = lpCard.BARCODE,
                  ID_LOT_GLOBAL = chequeItem.ID_LOT_GLOBAL,
                  ID_DISCOUNT2_GLOBAL = ARM_DISCOUNT2_PROGRAM.MindboxDiscountGUID,
                  DISCOUNT2_NAME = this.GetResourceString(appliedDiscount.Type, "MindboxDiscountName", (object) this.Name),
                  TYPE = upper,
                  VALUE = appliedDiscount.Amount
                };
                dictionary.Add(upper, discountValueInfo);
              }
              if (appliedDiscount.Type.Equals("balance", StringComparison.InvariantCultureIgnoreCase))
              {
                PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM()
                {
                  CLIENT_ID = this.ClientPublicId,
                  CLIENT_ID_TYPE = (int) this.LoyaltyType,
                  ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL,
                  OPER_TYPE = "DEBIT",
                  SUMM = appliedDiscount.Amount,
                  BALANCE_NAME = appliedDiscount.BalanceType.Ids.SystemName,
                  ID_PROMOACTION = appliedDiscount.PromoAction.Ids.ExternalId
                };
                lpCard.BalanceChangeList.Add(pcxChequeItem);
              }
            }
          }
          lpCard.ExtraDiscounts.AddRange((IEnumerable<DISCOUNT_VALUE_INFO>) dictionary.Values);
          if (line.AcquiredBalanceChanges != null)
          {
            foreach (AcquiredBalanceChange acquiredBalanceChange in line.AcquiredBalanceChanges)
            {
              if (acquiredBalanceChange.Type.Equals("balance", StringComparison.InvariantCultureIgnoreCase))
              {
                PCX_CHEQUE_ITEM pcxChequeItem = new PCX_CHEQUE_ITEM()
                {
                  CLIENT_ID = this.ClientPublicId,
                  CLIENT_ID_TYPE = (int) this.LoyaltyType,
                  ID_CHEQUE_ITEM_GLOBAL = chequeItem.ID_CHEQUE_ITEM_GLOBAL,
                  OPER_TYPE = "CHARGE",
                  SUMM = acquiredBalanceChange.Amount,
                  BALANCE_NAME = acquiredBalanceChange.BalanceType.Ids.SystemName,
                  ID_PROMOACTION = acquiredBalanceChange.PromoAction.Ids.ExternalId
                };
                lpCard.BalanceChangeList.Add(pcxChequeItem);
              }
            }
          }
        }
      }
    }

    private void AddMessages(Order result, LoyaltyCard lpCard)
    {
      if (!(lpCard is ILoyaltyMessageList loyaltyMessageList) || result.PlaceHolders == null || !result.PlaceHolders.Any<PlaceHolder>() || !this.m_params.LoyaltyInformerOptionList.Any<LoyaltyInformerOption>())
        return;
      foreach (PlaceHolder placeHolder in result.PlaceHolders)
      {
        PlaceHolder ph = placeHolder;
        foreach (LoyaltyInformerOption loyaltyInformerOption in this.m_params.LoyaltyInformerOptionList.Where<LoyaltyInformerOption>((Func<LoyaltyInformerOption, bool>) (o => o.Name.Equals(ph.Id, StringComparison.InvariantCultureIgnoreCase))))
        {
          foreach (ContentItem contentItem in ph.ContentItemList)
            loyaltyMessageList.Add((ILoyaltyMessage) new LoyaltyMessage(loyaltyInformerOption.MessageType, contentItem.Value.Replace(Environment.NewLine, "\n").Replace("\n", Environment.NewLine)));
        }
      }
    }

    public override Decimal CalculateMaxSumBonus(CHEQUE cheque)
    {
      int hashCode = cheque.GetHashCode();
      if (this.MaxDiscountDictionary.ContainsKey(hashCode))
        return this.MaxDiscountDictionary[hashCode];
      this.PreOrderCalculation(cheque);
      return this.MaxDiscountDictionary[cheque.GetHashCode()];
    }

    public override bool IsPreOrderCalculationRequired => true;

    protected override LoyaltyCardInfo? DoGetLoyaltyCardInfoByCheque(CHEQUE cheque)
    {
      ARMLogger.Info("Получение информации по чеку в сервисе Mindbox");
      Order recalculationMindboxOrder = this.CreateRecalculationMindboxOrder(cheque);
      Order order = (Order) null;
      try
      {
        order = this.Api.GetOrderRecalculation(recalculationMindboxOrder);
      }
      catch (MindboxApiExcepion ex)
      {
        ARMLogger.Warn(ex.Message);
      }
      catch (Exception ex)
      {
        ARMLogger.Error(ex.ToString());
      }
      if (order == null)
        return new LoyaltyCardInfo?();
      LoyaltyCardInfo loyaltyCardInfo;
      if (order.Customer.MobilePhone.Any<char>())
      {
        string mobilePhone = order.Customer.MobilePhone;
        loyaltyCardInfo = new LoyaltyCardInfo()
        {
          ClientId = mobilePhone,
          ClientIdType = PublicIdType.Phone
        };
      }
      else
        loyaltyCardInfo = new LoyaltyCardInfo()
        {
          ClientId = (((IEnumerable<DiscountCard>) order.Customer.DiscountCards).FirstOrDefault<DiscountCard>() ?? throw new LoyaltyException((ILoyaltyProgram) this, "У клиента не ни одной карты")).Ids.Number,
          ClientIdType = PublicIdType.CardNumber
        };
      return new LoyaltyCardInfo?(loyaltyCardInfo);
    }

    protected override void DoPreOrderCalculation(CHEQUE cheque)
    {
      LoyaltyCard loyaltyCard = this.GetLoyaltyCard(cheque);
      Decimal discountSum1 = loyaltyCard == null ? 0.0M : loyaltyCard.DiscountSum;
      Decimal val2 = 0M;
      this.ClearSelfDiscounts(cheque);
      loyaltyCard.BalanceChangeList.Clear();
      int hashCode = cheque.GetHashCode();
      if (this.MaxDiscountDictionary.ContainsKey(hashCode))
      {
        val2 = this.MaxDiscountDictionary[hashCode];
      }
      else
      {
        Order mindboxOrder = this.CreateMindboxOrder(cheque, 1000000000M);
        Order preOrderInfo = this.Api.GetPreOrderInfo(mindboxOrder);
        if (mindboxOrder.Customer.MobilePhone == null)
        {
          if (mindboxOrder.Customer.DiscountCard == null)
          {
            cheque.IS_ONLINE = new bool?(false);
            goto label_13;
          }
        }
        Customer customer;
        try
        {
          customer = this.Api.GetCustomer(mindboxOrder.Customer).Customer;
        }
        catch (CustomerNotFoundException ex)
        {
          customer = (Customer) null;
        }
        if (customer != null)
        {
          if (customer.CustomFields != null)
            cheque.OFD_PERMISSION = customer.CustomFields.OfdPermission;
          cheque.IS_ONLINE = new bool?(customer.CustomFields != null && cheque.OFD_PERMISSION.HasValue && cheque.OFD_PERMISSION.Value);
          if (AppConfigurator.IsRiglaLic && cheque.IS_ONLINE.HasValue && cheque.IS_ONLINE.Value)
            cheque.SetDigitalChequeInfo(customer.Email, customer.Email);
        }
        else
          cheque.IS_ONLINE = new bool?(false);
label_13:
        DiscountInfo discountInfo = ((IEnumerable<DiscountInfo>) preOrderInfo.DiscountsInfo).FirstOrDefault<DiscountInfo>((Func<DiscountInfo, bool>) (i => i.Type.Equals("balance")));
        if (discountInfo != null)
          val2 = discountInfo.AvailableAmountForCurrentOrder;
      }
      if (!this.MaxDiscountDictionary.ContainsKey(hashCode))
        this.MaxDiscountDictionary.Add(hashCode, val2);
      Decimal discountSum2 = Math.Min(discountSum1, val2);
      Order mindboxOrder1 = this.CreateMindboxOrder(cheque, discountSum2);
      ARMLogger.Trace("Minbox: Предварительный расчет скидок на сервере");
      Order preOrderInfo1 = this.Api.GetPreOrderInfo(mindboxOrder1);
      loyaltyCard.AcquiredBalanceChange = preOrderInfo1.TotalAcquiredBalanceChange;
      if (loyaltyCard != null)
      {
        this.AddMindboxExtraDiscounts(preOrderInfo1, cheque, loyaltyCard);
        this.AddMessages(preOrderInfo1, loyaltyCard);
      }
      Task.Factory.StartNew((Action) (() => this.GetRecommendations(cheque.CHEQUE_ITEMS.Select<CHEQUE_ITEM, string>((Func<CHEQUE_ITEM, string>) (ci => ci.CODE)))));
      this.loyaltyCard = (MindboxCard) loyaltyCard;
    }

    protected MindboxCard loyaltyCard { get; private set; }

    public override LoyaltyCard GetLoyaltyCard() => (LoyaltyCard) this.loyaltyCard;

    protected override void DoRollback(out string slipCheque)
    {
      slipCheque = string.Empty;
      if (!this.m_registeredOrders.Any<KeyValuePair<CHEQUE, Order>>())
        return;
      foreach (CHEQUE cheque in (IEnumerable<CHEQUE>) this.m_registeredOrders.Keys.ToArray<CHEQUE>())
        this.CancelOrder(cheque);
    }

    private LoyaltyCard GetLoyaltyCard(CHEQUE cheque)
    {
      if (cheque.DiscountCardPolicyList.Find((Predicate<DISCOUNT2_CARD_POLICY>) (c => c is MindboxCard)) is MindboxCard loyaltyCard)
        this.loyaltyCard = loyaltyCard;
      else
        loyaltyCard = this.loyaltyCard;
      return (LoyaltyCard) loyaltyCard;
    }

    protected override bool OnSendAuthenticationCode()
    {
      if (this.m_isAuthenticationCodeSent && this.m_authenticationCodeSentDateTime.HasValue && (DateTime.Now - this.m_authenticationCodeSentDateTime.Value).TotalMinutes <= 0.0)
        return false;
      this.Api.ConfirmationCodeSend(this.GetCustomer(), MindboxLoyaltyProgram.PointOfContact);
      this.m_isAuthenticationCodeSent = true;
      this.m_authenticationCodeSentDateTime = new DateTime?(DateTime.Now);
      return true;
    }

    private void CancelOrder(CHEQUE cheque)
    {
            /*if (!this.m_registeredOrders.ContainsKey(cheque))
              return;
            this.Api.CancelledAll(this.CreateCancelAllOrder(cheque, (this.m_registeredOrders[cheque].Ids.MindboxOrderId ?? throw new LoyaltyException((ILoyaltyProgram) this, "Отсутствует MindboxId для корректной отмены закака")).Value));
            this.m_registeredOrders.Remove(cheque);*/
            if (!this.m_registeredOrders.ContainsKey(cheque))
            {
                return;
            }
            long? mindboxOrderId = this.m_registeredOrders[cheque].Ids.MindboxOrderId;
            if (!mindboxOrderId.HasValue)
            {
                throw new LoyaltyException(this, "Отсутствует MindboxId для корректной отмены закака");
            }
            Order order = this.CreateCancelAllOrder(cheque, mindboxOrderId.Value);
            this.Api.CancelledAll(order);
            this.m_registeredOrders.Remove(cheque);

        }

        public bool SendRecomendationRequests => this.m_params.SendRecomendationRequests;

    public bool SendPersonalRecomendationRequests => this.m_params.SendPersonalRecomendationRequests;

    public bool TakeSurvey => this.m_params.TakeSurvey;

    private List<STOCK_DETAIL> GetStockDetails(string code) => new StocksCache((ICacheManager) new MemoryCacheManager(MemoryCache.Default)).GetCache().Values.Where<STOCK_DETAIL>((Func<STOCK_DETAIL, bool>) (s => s.CODE != null && s.CODE.Equals(code))).ToList<STOCK_DETAIL>();

    private void AddRecomendation(
      CHEQUE cheque,
      RecommendationType type,
      string code,
      params string[] codes)
    {
      List<STOCK_DETAIL> stockDetails1 = string.IsNullOrEmpty(code) ? (List<STOCK_DETAIL>) null : this.GetStockDetails(code);
      foreach (string code1 in codes)
      {
        List<STOCK_DETAIL> stockDetails2 = this.GetStockDetails(code1);
        string typeString = "Персональная";
        if (stockDetails1 != null && stockDetails1.Any<STOCK_DETAIL>())
          typeString = "К товару \"" + stockDetails1.First<STOCK_DETAIL>().GOODS_NAME + (object) '"';
        List<STOCK_DETAIL> list = stockDetails2.Where<STOCK_DETAIL>((Func<STOCK_DETAIL, bool>) (x => x.ACCESSIBLE > 0M && x.LOT_PRICE_VAT > (Decimal) AppConfigurator.MinimumRecomendationPrice)).ToList<STOCK_DETAIL>();
        bool show = list.Any<STOCK_DETAIL>();
        if (!list.Any<STOCK_DETAIL>())
          list.Add(new STOCK_DETAIL() { CODE = code1 });
        MindboxRecommendation mindboxRecommendation = new MindboxRecommendation(type, typeString, code, (IEnumerable<STOCK_DETAIL>) list, show);
        RecomendationForSorting what = new RecomendationForSorting()
        {
          Id = Guid.NewGuid(),
          Recommendation = (IRecommendation) mindboxRecommendation
        };
        if (list.Any<STOCK_DETAIL>())
        {
          what.BEST_BEFORE = list.Min<STOCK_DETAIL, DateTime>((Func<STOCK_DETAIL, DateTime>) (x => x.BEST_BEFORE));
          what.BEST_BEFORE_SORT = what.BEST_BEFORE == DateTime.MinValue ? DateTime.MaxValue : what.BEST_BEFORE;
          what.ID_GOODS = list.First<STOCK_DETAIL>().ID_GOODS;
          what.LOT_PRICE_VAT = mindboxRecommendation.Price;
          what.MARGIN_DISPLAY = list.First<STOCK_DETAIL>().MARGIN_DISPLAY;
          what.NO_LIQUID = list.Any<STOCK_DETAIL>((Func<STOCK_DETAIL, bool>) (x => x.NO_LIQUID));
          what.ONLY_VIEW = list.Any<STOCK_DETAIL>((Func<STOCK_DETAIL, bool>) (x => x.ONLY_VIEW));
          what.RATING = list.Max<STOCK_DETAIL>((Func<STOCK_DETAIL, Decimal>) (x => x.RATING));
          what.RISK_ISG = list.Any<STOCK_DETAIL>((Func<STOCK_DETAIL, bool>) (x => x.RISK_ISG));
        }
        cheque.AddRecomendation(what);
      }
    }

    private void RiseOnUpdated(CHEQUE cheque) => BusinessLogicEvents.Instance.OnRecommendationsUpdated(new RecommendationsEventArgs((IEnumerable<RecomendationForSorting>) cheque.GetRecomendations()));

    public void GetClientRecomendations(CHEQUE cheque)
    {
      try
      {
        this.InitInternal();
        if (!string.IsNullOrEmpty(this.loyaltyCard.CustomerExternalId))
        {
          RecomendationsResultV3 recomendationsV3 = this.Api.GetRecomendationsV3(this.loyaltyCard.CustomerExternalId, 10);
          if (recomendationsV3.Recomendations != null)
            this.AddRecomendation(cheque, RecommendationType.Person, (string) null, ((IEnumerable<Recomendation>) recomendationsV3.Recomendations).Select<Recomendation, string>((Func<Recomendation, string>) (x => x.Ids.Id)).ToArray<string>());
        }
        this.RiseOnUpdated(cheque);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Ошибка при запросе рекомендаций: " + ex.Message);
      }
    }

    public void GetProductRecommendations(string code, CHEQUE cheque)
    {
      try
      {
        this.InitInternal();
        string[] array = this.Api.GetRecomendationsV3((IEnumerable<string>) new string[1]
        {
          code
        }, 10).SelectMany<RecomendationsResultV3, Recomendation>((Func<RecomendationsResultV3, IEnumerable<Recomendation>>) (x => (IEnumerable<Recomendation>) x.Recomendations)).Select<Recomendation, string>((Func<Recomendation, string>) (x => x.Ids.Id)).ToArray<string>();
        this.AddRecomendation(cheque, RecommendationType.Goods, code, array);
        this.RiseOnUpdated(cheque);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Ошибка при запросе рекомендаций: " + ex.Message);
      }
    }
  }
}
