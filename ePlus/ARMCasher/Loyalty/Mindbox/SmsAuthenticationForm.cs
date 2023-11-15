// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Mindbox.SmsAuthenticationForm
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCommon.Controls;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Mindbox
{
  public class SmsAuthenticationForm : Form
  {
    private IContainer components;
    private ARMTextBox armTextBoxCode;
    private ARMButton armButtonOk;
    private ARMButton armButtonCancel;
    private ARMLabel armLabel1;
    private ARMButton armButtonResend;

    public SmsAuthenticationForm()
    {
      this.InitializeComponent();
      this.FormClosing += new FormClosingEventHandler(this.SmsAuthenticationForm_FormClosing);
    }

    private void SmsAuthenticationForm_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || !string.IsNullOrEmpty(this.armTextBoxCode.Text))
        return;
      e.Cancel = true;
    }

    public string Code => this.armTextBoxCode.Text;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (SmsAuthenticationForm));
      this.armTextBoxCode = new ARMTextBox();
      this.armButtonOk = new ARMButton();
      this.armButtonCancel = new ARMButton();
      this.armLabel1 = new ARMLabel();
      this.armButtonResend = new ARMButton();
      this.SuspendLayout();
      this.armTextBoxCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.armTextBoxCode.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armTextBoxCode.Location = new Point(15, 39);
      this.armTextBoxCode.Margin = new Padding(6);
      this.armTextBoxCode.Name = "armTextBoxCode";
      this.armTextBoxCode.Size = new Size(387, 23);
      this.armTextBoxCode.TabIndex = 0;
      this.armButtonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.armButtonOk.DialogResult = DialogResult.OK;
      this.armButtonOk.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armButtonOk.Location = new Point(252, 88);
      this.armButtonOk.Margin = new Padding(6);
      this.armButtonOk.Name = "armButtonOk";
      this.armButtonOk.Size = new Size(150, 42);
      this.armButtonOk.TabIndex = 1;
      this.armButtonOk.Text = "ОК";
      this.armButtonOk.UseVisualStyleBackColor = true;
      this.armButtonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.armButtonCancel.DialogResult = DialogResult.Cancel;
      this.armButtonCancel.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armButtonCancel.Location = new Point(3, 74);
      this.armButtonCancel.Margin = new Padding(6);
      this.armButtonCancel.Name = "armButtonCancel";
      this.armButtonCancel.Size = new Size(150, 42);
      this.armButtonCancel.TabIndex = 2;
      this.armButtonCancel.Text = "Отмена";
      this.armButtonCancel.UseVisualStyleBackColor = true;
      this.armButtonCancel.Visible = false;
      this.armLabel1.AutoSize = true;
      this.armLabel1.CharacterCasing = CharacterCasing.Normal;
      this.armLabel1.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armLabel1.Location = new Point(15, 9);
      this.armLabel1.Margin = new Padding(6, 0, 6, 0);
      this.armLabel1.Name = "armLabel1";
      this.armLabel1.Size = new Size(181, 16);
      this.armLabel1.TabIndex = 3;
      this.armLabel1.Text = "Код из СМС сообщения:";
      this.armButtonResend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.armButtonResend.DialogResult = DialogResult.Retry;
      this.armButtonResend.Font = new Font("Arial", 10f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armButtonResend.Location = new Point(15, 88);
      this.armButtonResend.Margin = new Padding(6);
      this.armButtonResend.Name = "armButtonResend";
      this.armButtonResend.Size = new Size(225, 42);
      this.armButtonResend.TabIndex = 4;
      this.armButtonResend.Text = "Повторить отправку кода";
      this.armButtonResend.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this.armButtonOk;
      this.AutoScaleDimensions = new SizeF(10f, 19f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.armButtonCancel;
      this.ClientSize = new Size(417, 145);
      this.Controls.Add((Control) this.armButtonResend);
      this.Controls.Add((Control) this.armLabel1);
      this.Controls.Add((Control) this.armButtonCancel);
      this.Controls.Add((Control) this.armButtonOk);
      this.Controls.Add((Control) this.armTextBoxCode);
      this.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Margin = new Padding(6);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (SmsAuthenticationForm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Необходимо ввести код подтверждения";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
