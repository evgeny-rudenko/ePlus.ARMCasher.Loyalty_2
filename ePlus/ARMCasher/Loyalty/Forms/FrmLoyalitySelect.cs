// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmLoyalitySelect
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCommon;
using ePlus.ARMCommon.Controls;
using ePlus.Loyalty;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmLoyalitySelect : Form, IFrmLoyality, IBaseView
  {
    private readonly LinkedList<ArmRadioButton> _rbtns = new LinkedList<ArmRadioButton>();
    private IContainer components;
    private Button buttonAccept;
    private Button buttonCancel;
    private Panel panel1;
    private TableLayoutPanel tableLayoutPanel1;

    public event Action<LoyaltySettings> LoyaltyTypeSelected;

    private void OnLoyaltyTypeSelected(LoyaltySettings obj)
    {
      Action<LoyaltySettings> loyaltyTypeSelected = this.LoyaltyTypeSelected;
      if (loyaltyTypeSelected == null)
        return;
      loyaltyTypeSelected(obj);
    }

    public FrmLoyalitySelect() => this.InitializeComponent();

    private void rbCheckedChanged(object sender, EventArgs e)
    {
      if (!(sender is ArmRadioButton armRadioButton) || !armRadioButton.Checked)
        return;
      this.OnLoyaltyTypeSelected((LoyaltySettings) armRadioButton.Tag);
    }

    public void Bind(Dictionary<LoyaltySettings, string> list)
    {
      this.tableLayoutPanel1.RowCount = list.Count;
      foreach (KeyValuePair<LoyaltySettings, string> keyValuePair in list)
      {
        ArmRadioButton armRadioButton1 = new ArmRadioButton();
        armRadioButton1.Text = keyValuePair.Value;
        armRadioButton1.Tag = (object) keyValuePair.Key;
        armRadioButton1.Dock = DockStyle.None;
        armRadioButton1.AutoSize = true;
        ArmRadioButton armRadioButton2 = armRadioButton1;
        armRadioButton2.CheckedChanged += new EventHandler(this.rbCheckedChanged);
        this.tableLayoutPanel1.Controls.Add((Control) armRadioButton2);
        this._rbtns.AddLast(armRadioButton2);
      }
      if (this._rbtns.Count <= 0)
        return;
      this._rbtns.First.Value.Checked = true;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Up)
      {
        LinkedListNode<ArmRadioButton> last = this._rbtns.Last;
        this._rbtns.RemoveLast();
        this._rbtns.AddFirst(last);
        this._rbtns.First.Value.Checked = true;
        return true;
      }
      if (keyData != Keys.Down)
        return base.ProcessCmdKey(ref msg, keyData);
      LinkedListNode<ArmRadioButton> first = this._rbtns.First;
      this._rbtns.RemoveFirst();
      this._rbtns.AddLast(first);
      this._rbtns.First.Value.Checked = true;
      return true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.buttonAccept = new Button();
      this.buttonCancel = new Button();
      this.panel1 = new Panel();
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.buttonAccept.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonAccept.DialogResult = DialogResult.OK;
      this.buttonAccept.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.buttonAccept.Location = new Point(118, 235);
      this.buttonAccept.Name = "buttonAccept";
      this.buttonAccept.Size = new Size(100, 28);
      this.buttonAccept.TabIndex = 5;
      this.buttonAccept.Text = "ОК";
      this.buttonAccept.UseVisualStyleBackColor = true;
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.DialogResult = DialogResult.Cancel;
      this.buttonCancel.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.buttonCancel.Location = new Point(224, 235);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(100, 28);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "Отмена";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.panel1.Controls.Add((Control) this.tableLayoutPanel1);
      this.panel1.Location = new Point(12, 13);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(312, 212);
      this.panel1.TabIndex = 7;
      this.tableLayoutPanel1.AutoSize = true;
      this.tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.tableLayoutPanel1.ColumnCount = 1;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Dock = DockStyle.Top;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel1.Size = new Size(312, 0);
      this.tableLayoutPanel1.TabIndex = 0;
      this.AcceptButton = (IButtonControl) this.buttonAccept;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.CancelButton = (IButtonControl) this.buttonCancel;
      this.ClientSize = new Size(336, 271);
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.buttonCancel);
      this.Controls.Add((Control) this.buttonAccept);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MaximumSize = new Size(352, 310);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(352, 310);
      this.Name = nameof (FrmLoyalitySelect);
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Выбор программы лояльности";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
    }

    DialogResult IBaseView.ShowDialog() => this.ShowDialog();

    void IBaseView.Close() => this.Close();
  }
}
