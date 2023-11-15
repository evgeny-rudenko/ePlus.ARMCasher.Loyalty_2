// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.AstraZeneca.Forms.FormAccountInfo
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCommon.Log;
using ePlus.ARMUtils;
using ePlus.Loyalty;
using ePlus.Loyalty.AstraZeneca;
using ePlus.Loyalty.AstraZeneca.API;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.AstraZeneca.Forms
{
  public class FormAccountInfo : Form
  {
    private IContainer components;
    private Button buttonOK;
    private Button buttonCancel;
    private Label label1;
    private TextBox textBoxCardNumber;
    private Label label2;
    private MaskedTextBox textBoxPhone;

    public FormAccountInfo() => this.InitializeComponent();

    public string CardNumber => this.textBoxCardNumber.Text.Trim();

    private string PhoneEnteredString => this.textBoxPhone.MaskedTextProvider.ToString(false, false);

    public string PhoneNumber => !this.textBoxPhone.MaskFull ? (string) null : "+7" + this.PhoneEnteredString;

    public string ClientId
    {
      get
      {
        string clientId = this.CardNumber;
        if (string.IsNullOrWhiteSpace(this.CardNumber))
          clientId = this.PhoneNumber;
        return clientId;
      }
    }

    private bool ValidateValues()
    {
      if (!string.IsNullOrWhiteSpace(this.PhoneEnteredString) && !this.textBoxPhone.MaskFull)
      {
        int num = (int) UtilsArm.ShowMessageInformationOK("Введен некорректный номер телефона.");
        return false;
      }
      if (!string.IsNullOrWhiteSpace(this.ClientId))
        return true;
      int num1 = (int) UtilsArm.ShowMessageInformationOK("Для продолжения необходимо ввести ШК или номер телефона.");
      return false;
    }

    private void FormAccountInfo_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.OK)
        return;
      if (this.textBoxCardNumber.Focused)
      {
        this.textBoxPhone.Select();
        e.Cancel = true;
      }
      else if (!this.ValidateValues())
      {
        e.Cancel = true;
      }
      else
      {
        string phoneNumber = this.PhoneNumber;
        string cardNumber = this.CardNumber;
        if (string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(cardNumber))
          return;
        ARMLogger.Info("АстраЗенека: регистрация карты");
        AstraZenecaWebApi astraZenecaWebApi = new AstraZenecaWebApi(new SettingsModel().Deserialize<Settings>(((IEnumerable<LoyaltySettings>) LoyaltyProgManager.LoyaltySettings[LoyaltyType.AstraZeneca]).First<LoyaltySettings>().SETTINGS, "Settings"));
        RequestRegister request1 = astraZenecaWebApi.CreateRequest<RequestRegister>();
        request1.CardNumber = cardNumber;
        request1.PhoneNumber = phoneNumber;
        Response response1 = astraZenecaWebApi.Register(request1);
        if (response1.IsSuccess)
        {
          ARMLogger.Info("АстраЗенека: запрашиваем код подтверждения");
          using (FormConfirmationCode confirmationCode = new FormConfirmationCode())
          {
            if (confirmationCode.ShowDialog() == DialogResult.OK)
            {
              RequestConfirmCode request2 = astraZenecaWebApi.CreateRequest<RequestConfirmCode>();
              request2.Code = confirmationCode.ConfirmationCode;
              Response response2 = astraZenecaWebApi.ConfirmCode(request2);
              if (response2.IsSuccess)
                return;
              int num = (int) MessageBox.Show((IWin32Window) this, response2.Message, "Ошибка ввода кода подтверждения", MessageBoxButtons.OK, MessageBoxIcon.Hand);
              e.Cancel = true;
            }
            else
              e.Cancel = true;
          }
        }
        else
        {
          int num = (int) MessageBox.Show((IWin32Window) this, response1.Message, "Ошибка регистрации карты", MessageBoxButtons.OK, MessageBoxIcon.Hand);
          e.Cancel = true;
        }
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
      this.buttonOK = new Button();
      this.buttonCancel = new Button();
      this.label1 = new Label();
      this.textBoxCardNumber = new TextBox();
      this.label2 = new Label();
      this.textBoxPhone = new MaskedTextBox();
      this.SuspendLayout();
      this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(149, 169);
      this.buttonOK.Margin = new Padding(5);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(150, 45);
      this.buttonOK.TabIndex = 4;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(308, 169);
      this.buttonCancel.Margin = new Padding(5);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(150, 45);
      this.buttonCancel.TabIndex = 5;
      this.buttonCancel.Text = "Отмена";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 11);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(101, 19);
      this.label1.TabIndex = 0;
      this.label1.Text = "Штрих-код:";
      this.textBoxCardNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxCardNumber.Location = new Point(14, 39);
      this.textBoxCardNumber.Margin = new Padding(4);
      this.textBoxCardNumber.Name = "textBoxCardNumber";
      this.textBoxCardNumber.Size = new Size(444, 26);
      this.textBoxCardNumber.TabIndex = 1;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(14, 77);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(150, 19);
      this.label2.TabIndex = 2;
      this.label2.Text = "Номер телефона:";
      this.textBoxPhone.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxPhone.Location = new Point(14, 105);
      this.textBoxPhone.Margin = new Padding(4);
      this.textBoxPhone.Mask = "+7 (999) 000-00-00";
      this.textBoxPhone.Name = "textBoxPhone";
      this.textBoxPhone.Size = new Size(444, 26);
      this.textBoxPhone.TabIndex = 3;
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleDimensions = new SizeF(10f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(474, 231);
      this.Controls.Add((Control) this.textBoxPhone);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textBoxCardNumber);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOK);
      this.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.Margin = new Padding(5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FormAccountInfo);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Введите данные";
      this.FormClosing += new FormClosingEventHandler(this.FormAccountInfo_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
