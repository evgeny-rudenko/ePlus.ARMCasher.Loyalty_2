// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.AstraZeneca.Forms.FormConfirmationCode
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMUtils;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.AstraZeneca.Forms
{
  public class FormConfirmationCode : Form
  {
    private IContainer components;
    private Button buttonOK;
    private Button buttonCancel;
    private Label label1;
    private TextBox textBoxConfirmationCode;

    public FormConfirmationCode() => this.InitializeComponent();

    public string ConfirmationCode => this.textBoxConfirmationCode.Text.Trim();

    private bool ValidateValues() => !string.IsNullOrWhiteSpace(this.ConfirmationCode);

    private void FormConfirmationCode_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.OK || this.ValidateValues())
        return;
      int num = (int) UtilsArm.ShowMessageInformationOK("Для продолжения необходимо ввести код подтверждения.");
      e.Cancel = true;
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
      this.SuspendLayout();
      this.buttonOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOK.DialogResult = DialogResult.OK;
      this.buttonOK.Location = new Point(39, 99);
      this.buttonOK.Margin = new Padding(5);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new Size(150, 45);
      this.buttonOK.TabIndex = 2;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Location = new Point(198, 99);
      this.buttonCancel.Margin = new Padding(5);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(150, 45);
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
      this.textBoxConfirmationCode.Size = new Size(334, 30);
      this.textBoxConfirmationCode.TabIndex = 1;
      this.AcceptButton = (IButtonControl) this.buttonOK;
      this.AutoScaleDimensions = new SizeF(12f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(364, 161);
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
      this.Name = nameof (FormConfirmationCode);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Введите код";
      this.FormClosing += new FormClosingEventHandler(this.FormConfirmationCode_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
