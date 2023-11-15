﻿// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.Forms.DialogCheque
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using BELLib;
using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint.Forms
{
  internal class DialogCheque : Form
  {
    private const short OPERATION_MODIFY_CHEQUE = 6;
    private const short PAY_TYPE_SEL_CARD_CASH = 1;
    private const short PAY_TYPE_SEL_CARD_SIMPLE = 2;
    private const short PAY_TYPE_SEL_CARD_ANOTHER = 3;
    private const short PAY_TYPE_SEL_BONUS_SIMPLE = 4;
    private const short PAY_TYPE_SEL_BONUS_ANOTHER = 5;
    private const short PAY_TYPE_SEL_CARD_CARD_ONLY = 6;
    private const short PAY_TYPE_CASH = 7;
    private const short PAY_TYPE_SEL_PREPAID_SIMPLE = 8;
    private const short PAY_TYPE_SEL_PREPAID_ANOTHER = 9;
    private const short ECR_FLAG_WAIT_ANSWER = 4;
    private const short ECR_FLAG_NO_USE_BS = 8;
    private const short ECR_FLAG_NEPOLN_DATA = 16;
    private const short ECR_FLAG_DISCOUNT_TYPE = 32;
    private const short CHEQUE_FLAG_WITH_DISCOUNT = 1;
    private const short CHEQUE_FLAG_DISCOUNT_IN_AMOUNT = 2;
    private const short CHEQUE_FLAG_NEED_RECALC = 4;
    private const short CHEQUE_FLAG_PERFORORMED_PAYMENTS_CHANGE = 8;
    private const short CHEQUE_FLAG_DISCOUNT_BY_GOODS = 16;
    private const short CHEQUE_GOOD_FLAG_WITH_DISCOUNT = 1;
    private const short CHEQUE_GOOD_FLAG_DISCOUNT_TYPE = 2;
    private const short CHEQUE_GOOD_FLAG_NEED_RECALC = 4;
    private const short CHEQUE_GOOD_FLAG_BONUS_DISCOUNT_TYPE = 16;
    private static DialogCheque m_vb6FormDefInstance;
    private int summAmount;
    private int PrevChequePerformedPaymentCash;
    private int PrevChequePerformedPaymentCard;
    private int PrevChequePerformedPaymentCredit;
    private int PrevChequePerformedPaymentBonuses;
    private int PrevChequePerformedPaymentPrepaid;
    private int PrevChequeAmount;
    public bool bChequeWasModified;
    public bool bFirstTime;
    public bool bPaymentTypeBeforeModification;
    private IContainer components;
    public TextBox TextModifChequePaymentBonuses;
    public ToolTip ToolTip1;
    public TextBox TextModifChequeDiscount1;
    public TextBox TextModifGoodCode;
    public TextBox TextModifGoodQuantity;
    public TextBox TextModifChequePaymentCard;
    public TextBox TextModifGoodPrice;
    public TextBox TextModifGoodBonusPrice;
    public TextBox TextModifChequePaymentCash;
    public TextBox TextModifGoodDiscount;
    public TextBox TextModifGoodBonusesDiscount;
    public TextBox TextModifGoodBonusesDiscountPrice;
    public TextBox TextModifChequePaymentCredit;
    public TextBox TextModifGoodAmount;
    public TextBox TextModifChequeBonuses;
    public Label Label21;
    public Button ButtonModification;
    public Label Label19;
    public Button ChequeTimeButton2;
    public TextBox TextModifChequePaymentPrepaid;
    public Label Label12;
    public Label Label13;
    public Label Label14;
    public TextBox TextModifGoodName;
    public Label Label17;
    public Label Label18;
    public GroupBox Frame1;
    public TextBox TextModifGoodBonusesAmount;
    public GroupBox Frame2;
    public Button ButtonPayModif;
    public TextBox TextModifChequeAmount;
    public Button Command1;
    public Label Label28;
    public Label LabelModifCheque;
    public Label Label26;
    public Label Label25;
    public Label Label24;
    public Label Label20;
    public Label Label3;
    public Label Label111;
    public Label Label9;
    public Label Label10;
    public Label Label8;
    public Label Label11;
    public Label Label16;
    public Label Label23;
    public TextBox TextGoodCode;
    public GroupBox FrameMain;
    public GroupBox FrameSourceCheque;
    public Button ButtomPaySource;
    public TextBox TextSourceChequeAmount;
    public TextBox TextGoodAmount;
    public TextBox TextGoodPrice;
    public TextBox TextGoodQuantity;
    public TextBox TextGoodName;
    public Label Label2;
    public Label Label1;
    public Label Label7;
    public Label Label6;
    public Label Label5;
    public Label Label4;
    public Label LabelSourceCheque;
    public CheckBox Check_DiscountByGoods;
    public CheckBox CheckAcceptCheque;
    public TextBox ScrMessage;
    public TextBox PrnMessage;
    public Label Label15;

    [DllImport("User32", EntryPoint = "OemToCharA", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int OemToChar(string lpszSrc, string lpszDst);

    public static DialogCheque DefInstance
    {
      get
      {
        if (DialogCheque.m_vb6FormDefInstance == null || DialogCheque.m_vb6FormDefInstance.IsDisposed)
          DialogCheque.m_vb6FormDefInstance = new DialogCheque();
        return DialogCheque.m_vb6FormDefInstance;
      }
      set => DialogCheque.m_vb6FormDefInstance = value;
    }

    private void CancelButton_Click() => this.Hide();

    private void OKButton_Click()
    {
      this.CheckAcceptCheque.CheckState = CheckState.Checked;
      this.Hide();
    }

    private string GetParamFromAddFile(ref int iParamNumber)
    {
      string paramFromAddFile = "";
      try
      {
        string str = Bel.Instance.Bpecr1.get_Parameter((ParamName) iParamNumber) as string;
      }
      catch (Exception ex)
      {
        return paramFromAddFile;
      }
      return (string) null;
    }

    private void OutputScrInfo() => this.ScrMessage.Text += (Bel.Instance.Bpecr1.ScreenMessage as string) + "\r\n";

    private void OutputPrnInfo() => this.PrnMessage.Text += (Bel.Instance.Bpecr1.PrinterMessage as string) + "\r\n";

    public void ClearModifCheque()
    {
      this.TextModifGoodCode.Text = "";
      this.TextModifGoodName.Text = "";
      this.TextModifGoodQuantity.Text = "";
      this.TextModifGoodPrice.Text = "";
      this.TextModifGoodBonusPrice.Text = "";
      this.TextModifGoodDiscount.Text = "";
      this.TextModifGoodBonusesDiscount.Text = "";
      this.TextModifGoodBonusesDiscountPrice.Text = "";
      this.TextModifGoodAmount.Text = "";
      this.TextModifGoodBonusesAmount.Text = "";
      this.TextModifChequePaymentCash.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.Amount / 100));
      this.TextModifChequePaymentCard.Text = "0.00";
      this.TextModifChequePaymentCredit.Text = "0.00";
      this.TextModifChequePaymentBonuses.Text = "0.00";
      this.TextModifChequePaymentPrepaid.Text = "0.00";
      this.TextModifChequeAmount.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.Amount / 100));
      this.TextModifChequeBonuses.Text = "0.00";
      this.TextModifChequeDiscount1.Text = "0.00";
    }

    public bool ModifyCheque()
    {
      bool flag1 = false;
      int num1 = 0;
      try
      {
        this.ButtonPayModif.Enabled = false;
        if (this.bFirstTime)
        {
          this.bFirstTime = false;
          Bel.Instance.TMPCheque = Bel.Instance.Bpecr1.Cheque;
          this.ClearModifCheque();
          num1 = 0;
          this.summAmount = 0;
          this.PrevChequePerformedPaymentCash = 0;
          this.PrevChequePerformedPaymentCard = 0;
          this.PrevChequePerformedPaymentCredit = 0;
          this.PrevChequePerformedPaymentBonuses = 0;
          this.PrevChequePerformedPaymentPrepaid = 0;
          this.PrevChequeAmount = 0;
          if (this.bPaymentTypeBeforeModification)
            return true;
        }
        else
        {
          this.RestoreSourceCheque();
          this.GetPerformedPayments();
        }
        if (this.Check_DiscountByGoods.CheckState != CheckState.Unchecked)
          Bel.Instance.Bpecr1.ChequeFlags |= (short) 16;
        else
          Bel.Instance.Bpecr1.ChequeFlags &= (short) -17;
        if (Bel.Instance.ConnectToPos() != ErrorInterpreter.ReturnCode.Ok)
        {
          this.RestoreSourceCheque();
          return false;
        }
        Bel.Instance.PreProcessOperationId();
        Bel.Instance.WrapPerformPosOperations((short) 6);
        Bel.Instance.PostProcessOperationId(false);
        if (Bel.Instance.Bpecr1.RetCode == 0)
        {
          this.OutputScrInfo();
          this.OutputPrnInfo();
          if (!Bel.Instance.IsChequePresent())
          {
            this.RestoreSourceCheque();
            this.ClearModifCheque();
            int num2 = (int) MessageBox.Show("Отсутсвует чек в ответе на запрос 'Модифицикации чека'.");
            flag1 = true;
            return true;
          }
          this.ButtonPayModif.Enabled = true;
          this.ClearModifCheque();
          int num3 = 0;
          int num4 = 0;
          this.summAmount = 0;
          int num5 = 0;
          bool flag2 = false;
          if (Bel.Instance.Bpecr1.goodCount > (short) 0)
          {
            Bel.Instance.Bpecr1.GetFirstGood();
            while (num5 < (int) Bel.Instance.Bpecr1.goodCount)
            {
              this.TextModifGoodCode.Text += (string) Bel.Instance.Bpecr1.goodCode;
              this.TextModifGoodCode.Text += "\r\n";
              this.TextModifGoodName.Text += (string) Bel.Instance.Bpecr1.goodName;
              this.TextModifGoodName.Text += "\r\n";
              this.TextModifGoodQuantity.Text = this.TextModifGoodQuantity.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodQuantity / 1000)) + "\r\n";
              this.TextModifGoodPrice.Text = this.TextModifGoodPrice.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodPrice / 100)) + "\r\n";
              this.TextModifGoodBonusPrice.Text = this.TextModifGoodBonusPrice.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodBonusPrice / 100)) + "\r\n";
              this.TextModifGoodDiscount.Text += string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodDiscount / 100));
              if (((int) Bel.Instance.Bpecr1.goodFlags & 2) == 1)
                this.TextModifGoodDiscount.Text += ".руб";
              else
                this.TextModifGoodDiscount.Text += "%";
              this.TextModifGoodDiscount.Text += "\r\n";
              this.TextModifGoodBonusesDiscount.Text += string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodBonusDiscount / 100));
              if (((int) Bel.Instance.Bpecr1.goodFlags & 16) == 1)
                this.TextModifGoodBonusesDiscount.Text += ".руб";
              else
                this.TextModifGoodBonusesDiscount.Text += "%";
              this.TextModifGoodBonusesDiscount.Text += "\r\n";
              this.TextModifGoodBonusesDiscountPrice.Text = this.TextModifGoodBonusesDiscountPrice.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodBonusDiscountPrice / 100)) + "\r\n";
              int num6;
              if (((int) Bel.Instance.Bpecr1.goodFlags & 4) == 1)
              {
                flag2 = true;
                Bel.Instance.Bpecr1.goodFlags &= (short) -5;
                Bel.Instance.Bpecr1.goodSummPrice = Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodPrice;
                num6 = Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodBonusPrice;
                if (Bel.Instance.Bpecr1.goodDiscount != 0)
                {
                  Bel.Instance.Bpecr1.goodFlags |= (short) 1;
                  num1 = ((int) Bel.Instance.Bpecr1.goodFlags & 2) != 1 ? Bel.Instance.Bpecr1.goodQuantity * (Bel.Instance.Bpecr1.goodPrice / 10000 * (10000 - Bel.Instance.Bpecr1.goodDiscount)) / 1000 : Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodPrice - Bel.Instance.Bpecr1.goodDiscount;
                  Bel.Instance.Bpecr1.goodSummPrice = num1;
                }
              }
              else
              {
                num1 = Bel.Instance.Bpecr1.goodSummPrice;
                num6 = 0;
              }
              this.TextModifGoodAmount.Text = this.TextModifGoodAmount.Text + string.Format("{0:#,#0.00}", (object) (num1 / 100)) + "\r\n";
              this.TextModifGoodBonusesAmount.Text = this.TextModifGoodBonusesAmount.Text + string.Format("{0:#,#0.00}", (object) (num6 / 100)) + "\r\n";
              num3 += num1;
              num4 += num6;
              ++num5;
              if (num5 < (int) Bel.Instance.Bpecr1.goodCount)
                Bel.Instance.Bpecr1.GetNextGood();
            }
          }
          this.TextModifChequeDiscount1.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.Discount / 100));
          if (((int) Bel.Instance.Bpecr1.ChequeFlags & 2) == 1)
            this.TextModifChequeDiscount1.Text += "руб.";
          else
            this.TextModifChequeDiscount1.Text += "%";
          this.summAmount = Bel.Instance.Bpecr1.ChequeSummPrice;
          if (this.summAmount == 0)
          {
            int num7 = (int) MessageBox.Show("Ошибка формата фискального чека: сумма чека равна 0.", "Модификация чека", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            this.summAmount = num3;
          }
          if (((int) Bel.Instance.Bpecr1.ChequeFlags & 4) == 1 || flag2)
          {
            Bel.Instance.Bpecr1.ChequeFlags &= (short) -5;
            this.summAmount = num3;
            if (Bel.Instance.Bpecr1.Discount != 0)
            {
              Bel.Instance.Bpecr1.ChequeFlags |= (short) 1;
              if (((int) Bel.Instance.Bpecr1.ChequeFlags & 2) == 1)
                this.summAmount -= Bel.Instance.Bpecr1.Discount;
              else
                this.summAmount = this.summAmount / 10000 * (10000 - Bel.Instance.Bpecr1.goodDiscount);
            }
          }
          Bel.Instance.Bpecr1.Amount = this.summAmount;
          Bel.Instance.Bpecr1.ChequeSummPrice = this.summAmount;
          this.TextModifChequeAmount.Text = string.Format("{0:#,#0.00}", (object) (this.summAmount / 100));
          this.TextModifChequeBonuses.Text = string.Format("{0:#,#0.00}", (object) (num4 / 100));
          this.TextModifChequePaymentCash.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentCash / 100));
          this.TextModifChequePaymentCard.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentCard / 100));
          this.TextModifChequePaymentCredit.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentCredit / 100));
          this.TextModifChequePaymentBonuses.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentBonuses / 100));
          this.TextModifChequePaymentPrepaid.Text = string.Format("{0:#,#0.00}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentPrepaid / 100));
          if (Bel.Instance.Bpecr1.ChequePerformedPaymentCash == 0 && Bel.Instance.Bpecr1.ChequePerformedPaymentCard == 0 && Bel.Instance.Bpecr1.ChequePerformedPaymentCredit == 0 && Bel.Instance.Bpecr1.ChequePerformedPaymentBonuses == 0 && Bel.Instance.Bpecr1.ChequePerformedPaymentPrepaid == 0)
            this.TextModifChequePaymentCash.Text = string.Format("{0:#,#0.00}", (object) ((Decimal) this.summAmount / 100M));
          if (this.PrevChequePerformedPaymentCash != Bel.Instance.Bpecr1.ChequePerformedPaymentCash || this.PrevChequePerformedPaymentCard != Bel.Instance.Bpecr1.ChequePerformedPaymentCard || this.PrevChequePerformedPaymentCredit != Bel.Instance.Bpecr1.ChequePerformedPaymentCredit || this.PrevChequePerformedPaymentPrepaid != Bel.Instance.Bpecr1.ChequePerformedPaymentPrepaid || this.PrevChequePerformedPaymentBonuses != Bel.Instance.Bpecr1.ChequePerformedPaymentBonuses)
          {
            int num8 = (int) MessageBox.Show("Внимание! При пересчете суммы оплат были изменены.");
          }
          else if (this.PrevChequeAmount != this.summAmount & this.PrevChequeAmount != 0)
          {
            int num9 = (int) MessageBox.Show("Внимание! Общая сумма чека изменилась.");
          }
          else
          {
            int num10 = (int) MessageBox.Show("При пересчете суммы оплат не изменились.");
          }
          this.CheckAcceptCheque.CheckState = CheckState.Unchecked;
          flag1 = true;
          return true;
        }
        int retCode = Bel.Instance.Bpecr1.RetCode;
        if (retCode == 2 & Bel.Instance.bFisicalConnectToPOS)
        {
          Bel.Instance.Bpecr1.DisconnectFromPOS();
          Bel.Instance.bFisicalConnectToPOS = false;
        }
        ErrorInterpreter.OutputErrorInfo((ErrorInterpreter.ReturnCode) retCode, "Ошибка выполнения операции 'Модификация чека'");
        this.RestoreSourceCheque();
        return false;
      }
      catch (Exception ex)
      {
        int num11 = (int) MessageBox.Show("Возникла ошибка обработки операции!", "Модификация чека", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return flag1;
      }
    }

    private void ButtomPaySource_Click(object eventSender, EventArgs eventArgs)
    {
      this.RestoreSourceCheque();
      this.Hide();
    }

    private void ButtonModification_Click(object eventSender, EventArgs eventArgs) => this.ModifyCheque();

    private void GetPerformedPayments()
    {
      Bel.Instance.Bpecr1.ChequePerformedPaymentCash = Convert.ToInt32(this.TextModifChequePaymentCash.Text.Replace(".", "").Replace(",", ""));
      this.PrevChequePerformedPaymentCash = Bel.Instance.Bpecr1.ChequePerformedPaymentCash;
      Bel.Instance.Bpecr1.ChequePerformedPaymentCard = Convert.ToInt32(this.TextModifChequePaymentCard.Text.Replace(".", "").Replace(",", ""));
      this.PrevChequePerformedPaymentCard = Bel.Instance.Bpecr1.ChequePerformedPaymentCard;
      Bel.Instance.Bpecr1.ChequePerformedPaymentCredit = Convert.ToInt32(this.TextModifChequePaymentCredit.Text.Replace(".", "").Replace(",", ""));
      this.PrevChequePerformedPaymentCredit = Bel.Instance.Bpecr1.ChequePerformedPaymentCredit;
      Bel.Instance.Bpecr1.ChequePerformedPaymentBonuses = Convert.ToInt32(this.TextModifChequePaymentBonuses.Text.Replace(".", "").Replace(",", ""));
      this.PrevChequePerformedPaymentBonuses = Bel.Instance.Bpecr1.ChequePerformedPaymentBonuses;
      Bel.Instance.Bpecr1.ChequePerformedPaymentPrepaid = Convert.ToInt32(this.TextModifChequePaymentPrepaid.Text.Replace(".", "").Replace(",", ""));
      this.PrevChequePerformedPaymentPrepaid = Bel.Instance.Bpecr1.ChequePerformedPaymentPrepaid;
      this.PrevChequeAmount = this.summAmount;
    }

    private void RestoreSourceCheque() => Bel.Instance.Bpecr1.Cheque = Bel.Instance.TMPCheque;

    private void ButtonPayModif_Click(object eventSender, EventArgs eventArgs)
    {
      this.bChequeWasModified = true;
      this.Hide();
    }

    public bool SetGoods()
    {
      int num1 = 0;
      num1 = 0;
      int num2 = 0;
      int num3 = 0;
      this.TextModifGoodCode.Text = "";
      this.TextModifGoodName.Text = "";
      this.TextModifGoodPrice.Text = "";
      this.TextModifGoodQuantity.Text = "";
      this.TextModifGoodAmount.Text = "";
      this.TextModifGoodDiscount.Text = "";
      this.TextModifGoodBonusesDiscount.Text = "";
      this.TextModifGoodAmount.Text = "";
      this.TextModifGoodBonusPrice.Text = "";
      this.TextModifGoodBonusesDiscount.Text = "";
      this.TextModifGoodBonusesAmount.Text = "";
      this.TextModifGoodBonusesDiscountPrice.Text = "";
      int goodCount = (int) Bel.Instance.Bpecr1.goodCount;
      while (goodCount != 0)
      {
        if (goodCount == (int) Bel.Instance.Bpecr1.goodCount)
          Bel.Instance.Bpecr1.GetFirstGood();
        this.TextModifGoodCode.Text += (string) Bel.Instance.Bpecr1.goodCode;
        this.TextModifGoodCode.Text += "\r\n";
        this.TextModifGoodName.Text += (string) Bel.Instance.Bpecr1.goodName;
        this.TextModifGoodName.Text += "\r\n";
        this.TextModifGoodQuantity.Text = this.TextModifGoodQuantity.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodQuantity / 1000)) + "\r\n";
        this.TextModifGoodPrice.Text = this.TextModifGoodPrice.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodPrice / 100)) + "\r\n";
        this.TextModifGoodBonusPrice.Text = this.TextModifGoodBonusPrice.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodBonusPrice / 100)) + "\r\n";
        this.TextModifGoodDiscount.Text += string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodDiscount / 100));
        if (((int) Bel.Instance.Bpecr1.goodFlags & 2) == 1)
          this.TextModifGoodDiscount.Text += ".руб";
        else
          this.TextModifGoodDiscount.Text += "%";
        this.TextModifGoodDiscount.Text += "\r\n";
        this.TextModifGoodBonusesDiscount.Text += string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodBonusDiscount / 100));
        if (((int) Bel.Instance.Bpecr1.goodFlags & 16) == 1)
          this.TextModifGoodBonusesDiscount.Text += ".руб";
        else
          this.TextModifGoodBonusesDiscount.Text += "%";
        this.TextModifGoodBonusesDiscount.Text += "\r\n";
        this.TextModifGoodBonusesDiscountPrice.Text = this.TextModifGoodBonusesDiscountPrice.Text + string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.goodBonusDiscountPrice / 100), (object) "#,#0.00") + "\r\n";
        int num4;
        int num5;
        if (((int) Bel.Instance.Bpecr1.goodFlags & 4) == 1)
        {
          if (Bel.Instance.Bpecr1.goodBonusPrice != 0)
          {
            num4 = Bel.Instance.Bpecr1.goodQuantity / 10;
            num5 = Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodBonusPrice;
          }
          else if (Bel.Instance.Bpecr1.goodDiscount != 0)
          {
            num5 = 0;
            num4 = ((int) Bel.Instance.Bpecr1.goodFlags & 2) != 1 ? Bel.Instance.Bpecr1.goodQuantity * (Bel.Instance.Bpecr1.goodPrice / 10000 * (10000 - Bel.Instance.Bpecr1.goodDiscount)) / 1000 : Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodPrice - Bel.Instance.Bpecr1.goodDiscount;
          }
          else if (Bel.Instance.Bpecr1.goodBonusDiscount != 0)
          {
            num5 = Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodBonusDiscountPrice;
            num4 = ((int) Bel.Instance.Bpecr1.goodFlags & 16) != 1 ? Bel.Instance.Bpecr1.goodQuantity * (Bel.Instance.Bpecr1.goodPrice / 10000 * (10000 - Bel.Instance.Bpecr1.goodBonusDiscount)) / 1000 : Bel.Instance.Bpecr1.goodQuantity / 1000 * (Bel.Instance.Bpecr1.goodPrice - Bel.Instance.Bpecr1.goodBonusDiscount);
          }
          else
          {
            num5 = 0;
            num4 = Bel.Instance.Bpecr1.goodQuantity / 1000 * Bel.Instance.Bpecr1.goodPrice;
          }
        }
        else
        {
          num4 = Bel.Instance.Bpecr1.goodSummPrice;
          num5 = 0;
        }
        this.TextModifGoodAmount.Text = this.TextModifGoodAmount.Text + string.Format("{0:#,#0.000}", (object) (num4 / 100)) + "\r\n";
        this.TextModifGoodBonusesAmount.Text = this.TextModifGoodBonusesAmount.Text + string.Format("{0:#,#0.000}", (object) (num5 / 100)) + "\r\n";
        num2 += num4;
        num3 += num5;
        --goodCount;
        if (goodCount > 0)
          Bel.Instance.Bpecr1.GetNextGood();
      }
      this.TextModifChequeDiscount1.Text = string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.Discount / 100));
      if (((int) Bel.Instance.Bpecr1.ChequeFlags & 2) == 1)
        this.TextModifChequeDiscount1.Text += "руб.";
      else
        this.TextModifChequeDiscount1.Text += "%";
      this.summAmount = Bel.Instance.Bpecr1.Amount;
      if (this.summAmount == 0)
        this.summAmount = num2;
      if (((int) Bel.Instance.Bpecr1.goodFlags & 4) == 1)
      {
        this.summAmount = num2;
        if (Bel.Instance.Bpecr1.Discount != 0)
        {
          if (((int) Bel.Instance.Bpecr1.ChequeFlags & 2) == 1)
            this.summAmount -= Bel.Instance.Bpecr1.Discount;
          else
            this.summAmount = this.summAmount / 10000 * (10000 - Bel.Instance.Bpecr1.goodDiscount);
        }
      }
      this.TextModifChequeAmount.Text = string.Format("{0:#,#0.000}", (object) (this.summAmount / 100));
      if (((int) Bel.Instance.Bpecr1.ChequeFlags & 8) == 1)
      {
        this.TextModifChequePaymentCash.Text = string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentCash / 100));
        this.TextModifChequePaymentCard.Text = string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentCard / 100));
        this.TextModifChequePaymentCredit.Text = string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentCredit / 100));
        this.TextModifChequePaymentBonuses.Text = string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentBonuses / 100));
        this.TextModifChequePaymentPrepaid.Text = string.Format("{0:#,#0.000}", (object) (Bel.Instance.Bpecr1.ChequePerformedPaymentPrepaid / 100));
      }
      return true;
    }

    private void ChequeTimeButton2_Click(object eventSender, EventArgs eventArgs)
    {
      DateTime dateTime = new DateTime();
      try
      {
        string str = Interaction.InputBox("Введите время в указанном формате", "Изменение времени", DateAndTime.Now.ToString());
        if (string.IsNullOrEmpty(str))
          return;
        Bel.Instance.Bpecr1.ChequeDate = Convert.ToDateTime(str);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show("Возникла ошибка преобразования даты/времени! Проверьте правильность ввода и соответствие формату.", "Изменение времени", MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    private void Command1_Click(object eventSender, EventArgs eventArgs) => this.Hide();

    private void DialogCheque_Load(object sender, EventArgs e) => DialogCheque.DefInstance = this;

    public DialogCheque()
    {
      this.InitializeComponent();
      this.Load += new EventHandler(this.DialogCheque_Load);
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
      this.TextModifChequePaymentBonuses = new TextBox();
      this.ToolTip1 = new ToolTip(this.components);
      this.TextModifChequeDiscount1 = new TextBox();
      this.TextModifGoodCode = new TextBox();
      this.TextModifGoodQuantity = new TextBox();
      this.TextModifChequePaymentCard = new TextBox();
      this.TextModifGoodPrice = new TextBox();
      this.TextModifGoodBonusPrice = new TextBox();
      this.TextModifChequePaymentCash = new TextBox();
      this.TextModifGoodDiscount = new TextBox();
      this.TextModifGoodBonusesDiscount = new TextBox();
      this.TextModifGoodBonusesDiscountPrice = new TextBox();
      this.TextModifChequePaymentCredit = new TextBox();
      this.TextModifGoodAmount = new TextBox();
      this.TextModifChequeBonuses = new TextBox();
      this.Label21 = new Label();
      this.ButtonModification = new Button();
      this.Label19 = new Label();
      this.ChequeTimeButton2 = new Button();
      this.TextModifChequePaymentPrepaid = new TextBox();
      this.Label12 = new Label();
      this.Label13 = new Label();
      this.Label14 = new Label();
      this.TextModifGoodName = new TextBox();
      this.Label17 = new Label();
      this.Label18 = new Label();
      this.Frame1 = new GroupBox();
      this.TextModifGoodBonusesAmount = new TextBox();
      this.Frame2 = new GroupBox();
      this.ButtonPayModif = new Button();
      this.TextModifChequeAmount = new TextBox();
      this.Command1 = new Button();
      this.Label28 = new Label();
      this.LabelModifCheque = new Label();
      this.Label26 = new Label();
      this.Label25 = new Label();
      this.Label24 = new Label();
      this.Label20 = new Label();
      this.Label3 = new Label();
      this.Label111 = new Label();
      this.Label9 = new Label();
      this.Label10 = new Label();
      this.Label8 = new Label();
      this.Label11 = new Label();
      this.Label16 = new Label();
      this.Label23 = new Label();
      this.TextGoodCode = new TextBox();
      this.FrameMain = new GroupBox();
      this.FrameSourceCheque = new GroupBox();
      this.ButtomPaySource = new Button();
      this.TextSourceChequeAmount = new TextBox();
      this.TextGoodAmount = new TextBox();
      this.TextGoodPrice = new TextBox();
      this.TextGoodQuantity = new TextBox();
      this.TextGoodName = new TextBox();
      this.Label2 = new Label();
      this.Label1 = new Label();
      this.Label7 = new Label();
      this.Label6 = new Label();
      this.Label5 = new Label();
      this.Label4 = new Label();
      this.LabelSourceCheque = new Label();
      this.Check_DiscountByGoods = new CheckBox();
      this.CheckAcceptCheque = new CheckBox();
      this.ScrMessage = new TextBox();
      this.PrnMessage = new TextBox();
      this.Label15 = new Label();
      this.Frame1.SuspendLayout();
      this.Frame2.SuspendLayout();
      this.FrameMain.SuspendLayout();
      this.FrameSourceCheque.SuspendLayout();
      this.SuspendLayout();
      this.TextModifChequePaymentBonuses.AcceptsReturn = true;
      this.TextModifChequePaymentBonuses.BackColor = SystemColors.Window;
      this.TextModifChequePaymentBonuses.Cursor = Cursors.IBeam;
      this.TextModifChequePaymentBonuses.ForeColor = SystemColors.WindowText;
      this.TextModifChequePaymentBonuses.Location = new Point(112, 48);
      this.TextModifChequePaymentBonuses.MaxLength = 0;
      this.TextModifChequePaymentBonuses.Name = "TextModifChequePaymentBonuses";
      this.TextModifChequePaymentBonuses.RightToLeft = RightToLeft.No;
      this.TextModifChequePaymentBonuses.Size = new Size(89, 20);
      this.TextModifChequePaymentBonuses.TabIndex = 3;
      this.TextModifChequePaymentBonuses.Text = "0.00";
      this.TextModifChequeDiscount1.AcceptsReturn = true;
      this.TextModifChequeDiscount1.BackColor = SystemColors.Window;
      this.TextModifChequeDiscount1.Cursor = Cursors.IBeam;
      this.TextModifChequeDiscount1.ForeColor = SystemColors.WindowText;
      this.TextModifChequeDiscount1.Location = new Point(136, 341);
      this.TextModifChequeDiscount1.MaxLength = 0;
      this.TextModifChequeDiscount1.Name = "TextModifChequeDiscount1";
      this.TextModifChequeDiscount1.RightToLeft = RightToLeft.No;
      this.TextModifChequeDiscount1.Size = new Size(97, 20);
      this.TextModifChequeDiscount1.TabIndex = 61;
      this.TextModifGoodCode.AcceptsReturn = true;
      this.TextModifGoodCode.BackColor = SystemColors.Window;
      this.TextModifGoodCode.Cursor = Cursors.IBeam;
      this.TextModifGoodCode.ForeColor = SystemColors.WindowText;
      this.TextModifGoodCode.Location = new Point(8, 80);
      this.TextModifGoodCode.MaxLength = 0;
      this.TextModifGoodCode.Multiline = true;
      this.TextModifGoodCode.Name = "TextModifGoodCode";
      this.TextModifGoodCode.RightToLeft = RightToLeft.No;
      this.TextModifGoodCode.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodCode.Size = new Size(49, 257);
      this.TextModifGoodCode.TabIndex = 58;
      this.TextModifGoodCode.WordWrap = false;
      this.TextModifGoodQuantity.AcceptsReturn = true;
      this.TextModifGoodQuantity.BackColor = SystemColors.Window;
      this.TextModifGoodQuantity.Cursor = Cursors.IBeam;
      this.TextModifGoodQuantity.ForeColor = SystemColors.WindowText;
      this.TextModifGoodQuantity.Location = new Point(184, 80);
      this.TextModifGoodQuantity.MaxLength = 0;
      this.TextModifGoodQuantity.Multiline = true;
      this.TextModifGoodQuantity.Name = "TextModifGoodQuantity";
      this.TextModifGoodQuantity.RightToLeft = RightToLeft.No;
      this.TextModifGoodQuantity.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodQuantity.Size = new Size(49, 257);
      this.TextModifGoodQuantity.TabIndex = 53;
      this.TextModifGoodQuantity.WordWrap = false;
      this.TextModifChequePaymentCard.AcceptsReturn = true;
      this.TextModifChequePaymentCard.BackColor = SystemColors.Window;
      this.TextModifChequePaymentCard.Cursor = Cursors.IBeam;
      this.TextModifChequePaymentCard.ForeColor = SystemColors.WindowText;
      this.TextModifChequePaymentCard.Location = new Point(112, 80);
      this.TextModifChequePaymentCard.MaxLength = 0;
      this.TextModifChequePaymentCard.Name = "TextModifChequePaymentCard";
      this.TextModifChequePaymentCard.RightToLeft = RightToLeft.No;
      this.TextModifChequePaymentCard.Size = new Size(89, 20);
      this.TextModifChequePaymentCard.TabIndex = 4;
      this.TextModifChequePaymentCard.Text = "0.00";
      this.TextModifGoodPrice.AcceptsReturn = true;
      this.TextModifGoodPrice.BackColor = SystemColors.Window;
      this.TextModifGoodPrice.Cursor = Cursors.IBeam;
      this.TextModifGoodPrice.ForeColor = SystemColors.WindowText;
      this.TextModifGoodPrice.Location = new Point(232, 80);
      this.TextModifGoodPrice.MaxLength = 0;
      this.TextModifGoodPrice.Multiline = true;
      this.TextModifGoodPrice.Name = "TextModifGoodPrice";
      this.TextModifGoodPrice.RightToLeft = RightToLeft.No;
      this.TextModifGoodPrice.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodPrice.Size = new Size(57, 257);
      this.TextModifGoodPrice.TabIndex = 52;
      this.TextModifGoodPrice.WordWrap = false;
      this.TextModifGoodBonusPrice.AcceptsReturn = true;
      this.TextModifGoodBonusPrice.BackColor = SystemColors.Window;
      this.TextModifGoodBonusPrice.Cursor = Cursors.IBeam;
      this.TextModifGoodBonusPrice.ForeColor = SystemColors.WindowText;
      this.TextModifGoodBonusPrice.Location = new Point(344, 80);
      this.TextModifGoodBonusPrice.MaxLength = 0;
      this.TextModifGoodBonusPrice.Multiline = true;
      this.TextModifGoodBonusPrice.Name = "TextModifGoodBonusPrice";
      this.TextModifGoodBonusPrice.RightToLeft = RightToLeft.No;
      this.TextModifGoodBonusPrice.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodBonusPrice.Size = new Size(57, 257);
      this.TextModifGoodBonusPrice.TabIndex = 43;
      this.TextModifGoodBonusPrice.WordWrap = false;
      this.TextModifChequePaymentCash.AcceptsReturn = true;
      this.TextModifChequePaymentCash.BackColor = SystemColors.Window;
      this.TextModifChequePaymentCash.Cursor = Cursors.IBeam;
      this.TextModifChequePaymentCash.ForeColor = SystemColors.WindowText;
      this.TextModifChequePaymentCash.Location = new Point(112, 16);
      this.TextModifChequePaymentCash.MaxLength = 0;
      this.TextModifChequePaymentCash.Name = "TextModifChequePaymentCash";
      this.TextModifChequePaymentCash.RightToLeft = RightToLeft.No;
      this.TextModifChequePaymentCash.Size = new Size(89, 20);
      this.TextModifChequePaymentCash.TabIndex = 2;
      this.TextModifChequePaymentCash.Text = "0.00";
      this.TextModifGoodDiscount.AcceptsReturn = true;
      this.TextModifGoodDiscount.BackColor = SystemColors.Window;
      this.TextModifGoodDiscount.Cursor = Cursors.IBeam;
      this.TextModifGoodDiscount.ForeColor = SystemColors.WindowText;
      this.TextModifGoodDiscount.Location = new Point(288, 80);
      this.TextModifGoodDiscount.MaxLength = 0;
      this.TextModifGoodDiscount.Multiline = true;
      this.TextModifGoodDiscount.Name = "TextModifGoodDiscount";
      this.TextModifGoodDiscount.RightToLeft = RightToLeft.No;
      this.TextModifGoodDiscount.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodDiscount.Size = new Size(57, 257);
      this.TextModifGoodDiscount.TabIndex = 42;
      this.TextModifGoodDiscount.WordWrap = false;
      this.TextModifGoodBonusesDiscount.AcceptsReturn = true;
      this.TextModifGoodBonusesDiscount.BackColor = SystemColors.Window;
      this.TextModifGoodBonusesDiscount.Cursor = Cursors.IBeam;
      this.TextModifGoodBonusesDiscount.ForeColor = SystemColors.WindowText;
      this.TextModifGoodBonusesDiscount.Location = new Point(400, 80);
      this.TextModifGoodBonusesDiscount.MaxLength = 0;
      this.TextModifGoodBonusesDiscount.Multiline = true;
      this.TextModifGoodBonusesDiscount.Name = "TextModifGoodBonusesDiscount";
      this.TextModifGoodBonusesDiscount.RightToLeft = RightToLeft.No;
      this.TextModifGoodBonusesDiscount.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodBonusesDiscount.Size = new Size(57, 257);
      this.TextModifGoodBonusesDiscount.TabIndex = 41;
      this.TextModifGoodBonusesDiscount.WordWrap = false;
      this.TextModifGoodBonusesDiscountPrice.AcceptsReturn = true;
      this.TextModifGoodBonusesDiscountPrice.BackColor = SystemColors.Window;
      this.TextModifGoodBonusesDiscountPrice.Cursor = Cursors.IBeam;
      this.TextModifGoodBonusesDiscountPrice.ForeColor = SystemColors.WindowText;
      this.TextModifGoodBonusesDiscountPrice.Location = new Point(456, 80);
      this.TextModifGoodBonusesDiscountPrice.MaxLength = 0;
      this.TextModifGoodBonusesDiscountPrice.Multiline = true;
      this.TextModifGoodBonusesDiscountPrice.Name = "TextModifGoodBonusesDiscountPrice";
      this.TextModifGoodBonusesDiscountPrice.RightToLeft = RightToLeft.No;
      this.TextModifGoodBonusesDiscountPrice.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodBonusesDiscountPrice.Size = new Size(57, 257);
      this.TextModifGoodBonusesDiscountPrice.TabIndex = 40;
      this.TextModifGoodBonusesDiscountPrice.WordWrap = false;
      this.TextModifChequePaymentCredit.AcceptsReturn = true;
      this.TextModifChequePaymentCredit.BackColor = SystemColors.Window;
      this.TextModifChequePaymentCredit.Cursor = Cursors.IBeam;
      this.TextModifChequePaymentCredit.ForeColor = SystemColors.WindowText;
      this.TextModifChequePaymentCredit.Location = new Point(112, 112);
      this.TextModifChequePaymentCredit.MaxLength = 0;
      this.TextModifChequePaymentCredit.Name = "TextModifChequePaymentCredit";
      this.TextModifChequePaymentCredit.RightToLeft = RightToLeft.No;
      this.TextModifChequePaymentCredit.Size = new Size(89, 20);
      this.TextModifChequePaymentCredit.TabIndex = 5;
      this.TextModifChequePaymentCredit.Text = "0.00";
      this.TextModifGoodAmount.AcceptsReturn = true;
      this.TextModifGoodAmount.BackColor = SystemColors.Window;
      this.TextModifGoodAmount.Cursor = Cursors.IBeam;
      this.TextModifGoodAmount.ForeColor = SystemColors.WindowText;
      this.TextModifGoodAmount.Location = new Point(512, 80);
      this.TextModifGoodAmount.MaxLength = 0;
      this.TextModifGoodAmount.Multiline = true;
      this.TextModifGoodAmount.Name = "TextModifGoodAmount";
      this.TextModifGoodAmount.RightToLeft = RightToLeft.No;
      this.TextModifGoodAmount.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodAmount.Size = new Size(89, 257);
      this.TextModifGoodAmount.TabIndex = 39;
      this.TextModifGoodAmount.WordWrap = false;
      this.TextModifChequeBonuses.AcceptsReturn = true;
      this.TextModifChequeBonuses.BackColor = SystemColors.Window;
      this.TextModifChequeBonuses.Cursor = Cursors.IBeam;
      this.TextModifChequeBonuses.ForeColor = SystemColors.ControlText;
      this.TextModifChequeBonuses.Location = new Point(577, 341);
      this.TextModifChequeBonuses.MaxLength = 0;
      this.TextModifChequeBonuses.Name = "TextModifChequeBonuses";
      this.TextModifChequeBonuses.RightToLeft = RightToLeft.No;
      this.TextModifChequeBonuses.Size = new Size(89, 20);
      this.TextModifChequeBonuses.TabIndex = 63;
      this.TextModifChequeBonuses.Text = "0.00";
      this.Label21.BackColor = SystemColors.Control;
      this.Label21.Cursor = Cursors.Default;
      this.Label21.ForeColor = SystemColors.ControlText;
      this.Label21.Location = new Point(745, 588);
      this.Label21.Name = "Label21";
      this.Label21.RightToLeft = RightToLeft.No;
      this.Label21.Size = new Size(125, 17);
      this.Label21.TabIndex = 73;
      this.Label21.Text = "Сообщение на ЭКРАН";
      this.ButtonModification.BackColor = SystemColors.Control;
      this.ButtonModification.Cursor = Cursors.Default;
      this.ButtonModification.ForeColor = SystemColors.ControlText;
      this.ButtonModification.Location = new Point(403, 488);
      this.ButtonModification.Name = "ButtonModification";
      this.ButtonModification.RightToLeft = RightToLeft.No;
      this.ButtonModification.Size = new Size(105, 25);
      this.ButtonModification.TabIndex = 1;
      this.ButtonModification.Text = "Пересчитать";
      this.ButtonModification.UseVisualStyleBackColor = false;
      this.ButtonModification.Click += new EventHandler(this.ButtonModification_Click);
      this.Label19.BackColor = SystemColors.Control;
      this.Label19.Cursor = Cursors.Default;
      this.Label19.ForeColor = SystemColors.ControlText;
      this.Label19.Location = new Point(193, 588);
      this.Label19.Name = "Label19";
      this.Label19.RightToLeft = RightToLeft.No;
      this.Label19.Size = new Size(140, 17);
      this.Label19.TabIndex = 72;
      this.Label19.Text = "Сообщение на ПРИНТЕР";
      this.ChequeTimeButton2.BackColor = SystemColors.Control;
      this.ChequeTimeButton2.Cursor = Cursors.Default;
      this.ChequeTimeButton2.ForeColor = SystemColors.ControlText;
      this.ChequeTimeButton2.Location = new Point(403, 520);
      this.ChequeTimeButton2.Name = "ChequeTimeButton2";
      this.ChequeTimeButton2.RightToLeft = RightToLeft.No;
      this.ChequeTimeButton2.Size = new Size(105, 25);
      this.ChequeTimeButton2.TabIndex = 67;
      this.ChequeTimeButton2.Text = "Время чека";
      this.ChequeTimeButton2.UseVisualStyleBackColor = false;
      this.ChequeTimeButton2.Click += new EventHandler(this.ChequeTimeButton2_Click);
      this.TextModifChequePaymentPrepaid.AcceptsReturn = true;
      this.TextModifChequePaymentPrepaid.BackColor = SystemColors.Window;
      this.TextModifChequePaymentPrepaid.Cursor = Cursors.IBeam;
      this.TextModifChequePaymentPrepaid.ForeColor = SystemColors.WindowText;
      this.TextModifChequePaymentPrepaid.Location = new Point(112, 144);
      this.TextModifChequePaymentPrepaid.MaxLength = 0;
      this.TextModifChequePaymentPrepaid.Name = "TextModifChequePaymentPrepaid";
      this.TextModifChequePaymentPrepaid.RightToLeft = RightToLeft.No;
      this.TextModifChequePaymentPrepaid.Size = new Size(89, 20);
      this.TextModifChequePaymentPrepaid.TabIndex = 6;
      this.TextModifChequePaymentPrepaid.Text = "0.00";
      this.Label12.BackColor = SystemColors.Control;
      this.Label12.Cursor = Cursors.Default;
      this.Label12.ForeColor = SystemColors.ControlText;
      this.Label12.Location = new Point(7, 22);
      this.Label12.Name = "Label12";
      this.Label12.RightToLeft = RightToLeft.No;
      this.Label12.Size = new Size(73, 25);
      this.Label12.TabIndex = 37;
      this.Label12.Text = "Наличными:";
      this.Label13.BackColor = SystemColors.Control;
      this.Label13.Cursor = Cursors.Default;
      this.Label13.ForeColor = SystemColors.ControlText;
      this.Label13.Location = new Point(7, 51);
      this.Label13.Name = "Label13";
      this.Label13.RightToLeft = RightToLeft.No;
      this.Label13.Size = new Size(66, 25);
      this.Label13.TabIndex = 36;
      this.Label13.Text = "Бонусами:";
      this.Label14.BackColor = SystemColors.Control;
      this.Label14.Cursor = Cursors.Default;
      this.Label14.ForeColor = SystemColors.ControlText;
      this.Label14.Location = new Point(7, 83);
      this.Label14.Name = "Label14";
      this.Label14.RightToLeft = RightToLeft.No;
      this.Label14.Size = new Size(81, 25);
      this.Label14.TabIndex = 35;
      this.Label14.Text = "Картой:";
      this.TextModifGoodName.AcceptsReturn = true;
      this.TextModifGoodName.BackColor = SystemColors.Window;
      this.TextModifGoodName.Cursor = Cursors.IBeam;
      this.TextModifGoodName.ForeColor = SystemColors.WindowText;
      this.TextModifGoodName.Location = new Point(56, 80);
      this.TextModifGoodName.MaxLength = 0;
      this.TextModifGoodName.Multiline = true;
      this.TextModifGoodName.Name = "TextModifGoodName";
      this.TextModifGoodName.RightToLeft = RightToLeft.No;
      this.TextModifGoodName.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodName.Size = new Size(129, 257);
      this.TextModifGoodName.TabIndex = 54;
      this.TextModifGoodName.WordWrap = false;
      this.Label17.BackColor = SystemColors.Control;
      this.Label17.Cursor = Cursors.Default;
      this.Label17.ForeColor = SystemColors.ControlText;
      this.Label17.Location = new Point(7, 118);
      this.Label17.Name = "Label17";
      this.Label17.RightToLeft = RightToLeft.No;
      this.Label17.Size = new Size(66, 25);
      this.Label17.TabIndex = 34;
      this.Label17.Text = "Кредит:";
      this.Label18.BackColor = SystemColors.Control;
      this.Label18.Cursor = Cursors.Default;
      this.Label18.ForeColor = SystemColors.ControlText;
      this.Label18.Location = new Point(8, 147);
      this.Label18.Name = "Label18";
      this.Label18.RightToLeft = RightToLeft.No;
      this.Label18.Size = new Size(80, 20);
      this.Label18.TabIndex = 33;
      this.Label18.Text = "Предоплата:";
      this.Frame1.BackColor = SystemColors.Control;
      this.Frame1.Controls.Add((Control) this.ChequeTimeButton2);
      this.Frame1.Controls.Add((Control) this.TextModifChequeDiscount1);
      this.Frame1.Controls.Add((Control) this.TextModifGoodCode);
      this.Frame1.Controls.Add((Control) this.TextModifChequeBonuses);
      this.Frame1.Controls.Add((Control) this.ButtonModification);
      this.Frame1.Controls.Add((Control) this.TextModifGoodName);
      this.Frame1.Controls.Add((Control) this.TextModifGoodQuantity);
      this.Frame1.Controls.Add((Control) this.TextModifGoodPrice);
      this.Frame1.Controls.Add((Control) this.TextModifGoodBonusPrice);
      this.Frame1.Controls.Add((Control) this.TextModifGoodDiscount);
      this.Frame1.Controls.Add((Control) this.TextModifGoodBonusesDiscount);
      this.Frame1.Controls.Add((Control) this.TextModifGoodBonusesDiscountPrice);
      this.Frame1.Controls.Add((Control) this.TextModifGoodAmount);
      this.Frame1.Controls.Add((Control) this.TextModifGoodBonusesAmount);
      this.Frame1.Controls.Add((Control) this.Frame2);
      this.Frame1.Controls.Add((Control) this.ButtonPayModif);
      this.Frame1.Controls.Add((Control) this.TextModifChequeAmount);
      this.Frame1.Controls.Add((Control) this.Command1);
      this.Frame1.Controls.Add((Control) this.Label28);
      this.Frame1.Controls.Add((Control) this.LabelModifCheque);
      this.Frame1.Controls.Add((Control) this.Label26);
      this.Frame1.Controls.Add((Control) this.Label25);
      this.Frame1.Controls.Add((Control) this.Label24);
      this.Frame1.Controls.Add((Control) this.Label20);
      this.Frame1.Controls.Add((Control) this.Label3);
      this.Frame1.Controls.Add((Control) this.Label111);
      this.Frame1.Controls.Add((Control) this.Label9);
      this.Frame1.Controls.Add((Control) this.Label10);
      this.Frame1.Controls.Add((Control) this.Label8);
      this.Frame1.Controls.Add((Control) this.Label11);
      this.Frame1.Controls.Add((Control) this.Label16);
      this.Frame1.Controls.Add((Control) this.Label23);
      this.Frame1.ForeColor = SystemColors.ControlText;
      this.Frame1.Location = new Point(376, 16);
      this.Frame1.Name = "Frame1";
      this.Frame1.RightToLeft = RightToLeft.No;
      this.Frame1.Size = new Size(697, 561);
      this.Frame1.TabIndex = 30;
      this.Frame1.TabStop = false;
      this.TextModifGoodBonusesAmount.AcceptsReturn = true;
      this.TextModifGoodBonusesAmount.BackColor = SystemColors.Window;
      this.TextModifGoodBonusesAmount.Cursor = Cursors.IBeam;
      this.TextModifGoodBonusesAmount.ForeColor = SystemColors.WindowText;
      this.TextModifGoodBonusesAmount.Location = new Point(600, 80);
      this.TextModifGoodBonusesAmount.MaxLength = 0;
      this.TextModifGoodBonusesAmount.Multiline = true;
      this.TextModifGoodBonusesAmount.Name = "TextModifGoodBonusesAmount";
      this.TextModifGoodBonusesAmount.RightToLeft = RightToLeft.No;
      this.TextModifGoodBonusesAmount.ScrollBars = ScrollBars.Horizontal;
      this.TextModifGoodBonusesAmount.Size = new Size(89, 257);
      this.TextModifGoodBonusesAmount.TabIndex = 38;
      this.TextModifGoodBonusesAmount.WordWrap = false;
      this.Frame2.BackColor = SystemColors.Control;
      this.Frame2.Controls.Add((Control) this.TextModifChequePaymentCash);
      this.Frame2.Controls.Add((Control) this.TextModifChequePaymentBonuses);
      this.Frame2.Controls.Add((Control) this.TextModifChequePaymentCard);
      this.Frame2.Controls.Add((Control) this.TextModifChequePaymentCredit);
      this.Frame2.Controls.Add((Control) this.TextModifChequePaymentPrepaid);
      this.Frame2.Controls.Add((Control) this.Label12);
      this.Frame2.Controls.Add((Control) this.Label13);
      this.Frame2.Controls.Add((Control) this.Label14);
      this.Frame2.Controls.Add((Control) this.Label17);
      this.Frame2.Controls.Add((Control) this.Label18);
      this.Frame2.ForeColor = SystemColors.ControlText;
      this.Frame2.Location = new Point(160, 376);
      this.Frame2.Name = "Frame2";
      this.Frame2.RightToLeft = RightToLeft.No;
      this.Frame2.Size = new Size(233, 177);
      this.Frame2.TabIndex = 32;
      this.Frame2.TabStop = false;
      this.Frame2.Text = "Оплаты";
      this.ButtonPayModif.BackColor = SystemColors.Control;
      this.ButtonPayModif.Cursor = Cursors.Default;
      this.ButtonPayModif.Enabled = false;
      this.ButtonPayModif.ForeColor = SystemColors.ControlText;
      this.ButtonPayModif.Location = new Point(403, 453);
      this.ButtonPayModif.Name = "ButtonPayModif";
      this.ButtonPayModif.RightToLeft = RightToLeft.No;
      this.ButtonPayModif.Size = new Size(105, 25);
      this.ButtonPayModif.TabIndex = 7;
      this.ButtonPayModif.Text = "Оплатить";
      this.ButtonPayModif.UseVisualStyleBackColor = false;
      this.ButtonPayModif.Click += new EventHandler(this.ButtonPayModif_Click);
      this.TextModifChequeAmount.AcceptsReturn = true;
      this.TextModifChequeAmount.BackColor = SystemColors.Window;
      this.TextModifChequeAmount.Cursor = Cursors.IBeam;
      this.TextModifChequeAmount.ForeColor = SystemColors.ControlText;
      this.TextModifChequeAmount.Location = new Point(368, 341);
      this.TextModifChequeAmount.MaxLength = 0;
      this.TextModifChequeAmount.Name = "TextModifChequeAmount";
      this.TextModifChequeAmount.RightToLeft = RightToLeft.No;
      this.TextModifChequeAmount.Size = new Size(89, 20);
      this.TextModifChequeAmount.TabIndex = 31;
      this.TextModifChequeAmount.Text = "0.00";
      this.Command1.BackColor = SystemColors.Control;
      this.Command1.Cursor = Cursors.Default;
      this.Command1.ForeColor = SystemColors.ControlText;
      this.Command1.Location = new Point(403, 392);
      this.Command1.Name = "Command1";
      this.Command1.RightToLeft = RightToLeft.No;
      this.Command1.Size = new Size(105, 42);
      this.Command1.TabIndex = 62;
      this.Command1.Text = "Продолжить";
      this.Command1.UseVisualStyleBackColor = false;
      this.Command1.Visible = false;
      this.Command1.Click += new EventHandler(this.Command1_Click);
      this.Label28.BackColor = SystemColors.Control;
      this.Label28.Cursor = Cursors.Default;
      this.Label28.ForeColor = SystemColors.ControlText;
      this.Label28.Location = new Point(488, 344);
      this.Label28.Name = "Label28";
      this.Label28.RightToLeft = RightToLeft.No;
      this.Label28.Size = new Size(89, 33);
      this.Label28.TabIndex = 65;
      this.Label28.Text = "Общая сумма бонусов:";
      this.LabelModifCheque.BackColor = SystemColors.Control;
      this.LabelModifCheque.Cursor = Cursors.Default;
      this.LabelModifCheque.ForeColor = SystemColors.ControlText;
      this.LabelModifCheque.Location = new Point(245, 8);
      this.LabelModifCheque.Name = "LabelModifCheque";
      this.LabelModifCheque.RightToLeft = RightToLeft.No;
      this.LabelModifCheque.Size = new Size(156, 25);
      this.LabelModifCheque.TabIndex = 60;
      this.LabelModifCheque.Text = "Модифицированный чек";
      this.LabelModifCheque.TextAlign = ContentAlignment.TopCenter;
      this.Label26.BackColor = SystemColors.Control;
      this.Label26.Cursor = Cursors.Default;
      this.Label26.ForeColor = SystemColors.ControlText;
      this.Label26.Location = new Point(8, 64);
      this.Label26.Name = "Label26";
      this.Label26.RightToLeft = RightToLeft.No;
      this.Label26.Size = new Size(49, 17);
      this.Label26.TabIndex = 59;
      this.Label26.Text = "Код";
      this.Label25.BackColor = SystemColors.Control;
      this.Label25.Cursor = Cursors.Default;
      this.Label25.ForeColor = SystemColors.ControlText;
      this.Label25.Location = new Point(64, 64);
      this.Label25.Name = "Label25";
      this.Label25.RightToLeft = RightToLeft.No;
      this.Label25.Size = new Size(89, 13);
      this.Label25.TabIndex = 57;
      this.Label25.Text = "Наименование";
      this.Label24.BackColor = SystemColors.Control;
      this.Label24.Cursor = Cursors.Default;
      this.Label24.ForeColor = SystemColors.ControlText;
      this.Label24.Location = new Point(189, 64);
      this.Label24.Name = "Label24";
      this.Label24.RightToLeft = RightToLeft.No;
      this.Label24.Size = new Size(41, 17);
      this.Label24.TabIndex = 56;
      this.Label24.Text = "Кол-во";
      this.Label20.BackColor = SystemColors.Control;
      this.Label20.Cursor = Cursors.Default;
      this.Label20.ForeColor = SystemColors.ControlText;
      this.Label20.Location = new Point(237, 50);
      this.Label20.Name = "Label20";
      this.Label20.RightToLeft = RightToLeft.No;
      this.Label20.Size = new Size(53, 27);
      this.Label20.TabIndex = 55;
      this.Label20.Text = "Цена за единицу";
      this.Label3.BackColor = SystemColors.Control;
      this.Label3.Cursor = Cursors.Default;
      this.Label3.ForeColor = SystemColors.ControlText;
      this.Label3.Location = new Point(346, 52);
      this.Label3.Name = "Label3";
      this.Label3.RightToLeft = RightToLeft.No;
      this.Label3.Size = new Size(49, 29);
      this.Label3.TabIndex = 51;
      this.Label3.Text = "Начисл. бонусы";
      this.Label111.BackColor = SystemColors.Control;
      this.Label111.Cursor = Cursors.Default;
      this.Label111.ForeColor = SystemColors.ControlText;
      this.Label111.Location = new Point(296, 64);
      this.Label111.Name = "Label111";
      this.Label111.RightToLeft = RightToLeft.No;
      this.Label111.Size = new Size(49, 17);
      this.Label111.TabIndex = 50;
      this.Label111.Text = "Скидка";
      this.Label9.BackColor = SystemColors.Control;
      this.Label9.Cursor = Cursors.Default;
      this.Label9.ForeColor = SystemColors.ControlText;
      this.Label9.Location = new Point(400, 40);
      this.Label9.Name = "Label9";
      this.Label9.RightToLeft = RightToLeft.No;
      this.Label9.Size = new Size(57, 41);
      this.Label9.TabIndex = 49;
      this.Label9.Text = "Скидка за бонусы";
      this.Label10.BackColor = SystemColors.Control;
      this.Label10.Cursor = Cursors.Default;
      this.Label10.ForeColor = SystemColors.ControlText;
      this.Label10.Location = new Point(456, 40);
      this.Label10.Name = "Label10";
      this.Label10.RightToLeft = RightToLeft.No;
      this.Label10.Size = new Size(65, 41);
      this.Label10.TabIndex = 48;
      this.Label10.Text = "Цена скидки за бонусы";
      this.Label8.BackColor = SystemColors.Control;
      this.Label8.Cursor = Cursors.Default;
      this.Label8.ForeColor = SystemColors.ControlText;
      this.Label8.Location = new Point(525, 64);
      this.Label8.Name = "Label8";
      this.Label8.RightToLeft = RightToLeft.No;
      this.Label8.Size = new Size(69, 13);
      this.Label8.TabIndex = 47;
      this.Label8.Text = "Стоимость";
      this.Label11.BackColor = SystemColors.Control;
      this.Label11.Cursor = Cursors.Default;
      this.Label11.ForeColor = SystemColors.ControlText;
      this.Label11.Location = new Point(609, 50);
      this.Label11.Name = "Label11";
      this.Label11.RightToLeft = RightToLeft.No;
      this.Label11.Size = new Size(57, 29);
      this.Label11.TabIndex = 46;
      this.Label11.Text = "Сумма бонусов";
      this.Label16.BackColor = SystemColors.Control;
      this.Label16.Cursor = Cursors.Default;
      this.Label16.ForeColor = SystemColors.ControlText;
      this.Label16.Location = new Point(52, 344);
      this.Label16.Name = "Label16";
      this.Label16.RightToLeft = RightToLeft.No;
      this.Label16.Size = new Size(94, 17);
      this.Label16.TabIndex = 45;
      this.Label16.Text = "Скидка на чек:";
      this.Label23.BackColor = SystemColors.Control;
      this.Label23.Cursor = Cursors.Default;
      this.Label23.ForeColor = SystemColors.ControlText;
      this.Label23.Location = new Point(280, 344);
      this.Label23.Name = "Label23";
      this.Label23.RightToLeft = RightToLeft.No;
      this.Label23.Size = new Size(81, 17);
      this.Label23.TabIndex = 44;
      this.Label23.Text = "Общая сумма:";
      this.TextGoodCode.AcceptsReturn = true;
      this.TextGoodCode.BackColor = SystemColors.Window;
      this.TextGoodCode.Cursor = Cursors.IBeam;
      this.TextGoodCode.ForeColor = SystemColors.WindowText;
      this.TextGoodCode.Location = new Point(20, 96);
      this.TextGoodCode.MaxLength = 0;
      this.TextGoodCode.Multiline = true;
      this.TextGoodCode.Name = "TextGoodCode";
      this.TextGoodCode.RightToLeft = RightToLeft.No;
      this.TextGoodCode.ScrollBars = ScrollBars.Horizontal;
      this.TextGoodCode.Size = new Size(49, 257);
      this.TextGoodCode.TabIndex = 68;
      this.TextGoodCode.WordWrap = false;
      this.FrameMain.BackColor = SystemColors.Control;
      this.FrameMain.Controls.Add((Control) this.FrameSourceCheque);
      this.FrameMain.Controls.Add((Control) this.Frame1);
      this.FrameMain.ForeColor = SystemColors.ControlText;
      this.FrameMain.Location = new Point(4, 0);
      this.FrameMain.Name = "FrameMain";
      this.FrameMain.RightToLeft = RightToLeft.No;
      this.FrameMain.Size = new Size(1081, 585);
      this.FrameMain.TabIndex = 67;
      this.FrameMain.TabStop = false;
      this.FrameSourceCheque.BackColor = SystemColors.Control;
      this.FrameSourceCheque.Controls.Add((Control) this.ButtomPaySource);
      this.FrameSourceCheque.Controls.Add((Control) this.TextSourceChequeAmount);
      this.FrameSourceCheque.Controls.Add((Control) this.TextGoodAmount);
      this.FrameSourceCheque.Controls.Add((Control) this.TextGoodPrice);
      this.FrameSourceCheque.Controls.Add((Control) this.TextGoodQuantity);
      this.FrameSourceCheque.Controls.Add((Control) this.TextGoodName);
      this.FrameSourceCheque.Controls.Add((Control) this.Label2);
      this.FrameSourceCheque.Controls.Add((Control) this.Label1);
      this.FrameSourceCheque.Controls.Add((Control) this.Label7);
      this.FrameSourceCheque.Controls.Add((Control) this.Label6);
      this.FrameSourceCheque.Controls.Add((Control) this.Label5);
      this.FrameSourceCheque.Controls.Add((Control) this.Label4);
      this.FrameSourceCheque.Controls.Add((Control) this.LabelSourceCheque);
      this.FrameSourceCheque.ForeColor = SystemColors.ControlText;
      this.FrameSourceCheque.Location = new Point(8, 16);
      this.FrameSourceCheque.Name = "FrameSourceCheque";
      this.FrameSourceCheque.RightToLeft = RightToLeft.No;
      this.FrameSourceCheque.Size = new Size(361, 561);
      this.FrameSourceCheque.TabIndex = 8;
      this.FrameSourceCheque.TabStop = false;
      this.ButtomPaySource.BackColor = SystemColors.Control;
      this.ButtomPaySource.Cursor = Cursors.Default;
      this.ButtomPaySource.ForeColor = SystemColors.ControlText;
      this.ButtomPaySource.Location = new Point(144, 392);
      this.ButtomPaySource.Name = "ButtomPaySource";
      this.ButtomPaySource.RightToLeft = RightToLeft.No;
      this.ButtomPaySource.Size = new Size(113, 25);
      this.ButtomPaySource.TabIndex = 22;
      this.ButtomPaySource.Text = "Оплатить";
      this.ButtomPaySource.UseVisualStyleBackColor = false;
      this.ButtomPaySource.Click += new EventHandler(this.ButtomPaySource_Click);
      this.TextSourceChequeAmount.AcceptsReturn = true;
      this.TextSourceChequeAmount.BackColor = SystemColors.Window;
      this.TextSourceChequeAmount.Cursor = Cursors.IBeam;
      this.TextSourceChequeAmount.ForeColor = SystemColors.WindowText;
      this.TextSourceChequeAmount.Location = new Point(160, 352);
      this.TextSourceChequeAmount.MaxLength = 0;
      this.TextSourceChequeAmount.Name = "TextSourceChequeAmount";
      this.TextSourceChequeAmount.RightToLeft = RightToLeft.No;
      this.TextSourceChequeAmount.Size = new Size(81, 20);
      this.TextSourceChequeAmount.TabIndex = 20;
      this.TextSourceChequeAmount.Text = "0.0";
      this.TextGoodAmount.AcceptsReturn = true;
      this.TextGoodAmount.BackColor = SystemColors.Window;
      this.TextGoodAmount.Cursor = Cursors.IBeam;
      this.TextGoodAmount.ForeColor = SystemColors.WindowText;
      this.TextGoodAmount.Location = new Point(288, 80);
      this.TextGoodAmount.MaxLength = 0;
      this.TextGoodAmount.Multiline = true;
      this.TextGoodAmount.Name = "TextGoodAmount";
      this.TextGoodAmount.RightToLeft = RightToLeft.No;
      this.TextGoodAmount.ScrollBars = ScrollBars.Horizontal;
      this.TextGoodAmount.Size = new Size(57, 257);
      this.TextGoodAmount.TabIndex = 14;
      this.TextGoodAmount.WordWrap = false;
      this.TextGoodPrice.AcceptsReturn = true;
      this.TextGoodPrice.BackColor = SystemColors.Window;
      this.TextGoodPrice.Cursor = Cursors.IBeam;
      this.TextGoodPrice.ForeColor = SystemColors.WindowText;
      this.TextGoodPrice.Location = new Point(232, 80);
      this.TextGoodPrice.MaxLength = 0;
      this.TextGoodPrice.Multiline = true;
      this.TextGoodPrice.Name = "TextGoodPrice";
      this.TextGoodPrice.RightToLeft = RightToLeft.No;
      this.TextGoodPrice.ScrollBars = ScrollBars.Horizontal;
      this.TextGoodPrice.Size = new Size(57, 257);
      this.TextGoodPrice.TabIndex = 12;
      this.TextGoodPrice.WordWrap = false;
      this.TextGoodQuantity.AcceptsReturn = true;
      this.TextGoodQuantity.BackColor = SystemColors.Window;
      this.TextGoodQuantity.Cursor = Cursors.IBeam;
      this.TextGoodQuantity.ForeColor = SystemColors.WindowText;
      this.TextGoodQuantity.Location = new Point(184, 80);
      this.TextGoodQuantity.MaxLength = 0;
      this.TextGoodQuantity.Multiline = true;
      this.TextGoodQuantity.Name = "TextGoodQuantity";
      this.TextGoodQuantity.RightToLeft = RightToLeft.No;
      this.TextGoodQuantity.ScrollBars = ScrollBars.Horizontal;
      this.TextGoodQuantity.Size = new Size(49, 257);
      this.TextGoodQuantity.TabIndex = 11;
      this.TextGoodQuantity.WordWrap = false;
      this.TextGoodName.AcceptsReturn = true;
      this.TextGoodName.BackColor = SystemColors.Window;
      this.TextGoodName.Cursor = Cursors.IBeam;
      this.TextGoodName.ForeColor = SystemColors.WindowText;
      this.TextGoodName.Location = new Point(56, 80);
      this.TextGoodName.MaxLength = 0;
      this.TextGoodName.Multiline = true;
      this.TextGoodName.Name = "TextGoodName";
      this.TextGoodName.RightToLeft = RightToLeft.No;
      this.TextGoodName.ScrollBars = ScrollBars.Horizontal;
      this.TextGoodName.Size = new Size(129, 257);
      this.TextGoodName.TabIndex = 10;
      this.TextGoodName.WordWrap = false;
      this.Label2.BackColor = SystemColors.Control;
      this.Label2.Cursor = Cursors.Default;
      this.Label2.ForeColor = SystemColors.ControlText;
      this.Label2.Location = new Point(88, 360);
      this.Label2.Name = "Label2";
      this.Label2.RightToLeft = RightToLeft.No;
      this.Label2.Size = new Size(73, 25);
      this.Label2.TabIndex = 21;
      this.Label2.Text = "Сумма чека:";
      this.Label1.BackColor = SystemColors.Control;
      this.Label1.Cursor = Cursors.Default;
      this.Label1.ForeColor = SystemColors.ControlText;
      this.Label1.Location = new Point(8, 64);
      this.Label1.Name = "Label1";
      this.Label1.RightToLeft = RightToLeft.No;
      this.Label1.Size = new Size(42, 17);
      this.Label1.TabIndex = 19;
      this.Label1.Text = "Код";
      this.Label7.BackColor = SystemColors.Control;
      this.Label7.Cursor = Cursors.Default;
      this.Label7.ForeColor = SystemColors.ControlText;
      this.Label7.Location = new Point(290, 64);
      this.Label7.Name = "Label7";
      this.Label7.RightToLeft = RightToLeft.No;
      this.Label7.Size = new Size(65, 17);
      this.Label7.TabIndex = 18;
      this.Label7.Text = "Стоимость";
      this.Label6.BackColor = SystemColors.Control;
      this.Label6.Cursor = Cursors.Default;
      this.Label6.ForeColor = SystemColors.ControlText;
      this.Label6.Location = new Point(232, 51);
      this.Label6.Name = "Label6";
      this.Label6.RightToLeft = RightToLeft.No;
      this.Label6.Size = new Size(57, 26);
      this.Label6.TabIndex = 17;
      this.Label6.Text = "Цена за единицу";
      this.Label5.BackColor = SystemColors.Control;
      this.Label5.Cursor = Cursors.Default;
      this.Label5.ForeColor = SystemColors.ControlText;
      this.Label5.Location = new Point(185, 64);
      this.Label5.Name = "Label5";
      this.Label5.RightToLeft = RightToLeft.No;
      this.Label5.Size = new Size(41, 17);
      this.Label5.TabIndex = 16;
      this.Label5.Text = "Кол-во";
      this.Label4.BackColor = SystemColors.Control;
      this.Label4.Cursor = Cursors.Default;
      this.Label4.ForeColor = SystemColors.ControlText;
      this.Label4.Location = new Point(63, 64);
      this.Label4.Name = "Label4";
      this.Label4.RightToLeft = RightToLeft.No;
      this.Label4.Size = new Size(89, 17);
      this.Label4.TabIndex = 15;
      this.Label4.Text = "Наименование";
      this.LabelSourceCheque.BackColor = SystemColors.Control;
      this.LabelSourceCheque.Cursor = Cursors.Default;
      this.LabelSourceCheque.ForeColor = SystemColors.ControlText;
      this.LabelSourceCheque.Location = new Point(136, 8);
      this.LabelSourceCheque.Name = "LabelSourceCheque";
      this.LabelSourceCheque.RightToLeft = RightToLeft.No;
      this.LabelSourceCheque.Size = new Size(86, 25);
      this.LabelSourceCheque.TabIndex = 9;
      this.LabelSourceCheque.Text = "Исходный чек";
      this.Check_DiscountByGoods.BackColor = SystemColors.Control;
      this.Check_DiscountByGoods.Cursor = Cursors.Default;
      this.Check_DiscountByGoods.ForeColor = SystemColors.ControlText;
      this.Check_DiscountByGoods.Location = new Point(472, 588);
      this.Check_DiscountByGoods.Name = "Check_DiscountByGoods";
      this.Check_DiscountByGoods.RightToLeft = RightToLeft.No;
      this.Check_DiscountByGoods.Size = new Size(129, 17);
      this.Check_DiscountByGoods.TabIndex = 77;
      this.Check_DiscountByGoods.Text = "Скидка товаром";
      this.Check_DiscountByGoods.UseVisualStyleBackColor = false;
      this.CheckAcceptCheque.BackColor = SystemColors.Control;
      this.CheckAcceptCheque.Cursor = Cursors.Default;
      this.CheckAcceptCheque.ForeColor = SystemColors.ControlText;
      this.CheckAcceptCheque.Location = new Point(892, 585);
      this.CheckAcceptCheque.Name = "CheckAcceptCheque";
      this.CheckAcceptCheque.RightToLeft = RightToLeft.No;
      this.CheckAcceptCheque.Size = new Size(137, 19);
      this.CheckAcceptCheque.TabIndex = 75;
      this.CheckAcceptCheque.Text = "Check_AcceptCheque";
      this.CheckAcceptCheque.UseVisualStyleBackColor = false;
      this.CheckAcceptCheque.Visible = false;
      this.ScrMessage.AcceptsReturn = true;
      this.ScrMessage.BackColor = SystemColors.Window;
      this.ScrMessage.Cursor = Cursors.IBeam;
      this.ScrMessage.ForeColor = SystemColors.WindowText;
      this.ScrMessage.Location = new Point(540, 608);
      this.ScrMessage.MaxLength = 0;
      this.ScrMessage.Multiline = true;
      this.ScrMessage.Name = "ScrMessage";
      this.ScrMessage.RightToLeft = RightToLeft.No;
      this.ScrMessage.Size = new Size(542, 105);
      this.ScrMessage.TabIndex = 71;
      this.PrnMessage.AcceptsReturn = true;
      this.PrnMessage.BackColor = SystemColors.Window;
      this.PrnMessage.Cursor = Cursors.IBeam;
      this.PrnMessage.ForeColor = SystemColors.WindowText;
      this.PrnMessage.Location = new Point(8, 608);
      this.PrnMessage.MaxLength = 0;
      this.PrnMessage.Multiline = true;
      this.PrnMessage.Name = "PrnMessage";
      this.PrnMessage.RightToLeft = RightToLeft.No;
      this.PrnMessage.Size = new Size(518, 105);
      this.PrnMessage.TabIndex = 70;
      this.Label15.BackColor = SystemColors.Info;
      this.Label15.Cursor = Cursors.Default;
      this.Label15.ForeColor = SystemColors.ControlText;
      this.Label15.Location = new Point(444, 400);
      this.Label15.Name = "Label15";
      this.Label15.RightToLeft = RightToLeft.No;
      this.Label15.Size = new Size(97, 25);
      this.Label15.TabIndex = 69;
      this.Label15.Text = "Сумма наличными:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1088, 713);
      this.Controls.Add((Control) this.Label21);
      this.Controls.Add((Control) this.Label19);
      this.Controls.Add((Control) this.TextGoodCode);
      this.Controls.Add((Control) this.FrameMain);
      this.Controls.Add((Control) this.Check_DiscountByGoods);
      this.Controls.Add((Control) this.CheckAcceptCheque);
      this.Controls.Add((Control) this.ScrMessage);
      this.Controls.Add((Control) this.PrnMessage);
      this.Controls.Add((Control) this.Label15);
      this.MaximumSize = new Size(1104, 751);
      this.MinimumSize = new Size(1104, 751);
      this.Name = nameof (DialogCheque);
      this.Text = "Модификация чека...";
      this.Frame1.ResumeLayout(false);
      this.Frame1.PerformLayout();
      this.Frame2.ResumeLayout(false);
      this.Frame2.PerformLayout();
      this.FrameMain.ResumeLayout(false);
      this.FrameSourceCheque.ResumeLayout(false);
      this.FrameSourceCheque.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
