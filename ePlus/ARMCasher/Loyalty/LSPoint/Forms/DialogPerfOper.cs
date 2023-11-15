// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.Forms.DialogPerfOper
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint.Forms
{
  internal class DialogPerfOper : Form
  {
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
    private const byte EM_TYPE_INPUTBOX = 1;
    private const byte EM_TYPE_INPUTBOX_NUMBER = 2;
    private const byte EM_TYPE_INPUTBOX_BOOLEAN = 3;
    private const byte EM_TYPE_INPUT_CARDDATA = 4;
    private const long EM_FLAG_MANDATORY = 1;
    private const short DLG_CONTROL_MODE_HIDE = 0;
    private const short DLG_CONTROL_MODE_EM_01 = 1;
    private const short DLG_CONTROL_MODE_EM_02 = 2;
    private const short DLG_CONTROL_MODE_EM_03 = 3;
    private const short DLG_CONTROL_MODE_EM_04 = 4;
    private const int MAX_NUM_BUTTONS = 8;
    private bool bCancelOperation;
    private bool bItegralProtocol;
    private bool bPushNext;
    private bool bPushSet;
    private bool IsStrFormatRequest;
    private int IndexButtonSelect = -1;
    private Bel pBEL;
    private string strMagTrack;
    private List<Button> listDynBtn;
    private Label captionBtn;
    public int OperationID;
    private Thread tRecvThread;
    public bool IsCancelled;
    private IContainer components;
    internal TextBox Label_EMBody;
    internal Button Break_Button;
    internal CheckBox CheckBoxDataInHexString;
    internal Label LabelDataCardSet;
    internal Button ButtonSetDataCardFromMainForm;
    public GroupBox Frame_EM;
    internal ComboBox ComboBoxSelectCardType;
    internal Button Button_EMNext;
    internal Button Button_EMSet;
    internal TextBox TextBoxTrack2;
    internal TextBox TextBoxTrack1;
    internal TextBox TextBox_EMValue;
    internal Button Reset_Button;
    internal Label Label_ParformOperMessage;
    internal System.Windows.Forms.Timer Timer1;

    public event DialogPerfOper.ThreadCompleteEventHandler ThreadComplete;

    public Bel pBELForm
    {
      get => this.pBEL;
      set => this.pBEL = value;
    }

    private void EndHandling(int RetCode)
    {
      Bel.Instance.Bpecr1.RetCode = RetCode;
      if (RetCode == 0 && this.bCancelOperation)
        Bel.Instance.Bpecr1.RetCode = 12;
      else
        Bel.Instance.Bpecr1.RetCode = RetCode;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void OK_Button_Click(object sender, EventArgs e)
    {
      this.IsCancelled = true;
      this.CancelOperation();
    }

    private void CancelOperation()
    {
      if (this.bItegralProtocol)
      {
        Bel.Instance.Bpecr1.CancelOperation();
        this.Reset_Button.Text = "Сбросить принудительно";
        this.Break_Button.Enabled = false;
        this.Label_ParformOperMessage.Text = "Подождите, операция прерывается...";
        this.bCancelOperation = true;
      }
      else
      {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
    }

    private void Cancel_Button_Click(object sender, EventArgs e)
    {
      this.Reset_Button.Enabled = false;
      Bel.Instance.Bpecr1.AbortOperation();
      this.tRecvThread.Abort();
      this.DialogResult = DialogResult.Cancel;
      this.IsCancelled = true;
      this.Close();
    }

    private void SetExtraMessVisibble(short ModeShowControls)
    {
      switch (ModeShowControls)
      {
        case 0:
          this.Frame_EM.Visible = false;
          this.Button_EMSet.Visible = false;
          this.Button_EMNext.Visible = false;
          this.TextBox_EMValue.Visible = false;
          this.Label_EMBody.Visible = false;
          this.LabelDataCardSet.Visible = false;
          this.ButtonSetDataCardFromMainForm.Visible = false;
          this.ComboBoxSelectCardType.Visible = false;
          this.TextBoxTrack1.Visible = false;
          this.TextBoxTrack2.Visible = false;
          this.CheckBoxDataInHexString.Visible = false;
          break;
        case 1:
          this.Frame_EM.Visible = true;
          this.Button_EMSet.Visible = true;
          this.Button_EMNext.Visible = true;
          this.TextBox_EMValue.Visible = true;
          this.Label_EMBody.Visible = true;
          this.Frame_EM.Select();
          this.Button_EMSet.Select();
          this.Button_EMSet.Focus();
          break;
        case 2:
          this.Frame_EM.Visible = true;
          this.Button_EMSet.Visible = true;
          this.Button_EMNext.Visible = true;
          this.TextBox_EMValue.Visible = true;
          this.Label_EMBody.Visible = true;
          this.Frame_EM.Select();
          this.Button_EMSet.Select();
          this.Button_EMSet.Focus();
          break;
        case 3:
          this.Frame_EM.Visible = true;
          this.Button_EMSet.Visible = true;
          this.Button_EMNext.Visible = true;
          this.TextBox_EMValue.Visible = true;
          this.Label_EMBody.Visible = true;
          this.Frame_EM.Select();
          this.Button_EMSet.Select();
          this.Button_EMSet.Focus();
          break;
        case 4:
          this.Frame_EM.Visible = true;
          this.Button_EMSet.Visible = true;
          this.Button_EMNext.Visible = true;
          this.TextBox_EMValue.Visible = true;
          this.Label_EMBody.Visible = true;
          this.LabelDataCardSet.Visible = true;
          this.ButtonSetDataCardFromMainForm.Visible = true;
          this.ComboBoxSelectCardType.Visible = true;
          this.TextBoxTrack1.Visible = true;
          this.TextBoxTrack2.Visible = true;
          this.CheckBoxDataInHexString.Visible = true;
          this.Frame_EM.Select();
          this.Button_EMSet.Select();
          this.Button_EMSet.Focus();
          break;
      }
    }

    private void ShowCurrentExtraMess()
    {
      this.bPushSet = false;
      this.bPushNext = false;
      if (Bel.Instance.Bpecr1.ExtraMessageType == (byte) 1 || Bel.Instance.Bpecr1.ExtraMessageType == (byte) 2 || Bel.Instance.Bpecr1.ExtraMessageType == (byte) 3)
      {
        this.TextBox_EMValue.Text = Bel.Instance.Bpecr1.ExtraMessageValue as string;
        this.Label_EMBody.Text = Bel.Instance.Bpecr1.ExtraMessageBody as string;
        if (Bel.Instance.Bpecr1.ExtraMessageFlags == 1U)
          this.Button_EMNext.Enabled = false;
        else
          this.Button_EMNext.Enabled = true;
        string text = this.Label_EMBody.Text;
        if (this.RadioButtonMessageShow(ref text))
        {
          this.Label_EMBody.Text = text;
          this.IsStrFormatRequest = true;
        }
        else
        {
          this.IsStrFormatRequest = false;
          this.SetExtraMessVisibble((short) 1);
        }
      }
      else if (Bel.Instance.Bpecr1.ExtraMessageType == (byte) 4)
      {
        this.TextBox_EMValue.Text = Bel.Instance.Bpecr1.ExtraMessageValue as string;
        this.Label_EMBody.Text = Bel.Instance.Bpecr1.ExtraMessageBody as string;
        if (Bel.Instance.Bpecr1.ExtraMessageFlags == 1U)
          this.Button_EMNext.Enabled = false;
        else
          this.Button_EMNext.Enabled = true;
        this.ComboBoxSelectCardType.SelectedIndex = 12;
        this.SetExtraMessVisibble((short) 4);
      }
      else
      {
        this.SetExtraMessVisibble((short) 0);
        this.bPushNext = true;
      }
    }

    private void CreateButtonList()
    {
      for (int index = 0; index <= 8; ++index)
      {
        Button button = new Button();
        button.Name = "dynBtn_" + index.ToString();
        button.Size = new Size(250, 30);
        button.Location = new Point(60, 90 + index * 40);
        button.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point, Convert.ToByte(204));
        button.ForeColor = Color.Navy;
        button.Text = "";
        button.Click += new EventHandler(this.DynBtn_MouseClk);
        this.Controls.Add((Control) button);
        this.listDynBtn.Add(button);
      }
      for (int index = 0; index <= 8; ++index)
      {
        this.listDynBtn[index].Visible = false;
        this.listDynBtn[index].Enabled = false;
      }
    }

    public bool RadioButtonMessageShow(ref string ExtraMessageStrFull)
    {
      string str1 = (string) null;
      int num1 = 0;
      int num2 = 0;
      try
      {
        List<string> stringList = new List<string>();
        num1 = 0;
        num2 = 0;
        string str2 = ":";
        int length = ExtraMessageStrFull.IndexOf(str2);
        string str3;
        if (length != -1)
        {
          this.captionBtn.Text = ExtraMessageStrFull.Substring(0, length);
          str3 = ExtraMessageStrFull.Substring(length + 1);
        }
        else
          str3 = ExtraMessageStrFull;
        int startIndex = 0;
        num2 = 0;
        string str4 = "/";
        int num3 = str3.IndexOf(str4);
        if (num3 == -1)
          return false;
        this.captionBtn.Enabled = true;
        this.captionBtn.Visible = true;
        for (; num3 != -1; num3 = str3.IndexOf(str4, startIndex))
        {
          str1 = "";
          string str5 = str3.Substring(startIndex, num3 - startIndex);
          startIndex = num3 + 1;
          if (!string.IsNullOrEmpty(str5))
            stringList.Add(str5);
          else
            break;
        }
        string str6 = str3.Substring(startIndex);
        stringList.Add(str6);
        if (stringList.Count > 0)
        {
          this.SetExtraMessVisibble((short) 0);
          for (int index = 0; index <= stringList.Count - 1 && this.listDynBtn.Count != index; ++index)
          {
            this.listDynBtn[index].Text = stringList[index];
            this.listDynBtn[index].Enabled = true;
            this.listDynBtn[index].Visible = true;
            this.listDynBtn[index].Focus();
          }
        }
        this.Update();
        return true;
      }
      catch (Exception ex)
      {
        int num4 = (int) MessageBox.Show(ex.Message);
        return false;
      }
    }

    public void DynBtn_MouseClk(object sender, EventArgs e)
    {
      Button button = (Button) sender;
      if (this.IsStrFormatRequest)
      {
        for (int index = 0; index <= 8; ++index)
        {
          if (button.Equals((object) this.listDynBtn[index]))
            this.IndexButtonSelect = index;
        }
      }
      this.SetExtraMessVisibble((short) 0);
      for (int index = 0; index <= 8; ++index)
      {
        this.listDynBtn[index].Visible = false;
        this.listDynBtn[index].Enabled = false;
      }
      this.captionBtn.Enabled = false;
      this.captionBtn.Visible = false;
      this.Update();
      this.bPushSet = true;
    }

    private void RecvProcess()
    {
      try
      {
        bool flag1 = false;
        switch (this.OperationID)
        {
          case 3:
            Bel.Instance.Bpecr1.PerformPayment();
            break;
          case 4:
            Bel.Instance.Bpecr1.CloseCheque();
            break;
          case 6:
            Bel.Instance.Bpecr1.GetDiscount();
            break;
          case 7:
            Bel.Instance.Bpecr1.PerformPrepaidCharge();
            break;
          case 8:
            Bel.Instance.Bpecr1.PerformCardLinking();
            break;
          case 9:
            Bel.Instance.Bpecr1.PerformCardSubstitution();
            break;
          case 10:
            Bel.Instance.Bpecr1.PerformBprrnCancel();
            break;
          case 11:
            Bel.Instance.Bpecr1.PerformChangePIN();
            break;
          case 12:
            Bel.Instance.Bpecr1.PerformRRNCancel();
            break;
          case 13:
            Bel.Instance.Bpecr1.PerformFormAppliance();
            break;
          case 14:
            Bel.Instance.Bpecr1.GetCardInfo();
            break;
          case 15:
            Bel.Instance.Bpecr1.PerformRollback();
            break;
          case 16:
            Bel.Instance.Bpecr1.PerformPromo();
            break;
          default:
            flag1 = true;
            break;
        }
        if (flag1)
        {
          if (this.ThreadComplete == null)
            return;
          this.ThreadComplete(18);
        }
        else
        {
          int retCode1 = Bel.Instance.Bpecr1.RetCode;
          bool flag2 = Bel.Instance.Bpecr1.UseContextlessProtocol == 1;
          bool flag3 = Bel.Instance.Bpecr1.UseIntegralProtocol == 1;
          if (retCode1 != 0 || !flag2 || !flag3)
          {
            if (this.ThreadComplete == null)
              return;
            this.ThreadComplete(retCode1);
          }
          else
          {
            int num = 0;
            this.bPushSet = false;
            this.bPushNext = false;
            bool flag4 = true;
            for (bool flag5 = Bel.Instance.Bpecr1.IsOperationRunning == 1; flag5; flag5 = Bel.Instance.Bpecr1.IsOperationRunning == 1)
            {
              ushort extraMessageCount = Bel.Instance.Bpecr1.ExtraMessageCount;
              if ((int) extraMessageCount > num && (this.bPushNext || this.bPushSet || flag4))
              {
                if (this.bPushSet)
                {
                  if (this.IsStrFormatRequest)
                  {
                    Bel.Instance.Bpecr1.ExtraMessageValue = (object) (this.IndexButtonSelect + 1).ToString();
                    Bel.Instance.Bpecr1.SetExtraMessage(Convert.ToUInt16(num));
                  }
                  else
                  {
                    Bel.Instance.Bpecr1.ExtraMessageValue = (object) this.TextBox_EMValue.Text;
                    Bel.Instance.Bpecr1.SetExtraMessage(Convert.ToUInt16(num));
                  }
                  this.bPushNext = true;
                }
                if (this.bPushNext && (int) extraMessageCount > num)
                {
                  ++num;
                  flag4 = true;
                }
                this.bPushSet = false;
                this.bPushNext = false;
                if (flag4 && (int) extraMessageCount > num)
                {
                  flag4 = false;
                  Bel.Instance.Bpecr1.GetExtraMessage(Convert.ToUInt16(num));
                  if (Bel.Instance.Bpecr1.RetCode == 0)
                    this.ShowCurrentExtraMess();
                  else
                    this.bPushNext = true;
                }
              }
              Thread.Sleep(100);
            }
            int retCode2 = Bel.Instance.Bpecr1.RetCode;
            if (this.ThreadComplete == null)
              return;
            this.ThreadComplete(retCode2);
          }
        }
      }
      catch (Exception ex)
      {
        if (this.ThreadComplete == null)
          return;
        this.ThreadComplete(2);
      }
    }

    public void DialogPerfOperEventHandler(int RetCode) => this.EndHandling(RetCode);

    private void DialogPerfOper_Load(object sender, EventArgs e)
    {
      try
      {
        bool flag1 = Bel.Instance.Bpecr1.UseContextlessProtocol == 1;
        bool flag2 = Bel.Instance.Bpecr1.UseIntegralProtocol == 1;
        this.bCancelOperation = false;
        this.bItegralProtocol = true;
        if (!flag1 || !flag2)
        {
          this.Break_Button.Visible = false;
          this.Reset_Button.Visible = false;
          this.bItegralProtocol = false;
        }
        this.IsStrFormatRequest = false;
        this.captionBtn = new Label();
        this.captionBtn.Size = new Size(300, 40);
        this.captionBtn.Location = new Point(35, 30);
        this.Controls.Add((Control) this.captionBtn);
        this.captionBtn.Visible = false;
        this.captionBtn.Enabled = false;
        this.captionBtn.BorderStyle = BorderStyle.FixedSingle;
        this.captionBtn.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, Convert.ToByte(204));
        this.captionBtn.TextAlign = ContentAlignment.MiddleCenter;
        this.captionBtn.ForeColor = Color.Maroon;
        this.listDynBtn = new List<Button>();
        this.CreateButtonList();
        this.SetExtraMessVisibble((short) 0);
        Control.CheckForIllegalCrossThreadCalls = false;
        this.tRecvThread = new Thread(new ThreadStart(this.RecvProcess));
        this.tRecvThread.Start();
      }
      catch (Exception ex)
      {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
      }
    }

    private void Label_EMBody_Click(object sender, EventArgs e)
    {
    }

    private void TextBox_EMValue_TextChanged(object sender, EventArgs e)
    {
    }

    private void GroupBox_EM_Enter(object sender, EventArgs e)
    {
    }

    private void Button_EMSet_Click(object sender, EventArgs e)
    {
    }

    private void Button_EMSet_Click_1(object sender, EventArgs e)
    {
      Bel.Instance.Bpecr1.ExtraMessageCardType = this.ComboBoxSelectCardType.SelectedIndex + 1;
      if (this.CheckBoxDataInHexString.Checked)
      {
        string text1 = this.TextBoxTrack1.Text;
        Bel.Instance.Bpecr1.ExtraMessageTrack1 = (object) this.ConverHexString(ref text1);
        string text2 = this.TextBoxTrack2.Text;
        Bel.Instance.Bpecr1.ExtraMessageTrack2 = (object) this.ConverHexString(ref text2);
      }
      else
      {
        Bel.Instance.Bpecr1.ExtraMessageTrack1 = (object) this.TextBoxTrack1.Text;
        Bel.Instance.Bpecr1.ExtraMessageTrack2 = (object) this.TextBoxTrack2.Text;
      }
      this.SetExtraMessVisibble((short) 0);
      this.bPushSet = true;
    }

    private void Button_EMNext_Click(object sender, EventArgs e)
    {
      this.SetExtraMessVisibble((short) 0);
      this.bPushNext = true;
    }

    private void OnKeyPressForm(object sender, KeyPressEventArgs eventArgs)
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

    private void Timer1_Tick(object sender, EventArgs e)
    {
      string Track1 = "";
      string Track2 = "";
      string Track3 = "";
      this.Timer1.Enabled = false;
      if (!this.pBELForm.ConvertMagTrack(ref this.strMagTrack, ref Track1, ref Track2, ref Track3))
        return;
      if (this.CheckBoxDataInHexString.Checked)
      {
        this.TextBoxTrack1.Text = Utils.Str2Hex(Track1);
        this.TextBoxTrack2.Text = Utils.Str2Hex(Track2);
      }
      else
      {
        this.TextBoxTrack1.Text = Track1;
        this.TextBoxTrack2.Text = Track2;
      }
    }

    private void ButtonSetDataCardFromMainForm_Click(object sender, EventArgs e)
    {
      this.ComboBoxSelectCardType.SelectedIndex = this.pBELForm.IndexCardTypeComboBox;
      this.TextBoxTrack1.Text = this.pBELForm.Track1StrValue;
      this.TextBoxTrack2.Text = this.pBELForm.Track2StrValue;
    }

    private string ConverHexString(ref string strSource)
    {
      string str = "";
      int length = strSource.Length;
      for (int index = 1; index <= length / 2; ++index)
      {
        byte CharCode = Convert.ToByte(strSource.Substring((index - 1) * 2, 2), 16);
        str += (string) (object) Strings.ChrW((int) CharCode);
      }
      return str;
    }

    private void DialogPerfOperFormClosed(object sender, FormClosedEventArgs e)
    {
      if (this.tRecvThread.ThreadState != ThreadState.Running)
        return;
      this.tRecvThread.Abort();
    }

    public DialogPerfOper()
    {
      this.InitializeComponent();
      this.ThreadComplete += new DialogPerfOper.ThreadCompleteEventHandler(this.DialogPerfOperEventHandler);
      this.IsCancelled = false;
    }

    private void DialogPerfOperFormClosing(object sender, FormClosingEventArgs e) => this.CancelOperation();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      this.Label_EMBody = new TextBox();
      this.Break_Button = new Button();
      this.CheckBoxDataInHexString = new CheckBox();
      this.LabelDataCardSet = new Label();
      this.ButtonSetDataCardFromMainForm = new Button();
      this.Frame_EM = new GroupBox();
      this.ComboBoxSelectCardType = new ComboBox();
      this.Button_EMNext = new Button();
      this.Button_EMSet = new Button();
      this.TextBoxTrack2 = new TextBox();
      this.TextBoxTrack1 = new TextBox();
      this.TextBox_EMValue = new TextBox();
      this.Reset_Button = new Button();
      this.Label_ParformOperMessage = new Label();
      this.Timer1 = new System.Windows.Forms.Timer(this.components);
      this.Frame_EM.SuspendLayout();
      this.SuspendLayout();
      this.Label_EMBody.BackColor = SystemColors.Window;
      this.Label_EMBody.Location = new Point(9, 21);
      this.Label_EMBody.Multiline = true;
      this.Label_EMBody.Name = "Label_EMBody";
      this.Label_EMBody.ReadOnly = true;
      this.Label_EMBody.ScrollBars = ScrollBars.Vertical;
      this.Label_EMBody.Size = new Size(326, 131);
      this.Label_EMBody.TabIndex = 12;
      this.Break_Button.Anchor = AnchorStyles.None;
      this.Break_Button.Location = new Point(76, 411);
      this.Break_Button.Name = "Break_Button";
      this.Break_Button.Size = new Size(85, 24);
      this.Break_Button.TabIndex = 69;
      this.Break_Button.Text = "Прервать";
      this.Break_Button.Click += new EventHandler(this.OK_Button_Click);
      this.CheckBoxDataInHexString.AutoSize = true;
      this.CheckBoxDataInHexString.Location = new Point(11, 300);
      this.CheckBoxDataInHexString.Name = "CheckBoxDataInHexString";
      this.CheckBoxDataInHexString.Size = new Size(225, 17);
      this.CheckBoxDataInHexString.TabIndex = 11;
      this.CheckBoxDataInHexString.Text = "Данные треков в формате HEX-строки";
      this.CheckBoxDataInHexString.UseVisualStyleBackColor = true;
      this.LabelDataCardSet.AutoSize = true;
      this.LabelDataCardSet.Location = new Point(15, 192);
      this.LabelDataCardSet.Name = "LabelDataCardSet";
      this.LabelDataCardSet.Size = new Size(165, 13);
      this.LabelDataCardSet.TabIndex = 10;
      this.LabelDataCardSet.Text = "Установить из главной формы";
      this.ButtonSetDataCardFromMainForm.Location = new Point(233, 186);
      this.ButtonSetDataCardFromMainForm.Name = "ButtonSetDataCardFromMainForm";
      this.ButtonSetDataCardFromMainForm.Size = new Size(102, 24);
      this.ButtonSetDataCardFromMainForm.TabIndex = 9;
      this.ButtonSetDataCardFromMainForm.Text = "Установить";
      this.ButtonSetDataCardFromMainForm.UseVisualStyleBackColor = true;
      this.ButtonSetDataCardFromMainForm.Click += new EventHandler(this.ButtonSetDataCardFromMainForm_Click);
      this.Frame_EM.BackColor = SystemColors.Control;
      this.Frame_EM.Controls.Add((Control) this.Label_EMBody);
      this.Frame_EM.Controls.Add((Control) this.CheckBoxDataInHexString);
      this.Frame_EM.Controls.Add((Control) this.LabelDataCardSet);
      this.Frame_EM.Controls.Add((Control) this.ButtonSetDataCardFromMainForm);
      this.Frame_EM.Controls.Add((Control) this.ComboBoxSelectCardType);
      this.Frame_EM.Controls.Add((Control) this.Button_EMNext);
      this.Frame_EM.Controls.Add((Control) this.Button_EMSet);
      this.Frame_EM.Controls.Add((Control) this.TextBoxTrack2);
      this.Frame_EM.Controls.Add((Control) this.TextBoxTrack1);
      this.Frame_EM.Controls.Add((Control) this.TextBox_EMValue);
      this.Frame_EM.ForeColor = SystemColors.ControlText;
      this.Frame_EM.Location = new Point(7, 35);
      this.Frame_EM.Name = "Frame_EM";
      this.Frame_EM.RightToLeft = RightToLeft.No;
      this.Frame_EM.Size = new Size(341, 362);
      this.Frame_EM.TabIndex = 71;
      this.Frame_EM.TabStop = false;
      this.Frame_EM.Text = "Экстра сообщение";
      this.ComboBoxSelectCardType.FormattingEnabled = true;
      this.ComboBoxSelectCardType.Items.AddRange(new object[13]
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
      this.ComboBoxSelectCardType.Location = new Point(9, 216);
      this.ComboBoxSelectCardType.MaxDropDownItems = 13;
      this.ComboBoxSelectCardType.Name = "ComboBoxSelectCardType";
      this.ComboBoxSelectCardType.Size = new Size(326, 21);
      this.ComboBoxSelectCardType.TabIndex = 8;
      this.Button_EMNext.Location = new Point(183, 323);
      this.Button_EMNext.Name = "Button_EMNext";
      this.Button_EMNext.Size = new Size(85, 24);
      this.Button_EMNext.TabIndex = 7;
      this.Button_EMNext.Text = "Пропустить";
      this.Button_EMNext.UseVisualStyleBackColor = true;
      this.Button_EMNext.Click += new EventHandler(this.Button_EMNext_Click);
      this.Button_EMSet.Location = new Point(69, 323);
      this.Button_EMSet.Name = "Button_EMSet";
      this.Button_EMSet.Size = new Size(85, 24);
      this.Button_EMSet.TabIndex = 0;
      this.Button_EMSet.Text = "Ответить";
      this.Button_EMSet.UseVisualStyleBackColor = true;
      this.Button_EMSet.Click += new EventHandler(this.Button_EMSet_Click_1);
      this.TextBoxTrack2.Location = new Point(9, 273);
      this.TextBoxTrack2.Name = "TextBoxTrack2";
      this.TextBoxTrack2.Size = new Size(326, 20);
      this.TextBoxTrack2.TabIndex = 5;
      this.TextBoxTrack1.Location = new Point(9, 243);
      this.TextBoxTrack1.Name = "TextBoxTrack1";
      this.TextBoxTrack1.Size = new Size(326, 20);
      this.TextBoxTrack1.TabIndex = 5;
      this.TextBox_EMValue.Location = new Point(9, 161);
      this.TextBox_EMValue.Name = "TextBox_EMValue";
      this.TextBox_EMValue.Size = new Size(326, 20);
      this.TextBox_EMValue.TabIndex = 5;
      this.Reset_Button.Anchor = AnchorStyles.None;
      this.Reset_Button.DialogResult = DialogResult.Cancel;
      this.Reset_Button.Location = new Point(190, 411);
      this.Reset_Button.Name = "Reset_Button";
      this.Reset_Button.Size = new Size(85, 24);
      this.Reset_Button.TabIndex = 70;
      this.Reset_Button.Text = "Сбросить";
      this.Reset_Button.Click += new EventHandler(this.Cancel_Button_Click);
      this.Label_ParformOperMessage.AutoSize = true;
      this.Label_ParformOperMessage.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.Label_ParformOperMessage.Location = new Point(9, 5);
      this.Label_ParformOperMessage.Name = "Label_ParformOperMessage";
      this.Label_ParformOperMessage.Size = new Size(275, 16);
      this.Label_ParformOperMessage.TabIndex = 68;
      this.Label_ParformOperMessage.Text = "Операция выполняется. Ожидайте...";
      this.Timer1.Tick += new EventHandler(this.Timer1_Tick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(355, 441);
      this.ControlBox = false;
      this.Controls.Add((Control) this.Break_Button);
      this.Controls.Add((Control) this.Frame_EM);
      this.Controls.Add((Control) this.Reset_Button);
      this.Controls.Add((Control) this.Label_ParformOperMessage);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(371, 479);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(371, 479);
      this.Name = nameof (DialogPerfOper);
      this.ShowIcon = false;
      this.Text = "Выполнение операции...";
      this.Load += new EventHandler(this.DialogPerfOper_Load);
      this.FormClosed += new FormClosedEventHandler(this.DialogPerfOperFormClosed);
      this.KeyPress += new KeyPressEventHandler(this.OnKeyPressForm);
      this.FormClosing += new FormClosingEventHandler(this.DialogPerfOperFormClosing);
      this.Frame_EM.ResumeLayout(false);
      this.Frame_EM.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    public delegate void EndHandlingCallback(int RetCode);

    public delegate void ThreadCompleteEventHandler(int RetCode);
  }
}
