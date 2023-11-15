// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmDebit
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.Cotrols;
using ePlus.ARMCommon.Controls;
using ePlus.Loyalty;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmDebit : Form
  {
    private LoyaltyCardInfo _cardInfo;
    private IContainer components;
    private ucDebit ucDebit;
    private ARMButton btnOk;
    private ARMButton btnCancel;

    public event EventHandler EmailEditEvent;

    public string Email
    {
      set => this.ucDebit.Email = value;
    }

    public FrmDebit()
    {
      this.InitializeComponent();
      this.btnOk.Click += new EventHandler(this.Ok_Click);
      this.btnCancel.Click += new EventHandler(this.Cancel_Click);
      this.ucDebit.EmailEditEvent += new EventHandler(this.ucDebit_EmailEditEvent);
    }

    private void ucDebit_EmailEditEvent(object sender, EventArgs e)
    {
      if (this.EmailEditEvent == null)
        return;
      this.EmailEditEvent((object) this, e);
    }

    public void Bind(ILoyaltyProgram obj, Decimal discountSum, CHEQUE cheque)
    {
      this._cardInfo = obj.GetLoyaltyCardInfo();
      this.btnOk.Enabled = this._cardInfo.CardStatusId != LoyaltyCardStatus.Blocked && this._cardInfo.CardStatusId != LoyaltyCardStatus.NotFound;
      this.ucDebit.Bind(obj, discountSum, cheque);
      this.Text = obj.GetDebitOperationDescription();
      if (obj.LoyaltyType != LoyaltyType.SailPlay)
        return;
      this.ucDebit.EmailVisible = true;
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      switch (keyData)
      {
        case Keys.Return:
          if (this.btnOk.Enabled)
          {
            this.DialogResult = DialogResult.OK;
            this.Close();
            break;
          }
          break;
        case Keys.Escape:
          this.Cancel();
          break;
      }
      return base.ProcessCmdKey(ref msg, keyData);
    }

    private void Ok_Click(object sender, EventArgs e)
    {
      if (!this.ucDebit.Validate())
        return;
      this.DialogResult = DialogResult.OK;
    }

    private void Cancel_Click(object sender, EventArgs e) => this.Cancel();

    private void Cancel()
    {
      this.DialogResult = this._cardInfo.CardStatusId == LoyaltyCardStatus.Blocked ? DialogResult.Abort : DialogResult.Cancel;
      this.Close();
    }

    public Decimal DiscountSum => this.DialogResult != DialogResult.OK ? 0M : this.ucDebit.DiscountSum;

    public Decimal MaxAllowSum => this.ucDebit.MaxAllowSum;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnOk = new ARMButton();
      this.btnCancel = new ARMButton();
      this.ucDebit = new ucDebit();
      this.SuspendLayout();
      this.btnOk.Anchor = AnchorStyles.Bottom;
      this.btnOk.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.btnOk.Location = new Point(91, 242);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(108, 27);
      this.btnOk.TabIndex = 1;
      this.btnOk.Text = "Применить";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnCancel.Anchor = AnchorStyles.Bottom;
      this.btnCancel.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.btnCancel.Location = new Point(205, 242);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(108, 27);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Отмена";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.ucDebit.AutoSize = true;
      this.ucDebit.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ucDebit.Dock = DockStyle.Top;
      this.ucDebit.Location = new Point(0, 0);
      this.ucDebit.Name = "ucDebit";
      this.ucDebit.Size = new Size(393, 194);
      this.ucDebit.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ClientSize = new Size(393, 281);
      this.ControlBox = false;
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.ucDebit);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(409, 320);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(409, 295);
      this.Name = nameof (FrmDebit);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = nameof (FrmDebit);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
