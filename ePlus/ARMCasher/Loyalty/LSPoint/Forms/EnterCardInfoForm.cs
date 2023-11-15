// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.Forms.EnterCardInfoForm
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint.Forms
{
  internal class EnterCardInfoForm : Form
  {
    private LSPointLoyaltyProgram _lsPoint;
    private IContainer components;
    public ComboBox ComboBoxCardType;
    public TextBox MagTrack1Field;
    public TextBox MagTrack2Field;
    public Label Label50;
    public Label Label49;
    public Label Label47;
    private Button InfoButton;
    private Button PromoButton;
    private Button buttonCancel;

    public EnterCardInfoForm(LSPointLoyaltyProgram lsPoint)
    {
      this._lsPoint = lsPoint;
      this.InitializeComponent();
      this.Label49.Visible = false;
      this.MagTrack1Field.Visible = false;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData != Keys.Escape)
        return base.ProcessCmdKey(ref msg, keyData);
      this.Close();
      return true;
    }

    private void EnterCardInfoFormLoad(object sender, EventArgs e) => this.AcceptButton = (IButtonControl) this.PromoButton;

    private void UpdateCardInfo()
    {
      this._lsPoint.CardNumber = this.MagTrack2Field.Text;
      Bel.Instance.ComboBoxCardType.Text = this.ComboBoxCardType.Text;
      Bel.Instance.MagTrack1Field.Text = Utils.Str2Hex(this.MagTrack1Field.Text);
      Bel.Instance.MagTrack2Field.Text = Utils.Str2Hex(this.MagTrack2Field.Text);
      Bel.Instance.Check_TransmitCardData.Checked = true;
    }

    private void CancelButtonClick(object sender, EventArgs e) => this.Close();

    private void InfoButtonClick(object sender, EventArgs e)
    {
      this.UpdateCardInfo();
      this._lsPoint.Info();
    }

    private void Promo()
    {
      this.UpdateCardInfo();
      if (!this._lsPoint.Promo() || !(this._lsPoint.PaidBonus != 0M) && !(this._lsPoint.PaidCard != 0M) && !(this._lsPoint.PaidCash != 0M))
        return;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void PromoButtonClick(object sender, EventArgs e) => this.Promo();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.ComboBoxCardType = new ComboBox();
      this.MagTrack1Field = new TextBox();
      this.MagTrack2Field = new TextBox();
      this.Label50 = new Label();
      this.Label49 = new Label();
      this.Label47 = new Label();
      this.InfoButton = new Button();
      this.PromoButton = new Button();
      this.buttonCancel = new Button();
      this.SuspendLayout();
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
      this.ComboBoxCardType.Location = new Point(78, 6);
      this.ComboBoxCardType.MaxDropDownItems = 13;
      this.ComboBoxCardType.Name = "ComboBoxCardType";
      this.ComboBoxCardType.Size = new Size(297, 21);
      this.ComboBoxCardType.TabIndex = 161;
      this.ComboBoxCardType.Text = "11 - Карта с магнитной полосой (только бонусная)";
      this.MagTrack1Field.AcceptsReturn = true;
      this.MagTrack1Field.BackColor = SystemColors.Window;
      this.MagTrack1Field.Cursor = Cursors.IBeam;
      this.MagTrack1Field.ForeColor = SystemColors.WindowText;
      this.MagTrack1Field.Location = new Point(78, 33);
      this.MagTrack1Field.MaxLength = 0;
      this.MagTrack1Field.Name = "MagTrack1Field";
      this.MagTrack1Field.RightToLeft = RightToLeft.No;
      this.MagTrack1Field.Size = new Size(297, 20);
      this.MagTrack1Field.TabIndex = 158;
      this.MagTrack2Field.AcceptsReturn = true;
      this.MagTrack2Field.BackColor = SystemColors.Window;
      this.MagTrack2Field.Cursor = Cursors.IBeam;
      this.MagTrack2Field.ForeColor = SystemColors.WindowText;
      this.MagTrack2Field.Location = new Point(78, 59);
      this.MagTrack2Field.MaxLength = 0;
      this.MagTrack2Field.Name = "MagTrack2Field";
      this.MagTrack2Field.RightToLeft = RightToLeft.No;
      this.MagTrack2Field.Size = new Size(297, 20);
      this.MagTrack2Field.TabIndex = 156;
      this.Label50.Cursor = Cursors.Default;
      this.Label50.ForeColor = SystemColors.ControlText;
      this.Label50.Location = new Point(12, 9);
      this.Label50.Name = "Label50";
      this.Label50.RightToLeft = RightToLeft.No;
      this.Label50.Size = new Size(69, 18);
      this.Label50.TabIndex = 160;
      this.Label50.Text = "Тип карты:";
      this.Label49.Cursor = Cursors.Default;
      this.Label49.ForeColor = SystemColors.ControlText;
      this.Label49.Location = new Point(12, 36);
      this.Label49.Name = "Label49";
      this.Label49.RightToLeft = RightToLeft.No;
      this.Label49.Size = new Size(69, 20);
      this.Label49.TabIndex = 159;
      this.Label49.Text = "MagTrack1:";
      this.Label47.Cursor = Cursors.Default;
      this.Label47.ForeColor = SystemColors.ControlText;
      this.Label47.Location = new Point(12, 62);
      this.Label47.Name = "Label47";
      this.Label47.RightToLeft = RightToLeft.No;
      this.Label47.Size = new Size(69, 20);
      this.Label47.TabIndex = 157;
      this.Label47.Text = "Номер:";
      this.InfoButton.Location = new Point(175, 86);
      this.InfoButton.Name = "InfoButton";
      this.InfoButton.Size = new Size(86, 23);
      this.InfoButton.TabIndex = 164;
      this.InfoButton.Text = "Инфо";
      this.InfoButton.UseVisualStyleBackColor = true;
      this.InfoButton.Click += new EventHandler(this.InfoButtonClick);
      this.PromoButton.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.PromoButton.Location = new Point(289, 86);
      this.PromoButton.Name = "PromoButton";
      this.PromoButton.Size = new Size(86, 23);
      this.PromoButton.TabIndex = 165;
      this.PromoButton.Text = "Промо";
      this.PromoButton.UseVisualStyleBackColor = true;
      this.PromoButton.Click += new EventHandler(this.PromoButtonClick);
      this.buttonCancel.Location = new Point(59, 86);
      this.buttonCancel.Name = "CancelButton";
      this.buttonCancel.Size = new Size(86, 23);
      this.buttonCancel.TabIndex = 163;
      this.buttonCancel.Text = "Отмена";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new EventHandler(this.CancelButtonClick);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(387, 122);
      this.Controls.Add((Control) this.PromoButton);
      this.Controls.Add((Control) this.InfoButton);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.ComboBoxCardType);
      this.Controls.Add((Control) this.MagTrack1Field);
      this.Controls.Add((Control) this.MagTrack2Field);
      this.Controls.Add((Control) this.Label50);
      this.Controls.Add((Control) this.Label49);
      this.Controls.Add((Control) this.Label47);
      this.MaximumSize = new Size(403, 160);
      this.MinimumSize = new Size(403, 160);
      this.Name = nameof (EnterCardInfoForm);
      this.Text = "Ввод карты LSPoint";
      this.Load += new EventHandler(this.EnterCardInfoFormLoad);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
