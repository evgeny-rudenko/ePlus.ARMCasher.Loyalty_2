// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.Forms.Bel
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using BELLib;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCommon.Log;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint.Forms
{
  internal class Bel : Form
  {
    private const bool showEndOfServiceWindow = false;
    private const short PAY_TYPE_SEL_CARD_CASH = 1;
    private const short PAY_TYPE_SEL_CARD_SIMPLE = 2;
    private const short PAY_TYPE_SEL_CARD_ANOTHER = 3;
    private const short PAY_TYPE_SEL_BONUS_SIMPLE = 4;
    private const short PAY_TYPE_SEL_BONUS_ANOTHER = 5;
    private const short PAY_TYPE_SEL_CARD_CARD_ONLY = 6;
    private const short PAY_TYPE_CASH = 7;
    private const short PAY_TYPE_SEL_PREPAID_SIMPLE = 8;
    private const short PAY_TYPE_SEL_PREPAID_ANOTHER = 9;
    private const short PAY_TYPE_SEL_PRE_AUTH_SIMPLE = 10;
    private const short PAY_TYPE_SEL_PRE_AUTH_ANOTER = 11;
    private const short PAY_TYPE_SEL_COMPLETION_SIMPLE = 12;
    private const short PAY_TYPE_SEL_COMPLETION_ANOTHER = 99;
    private const short PAY_TYPE_SEL_REFUND_SIMPLE = 13;
    private const short PAY_TYPE_SEL_REFUND_ANOTHER = 14;
    private const short ECR_FLAG_WAIT_ANSWER = 4;
    private const short ECR_FLAG_NO_USE_BS = 8;
    private const short ECR_FLAG_NEPOLN_DATA = 16;
    private const short ECR_FLAG_DISCOUNT_TYPE = 32;
    private const short CHEQUE_FLAG_DISCOUNT_IN_AMOUNT = 2;
    private const short CHEQUE_FLAG_NEED_RECALC = 4;
    private const short CHEQUE_FLAG_DISCOUNT_BY_GOODS = 16;
    private const short OPERATION_SERVICE_FLAGS_NO_CHEQUE = 16;
    private const short CHEQUE_GOOD_FLAG_WITH_DISCOUNT = 1;
    private const short CHEQUE_GOOD_FLAG_DISCOUNT_TYPE = 2;
    private const short CHEQUE_GOOD_FLAG_NEED_RECALC = 4;
    private const short CHEQUE_GOOD_FLAG_BONUS_DISCOUNT_TYPE = 8;
    private const short OPERATION_PAYMENT = 3;
    private const short OPERATION_CLOSE_CHEQUE = 4;
    private const short OPERATION_MODIFY_CHEQUE = 6;
    private const short OPERATION_PREPAID = 7;
    private const short OPERATION_CARD_LINKING = 8;
    private const short OPERATION_CARD_SUBSTITUTION = 9;
    private const short OPERATION_BPRRN_CANCEL = 10;
    private const short OPERATION_CHANGE_PIN = 11;
    private const short OPERATION_EMV_PAYMENT_CANCEL = 12;
    private const short OPERATION_FORM_APPLIANCE = 13;
    private const short OPERATION_GET_INFO = 14;
    private const short OPERATION_ROLLBACK = 15;
    private const short OPERATION_PROMO = 16;
    private const byte COMMUNICATION_ENVIRONMENT_TCP = 0;
    private const byte COMMUNICATION_ENVIRONMENT_RS232 = 1;
    public bool IsCancelled;
    private static Bel _instance;
    public bool IsPromoEnabled;
    public BPECR Bpecr1;
    private int SummaryPrice;
    private int SummaryPriceCash;
    private int SummaryPriceBonus;
    private int SummaryPriceCredit;
    private int SummaryPriceCard;
    private int SummaryPricePrepaid;
    private int SummaryPriceAnother;
    private int AlreadyPayed;
    private int _alreadyPayCash;
    private int _alreadyPayBonus;
    private int _alreadyPayCredit;
    private int _alreadyPayCard;
    private int _alreadyPayPrepaid;
    private int AlreadyPayAnother;
    private int AlreadyPayPreAuthSimple;
    private int AlreadyPayPreAuthAnother;
    private int AlreadyPayCommitAuthSimple;
    private int AlreadyPayCommitAuthAnother;
    private Decimal SummaryPriceWithoutDiscount;
    private int Discount;
    private bool DiscountType;
    public string strFCTempPath;
    private int SumPriceCash;
    private int SumPriceCard;
    private int SumPriceBnss;
    private int SumPriceCdts;
    private int SumPricePrep;
    public string strScrPath;
    public string strPrnPath;
    public bool bFisicalConnectToPOS;
    private bool bChequeWasModified;
    public object TMPCheque;
    private int GoodsCount;
    private string strMagTrack;
    private bool g_WasStartServer;
    private string g_StringTitle = "BEL Master Light  1.12.27";
    public List<GoodsInfo> GoodsList = new List<GoodsInfo>();
    public bool IsApplyEnabled;
    private long _bprrn;
    public bool InitSuccess;
    private IContainer components;
    public TextBox AlreadyPayedPreAuthorization;
    public ToolTip ToolTip1;
    public TextBox GetPayValPreAuthorization;
    public GroupBox Frame19;
    public Button PayAnotherPreAuthorization;
    public TextBox AlreadyPayedAnotherPreAuthorization;
    public TextBox GetPayValAnotherPreAuthorization;
    public Label Label39;
    public Label Label38;
    public GroupBox Frame16;
    public Button PayPreAuthorization;
    public Label Label32;
    public Label Label31;
    public Button RefundSimple;
    public TextBox TextRRNSimple;
    public TextBox GetPayVal;
    public GroupBox Frame17;
    public Button PayCommitAuthorization;
    public TextBox AlreadyCommitAuthorization;
    public TextBox GetPayValCommitAuthorization;
    public Label Label44;
    public Label Label35;
    public Label Label34;
    public Button PayCash;
    public Label Label41;
    public TextBox AlreadyPayedCash;
    public GroupBox Frame6;
    public Label LabelGetPayVal;
    public Label Label16;
    public Label Label40;
    public GroupBox GroupBox2;
    public RadioButton RB_CP1251;
    public RadioButton RB_CP866;
    public GroupBox Frame15;
    public GroupBox Frame18;
    public TextBox TextRRNAnother;
    public TextBox GetPayValAnotherCommitAuthorization;
    public TextBox AlreadyAnotherCommitAuthorization;
    public Button PayAnotherCommitAuthorization;
    public Button RefundAnother;
    public Label Label45;
    public Label Label37;
    public Label Label36;
    public TextBox GoodsNames;
    public Timer Timer2;
    public Button PayPrepaidAnother;
    public TextBox GetPayValAnother;
    public TextBox AlreadyPayedAnother;
    public Label Label26;
    public Label Label25;
    public Button PayPrepaid;
    public Button PayAnotherCard;
    public Button PayCardWOReg;
    public TextBox AlreadyPayedPrepaid;
    public TextBox GetPayValPrepaid;
    public GroupBox Frame13;
    public Button FCChequeTimeButton;
    public Button CloseFC;
    public Label LabeAlreadyPayed;
    public Label LabelForLabelAlreadyPayed;
    public Label LabelSummPrice;
    public Label SummPriceWithoutDiscount;
    public Label Label24;
    public GroupBox Frame26;
    public Label Label23;
    public Label LabelBonusBalance;
    public Timer Timer3;
    public Button InfoBtn;
    public GroupBox Frame5;
    public GroupBox Frame12;
    public Button PayAnotherBonuses;
    public Button PayAnothBomuses;
    public GroupBox Frame10;
    public Label LabelGetPayValPrepaid;
    public GroupBox Frame9;
    public Button PayCredit;
    public TextBox GetPayValCredit;
    public TextBox AlreadyPayedCredit;
    public Label LabelGetPayValCredit;
    public Label Label21;
    public GroupBox Frame8;
    public Button PayBonuses;
    public TextBox AlreadyPayedBonuses;
    public TextBox GetPayValBonuses;
    public Label Label20;
    public Label LabelGetPayValBonuses;
    public GroupBox Frame7;
    public Button PayCard;
    public TextBox GetPayValCard;
    public TextBox AlreadyPayedCard;
    public Label LabelGetPayValCard;
    public Label Label17;
    public GroupBox GroupBox1;
    public RadioButton RB_RS232;
    public RadioButton RB_Channel;
    public Button ButtonRollback;
    public Button CardsLinking;
    public TextBox AmountLinkingBonuses;
    public TextBox AmountForCancel;
    public TextBox AmountSubstitutionCirculatings;
    public TextBox GetTimeout;
    public Button FormAppliance;
    public Label LabelClientID;
    public GroupBox Frame4;
    public Button ChangePIN;
    public TextBox BpRrnField;
    public CheckBox Check_Deposit;
    public Label Label11;
    public TextBox GoodsAmount;
    public TextBox GoodsQuantity;
    public Button NewOrder;
    public TextBox PrnText;
    public Button BpRrnCancel;
    public TextBox GoodsPrices;
    public Button ResetBtn;
    public Button OK;
    public TextBox BarCoder;
    public TextBox CoodCount;
    public GroupBox FrameBprrnCancel;
    public Label Label9;
    public Label Label8;
    public TextBox GoodsCode;
    public Label Label28;
    public ComboBox ComboBoxCardType;
    public TextBox MagTrack1Field;
    public GroupBox Frame23;
    public CheckBox Check_TransmitCardData;
    public TextBox MagTrack2Field;
    public Label Label50;
    public Label Label49;
    public Label Label47;
    public Timer Timer1;
    public CheckBox Check_UseKLProtocol;
    public GroupBox Frame22;
    public Label Label48;
    public Label LabelClientCardID;
    public GroupBox Frame20;
    public TextBox AmountForCancelEMV;
    public TextBox MerchRrnField;
    public Button MerchRrnCancel;
    public Label Label43;
    public Label Label42;
    public GroupBox Frame14;
    public CheckBox Check_ModificationBeforePayments;
    public CheckBox Check_DiscountByGoods;
    public CheckBox CheckAutoGenECROpId;
    public TextBox AmountPrepaid;
    public Label Label46;
    public CheckBox CheckAutoIncECROpId;
    public Label Label33;
    public GroupBox FrameOpId;
    public CheckBox CheckAutoGenBpSId;
    public TextBox ECROpId;
    public TextBox BpSId;
    public Button PromoBtn;
    public Button SetDiscount;
    public Label Label30;
    public Label Label15;
    public Label Label14;
    public Label Label7;
    public Label Label6;
    public Button DiscountRequest;
    public Label Label5;
    public Label Label4;
    public Label Label3;
    public Label Label2;
    public Label LabelStatus;
    public TextBox CustomDesplayMessageText;
    public GroupBox Frame3;
    public Button CardSubstitution;
    public Label Label13;
    public Label Label12;
    public TextBox AmountSubstitutionBonuses;
    public CheckBox StornoCheck;
    public TextBox ScreenText;
    public Button PrepaidCharge;
    public GroupBox Frame1;
    public GroupBox Frame2;
    public Label Label10;
    public Button ApplyBtn;
    public Label Label22;
    public GroupBox Frame11;
    public TextBox ConsultantField;
    public Label _Label1_0;
    public TextBox DiscountField;
    public Label Label19;
    public Label LabelDiscountRubl;
    public TextBox InterConnectTimeoutVal;
    public GroupBox Frame24;
    public TextBox RetryQuantityVal;
    public CheckBox Check_IntegralProt;
    public Button ButtonDisconnect;
    public GroupBox Frame25;
    public Button ButtonStartServer;
    public Button ButtonStopServer;
    public Label LabelStateConnect;
    public CheckBox CheckBox_RestartServer;
    public Label Label18;
    public GroupBox GroupBox3;

    public static Bel Instance
    {
      get
      {
        if (Bel._instance == null || Bel._instance.IsDisposed)
          Bel._instance = new Bel();
        return Bel._instance;
      }
      set => Bel._instance = value;
    }

    public string Track1StrValue => this.MagTrack1Field.Text;

    public string Track2StrValue => this.MagTrack2Field.Text;

    public int IndexCardTypeComboBox => this.ComboBoxCardType.SelectedIndex;

    public int AlreadyPayCash
    {
      get => this._alreadyPayCash;
      private set => this._alreadyPayCash = value;
    }

    public int AlreadyPayBonus
    {
      get => this._alreadyPayBonus;
      private set => this._alreadyPayBonus = value;
    }

    public int AlreadyPayCredit
    {
      get => this._alreadyPayCredit;
      private set => this._alreadyPayCredit = value;
    }

    public int AlreadyPayCard
    {
      get => this._alreadyPayCard;
      private set => this._alreadyPayCard = value;
    }

    public int AlreadyPayPrepaid
    {
      get => this._alreadyPayPrepaid;
      private set => this._alreadyPayPrepaid = value;
    }

    private void OutputScrInfo()
    {
      string screenMessage = this.Bpecr1.ScreenMessage as string;
      if (string.IsNullOrEmpty(screenMessage))
        return;
      this.ScreenText.Text += screenMessage;
      this.Bpecr1.ScreenMessage = (object) "";
    }

    private void OutputPrnInfo()
    {
      string printerMessage = this.Bpecr1.PrinterMessage as string;
      if (string.IsNullOrEmpty(printerMessage))
        return;
      this.PrnText.Text += printerMessage + "\r\n";
      this.Bpecr1.PrinterMessage = (object) "";
    }

    private void OutputCustomerInfo()
    {
      string customerDisplayMessage = this.Bpecr1.CustomerDisplayMessage as string;
      if (string.IsNullOrEmpty(customerDisplayMessage))
        return;
      this.CustomDesplayMessageText.Text += Utils.PrepareString0D0A(ref customerDisplayMessage) + "\r\n";
    }

    private string GetParamFromAddFile(ref int iParamNumber) => (string) this.Bpecr1.get_Parameter((ParamName) iParamNumber);

    private int GetDiscount()
    {
      int num = 0;
      num = 0;
      try
      {
        return Convert.ToInt32((string) this.Bpecr1.get_Parameter(ParamName.Discount));
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    private bool IsDiscountInMoney()
    {
      bool flag = false;
      if (((int) Convert.ToUInt16((string) this.Bpecr1.get_Parameter(ParamName.BPPRN)) & 32) == 1)
        flag = true;
      return flag;
    }

    private void DisableItemsByPayment()
    {
      this.DiscountField.Enabled = false;
      this.SetDiscount.Enabled = false;
      this.DiscountRequest.Enabled = false;
      this.BpRrnCancel.Enabled = false;
    }

    private void RefreshSummPrice()
    {
    }

    private void RefreshSummPriceWithoutDiscount() => this.SummPriceWithoutDiscount.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPriceWithoutDiscount / 100M));

    private void RefreshGetPayVals()
    {
      if (this.SummaryPrice - this.AlreadyPayed >= 0)
        this.GetPayVal.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPrice - this.AlreadyPayed) / 100));
      else
        this.GetPayVal.Text = "0.00";
      if (this.SummaryPriceCash - this.AlreadyPayCash >= 0)
        this.GetPayVal.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPriceCash - this.AlreadyPayCash) / 100));
      else
        this.GetPayVal.Text = "0.00";
      if (this.SummaryPriceCard - this.AlreadyPayCard >= 0)
        this.GetPayValCard.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPriceCard - this.AlreadyPayCard) / 100));
      else
        this.GetPayValCard.Text = "0.00";
      if (this.SummaryPriceCredit - this.AlreadyPayCredit >= 0)
        this.GetPayValCredit.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPriceCredit - this.AlreadyPayCredit) / 100));
      else
        this.GetPayValCredit.Text = "0.00";
      if (this.SummaryPriceBonus - this.AlreadyPayBonus >= 0)
        this.GetPayValBonuses.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPriceBonus - this.AlreadyPayBonus) / 100));
      else
        this.GetPayValBonuses.Text = "0.00";
      if (this.SummaryPricePrepaid - this.AlreadyPayPrepaid >= 0)
        this.GetPayValPrepaid.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPricePrepaid - this.AlreadyPayPrepaid) / 100));
      else
        this.GetPayValPrepaid.Text = "0.00";
      if (this.SummaryPriceAnother - this.AlreadyPayAnother >= 0)
        this.GetPayValAnother.Text = string.Format("{0:#,#0.00}", (object) ((this.SummaryPriceAnother - this.AlreadyPayAnother) / 100));
      else
        this.GetPayValAnother.Text = "0.00";
      if (this.AlreadyPayPreAuthSimple >= 0)
        this.GetPayValPreAuthorization.Text = string.Format("{0:#,#0.00}", (object) (this.AlreadyPayPreAuthSimple / 100));
      else
        this.GetPayValPreAuthorization.Text = "0.00";
      if (this.AlreadyPayPreAuthAnother >= 0)
        this.GetPayValAnotherPreAuthorization.Text = string.Format("{0:#,#0.00}", (object) (this.AlreadyPayPreAuthAnother / 100));
      else
        this.GetPayValAnotherPreAuthorization.Text = "0.00";
      if (this.AlreadyPayPreAuthSimple - this.AlreadyPayCommitAuthSimple > 0)
        this.GetPayValCommitAuthorization.Text = string.Format("{0:#,#0.00}", (object) ((this.AlreadyPayPreAuthSimple - this.AlreadyPayCommitAuthSimple) / 100));
      else
        this.GetPayValCommitAuthorization.Text = "0.00";
      if (this.AlreadyPayPreAuthAnother - this.AlreadyPayCommitAuthAnother > 0)
        this.GetPayValAnotherCommitAuthorization.Text = string.Format("{0:#,#0.00}", (object) ((this.AlreadyPayPreAuthAnother - this.AlreadyPayCommitAuthAnother) / 100));
      else
        this.GetPayValAnotherCommitAuthorization.Text = "0.00";
    }

    private void RefreshAlreadyPayeds()
    {
      this.LabeAlreadyPayed.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayed / 100M));
      this.AlreadyPayedCard.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayCard / 100M));
      this.AlreadyPayedCredit.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayCredit / 100M));
      this.AlreadyPayedCash.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayCash / 100M));
      this.AlreadyPayedBonuses.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayBonus / 100M));
      this.AlreadyPayedPrepaid.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayPrepaid / 100M));
      this.AlreadyPayedAnother.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.AlreadyPayAnother / 100M));
      if (this.AlreadyPayPreAuthSimple - this.AlreadyPayCommitAuthSimple > 0)
        this.AlreadyPayedPreAuthorization.Text = string.Format("{0:#,#0.00}", (object) ((this.AlreadyPayPreAuthSimple - this.AlreadyPayCommitAuthSimple) / 100));
      else
        this.AlreadyPayedPreAuthorization.Text = "0.00";
      if (this.AlreadyPayPreAuthAnother - this.AlreadyPayCommitAuthAnother > 0)
        this.AlreadyPayedAnotherPreAuthorization.Text = string.Format("{0:#,#0.00}", (object) ((this.AlreadyPayPreAuthAnother - this.AlreadyPayCommitAuthAnother) / 100));
      else
        this.AlreadyPayedAnotherPreAuthorization.Text = "0.00";
      this.AlreadyCommitAuthorization.Text = string.Format("{0:#,#0.00}", (object) (this.AlreadyPayCommitAuthSimple / 100));
      this.AlreadyAnotherCommitAuthorization.Text = string.Format("{0:#,#0.00}", (object) (this.AlreadyPayCommitAuthAnother / 100));
      if (this.AlreadyPayPreAuthSimple == 0)
        this.AlreadyPayCommitAuthSimple = 0;
      if (this.AlreadyPayPreAuthAnother == 0)
        this.AlreadyPayCommitAuthAnother = 0;
      this.GetPayValCommitAuthorization.Text = this.AlreadyPayedPreAuthorization.Text;
      this.GetPayValAnotherCommitAuthorization.Text = this.AlreadyPayedAnotherPreAuthorization.Text;
      if (this.AlreadyPayed < this.SummaryPrice || this.SummaryPrice == 0)
        return;
      this.CloseFC.Enabled = true;
    }

    public void PrepareCommunications()
    {
      this.Bpecr1.UseCP1251 = this.RB_CP1251.Checked ? 1 : 0;
      this.Bpecr1.CommChannel = this.RB_RS232.Checked ? (byte) 1 : (byte) 0;
      this.Bpecr1.UseIntegralProtocol = this.Check_IntegralProt.CheckState == CheckState.Checked ? 1 : 0;
      this.Bpecr1.UseContextlessProtocol = this.Check_UseKLProtocol.CheckState == CheckState.Checked ? 1 : 0;
      this.Bpecr1.RetryQuantity = 5U;
      this.Bpecr1.InterConnectTimeout = 5U;
    }

    public ErrorInterpreter.ReturnCode ConnectToPos()
    {
      try
      {
        ErrorInterpreter.ReturnCode pos = ErrorInterpreter.ReturnCode.Ok;
        if (this.Bpecr1.IsPOSConnected > 0)
          return pos;
        if (this.Bpecr1.IsServerModeRunning > 0)
        {
          int num = (int) MessageBox.Show("Ошибка! Терминал не установил соединение с ККМ.", "ConnectToPOS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          return ErrorInterpreter.ReturnCode.CommunicationError;
        }
        this.PrepareCommunications();
        this.Bpecr1.ConnectToPOS();
        if (this.Bpecr1.RetCode != 0)
        {
          int operationRunning = this.Bpecr1.IsOperationRunning;
          int num = (int) MessageBox.Show("Ошибка установления соединения с POS!", "ConnectToPOS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          pos = (ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode;
          this.bFisicalConnectToPOS = this.Bpecr1.IsPOSConnected == 1;
        }
        else
          this.bFisicalConnectToPOS = this.Bpecr1.IsPOSConnected == 1;
        return pos;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Ошибка установления соединения с POS!", "ConnectToPOS", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return ErrorInterpreter.ReturnCode.CommunicationError;
      }
    }

    private void ButtonDisconnectClick(object eventSender, EventArgs eventArgs)
    {
      if (this.Check_UseKLProtocol.CheckState == CheckState.Checked)
        this.Bpecr1.DisconnectFromPOS();
      this.bFisicalConnectToPOS = false;
    }

    public void PreProcessOperationId()
    {
      if (!this.CheckAutoGenBpSId.Checked)
        this.Bpecr1.BPSID = (object) this.BpSId.Text;
      if (!this.CheckAutoGenECROpId.Checked)
        this.Bpecr1.ECROpId = Convert.ToInt16(Conversion.Val(this.ECROpId.Text));
      if (this.Check_IntegralProt.Checked)
        this.Bpecr1.UseIntegralProtocol = 1;
      else
        this.Bpecr1.UseIntegralProtocol = 0;
    }

    public void PostProcessOperationId(bool isNewSession)
    {
      if (this.Bpecr1.AutoGenBPSID != 0)
      {
        this.BpSId.Text = this.Bpecr1.BPSID as string;
        this.BpSId.Refresh();
      }
      else
      {
        int num = isNewSession ? 1 : 0;
        this.Bpecr1.BPSID = (object) this.BpSId.Text;
      }
      if (this.Bpecr1.AutoGenECROpId != 0)
      {
        this.ECROpId.Text = Conversion.Str((object) this.Bpecr1.ECROpId);
      }
      else
      {
        if (isNewSession)
          this.ECROpId.Text = "1";
        else if (this.CheckAutoIncECROpId.Checked)
          this.ECROpId.Text = Conversion.Str((object) (Convert.ToInt32(Conversion.Val(this.ECROpId.Text)) + 1));
        this.ECROpId.Refresh();
        this.Bpecr1.ECROpId = Convert.ToInt16(Conversion.Val(this.ECROpId.Text));
      }
    }

    public long WrapPerformPosOperations(short operationId)
    {
      DialogPerfOper dialogPerfOper = new DialogPerfOper();
      try
      {
        dialogPerfOper.pBELForm = this;
        dialogPerfOper.OperationID = (int) operationId;
        DialogResult dialogResult = dialogPerfOper.ShowDialog();
        this.IsCancelled = dialogPerfOper.IsCancelled;
        long num1;
        if (dialogResult == DialogResult.OK)
        {
          num1 = 0L;
        }
        else
        {
          num1 = 2L;
          this.Bpecr1.DisconnectFromPOS();
          this.RestartServer();
          this.bFisicalConnectToPOS = false;
          int num2 = (int) MessageBox.Show("Операция прервана.");
        }
        return num1;
      }
      catch (Exception ex)
      {
        return 4;
      }
    }

    private void OnApply()
    {
      try
      {
        this.LabelStatus.Text = "Ожидайте...";
        this.LabelStatus.Refresh();
        this.LabelBonusBalance.Text = "0,00";
        this.Frame26.Visible = false;
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.SetCardData();
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
          return;
        this.TMPCheque = this.Bpecr1.Cheque;
        this.PreProcessOperationId();
        if (this.WrapPerformPosOperations((short) 14) != 0L)
        {
          this.Bpecr1.Cheque = this.TMPCheque;
        }
        else
        {
          this.PostProcessOperationId(false);
          this.Bpecr1.Cheque = this.TMPCheque;
          if (this.Bpecr1.RetCode == 0)
          {
            this.Check_UseKLProtocol.Enabled = false;
            this.OutputScrInfo();
            this.OutputPrnInfo();
            this.OutputCustomerInfo();
            if (!string.IsNullOrEmpty(this.Bpecr1.CustomerDisplayMessage as string))
            {
              string customerDisplayMessage = this.Bpecr1.CustomerDisplayMessage as string;
              this.CustomDesplayMessageText.Text = Utils.PrepareString0D0A(ref customerDisplayMessage);
              this.CustomDesplayMessageText.Text += "\r\n";
            }
            int iParamNumber = 19;
            string paramFromAddFile = this.GetParamFromAddFile(ref iParamNumber);
            if (!string.IsNullOrEmpty(paramFromAddFile))
            {
              this.LabelClientID.Text = paramFromAddFile;
              this.Frame14.Visible = true;
            }
            this.bChequeWasModified = false;
            if (this.Check_Deposit.CheckState == CheckState.Checked)
            {
              this.LabelStatus.Text = "Модификация чека...";
              this.LabelStatus.Refresh();
              if (this.ModifyCheque())
                this.bChequeWasModified = true;
            }
            this.LabelStatus.Text = "Режим оплаты";
            this.LabelStatus.Refresh();
            this.IsApplyEnabled = false;
            this.IsPromoEnabled = false;
            this.MakePaymentsVisible();
            if (this.Check_Deposit.CheckState == CheckState.Checked)
            {
              if (this.bChequeWasModified)
              {
                this.DiscountRequest.Visible = false;
                this.SetDiscount.Visible = false;
                this.DiscountField.Visible = false;
                this.LabelDiscountRubl.Visible = false;
              }
              this.SummaryPrice = (int) this.SummaryPriceWithoutDiscount;
            }
            else
            {
              this.Discount = this.GetDiscount();
              this.DiscountType = this.IsDiscountInMoney();
              if (this.DiscountType)
              {
                this.LabelDiscountRubl.Visible = true;
                this.SummaryPrice = !(this.SummaryPriceWithoutDiscount - (Decimal) this.Discount <= 0M) ? (int) (this.SummaryPriceWithoutDiscount - (Decimal) this.Discount) : 0;
              }
              else
              {
                this.LabelDiscountRubl.Visible = false;
                this.SummaryPrice = this.Discount >= 10000 || this.Discount < 0 ? 0 : (int) (this.SummaryPriceWithoutDiscount / 10000M * (Decimal) (10000 - this.Discount));
              }
              this.DiscountField.Text = string.Format("{0:#,#0.00}", (object) (this.Discount / 100));
              this.DiscountRequest.Visible = true;
              this.SetDiscount.Visible = true;
              this.DiscountField.Visible = true;
              this.DiscountRequest.Enabled = true;
              this.SetDiscount.Enabled = true;
              this.DiscountField.Enabled = true;
            }
            this.RefreshSummPriceWithoutDiscount();
            this.RefreshSummPrice();
            this.RefreshGetPayVals();
          }
          else
          {
            if (this.Bpecr1.RetCode == 2 && this.bFisicalConnectToPOS)
            {
              this.Bpecr1.DisconnectFromPOS();
              this.bFisicalConnectToPOS = false;
              this.RestartServer();
            }
            ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка операции 'Инфо карты': ");
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Подытог", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void ApplyBtn_Click(object eventSender, EventArgs eventArgs) => this.OnApply();

    private void BpRrnCancel_Click(object eventSender, EventArgs eventArgs) => this.OnBpRrnCancel();

    public bool OnBpRrnCancel()
    {
      string str1 = (string) null;
      try
      {
        str1 = this.LabelStatus.Text;
        string Expression = this.BpRrnField.Text;
        int num1 = Strings.Len(Expression);
        if (num1 > 20)
        {
          int num2 = (int) MessageBox.Show("Длина BpRrn не может быть больше 20. Проверьте правильность ввода BpRrn.");
          return false;
        }
        if (num1 < 20)
        {
          string str2 = Strings.Mid("00000000000000000000", 1, 20 - num1) + Expression;
          this.BpRrnField.Text = str2;
          Expression = str2;
        }
        this.Bpecr1.BPRRN = (object) Expression;
        DialogCheque.DefInstance.ClearModifCheque();
        if (this.GoodsCount > 0)
          this.AddAllGoods();
        this.Bpecr1.Amount = Convert.ToInt32(this.AmountForCancel.Text.Replace(".", "").Replace(",", ""));
        short flags = this.Bpecr1.Flags;
        if (this.Bpecr1.goodCount == (short) 0)
        {
          this.Bpecr1.Flags |= (short) 16;
        }
        else
        {
          this.Bpecr1.Flags &= (short) -17;
          this.Bpecr1.BPRRNOrig = (object) Expression;
        }
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.SetCardData();
        this.LabelStatus.Text = "Отмена операции...";
        this.LabelStatus.Refresh();
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.LabelStatus.Text = str1;
          return false;
        }
        this.TMPCheque = this.Bpecr1.Cheque;
        this.PreProcessOperationId();
        if (this.WrapPerformPosOperations((short) 10) != 0L)
          return false;
        this.PostProcessOperationId(false);
        this.LabelStatus.Text = str1;
        this.LabelStatus.Refresh();
        this.Bpecr1.Flags = flags;
        if (this.Bpecr1.RetCode == 0)
        {
          this.OutputScrInfo();
          this.OutputPrnInfo();
          this.OutputCustomerInfo();
          if (this.IsChequePresent())
          {
            DialogCheque.DefInstance.TextGoodCode.Text = this.GoodsCode.Text;
            DialogCheque.DefInstance.TextGoodName.Text = this.GoodsNames.Text;
            DialogCheque.DefInstance.TextGoodPrice.Text = this.GoodsPrices.Text;
            DialogCheque.DefInstance.TextGoodQuantity.Text = this.GoodsQuantity.Text;
            DialogCheque.DefInstance.TextGoodAmount.Text = this.GoodsAmount.Text;
            DialogCheque.DefInstance.SetGoods();
            string text1 = DialogCheque.DefInstance.Text;
            DialogCheque.DefInstance.Text = "Отмена покупки";
            string text2 = DialogCheque.DefInstance.LabelModifCheque.Text;
            DialogCheque.DefInstance.LabelModifCheque.Text = "Список отменяемых товаров";
            DialogCheque.DefInstance.Command1.Visible = true;
            DialogCheque.DefInstance.ButtonPayModif.Visible = false;
            DialogCheque.DefInstance.ButtonModification.Visible = false;
            DialogCheque.DefInstance.ChequeTimeButton2.Visible = false;
            DialogCheque.DefInstance.ButtomPaySource.Visible = false;
            DialogCheque.DefInstance.TextModifChequeDiscount1.Visible = false;
            DialogCheque.DefInstance.TextModifChequeAmount.Visible = false;
            DialogCheque.DefInstance.Label23.Visible = false;
            DialogCheque.DefInstance.Label16.Visible = false;
            DialogCheque.DefInstance.Text = text1;
            DialogCheque.DefInstance.LabelModifCheque.Text = text2;
            DialogCheque.DefInstance.Command1.Visible = false;
            DialogCheque.DefInstance.ButtonPayModif.Visible = true;
            DialogCheque.DefInstance.ButtonModification.Visible = true;
            DialogCheque.DefInstance.ChequeTimeButton2.Visible = true;
            DialogCheque.DefInstance.ButtomPaySource.Visible = true;
            DialogCheque.DefInstance.TextModifChequeDiscount1.Visible = true;
            DialogCheque.DefInstance.TextModifChequeAmount.Visible = true;
            DialogCheque.DefInstance.Label23.Visible = true;
            DialogCheque.DefInstance.Label16.Visible = true;
          }
          else
          {
            string Prompt = "Операция отмены проведена успешно.";
            int iParamNumber = 3;
            string paramFromAddFile = this.GetParamFromAddFile(ref iParamNumber);
            if (!string.IsNullOrEmpty(paramFromAddFile))
            {
              int int32 = Convert.ToInt32(paramFromAddFile);
              Prompt = Prompt + "\r\n" + "Сумма к возврату: " + string.Format("{0:#,#0.00}", (object) ((Decimal) int32 / 100M));
            }
            int num3 = (int) Interaction.MsgBox((object) Prompt);
            this.Bpecr1.Amount = 0;
            this.Bpecr1.Discount = 0;
            this.Bpecr1.RemoveAllItemsFromCheque();
            this.ClearGoodFileds();
          }
        }
        else
        {
          if (this.Bpecr1.RetCode == 2 && this.bFisicalConnectToPOS)
          {
            this.Bpecr1.DisconnectFromPOS();
            this.bFisicalConnectToPOS = false;
            this.RestartServer();
          }
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка операции 'Отмена по BpRrn': ");
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Подытог", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str1;
        this.LabelStatus.Refresh();
        return false;
      }
      return true;
    }

    private void CardsLinking_Click(object eventSender, EventArgs eventArgs)
    {
      string str = (string) null;
      try
      {
        str = this.LabelStatus.Text;
        this.Bpecr1.AmountBonuses = Convert.ToInt32(this.AmountLinkingBonuses.Text.Replace(".", "").Replace(",", ""));
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.LabelStatus.Text = "Привязка карт...";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.LabelStatus.Text = str;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 8) != 0L)
          {
            this.Bpecr1.Cheque = this.TMPCheque;
          }
          else
          {
            this.PostProcessOperationId(false);
            this.Bpecr1.Cheque = this.TMPCheque;
            this.LabelStatus.Text = str;
            if (this.Bpecr1.RetCode == 0)
            {
              this.OutputScrInfo();
              this.OutputPrnInfo();
              this.OutputCustomerInfo();
            }
            else
            {
              int retCode = this.Bpecr1.RetCode;
              if (retCode == 2 && this.bFisicalConnectToPOS)
              {
                this.Bpecr1.DisconnectFromPOS();
                this.bFisicalConnectToPOS = false;
                this.RestartServer();
              }
              string errorMessage = "Ошибка операции 'Привязка карты'";
              ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, errorMessage);
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Привязка карты", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str;
      }
    }

    private void CardSubstitution_Click(object eventSender, EventArgs eventArgs)
    {
      string str = (string) null;
      try
      {
        str = this.LabelStatus.Text;
        this.Bpecr1.AmountBonuses = Convert.ToInt32(this.AmountSubstitutionBonuses.Text.Replace(".", "").Replace(",", ""));
        this.Bpecr1.AmountCirculatings = Convert.ToInt32(this.AmountSubstitutionCirculatings.Text.Replace(".", "").Replace(",", ""));
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.LabelStatus.Text = "Замена карты...";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.LabelStatus.Text = str;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 9) != 0L)
          {
            this.Bpecr1.Cheque = this.TMPCheque;
          }
          else
          {
            this.PostProcessOperationId(false);
            this.Bpecr1.Cheque = this.TMPCheque;
            this.LabelStatus.Text = str;
            if (this.Bpecr1.RetCode == 0)
            {
              this.OutputScrInfo();
              this.OutputPrnInfo();
              this.OutputCustomerInfo();
            }
            else
            {
              int retCode = this.Bpecr1.RetCode;
              if (retCode == 2 && this.bFisicalConnectToPOS)
              {
                this.Bpecr1.DisconnectFromPOS();
                this.bFisicalConnectToPOS = false;
                this.RestartServer();
              }
              ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Замена карты'");
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Замена карты", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str;
      }
    }

    private void ChangePIN_Click(object eventSender, EventArgs eventArgs)
    {
      string str = (string) null;
      try
      {
        str = this.LabelStatus.Text;
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.SetCardData();
        this.LabelStatus.Text = "Смена PIN...";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.LabelStatus.Text = str;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 11) != 0L)
          {
            this.Bpecr1.Cheque = this.TMPCheque;
          }
          else
          {
            this.PostProcessOperationId(false);
            this.Bpecr1.Cheque = this.TMPCheque;
            this.LabelStatus.Text = str;
            if (this.Bpecr1.RetCode == 0)
            {
              this.OutputScrInfo();
              this.OutputPrnInfo();
              this.OutputCustomerInfo();
            }
            else
            {
              int retCode = this.Bpecr1.RetCode;
              if (retCode == 2 && this.bFisicalConnectToPOS)
              {
                this.Bpecr1.DisconnectFromPOS();
                this.bFisicalConnectToPOS = false;
                this.RestartServer();
              }
              ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Смена PIN'");
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Смена ПИН", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str;
      }
    }

    private void CloseFC_Click(object eventSender, EventArgs eventArgs)
    {
      string str1 = (string) null;
      int code = 0;
      try
      {
        string str2 = "";
        str1 = this.LabelStatus.Text;
        this.Bpecr1.Flags = (short) 0;
        this.Bpecr1.RetCode = 0;
        this.Bpecr1.ShopAssistantId = Convert.ToInt32(this.ConsultantField.Text);
        if (this.Check_Deposit.CheckState != CheckState.Checked)
          this.Bpecr1.Discount = Convert.ToInt32(Conversion.Val(this.DiscountField.Text.Replace(".", "").Replace(",", "")));
        this.Bpecr1.Amount = this.SumPriceCash;
        this.Bpecr1.AmountCards = this.SumPriceCard;
        this.Bpecr1.AmountBonuses = this.SumPriceBnss;
        this.Bpecr1.AmountCredits = this.SumPriceCdts;
        this.Bpecr1.AmountPrepaids = this.SumPricePrep;
        this.SetCardData();
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.LabelStatus.Text = "Ожидайте...";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) code, "Ошибка операции 'Закрытие чека'");
          this.LabelStatus.Text = str1;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 4) != 0L)
            return;
          this.PostProcessOperationId(true);
          this.LabelStatus.Text = str1;
          if (this.Bpecr1.RetCode != 0)
          {
            int retCode = this.Bpecr1.RetCode;
            if (retCode == 2 && this.bFisicalConnectToPOS)
            {
              this.Bpecr1.DisconnectFromPOS();
              this.bFisicalConnectToPOS = false;
              this.RestartServer();
            }
            ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Закрытие чека'");
          }
          else
          {
            this.OutputScrInfo();
            this.OutputPrnInfo();
            this.OutputCustomerInfo();
            int iParamNumber = 19;
            string paramFromAddFile = this.GetParamFromAddFile(ref iParamNumber);
            if (!string.IsNullOrEmpty(paramFromAddFile))
            {
              for (int index = 0; index < Strings.Len(paramFromAddFile); ++index)
              {
                string str3 = Strings.Mid(paramFromAddFile, index + 1, 1);
                if (!(str3 == "."))
                  str2 += str3;
                else
                  break;
              }
              this.LabelClientID.Text = str2;
              this.Frame14.Visible = true;
            }
          }
          if (this.bFisicalConnectToPOS)
          {
            this.Bpecr1.DisconnectFromPOS();
            this.bFisicalConnectToPOS = false;
          }
          this.MakePaymentsInvisible();
          this.NewOrder.Visible = true;
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Закрытие чека", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str1;
      }
    }

    private void ReloadParams(ref bool bIncSlipNumber)
    {
      this.Bpecr1.BELIPAddress = (object) "127.0.0.1";
      this.Bpecr1.BELPort = 7878;
      this.Bpecr1.ECRNumber = 1;
      this.Bpecr1.ShopNumber = 2;
      this.Bpecr1.SlipNumber = 3;
      this.Bpecr1.SecondsToWait = 90;
      this.strScrPath = "c:\\bp_scr.txt";
      this.Bpecr1.ScrInfoPath = (object) this.strScrPath;
      this.strPrnPath = "c:\\bp_prn.txt";
      this.Bpecr1.PrnInfoPath = (object) this.strPrnPath;
      this.strFCTempPath = "c:\\bp_fc_temp.dat";
      this.Bpecr1.UseCP1251 = 1;
      this.RB_CP1251.Checked = true;
      this.RB_CP866.Checked = false;
    }

    private void CredtBtn_Click()
    {
      string str = this.GetPayValCredit.Text.Replace(".", "").Replace(",", "");
      if (Interaction.MsgBox((object) "Подтвердите оплату в кредит", MsgBoxStyle.YesNo, (object) ("ОПЛАТА " + this.GetPayValCredit.Text)) == MsgBoxResult.Yes)
      {
        this.AlreadyPayed += Convert.ToInt32(str);
        this.SumPriceCdts += Convert.ToInt32(str);
        this.AlreadyPayCredit += Convert.ToInt32(str);
        this.RefreshAlreadyPayeds();
      }
      this.DisableItemsByPayment();
      this.RefreshGetPayVals();
    }

    private void DiscountRequest_Click(object eventSender, EventArgs eventArgs)
    {
      try
      {
        string text = this.LabelStatus.Text;
        this.LabelStatus.Text = "Получение скидки...";
        this.LabelStatus.Refresh();
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.LabelStatus.Text = text;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 6) != 0L)
            return;
          this.PostProcessOperationId(false);
          this.LabelStatus.Text = text;
          if (this.Bpecr1.RetCode == 0)
          {
            this.OutputScrInfo();
            this.OutputPrnInfo();
            this.OutputCustomerInfo();
            this.Discount = this.GetDiscount();
            this.DiscountType = this.IsDiscountInMoney();
            if (this.DiscountType)
            {
              this.LabelDiscountRubl.Visible = true;
              this.SummaryPrice = !(this.SummaryPriceWithoutDiscount - (Decimal) this.Discount <= 0M) ? (int) (this.SummaryPriceWithoutDiscount - (Decimal) this.Discount) : 0;
            }
            else
            {
              this.LabelDiscountRubl.Visible = false;
              this.SummaryPrice = this.Discount >= 10000 || this.Discount < 0 ? 0 : (int) (this.SummaryPriceWithoutDiscount / 10000M * (Decimal) (10000 - this.Discount));
            }
            this.DiscountField.Text = string.Format("{0:#,#0.00}", (object) (this.Discount / 100));
            this.RefreshSummPrice();
            this.RefreshGetPayVals();
          }
          else
          {
            int retCode = this.Bpecr1.RetCode;
            if (retCode == 2 && this.bFisicalConnectToPOS)
            {
              this.Bpecr1.DisconnectFromPOS();
              this.bFisicalConnectToPOS = false;
              this.RestartServer();
            }
            ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Модификация чека'");
          }
          this.LabelStatus.Text = "Режим оплаты";
        }
      }
      catch (Exception ex)
      {
        this.LabelStatus.Text = "Режим оплаты";
        int num = (int) MessageBox.Show("Возникла ошибка при обработке операции!", "Модификация чека", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void FCChequeTimeButton_Click(object eventSender, EventArgs eventArgs)
    {
      DateTime dateTime = new DateTime();
      try
      {
        string str = Interaction.InputBox("Введите время в указанном формате", "Изменение времени", Convert.ToString(DateAndTime.Now));
        if (string.IsNullOrEmpty(str))
          return;
        this.Bpecr1.ChequeDate = Convert.ToDateTime(str);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка преобразования даты/времени! Проверьте правильность ввода и соответствие формату.", "Изменение времени", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void BEL_KeyPress(object eventSender, KeyPressEventArgs eventArgs)
    {
      int CharCode = Strings.Asc(eventArgs.KeyChar);
      string String1 = "йЙцЦуУкКеЕнНгГшШщЩзЗхХъЪфФыЫвВаАпПрРоОлЛдДжЖэЭяЯчЧсСмМиИтТьЬбБюЮ.,";
      string str = "qQwWeErRtTyYuUiIoOpP[{]}aAsSdDfFgGhHjJkKlL;;'_zZxXcCvVbBnNmM,<.>/?";
      string String2 = Strings.Chr(CharCode).ToString();
      int Start = Strings.InStr(String1, String2, CompareMethod.Text);
      if (Start != 0)
        String2 = Strings.Mid(str, Start, 1);
      if (this.Timer1.Enabled)
      {
        this.strMagTrack += String2;
        CharCode = 0;
        this.Timer1.Enabled = false;
        this.Timer1.Enabled = true;
      }
      else if (String2 == "%" || String2 == ";")
      {
        this.strMagTrack = String2;
        this.Timer1.Enabled = true;
        CharCode = 0;
      }
      eventArgs.KeyChar = Strings.Chr(CharCode);
      if (CharCode != 0)
        return;
      eventArgs.Handled = true;
    }

    [DllImport("kernel32", EntryPoint = "GetCommandLineA", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern string GetCommandLine();

    public string ParamStr(int index)
    {
      string str = (string) null;
      bool flag = false;
      int index1 = 0;
      string[] array = new string[index1 + 1];
      for (int Start = 1; Start <= Strings.Len(str); ++Start)
      {
        if (Strings.Mid(str, Start, 1) != " ")
          array[index1] = array[index1] + Strings.Mid(str, Start, 1);
        if (Strings.Mid(str, Start, 1) == Strings.Chr(34).ToString() && !flag)
          flag = true;
        else if (Strings.Mid(str, Start, 1) == Strings.Chr(34).ToString() && flag)
          flag = false;
        if (Strings.Mid(str, Start, 1) == " " && !flag)
        {
          ++index1;
          Array.Resize<string>(ref array, index1 + 1);
        }
      }
      return index <= index1 ? this.deleteQ(array[index]) : "";
    }

    public string deleteQ(string str)
    {
      string str1 = "";
      for (int Start = 1; Start <= Strings.Len(str); ++Start)
      {
        if (Strings.Mid(str, Start, 1) != Strings.Chr(34).ToString())
          str1 += Strings.Mid(str, Start, 1);
      }
      return str1;
    }

    private void OnFormLoad()
    {
      Bel.Instance = this;
      bool bIncSlipNumber = false;
      this.ReloadParams(ref bIncSlipNumber);
      this.SumPriceCash = 0;
      this.SumPriceCard = 0;
      this.SumPriceBnss = 0;
      this.SumPriceCdts = 0;
      this.SumPricePrep = 0;
      this.SummaryPrice = 0;
      this.AlreadyPayed = 0;
      this.AlreadyPayCard = 0;
      this.AlreadyPayCash = 0;
      this.AlreadyPayCredit = 0;
      this.AlreadyPayBonus = 0;
      this.AlreadyPayPrepaid = 0;
      this.AlreadyPayAnother = 0;
      this.AlreadyPayPreAuthSimple = 0;
      this.AlreadyPayPreAuthAnother = 0;
      this.AlreadyPayCommitAuthSimple = 0;
      this.AlreadyPayCommitAuthAnother = 0;
      this.bFisicalConnectToPOS = false;
      if (this.Bpecr1.AutoGenBPSID == 1)
      {
        this.CheckAutoGenBpSId.CheckState = CheckState.Checked;
        this.BpSId.Enabled = false;
      }
      else
      {
        this.CheckAutoGenBpSId.CheckState = CheckState.Unchecked;
        this.BpSId.Enabled = true;
      }
      if (this.Bpecr1.AutoGenECROpId == 1)
      {
        this.CheckAutoGenECROpId.CheckState = CheckState.Checked;
        this.ECROpId.Enabled = false;
        this.CheckAutoIncECROpId.Enabled = false;
      }
      else
      {
        this.CheckAutoGenECROpId.CheckState = CheckState.Unchecked;
        this.CheckAutoIncECROpId.CheckState = CheckState.Checked;
        this.CheckAutoIncECROpId.Enabled = true;
        this.ECROpId.Enabled = true;
      }
      this.PostProcessOperationId(true);
    }

    private void FormAppliance_Click(object eventSender, EventArgs eventArgs)
    {
      string str = (string) null;
      int code = 0;
      try
      {
        str = this.LabelStatus.Text;
        this.SetCardData();
        this.LabelStatus.Text = "Прием анкеты...";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) code, "Ошибка операции 'Прием анкеты'");
          this.LabelStatus.Text = str;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 13) != 0L)
          {
            this.Bpecr1.Cheque = this.TMPCheque;
          }
          else
          {
            this.PostProcessOperationId(false);
            this.Bpecr1.Cheque = this.TMPCheque;
            this.LabelStatus.Text = str;
            if (this.Bpecr1.RetCode == 0)
            {
              this.OutputScrInfo();
              this.OutputPrnInfo();
              this.OutputCustomerInfo();
            }
            else
            {
              int retCode = this.Bpecr1.RetCode;
              if (retCode == 2 && this.bFisicalConnectToPOS)
              {
                this.Bpecr1.DisconnectFromPOS();
                this.bFisicalConnectToPOS = false;
                this.RestartServer();
              }
              ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Прием анкеты'");
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Прием анкеты", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str;
      }
    }

    private void MerchRrnCancel_Click(object eventSender, EventArgs eventArgs)
    {
      string str = (string) null;
      try
      {
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        str = this.LabelStatus.Text;
        string text = this.MerchRrnField.Text;
        int num1 = Strings.Len(text);
        if (num1 != 12)
        {
          int num2 = (int) MessageBox.Show("Длина RRN должна быть равна 12. Проверьте правильность ввода RRN.");
          if (num1 >= 12)
            return;
          this.MerchRrnField.Text = Strings.Mid("000000000000", 1, 12 - num1) + text;
        }
        else
        {
          this.Bpecr1.RRN = (object) text;
          this.Bpecr1.Amount = Convert.ToInt32(this.AmountForCancelEMV.Text.Replace(".", "").Replace(",", ""));
          this.LabelStatus.Text = "Отмена операции оплаты МПС...";
          this.SetCardData();
          this.Bpecr1.Flags = (short) 0;
          if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
          {
            this.LabelStatus.Text = str;
          }
          else
          {
            this.TMPCheque = this.Bpecr1.Cheque;
            this.PreProcessOperationId();
            if (this.WrapPerformPosOperations((short) 12) != 0L)
            {
              this.Bpecr1.Cheque = this.TMPCheque;
            }
            else
            {
              this.PostProcessOperationId(false);
              this.Bpecr1.Cheque = this.TMPCheque;
              if (this.Bpecr1.RetCode == 0)
              {
                this.OutputScrInfo();
                this.OutputPrnInfo();
                this.OutputCustomerInfo();
              }
              else
              {
                int retCode = this.Bpecr1.RetCode;
                if (retCode == 2 && this.bFisicalConnectToPOS)
                {
                  this.Bpecr1.DisconnectFromPOS();
                  this.bFisicalConnectToPOS = false;
                  this.RestartServer();
                }
                ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Отмена по RRN'");
              }
              this.LabelStatus.Text = str;
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Отмена МПС по RRN", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str;
      }
    }

    private void NewOrder_Click(object eventSender, EventArgs eventArgs)
    {
      bool bButtonPressed = false;
      this.ResetContext(ref bButtonPressed);
      this.Bpecr1.RenewBpSId();
      this.PostProcessOperationId(true);
      if (this.CheckBox_RestartServer.CheckState == CheckState.Checked && this.g_WasStartServer)
        this.ButtonStartServer_Click_1(eventSender, eventArgs);
      this.NewOrder.Visible = false;
    }

    private void GetListGoods(List<string> listGood, List<string> listQuantity)
    {
      string text1 = this.GoodsCode.Text;
      string text2 = this.GoodsQuantity.Text;
      string[] strArray1 = text1.Split(Strings.ChrW(13), Strings.ChrW(10));
      string[] strArray2 = text2.Split(Strings.ChrW(13), Strings.ChrW(10));
      foreach (string str in strArray1)
      {
        if (!string.IsNullOrEmpty(str.Trim()))
          listGood.Add(str);
      }
      foreach (string str in strArray2)
      {
        if (!string.IsNullOrEmpty(str.Trim()))
          listQuantity.Add(str);
      }
      this.GoodsAmount.Text = "";
      this.GoodsPrices.Text = "";
      this.GoodsQuantity.Text = "";
      this.GoodsNames.Text = "";
      this.GoodsCode.Text = "";
    }

    private void AddAllGoods()
    {
      this.GetListGoods(new List<string>(), new List<string>());
      this.SummaryPriceWithoutDiscount = 0M;
      this.Bpecr1.Amount = 0;
      this.Bpecr1.Discount = 0;
      this.Bpecr1.RemoveAllItemsFromCheque();
      this.ClearGoodFileds();
      if (this.GoodsList.Count <= 0)
        return;
      foreach (GoodsInfo goods in this.GoodsList)
        this.AddGoods(goods);
    }

    private string ReplaceBadCharacters(string input) => input.Replace('№', 'N').Replace('/', '|');

    public bool AddGoods(GoodsInfo goods)
    {
      try
      {
        this.Bpecr1.goodCode = (object) goods.BarCode;
        this.Bpecr1.goodFlags = goods.Flags;
        this.Bpecr1.goodName = (object) this.ReplaceBadCharacters(goods.Name);
        this.Bpecr1.goodQuantity = (int) (goods.Quantity * 1000M);
        this.Bpecr1.goodPrice = (int) (goods.Price * 100M);
        this.Bpecr1.goodSummPrice = (int) (goods.Price * 100M * goods.Quantity);
        this.Bpecr1.AddGood();
        if (this.Bpecr1.RetCode != 0)
        {
          int num = (int) MessageBox.Show("Не удалось добавить товар в бонусную систему", "Добавление товара", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, string.Empty);
        }
        else
        {
          TextBox goodsCode = this.GoodsCode;
          goodsCode.Text = goodsCode.Text + goods.BarCode + "\r\n";
          TextBox goodsNames = this.GoodsNames;
          goodsNames.Text = goodsNames.Text + goods.Name + "\r\n";
          TextBox goodsPrices = this.GoodsPrices;
          goodsPrices.Text = goodsPrices.Text + string.Format("{0:#,#0.00}", (object) goods.Price) + "\r\n";
          TextBox goodsQuantity = this.GoodsQuantity;
          goodsQuantity.Text = goodsQuantity.Text + string.Format("{0:#,#0.000}", (object) goods.Quantity) + "\r\n";
          TextBox goodsAmount = this.GoodsAmount;
          goodsAmount.Text = goodsAmount.Text + string.Format("{0:#,#0.00}", (object) (goods.Quantity * goods.Price)) + "\r\n";
          this.BarCoder.Text = "";
          this.CoodCount.Text = "1.000";
          this.SummaryPriceWithoutDiscount += goods.Price * goods.Quantity * 100M;
          this.Bpecr1.Amount = (int) this.SummaryPriceWithoutDiscount;
          this.Bpecr1.ChequeSummPrice = (int) this.SummaryPriceWithoutDiscount;
          this.RefreshSummPriceWithoutDiscount();
          this.AmountForCancel.Text = this.SummPriceWithoutDiscount.Text;
          this.IsApplyEnabled = true;
          this.IsPromoEnabled = true;
          ++this.GoodsCount;
        }
        this.BarCoder.Focus();
        return true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Добавление товара", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
      }
    }

    private void PerformPayment(ref short PayType, ref short Flags)
    {
      try
      {
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        string text = this.GetPayValAnother.Text;
        string str1 = "";
        short num = PayType;
        this.Bpecr1.RRN = (object) str1;
        switch (PayType)
        {
          case 2:
            text = this.GetPayValCard.Text;
            break;
          case 4:
            text = this.GetPayValBonuses.Text;
            break;
          case 8:
            text = this.GetPayValPrepaid.Text;
            break;
          case 10:
            text = this.GetPayValPreAuthorization.Text;
            break;
          case 11:
            text = this.GetPayValAnotherPreAuthorization.Text;
            break;
          case 12:
            text = this.GetPayValCommitAuthorization.Text;
            this.Bpecr1.RRN = (object) this.TextRRNSimple.Text;
            break;
          case 13:
            text = this.GetPayValCommitAuthorization.Text;
            this.Bpecr1.RRN = (object) this.TextRRNSimple.Text;
            break;
          case 14:
            text = this.GetPayValAnotherCommitAuthorization.Text;
            this.Bpecr1.RRN = (object) this.TextRRNAnother.Text;
            break;
          case 99:
            text = this.GetPayValAnotherCommitAuthorization.Text;
            this.Bpecr1.RRN = (object) this.TextRRNAnother.Text;
            num = (short) 12;
            break;
        }
        string str2 = text.Replace(".", "").Replace(",", "");
        this.LabelStatus.Text = "Ожидайте...";
        this.LabelStatus.Refresh();
        this.Bpecr1.Amount = Convert.ToInt32(str2);
        this.Bpecr1.PayType = (int) num;
        this.Bpecr1.Flags = Flags;
        this.SetCardData();
        this.LabelStatus.Text = "Режим оплаты";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
          return;
        this.TMPCheque = this.Bpecr1.Cheque;
        this.PreProcessOperationId();
        if (this.WrapPerformPosOperations((short) 3) != 0L)
        {
          this.Bpecr1.Cheque = this.TMPCheque;
        }
        else
        {
          this.PostProcessOperationId(false);
          this.Bpecr1.Cheque = this.TMPCheque;
          if (this.Bpecr1.RetCode == 0)
          {
            this.AlreadyPayed += this.Bpecr1.Amount;
            int iParamNumber1 = 20;
            switch (PayType)
            {
              case 2:
                this.SumPriceCard += this.Bpecr1.Amount;
                this.AlreadyPayCard += this.Bpecr1.Amount;
                break;
              case 3:
                this.SumPriceCard += this.Bpecr1.Amount;
                this.AlreadyPayAnother += this.Bpecr1.Amount;
                break;
              case 4:
                this.SumPriceBnss += this.Bpecr1.Amount;
                this.AlreadyPayBonus += this.Bpecr1.Amount;
                break;
              case 5:
                this.SumPriceBnss += this.Bpecr1.Amount;
                this.AlreadyPayAnother += this.Bpecr1.Amount;
                break;
              case 6:
                this.SumPriceCard += this.Bpecr1.Amount;
                this.AlreadyPayAnother += this.Bpecr1.Amount;
                break;
              case 8:
                this.AlreadyPayPrepaid += this.Bpecr1.Amount;
                this.SumPricePrep += this.Bpecr1.Amount;
                break;
              case 9:
                this.AlreadyPayAnother += this.Bpecr1.Amount;
                this.SumPricePrep += this.Bpecr1.Amount;
                break;
              case 10:
                this.AlreadyPayPreAuthSimple += this.Bpecr1.Amount;
                this.TextRRNSimple.Text = this.GetParamFromAddFile(ref iParamNumber1);
                this.SumPriceCard += this.Bpecr1.Amount;
                break;
              case 11:
                this.AlreadyPayPreAuthAnother += this.Bpecr1.Amount;
                this.TextRRNAnother.Text = this.GetParamFromAddFile(ref iParamNumber1);
                this.SumPriceCard += this.Bpecr1.Amount;
                break;
            }
            int iParamNumber2 = 19;
            string paramFromAddFile = this.GetParamFromAddFile(ref iParamNumber2);
            if (!string.IsNullOrEmpty(paramFromAddFile))
            {
              this.LabelClientCardID.Text = paramFromAddFile;
              this.LabelClientCardID.Visible = true;
              this.Frame22.Visible = true;
              this.Label48.Visible = true;
            }
            else
            {
              this.LabelClientCardID.Visible = false;
              this.Frame22.Visible = false;
              this.Label48.Visible = false;
            }
            this.RefreshAlreadyPayeds();
            this.OutputScrInfo();
            this.OutputPrnInfo();
            this.OutputCustomerInfo();
          }
          else
          {
            if (this.Bpecr1.RetCode == 2 && this.bFisicalConnectToPOS)
            {
              this.Bpecr1.DisconnectFromPOS();
              this.bFisicalConnectToPOS = false;
              this.RestartServer();
            }
            ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка операции 'Оплата'");
          }
          this.RefreshGetPayVals();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Оплата", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void PayAnotherBonuses_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short PayType = 5;
      short Flags = 0;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayAnotherCard_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short PayType = 3;
      short Flags = 0;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayAnotherCommitAuthorization_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short PayType = 99;
      short Flags = 0;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayCredit_Click(object eventSender, EventArgs eventArgs)
    {
      try
      {
        string str = this.GetPayValCredit.Text.Replace(".", "").Replace(",", "");
        if (Interaction.MsgBox((object) "Подтвердите оплату в кредит", MsgBoxStyle.YesNo, (object) ("ОПЛАТА " + this.GetPayVal.Text)) == MsgBoxResult.Yes)
        {
          this.AlreadyPayed += Convert.ToInt32(str);
          this.SumPriceCdts += Convert.ToInt32(str);
          this.AlreadyPayCredit += Convert.ToInt32(str);
          this.RefreshAlreadyPayeds();
        }
        this.DisableItemsByPayment();
        this.RefreshGetPayVals();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Оплата в кредит", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void RefundAnother_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short PayType = 14;
      short Flags = 0;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void RefundSimple_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 13;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayAnotherPreAuthorization_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 11;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayBonuses_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 4;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayCard_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 2;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayCardWOReg_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short PayType = 6;
      short Flags = 8;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayCash_Click(object eventSender, EventArgs eventArgs)
    {
      try
      {
        string str = this.GetPayVal.Text.Replace(".", "").Replace(",", "");
        if (Interaction.MsgBox((object) "Подтвердите оплату наличными", MsgBoxStyle.YesNo, (object) ("ОПЛАТА " + this.GetPayVal.Text)) == MsgBoxResult.Yes)
        {
          this.AlreadyPayed += Convert.ToInt32(str);
          this.SumPriceCash += Convert.ToInt32(str);
          this.AlreadyPayCash += Convert.ToInt32(str);
          this.PostProcessOperationId(false);
          this.RefreshAlreadyPayeds();
        }
        this.DisableItemsByPayment();
        this.RefreshGetPayVals();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Оплата наличными", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void SetDefCardDataByType(int cardType)
    {
      this.ComboBoxCardType.SelectedIndex = cardType - 1;
      if (cardType == 1 || cardType == 2 || cardType > 3 && cardType < 11)
      {
        this.MagTrack1Field.Enabled = false;
        this.MagTrack2Field.Enabled = false;
        this.MagTrack1Field.Text = "";
        this.MagTrack2Field.Text = "";
      }
      else
      {
        this.MagTrack1Field.Enabled = true;
        this.MagTrack2Field.Enabled = true;
        this.MagTrack1Field.Text = "";
        this.MagTrack2Field.Text = "3737373737373737373737";
      }
    }

    private void ClearGoodFileds()
    {
      this.Bpecr1.goodCode = (object) "";
      this.Bpecr1.goodName = (object) "";
      this.Bpecr1.goodCategory = (object) "";
      this.Bpecr1.goodPrice = 0;
      this.Bpecr1.goodQuantity = 0;
      this.Bpecr1.goodSummPrice = 0;
      this.Bpecr1.goodDiscount = 0;
      this.Bpecr1.goodFlags = (short) 0;
      this.Bpecr1.goodBonusPrice = 0;
      this.Bpecr1.goodBonusDiscount = 0;
      this.Bpecr1.goodBonusDiscountPrice = 0;
    }

    private void ResetContext(ref bool bButtonPressed)
    {
      this.LabelStatus.Text = "Режим готовности";
      this.MakePaymentsInvisible();
      this.StornoCheck.Enabled = true;
      this.IsApplyEnabled = false;
      this.IsPromoEnabled = false;
      this.LabeAlreadyPayed.Visible = false;
      this.LabelForLabelAlreadyPayed.Visible = false;
      this.SetDiscount.Visible = false;
      this.DiscountRequest.Visible = false;
      this.DiscountField.Visible = true;
      this.LabelDiscountRubl.Visible = false;
      this.BpRrnCancel.Enabled = true;
      this.LabelDiscountRubl.Visible = true;
      this.LabelClientCardID.Visible = false;
      this.Frame22.Visible = false;
      this.Label48.Visible = false;
      this.Frame14.Visible = false;
      this.LabelGetPayVal.Visible = true;
      this.GetPayVal.Visible = true;
      this.PayCash.Visible = true;
      this.LabelGetPayValCard.Visible = true;
      this.GetPayValCard.Visible = true;
      this.PayCard.Visible = true;
      this.LabelGetPayValBonuses.Visible = true;
      this.GetPayValBonuses.Visible = true;
      this.PayBonuses.Visible = true;
      this.LabelGetPayValCredit.Visible = true;
      this.GetPayValCredit.Visible = true;
      this.PayCredit.Visible = true;
      this.LabelGetPayValPrepaid.Visible = true;
      this.GetPayValPrepaid.Visible = true;
      this.PayPrepaid.Visible = true;
      this.SummPriceWithoutDiscount.Visible = true;
      this.LabelSummPrice.Visible = true;
      this.LabelForLabelAlreadyPayed.Visible = true;
      this.LabeAlreadyPayed.Visible = true;
      this.CloseFC.Visible = true;
      this.FCChequeTimeButton.Visible = true;
      this.SummaryPrice = 0;
      this.SummaryPriceCash = 0;
      this.SummaryPriceCredit = 0;
      this.SummaryPriceCard = 0;
      this.SummaryPriceBonus = 0;
      this.SummaryPricePrepaid = 0;
      this.SummaryPriceAnother = 0;
      this.SummaryPriceWithoutDiscount = 0M;
      this.SumPriceCash = 0;
      this.SumPriceCard = 0;
      this.SumPriceBnss = 0;
      this.SumPriceCdts = 0;
      this.SumPricePrep = 0;
      this.Discount = 0;
      this.AlreadyPayed = 0;
      this.AlreadyPayCash = 0;
      this.AlreadyPayCredit = 0;
      this.AlreadyPayCard = 0;
      this.AlreadyPayBonus = 0;
      this.AlreadyPayPrepaid = 0;
      this.AlreadyPayAnother = 0;
      this.AlreadyPayPreAuthSimple = 0;
      this.AlreadyPayPreAuthAnother = 0;
      this.RefreshSummPrice();
      this.RefreshAlreadyPayeds();
      this.GoodsCount = 0;
      this.BarCoder.Text = "";
      this.CoodCount.Text = "1.000";
      this.StornoCheck.CheckState = CheckState.Unchecked;
      this.GoodsCode.Text = "";
      this.GoodsNames.Text = "";
      this.GoodsPrices.Text = "";
      this.GoodsQuantity.Text = "";
      this.GoodsAmount.Text = "";
      this.ScreenText.Text = "";
      this.PrnText.Text = "";
      this.CustomDesplayMessageText.Text = "";
      this.GetPayVal.Text = "0,00";
      this.SummPriceWithoutDiscount.Text = "0,00";
      this.DiscountField.Text = "0,00";
      this.BpRrnField.Text = "0";
      this.AmountForCancel.Text = "0,00";
      this.LabelBonusBalance.Text = "0,00";
      this.Frame26.Visible = false;
      this.LabelBonusBalance.Text = "0,00";
      this.CheckAutoGenBpSId.CheckState = this.Bpecr1.AutoGenBPSID == 1 ? CheckState.Checked : CheckState.Unchecked;
      this.CheckAutoGenECROpId.CheckState = this.Bpecr1.AutoGenECROpId == 1 ? CheckState.Checked : CheckState.Unchecked;
      this.Bpecr1.Amount = 0;
      this.Bpecr1.Discount = 0;
      this.Bpecr1.BPRRN = (object) "";
      if (this.Check_UseKLProtocol.Checked)
        this.Bpecr1.UseContextlessProtocol = 1;
      else
        this.Bpecr1.UseContextlessProtocol = 0;
      if (this.bFisicalConnectToPOS)
        this.Bpecr1.DisconnectFromPOS();
      this.bFisicalConnectToPOS = false;
      this.Check_UseKLProtocol.Enabled = true;
      this.Bpecr1.StopServerMode();
      this.ButtonStartServer.Enabled = true;
      this.Bpecr1.RemoveAllItemsFromCheque();
      this.ClearGoodFileds();
      if (bButtonPressed)
      {
        this.Bpecr1.ResetContext();
        if (this.Bpecr1.RetCode != 0)
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, string.Empty);
      }
      else if (this.Bpecr1.RetCode != 0)
        ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, string.Empty);
      bool bIncSlipNumber = true;
      this.ReloadParams(ref bIncSlipNumber);
      this.PostProcessOperationId(true);
      this.BarCoder.Focus();
      this.GoodsList.Clear();
    }

    private void SetCardData()
    {
      if (this.Check_TransmitCardData.CheckState == CheckState.Checked)
      {
        this.Bpecr1.CardType = this.ComboBoxCardType.SelectedIndex + 1;
        this.Bpecr1.MagTrack1 = (object) this.MagTrack1Field.Text;
        this.Bpecr1.MagTrack2 = (object) this.MagTrack2Field.Text;
      }
      else
      {
        this.Bpecr1.CardType = 0;
        this.Bpecr1.MagTrack1 = (object) "";
        this.Bpecr1.MagTrack2 = (object) "";
      }
    }

    private void PayCommitAuthorization_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short PayType = 12;
      short Flags = 0;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayPreAuthorization_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 10;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayPrepaid_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 8;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PayPrepaidAnother_Click(object eventSender, EventArgs eventArgs)
    {
      this.DisableItemsByPayment();
      short Flags = 0;
      short PayType = 9;
      this.PerformPayment(ref PayType, ref Flags);
    }

    private void PrepaidCharge_Click(object eventSender, EventArgs eventArgs)
    {
      string str = (string) null;
      try
      {
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        str = this.LabelStatus.Text;
        this.Bpecr1.Amount = Convert.ToInt32(this.AmountPrepaid.Text.Replace(".", "").Replace(",", ""));
        this.SetCardData();
        this.LabelStatus.Text = "Пополнение...";
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.LabelStatus.Text = str;
        }
        else
        {
          this.TMPCheque = this.Bpecr1.Cheque;
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 7) != 0L)
          {
            this.Bpecr1.Cheque = this.TMPCheque;
          }
          else
          {
            this.PostProcessOperationId(false);
            this.Bpecr1.Cheque = this.TMPCheque;
            this.LabelStatus.Text = str;
            if (this.Bpecr1.RetCode == 0)
            {
              this.OutputScrInfo();
              this.OutputPrnInfo();
              this.OutputCustomerInfo();
              int num = (int) MessageBox.Show("Операция выполнена успешно!", "Пополнение предоплаченного счета", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
              int retCode = this.Bpecr1.RetCode;
              if (retCode == 2 && this.bFisicalConnectToPOS)
              {
                this.Bpecr1.DisconnectFromPOS();
                this.bFisicalConnectToPOS = false;
                this.RestartServer();
              }
              ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка операции 'Пополнение предоплаченного счета'");
            }
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции. Необходим повторный запуск системы", "Пополнение предоплаченного счета", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = str;
      }
    }

    private void StartServer()
    {
      this.PrepareCommunications();
      this.g_WasStartServer = false;
      this.Bpecr1.StartServerMode();
      if (this.Bpecr1.RetCode != 0)
        return;
      this.ButtonStartServer.Enabled = false;
      this.g_WasStartServer = true;
    }

    private void RestartServer()
    {
      if (this.CheckBox_RestartServer.CheckState != CheckState.Checked || !this.g_WasStartServer)
        return;
      this.Bpecr1.StopServerMode();
      this.StartServer();
    }

    private void ResetBtn_Click(object eventSender, EventArgs eventArgs) => this.Reset();

    private void SetDiscount_Click(object eventSender, EventArgs eventArgs)
    {
      try
      {
        this.Discount = Convert.ToInt32(this.DiscountField.Text.Replace(".", "").Replace(",", ""));
        if (this.DiscountType)
        {
          if (this.SummaryPriceWithoutDiscount - (Decimal) this.Discount <= 0M)
            this.Discount = (int) this.SummaryPriceWithoutDiscount;
          this.SummaryPrice = (int) (this.SummaryPriceWithoutDiscount - (Decimal) this.Discount);
        }
        else
        {
          if (this.Discount >= 10000 || this.Discount < 0)
            this.Discount = 10000;
          this.SummaryPrice = (int) (this.SummaryPriceWithoutDiscount / 10000M * (Decimal) (10000 - this.Discount));
        }
        this.DiscountField.Text = string.Format("{0:#,#0.00}", (object) (this.Discount / 100));
        this.RefreshSummPrice();
        this.RefreshGetPayVals();
      }
      catch (Exception ex)
      {
        this.Discount = 0;
        this.DiscountField.Text = string.Format("{0:#,#0.00}", (object) (this.Discount / 100));
        this.DiscountField.Text = "0";
        this.RefreshSummPrice();
        this.RefreshGetPayVals();
      }
    }

    private void MakePaymentsVisible()
    {
      this.Frame6.Visible = true;
      this.Frame7.Visible = true;
      this.Frame8.Visible = true;
      this.Frame9.Visible = true;
      this.Frame10.Visible = true;
      this.Frame12.Visible = true;
      this.Frame15.Visible = true;
      this.LabelForLabelAlreadyPayed.Visible = true;
      this.LabeAlreadyPayed.Visible = true;
      this.CloseFC.Visible = true;
    }

    private void MakePaymentsInvisible()
    {
      this.Frame6.Visible = false;
      this.Frame7.Visible = false;
      this.Frame8.Visible = false;
      this.Frame9.Visible = false;
      this.Frame10.Visible = false;
      this.Frame12.Visible = false;
      this.LabelForLabelAlreadyPayed.Visible = false;
      this.LabeAlreadyPayed.Visible = false;
      this.CloseFC.Visible = false;
    }

    private void MakeAlreadyPaymentsVisible()
    {
      this.Frame6.Visible = true;
      this.Frame7.Visible = true;
      this.Frame8.Visible = true;
      this.Frame9.Visible = true;
      this.Frame10.Visible = true;
      this.LabelGetPayVal.Visible = false;
      this.GetPayVal.Visible = false;
      this.PayCash.Visible = false;
      this.LabelGetPayValCard.Visible = false;
      this.GetPayValCard.Visible = false;
      this.PayCard.Visible = false;
      this.LabelGetPayValBonuses.Visible = false;
      this.GetPayValBonuses.Visible = false;
      this.PayBonuses.Visible = false;
      this.LabelGetPayValCredit.Visible = false;
      this.GetPayValCredit.Visible = false;
      this.PayCredit.Visible = false;
      this.LabelGetPayValPrepaid.Visible = false;
      this.GetPayValPrepaid.Visible = false;
      this.PayPrepaid.Visible = false;
      this.SummPriceWithoutDiscount.Visible = false;
      this.LabelSummPrice.Visible = false;
      this.LabelForLabelAlreadyPayed.Visible = true;
      this.LabeAlreadyPayed.Visible = true;
      this.CloseFC.Visible = false;
      this.FCChequeTimeButton.Visible = false;
    }

    public long BPRRN => this._bprrn;

    private void FillAlreadyPayedFromAddFile()
    {
      int iParamNumber1 = 9;
      string paramFromAddFile1 = this.GetParamFromAddFile(ref iParamNumber1);
      this._bprrn = !string.IsNullOrEmpty(paramFromAddFile1) ? Convert.ToInt64(paramFromAddFile1) : 0L;
      int iParamNumber2 = 3;
      string paramFromAddFile2 = this.GetParamFromAddFile(ref iParamNumber2);
      this.AlreadyPayCash = !string.IsNullOrEmpty(paramFromAddFile2) ? Convert.ToInt32(paramFromAddFile2) : 0;
      int iParamNumber3 = 10;
      string paramFromAddFile3 = this.GetParamFromAddFile(ref iParamNumber3);
      this.AlreadyPayCard = !string.IsNullOrEmpty(paramFromAddFile3) ? Convert.ToInt32(paramFromAddFile3) : 0;
      int iParamNumber4 = 11;
      string paramFromAddFile4 = this.GetParamFromAddFile(ref iParamNumber4);
      this.AlreadyPayBonus = string.IsNullOrEmpty(paramFromAddFile4) ? 0 : Convert.ToInt32(paramFromAddFile4);
      int iParamNumber5 = 13;
      string paramFromAddFile5 = this.GetParamFromAddFile(ref iParamNumber5);
      this.AlreadyPayCredit = !string.IsNullOrEmpty(paramFromAddFile5) ? Convert.ToInt32(paramFromAddFile5) : 0;
      int iParamNumber6 = 16;
      string paramFromAddFile6 = this.GetParamFromAddFile(ref iParamNumber6);
      this.AlreadyPayPrepaid = !string.IsNullOrEmpty(paramFromAddFile6) ? Convert.ToInt32(paramFromAddFile6) : 0;
      this.AlreadyPayed = this.AlreadyPayCash + this.AlreadyPayBonus + this.AlreadyPayCard + this.AlreadyPayCredit + this.AlreadyPayPrepaid;
      this.RefreshAlreadyPayeds();
    }

    private bool ModifyCheque()
    {
      bool flag = false;
      try
      {
        flag = false;
        this.bChequeWasModified = false;
        DialogCheque.DefInstance.TextGoodCode.Text = this.GoodsCode.Text;
        DialogCheque.DefInstance.TextGoodName.Text = this.GoodsNames.Text;
        DialogCheque.DefInstance.TextGoodPrice.Text = this.GoodsPrices.Text;
        DialogCheque.DefInstance.TextGoodQuantity.Text = this.GoodsQuantity.Text;
        DialogCheque.DefInstance.TextGoodAmount.Text = this.GoodsAmount.Text;
        DialogCheque.DefInstance.TextSourceChequeAmount.Text = this.SummPriceWithoutDiscount.Text;
        DialogCheque.DefInstance.ClearModifCheque();
        DialogCheque.DefInstance.PrnMessage.Text = "";
        DialogCheque.DefInstance.ScrMessage.Text = "";
        DialogCheque.DefInstance.bPaymentTypeBeforeModification = this.Check_ModificationBeforePayments.CheckState == CheckState.Unchecked;
        DialogCheque.DefInstance.bChequeWasModified = false;
        DialogCheque.DefInstance.bFirstTime = true;
        DialogCheque.DefInstance.Check_DiscountByGoods.CheckState = this.Check_DiscountByGoods.CheckState;
        if (Convert.ToBoolean(Convert.ToString(DialogCheque.DefInstance.ModifyCheque())))
        {
          if (DialogCheque.DefInstance.bChequeWasModified)
          {
            this.SummaryPriceCard = this.Bpecr1.ChequePerformedPaymentCard;
            this.GetPayValCard.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPriceCard / 100));
            int summaryPriceCard = this.SummaryPriceCard;
            this.SummaryPriceCredit = this.Bpecr1.ChequePerformedPaymentCredit;
            this.GetPayValCredit.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPriceCredit / 100));
            int num1 = summaryPriceCard + this.SummaryPriceCredit;
            this.SummaryPriceBonus = this.Bpecr1.ChequePerformedPaymentBonuses;
            this.GetPayValBonuses.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPriceBonus / 100));
            int num2 = num1 + this.SummaryPriceBonus;
            this.SummaryPricePrepaid = this.Bpecr1.ChequePerformedPaymentPrepaid;
            this.GetPayValPrepaid.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPricePrepaid / 100));
            int num3 = num2 + this.SummaryPricePrepaid;
            this.SummaryPriceCash = this.Bpecr1.ChequePerformedPaymentCash;
            this.GetPayVal.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPriceCash / 100));
            if (num3 + this.SummaryPriceCash == 0)
            {
              this.SummaryPriceCash = this.Bpecr1.Amount;
              this.GetPayVal.Text = string.Format("{0:#,#0.00}", (object) (this.SummaryPriceCash / 100));
            }
            this.SummaryPriceWithoutDiscount = (Decimal) this.Bpecr1.Amount;
          }
          else
            this.SummaryPriceCash = (int) this.SummaryPriceWithoutDiscount;
          this.SummaryPriceAnother = 0;
          return true;
        }
        ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка выполнения операции 'Модификация чека'");
        return flag;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Модификация чека", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return flag;
      }
    }

    public bool ConvertMagTrack(
      ref string RawTrack,
      ref string Track1,
      ref string Track2,
      ref string Track3)
    {
      string[] strArray = Strings.Split(RawTrack, "?", 3, CompareMethod.Text);
      Track1 = strArray[0];
      Track2 = strArray[1];
      if (Information.UBound((Array) strArray) > 2)
        Track3 = strArray[2];
      bool flag;
      if (Strings.Left(Track1, 1) == ";")
      {
        if (Strings.Len(Track2) > 0)
        {
          if (Strings.Left(Track2, 1) == "+")
          {
            Track3 = Strings.Mid(Track2, 2, 10000);
            Track2 = Strings.Mid(Track1, 2, 10000);
            Track1 = "";
            flag = true;
            goto label_13;
          }
        }
        else
        {
          Track2 = Strings.Mid(Track1, 2, 10000);
          Track3 = "";
          Track1 = "";
          flag = true;
          goto label_13;
        }
      }
      else if (Strings.Left(Track1, 1) == "%" && Strings.Left(Track2, 1) == ";")
      {
        if (Strings.Len(Track3) > 0)
        {
          if (Strings.Left(Track3, 1) == "+")
          {
            Track1 = Strings.Mid(Track1, 2, 10000);
            Track2 = Strings.Mid(Track2, 2, 10000);
            Track3 = Strings.Mid(Track3, 2, 10000);
            flag = true;
            goto label_13;
          }
        }
        else
        {
          Track1 = Strings.Mid(Track1, 2, 10000);
          Track2 = Strings.Mid(Track2, 2, 10000);
          Track3 = "";
          flag = true;
          goto label_13;
        }
      }
      flag = false;
label_13:
      if (flag)
        flag = Strings.StrComp(Track1, "E!!", CompareMethod.Text) != 0;
      if (flag)
        flag = Strings.StrComp(Track2, "E!!", CompareMethod.Text) != 0;
      if (flag)
        flag = Strings.StrComp(Track3, "E!!", CompareMethod.Text) != 0;
      return flag;
    }

    [Obsolete("Используйте Utils.Str2Hex(string input)")]
    public string Str2Hex(ref string str_Renamed)
    {
      string str = "";
      for (int Start = 1; Start <= Strings.Len(str_Renamed); ++Start)
        str += Conversion.Hex(Strings.Asc(Strings.Mid(str_Renamed, Start, 1)));
      return str;
    }

    private void Timer1_Tick(object eventSender, EventArgs eventArgs)
    {
      string Track1 = "";
      string Track2 = "";
      string Track3 = "";
      this.Timer1.Enabled = false;
      if (!this.ConvertMagTrack(ref this.strMagTrack, ref Track1, ref Track2, ref Track3))
        return;
      this.MagTrack1Field.Text = Utils.Str2Hex(Track1);
      this.MagTrack2Field.Text = Utils.Str2Hex(Track2);
    }

    private void CheckAutoGenBpSId_CheckedChanged(object sender, EventArgs e)
    {
      if (this.CheckAutoGenBpSId.Checked)
      {
        this.Bpecr1.AutoGenBPSID = 1;
        this.BpSId.Text = this.Bpecr1.BPSID as string;
        this.BpSId.Refresh();
        this.BpSId.Enabled = false;
      }
      else
      {
        this.Bpecr1.AutoGenBPSID = 0;
        this.Bpecr1.BPSID = (object) this.BpSId.Text;
        this.BpSId.Enabled = true;
      }
    }

    private void CheckAutoGenECROpId_CheckedChanged(object sender, EventArgs e)
    {
      if (this.CheckAutoGenECROpId.Checked)
      {
        this.Bpecr1.AutoGenECROpId = 1;
        this.ECROpId.Text = Conversion.Str((object) this.Bpecr1.ECROpId);
        this.ECROpId.Refresh();
        this.ECROpId.Enabled = false;
        this.CheckAutoIncECROpId.Enabled = false;
      }
      else
      {
        this.Bpecr1.AutoGenECROpId = 0;
        this.Bpecr1.ECROpId = (short) Convert.ToInt32(Conversion.Val(this.ECROpId.Text));
        this.ECROpId.Enabled = true;
        this.CheckAutoIncECROpId.Enabled = true;
        this.CheckAutoIncECROpId.CheckState = CheckState.Checked;
      }
    }

    private void Check_UseKLProtocol_CheckedChanged(object sender, EventArgs e)
    {
      if (this.Check_UseKLProtocol.Checked)
      {
        this.FrameOpId.Visible = true;
        this.ButtonRollback.Visible = true;
        this.Check_IntegralProt.Enabled = true;
      }
      else
      {
        this.FrameOpId.Visible = false;
        this.ButtonRollback.Visible = false;
        this.Check_IntegralProt.Enabled = false;
      }
      if (this.Check_UseKLProtocol.Checked && this.Check_IntegralProt.Checked)
        this.InfoBtn.Enabled = true;
      else
        this.InfoBtn.Enabled = false;
    }

    public bool OnPerformRollback(string BPSID, string ECROpId)
    {
      DialogRollback dialogRollback = new DialogRollback();
      int code = 0;
      if (!this.Check_UseKLProtocol.Checked)
      {
        int num = (int) MessageBox.Show("Не поддерживается при работе по классическомку протоколу!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        return false;
      }
      string text = this.LabelStatus.Text;
      try
      {
        if (string.IsNullOrEmpty(BPSID) || string.IsNullOrEmpty(ECROpId))
        {
          if (dialogRollback.ShowDialog() == DialogResult.OK)
          {
            this.Bpecr1.BPSIDOrig = (object) dialogRollback.BpSIdOrig.Text;
            this.Bpecr1.ECROpIdOrig = (short) Convert.ToInt32(Conversion.Val(dialogRollback.ECROpIdOrig.Text));
          }
          else
          {
            int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
          }
        }
        else
        {
          this.Bpecr1.BPSIDOrig = (object) BPSID;
          this.Bpecr1.ECROpIdOrig = (short) Convert.ToInt32(Conversion.Val(ECROpId));
        }
        this.LabelStatus.Text = "Rollback. Ожидайте...";
        this.Refresh();
        this.SetCardData();
        if (this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) code, "Ошибка операции 'Rollback'");
          this.LabelStatus.Text = text;
          return false;
        }
        this.TMPCheque = this.Bpecr1.Cheque;
        this.PreProcessOperationId();
        if (this.WrapPerformPosOperations((short) 15) != 0L)
        {
          this.Bpecr1.Cheque = this.TMPCheque;
          return false;
        }
        this.PostProcessOperationId(false);
        this.Bpecr1.Cheque = this.TMPCheque;
        this.LabelStatus.Text = text;
        if (this.Bpecr1.RetCode == 0)
        {
          this.OutputScrInfo();
          this.OutputPrnInfo();
          this.OutputCustomerInfo();
          int num = (int) MessageBox.Show("Операция выполнена успешно!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
          if (this.Bpecr1.RetCode == 2 && this.bFisicalConnectToPOS)
          {
            this.Bpecr1.DisconnectFromPOS();
            this.bFisicalConnectToPOS = false;
            this.RestartServer();
          }
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка операции 'Rollback'");
          this.RestartServer();
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Rollback", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.LabelStatus.Text = text;
        return false;
      }
      return true;
    }

    private void ButtonRollback_Click(object sender, EventArgs e) => this.OnPerformRollback((string) null, (string) null);

    private void ComboBoxCardType_SelectedIndexChanged(object sender, EventArgs e) => this.SetDefCardDataByType(this.ComboBoxCardType.SelectedIndex + 1);

    private void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
      if (this.Check_UseKLProtocol.Checked && this.Check_IntegralProt.Checked)
        this.InfoBtn.Enabled = true;
      else
        this.InfoBtn.Enabled = false;
    }

    public void Info()
    {
      try
      {
        this.LabelStatus.Text = "Ожидайте...";
        this.LabelStatus.Refresh();
        this.Bpecr1.IsBonusBalance = 0;
        this.LabelBonusBalance.Text = "0,00";
        this.Frame26.Visible = false;
        if (this.Bpecr1.IsServerModeRunning > 0)
        {
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 14) != 0L)
          {
            this.Bpecr1.Cheque = this.TMPCheque;
            return;
          }
          this.PostProcessOperationId(false);
        }
        else
        {
          if (this.Bpecr1.IsPOSConnected != 1 && this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
          {
            ErrorInterpreter.OutputErrorInfo(ErrorInterpreter.ReturnCode.Ok, "Ошибка операции 'Инфо по карте': ");
            return;
          }
          this.SetCardData();
          this.PreProcessOperationId();
          if (this.WrapPerformPosOperations((short) 14) != 0L)
            return;
          this.PostProcessOperationId(false);
        }
        this.OutputScrInfo();
        this.OutputPrnInfo();
        this.OutputCustomerInfo();
        if (this.Bpecr1.RetCode == 0)
        {
          if (this.Bpecr1.IsBonusBalance == 1)
          {
            this.Frame26.Visible = true;
            this.LabelBonusBalance.Text = (this.Bpecr1.BonusBalance / 100).ToString();
          }
          else
          {
            this.LabelBonusBalance.Text = "0,00";
            this.Frame26.Visible = false;
          }
          if (string.IsNullOrEmpty(this.Bpecr1.CustomerDisplayMessage as string))
            return;
          string customerDisplayMessage = (string) this.Bpecr1.CustomerDisplayMessage;
          this.CustomDesplayMessageText.Text = Utils.PrepareString0D0A(ref customerDisplayMessage);
          this.CustomDesplayMessageText.Text += "\r\n";
        }
        else
        {
          if (this.Bpecr1.RetCode != 4 && this.Bpecr1.RetCode != 2)
            return;
          if (this.Bpecr1.IsServerModeRunning > 0)
          {
            this.RestartServer();
          }
          else
          {
            this.Bpecr1.DisconnectFromPOS();
            this.bFisicalConnectToPOS = false;
          }
        }
      }
      catch (Exception ex)
      {
        if (this.Bpecr1.RetCode == 0)
        {
          int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Инфо по карте", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка операции 'Промо'");
      }
    }

    private void InfoBtn_Click(object sender, EventArgs e) => this.Info();

    public bool Promo()
    {
      int code = 0;
      try
      {
        this.LabelStatus.Text = "Ожидайте...";
        this.LabelStatus.Refresh();
        this.LabelBonusBalance.Text = "0,00";
        this.Frame26.Visible = false;
        this.Bpecr1.IsBonusBalance = 0;
        if (this.GetTimeout.Visible)
          this.Bpecr1.SecondsToWait = Convert.ToInt32(this.GetTimeout.Text);
        this.SetCardData();
        DialogCheque.DefInstance.ClearModifCheque();
        if (this.GoodsCount > 0)
          this.AddAllGoods();
        if (this.Bpecr1.IsPOSConnected != 1 && this.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) code, "Ошибка операции 'Промо Акция'");
          return false;
        }
        if (!string.IsNullOrEmpty(FileSystem.Dir(Bel.Instance.Bpecr1.FCPath as string, FileAttribute.Hidden)))
        {
          FileSystem.SetAttr(Bel.Instance.Bpecr1.FCPath as string, FileAttribute.Normal);
          FileSystem.Kill(Bel.Instance.Bpecr1.FCPath as string);
        }
        this.TMPCheque = this.Bpecr1.Cheque;
        int chequeSummPrice1 = this.Bpecr1.ChequeSummPrice;
        this.PreProcessOperationId();
        if (this.WrapPerformPosOperations((short) 16) != 0L)
          return false;
        this.PostProcessOperationId(false);
        if (this.Bpecr1.RetCode == 0)
        {
          this.Check_UseKLProtocol.Enabled = false;
          this.OutputScrInfo();
          this.OutputPrnInfo();
          this.OutputCustomerInfo();
          int iParamNumber1 = 19;
          string paramFromAddFile1 = this.GetParamFromAddFile(ref iParamNumber1);
          if (!string.IsNullOrEmpty(paramFromAddFile1))
          {
            this.LabelClientID.Text = paramFromAddFile1;
            this.Frame14.Visible = true;
          }
          if (this.Bpecr1.RetCode == 0)
          {
            if (this.IsChequePresent())
            {
              DialogCheque.DefInstance.SetGoods();
              int chequeSummPrice2 = this.Bpecr1.ChequeSummPrice;
            }
            int iParamNumber2 = 3;
            string paramFromAddFile2 = this.GetParamFromAddFile(ref iParamNumber2);
            if (!string.IsNullOrEmpty(paramFromAddFile2))
              DialogCheque.DefInstance.TextModifChequePaymentCash.Text = string.Format("{0:#,#0.00}", (object) (Convert.ToDecimal(paramFromAddFile2) / 100M));
            int iParamNumber3 = 10;
            string paramFromAddFile3 = this.GetParamFromAddFile(ref iParamNumber3);
            if (!string.IsNullOrEmpty(paramFromAddFile3))
              DialogCheque.DefInstance.TextModifChequePaymentCard.Text = string.Format("{0:#,#0.00}", (object) (Convert.ToDecimal(paramFromAddFile3) / 100M));
            int iParamNumber4 = 11;
            string paramFromAddFile4 = this.GetParamFromAddFile(ref iParamNumber4);
            if (!string.IsNullOrEmpty(paramFromAddFile4))
              DialogCheque.DefInstance.TextModifChequePaymentBonuses.Text = string.Format("{0:#,#0.00}", (object) (Convert.ToDecimal(paramFromAddFile4) / 100M));
            int iParamNumber5 = 13;
            string paramFromAddFile5 = this.GetParamFromAddFile(ref iParamNumber5);
            if (!string.IsNullOrEmpty(paramFromAddFile5))
              DialogCheque.DefInstance.TextModifChequePaymentCredit.Text = string.Format("{0:#,#0.00}", (object) (Convert.ToDecimal(paramFromAddFile5) / 100M));
            int iParamNumber6 = 16;
            string paramFromAddFile6 = this.GetParamFromAddFile(ref iParamNumber6);
            if (!string.IsNullOrEmpty(paramFromAddFile6))
              DialogCheque.DefInstance.TextModifChequePaymentPrepaid.Text = string.Format("{0:#,#0.00}", (object) (Convert.ToDecimal(paramFromAddFile6) / 100M));
            DialogCheque.DefInstance.TextGoodCode.Text = this.GoodsCode.Text;
            DialogCheque.DefInstance.TextGoodName.Text = this.GoodsNames.Text;
            DialogCheque.DefInstance.TextGoodPrice.Text = this.GoodsPrices.Text;
            DialogCheque.DefInstance.TextGoodQuantity.Text = this.GoodsQuantity.Text;
            DialogCheque.DefInstance.TextGoodAmount.Text = this.GoodsAmount.Text;
            string text1 = DialogCheque.DefInstance.Text;
            DialogCheque.DefInstance.Text = "Результат Промо Акции";
            string text2 = DialogCheque.DefInstance.LabelModifCheque.Text;
            DialogCheque.DefInstance.LabelModifCheque.Text = "Модифицированный список товаров";
            DialogCheque.DefInstance.Command1.Visible = true;
            DialogCheque.DefInstance.ButtonPayModif.Visible = false;
            DialogCheque.DefInstance.ButtonModification.Visible = false;
            DialogCheque.DefInstance.ChequeTimeButton2.Visible = false;
            DialogCheque.DefInstance.ButtomPaySource.Visible = false;
            DialogCheque.DefInstance.TextModifChequeDiscount1.Visible = false;
            DialogCheque.DefInstance.TextModifChequeAmount.Visible = false;
            DialogCheque.DefInstance.Label23.Visible = false;
            DialogCheque.DefInstance.Label16.Visible = false;
            DialogCheque.DefInstance.Text = text1;
            DialogCheque.DefInstance.LabelModifCheque.Text = text2;
            DialogCheque.DefInstance.Command1.Visible = false;
            DialogCheque.DefInstance.ButtonPayModif.Visible = true;
            DialogCheque.DefInstance.ButtonModification.Visible = true;
            DialogCheque.DefInstance.ChequeTimeButton2.Visible = true;
            DialogCheque.DefInstance.ButtomPaySource.Visible = true;
            DialogCheque.DefInstance.TextModifChequeDiscount1.Visible = true;
            DialogCheque.DefInstance.TextModifChequeAmount.Visible = true;
            DialogCheque.DefInstance.Label23.Visible = true;
            DialogCheque.DefInstance.Label16.Visible = true;
          }
          else
          {
            string Prompt = "Операция ПА проведена успешно.";
            int iParamNumber7 = 3;
            string paramFromAddFile7 = this.GetParamFromAddFile(ref iParamNumber7);
            if (!string.IsNullOrEmpty(paramFromAddFile7))
            {
              long int32 = (long) Convert.ToInt32(paramFromAddFile7);
              Prompt = Prompt + "\r\n" + "Сумма: " + string.Format("{0:#,#0.00}", (object) (int32 / 100L));
            }
            int num = (int) Interaction.MsgBox((object) Prompt);
            this.Bpecr1.Amount = 0;
            this.Bpecr1.Discount = 0;
            this.Bpecr1.RemoveAllItemsFromCheque();
            this.ClearGoodFileds();
          }
          this.MakeAlreadyPaymentsVisible();
          this.FillAlreadyPayedFromAddFile();
          if (this.Bpecr1.IsBonusBalance == 1)
          {
            this.Frame26.Visible = true;
            this.LabelBonusBalance.Text = (this.Bpecr1.BonusBalance / 100).ToString();
          }
          else
          {
            this.LabelBonusBalance.Text = "0,00";
            this.Frame26.Visible = false;
          }
          if (!string.IsNullOrEmpty(this.Bpecr1.CustomerDisplayMessage as string))
          {
            string customerDisplayMessage = (string) this.Bpecr1.CustomerDisplayMessage;
            this.CustomDesplayMessageText.Text = Utils.PrepareString0D0A(ref customerDisplayMessage);
            this.CustomDesplayMessageText.Text += "\r\n";
          }
          return true;
        }
        this.OutputScrInfo();
        this.OutputPrnInfo();
        this.OutputCustomerInfo();
        ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) this.Bpecr1.RetCode, "Ошибка операции 'Промо'");
        if (this.Bpecr1.RetCode == 4 | this.Bpecr1.RetCode == 2)
        {
          if (this.Bpecr1.IsServerModeRunning > 0)
          {
            this.RestartServer();
          }
          else
          {
            this.Bpecr1.DisconnectFromPOS();
            this.bFisicalConnectToPOS = false;
          }
        }
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Промо-акция", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
      return false;
    }

    private void PromoBtn_Click(object sender, EventArgs e) => this.Promo();

    private void ButtonStartServer_Click(object sender, EventArgs e)
    {
      this.PrepareCommunications();
      this.Bpecr1.StartServerMode();
      if (this.Bpecr1.RetCode == 0)
      {
        this.ButtonStartServer.Enabled = false;
      }
      else
      {
        int num = (int) MessageBox.Show("Возникла ошибка при старте сервера!", "Серверный режим", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void Button1_Click(object sender, EventArgs e)
    {
      try
      {
        this.Bpecr1.StopServerMode();
        this.ButtonStartServer.Enabled = true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка остановки сервера!", "Серверный режим", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void Timer2_Tick(object sender, EventArgs e)
    {
      this.bFisicalConnectToPOS = this.Bpecr1.IsPOSConnected == 1;
      if (this.Bpecr1.IsPOSConnected > 0)
      {
        this.LabelStatus.Text = "Connect.";
        this.LabelStatus.BackColor = Color.Lime;
      }
      else
      {
        this.LabelStatus.Text = "Disconnect.";
        this.LabelStatus.BackColor = Color.Pink;
      }
      if (this.Bpecr1.IsServerModeRunning > 0)
      {
        this.LabelStatus.Text += " Server is running...";
        this.ButtonStartServer.Enabled = false;
        this.ButtonStopServer.Enabled = true;
      }
      else
      {
        this.ButtonStartServer.Enabled = true;
        this.ButtonStopServer.Enabled = false;
      }
      if (!string.IsNullOrEmpty(this.Bpecr1.ScreenMessage as string))
        this.OutputScrInfo();
      if (!string.IsNullOrEmpty(this.Bpecr1.PrinterMessage as string))
        this.OutputPrnInfo();
      if (string.IsNullOrEmpty(this.Bpecr1.CustomerDisplayMessage as string))
        return;
      this.OutputCustomerInfo();
    }

    private void BEL_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void ButtonStartServer_Click_1(object sender, EventArgs e)
    {
      try
      {
        this.StartServer();
        this.Timer3_Tick(sender, e);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка при старте сервера!", "Серверный режим", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void ButtonStopServer_Click(object sender, EventArgs e)
    {
      try
      {
        this.Bpecr1.StopServerMode();
        this.ButtonStartServer.Enabled = true;
        this.g_WasStartServer = false;
        this.Timer3_Tick(sender, e);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка остановки сервера!", "Серверный режим", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public bool IsChequePresent()
    {
      bool flag = false;
      try
      {
        if (this.Bpecr1.goodCount > (short) 0)
          flag = true;
      }
      catch (Exception ex)
      {
        flag = false;
      }
      return flag;
    }

    public void GetTrackStrValue(ref string track1, ref string track2)
    {
      track1 = this.MagTrack1Field.Text;
      track2 = this.MagTrack2Field.Text;
    }

    private void Timer3_Tick(object sender, EventArgs e)
    {
      if (this.Check_UseKLProtocol.Checked && this.Check_IntegralProt.Checked && this.Bpecr1.IsServerModeRunning > 0)
      {
        if (this.Text.Equals(this.g_StringTitle + " (режим №4)"))
          return;
        this.Text = this.g_StringTitle + " (режим №4)";
      }
      else if (this.Check_UseKLProtocol.Checked && this.Check_IntegralProt.Checked && this.Bpecr1.IsServerModeRunning <= 0)
      {
        if (this.Text.Equals(this.g_StringTitle + " (режим №3)"))
          return;
        this.Text = this.g_StringTitle + " (режим №3)";
      }
      else if (this.Check_UseKLProtocol.Checked && !this.Check_IntegralProt.Checked && this.Bpecr1.IsServerModeRunning <= 0)
      {
        if (this.Text.Equals(this.g_StringTitle + " (режим №2)"))
          return;
        this.Text = this.g_StringTitle + " (режим №2)";
      }
      else if (!this.Check_UseKLProtocol.Checked && !this.Check_IntegralProt.Checked && this.Bpecr1.IsServerModeRunning > 0)
      {
        if (this.Text.Equals(this.g_StringTitle + " (режим №1)"))
          return;
        this.Text = this.g_StringTitle + " (режим №1)";
      }
      else
        this.Text = this.g_StringTitle + " (режим неопределен)";
    }

    public Bel()
    {
      try
      {
        this.Bpecr1 = (BPECR) new BPECRClass();
        this.InitSuccess = true;
        this.InitializeComponent();
        this.FormClosing += new FormClosingEventHandler(this.BEL_FormClosing);
        this.OnFormLoad();
        this.KeyPress += new KeyPressEventHandler(this.BEL_KeyPress);
      }
      catch (Exception ex)
      {
        this.InitSuccess = false;
        if (!AppConfigurator.EnableLSPoint)
          return;
        AppConfigurator.EnableLSPoint = false;
        ARMLogger.Error("Не удалось загрузить библиотеку bel.dll для LSPoint.\nОбратитесь к администратору.");
      }
    }

    public void Reset()
    {
      try
      {
        bool bButtonPressed = true;
        this.ResetContext(ref bButtonPressed);
        this.RestartServer();
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Сброс", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.AlreadyPayedPreAuthorization = new TextBox();
      this.ToolTip1 = new ToolTip(this.components);
      this.GetPayValPreAuthorization = new TextBox();
      this.Frame19 = new GroupBox();
      this.PayAnotherPreAuthorization = new Button();
      this.AlreadyPayedAnotherPreAuthorization = new TextBox();
      this.GetPayValAnotherPreAuthorization = new TextBox();
      this.Label39 = new Label();
      this.Label38 = new Label();
      this.Frame16 = new GroupBox();
      this.PayPreAuthorization = new Button();
      this.Label32 = new Label();
      this.Label31 = new Label();
      this.RefundSimple = new Button();
      this.TextRRNSimple = new TextBox();
      this.GetPayVal = new TextBox();
      this.Frame17 = new GroupBox();
      this.PayCommitAuthorization = new Button();
      this.AlreadyCommitAuthorization = new TextBox();
      this.GetPayValCommitAuthorization = new TextBox();
      this.Label44 = new Label();
      this.Label35 = new Label();
      this.Label34 = new Label();
      this.PayCash = new Button();
      this.Label41 = new Label();
      this.AlreadyPayedCash = new TextBox();
      this.Frame6 = new GroupBox();
      this.LabelGetPayVal = new Label();
      this.Label16 = new Label();
      this.Label40 = new Label();
      this.GroupBox2 = new GroupBox();
      this.RB_CP1251 = new RadioButton();
      this.RB_CP866 = new RadioButton();
      this.Frame15 = new GroupBox();
      this.Frame18 = new GroupBox();
      this.TextRRNAnother = new TextBox();
      this.GetPayValAnotherCommitAuthorization = new TextBox();
      this.AlreadyAnotherCommitAuthorization = new TextBox();
      this.PayAnotherCommitAuthorization = new Button();
      this.RefundAnother = new Button();
      this.Label45 = new Label();
      this.Label37 = new Label();
      this.Label36 = new Label();
      this.GoodsNames = new TextBox();
      this.Timer2 = new Timer(this.components);
      this.PayPrepaidAnother = new Button();
      this.GetPayValAnother = new TextBox();
      this.AlreadyPayedAnother = new TextBox();
      this.Label26 = new Label();
      this.Label25 = new Label();
      this.PayPrepaid = new Button();
      this.PayAnotherCard = new Button();
      this.PayCardWOReg = new Button();
      this.AlreadyPayedPrepaid = new TextBox();
      this.GetPayValPrepaid = new TextBox();
      this.Frame13 = new GroupBox();
      this.FCChequeTimeButton = new Button();
      this.CloseFC = new Button();
      this.LabeAlreadyPayed = new Label();
      this.LabelForLabelAlreadyPayed = new Label();
      this.LabelSummPrice = new Label();
      this.SummPriceWithoutDiscount = new Label();
      this.Label24 = new Label();
      this.Frame26 = new GroupBox();
      this.Label23 = new Label();
      this.LabelBonusBalance = new Label();
      this.Timer3 = new Timer(this.components);
      this.InfoBtn = new Button();
      this.Frame5 = new GroupBox();
      this.Frame12 = new GroupBox();
      this.PayAnotherBonuses = new Button();
      this.PayAnothBomuses = new Button();
      this.Frame10 = new GroupBox();
      this.LabelGetPayValPrepaid = new Label();
      this.Frame9 = new GroupBox();
      this.PayCredit = new Button();
      this.GetPayValCredit = new TextBox();
      this.AlreadyPayedCredit = new TextBox();
      this.LabelGetPayValCredit = new Label();
      this.Label21 = new Label();
      this.Frame8 = new GroupBox();
      this.PayBonuses = new Button();
      this.AlreadyPayedBonuses = new TextBox();
      this.GetPayValBonuses = new TextBox();
      this.Label20 = new Label();
      this.LabelGetPayValBonuses = new Label();
      this.Frame7 = new GroupBox();
      this.PayCard = new Button();
      this.GetPayValCard = new TextBox();
      this.AlreadyPayedCard = new TextBox();
      this.LabelGetPayValCard = new Label();
      this.Label17 = new Label();
      this.GroupBox1 = new GroupBox();
      this.RB_RS232 = new RadioButton();
      this.RB_Channel = new RadioButton();
      this.ButtonRollback = new Button();
      this.CardsLinking = new Button();
      this.AmountLinkingBonuses = new TextBox();
      this.AmountForCancel = new TextBox();
      this.AmountSubstitutionCirculatings = new TextBox();
      this.GetTimeout = new TextBox();
      this.FormAppliance = new Button();
      this.LabelClientID = new Label();
      this.Frame4 = new GroupBox();
      this.ChangePIN = new Button();
      this.BpRrnField = new TextBox();
      this.Check_Deposit = new CheckBox();
      this.Label11 = new Label();
      this.GoodsAmount = new TextBox();
      this.GoodsQuantity = new TextBox();
      this.NewOrder = new Button();
      this.PrnText = new TextBox();
      this.BpRrnCancel = new Button();
      this.GoodsPrices = new TextBox();
      this.ResetBtn = new Button();
      this.OK = new Button();
      this.BarCoder = new TextBox();
      this.CoodCount = new TextBox();
      this.FrameBprrnCancel = new GroupBox();
      this.Label9 = new Label();
      this.Label8 = new Label();
      this.GoodsCode = new TextBox();
      this.Label28 = new Label();
      this.ComboBoxCardType = new ComboBox();
      this.MagTrack1Field = new TextBox();
      this.Frame23 = new GroupBox();
      this.Check_TransmitCardData = new CheckBox();
      this.MagTrack2Field = new TextBox();
      this.Label50 = new Label();
      this.Label49 = new Label();
      this.Label47 = new Label();
      this.Timer1 = new Timer(this.components);
      this.Check_UseKLProtocol = new CheckBox();
      this.Frame22 = new GroupBox();
      this.Label48 = new Label();
      this.LabelClientCardID = new Label();
      this.Frame20 = new GroupBox();
      this.AmountForCancelEMV = new TextBox();
      this.MerchRrnField = new TextBox();
      this.MerchRrnCancel = new Button();
      this.Label43 = new Label();
      this.Label42 = new Label();
      this.Frame14 = new GroupBox();
      this.Check_ModificationBeforePayments = new CheckBox();
      this.Check_DiscountByGoods = new CheckBox();
      this.CheckAutoGenECROpId = new CheckBox();
      this.AmountPrepaid = new TextBox();
      this.Label46 = new Label();
      this.CheckAutoIncECROpId = new CheckBox();
      this.Label33 = new Label();
      this.FrameOpId = new GroupBox();
      this.CheckAutoGenBpSId = new CheckBox();
      this.ECROpId = new TextBox();
      this.BpSId = new TextBox();
      this.PromoBtn = new Button();
      this.SetDiscount = new Button();
      this.Label30 = new Label();
      this.Label15 = new Label();
      this.Label14 = new Label();
      this.Label7 = new Label();
      this.Label6 = new Label();
      this.DiscountRequest = new Button();
      this.Label5 = new Label();
      this.Label4 = new Label();
      this.Label3 = new Label();
      this.Label2 = new Label();
      this.LabelStatus = new Label();
      this.CustomDesplayMessageText = new TextBox();
      this.Frame3 = new GroupBox();
      this.CardSubstitution = new Button();
      this.Label13 = new Label();
      this.Label12 = new Label();
      this.AmountSubstitutionBonuses = new TextBox();
      this.StornoCheck = new CheckBox();
      this.ScreenText = new TextBox();
      this.PrepaidCharge = new Button();
      this.Frame1 = new GroupBox();
      this.Frame2 = new GroupBox();
      this.Label10 = new Label();
      this.ApplyBtn = new Button();
      this.Label22 = new Label();
      this.Frame11 = new GroupBox();
      this.ConsultantField = new TextBox();
      this._Label1_0 = new Label();
      this.DiscountField = new TextBox();
      this.Label19 = new Label();
      this.LabelDiscountRubl = new Label();
      this.InterConnectTimeoutVal = new TextBox();
      this.Frame24 = new GroupBox();
      this.RetryQuantityVal = new TextBox();
      this.Check_IntegralProt = new CheckBox();
      this.ButtonDisconnect = new Button();
      this.Frame25 = new GroupBox();
      this.ButtonStartServer = new Button();
      this.ButtonStopServer = new Button();
      this.LabelStateConnect = new Label();
      this.CheckBox_RestartServer = new CheckBox();
      this.Label18 = new Label();
      this.GroupBox3 = new GroupBox();
      this.Frame19.SuspendLayout();
      this.Frame16.SuspendLayout();
      this.Frame17.SuspendLayout();
      this.Frame6.SuspendLayout();
      this.GroupBox2.SuspendLayout();
      this.Frame15.SuspendLayout();
      this.Frame18.SuspendLayout();
      this.Frame13.SuspendLayout();
      this.Frame26.SuspendLayout();
      this.Frame5.SuspendLayout();
      this.Frame12.SuspendLayout();
      this.Frame10.SuspendLayout();
      this.Frame9.SuspendLayout();
      this.Frame8.SuspendLayout();
      this.Frame7.SuspendLayout();
      this.GroupBox1.SuspendLayout();
      this.Frame4.SuspendLayout();
      this.FrameBprrnCancel.SuspendLayout();
      this.Frame23.SuspendLayout();
      this.Frame22.SuspendLayout();
      this.Frame20.SuspendLayout();
      this.Frame14.SuspendLayout();
      this.FrameOpId.SuspendLayout();
      this.Frame3.SuspendLayout();
      this.Frame1.SuspendLayout();
      this.Frame2.SuspendLayout();
      this.Frame11.SuspendLayout();
      this.Frame24.SuspendLayout();
      this.Frame25.SuspendLayout();
      this.GroupBox3.SuspendLayout();
      this.SuspendLayout();
      this.AlreadyPayedPreAuthorization.AcceptsReturn = true;
      this.AlreadyPayedPreAuthorization.BackColor = SystemColors.Window;
      this.AlreadyPayedPreAuthorization.Cursor = Cursors.IBeam;
      this.AlreadyPayedPreAuthorization.ForeColor = Color.Red;
      this.AlreadyPayedPreAuthorization.Location = new Point(80, 36);
      this.AlreadyPayedPreAuthorization.MaxLength = 0;
      this.AlreadyPayedPreAuthorization.Name = "AlreadyPayedPreAuthorization";
      this.AlreadyPayedPreAuthorization.RightToLeft = RightToLeft.No;
      this.AlreadyPayedPreAuthorization.Size = new Size(73, 20);
      this.AlreadyPayedPreAuthorization.TabIndex = 104;
      this.AlreadyPayedPreAuthorization.Text = "0.00";
      this.GetPayValPreAuthorization.AcceptsReturn = true;
      this.GetPayValPreAuthorization.BackColor = SystemColors.Window;
      this.GetPayValPreAuthorization.Cursor = Cursors.IBeam;
      this.GetPayValPreAuthorization.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValPreAuthorization.Location = new Point(80, 11);
      this.GetPayValPreAuthorization.MaxLength = 0;
      this.GetPayValPreAuthorization.Name = "GetPayValPreAuthorization";
      this.GetPayValPreAuthorization.RightToLeft = RightToLeft.No;
      this.GetPayValPreAuthorization.Size = new Size(73, 20);
      this.GetPayValPreAuthorization.TabIndex = 105;
      this.GetPayValPreAuthorization.Text = "0.00";
      this.Frame19.Controls.Add((Control) this.PayAnotherPreAuthorization);
      this.Frame19.Controls.Add((Control) this.AlreadyPayedAnotherPreAuthorization);
      this.Frame19.Controls.Add((Control) this.GetPayValAnotherPreAuthorization);
      this.Frame19.Controls.Add((Control) this.Label39);
      this.Frame19.Controls.Add((Control) this.Label38);
      this.Frame19.ForeColor = SystemColors.ControlText;
      this.Frame19.Location = new Point(8, 184);
      this.Frame19.Name = "Frame19";
      this.Frame19.RightToLeft = RightToLeft.No;
      this.Frame19.Size = new Size(275, 61);
      this.Frame19.TabIndex = 120;
      this.Frame19.TabStop = false;
      this.PayAnotherPreAuthorization.Cursor = Cursors.Default;
      this.PayAnotherPreAuthorization.ForeColor = SystemColors.ControlText;
      this.PayAnotherPreAuthorization.Location = new Point(167, 14);
      this.PayAnotherPreAuthorization.Name = "PayAnotherPreAuthorization";
      this.PayAnotherPreAuthorization.RightToLeft = RightToLeft.No;
      this.PayAnotherPreAuthorization.Size = new Size(96, 40);
      this.PayAnotherPreAuthorization.TabIndex = 123;
      this.PayAnotherPreAuthorization.Text = "Предавториза-ция";
      this.PayAnotherPreAuthorization.Click += new EventHandler(this.PayAnotherPreAuthorization_Click);
      this.AlreadyPayedAnotherPreAuthorization.AcceptsReturn = true;
      this.AlreadyPayedAnotherPreAuthorization.BackColor = SystemColors.Window;
      this.AlreadyPayedAnotherPreAuthorization.Cursor = Cursors.IBeam;
      this.AlreadyPayedAnotherPreAuthorization.ForeColor = Color.Red;
      this.AlreadyPayedAnotherPreAuthorization.Location = new Point(79, 36);
      this.AlreadyPayedAnotherPreAuthorization.MaxLength = 0;
      this.AlreadyPayedAnotherPreAuthorization.Name = "AlreadyPayedAnotherPreAuthorization";
      this.AlreadyPayedAnotherPreAuthorization.RightToLeft = RightToLeft.No;
      this.AlreadyPayedAnotherPreAuthorization.Size = new Size(73, 20);
      this.AlreadyPayedAnotherPreAuthorization.TabIndex = 122;
      this.AlreadyPayedAnotherPreAuthorization.Text = "0.00";
      this.GetPayValAnotherPreAuthorization.AcceptsReturn = true;
      this.GetPayValAnotherPreAuthorization.BackColor = SystemColors.Window;
      this.GetPayValAnotherPreAuthorization.Cursor = Cursors.IBeam;
      this.GetPayValAnotherPreAuthorization.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValAnotherPreAuthorization.Location = new Point(79, 10);
      this.GetPayValAnotherPreAuthorization.MaxLength = 0;
      this.GetPayValAnotherPreAuthorization.Name = "GetPayValAnotherPreAuthorization";
      this.GetPayValAnotherPreAuthorization.RightToLeft = RightToLeft.No;
      this.GetPayValAnotherPreAuthorization.Size = new Size(73, 20);
      this.GetPayValAnotherPreAuthorization.TabIndex = 121;
      this.GetPayValAnotherPreAuthorization.Text = "0.00";
      this.Label39.Cursor = Cursors.Default;
      this.Label39.ForeColor = SystemColors.ControlText;
      this.Label39.Location = new Point(5, 37);
      this.Label39.Name = "Label39";
      this.Label39.RightToLeft = RightToLeft.No;
      this.Label39.Size = new Size(65, 19);
      this.Label39.TabIndex = 125;
      this.Label39.Text = "Заблокир.:";
      this.Label38.Cursor = Cursors.Default;
      this.Label38.ForeColor = SystemColors.ControlText;
      this.Label38.Location = new Point(5, 13);
      this.Label38.Name = "Label38";
      this.Label38.RightToLeft = RightToLeft.No;
      this.Label38.Size = new Size(67, 17);
      this.Label38.TabIndex = 124;
      this.Label38.Text = "Сумма:";
      this.Frame16.Controls.Add((Control) this.GetPayValPreAuthorization);
      this.Frame16.Controls.Add((Control) this.AlreadyPayedPreAuthorization);
      this.Frame16.Controls.Add((Control) this.PayPreAuthorization);
      this.Frame16.Controls.Add((Control) this.Label32);
      this.Frame16.Controls.Add((Control) this.Label31);
      this.Frame16.ForeColor = SystemColors.ControlText;
      this.Frame16.Location = new Point(8, 24);
      this.Frame16.Name = "Frame16";
      this.Frame16.RightToLeft = RightToLeft.No;
      this.Frame16.Size = new Size(275, 62);
      this.Frame16.TabIndex = 102;
      this.Frame16.TabStop = false;
      this.PayPreAuthorization.Cursor = Cursors.Default;
      this.PayPreAuthorization.ForeColor = SystemColors.ControlText;
      this.PayPreAuthorization.Location = new Point(167, 14);
      this.PayPreAuthorization.Name = "PayPreAuthorization";
      this.PayPreAuthorization.RightToLeft = RightToLeft.No;
      this.PayPreAuthorization.Size = new Size(96, 40);
      this.PayPreAuthorization.TabIndex = 103;
      this.PayPreAuthorization.Text = "Предавториза-ция";
      this.PayPreAuthorization.Click += new EventHandler(this.PayPreAuthorization_Click);
      this.Label32.Cursor = Cursors.Default;
      this.Label32.ForeColor = SystemColors.ControlText;
      this.Label32.Location = new Point(8, 14);
      this.Label32.Name = "Label32";
      this.Label32.RightToLeft = RightToLeft.No;
      this.Label32.Size = new Size(57, 17);
      this.Label32.TabIndex = 107;
      this.Label32.Text = "Сумма:";
      this.Label31.Cursor = Cursors.Default;
      this.Label31.ForeColor = SystemColors.ControlText;
      this.Label31.Location = new Point(8, 39);
      this.Label31.Name = "Label31";
      this.Label31.RightToLeft = RightToLeft.No;
      this.Label31.Size = new Size(68, 17);
      this.Label31.TabIndex = 106;
      this.Label31.Text = "Заблокир.:";
      this.RefundSimple.Cursor = Cursors.Default;
      this.RefundSimple.ForeColor = SystemColors.ControlText;
      this.RefundSimple.Location = new Point(167, 54);
      this.RefundSimple.Name = "RefundSimple";
      this.RefundSimple.RightToLeft = RightToLeft.No;
      this.RefundSimple.Size = new Size(96, 26);
      this.RefundSimple.TabIndex = 135;
      this.RefundSimple.Text = "Возврат";
      this.RefundSimple.Click += new EventHandler(this.RefundSimple_Click);
      this.TextRRNSimple.AcceptsReturn = true;
      this.TextRRNSimple.BackColor = SystemColors.Window;
      this.TextRRNSimple.Cursor = Cursors.IBeam;
      this.TextRRNSimple.ForeColor = SystemColors.WindowText;
      this.TextRRNSimple.Location = new Point(48, 59);
      this.TextRRNSimple.MaxLength = 0;
      this.TextRRNSimple.Name = "TextRRNSimple";
      this.TextRRNSimple.RightToLeft = RightToLeft.No;
      this.TextRRNSimple.Size = new Size(105, 20);
      this.TextRRNSimple.TabIndex = 131;
      this.TextRRNSimple.Text = "ｈ";
      this.GetPayVal.AcceptsReturn = true;
      this.GetPayVal.BackColor = SystemColors.Window;
      this.GetPayVal.Cursor = Cursors.IBeam;
      this.GetPayVal.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayVal.Location = new Point(80, 16);
      this.GetPayVal.MaxLength = 0;
      this.GetPayVal.Name = "GetPayVal";
      this.GetPayVal.RightToLeft = RightToLeft.No;
      this.GetPayVal.Size = new Size(73, 20);
      this.GetPayVal.TabIndex = 16;
      this.GetPayVal.Text = "0.00";
      this.Frame17.Controls.Add((Control) this.RefundSimple);
      this.Frame17.Controls.Add((Control) this.TextRRNSimple);
      this.Frame17.Controls.Add((Control) this.PayCommitAuthorization);
      this.Frame17.Controls.Add((Control) this.AlreadyCommitAuthorization);
      this.Frame17.Controls.Add((Control) this.GetPayValCommitAuthorization);
      this.Frame17.Controls.Add((Control) this.Label44);
      this.Frame17.Controls.Add((Control) this.Label35);
      this.Frame17.Controls.Add((Control) this.Label34);
      this.Frame17.ForeColor = SystemColors.ControlText;
      this.Frame17.Location = new Point(8, 86);
      this.Frame17.Name = "Frame17";
      this.Frame17.RightToLeft = RightToLeft.No;
      this.Frame17.Size = new Size(275, 86);
      this.Frame17.TabIndex = 108;
      this.Frame17.TabStop = false;
      this.PayCommitAuthorization.Cursor = Cursors.Default;
      this.PayCommitAuthorization.ForeColor = SystemColors.ControlText;
      this.PayCommitAuthorization.Location = new Point(167, 11);
      this.PayCommitAuthorization.Name = "PayCommitAuthorization";
      this.PayCommitAuthorization.RightToLeft = RightToLeft.No;
      this.PayCommitAuthorization.Size = new Size(96, 39);
      this.PayCommitAuthorization.TabIndex = 111;
      this.PayCommitAuthorization.Text = "Завершение расчета";
      this.PayCommitAuthorization.Click += new EventHandler(this.PayCommitAuthorization_Click);
      this.AlreadyCommitAuthorization.AcceptsReturn = true;
      this.AlreadyCommitAuthorization.BackColor = SystemColors.Window;
      this.AlreadyCommitAuthorization.Cursor = Cursors.IBeam;
      this.AlreadyCommitAuthorization.ForeColor = Color.Red;
      this.AlreadyCommitAuthorization.Location = new Point(79, 35);
      this.AlreadyCommitAuthorization.MaxLength = 0;
      this.AlreadyCommitAuthorization.Name = "AlreadyCommitAuthorization";
      this.AlreadyCommitAuthorization.RightToLeft = RightToLeft.No;
      this.AlreadyCommitAuthorization.Size = new Size(73, 20);
      this.AlreadyCommitAuthorization.TabIndex = 110;
      this.AlreadyCommitAuthorization.Text = "0.00";
      this.GetPayValCommitAuthorization.AcceptsReturn = true;
      this.GetPayValCommitAuthorization.BackColor = SystemColors.Window;
      this.GetPayValCommitAuthorization.Cursor = Cursors.IBeam;
      this.GetPayValCommitAuthorization.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValCommitAuthorization.Location = new Point(80, 11);
      this.GetPayValCommitAuthorization.MaxLength = 0;
      this.GetPayValCommitAuthorization.Name = "GetPayValCommitAuthorization";
      this.GetPayValCommitAuthorization.RightToLeft = RightToLeft.No;
      this.GetPayValCommitAuthorization.Size = new Size(73, 20);
      this.GetPayValCommitAuthorization.TabIndex = 109;
      this.GetPayValCommitAuthorization.Text = "0.00";
      this.Label44.Cursor = Cursors.Default;
      this.Label44.ForeColor = SystemColors.ControlText;
      this.Label44.Location = new Point(5, 59);
      this.Label44.Name = "Label44";
      this.Label44.RightToLeft = RightToLeft.No;
      this.Label44.Size = new Size(33, 22);
      this.Label44.TabIndex = 132;
      this.Label44.Text = "RRN:";
      this.Label35.Cursor = Cursors.Default;
      this.Label35.ForeColor = SystemColors.ControlText;
      this.Label35.Location = new Point(5, 35);
      this.Label35.Name = "Label35";
      this.Label35.RightToLeft = RightToLeft.No;
      this.Label35.Size = new Size(73, 17);
      this.Label35.TabIndex = 113;
      this.Label35.Text = "Подтвержд.:";
      this.Label34.Cursor = Cursors.Default;
      this.Label34.ForeColor = SystemColors.ControlText;
      this.Label34.Location = new Point(5, 11);
      this.Label34.Name = "Label34";
      this.Label34.RightToLeft = RightToLeft.No;
      this.Label34.Size = new Size(54, 20);
      this.Label34.TabIndex = 112;
      this.Label34.Text = "Сумма:";
      this.PayCash.Cursor = Cursors.Default;
      this.PayCash.ForeColor = SystemColors.ControlText;
      this.PayCash.Location = new Point(162, 16);
      this.PayCash.Name = "PayCash";
      this.PayCash.RightToLeft = RightToLeft.No;
      this.PayCash.Size = new Size(89, 44);
      this.PayCash.TabIndex = 18;
      this.PayCash.Text = "Наличные";
      this.PayCash.Click += new EventHandler(this.PayCash_Click);
      this.Label41.Cursor = Cursors.Default;
      this.Label41.ForeColor = SystemColors.ControlText;
      this.Label41.Location = new Point(8, 173);
      this.Label41.Name = "Label41";
      this.Label41.RightToLeft = RightToLeft.No;
      this.Label41.Size = new Size(97, 15);
      this.Label41.TabIndex = (int) sbyte.MaxValue;
      this.Label41.Text = "Другой картой";
      this.AlreadyPayedCash.AcceptsReturn = true;
      this.AlreadyPayedCash.BackColor = SystemColors.Window;
      this.AlreadyPayedCash.Cursor = Cursors.IBeam;
      this.AlreadyPayedCash.ForeColor = Color.Red;
      this.AlreadyPayedCash.Location = new Point(80, 40);
      this.AlreadyPayedCash.MaxLength = 0;
      this.AlreadyPayedCash.Name = "AlreadyPayedCash";
      this.AlreadyPayedCash.RightToLeft = RightToLeft.No;
      this.AlreadyPayedCash.Size = new Size(73, 20);
      this.AlreadyPayedCash.TabIndex = 17;
      this.AlreadyPayedCash.Text = "0.00";
      this.Frame6.Controls.Add((Control) this.GetPayVal);
      this.Frame6.Controls.Add((Control) this.PayCash);
      this.Frame6.Controls.Add((Control) this.AlreadyPayedCash);
      this.Frame6.Controls.Add((Control) this.LabelGetPayVal);
      this.Frame6.Controls.Add((Control) this.Label16);
      this.Frame6.ForeColor = SystemColors.ControlText;
      this.Frame6.Location = new Point(8, 14);
      this.Frame6.Name = "Frame6";
      this.Frame6.RightToLeft = RightToLeft.No;
      this.Frame6.Size = new Size(257, 69);
      this.Frame6.TabIndex = 66;
      this.Frame6.TabStop = false;
      this.Frame6.Visible = false;
      this.LabelGetPayVal.Cursor = Cursors.Default;
      this.LabelGetPayVal.ForeColor = SystemColors.ControlText;
      this.LabelGetPayVal.Location = new Point(8, 16);
      this.LabelGetPayVal.Name = "LabelGetPayVal";
      this.LabelGetPayVal.RightToLeft = RightToLeft.No;
      this.LabelGetPayVal.Size = new Size(73, 17);
      this.LabelGetPayVal.TabIndex = 67;
      this.LabelGetPayVal.Text = "Наличными:";
      this.Label16.Cursor = Cursors.Default;
      this.Label16.ForeColor = SystemColors.ControlText;
      this.Label16.Location = new Point(8, 32);
      this.Label16.Name = "Label16";
      this.Label16.RightToLeft = RightToLeft.No;
      this.Label16.Size = new Size(73, 28);
      this.Label16.TabIndex = 68;
      this.Label16.Text = "Оплачено наличными:";
      this.Label40.Cursor = Cursors.Default;
      this.Label40.ForeColor = SystemColors.ControlText;
      this.Label40.Location = new Point(8, 13);
      this.Label40.Name = "Label40";
      this.Label40.RightToLeft = RightToLeft.No;
      this.Label40.Size = new Size(81, 14);
      this.Label40.TabIndex = 126;
      this.Label40.Text = "Этой картой";
      this.GroupBox2.BackColor = Color.Transparent;
      this.GroupBox2.Controls.Add((Control) this.RB_CP1251);
      this.GroupBox2.Controls.Add((Control) this.RB_CP866);
      this.GroupBox2.Enabled = false;
      this.GroupBox2.Location = new Point(686, 5);
      this.GroupBox2.Name = "GroupBox2";
      this.GroupBox2.Size = new Size(92, 62);
      this.GroupBox2.TabIndex = 229;
      this.GroupBox2.TabStop = false;
      this.GroupBox2.Text = "Кодировка";
      this.RB_CP1251.AutoSize = true;
      this.RB_CP1251.Location = new Point(5, 34);
      this.RB_CP1251.Name = "RB_CP1251";
      this.RB_CP1251.Size = new Size(63, 17);
      this.RB_CP1251.TabIndex = 169;
      this.RB_CP1251.TabStop = true;
      this.RB_CP1251.Text = "CP1251";
      this.RB_CP1251.UseVisualStyleBackColor = true;
      this.RB_CP866.AutoSize = true;
      this.RB_CP866.Checked = true;
      this.RB_CP866.Location = new Point(5, 14);
      this.RB_CP866.Name = "RB_CP866";
      this.RB_CP866.Size = new Size(57, 17);
      this.RB_CP866.TabIndex = 168;
      this.RB_CP866.TabStop = true;
      this.RB_CP866.Text = "CP866";
      this.RB_CP866.UseVisualStyleBackColor = true;
      this.Frame15.Controls.Add((Control) this.Frame18);
      this.Frame15.Controls.Add((Control) this.Frame17);
      this.Frame15.Controls.Add((Control) this.Frame16);
      this.Frame15.Controls.Add((Control) this.Frame19);
      this.Frame15.Controls.Add((Control) this.Label41);
      this.Frame15.Controls.Add((Control) this.Label40);
      this.Frame15.ForeColor = SystemColors.ControlText;
      this.Frame15.Location = new Point(456, 14);
      this.Frame15.Name = "Frame15";
      this.Frame15.RightToLeft = RightToLeft.No;
      this.Frame15.Size = new Size(289, 338);
      this.Frame15.TabIndex = 101;
      this.Frame15.TabStop = false;
      this.Frame15.Text = "МПС";
      this.Frame18.Controls.Add((Control) this.TextRRNAnother);
      this.Frame18.Controls.Add((Control) this.GetPayValAnotherCommitAuthorization);
      this.Frame18.Controls.Add((Control) this.AlreadyAnotherCommitAuthorization);
      this.Frame18.Controls.Add((Control) this.PayAnotherCommitAuthorization);
      this.Frame18.Controls.Add((Control) this.RefundAnother);
      this.Frame18.Controls.Add((Control) this.Label45);
      this.Frame18.Controls.Add((Control) this.Label37);
      this.Frame18.Controls.Add((Control) this.Label36);
      this.Frame18.ForeColor = SystemColors.ControlText;
      this.Frame18.Location = new Point(8, 245);
      this.Frame18.Name = "Frame18";
      this.Frame18.RightToLeft = RightToLeft.No;
      this.Frame18.Size = new Size(275, 89);
      this.Frame18.TabIndex = 114;
      this.Frame18.TabStop = false;
      this.TextRRNAnother.AcceptsReturn = true;
      this.TextRRNAnother.BackColor = SystemColors.Window;
      this.TextRRNAnother.Cursor = Cursors.IBeam;
      this.TextRRNAnother.ForeColor = SystemColors.WindowText;
      this.TextRRNAnother.Location = new Point(48, 60);
      this.TextRRNAnother.MaxLength = 0;
      this.TextRRNAnother.Name = "TextRRNAnother";
      this.TextRRNAnother.RightToLeft = RightToLeft.No;
      this.TextRRNAnother.Size = new Size(105, 20);
      this.TextRRNAnother.TabIndex = 133;
      this.TextRRNAnother.Text = "0";
      this.GetPayValAnotherCommitAuthorization.AcceptsReturn = true;
      this.GetPayValAnotherCommitAuthorization.BackColor = SystemColors.Window;
      this.GetPayValAnotherCommitAuthorization.Cursor = Cursors.IBeam;
      this.GetPayValAnotherCommitAuthorization.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValAnotherCommitAuthorization.Location = new Point(79, 11);
      this.GetPayValAnotherCommitAuthorization.MaxLength = 0;
      this.GetPayValAnotherCommitAuthorization.Name = "GetPayValAnotherCommitAuthorization";
      this.GetPayValAnotherCommitAuthorization.RightToLeft = RightToLeft.No;
      this.GetPayValAnotherCommitAuthorization.Size = new Size(73, 20);
      this.GetPayValAnotherCommitAuthorization.TabIndex = 117;
      this.GetPayValAnotherCommitAuthorization.Text = "0.00";
      this.AlreadyAnotherCommitAuthorization.AcceptsReturn = true;
      this.AlreadyAnotherCommitAuthorization.BackColor = SystemColors.Window;
      this.AlreadyAnotherCommitAuthorization.Cursor = Cursors.IBeam;
      this.AlreadyAnotherCommitAuthorization.ForeColor = Color.Red;
      this.AlreadyAnotherCommitAuthorization.Location = new Point(79, 35);
      this.AlreadyAnotherCommitAuthorization.MaxLength = 0;
      this.AlreadyAnotherCommitAuthorization.Name = "AlreadyAnotherCommitAuthorization";
      this.AlreadyAnotherCommitAuthorization.RightToLeft = RightToLeft.No;
      this.AlreadyAnotherCommitAuthorization.Size = new Size(73, 20);
      this.AlreadyAnotherCommitAuthorization.TabIndex = 116;
      this.AlreadyAnotherCommitAuthorization.Text = "0.00";
      this.PayAnotherCommitAuthorization.Cursor = Cursors.Default;
      this.PayAnotherCommitAuthorization.ForeColor = SystemColors.ControlText;
      this.PayAnotherCommitAuthorization.Location = new Point(164, 13);
      this.PayAnotherCommitAuthorization.Name = "PayAnotherCommitAuthorization";
      this.PayAnotherCommitAuthorization.RightToLeft = RightToLeft.No;
      this.PayAnotherCommitAuthorization.Size = new Size(96, 41);
      this.PayAnotherCommitAuthorization.TabIndex = 115;
      this.PayAnotherCommitAuthorization.Text = "Завершение расчета";
      this.PayAnotherCommitAuthorization.Click += new EventHandler(this.PayAnotherCommitAuthorization_Click);
      this.RefundAnother.Cursor = Cursors.Default;
      this.RefundAnother.ForeColor = SystemColors.ControlText;
      this.RefundAnother.Location = new Point(164, 57);
      this.RefundAnother.Name = "RefundAnother";
      this.RefundAnother.RightToLeft = RightToLeft.No;
      this.RefundAnother.Size = new Size(96, 25);
      this.RefundAnother.TabIndex = 136;
      this.RefundAnother.Text = "Возврат";
      this.RefundAnother.Click += new EventHandler(this.RefundAnother_Click);
      this.Label45.Cursor = Cursors.Default;
      this.Label45.ForeColor = SystemColors.ControlText;
      this.Label45.Location = new Point(5, 60);
      this.Label45.Name = "Label45";
      this.Label45.RightToLeft = RightToLeft.No;
      this.Label45.Size = new Size(36, 21);
      this.Label45.TabIndex = 134;
      this.Label45.Text = "RRN:";
      this.Label37.Cursor = Cursors.Default;
      this.Label37.ForeColor = SystemColors.ControlText;
      this.Label37.Location = new Point(6, 11);
      this.Label37.Name = "Label37";
      this.Label37.RightToLeft = RightToLeft.No;
      this.Label37.Size = new Size(50, 20);
      this.Label37.TabIndex = 119;
      this.Label37.Text = "Сумма:";
      this.Label36.Cursor = Cursors.Default;
      this.Label36.ForeColor = SystemColors.ControlText;
      this.Label36.Location = new Point(3, 35);
      this.Label36.Name = "Label36";
      this.Label36.RightToLeft = RightToLeft.No;
      this.Label36.Size = new Size(70, 23);
      this.Label36.TabIndex = 118;
      this.Label36.Text = "Подтвержд.:";
      this.GoodsNames.AcceptsReturn = true;
      this.GoodsNames.BackColor = SystemColors.Window;
      this.GoodsNames.Cursor = Cursors.IBeam;
      this.GoodsNames.ForeColor = SystemColors.WindowText;
      this.GoodsNames.Location = new Point(64, 67);
      this.GoodsNames.MaxLength = 0;
      this.GoodsNames.Multiline = true;
      this.GoodsNames.Name = "GoodsNames";
      this.GoodsNames.ReadOnly = true;
      this.GoodsNames.RightToLeft = RightToLeft.No;
      this.GoodsNames.ScrollBars = ScrollBars.Horizontal;
      this.GoodsNames.Size = new Size(177, 217);
      this.GoodsNames.TabIndex = 186;
      this.GoodsNames.TabStop = false;
      this.GoodsNames.WordWrap = false;
      this.Timer2.Enabled = true;
      this.Timer2.Interval = 50;
      this.Timer2.Tick += new EventHandler(this.Timer2_Tick);
      this.PayPrepaidAnother.Cursor = Cursors.Default;
      this.PayPrepaidAnother.ForeColor = SystemColors.ControlText;
      this.PayPrepaidAnother.Location = new Point(16, (int) sbyte.MaxValue);
      this.PayPrepaidAnother.Name = "PayPrepaidAnother";
      this.PayPrepaidAnother.RightToLeft = RightToLeft.No;
      this.PayPrepaidAnother.Size = new Size(71, 55);
      this.PayPrepaidAnother.TabIndex = 88;
      this.PayPrepaidAnother.Text = "Предопл. с друг. карты";
      this.PayPrepaidAnother.Click += new EventHandler(this.PayPrepaidAnother_Click);
      this.GetPayValAnother.AcceptsReturn = true;
      this.GetPayValAnother.BackColor = SystemColors.Window;
      this.GetPayValAnother.Cursor = Cursors.IBeam;
      this.GetPayValAnother.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValAnother.Location = new Point(88, 15);
      this.GetPayValAnother.MaxLength = 0;
      this.GetPayValAnother.Name = "GetPayValAnother";
      this.GetPayValAnother.RightToLeft = RightToLeft.No;
      this.GetPayValAnother.Size = new Size(73, 20);
      this.GetPayValAnother.TabIndex = 85;
      this.GetPayValAnother.Text = "0.00";
      this.AlreadyPayedAnother.AcceptsReturn = true;
      this.AlreadyPayedAnother.BackColor = SystemColors.Window;
      this.AlreadyPayedAnother.Cursor = Cursors.IBeam;
      this.AlreadyPayedAnother.ForeColor = Color.Red;
      this.AlreadyPayedAnother.Location = new Point(88, 40);
      this.AlreadyPayedAnother.MaxLength = 0;
      this.AlreadyPayedAnother.Name = "AlreadyPayedAnother";
      this.AlreadyPayedAnother.RightToLeft = RightToLeft.No;
      this.AlreadyPayedAnother.Size = new Size(73, 20);
      this.AlreadyPayedAnother.TabIndex = 84;
      this.AlreadyPayedAnother.Text = "0.00";
      this.Label26.Cursor = Cursors.Default;
      this.Label26.ForeColor = SystemColors.ControlText;
      this.Label26.Location = new Point(13, 12);
      this.Label26.Name = "Label26";
      this.Label26.RightToLeft = RightToLeft.No;
      this.Label26.Size = new Size(57, 31);
      this.Label26.TabIndex = 87;
      this.Label26.Text = "Другие оплаты:";
      this.Label25.Cursor = Cursors.Default;
      this.Label25.ForeColor = SystemColors.ControlText;
      this.Label25.Location = new Point(13, 43);
      this.Label25.Name = "Label25";
      this.Label25.RightToLeft = RightToLeft.No;
      this.Label25.Size = new Size(65, 21);
      this.Label25.TabIndex = 86;
      this.Label25.Text = "Оплачено:";
      this.PayPrepaid.Cursor = Cursors.Default;
      this.PayPrepaid.ForeColor = SystemColors.ControlText;
      this.PayPrepaid.Location = new Point(162, 16);
      this.PayPrepaid.Name = "PayPrepaid";
      this.PayPrepaid.RightToLeft = RightToLeft.No;
      this.PayPrepaid.Size = new Size(89, 46);
      this.PayPrepaid.TabIndex = 30;
      this.PayPrepaid.Text = "Предоплата";
      this.PayPrepaid.Click += new EventHandler(this.PayPrepaid_Click);
      this.PayAnotherCard.Cursor = Cursors.Default;
      this.PayAnotherCard.ForeColor = SystemColors.ControlText;
      this.PayAnotherCard.Location = new Point(16, 70);
      this.PayAnotherCard.Name = "PayAnotherCard";
      this.PayAnotherCard.RightToLeft = RightToLeft.No;
      this.PayAnotherCard.Size = new Size(71, 55);
      this.PayAnotherCard.TabIndex = 90;
      this.PayAnotherCard.Text = "Другой картой";
      this.PayAnotherCard.Click += new EventHandler(this.PayAnotherCard_Click);
      this.PayCardWOReg.Cursor = Cursors.Default;
      this.PayCardWOReg.ForeColor = SystemColors.ControlText;
      this.PayCardWOReg.Location = new Point(90, (int) sbyte.MaxValue);
      this.PayCardWOReg.Name = "PayCardWOReg";
      this.PayCardWOReg.RightToLeft = RightToLeft.No;
      this.PayCardWOReg.Size = new Size(71, 55);
      this.PayCardWOReg.TabIndex = 89;
      this.PayCardWOReg.Text = "Картой без БС";
      this.PayCardWOReg.Click += new EventHandler(this.PayCardWOReg_Click);
      this.AlreadyPayedPrepaid.AcceptsReturn = true;
      this.AlreadyPayedPrepaid.BackColor = SystemColors.Window;
      this.AlreadyPayedPrepaid.Cursor = Cursors.IBeam;
      this.AlreadyPayedPrepaid.ForeColor = Color.Red;
      this.AlreadyPayedPrepaid.Location = new Point(80, 40);
      this.AlreadyPayedPrepaid.MaxLength = 0;
      this.AlreadyPayedPrepaid.Name = "AlreadyPayedPrepaid";
      this.AlreadyPayedPrepaid.RightToLeft = RightToLeft.No;
      this.AlreadyPayedPrepaid.Size = new Size(73, 20);
      this.AlreadyPayedPrepaid.TabIndex = 29;
      this.AlreadyPayedPrepaid.Text = "0.00";
      this.GetPayValPrepaid.AcceptsReturn = true;
      this.GetPayValPrepaid.BackColor = SystemColors.Window;
      this.GetPayValPrepaid.Cursor = Cursors.IBeam;
      this.GetPayValPrepaid.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValPrepaid.Location = new Point(80, 16);
      this.GetPayValPrepaid.MaxLength = 0;
      this.GetPayValPrepaid.Name = "GetPayValPrepaid";
      this.GetPayValPrepaid.RightToLeft = RightToLeft.No;
      this.GetPayValPrepaid.Size = new Size(73, 20);
      this.GetPayValPrepaid.TabIndex = 28;
      this.GetPayValPrepaid.Text = "0.00";
      this.Frame13.Controls.Add((Control) this.FCChequeTimeButton);
      this.Frame13.Controls.Add((Control) this.CloseFC);
      this.Frame13.Controls.Add((Control) this.LabeAlreadyPayed);
      this.Frame13.Controls.Add((Control) this.LabelForLabelAlreadyPayed);
      this.Frame13.Controls.Add((Control) this.LabelSummPrice);
      this.Frame13.Controls.Add((Control) this.SummPriceWithoutDiscount);
      this.Frame13.ForeColor = SystemColors.ControlText;
      this.Frame13.Location = new Point(272, 205);
      this.Frame13.Name = "Frame13";
      this.Frame13.RightToLeft = RightToLeft.No;
      this.Frame13.Size = new Size(177, 147);
      this.Frame13.TabIndex = 93;
      this.Frame13.TabStop = false;
      this.FCChequeTimeButton.Cursor = Cursors.Default;
      this.FCChequeTimeButton.ForeColor = SystemColors.ControlText;
      this.FCChequeTimeButton.Location = new Point(16, 105);
      this.FCChequeTimeButton.Name = "FCChequeTimeButton";
      this.FCChequeTimeButton.RightToLeft = RightToLeft.No;
      this.FCChequeTimeButton.Size = new Size(145, 26);
      this.FCChequeTimeButton.TabIndex = 155;
      this.FCChequeTimeButton.Text = "Время чека";
      this.FCChequeTimeButton.Click += new EventHandler(this.FCChequeTimeButton_Click);
      this.CloseFC.Cursor = Cursors.Default;
      this.CloseFC.Enabled = false;
      this.CloseFC.ForeColor = SystemColors.ControlText;
      this.CloseFC.Location = new Point(16, 71);
      this.CloseFC.Name = "CloseFC";
      this.CloseFC.RightToLeft = RightToLeft.No;
      this.CloseFC.Size = new Size(145, 26);
      this.CloseFC.TabIndex = 138;
      this.CloseFC.Text = "Закрыть ЧЕК";
      this.CloseFC.Visible = false;
      this.CloseFC.Click += new EventHandler(this.CloseFC_Click);
      this.LabeAlreadyPayed.Cursor = Cursors.Default;
      this.LabeAlreadyPayed.Font = new Font("Monotype Corsiva", 14.25f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, (byte) 204);
      this.LabeAlreadyPayed.ForeColor = Color.Red;
      this.LabeAlreadyPayed.Location = new Point(76, 39);
      this.LabeAlreadyPayed.Name = "LabeAlreadyPayed";
      this.LabeAlreadyPayed.RightToLeft = RightToLeft.No;
      this.LabeAlreadyPayed.Size = new Size(89, 24);
      this.LabeAlreadyPayed.TabIndex = 140;
      this.LabeAlreadyPayed.Text = "0,00";
      this.LabeAlreadyPayed.Visible = false;
      this.LabelForLabelAlreadyPayed.Cursor = Cursors.Default;
      this.LabelForLabelAlreadyPayed.ForeColor = SystemColors.ControlText;
      this.LabelForLabelAlreadyPayed.Location = new Point(8, 45);
      this.LabelForLabelAlreadyPayed.Name = "LabelForLabelAlreadyPayed";
      this.LabelForLabelAlreadyPayed.RightToLeft = RightToLeft.No;
      this.LabelForLabelAlreadyPayed.Size = new Size(73, 16);
      this.LabelForLabelAlreadyPayed.TabIndex = 139;
      this.LabelForLabelAlreadyPayed.Text = "Оплачено:";
      this.LabelForLabelAlreadyPayed.Visible = false;
      this.LabelSummPrice.Cursor = Cursors.Default;
      this.LabelSummPrice.ForeColor = SystemColors.ControlText;
      this.LabelSummPrice.Location = new Point(8, 14);
      this.LabelSummPrice.Name = "LabelSummPrice";
      this.LabelSummPrice.RightToLeft = RightToLeft.No;
      this.LabelSummPrice.Size = new Size(62, 30);
      this.LabelSummPrice.TabIndex = 95;
      this.LabelSummPrice.Text = "Всего к оплате:";
      this.SummPriceWithoutDiscount.Cursor = Cursors.Default;
      this.SummPriceWithoutDiscount.Font = new Font("Monotype Corsiva", 14.25f, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, (byte) 204);
      this.SummPriceWithoutDiscount.ForeColor = Color.FromArgb(0, 192, 0);
      this.SummPriceWithoutDiscount.Location = new Point(76, 13);
      this.SummPriceWithoutDiscount.Name = "SummPriceWithoutDiscount";
      this.SummPriceWithoutDiscount.RightToLeft = RightToLeft.No;
      this.SummPriceWithoutDiscount.Size = new Size(89, 23);
      this.SummPriceWithoutDiscount.TabIndex = 94;
      this.SummPriceWithoutDiscount.Text = "0,00";
      this.Label24.Cursor = Cursors.Default;
      this.Label24.ForeColor = SystemColors.ControlText;
      this.Label24.Location = new Point(8, 32);
      this.Label24.Name = "Label24";
      this.Label24.RightToLeft = RightToLeft.No;
      this.Label24.Size = new Size(82, 28);
      this.Label24.TabIndex = 80;
      this.Label24.Text = "Оплачено по предоплате:";
      this.Frame26.Controls.Add((Control) this.Label23);
      this.Frame26.Controls.Add((Control) this.LabelBonusBalance);
      this.Frame26.ForeColor = SystemColors.ControlText;
      this.Frame26.Location = new Point(436, 344);
      this.Frame26.Name = "Frame26";
      this.Frame26.RightToLeft = RightToLeft.No;
      this.Frame26.Size = new Size(336, 32);
      this.Frame26.TabIndex = 232;
      this.Frame26.TabStop = false;
      this.Frame26.Visible = false;
      this.Label23.ForeColor = SystemColors.ControlText;
      this.Label23.ImageAlign = ContentAlignment.MiddleLeft;
      this.Label23.Location = new Point(6, 10);
      this.Label23.Name = "Label23";
      this.Label23.RightToLeft = RightToLeft.No;
      this.Label23.Size = new Size(94, 17);
      this.Label23.TabIndex = 146;
      this.Label23.Text = "Баланс бонусов:";
      this.LabelBonusBalance.Cursor = Cursors.Default;
      this.LabelBonusBalance.ForeColor = SystemColors.ControlText;
      this.LabelBonusBalance.Location = new Point(101, 9);
      this.LabelBonusBalance.Name = "LabelBonusBalance";
      this.LabelBonusBalance.RightToLeft = RightToLeft.No;
      this.LabelBonusBalance.Size = new Size(217, 17);
      this.LabelBonusBalance.TabIndex = 145;
      this.Timer3.Enabled = true;
      this.Timer3.Interval = 500;
      this.Timer3.Tick += new EventHandler(this.Timer3_Tick);
      this.InfoBtn.Enabled = false;
      this.InfoBtn.Location = new Point(435, 68);
      this.InfoBtn.Name = "InfoBtn";
      this.InfoBtn.Size = new Size(57, 50);
      this.InfoBtn.TabIndex = 231;
      this.InfoBtn.Text = "Инфо";
      this.InfoBtn.Click += new EventHandler(this.InfoBtn_Click);
      this.Frame5.Controls.Add((Control) this.Frame13);
      this.Frame5.Controls.Add((Control) this.Frame12);
      this.Frame5.Controls.Add((Control) this.Frame10);
      this.Frame5.Controls.Add((Control) this.Frame9);
      this.Frame5.Controls.Add((Control) this.Frame8);
      this.Frame5.Controls.Add((Control) this.Frame7);
      this.Frame5.Controls.Add((Control) this.Frame6);
      this.Frame5.Controls.Add((Control) this.Frame15);
      this.Frame5.ForeColor = SystemColors.ControlText;
      this.Frame5.Location = new Point(376, 376);
      this.Frame5.Name = "Frame5";
      this.Frame5.RightToLeft = RightToLeft.No;
      this.Frame5.Size = new Size(753, 358);
      this.Frame5.TabIndex = 213;
      this.Frame5.TabStop = false;
      this.Frame5.Text = "Оплаты";
      this.Frame12.Controls.Add((Control) this.PayAnotherBonuses);
      this.Frame12.Controls.Add((Control) this.PayAnothBomuses);
      this.Frame12.Controls.Add((Control) this.PayAnotherCard);
      this.Frame12.Controls.Add((Control) this.PayCardWOReg);
      this.Frame12.Controls.Add((Control) this.PayPrepaidAnother);
      this.Frame12.Controls.Add((Control) this.GetPayValAnother);
      this.Frame12.Controls.Add((Control) this.AlreadyPayedAnother);
      this.Frame12.Controls.Add((Control) this.Label26);
      this.Frame12.Controls.Add((Control) this.Label25);
      this.Frame12.ForeColor = SystemColors.ControlText;
      this.Frame12.Location = new Point(272, 14);
      this.Frame12.Name = "Frame12";
      this.Frame12.RightToLeft = RightToLeft.No;
      this.Frame12.Size = new Size(177, 191);
      this.Frame12.TabIndex = 83;
      this.Frame12.TabStop = false;
      this.Frame12.Visible = false;
      this.PayAnotherBonuses.Cursor = Cursors.Default;
      this.PayAnotherBonuses.ForeColor = SystemColors.ControlText;
      this.PayAnotherBonuses.Location = new Point(90, 70);
      this.PayAnotherBonuses.Name = "PayAnotherBonuses";
      this.PayAnotherBonuses.RightToLeft = RightToLeft.No;
      this.PayAnotherBonuses.Size = new Size(71, 55);
      this.PayAnotherBonuses.TabIndex = 92;
      this.PayAnotherBonuses.Text = "Бонусами с другой карты";
      this.PayAnotherBonuses.Click += new EventHandler(this.PayAnotherBonuses_Click);
      this.PayAnothBomuses.Cursor = Cursors.Default;
      this.PayAnothBomuses.ForeColor = SystemColors.ControlText;
      this.PayAnothBomuses.Location = new Point(208, 56);
      this.PayAnothBomuses.Name = "PayAnothBomuses";
      this.PayAnothBomuses.RightToLeft = RightToLeft.No;
      this.PayAnothBomuses.Size = new Size(49, 57);
      this.PayAnothBomuses.TabIndex = 91;
      this.PayAnothBomuses.Visible = false;
      this.Frame10.Controls.Add((Control) this.PayPrepaid);
      this.Frame10.Controls.Add((Control) this.AlreadyPayedPrepaid);
      this.Frame10.Controls.Add((Control) this.GetPayValPrepaid);
      this.Frame10.Controls.Add((Control) this.Label24);
      this.Frame10.Controls.Add((Control) this.LabelGetPayValPrepaid);
      this.Frame10.ForeColor = SystemColors.ControlText;
      this.Frame10.Location = new Point(6, 285);
      this.Frame10.Name = "Frame10";
      this.Frame10.RightToLeft = RightToLeft.No;
      this.Frame10.Size = new Size(257, 67);
      this.Frame10.TabIndex = 78;
      this.Frame10.TabStop = false;
      this.Frame10.Visible = false;
      this.LabelGetPayValPrepaid.Cursor = Cursors.Default;
      this.LabelGetPayValPrepaid.ForeColor = SystemColors.ControlText;
      this.LabelGetPayValPrepaid.Location = new Point(8, 16);
      this.LabelGetPayValPrepaid.Name = "LabelGetPayValPrepaid";
      this.LabelGetPayValPrepaid.RightToLeft = RightToLeft.No;
      this.LabelGetPayValPrepaid.Size = new Size(73, 21);
      this.LabelGetPayValPrepaid.TabIndex = 79;
      this.LabelGetPayValPrepaid.Text = "Предоплата:";
      this.Frame9.Controls.Add((Control) this.PayCredit);
      this.Frame9.Controls.Add((Control) this.GetPayValCredit);
      this.Frame9.Controls.Add((Control) this.AlreadyPayedCredit);
      this.Frame9.Controls.Add((Control) this.LabelGetPayValCredit);
      this.Frame9.Controls.Add((Control) this.Label21);
      this.Frame9.ForeColor = SystemColors.ControlText;
      this.Frame9.Location = new Point(6, 217);
      this.Frame9.Name = "Frame9";
      this.Frame9.RightToLeft = RightToLeft.No;
      this.Frame9.Size = new Size(257, 67);
      this.Frame9.TabIndex = 75;
      this.Frame9.TabStop = false;
      this.Frame9.Visible = false;
      this.PayCredit.Cursor = Cursors.Default;
      this.PayCredit.ForeColor = SystemColors.ControlText;
      this.PayCredit.Location = new Point(162, 16);
      this.PayCredit.Name = "PayCredit";
      this.PayCredit.RightToLeft = RightToLeft.No;
      this.PayCredit.Size = new Size(89, 44);
      this.PayCredit.TabIndex = 27;
      this.PayCredit.Text = "Кредит";
      this.PayCredit.Click += new EventHandler(this.PayCredit_Click);
      this.GetPayValCredit.AcceptsReturn = true;
      this.GetPayValCredit.BackColor = SystemColors.Window;
      this.GetPayValCredit.Cursor = Cursors.IBeam;
      this.GetPayValCredit.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValCredit.Location = new Point(80, 16);
      this.GetPayValCredit.MaxLength = 0;
      this.GetPayValCredit.Name = "GetPayValCredit";
      this.GetPayValCredit.RightToLeft = RightToLeft.No;
      this.GetPayValCredit.Size = new Size(73, 20);
      this.GetPayValCredit.TabIndex = 25;
      this.GetPayValCredit.Text = "0.00";
      this.AlreadyPayedCredit.AcceptsReturn = true;
      this.AlreadyPayedCredit.BackColor = SystemColors.Window;
      this.AlreadyPayedCredit.Cursor = Cursors.IBeam;
      this.AlreadyPayedCredit.ForeColor = Color.Red;
      this.AlreadyPayedCredit.Location = new Point(80, 40);
      this.AlreadyPayedCredit.MaxLength = 0;
      this.AlreadyPayedCredit.Name = "AlreadyPayedCredit";
      this.AlreadyPayedCredit.RightToLeft = RightToLeft.No;
      this.AlreadyPayedCredit.Size = new Size(73, 20);
      this.AlreadyPayedCredit.TabIndex = 26;
      this.AlreadyPayedCredit.Text = "0.00";
      this.LabelGetPayValCredit.Cursor = Cursors.Default;
      this.LabelGetPayValCredit.ForeColor = SystemColors.ControlText;
      this.LabelGetPayValCredit.Location = new Point(8, 16);
      this.LabelGetPayValCredit.Name = "LabelGetPayValCredit";
      this.LabelGetPayValCredit.RightToLeft = RightToLeft.No;
      this.LabelGetPayValCredit.Size = new Size(57, 17);
      this.LabelGetPayValCredit.TabIndex = 77;
      this.LabelGetPayValCredit.Text = "В кредит:";
      this.Label21.Cursor = Cursors.Default;
      this.Label21.ForeColor = SystemColors.ControlText;
      this.Label21.Location = new Point(8, 32);
      this.Label21.Name = "Label21";
      this.Label21.RightToLeft = RightToLeft.No;
      this.Label21.Size = new Size(57, 33);
      this.Label21.TabIndex = 76;
      this.Label21.Text = "Оплачено в кредит:";
      this.Frame8.Controls.Add((Control) this.PayBonuses);
      this.Frame8.Controls.Add((Control) this.AlreadyPayedBonuses);
      this.Frame8.Controls.Add((Control) this.GetPayValBonuses);
      this.Frame8.Controls.Add((Control) this.Label20);
      this.Frame8.Controls.Add((Control) this.LabelGetPayValBonuses);
      this.Frame8.ForeColor = SystemColors.ControlText;
      this.Frame8.Location = new Point(6, 150);
      this.Frame8.Name = "Frame8";
      this.Frame8.RightToLeft = RightToLeft.No;
      this.Frame8.Size = new Size(257, 67);
      this.Frame8.TabIndex = 72;
      this.Frame8.TabStop = false;
      this.Frame8.Visible = false;
      this.PayBonuses.Cursor = Cursors.Default;
      this.PayBonuses.ForeColor = SystemColors.ControlText;
      this.PayBonuses.Location = new Point(160, 16);
      this.PayBonuses.Name = "PayBonuses";
      this.PayBonuses.RightToLeft = RightToLeft.No;
      this.PayBonuses.Size = new Size(89, 44);
      this.PayBonuses.TabIndex = 24;
      this.PayBonuses.Text = "Бонусами";
      this.PayBonuses.Click += new EventHandler(this.PayBonuses_Click);
      this.AlreadyPayedBonuses.AcceptsReturn = true;
      this.AlreadyPayedBonuses.BackColor = SystemColors.Window;
      this.AlreadyPayedBonuses.Cursor = Cursors.IBeam;
      this.AlreadyPayedBonuses.ForeColor = Color.Red;
      this.AlreadyPayedBonuses.Location = new Point(80, 40);
      this.AlreadyPayedBonuses.MaxLength = 0;
      this.AlreadyPayedBonuses.Name = "AlreadyPayedBonuses";
      this.AlreadyPayedBonuses.RightToLeft = RightToLeft.No;
      this.AlreadyPayedBonuses.Size = new Size(73, 20);
      this.AlreadyPayedBonuses.TabIndex = 23;
      this.AlreadyPayedBonuses.Text = "0.00";
      this.GetPayValBonuses.AcceptsReturn = true;
      this.GetPayValBonuses.BackColor = SystemColors.Window;
      this.GetPayValBonuses.Cursor = Cursors.IBeam;
      this.GetPayValBonuses.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValBonuses.Location = new Point(80, 16);
      this.GetPayValBonuses.MaxLength = 0;
      this.GetPayValBonuses.Name = "GetPayValBonuses";
      this.GetPayValBonuses.RightToLeft = RightToLeft.No;
      this.GetPayValBonuses.Size = new Size(73, 20);
      this.GetPayValBonuses.TabIndex = 22;
      this.GetPayValBonuses.Text = "0.00";
      this.Label20.Cursor = Cursors.Default;
      this.Label20.ForeColor = SystemColors.ControlText;
      this.Label20.Location = new Point(8, 32);
      this.Label20.Name = "Label20";
      this.Label20.RightToLeft = RightToLeft.No;
      this.Label20.Size = new Size(66, 31);
      this.Label20.TabIndex = 74;
      this.Label20.Text = "Оплачено бонусами:";
      this.LabelGetPayValBonuses.Cursor = Cursors.Default;
      this.LabelGetPayValBonuses.ForeColor = SystemColors.ControlText;
      this.LabelGetPayValBonuses.Location = new Point(8, 16);
      this.LabelGetPayValBonuses.Name = "LabelGetPayValBonuses";
      this.LabelGetPayValBonuses.RightToLeft = RightToLeft.No;
      this.LabelGetPayValBonuses.Size = new Size(65, 17);
      this.LabelGetPayValBonuses.TabIndex = 73;
      this.LabelGetPayValBonuses.Text = "Бонусами:";
      this.Frame7.Controls.Add((Control) this.PayCard);
      this.Frame7.Controls.Add((Control) this.GetPayValCard);
      this.Frame7.Controls.Add((Control) this.AlreadyPayedCard);
      this.Frame7.Controls.Add((Control) this.LabelGetPayValCard);
      this.Frame7.Controls.Add((Control) this.Label17);
      this.Frame7.ForeColor = SystemColors.ControlText;
      this.Frame7.Location = new Point(8, 83);
      this.Frame7.Name = "Frame7";
      this.Frame7.RightToLeft = RightToLeft.No;
      this.Frame7.Size = new Size(257, 67);
      this.Frame7.TabIndex = 69;
      this.Frame7.TabStop = false;
      this.Frame7.Visible = false;
      this.PayCard.Cursor = Cursors.Default;
      this.PayCard.ForeColor = SystemColors.ControlText;
      this.PayCard.Location = new Point(162, 16);
      this.PayCard.Name = "PayCard";
      this.PayCard.RightToLeft = RightToLeft.No;
      this.PayCard.Size = new Size(89, 44);
      this.PayCard.TabIndex = 21;
      this.PayCard.Text = "Картой";
      this.PayCard.Click += new EventHandler(this.PayCard_Click);
      this.GetPayValCard.AcceptsReturn = true;
      this.GetPayValCard.BackColor = SystemColors.Window;
      this.GetPayValCard.Cursor = Cursors.IBeam;
      this.GetPayValCard.ForeColor = Color.FromArgb(0, 192, 0);
      this.GetPayValCard.Location = new Point(80, 16);
      this.GetPayValCard.MaxLength = 0;
      this.GetPayValCard.Name = "GetPayValCard";
      this.GetPayValCard.RightToLeft = RightToLeft.No;
      this.GetPayValCard.Size = new Size(73, 20);
      this.GetPayValCard.TabIndex = 19;
      this.GetPayValCard.Text = "0.00";
      this.AlreadyPayedCard.AcceptsReturn = true;
      this.AlreadyPayedCard.BackColor = SystemColors.Window;
      this.AlreadyPayedCard.Cursor = Cursors.IBeam;
      this.AlreadyPayedCard.ForeColor = Color.Red;
      this.AlreadyPayedCard.Location = new Point(80, 40);
      this.AlreadyPayedCard.MaxLength = 0;
      this.AlreadyPayedCard.Name = "AlreadyPayedCard";
      this.AlreadyPayedCard.RightToLeft = RightToLeft.No;
      this.AlreadyPayedCard.Size = new Size(73, 20);
      this.AlreadyPayedCard.TabIndex = 20;
      this.AlreadyPayedCard.Text = "0.00";
      this.LabelGetPayValCard.Cursor = Cursors.Default;
      this.LabelGetPayValCard.ForeColor = SystemColors.ControlText;
      this.LabelGetPayValCard.Location = new Point(8, 16);
      this.LabelGetPayValCard.Name = "LabelGetPayValCard";
      this.LabelGetPayValCard.RightToLeft = RightToLeft.No;
      this.LabelGetPayValCard.Size = new Size(49, 17);
      this.LabelGetPayValCard.TabIndex = 71;
      this.LabelGetPayValCard.Text = "Картой:";
      this.Label17.Cursor = Cursors.Default;
      this.Label17.ForeColor = SystemColors.ControlText;
      this.Label17.Location = new Point(8, 32);
      this.Label17.Name = "Label17";
      this.Label17.RightToLeft = RightToLeft.No;
      this.Label17.Size = new Size(57, 33);
      this.Label17.TabIndex = 70;
      this.Label17.Text = "Оплачено картой:";
      this.GroupBox1.BackColor = Color.Transparent;
      this.GroupBox1.Controls.Add((Control) this.RB_RS232);
      this.GroupBox1.Controls.Add((Control) this.RB_Channel);
      this.GroupBox1.Enabled = false;
      this.GroupBox1.Location = new Point(436, 5);
      this.GroupBox1.Name = "GroupBox1";
      this.GroupBox1.Size = new Size(66, 48);
      this.GroupBox1.TabIndex = 228;
      this.GroupBox1.TabStop = false;
      this.GroupBox1.Text = "Канал";
      this.RB_RS232.AutoSize = true;
      this.RB_RS232.Location = new Point(5, 26);
      this.RB_RS232.Name = "RB_RS232";
      this.RB_RS232.Size = new Size(58, 17);
      this.RB_RS232.TabIndex = 169;
      this.RB_RS232.TabStop = true;
      this.RB_RS232.Text = "RS232";
      this.RB_RS232.UseVisualStyleBackColor = true;
      this.RB_Channel.AutoSize = true;
      this.RB_Channel.Checked = true;
      this.RB_Channel.Location = new Point(5, 11);
      this.RB_Channel.Name = "RB_Channel";
      this.RB_Channel.Size = new Size(46, 17);
      this.RB_Channel.TabIndex = 168;
      this.RB_Channel.TabStop = true;
      this.RB_Channel.Text = "TCP";
      this.RB_Channel.UseVisualStyleBackColor = true;
      this.ButtonRollback.Location = new Point(7, 78);
      this.ButtonRollback.Name = "ButtonRollback";
      this.ButtonRollback.Size = new Size(121, 20);
      this.ButtonRollback.TabIndex = 160;
      this.ButtonRollback.Text = "Rollback";
      this.ButtonRollback.Click += new EventHandler(this.ButtonRollback_Click);
      this.CardsLinking.Cursor = Cursors.Default;
      this.CardsLinking.ForeColor = SystemColors.ControlText;
      this.CardsLinking.Location = new Point(31, 21);
      this.CardsLinking.Name = "CardsLinking";
      this.CardsLinking.RightToLeft = RightToLeft.No;
      this.CardsLinking.Size = new Size(153, 25);
      this.CardsLinking.TabIndex = 8;
      this.CardsLinking.Text = "Привязать карту";
      this.CardsLinking.Click += new EventHandler(this.CardsLinking_Click);
      this.AmountLinkingBonuses.AcceptsReturn = true;
      this.AmountLinkingBonuses.BackColor = SystemColors.Window;
      this.AmountLinkingBonuses.Cursor = Cursors.IBeam;
      this.AmountLinkingBonuses.ForeColor = SystemColors.WindowText;
      this.AmountLinkingBonuses.Location = new Point(119, 24);
      this.AmountLinkingBonuses.MaxLength = 0;
      this.AmountLinkingBonuses.Name = "AmountLinkingBonuses";
      this.AmountLinkingBonuses.RightToLeft = RightToLeft.No;
      this.AmountLinkingBonuses.Size = new Size(81, 20);
      this.AmountLinkingBonuses.TabIndex = 53;
      this.AmountLinkingBonuses.Text = "0,00";
      this.AmountLinkingBonuses.Visible = false;
      this.AmountForCancel.AcceptsReturn = true;
      this.AmountForCancel.BackColor = SystemColors.Window;
      this.AmountForCancel.Cursor = Cursors.IBeam;
      this.AmountForCancel.ForeColor = SystemColors.WindowText;
      this.AmountForCancel.Location = new Point(102, 13);
      this.AmountForCancel.MaxLength = 0;
      this.AmountForCancel.Name = "AmountForCancel";
      this.AmountForCancel.RightToLeft = RightToLeft.No;
      this.AmountForCancel.Size = new Size(81, 20);
      this.AmountForCancel.TabIndex = 3;
      this.AmountForCancel.Text = "0,00";
      this.AmountSubstitutionCirculatings.AcceptsReturn = true;
      this.AmountSubstitutionCirculatings.BackColor = SystemColors.Window;
      this.AmountSubstitutionCirculatings.Cursor = Cursors.IBeam;
      this.AmountSubstitutionCirculatings.ForeColor = SystemColors.WindowText;
      this.AmountSubstitutionCirculatings.Location = new Point(119, 45);
      this.AmountSubstitutionCirculatings.MaxLength = 0;
      this.AmountSubstitutionCirculatings.Name = "AmountSubstitutionCirculatings";
      this.AmountSubstitutionCirculatings.RightToLeft = RightToLeft.No;
      this.AmountSubstitutionCirculatings.Size = new Size(81, 20);
      this.AmountSubstitutionCirculatings.TabIndex = 10;
      this.AmountSubstitutionCirculatings.Text = "0,00";
      this.AmountSubstitutionCirculatings.Visible = false;
      this.GetTimeout.AcceptsReturn = true;
      this.GetTimeout.BackColor = SystemColors.Window;
      this.GetTimeout.Cursor = Cursors.IBeam;
      this.GetTimeout.ForeColor = SystemColors.WindowText;
      this.GetTimeout.Location = new Point(924, 268);
      this.GetTimeout.MaxLength = 0;
      this.GetTimeout.Name = "GetTimeout";
      this.GetTimeout.RightToLeft = RightToLeft.No;
      this.GetTimeout.Size = new Size(49, 20);
      this.GetTimeout.TabIndex = 207;
      this.GetTimeout.Text = "10";
      this.GetTimeout.Visible = false;
      this.FormAppliance.Cursor = Cursors.Default;
      this.FormAppliance.ForeColor = SystemColors.ControlText;
      this.FormAppliance.Location = new Point(8, 24);
      this.FormAppliance.Name = "FormAppliance";
      this.FormAppliance.RightToLeft = RightToLeft.No;
      this.FormAppliance.Size = new Size(121, 21);
      this.FormAppliance.TabIndex = 159;
      this.FormAppliance.Text = "Принять анкету";
      this.FormAppliance.Click += new EventHandler(this.FormAppliance_Click);
      this.LabelClientID.Cursor = Cursors.Default;
      this.LabelClientID.ForeColor = SystemColors.ControlText;
      this.LabelClientID.Location = new Point(77, 9);
      this.LabelClientID.Name = "LabelClientID";
      this.LabelClientID.RightToLeft = RightToLeft.No;
      this.LabelClientID.Size = new Size(246, 18);
      this.LabelClientID.TabIndex = 100;
      this.Frame4.Controls.Add((Control) this.ButtonRollback);
      this.Frame4.Controls.Add((Control) this.FormAppliance);
      this.Frame4.Controls.Add((Control) this.ChangePIN);
      this.Frame4.ForeColor = SystemColors.ControlText;
      this.Frame4.Location = new Point(233, 505);
      this.Frame4.Name = "Frame4";
      this.Frame4.RightToLeft = RightToLeft.No;
      this.Frame4.Size = new Size(137, 146);
      this.Frame4.TabIndex = 209;
      this.Frame4.TabStop = false;
      this.Frame4.Text = "Доп. действия";
      this.ChangePIN.Cursor = Cursors.Default;
      this.ChangePIN.ForeColor = SystemColors.ControlText;
      this.ChangePIN.Location = new Point(7, 51);
      this.ChangePIN.Name = "ChangePIN";
      this.ChangePIN.RightToLeft = RightToLeft.No;
      this.ChangePIN.Size = new Size(121, 21);
      this.ChangePIN.TabIndex = 12;
      this.ChangePIN.Text = "Сменить PIN карты";
      this.ChangePIN.Click += new EventHandler(this.ChangePIN_Click);
      this.BpRrnField.AcceptsReturn = true;
      this.BpRrnField.BackColor = SystemColors.Window;
      this.BpRrnField.Cursor = Cursors.IBeam;
      this.BpRrnField.ForeColor = SystemColors.WindowText;
      this.BpRrnField.Location = new Point(102, 35);
      this.BpRrnField.MaxLength = 0;
      this.BpRrnField.Name = "BpRrnField";
      this.BpRrnField.RightToLeft = RightToLeft.No;
      this.BpRrnField.Size = new Size(129, 20);
      this.BpRrnField.TabIndex = 4;
      this.BpRrnField.Text = "0";
      this.Check_Deposit.Checked = true;
      this.Check_Deposit.CheckState = CheckState.Checked;
      this.Check_Deposit.Cursor = Cursors.Default;
      this.Check_Deposit.ForeColor = SystemColors.ControlText;
      this.Check_Deposit.Location = new Point(960, 218);
      this.Check_Deposit.Name = "Check_Deposit";
      this.Check_Deposit.RightToLeft = RightToLeft.No;
      this.Check_Deposit.Size = new Size(129, 17);
      this.Check_Deposit.TabIndex = 210;
      this.Check_Deposit.Text = "Бонусы как задатки";
      this.Label11.Cursor = Cursors.Default;
      this.Label11.ForeColor = SystemColors.ControlText;
      this.Label11.Location = new Point(7, 18);
      this.Label11.Name = "Label11";
      this.Label11.RightToLeft = RightToLeft.No;
      this.Label11.Size = new Size(89, 19);
      this.Label11.TabIndex = 52;
      this.Label11.Text = "Сумма (руб.):";
      this.GoodsAmount.AcceptsReturn = true;
      this.GoodsAmount.BackColor = SystemColors.Window;
      this.GoodsAmount.Cursor = Cursors.IBeam;
      this.GoodsAmount.ForeColor = SystemColors.WindowText;
      this.GoodsAmount.Location = new Point(360, 67);
      this.GoodsAmount.MaxLength = 0;
      this.GoodsAmount.Multiline = true;
      this.GoodsAmount.Name = "GoodsAmount";
      this.GoodsAmount.RightToLeft = RightToLeft.No;
      this.GoodsAmount.ScrollBars = ScrollBars.Horizontal;
      this.GoodsAmount.Size = new Size(65, 217);
      this.GoodsAmount.TabIndex = 198;
      this.GoodsAmount.WordWrap = false;
      this.GoodsQuantity.AcceptsReturn = true;
      this.GoodsQuantity.BackColor = SystemColors.Window;
      this.GoodsQuantity.Cursor = Cursors.IBeam;
      this.GoodsQuantity.ForeColor = SystemColors.WindowText;
      this.GoodsQuantity.Location = new Point(240, 67);
      this.GoodsQuantity.MaxLength = 0;
      this.GoodsQuantity.Multiline = true;
      this.GoodsQuantity.Name = "GoodsQuantity";
      this.GoodsQuantity.RightToLeft = RightToLeft.No;
      this.GoodsQuantity.ScrollBars = ScrollBars.Horizontal;
      this.GoodsQuantity.Size = new Size(57, 217);
      this.GoodsQuantity.TabIndex = 195;
      this.GoodsQuantity.WordWrap = false;
      this.NewOrder.Location = new Point(11, 15);
      this.NewOrder.Name = "NewOrder";
      this.NewOrder.RightToLeft = RightToLeft.No;
      this.NewOrder.Size = new Size(417, 25);
      this.NewOrder.TabIndex = 194;
      this.NewOrder.Text = "Новое обслуживание";
      this.NewOrder.Visible = false;
      this.NewOrder.Click += new EventHandler(this.NewOrder_Click);
      this.PrnText.AcceptsReturn = true;
      this.PrnText.BackColor = Color.White;
      this.PrnText.Cursor = Cursors.IBeam;
      this.PrnText.ForeColor = SystemColors.WindowText;
      this.PrnText.Location = new Point(505, 236);
      this.PrnText.MaxLength = 0;
      this.PrnText.Multiline = true;
      this.PrnText.Name = "PrnText";
      this.PrnText.RightToLeft = RightToLeft.No;
      this.PrnText.ScrollBars = ScrollBars.Vertical;
      this.PrnText.Size = new Size(273, 53);
      this.PrnText.TabIndex = 193;
      this.BpRrnCancel.Cursor = Cursors.Default;
      this.BpRrnCancel.ForeColor = SystemColors.ControlText;
      this.BpRrnCancel.Location = new Point(239, 32);
      this.BpRrnCancel.Name = "BpRrnCancel";
      this.BpRrnCancel.RightToLeft = RightToLeft.No;
      this.BpRrnCancel.Size = new Size(105, 25);
      this.BpRrnCancel.TabIndex = 5;
      this.BpRrnCancel.Text = "Отменить";
      this.BpRrnCancel.Click += new EventHandler(this.BpRrnCancel_Click);
      this.GoodsPrices.AcceptsReturn = true;
      this.GoodsPrices.BackColor = SystemColors.Window;
      this.GoodsPrices.Cursor = Cursors.IBeam;
      this.GoodsPrices.ForeColor = SystemColors.WindowText;
      this.GoodsPrices.Location = new Point(296, 67);
      this.GoodsPrices.MaxLength = 0;
      this.GoodsPrices.Multiline = true;
      this.GoodsPrices.Name = "GoodsPrices";
      this.GoodsPrices.RightToLeft = RightToLeft.No;
      this.GoodsPrices.ScrollBars = ScrollBars.Horizontal;
      this.GoodsPrices.Size = new Size(65, 217);
      this.GoodsPrices.TabIndex = 191;
      this.GoodsPrices.WordWrap = false;
      this.ResetBtn.Cursor = Cursors.Default;
      this.ResetBtn.Enabled = false;
      this.ResetBtn.ForeColor = SystemColors.ControlText;
      this.ResetBtn.Location = new Point(368, 15);
      this.ResetBtn.Name = "ResetBtn";
      this.ResetBtn.RightToLeft = RightToLeft.No;
      this.ResetBtn.Size = new Size(57, 25);
      this.ResetBtn.TabIndex = 188;
      this.ResetBtn.TabStop = false;
      this.ResetBtn.Text = "Сброс";
      this.ResetBtn.Click += new EventHandler(this.ResetBtn_Click);
      this.OK.Cursor = Cursors.Default;
      this.OK.Enabled = false;
      this.OK.ForeColor = SystemColors.ControlText;
      this.OK.Location = new Point(311, 15);
      this.OK.Name = "OK";
      this.OK.RightToLeft = RightToLeft.No;
      this.OK.Size = new Size(51, 24);
      this.OK.TabIndex = 187;
      this.OK.TabStop = false;
      this.OK.Text = "OK";
      this.BarCoder.AcceptsReturn = true;
      this.BarCoder.BackColor = SystemColors.Window;
      this.BarCoder.Cursor = Cursors.IBeam;
      this.BarCoder.Enabled = false;
      this.BarCoder.ForeColor = SystemColors.WindowText;
      this.BarCoder.Location = new Point(96, 15);
      this.BarCoder.MaxLength = 0;
      this.BarCoder.Name = "BarCoder";
      this.BarCoder.RightToLeft = RightToLeft.No;
      this.BarCoder.Size = new Size(57, 20);
      this.BarCoder.TabIndex = 184;
      this.CoodCount.AcceptsReturn = true;
      this.CoodCount.BackColor = SystemColors.Window;
      this.CoodCount.Cursor = Cursors.IBeam;
      this.CoodCount.Enabled = false;
      this.CoodCount.ForeColor = SystemColors.WindowText;
      this.CoodCount.Location = new Point(240, 16);
      this.CoodCount.MaxLength = 0;
      this.CoodCount.Name = "CoodCount";
      this.CoodCount.RightToLeft = RightToLeft.No;
      this.CoodCount.Size = new Size(65, 20);
      this.CoodCount.TabIndex = 185;
      this.CoodCount.Text = "1.000";
      this.FrameBprrnCancel.Controls.Add((Control) this.Label9);
      this.FrameBprrnCancel.Controls.Add((Control) this.Label8);
      this.FrameBprrnCancel.Controls.Add((Control) this.AmountForCancel);
      this.FrameBprrnCancel.Controls.Add((Control) this.BpRrnField);
      this.FrameBprrnCancel.Controls.Add((Control) this.BpRrnCancel);
      this.FrameBprrnCancel.ForeColor = SystemColors.ControlText;
      this.FrameBprrnCancel.Location = new Point(8, 376);
      this.FrameBprrnCancel.Name = "FrameBprrnCancel";
      this.FrameBprrnCancel.RightToLeft = RightToLeft.No;
      this.FrameBprrnCancel.Size = new Size(361, 67);
      this.FrameBprrnCancel.TabIndex = 203;
      this.FrameBprrnCancel.TabStop = false;
      this.FrameBprrnCancel.Text = "Отмена операции";
      this.Label9.Cursor = Cursors.Default;
      this.Label9.ForeColor = SystemColors.ControlText;
      this.Label9.Location = new Point(7, 38);
      this.Label9.Name = "Label9";
      this.Label9.RightToLeft = RightToLeft.No;
      this.Label9.Size = new Size(57, 25);
      this.Label9.TabIndex = 50;
      this.Label9.Text = "BpRrn:";
      this.Label8.Cursor = Cursors.Default;
      this.Label8.ForeColor = SystemColors.ControlText;
      this.Label8.Location = new Point(7, 16);
      this.Label8.Name = "Label8";
      this.Label8.RightToLeft = RightToLeft.No;
      this.Label8.Size = new Size(89, 33);
      this.Label8.TabIndex = 49;
      this.Label8.Text = "Сумма (руб.):";
      this.GoodsCode.AcceptsReturn = true;
      this.GoodsCode.BackColor = SystemColors.Window;
      this.GoodsCode.Cursor = Cursors.IBeam;
      this.GoodsCode.ForeColor = SystemColors.WindowText;
      this.GoodsCode.Location = new Point(8, 67);
      this.GoodsCode.MaxLength = 0;
      this.GoodsCode.Multiline = true;
      this.GoodsCode.Name = "GoodsCode";
      this.GoodsCode.RightToLeft = RightToLeft.No;
      this.GoodsCode.ScrollBars = ScrollBars.Horizontal;
      this.GoodsCode.Size = new Size(57, 217);
      this.GoodsCode.TabIndex = 211;
      this.GoodsCode.WordWrap = false;
      this.Label28.Cursor = Cursors.Default;
      this.Label28.ForeColor = SystemColors.ControlText;
      this.Label28.Location = new Point(6, 10);
      this.Label28.Name = "Label28";
      this.Label28.RightToLeft = RightToLeft.No;
      this.Label28.Size = new Size(65, 17);
      this.Label28.TabIndex = 99;
      this.Label28.Text = "ID клиента:";
      this.ComboBoxCardType.FormattingEnabled = true;
      this.ComboBoxCardType.Items.AddRange(new object[13]
      {
        (object) "1 - Синхронная карта (ЛНР)",
        (object) "2 - Карта Mifare (ЛНР)",
        (object) "3 - Талон",
        (object) "4 - Чиповая карта с приложением LifeStyle Point",
        (object) "5 - Чиповая карта с приложением PetrolPlus",
        (object) "6 - Чиповая карта с приложением МПС",
        (object) "7 - Чиповая комбинированная карта (PetrolPlus + LifeStyle Point)",
        (object) "8 - Чиповая кобрендинговая карта (LifeStyle Point + МПС)",
        (object) "9 - Чиповая кобрендинговая карта (PetrolPlus + МПС)",
        (object) "10 - Чиповая комбинированная кобрендинговая карта (PetrolPlus + LifeStyle Point + МПС)",
        (object) "11 - Карта с магнитной полосой (только бонусная)",
        (object) "12 - Карта с магнитной полосой (только МПС)",
        (object) "13 - Кобрендинговая карта с магнитной полосой (LifeStyle Point + МПС)"
      });
      this.ComboBoxCardType.Location = new Point(72, 36);
      this.ComboBoxCardType.MaxDropDownItems = 13;
      this.ComboBoxCardType.Name = "ComboBoxCardType";
      this.ComboBoxCardType.Size = new Size(265, 21);
      this.ComboBoxCardType.TabIndex = 155;
      this.ComboBoxCardType.SelectedIndexChanged += new EventHandler(this.ComboBoxCardType_SelectedIndexChanged);
      this.MagTrack1Field.AcceptsReturn = true;
      this.MagTrack1Field.BackColor = SystemColors.Window;
      this.MagTrack1Field.Cursor = Cursors.IBeam;
      this.MagTrack1Field.ForeColor = SystemColors.WindowText;
      this.MagTrack1Field.Location = new Point(72, 63);
      this.MagTrack1Field.MaxLength = 0;
      this.MagTrack1Field.Name = "MagTrack1Field";
      this.MagTrack1Field.RightToLeft = RightToLeft.No;
      this.MagTrack1Field.Size = new Size(265, 20);
      this.MagTrack1Field.TabIndex = 151;
      this.Frame23.Controls.Add((Control) this.ComboBoxCardType);
      this.Frame23.Controls.Add((Control) this.MagTrack1Field);
      this.Frame23.Controls.Add((Control) this.Check_TransmitCardData);
      this.Frame23.Controls.Add((Control) this.MagTrack2Field);
      this.Frame23.Controls.Add((Control) this.Label50);
      this.Frame23.Controls.Add((Control) this.Label49);
      this.Frame23.Controls.Add((Control) this.Label47);
      this.Frame23.Enabled = false;
      this.Frame23.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Frame23.ForeColor = SystemColors.ControlText;
      this.Frame23.Location = new Point(784, 11);
      this.Frame23.Name = "Frame23";
      this.Frame23.RightToLeft = RightToLeft.No;
      this.Frame23.Size = new Size(345, 118);
      this.Frame23.TabIndex = 223;
      this.Frame23.TabStop = false;
      this.Frame23.Text = "Данные карты";
      this.Check_TransmitCardData.Checked = true;
      this.Check_TransmitCardData.CheckState = CheckState.Checked;
      this.Check_TransmitCardData.Cursor = Cursors.Default;
      this.Check_TransmitCardData.ForeColor = SystemColors.ControlText;
      this.Check_TransmitCardData.Location = new Point(72, 15);
      this.Check_TransmitCardData.Name = "Check_TransmitCardData";
      this.Check_TransmitCardData.RightToLeft = RightToLeft.No;
      this.Check_TransmitCardData.Size = new Size(185, 18);
      this.Check_TransmitCardData.TabIndex = 149;
      this.Check_TransmitCardData.Text = "Передавать данные карты";
      this.MagTrack2Field.AcceptsReturn = true;
      this.MagTrack2Field.BackColor = SystemColors.Window;
      this.MagTrack2Field.Cursor = Cursors.IBeam;
      this.MagTrack2Field.ForeColor = SystemColors.WindowText;
      this.MagTrack2Field.Location = new Point(72, 89);
      this.MagTrack2Field.MaxLength = 0;
      this.MagTrack2Field.Name = "MagTrack2Field";
      this.MagTrack2Field.RightToLeft = RightToLeft.No;
      this.MagTrack2Field.Size = new Size(265, 20);
      this.MagTrack2Field.TabIndex = 148;
      this.Label50.Cursor = Cursors.Default;
      this.Label50.ForeColor = SystemColors.ControlText;
      this.Label50.Location = new Point(6, 39);
      this.Label50.Name = "Label50";
      this.Label50.RightToLeft = RightToLeft.No;
      this.Label50.Size = new Size(69, 18);
      this.Label50.TabIndex = 154;
      this.Label50.Text = "Тип карты:";
      this.Label49.Cursor = Cursors.Default;
      this.Label49.ForeColor = SystemColors.ControlText;
      this.Label49.Location = new Point(6, 66);
      this.Label49.Name = "Label49";
      this.Label49.RightToLeft = RightToLeft.No;
      this.Label49.Size = new Size(69, 20);
      this.Label49.TabIndex = 152;
      this.Label49.Text = "MagTrack1:";
      this.Label47.Cursor = Cursors.Default;
      this.Label47.ForeColor = SystemColors.ControlText;
      this.Label47.Location = new Point(6, 92);
      this.Label47.Name = "Label47";
      this.Label47.RightToLeft = RightToLeft.No;
      this.Label47.Size = new Size(69, 20);
      this.Label47.TabIndex = 150;
      this.Label47.Text = "MagTrack2:";
      this.Timer1.Interval = 50;
      this.Timer1.Tick += new EventHandler(this.Timer1_Tick);
      this.Check_UseKLProtocol.Checked = true;
      this.Check_UseKLProtocol.CheckState = CheckState.Checked;
      this.Check_UseKLProtocol.Cursor = Cursors.Default;
      this.Check_UseKLProtocol.ForeColor = SystemColors.ControlText;
      this.Check_UseKLProtocol.Location = new Point(7, 9);
      this.Check_UseKLProtocol.Name = "Check_UseKLProtocol";
      this.Check_UseKLProtocol.RightToLeft = RightToLeft.No;
      this.Check_UseKLProtocol.Size = new Size(167, 31);
      this.Check_UseKLProtocol.TabIndex = 157;
      this.Check_UseKLProtocol.Text = "Использовать КН протокол";
      this.Frame22.Controls.Add((Control) this.Label48);
      this.Frame22.Controls.Add((Control) this.LabelClientCardID);
      this.Frame22.ForeColor = SystemColors.ControlText;
      this.Frame22.Location = new Point(784, 308);
      this.Frame22.Name = "Frame22";
      this.Frame22.RightToLeft = RightToLeft.No;
      this.Frame22.Size = new Size(335, 32);
      this.Frame22.TabIndex = 222;
      this.Frame22.TabStop = false;
      this.Frame22.Visible = false;
      this.Label48.Cursor = Cursors.Default;
      this.Label48.ForeColor = SystemColors.ControlText;
      this.Label48.Location = new Point(6, 10);
      this.Label48.Name = "Label48";
      this.Label48.RightToLeft = RightToLeft.No;
      this.Label48.Size = new Size(56, 17);
      this.Label48.TabIndex = 146;
      this.Label48.Text = "ID карты:";
      this.Label48.Visible = false;
      this.LabelClientCardID.Cursor = Cursors.Default;
      this.LabelClientCardID.ForeColor = SystemColors.ControlText;
      this.LabelClientCardID.Location = new Point(78, 10);
      this.LabelClientCardID.Name = "LabelClientCardID";
      this.LabelClientCardID.RightToLeft = RightToLeft.No;
      this.LabelClientCardID.Size = new Size(217, 17);
      this.LabelClientCardID.TabIndex = 145;
      this.LabelClientCardID.Visible = false;
      this.Frame20.Controls.Add((Control) this.AmountForCancelEMV);
      this.Frame20.Controls.Add((Control) this.MerchRrnField);
      this.Frame20.Controls.Add((Control) this.MerchRrnCancel);
      this.Frame20.Controls.Add((Control) this.Label43);
      this.Frame20.Controls.Add((Control) this.Label42);
      this.Frame20.ForeColor = SystemColors.ControlText;
      this.Frame20.Location = new Point(8, 654);
      this.Frame20.Name = "Frame20";
      this.Frame20.RightToLeft = RightToLeft.No;
      this.Frame20.Size = new Size(361, 80);
      this.Frame20.TabIndex = 217;
      this.Frame20.TabStop = false;
      this.Frame20.Text = "Отмена МПС";
      this.AmountForCancelEMV.AcceptsReturn = true;
      this.AmountForCancelEMV.BackColor = SystemColors.Window;
      this.AmountForCancelEMV.Cursor = Cursors.IBeam;
      this.AmountForCancelEMV.ForeColor = SystemColors.WindowText;
      this.AmountForCancelEMV.Location = new Point(112, 16);
      this.AmountForCancelEMV.MaxLength = 0;
      this.AmountForCancelEMV.Name = "AmountForCancelEMV";
      this.AmountForCancelEMV.RightToLeft = RightToLeft.No;
      this.AmountForCancelEMV.Size = new Size(81, 20);
      this.AmountForCancelEMV.TabIndex = 13;
      this.AmountForCancelEMV.Text = "0,00";
      this.MerchRrnField.AcceptsReturn = true;
      this.MerchRrnField.BackColor = SystemColors.Window;
      this.MerchRrnField.Cursor = Cursors.IBeam;
      this.MerchRrnField.ForeColor = SystemColors.WindowText;
      this.MerchRrnField.Location = new Point(112, 48);
      this.MerchRrnField.MaxLength = 0;
      this.MerchRrnField.Name = "MerchRrnField";
      this.MerchRrnField.RightToLeft = RightToLeft.No;
      this.MerchRrnField.Size = new Size(129, 20);
      this.MerchRrnField.TabIndex = 14;
      this.MerchRrnField.Text = "0";
      this.MerchRrnCancel.Cursor = Cursors.Default;
      this.MerchRrnCancel.ForeColor = SystemColors.ControlText;
      this.MerchRrnCancel.Location = new Point(248, 48);
      this.MerchRrnCancel.Name = "MerchRrnCancel";
      this.MerchRrnCancel.RightToLeft = RightToLeft.No;
      this.MerchRrnCancel.Size = new Size(105, 25);
      this.MerchRrnCancel.TabIndex = 15;
      this.MerchRrnCancel.Text = "Отменить";
      this.MerchRrnCancel.Click += new EventHandler(this.MerchRrnCancel_Click);
      this.Label43.Cursor = Cursors.Default;
      this.Label43.ForeColor = SystemColors.ControlText;
      this.Label43.Location = new Point(8, 16);
      this.Label43.Name = "Label43";
      this.Label43.RightToLeft = RightToLeft.No;
      this.Label43.Size = new Size(89, 33);
      this.Label43.TabIndex = 130;
      this.Label43.Text = "Сумма (руб.):";
      this.Label42.Cursor = Cursors.Default;
      this.Label42.ForeColor = SystemColors.ControlText;
      this.Label42.Location = new Point(56, 48);
      this.Label42.Name = "Label42";
      this.Label42.RightToLeft = RightToLeft.No;
      this.Label42.Size = new Size(41, 25);
      this.Label42.TabIndex = 129;
      this.Label42.Text = "RRN:";
      this.Frame14.Controls.Add((Control) this.LabelClientID);
      this.Frame14.Controls.Add((Control) this.Label28);
      this.Frame14.ForeColor = SystemColors.ControlText;
      this.Frame14.Location = new Point(784, 344);
      this.Frame14.Name = "Frame14";
      this.Frame14.RightToLeft = RightToLeft.No;
      this.Frame14.Size = new Size(336, 32);
      this.Frame14.TabIndex = 216;
      this.Frame14.TabStop = false;
      this.Frame14.Visible = false;
      this.Check_ModificationBeforePayments.Cursor = Cursors.Default;
      this.Check_ModificationBeforePayments.ForeColor = SystemColors.ControlText;
      this.Check_ModificationBeforePayments.Location = new Point(960, 234);
      this.Check_ModificationBeforePayments.Name = "Check_ModificationBeforePayments";
      this.Check_ModificationBeforePayments.RightToLeft = RightToLeft.No;
      this.Check_ModificationBeforePayments.Size = new Size(145, 17);
      this.Check_ModificationBeforePayments.TabIndex = 218;
      this.Check_ModificationBeforePayments.Text = "Модификация до оплат";
      this.Check_ModificationBeforePayments.Visible = false;
      this.Check_DiscountByGoods.Cursor = Cursors.Default;
      this.Check_DiscountByGoods.ForeColor = SystemColors.ControlText;
      this.Check_DiscountByGoods.Location = new Point(960, 250);
      this.Check_DiscountByGoods.Name = "Check_DiscountByGoods";
      this.Check_DiscountByGoods.RightToLeft = RightToLeft.No;
      this.Check_DiscountByGoods.Size = new Size(129, 17);
      this.Check_DiscountByGoods.TabIndex = 219;
      this.Check_DiscountByGoods.Text = "Скидка товаром";
      this.CheckAutoGenECROpId.Cursor = Cursors.Default;
      this.CheckAutoGenECROpId.ForeColor = SystemColors.ControlText;
      this.CheckAutoGenECROpId.Location = new Point(184, 11);
      this.CheckAutoGenECROpId.Name = "CheckAutoGenECROpId";
      this.CheckAutoGenECROpId.RightToLeft = RightToLeft.No;
      this.CheckAutoGenECROpId.Size = new Size(121, 18);
      this.CheckAutoGenECROpId.TabIndex = 170;
      this.CheckAutoGenECROpId.Text = "Автогенерировать ECROpId";
      this.CheckAutoGenECROpId.CheckedChanged += new EventHandler(this.CheckAutoGenECROpId_CheckedChanged);
      this.AmountPrepaid.AcceptsReturn = true;
      this.AmountPrepaid.BackColor = SystemColors.Window;
      this.AmountPrepaid.Cursor = Cursors.IBeam;
      this.AmountPrepaid.ForeColor = SystemColors.WindowText;
      this.AmountPrepaid.Location = new Point(102, 19);
      this.AmountPrepaid.MaxLength = 0;
      this.AmountPrepaid.Name = "AmountPrepaid";
      this.AmountPrepaid.RightToLeft = RightToLeft.No;
      this.AmountPrepaid.Size = new Size(81, 20);
      this.AmountPrepaid.TabIndex = 6;
      this.AmountPrepaid.Text = "0,00";
      this.Label46.BackColor = SystemColors.Info;
      this.Label46.Cursor = Cursors.Default;
      this.Label46.ForeColor = SystemColors.ControlText;
      this.Label46.Location = new Point(48, 219);
      this.Label46.Name = "Label46";
      this.Label46.RightToLeft = RightToLeft.No;
      this.Label46.Size = new Size(129, 17);
      this.Label46.TabIndex = 221;
      this.Label46.Text = "Идентификатор клиента:";
      this.CheckAutoIncECROpId.AutoSize = true;
      this.CheckAutoIncECROpId.Checked = true;
      this.CheckAutoIncECROpId.CheckState = CheckState.Checked;
      this.CheckAutoIncECROpId.Location = new Point(185, 28);
      this.CheckAutoIncECROpId.Name = "CheckAutoIncECROpId";
      this.CheckAutoIncECROpId.Size = new Size(105, 17);
      this.CheckAutoIncECROpId.TabIndex = 171;
      this.CheckAutoIncECROpId.Text = "Автоинкремент";
      this.CheckAutoIncECROpId.UseVisualStyleBackColor = true;
      this.Label33.BackColor = SystemColors.Info;
      this.Label33.Cursor = Cursors.Default;
      this.Label33.ForeColor = SystemColors.ControlText;
      this.Label33.Location = new Point(184, 219);
      this.Label33.Name = "Label33";
      this.Label33.RightToLeft = RightToLeft.No;
      this.Label33.Size = new Size(177, 17);
      this.Label33.TabIndex = 220;
      this.FrameOpId.Controls.Add((Control) this.CheckAutoIncECROpId);
      this.FrameOpId.Controls.Add((Control) this.CheckAutoGenECROpId);
      this.FrameOpId.Controls.Add((Control) this.CheckAutoGenBpSId);
      this.FrameOpId.Controls.Add((Control) this.ECROpId);
      this.FrameOpId.Controls.Add((Control) this.BpSId);
      this.FrameOpId.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.FrameOpId.ForeColor = SystemColors.ControlText;
      this.FrameOpId.Location = new Point(784, 135);
      this.FrameOpId.Name = "FrameOpId";
      this.FrameOpId.RightToLeft = RightToLeft.No;
      this.FrameOpId.Size = new Size(345, 78);
      this.FrameOpId.TabIndex = 226;
      this.FrameOpId.TabStop = false;
      this.FrameOpId.Text = "Идентификация операции";
      this.CheckAutoGenBpSId.Cursor = Cursors.Default;
      this.CheckAutoGenBpSId.ForeColor = SystemColors.ControlText;
      this.CheckAutoGenBpSId.Location = new Point(19, 19);
      this.CheckAutoGenBpSId.Name = "CheckAutoGenBpSId";
      this.CheckAutoGenBpSId.RightToLeft = RightToLeft.No;
      this.CheckAutoGenBpSId.Size = new Size(129, 18);
      this.CheckAutoGenBpSId.TabIndex = 169;
      this.CheckAutoGenBpSId.Text = "Автогенерировать BpSId";
      this.CheckAutoGenBpSId.CheckedChanged += new EventHandler(this.CheckAutoGenBpSId_CheckedChanged);
      this.ECROpId.AcceptsReturn = true;
      this.ECROpId.BackColor = SystemColors.Window;
      this.ECROpId.Cursor = Cursors.IBeam;
      this.ECROpId.ForeColor = SystemColors.WindowText;
      this.ECROpId.Location = new Point(184, 46);
      this.ECROpId.MaxLength = 0;
      this.ECROpId.Name = "ECROpId";
      this.ECROpId.RightToLeft = RightToLeft.No;
      this.ECROpId.Size = new Size(73, 20);
      this.ECROpId.TabIndex = 168;
      this.ECROpId.Text = "Text2";
      this.BpSId.AcceptsReturn = true;
      this.BpSId.BackColor = SystemColors.Window;
      this.BpSId.Cursor = Cursors.IBeam;
      this.BpSId.ForeColor = SystemColors.WindowText;
      this.BpSId.Location = new Point(19, 46);
      this.BpSId.MaxLength = 0;
      this.BpSId.Name = "BpSId";
      this.BpSId.RightToLeft = RightToLeft.No;
      this.BpSId.Size = new Size(145, 20);
      this.BpSId.TabIndex = 167;
      this.BpSId.Text = "Text1";
      this.PromoBtn.Cursor = Cursors.Default;
      this.PromoBtn.Enabled = false;
      this.PromoBtn.ForeColor = SystemColors.ControlText;
      this.PromoBtn.Location = new Point(435, 234);
      this.PromoBtn.Name = "PromoBtn";
      this.PromoBtn.RightToLeft = RightToLeft.No;
      this.PromoBtn.Size = new Size(57, 50);
      this.PromoBtn.TabIndex = 227;
      this.PromoBtn.TabStop = false;
      this.PromoBtn.Text = "Промо";
      this.PromoBtn.Click += new EventHandler(this.PromoBtn_Click);
      this.SetDiscount.Cursor = Cursors.Default;
      this.SetDiscount.ForeColor = SystemColors.ControlText;
      this.SetDiscount.Location = new Point(184, 10);
      this.SetDiscount.Name = "SetDiscount";
      this.SetDiscount.RightToLeft = RightToLeft.No;
      this.SetDiscount.Size = new Size(41, 25);
      this.SetDiscount.TabIndex = 164;
      this.SetDiscount.Text = "Уст.";
      this.SetDiscount.Visible = false;
      this.SetDiscount.Click += new EventHandler(this.SetDiscount_Click);
      this.Label30.Cursor = Cursors.Default;
      this.Label30.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.Label30.ForeColor = SystemColors.ControlText;
      this.Label30.Location = new Point(524, 168);
      this.Label30.Name = "Label30";
      this.Label30.RightToLeft = RightToLeft.No;
      this.Label30.Size = new Size(241, 13);
      this.Label30.TabIndex = 215;
      this.Label30.Text = "ЭКРАН / ПРИНТЕР / ПОЛЬЗОВАТЕЛЬ";
      this.Label15.Cursor = Cursors.Default;
      this.Label15.ForeColor = SystemColors.ControlText;
      this.Label15.Location = new Point(8, 53);
      this.Label15.Name = "Label15";
      this.Label15.RightToLeft = RightToLeft.No;
      this.Label15.Size = new Size(41, 15);
      this.Label15.TabIndex = 212;
      this.Label15.Text = "Код";
      this.Label14.Cursor = Cursors.Default;
      this.Label14.ForeColor = SystemColors.ControlText;
      this.Label14.Location = new Point(788, 271);
      this.Label14.Name = "Label14";
      this.Label14.RightToLeft = RightToLeft.No;
      this.Label14.Size = new Size(134, 16);
      this.Label14.TabIndex = 208;
      this.Label14.Text = "Таймаут операции (сек) :";
      this.Label14.Visible = false;
      this.Label7.Cursor = Cursors.Default;
      this.Label7.ForeColor = SystemColors.ControlText;
      this.Label7.Location = new Point(367, 53);
      this.Label7.Name = "Label7";
      this.Label7.RightToLeft = RightToLeft.No;
      this.Label7.Size = new Size(62, 15);
      this.Label7.TabIndex = 202;
      this.Label7.Text = "Стоимость";
      this.Label6.Cursor = Cursors.Default;
      this.Label6.ForeColor = SystemColors.ControlText;
      this.Label6.Location = new Point(295, 53);
      this.Label6.Name = "Label6";
      this.Label6.RightToLeft = RightToLeft.No;
      this.Label6.Size = new Size(74, 15);
      this.Label6.TabIndex = 201;
      this.Label6.Text = "Цена за ед.";
      this.DiscountRequest.Cursor = Cursors.Default;
      this.DiscountRequest.ForeColor = SystemColors.ControlText;
      this.DiscountRequest.Location = new Point(239, 10);
      this.DiscountRequest.Name = "DiscountRequest";
      this.DiscountRequest.RightToLeft = RightToLeft.No;
      this.DiscountRequest.Size = new Size(105, 25);
      this.DiscountRequest.TabIndex = 165;
      this.DiscountRequest.Text = "Запрос скидки";
      this.DiscountRequest.Visible = false;
      this.DiscountRequest.Click += new EventHandler(this.DiscountRequest_Click);
      this.Label5.Cursor = Cursors.Default;
      this.Label5.ForeColor = SystemColors.ControlText;
      this.Label5.Location = new Point(240, 53);
      this.Label5.Name = "Label5";
      this.Label5.RightToLeft = RightToLeft.No;
      this.Label5.Size = new Size(41, 15);
      this.Label5.TabIndex = 200;
      this.Label5.Text = "Кол-во";
      this.Label4.Cursor = Cursors.Default;
      this.Label4.ForeColor = SystemColors.ControlText;
      this.Label4.Location = new Point(64, 53);
      this.Label4.Name = "Label4";
      this.Label4.RightToLeft = RightToLeft.No;
      this.Label4.Size = new Size(89, 15);
      this.Label4.TabIndex = 199;
      this.Label4.Text = "Наименование";
      this.Label3.Cursor = Cursors.Default;
      this.Label3.ForeColor = SystemColors.ControlText;
      this.Label3.Location = new Point(185, 19);
      this.Label3.Name = "Label3";
      this.Label3.RightToLeft = RightToLeft.No;
      this.Label3.Size = new Size(54, 17);
      this.Label3.TabIndex = 197;
      this.Label3.Text = "Кол-во:";
      this.Label2.Cursor = Cursors.Default;
      this.Label2.ForeColor = SystemColors.ControlText;
      this.Label2.Location = new Point(12, 16);
      this.Label2.Name = "Label2";
      this.Label2.RightToLeft = RightToLeft.No;
      this.Label2.Size = new Size(78, 20);
      this.Label2.TabIndex = 196;
      this.Label2.Text = "Код товара:";
      this.LabelStatus.Cursor = Cursors.Default;
      this.LabelStatus.Font = new Font("Microsoft Sans Serif", 12f);
      this.LabelStatus.ForeColor = Color.Blue;
      this.LabelStatus.Location = new Point(8, 292);
      this.LabelStatus.Name = "LabelStatus";
      this.LabelStatus.RightToLeft = RightToLeft.No;
      this.LabelStatus.Size = new Size(417, 25);
      this.LabelStatus.TabIndex = 190;
      this.LabelStatus.Text = "Режим готовности";
      this.CustomDesplayMessageText.Location = new Point(505, 291);
      this.CustomDesplayMessageText.Multiline = true;
      this.CustomDesplayMessageText.Name = "CustomDesplayMessageText";
      this.CustomDesplayMessageText.ScrollBars = ScrollBars.Vertical;
      this.CustomDesplayMessageText.Size = new Size(273, 52);
      this.CustomDesplayMessageText.TabIndex = 233;
      this.Frame3.Controls.Add((Control) this.CardSubstitution);
      this.Frame3.Controls.Add((Control) this.Label13);
      this.Frame3.Controls.Add((Control) this.Label12);
      this.Frame3.Controls.Add((Control) this.AmountSubstitutionCirculatings);
      this.Frame3.Controls.Add((Control) this.AmountSubstitutionBonuses);
      this.Frame3.ForeColor = SystemColors.ControlText;
      this.Frame3.Location = new Point(8, 576);
      this.Frame3.Name = "Frame3";
      this.Frame3.RightToLeft = RightToLeft.No;
      this.Frame3.Size = new Size(217, 74);
      this.Frame3.TabIndex = 206;
      this.Frame3.TabStop = false;
      this.Frame3.Text = "Замена карты";
      this.CardSubstitution.Cursor = Cursors.Default;
      this.CardSubstitution.ForeColor = SystemColors.ControlText;
      this.CardSubstitution.Location = new Point(31, 26);
      this.CardSubstitution.Name = "CardSubstitution";
      this.CardSubstitution.RightToLeft = RightToLeft.No;
      this.CardSubstitution.Size = new Size(153, 25);
      this.CardSubstitution.TabIndex = 11;
      this.CardSubstitution.Text = "Заменить карту";
      this.CardSubstitution.Click += new EventHandler(this.CardSubstitution_Click);
      this.Label13.Cursor = Cursors.Default;
      this.Label13.ForeColor = SystemColors.ControlText;
      this.Label13.Location = new Point(8, 48);
      this.Label13.Name = "Label13";
      this.Label13.RightToLeft = RightToLeft.No;
      this.Label13.Size = new Size(105, 25);
      this.Label13.TabIndex = 58;
      this.Label13.Text = "Обороты (руб.):";
      this.Label13.Visible = false;
      this.Label12.Cursor = Cursors.Default;
      this.Label12.ForeColor = SystemColors.ControlText;
      this.Label12.Location = new Point(8, 16);
      this.Label12.Name = "Label12";
      this.Label12.RightToLeft = RightToLeft.No;
      this.Label12.Size = new Size(97, 41);
      this.Label12.TabIndex = 57;
      this.Label12.Text = "Бонусы за замену:";
      this.Label12.Visible = false;
      this.AmountSubstitutionBonuses.AcceptsReturn = true;
      this.AmountSubstitutionBonuses.BackColor = SystemColors.Window;
      this.AmountSubstitutionBonuses.Cursor = Cursors.IBeam;
      this.AmountSubstitutionBonuses.ForeColor = SystemColors.WindowText;
      this.AmountSubstitutionBonuses.Location = new Point(119, 16);
      this.AmountSubstitutionBonuses.MaxLength = 0;
      this.AmountSubstitutionBonuses.Name = "AmountSubstitutionBonuses";
      this.AmountSubstitutionBonuses.RightToLeft = RightToLeft.No;
      this.AmountSubstitutionBonuses.Size = new Size(81, 20);
      this.AmountSubstitutionBonuses.TabIndex = 9;
      this.AmountSubstitutionBonuses.Text = "0,00";
      this.AmountSubstitutionBonuses.Visible = false;
      this.StornoCheck.Appearance = Appearance.Button;
      this.StornoCheck.BackColor = Color.Transparent;
      this.StornoCheck.Cursor = Cursors.Default;
      this.StornoCheck.Enabled = false;
      this.StornoCheck.ForeColor = SystemColors.ControlText;
      this.StornoCheck.Location = new Point(435, 179);
      this.StornoCheck.Name = "StornoCheck";
      this.StornoCheck.RightToLeft = RightToLeft.No;
      this.StornoCheck.Size = new Size(57, 50);
      this.StornoCheck.TabIndex = 183;
      this.StornoCheck.TabStop = false;
      this.StornoCheck.Text = "Сторно";
      this.StornoCheck.TextAlign = ContentAlignment.MiddleCenter;
      this.StornoCheck.UseVisualStyleBackColor = false;
      this.ScreenText.AcceptsReturn = true;
      this.ScreenText.BackColor = SystemColors.Window;
      this.ScreenText.Cursor = Cursors.IBeam;
      this.ScreenText.ForeColor = SystemColors.WindowText;
      this.ScreenText.Location = new Point(505, 183);
      this.ScreenText.MaxLength = 0;
      this.ScreenText.Multiline = true;
      this.ScreenText.Name = "ScreenText";
      this.ScreenText.RightToLeft = RightToLeft.No;
      this.ScreenText.ScrollBars = ScrollBars.Vertical;
      this.ScreenText.Size = new Size(273, 51);
      this.ScreenText.TabIndex = 192;
      this.PrepaidCharge.Cursor = Cursors.Default;
      this.PrepaidCharge.ForeColor = SystemColors.ControlText;
      this.PrepaidCharge.Location = new Point(239, 12);
      this.PrepaidCharge.Name = "PrepaidCharge";
      this.PrepaidCharge.RightToLeft = RightToLeft.No;
      this.PrepaidCharge.Size = new Size(105, 25);
      this.PrepaidCharge.TabIndex = 7;
      this.PrepaidCharge.Text = "Пополнить";
      this.PrepaidCharge.Click += new EventHandler(this.PrepaidCharge_Click);
      this.Frame1.Controls.Add((Control) this.Label11);
      this.Frame1.Controls.Add((Control) this.AmountPrepaid);
      this.Frame1.Controls.Add((Control) this.PrepaidCharge);
      this.Frame1.ForeColor = SystemColors.ControlText;
      this.Frame1.Location = new Point(8, 451);
      this.Frame1.Name = "Frame1";
      this.Frame1.RightToLeft = RightToLeft.No;
      this.Frame1.Size = new Size(361, 49);
      this.Frame1.TabIndex = 204;
      this.Frame1.TabStop = false;
      this.Frame1.Text = "Пополнение предоплаченного счета";
      this.Frame2.Controls.Add((Control) this.CardsLinking);
      this.Frame2.Controls.Add((Control) this.Label10);
      this.Frame2.Controls.Add((Control) this.AmountLinkingBonuses);
      this.Frame2.ForeColor = SystemColors.ControlText;
      this.Frame2.Location = new Point(8, 505);
      this.Frame2.Name = "Frame2";
      this.Frame2.RightToLeft = RightToLeft.No;
      this.Frame2.Size = new Size(217, 65);
      this.Frame2.TabIndex = 205;
      this.Frame2.TabStop = false;
      this.Frame2.Text = "Привязка карты";
      this.Label10.Cursor = Cursors.Default;
      this.Label10.ForeColor = SystemColors.ControlText;
      this.Label10.Location = new Point(8, 16);
      this.Label10.Name = "Label10";
      this.Label10.RightToLeft = RightToLeft.No;
      this.Label10.Size = new Size(105, 33);
      this.Label10.TabIndex = 55;
      this.Label10.Text = "Сумма (бонусы) на осн. карте:";
      this.Label10.Visible = false;
      this.ApplyBtn.BackColor = Color.Transparent;
      this.ApplyBtn.Cursor = Cursors.Default;
      this.ApplyBtn.Enabled = false;
      this.ApplyBtn.ForeColor = SystemColors.ControlText;
      this.ApplyBtn.Location = new Point(435, 123);
      this.ApplyBtn.Name = "ApplyBtn";
      this.ApplyBtn.RightToLeft = RightToLeft.No;
      this.ApplyBtn.Size = new Size(57, 50);
      this.ApplyBtn.TabIndex = 189;
      this.ApplyBtn.TabStop = false;
      this.ApplyBtn.Text = "Подытог";
      this.ApplyBtn.UseVisualStyleBackColor = false;
      this.ApplyBtn.Click += new EventHandler(this.ApplyBtn_Click);
      this.Label22.Cursor = Cursors.Default;
      this.Label22.ForeColor = SystemColors.ControlText;
      this.Label22.Location = new Point(36, 81);
      this.Label22.Name = "Label22";
      this.Label22.RightToLeft = RightToLeft.No;
      this.Label22.Size = new Size(231, 13);
      this.Label22.TabIndex = 163;
      this.Label22.Text = "Пауза между повторами соединения, сек.";
      this.Frame11.Controls.Add((Control) this.ConsultantField);
      this.Frame11.ForeColor = SystemColors.ControlText;
      this.Frame11.Location = new Point(784, 215);
      this.Frame11.Name = "Frame11";
      this.Frame11.RightToLeft = RightToLeft.No;
      this.Frame11.Size = new Size(121, 45);
      this.Frame11.TabIndex = 214;
      this.Frame11.TabStop = false;
      this.Frame11.Text = "ID Консультанта";
      this.ConsultantField.AcceptsReturn = true;
      this.ConsultantField.BackColor = SystemColors.Window;
      this.ConsultantField.Cursor = Cursors.IBeam;
      this.ConsultantField.ForeColor = SystemColors.WindowText;
      this.ConsultantField.Location = new Point(8, 17);
      this.ConsultantField.MaxLength = 0;
      this.ConsultantField.Name = "ConsultantField";
      this.ConsultantField.RightToLeft = RightToLeft.No;
      this.ConsultantField.Size = new Size(103, 20);
      this.ConsultantField.TabIndex = 82;
      this.ConsultantField.Text = "0";
      this._Label1_0.Cursor = Cursors.Default;
      this._Label1_0.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this._Label1_0.ForeColor = SystemColors.ControlText;
      this._Label1_0.Location = new Point(6, 14);
      this._Label1_0.Name = "_Label1_0";
      this._Label1_0.RightToLeft = RightToLeft.No;
      this._Label1_0.Size = new Size(81, 17);
      this._Label1_0.TabIndex = 162;
      this._Label1_0.Text = "Скидка (%):";
      this.DiscountField.AcceptsReturn = true;
      this.DiscountField.BackColor = SystemColors.Window;
      this.DiscountField.Cursor = Cursors.IBeam;
      this.DiscountField.ForeColor = SystemColors.WindowText;
      this.DiscountField.Location = new Point(104, 11);
      this.DiscountField.MaxLength = 0;
      this.DiscountField.Name = "DiscountField";
      this.DiscountField.RightToLeft = RightToLeft.No;
      this.DiscountField.Size = new Size(57, 20);
      this.DiscountField.TabIndex = 163;
      this.DiscountField.Text = "0";
      this.Label19.Cursor = Cursors.Default;
      this.Label19.ForeColor = SystemColors.ControlText;
      this.Label19.Location = new Point(36, 59);
      this.Label19.Name = "Label19";
      this.Label19.RightToLeft = RightToLeft.No;
      this.Label19.Size = new Size(231, 17);
      this.Label19.TabIndex = 162;
      this.Label19.Text = "Количество попыток соединения с LSP.";
      this.LabelDiscountRubl.BackColor = SystemColors.Info;
      this.LabelDiscountRubl.Cursor = Cursors.Default;
      this.LabelDiscountRubl.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.LabelDiscountRubl.ForeColor = SystemColors.ControlText;
      this.LabelDiscountRubl.Location = new Point(6, 14);
      this.LabelDiscountRubl.Name = "LabelDiscountRubl";
      this.LabelDiscountRubl.RightToLeft = RightToLeft.No;
      this.LabelDiscountRubl.Size = new Size(81, 17);
      this.LabelDiscountRubl.TabIndex = 161;
      this.LabelDiscountRubl.Text = "Скидка (руб.)";
      this.InterConnectTimeoutVal.AcceptsReturn = true;
      this.InterConnectTimeoutVal.BackColor = SystemColors.Window;
      this.InterConnectTimeoutVal.Cursor = Cursors.IBeam;
      this.InterConnectTimeoutVal.ForeColor = SystemColors.WindowText;
      this.InterConnectTimeoutVal.Location = new Point(6, 77);
      this.InterConnectTimeoutVal.MaxLength = 0;
      this.InterConnectTimeoutVal.Name = "InterConnectTimeoutVal";
      this.InterConnectTimeoutVal.RightToLeft = RightToLeft.No;
      this.InterConnectTimeoutVal.Size = new Size(26, 20);
      this.InterConnectTimeoutVal.TabIndex = 161;
      this.InterConnectTimeoutVal.Text = "1";
      this.Frame24.Controls.Add((Control) this.Label22);
      this.Frame24.Controls.Add((Control) this.Label19);
      this.Frame24.Controls.Add((Control) this.InterConnectTimeoutVal);
      this.Frame24.Controls.Add((Control) this.RetryQuantityVal);
      this.Frame24.Controls.Add((Control) this.Check_IntegralProt);
      this.Frame24.Controls.Add((Control) this.Check_UseKLProtocol);
      this.Frame24.Controls.Add((Control) this.ButtonDisconnect);
      this.Frame24.ForeColor = SystemColors.Info;
      this.Frame24.Location = new Point(505, 67);
      this.Frame24.Name = "Frame24";
      this.Frame24.RightToLeft = RightToLeft.No;
      this.Frame24.Size = new Size(273, 100);
      this.Frame24.TabIndex = 224;
      this.Frame24.TabStop = false;
      this.RetryQuantityVal.AcceptsReturn = true;
      this.RetryQuantityVal.BackColor = SystemColors.Window;
      this.RetryQuantityVal.Cursor = Cursors.IBeam;
      this.RetryQuantityVal.ForeColor = SystemColors.WindowText;
      this.RetryQuantityVal.Location = new Point(6, 56);
      this.RetryQuantityVal.MaxLength = 0;
      this.RetryQuantityVal.Name = "RetryQuantityVal";
      this.RetryQuantityVal.RightToLeft = RightToLeft.No;
      this.RetryQuantityVal.Size = new Size(26, 20);
      this.RetryQuantityVal.TabIndex = 160;
      this.RetryQuantityVal.Text = "1";
      this.Check_IntegralProt.Checked = true;
      this.Check_IntegralProt.CheckState = CheckState.Checked;
      this.Check_IntegralProt.Cursor = Cursors.Default;
      this.Check_IntegralProt.ForeColor = SystemColors.ControlText;
      this.Check_IntegralProt.Location = new Point(7, 37);
      this.Check_IntegralProt.Name = "Check_IntegralProt";
      this.Check_IntegralProt.RightToLeft = RightToLeft.No;
      this.Check_IntegralProt.Size = new Size(154, 18);
      this.Check_IntegralProt.TabIndex = 159;
      this.Check_IntegralProt.Text = "Объединенный протокол";
      this.ButtonDisconnect.Cursor = Cursors.Default;
      this.ButtonDisconnect.ForeColor = SystemColors.ControlText;
      this.ButtonDisconnect.Location = new Point(182, 14);
      this.ButtonDisconnect.Name = "ButtonDisconnect";
      this.ButtonDisconnect.RightToLeft = RightToLeft.No;
      this.ButtonDisconnect.Size = new Size(85, 42);
      this.ButtonDisconnect.TabIndex = 158;
      this.ButtonDisconnect.Text = "Disconnect";
      this.ButtonDisconnect.Click += new EventHandler(this.ButtonDisconnectClick);
      this.Frame25.Controls.Add((Control) this.DiscountRequest);
      this.Frame25.Controls.Add((Control) this.SetDiscount);
      this.Frame25.Controls.Add((Control) this.DiscountField);
      this.Frame25.Controls.Add((Control) this._Label1_0);
      this.Frame25.Controls.Add((Control) this.LabelDiscountRubl);
      this.Frame25.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.Frame25.ForeColor = SystemColors.ControlText;
      this.Frame25.Location = new Point(8, 327);
      this.Frame25.Name = "Frame25";
      this.Frame25.RightToLeft = RightToLeft.No;
      this.Frame25.Size = new Size(362, 41);
      this.Frame25.TabIndex = 225;
      this.Frame25.TabStop = false;
      this.Frame25.Text = "Скидка";
      this.ButtonStartServer.Cursor = Cursors.Default;
      this.ButtonStartServer.ForeColor = SystemColors.ControlText;
      this.ButtonStartServer.Location = new Point(3, 33);
      this.ButtonStartServer.Name = "ButtonStartServer";
      this.ButtonStartServer.RightToLeft = RightToLeft.No;
      this.ButtonStartServer.Size = new Size(84, 25);
      this.ButtonStartServer.TabIndex = 177;
      this.ButtonStartServer.Text = "Старт Сервер";
      this.ButtonStopServer.Cursor = Cursors.Default;
      this.ButtonStopServer.ForeColor = SystemColors.ControlText;
      this.ButtonStopServer.Location = new Point(88, 33);
      this.ButtonStopServer.Name = "ButtonStopServer";
      this.ButtonStopServer.RightToLeft = RightToLeft.No;
      this.ButtonStopServer.Size = new Size(87, 25);
      this.ButtonStopServer.TabIndex = 178;
      this.ButtonStopServer.Text = "Стоп Сервер";
      this.LabelStateConnect.AutoSize = true;
      this.LabelStateConnect.BackColor = SystemColors.GradientActiveCaption;
      this.LabelStateConnect.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.LabelStateConnect.ForeColor = Color.DarkRed;
      this.LabelStateConnect.Location = new Point(177, 38);
      this.LabelStateConnect.Name = "LabelStateConnect";
      this.LabelStateConnect.Size = new Size(85, 16);
      this.LabelStateConnect.TabIndex = 179;
      this.LabelStateConnect.Text = "Disconnect";
      this.CheckBox_RestartServer.AutoSize = true;
      this.CheckBox_RestartServer.Checked = true;
      this.CheckBox_RestartServer.CheckState = CheckState.Checked;
      this.CheckBox_RestartServer.Location = new Point(5, 15);
      this.CheckBox_RestartServer.Name = "CheckBox_RestartServer";
      this.CheckBox_RestartServer.Size = new Size(154, 17);
      this.CheckBox_RestartServer.TabIndex = 180;
      this.CheckBox_RestartServer.Text = "Автоперезапуск сервера";
      this.Label18.AutoSize = true;
      this.Label18.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.Label18.Location = new Point(181, 16);
      this.Label18.Name = "Label18";
      this.Label18.Size = new Size(73, 16);
      this.Label18.TabIndex = 181;
      this.Label18.Text = "STATUS:";
      this.GroupBox3.BackColor = Color.Transparent;
      this.GroupBox3.Controls.Add((Control) this.Label18);
      this.GroupBox3.Controls.Add((Control) this.CheckBox_RestartServer);
      this.GroupBox3.Controls.Add((Control) this.LabelStateConnect);
      this.GroupBox3.Controls.Add((Control) this.ButtonStopServer);
      this.GroupBox3.Controls.Add((Control) this.ButtonStartServer);
      this.GroupBox3.Enabled = false;
      this.GroupBox3.ForeColor = SystemColors.ControlText;
      this.GroupBox3.Location = new Point(505, 5);
      this.GroupBox3.Name = "GroupBox3";
      this.GroupBox3.RightToLeft = RightToLeft.No;
      this.GroupBox3.Size = new Size(178, 62);
      this.GroupBox3.TabIndex = 230;
      this.GroupBox3.TabStop = false;
      this.GroupBox3.Text = "Серверный режим";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1136, 739);
      this.Controls.Add((Control) this.GroupBox2);
      this.Controls.Add((Control) this.GoodsNames);
      this.Controls.Add((Control) this.GroupBox3);
      this.Controls.Add((Control) this.Frame26);
      this.Controls.Add((Control) this.InfoBtn);
      this.Controls.Add((Control) this.Frame5);
      this.Controls.Add((Control) this.GroupBox1);
      this.Controls.Add((Control) this.GetTimeout);
      this.Controls.Add((Control) this.Frame4);
      this.Controls.Add((Control) this.Check_Deposit);
      this.Controls.Add((Control) this.GoodsAmount);
      this.Controls.Add((Control) this.GoodsQuantity);
      this.Controls.Add((Control) this.NewOrder);
      this.Controls.Add((Control) this.PrnText);
      this.Controls.Add((Control) this.GoodsPrices);
      this.Controls.Add((Control) this.ResetBtn);
      this.Controls.Add((Control) this.OK);
      this.Controls.Add((Control) this.BarCoder);
      this.Controls.Add((Control) this.CoodCount);
      this.Controls.Add((Control) this.FrameBprrnCancel);
      this.Controls.Add((Control) this.GoodsCode);
      this.Controls.Add((Control) this.Frame23);
      this.Controls.Add((Control) this.Frame22);
      this.Controls.Add((Control) this.Frame20);
      this.Controls.Add((Control) this.Frame14);
      this.Controls.Add((Control) this.Check_ModificationBeforePayments);
      this.Controls.Add((Control) this.Check_DiscountByGoods);
      this.Controls.Add((Control) this.Label46);
      this.Controls.Add((Control) this.Label33);
      this.Controls.Add((Control) this.FrameOpId);
      this.Controls.Add((Control) this.PromoBtn);
      this.Controls.Add((Control) this.Label30);
      this.Controls.Add((Control) this.Label15);
      this.Controls.Add((Control) this.Label14);
      this.Controls.Add((Control) this.Label7);
      this.Controls.Add((Control) this.Label6);
      this.Controls.Add((Control) this.Label5);
      this.Controls.Add((Control) this.Label4);
      this.Controls.Add((Control) this.Label3);
      this.Controls.Add((Control) this.Label2);
      this.Controls.Add((Control) this.LabelStatus);
      this.Controls.Add((Control) this.CustomDesplayMessageText);
      this.Controls.Add((Control) this.Frame3);
      this.Controls.Add((Control) this.StornoCheck);
      this.Controls.Add((Control) this.ScreenText);
      this.Controls.Add((Control) this.Frame1);
      this.Controls.Add((Control) this.Frame2);
      this.Controls.Add((Control) this.ApplyBtn);
      this.Controls.Add((Control) this.Frame11);
      this.Controls.Add((Control) this.Frame24);
      this.Controls.Add((Control) this.Frame25);
      this.MaximumSize = new Size(1152, 777);
      this.MinimumSize = new Size(1152, 777);
      this.Name = nameof (Bel);
      this.Text = "Form1";
      this.Frame19.ResumeLayout(false);
      this.Frame19.PerformLayout();
      this.Frame16.ResumeLayout(false);
      this.Frame16.PerformLayout();
      this.Frame17.ResumeLayout(false);
      this.Frame17.PerformLayout();
      this.Frame6.ResumeLayout(false);
      this.Frame6.PerformLayout();
      this.GroupBox2.ResumeLayout(false);
      this.GroupBox2.PerformLayout();
      this.Frame15.ResumeLayout(false);
      this.Frame18.ResumeLayout(false);
      this.Frame18.PerformLayout();
      this.Frame13.ResumeLayout(false);
      this.Frame26.ResumeLayout(false);
      this.Frame5.ResumeLayout(false);
      this.Frame12.ResumeLayout(false);
      this.Frame12.PerformLayout();
      this.Frame10.ResumeLayout(false);
      this.Frame10.PerformLayout();
      this.Frame9.ResumeLayout(false);
      this.Frame9.PerformLayout();
      this.Frame8.ResumeLayout(false);
      this.Frame8.PerformLayout();
      this.Frame7.ResumeLayout(false);
      this.Frame7.PerformLayout();
      this.GroupBox1.ResumeLayout(false);
      this.GroupBox1.PerformLayout();
      this.Frame4.ResumeLayout(false);
      this.FrameBprrnCancel.ResumeLayout(false);
      this.FrameBprrnCancel.PerformLayout();
      this.Frame23.ResumeLayout(false);
      this.Frame23.PerformLayout();
      this.Frame22.ResumeLayout(false);
      this.Frame20.ResumeLayout(false);
      this.Frame20.PerformLayout();
      this.Frame14.ResumeLayout(false);
      this.FrameOpId.ResumeLayout(false);
      this.FrameOpId.PerformLayout();
      this.Frame3.ResumeLayout(false);
      this.Frame3.PerformLayout();
      this.Frame1.ResumeLayout(false);
      this.Frame1.PerformLayout();
      this.Frame2.ResumeLayout(false);
      this.Frame2.PerformLayout();
      this.Frame11.ResumeLayout(false);
      this.Frame11.PerformLayout();
      this.Frame24.ResumeLayout(false);
      this.Frame24.PerformLayout();
      this.Frame25.ResumeLayout(false);
      this.Frame25.PerformLayout();
      this.GroupBox3.ResumeLayout(false);
      this.GroupBox3.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
