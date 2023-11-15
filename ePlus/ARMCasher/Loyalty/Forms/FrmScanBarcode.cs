// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmScanBarcode
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasherNew.Controls;
using ePlus.CommonEx.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmScanBarcode : Form
  {
    private IContainer components;
    private Button btnCancel;
    private Button btnOk;
    private Label lbTitle;
    private ARMPCXBarcodeTextBox txtBarcode;

    public string Barcode => this.txtBarcode.Text;

    public string Title
    {
      get => this.lbTitle.Text;
      set => this.lbTitle.Text = value;
    }

    public FrmScanBarcode() => this.InitializeComponent();

    private void btnOk_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.OK;

    private void btnCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    private void txtBarcode_TextChanged(object sender, EventArgs e) => this.btnOk.Enabled = !string.IsNullOrEmpty(this.txtBarcode.Text.Trim());

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Return:
          if (this.btnOk.Enabled)
          {
            this.btnOk_Click((object) null, (EventArgs) null);
            break;
          }
          break;
        case Keys.Escape:
          this.btnCancel_Click((object) null, (EventArgs) null);
          break;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnCancel = new Button();
      this.btnOk = new Button();
      this.lbTitle = new Label();
      this.txtBarcode = new ARMPCXBarcodeTextBox();
      this.SuspendLayout();
      this.btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnCancel.Location = new Point(287, 83);
      this.btnCancel.Margin = new Padding(6, 6, 6, 6);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(150, 45);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.btnOk.Enabled = false;
      this.btnOk.Location = new Point(125, 83);
      this.btnOk.Margin = new Padding(6, 6, 6, 6);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(150, 45);
      this.btnOk.TabIndex = 2;
      this.btnOk.Text = "ОК";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.lbTitle.AutoSize = true;
      this.lbTitle.Location = new Point(15, 9);
      this.lbTitle.Margin = new Padding(6, 0, 6, 0);
      this.lbTitle.Name = "lbTitle";
      this.lbTitle.Size = new Size(122, 24);
      this.lbTitle.TabIndex = 0;
      this.lbTitle.Text = "Штрих-код:";
      this.txtBarcode.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtBarcode.AutoValidating = false;
      this.txtBarcode.BarcodeType = BarcodeType.Other;
      this.txtBarcode.Location = new Point(15, 39);
      this.txtBarcode.Margin = new Padding(6);
      this.txtBarcode.Name = "txtBarcode";
      this.txtBarcode.SendDataRecivedOnEnter = true;
      this.txtBarcode.Size = new Size(422, 30);
      this.txtBarcode.TabIndex = 1;
      this.txtBarcode.UseBarcodeValidation = false;
      this.txtBarcode.TextChanged += new EventHandler(this.txtBarcode_TextChanged);
      this.AutoScaleDimensions = new SizeF(12f, 24f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(452, 143);
      this.Controls.Add((Control) this.txtBarcode);
      this.Controls.Add((Control) this.lbTitle);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.btnCancel);
      this.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.Margin = new Padding(6, 6, 6, 6);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmScanBarcode);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Введите ШК карты программы лояльности";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
