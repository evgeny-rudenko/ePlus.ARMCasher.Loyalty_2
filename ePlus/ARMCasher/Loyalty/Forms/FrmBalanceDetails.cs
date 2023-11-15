// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmBalanceDetails
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCommon.Controls;
using ePlus.Loyalty;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmBalanceDetails : Form
  {
    private IContainer components;
    private DataGridView dataGridViewDetails;
    private BindingSource iBalanceInfoRowBindingSource;
    private Panel panel1;
    private ARMButton armButtonOk;
    private DataGridViewTextBoxColumn BalanceTypeName;
    private DataGridViewTextBoxColumn amountDataGridViewTextBoxColumn;
    private DataGridViewTextBoxColumn expirationDateTimeDataGridViewTextBoxColumn;

    public FrmBalanceDetails() => this.InitializeComponent();

    public void Bind(IEnumerable<IBalanceInfoRow> rows) => this.dataGridViewDetails.DataSource = (object) rows;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new System.ComponentModel.Container();
      DataGridViewCellStyle gridViewCellStyle = new DataGridViewCellStyle();
      this.dataGridViewDetails = new DataGridView();
      this.iBalanceInfoRowBindingSource = new BindingSource(this.components);
      this.panel1 = new Panel();
      this.armButtonOk = new ARMButton();
      this.BalanceTypeName = new DataGridViewTextBoxColumn();
      this.amountDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      this.expirationDateTimeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
      ((ISupportInitialize) this.dataGridViewDetails).BeginInit();
      ((ISupportInitialize) this.iBalanceInfoRowBindingSource).BeginInit();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.dataGridViewDetails.AllowUserToAddRows = false;
      this.dataGridViewDetails.AllowUserToDeleteRows = false;
      this.dataGridViewDetails.AutoGenerateColumns = false;
      this.dataGridViewDetails.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewDetails.Columns.AddRange((DataGridViewColumn) this.BalanceTypeName, (DataGridViewColumn) this.amountDataGridViewTextBoxColumn, (DataGridViewColumn) this.expirationDateTimeDataGridViewTextBoxColumn);
      this.dataGridViewDetails.DataSource = (object) this.iBalanceInfoRowBindingSource;
      this.dataGridViewDetails.Dock = DockStyle.Fill;
      this.dataGridViewDetails.Location = new Point(0, 0);
      this.dataGridViewDetails.Margin = new Padding(5, 6, 5, 6);
      this.dataGridViewDetails.Name = "dataGridViewDetails";
      this.dataGridViewDetails.ReadOnly = true;
      this.dataGridViewDetails.RowHeadersVisible = false;
      this.dataGridViewDetails.RowTemplate.ReadOnly = true;
      this.dataGridViewDetails.RowTemplate.Resizable = DataGridViewTriState.False;
      this.dataGridViewDetails.Size = new Size(354, 463);
      this.dataGridViewDetails.TabIndex = 0;
      this.iBalanceInfoRowBindingSource.DataSource = (object) typeof (IBalanceInfoRow);
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.dataGridViewDetails);
      this.panel1.Location = new Point(3, 3);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(354, 463);
      this.panel1.TabIndex = 2;
      this.armButtonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.armButtonOk.DialogResult = DialogResult.OK;
      this.armButtonOk.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armButtonOk.Location = new Point(131, 481);
      this.armButtonOk.Name = "armButtonOk";
      this.armButtonOk.Size = new Size(105, 28);
      this.armButtonOk.TabIndex = 3;
      this.armButtonOk.Text = "ОК";
      this.armButtonOk.UseVisualStyleBackColor = true;
      this.BalanceTypeName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.BalanceTypeName.DataPropertyName = "Name";
      this.BalanceTypeName.HeaderText = "Название";
      this.BalanceTypeName.Name = "BalanceTypeName";
      this.BalanceTypeName.ReadOnly = true;
      this.amountDataGridViewTextBoxColumn.DataPropertyName = "Amount";
      gridViewCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
      gridViewCellStyle.Format = "N2";
      this.amountDataGridViewTextBoxColumn.DefaultCellStyle = gridViewCellStyle;
      this.amountDataGridViewTextBoxColumn.HeaderText = "Доступно";
      this.amountDataGridViewTextBoxColumn.Name = "amountDataGridViewTextBoxColumn";
      this.amountDataGridViewTextBoxColumn.ReadOnly = true;
      this.amountDataGridViewTextBoxColumn.Width = 150;
      this.expirationDateTimeDataGridViewTextBoxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      this.expirationDateTimeDataGridViewTextBoxColumn.DataPropertyName = "ExpirationDateTime";
      this.expirationDateTimeDataGridViewTextBoxColumn.HeaderText = "Дата сгорания";
      this.expirationDateTimeDataGridViewTextBoxColumn.Name = "expirationDateTimeDataGridViewTextBoxColumn";
      this.expirationDateTimeDataGridViewTextBoxColumn.ReadOnly = true;
      this.expirationDateTimeDataGridViewTextBoxColumn.Visible = false;
      this.AcceptButton = (IButtonControl) this.armButtonOk;
      this.AutoScaleDimensions = new SizeF(9f, 18f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.armButtonOk;
      this.ClientSize = new Size(360, 521);
      this.Controls.Add((Control) this.armButtonOk);
      this.Controls.Add((Control) this.panel1);
      this.Font = new Font("Arial", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(5, 6, 5, 6);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmBalanceDetails);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Детализация баланса";
      ((ISupportInitialize) this.dataGridViewDetails).EndInit();
      ((ISupportInitialize) this.iBalanceInfoRowBindingSource).EndInit();
      this.panel1.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
