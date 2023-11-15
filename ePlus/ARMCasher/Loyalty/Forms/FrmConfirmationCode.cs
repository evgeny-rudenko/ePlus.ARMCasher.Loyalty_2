// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmConfirmationCode
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMUtils;
using ePlus.Loyalty;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmConfirmationCode : Form
  {
    private ILoyaltyProgram loyaltyProgram;
    private Timer timer;
    private int counter;
    private IContainer components;
    private Button buttonOK;
    private Button buttonCancel;
    private Label label1;
    private TextBox textBoxConfirmationCode;
    private Button buttonGetCode;
    private StatusStrip statusStrip1;
    private ToolStripStatusLabel messageLabel;

    public event EventHandler GetCodeRequestEvent;

    public FrmConfirmationCode() => this.InitializeComponent();

    public FrmConfirmationCode(ILoyaltyProgram loyaltyProgram, bool codeRequested = false)
      : this()
    {
      this.loyaltyProgram = loyaltyProgram;
      this.InitTimer();
      if (!codeRequested)
        return;
      this.StartTimer();
    }

    private void InitTimer()
    {
      this.timer = new Timer();
      this.timer.Interval = 1000;
      this.timer.Tick += new EventHandler(this.timer_Tick);
    }

    private void timer_Tick(object sender, EventArgs e)
    {
      if (this.counter > 0)
        this.SetButtonText();
      --this.counter;
      if (this.counter >= 0)
        return;
      this.StopTimer();
    }

    private void SetButtonText() => this.messageLabel.Text = string.Format("Получить sms повторно через {0} сек", (object) this.counter);

    private void StartTimer()
    {
      this.counter = 60;
      this.buttonGetCode.Enabled = false;
      this.messageLabel.Visible = true;
      this.SetButtonText();
      this.timer.Start();
    }

    private void StopTimer()
    {
      this.buttonGetCode.Enabled = true;
      this.messageLabel.Visible = false;
      this.timer.Stop();
    }

    public string ConfirmationCode => this.textBoxConfirmationCode.Text.Trim();

    private bool ValidateValues() => !string.IsNullOrWhiteSpace(this.ConfirmationCode);

    private void FrmConfirmationCode_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.ValidateValues())
        return;
      int num = (int) UtilsArm.ShowMessageInformationOK("Введенный код некорректен. Пожалуйста, повторите попытку.");
      e.Cancel = true;
    }

    private void buttonGetCode_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.None;
      if (this.GetCodeRequestEvent == null)
        return;
      this.StartTimer();
      this.GetCodeRequestEvent((object) this.loyaltyProgram, (EventArgs) null);
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
      this.textBoxConfirmationCode = new TextBox();
      this.buttonGetCode = new Button();
      this.statusStrip1 = new StatusStrip();
      this.messageLabel = new ToolStripStatusLabel();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(165, 79);
      this.buttonOK.Margin = new Padding(5);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(100, 38);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(275, 79);
      this.buttonCancel.Margin = new Padding(5);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(100, 38);
      this.buttonCancel.TabIndex = 3;
      this.buttonCancel.Text = "Отмена";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(14, 11);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(216, 24);
      this.label1.TabIndex = 0;
      this.label1.Text = "Код подтверждения:";
      this.textBoxConfirmationCode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.textBoxConfirmationCode.Location = new Point(14, 39);
      this.textBoxConfirmationCode.Margin = new Padding(4);
      this.textBoxConfirmationCode.Name = "textBoxConfirmationCode";
      this.textBoxConfirmationCode.Size = new Size(361, 30);
      this.textBoxConfirmationCode.TabIndex = 1;
      this.buttonGetCode.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonGetCode.DialogResult = DialogResult.OK;
      this.buttonGetCode.Font = new Font("Arial", 10.2f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.buttonGetCode.Location = new Point(14, 79);
      this.buttonGetCode.Margin = new Padding(5);
      this.buttonGetCode.Name = "buttonGetCode";
      this.buttonGetCode.Size = new Size(141, 38);
      this.buttonGetCode.TabIndex = 4;
      this.buttonGetCode.Text = "Получить sms";
      this.buttonGetCode.UseVisualStyleBackColor = true;
      this.buttonGetCode.Click += new EventHandler(this.buttonGetCode_Click);
      this.statusStrip1.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.messageLabel
      });
      this.statusStrip1.Location = new Point(0, 125);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new Size(391, 25);
      this.statusStrip1.SizingGrip = false;
      this.statusStrip1.TabIndex = 5;
      this.messageLabel.ForeColor = SystemColors.Highlight;
      this.messageLabel.Name = "messageLabel";
      this.messageLabel.RightToLeft = RightToLeft.No;
      this.messageLabel.Size = new Size(319, 20);
      this.messageLabel.Text = "Получить sms повторно можно через 45 сек";
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleDimensions = new SizeF(12f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(391, 150);
      this.Controls.Add((Control) this.statusStrip1);
      this.Controls.Add((Control) this.buttonGetCode);
      this.Controls.Add((Control) this.textBoxConfirmationCode);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonOK);
      this.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.Margin = new Padding(5);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmConfirmationCode);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Введите код";
      this.FormClosing += new FormClosingEventHandler(this.FrmConfirmationCode_FormClosing);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
