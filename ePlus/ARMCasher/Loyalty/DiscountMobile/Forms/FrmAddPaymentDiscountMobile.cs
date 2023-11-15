// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.DiscountMobile.Forms.FrmAddPaymentDiscountMobile
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.CommonEx.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.DiscountMobile.Forms
{
  internal class FrmAddPaymentDiscountMobile : Form
  {
    private Decimal _maxSum;
    private Decimal _chequeSum;
    private bool _blockForm;
    private Decimal _scorePerSum;
    private IContainer components;
    private ePlusNumericBox txtSum;
    private Label label1;
    private Button btnOk;
    private Button btnCancel;
    private Label label2;
    private ErrorProvider errorProvider1;
    private TextBox txtMaxSum;
    private TextBox txtChequeSum;
    private Label label3;
    private TextBox txtMaxSumRub;
    private Label label5;
    private ePlusNumericBox txtSumRub;
    private Label label4;

    public Decimal ScorePerSum
    {
      get => this._scorePerSum;
      set => this._scorePerSum = value;
    }

    public Decimal MaxSum
    {
      get => this._maxSum;
      set => this._maxSum = value;
    }

    public Decimal ChequeSum
    {
      get => this._chequeSum;
      set => this._chequeSum = value;
    }

    public Decimal Sum
    {
      get => this.txtSum.Value;
      set => this.txtSum.Value = (Decimal) (int) value;
    }

    public bool BlockForm
    {
      get => this._blockForm;
      set => this._blockForm = value;
    }

    public FrmAddPaymentDiscountMobile() => this.InitializeComponent();

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Return:
          this.BtnOkClick((object) null, (EventArgs) null);
          break;
        case Keys.Escape:
          this.BtnCancelClick((object) null, (EventArgs) null);
          break;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    private void BtnOkClick(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.errorProvider1.GetError((Control) this.txtSum)))
        this.DialogResult = DialogResult.OK;
      else
        this.ActiveControl = (Control) this.txtSum;
    }

    private void BtnCancelClick(object sender, EventArgs e)
    {
      if (this._blockForm)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void TxtSumValueChanged(object sender, EventArgs e)
    {
      this.txtSumRub.Value = this.txtSum.Value / this.ScorePerSum;
      if (this.Sum > this.MaxSum)
        this.errorProvider1.SetError((Control) this.txtSum, "Сумма больше максимально возможной");
      else if (this.Sum == 0M)
        this.errorProvider1.SetError((Control) this.txtSum, "Сумма нулевая");
      else
        this.errorProvider1.SetError((Control) this.txtSum, "");
    }

    private void FrmAddPCXPaymentLoad(object sender, EventArgs e)
    {
      Decimal num = this.MaxSum / this.ScorePerSum;
      this.txtMaxSum.Text = this.MaxSum.ToString("#0.00");
      this.txtMaxSumRub.Text = num.ToString("#0.00");
      this.txtChequeSum.Text = this.ChequeSum.ToString("#0.00");
      this.ControlBox = this.btnCancel.Enabled = !this._blockForm;
    }

    private void TxtSumRubValueChanged(object sender, EventArgs e) => this.txtSum.Value = this.txtSumRub.Value * this.ScorePerSum;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmAddPaymentDiscountMobile));
      this.txtSum = new ePlusNumericBox();
      this.label1 = new Label();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.label2 = new Label();
      this.errorProvider1 = new ErrorProvider(this.components);
      this.label3 = new Label();
      this.txtChequeSum = new TextBox();
      this.txtMaxSum = new TextBox();
      this.label4 = new Label();
      this.txtSumRub = new ePlusNumericBox();
      this.label5 = new Label();
      this.txtMaxSumRub = new TextBox();
      ((ISupportInitialize) this.errorProvider1).BeginInit();
      this.SuspendLayout();
      this.txtSum.DecimalPlaces = 0;
      this.txtSum.DecimalSeparator = '.';
      this.txtSum.Font = new Font("Arial", 20.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtSum.Location = new Point(198, 45);
      this.txtSum.MaxLength = 18;
      this.txtSum.Name = "txtSum";
      this.txtSum.Positive = true;
      this.txtSum.Size = new Size(133, 39);
      this.txtSum.TabIndex = 0;
      this.txtSum.Text = "0";
      this.txtSum.TextAlign = HorizontalAlignment.Right;
      this.txtSum.ThousandSeparator = ' ';
      this.txtSum.TypingMode = TypingMode.Replace;
      this.txtSum.Value = new Decimal(new int[4]);
      this.txtSum.TextChanged += new EventHandler(this.TxtSumValueChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(20, 55);
      this.label1.Name = "label1";
      this.label1.Size = new Size(148, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "Сумма для списания:";
      this.btnOk.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.btnOk.Location = new Point(81, 124);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(91, 24);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "ОК";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.BtnOkClick);
      this.btnCancel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.btnCancel.Location = new Point(178, 124);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(91, 24);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.BtnCancelClick);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label2.ForeColor = SystemColors.GrayText;
      this.label2.Location = new Point(20, 91);
      this.label2.Name = "label2";
      this.label2.Size = new Size(172, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Максимально возможно:";
      this.errorProvider1.ContainerControl = (ContainerControl) this;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label3.Location = new Point(20, 18);
      this.label3.Name = "label3";
      this.label3.Size = new Size(164, 16);
      this.label3.TabIndex = 1;
      this.label3.Text = "Всего к оплате по чеку:";
      this.txtChequeSum.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtChequeSum.Location = new Point(198, 17);
      this.txtChequeSum.Name = "txtChequeSum";
      this.txtChequeSum.ReadOnly = true;
      this.txtChequeSum.Size = new Size(133, 22);
      this.txtChequeSum.TabIndex = 3;
      this.txtChequeSum.TabStop = false;
      this.txtChequeSum.TextAlign = HorizontalAlignment.Right;
      this.txtMaxSum.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtMaxSum.Location = new Point(198, 90);
      this.txtMaxSum.Name = "txtMaxSum";
      this.txtMaxSum.ReadOnly = true;
      this.txtMaxSum.Size = new Size(133, 22);
      this.txtMaxSum.TabIndex = 3;
      this.txtMaxSum.TabStop = false;
      this.txtMaxSum.TextAlign = HorizontalAlignment.Right;
      this.label4.AutoSize = true;
      this.label4.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label4.Location = new Point(538, 55);
      this.label4.Name = "label4";
      this.label4.Size = new Size(35, 16);
      this.label4.TabIndex = 4;
      this.label4.Text = "руб.";
      this.txtSumRub.DecimalPlaces = 2;
      this.txtSumRub.DecimalSeparator = '.';
      this.txtSumRub.Font = new Font("Arial", 20.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtSumRub.Location = new Point(399, 45);
      this.txtSumRub.MaxLength = 18;
      this.txtSumRub.Name = "txtSumRub";
      this.txtSumRub.Positive = true;
      this.txtSumRub.Size = new Size(133, 39);
      this.txtSumRub.TabIndex = 5;
      this.txtSumRub.Text = "0.00";
      this.txtSumRub.TextAlign = HorizontalAlignment.Right;
      this.txtSumRub.ThousandSeparator = ' ';
      this.txtSumRub.TypingMode = TypingMode.Replace;
      this.txtSumRub.Value = new Decimal(new int[4]
      {
        0,
        0,
        0,
        131072
      });
      this.txtSumRub.TextChanged += new EventHandler(this.TxtSumRubValueChanged);
      this.label5.AutoSize = true;
      this.label5.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label5.Location = new Point(337, 55);
      this.label5.Name = "label5";
      this.label5.Size = new Size(56, 16);
      this.label5.TabIndex = 6;
      this.label5.Text = "баллов";
      this.txtMaxSumRub.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtMaxSumRub.Location = new Point(399, 90);
      this.txtMaxSumRub.Name = "txtMaxSumRub";
      this.txtMaxSumRub.ReadOnly = true;
      this.txtMaxSumRub.Size = new Size(133, 22);
      this.txtMaxSumRub.TabIndex = 7;
      this.txtMaxSumRub.TabStop = false;
      this.txtMaxSumRub.TextAlign = HorizontalAlignment.Right;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(576, 158);
      this.Controls.Add((Control) this.txtMaxSumRub);
      this.Controls.Add((Control) this.label5);
      this.Controls.Add((Control) this.txtSumRub);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.txtMaxSum);
      this.Controls.Add((Control) this.txtChequeSum);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.txtSum);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmAddPaymentDiscountMobile);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Платеж картой ПЦ";
      this.Load += new EventHandler(this.FrmAddPCXPaymentLoad);
      ((ISupportInitialize) this.errorProvider1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
