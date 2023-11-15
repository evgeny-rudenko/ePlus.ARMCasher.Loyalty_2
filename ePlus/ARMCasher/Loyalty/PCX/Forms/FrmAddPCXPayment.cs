// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.PCX.Forms.FrmAddPCXPayment
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.CommonEx.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.PCX.Forms
{
  internal class FrmAddPCXPayment : Form
  {
    private Decimal maxSum;
    private Decimal chequeSum;
    private bool blockForm;
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

    public Decimal MaxSum
    {
      get => this.maxSum;
      set => this.maxSum = value;
    }

    public Decimal ChequeSum
    {
      get => this.chequeSum;
      set => this.chequeSum = value;
    }

    public Decimal Sum
    {
      get => this.txtSum.Value;
      set => this.txtSum.Value = value;
    }

    public bool BlockForm
    {
      get => this.blockForm;
      set => this.blockForm = value;
    }

    public FrmAddPCXPayment() => this.InitializeComponent();

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      Keys keys = keyData & Keys.KeyCode;
      switch (keyData)
      {
        case Keys.Return:
          this.btnOk_Click((object) null, (EventArgs) null);
          break;
        case Keys.Escape:
          this.btnCancel_Click((object) null, (EventArgs) null);
          break;
        default:
          if (keys == Keys.F4 && (keyData & Keys.Alt) != Keys.None && this.blockForm)
            return true;
          break;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(this.errorProvider1.GetError((Control) this.txtSum)))
        this.DialogResult = DialogResult.OK;
      else
        this.ActiveControl = (Control) this.txtSum;
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      if (this.blockForm)
        return;
      this.DialogResult = DialogResult.Cancel;
    }

    private void txtSum_ValueChanged(object sender, EventArgs e)
    {
      if (this.Sum > this.MaxSum)
        this.errorProvider1.SetError((Control) this.txtSum, "Сумма больше максимально возможной");
      else if (this.Sum == 0M)
        this.errorProvider1.SetError((Control) this.txtSum, "Сумма нулевая");
      else
        this.errorProvider1.SetError((Control) this.txtSum, "");
    }

    private void FrmAddPCXPayment_Load(object sender, EventArgs e)
    {
      this.txtMaxSum.Text = this.MaxSum.ToString("#0.00");
      this.txtChequeSum.Text = this.ChequeSum.ToString("#0.00");
      this.ControlBox = this.btnCancel.Enabled = !this.blockForm;
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
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmAddPCXPayment));
      this.txtSum = new ePlusNumericBox();
      this.label1 = new Label();
      this.btnOk = new Button();
      this.btnCancel = new Button();
      this.label2 = new Label();
      this.errorProvider1 = new ErrorProvider(this.components);
      this.label3 = new Label();
      this.txtChequeSum = new TextBox();
      this.txtMaxSum = new TextBox();
      ((ISupportInitialize) this.errorProvider1).BeginInit();
      this.SuspendLayout();
      this.txtSum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSum.DecimalPlaces = 2;
      this.txtSum.DecimalSeparator = '.';
      this.txtSum.Font = new Font("Arial", 20.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtSum.Location = new Point(203, 40);
      this.txtSum.MaxLength = 18;
      this.txtSum.Name = "txtSum";
      this.txtSum.Positive = true;
      this.txtSum.Size = new Size(145, 39);
      this.txtSum.TabIndex = 0;
      this.txtSum.Text = "0.00";
      this.txtSum.TextAlign = HorizontalAlignment.Right;
      this.txtSum.ThousandSeparator = ' ';
      this.txtSum.TypingMode = TypingMode.Replace;
      this.txtSum.Value = new Decimal(new int[4]
      {
        0,
        0,
        0,
        131072
      });
      this.txtSum.TextChanged += new EventHandler(this.txtSum_ValueChanged);
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(12, 51);
      this.label1.Name = "label1";
      this.label1.Size = new Size(148, 16);
      this.label1.TabIndex = 1;
      this.label1.Text = "Сумма для списания:";
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.btnOk.Location = new Point(162, 131);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(90, 28);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "ОК";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.btnCancel.Location = new Point(258, 131);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(90, 28);
      this.btnCancel.TabIndex = 2;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label2.ForeColor = SystemColors.GrayText;
      this.label2.Location = new Point(12, 88);
      this.label2.Name = "label2";
      this.label2.Size = new Size(172, 16);
      this.label2.TabIndex = 1;
      this.label2.Text = "Максимально возможно:";
      this.errorProvider1.ContainerControl = (ContainerControl) this;
      this.label3.AutoSize = true;
      this.label3.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label3.Location = new Point(12, 15);
      this.label3.Name = "label3";
      this.label3.Size = new Size(164, 16);
      this.label3.TabIndex = 1;
      this.label3.Text = "Всего к оплате по чеку:";
      this.txtChequeSum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtChequeSum.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtChequeSum.Location = new Point(203, 12);
      this.txtChequeSum.Name = "txtChequeSum";
      this.txtChequeSum.ReadOnly = true;
      this.txtChequeSum.Size = new Size(145, 22);
      this.txtChequeSum.TabIndex = 3;
      this.txtChequeSum.TabStop = false;
      this.txtChequeSum.TextAlign = HorizontalAlignment.Right;
      this.txtMaxSum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtMaxSum.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtMaxSum.Location = new Point(203, 85);
      this.txtMaxSum.Name = "txtMaxSum";
      this.txtMaxSum.ReadOnly = true;
      this.txtMaxSum.Size = new Size(145, 22);
      this.txtMaxSum.TabIndex = 3;
      this.txtMaxSum.TabStop = false;
      this.txtMaxSum.TextAlign = HorizontalAlignment.Right;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(360, 171);
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
      this.Name = nameof (FrmAddPCXPayment);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Платеж картой ПЦ";
      this.Load += new EventHandler(this.FrmAddPCXPayment_Load);
      ((ISupportInitialize) this.errorProvider1).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
