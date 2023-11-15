// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Cotrols.ucDebit
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCommon.Controls;
using ePlus.ARMUtils;
using ePlus.CommonEx.Controls;
using ePlus.Loyalty;
using ePlus.Loyalty.Domestic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Cotrols
{
  public class ucDebit : UserControl
  {
    private IContainer components;
    private ucBallance ucBallance1;
    private ARMLabel armLabel1;
    private ARMLabel armLabel2;
    private ARMLabel armLabel3;
    private ePlusNumericBox chequeTotal;
    private ePlusNumericBox maxAllowSum;
    private ePlusNumericBox txtSum;
    private Panel panel1;

    public event EventHandler EmailEditEvent;

    public bool EmailVisible
    {
      get => this.ucBallance1.EmailVisible;
      set => this.ucBallance1.EmailVisible = value;
    }

    public string Email
    {
      set => this.ucBallance1.Email = value;
    }

    public ucDebit()
    {
      this.InitializeComponent();
      this.ucBallance1.EmailEditEvent += new EventHandler(this.ucBallance1_EmailEditEvent);
    }

    public void Bind(ILoyaltyProgram obj, Decimal discountSum, CHEQUE cheque)
    {
      Params @params = (Params) null;
      if (obj.LoyaltyType == LoyaltyType.Domestic)
      {
        this.txtSum.DecimalPlaces = 2;
        this.txtSum.Text = "0,00";
        @params = obj.GetLoyaltyParams() as Params;
      }
      else
      {
        this.txtSum.DecimalPlaces = 0;
        this.txtSum.Text = "0";
      }
      this.ucBallance1.Bind(obj.GetLoyaltyCardInfo());
      Decimal maxSumBonus = obj.CalculateMaxSumBonus(cheque);
      this.maxAllowSum.Value = maxSumBonus;
      this.chequeTotal.Value = cheque.SUMM + obj.CalculateDiscountSum(cheque);
      if (obj.LoyaltyType == LoyaltyType.Domestic && @params != null && @params.ApplyDiscountAsPrepayment)
        this.txtSum.Value = discountSum == 0M ? maxSumBonus : (discountSum < maxSumBonus ? discountSum : maxSumBonus);
      else
        this.txtSum.Value = Math.Truncate(discountSum == 0M ? maxSumBonus : (discountSum < maxSumBonus ? discountSum : maxSumBonus));
    }

    private void ucBallance1_EmailEditEvent(object sender, EventArgs e)
    {
      if (this.EmailEditEvent == null)
        return;
      this.EmailEditEvent((object) this, e);
    }

    public Decimal DiscountSum => this.txtSum.Value;

    public Decimal MaxAllowSum => this.maxAllowSum.Value;

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) => keyData == Keys.Return && !this.Validate() || base.ProcessCmdKey(ref msg, keyData);

    private void ucDebit_Load(object sender, EventArgs e)
    {
      this.txtSum.Select();
      this.txtSum.SelectAll();
    }

    public new bool Validate()
    {
      Decimal num1 = this.maxAllowSum.Value > this.chequeTotal.Value ? this.chequeTotal.Value : this.maxAllowSum.Value;
      if (!(this.txtSum.Value > num1))
        return true;
      int num2 = (int) UtilsArm.ShowMessageExclamationOK(string.Format("Сумма для списания не может превышать {0:#,##0.00}", (object) num1));
      this.txtSum.Select();
      this.txtSum.SelectAll();
      return false;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.armLabel1 = new ARMLabel();
      this.armLabel2 = new ARMLabel();
      this.armLabel3 = new ARMLabel();
      this.chequeTotal = new ePlusNumericBox();
      this.maxAllowSum = new ePlusNumericBox();
      this.txtSum = new ePlusNumericBox();
      this.panel1 = new Panel();
      this.ucBallance1 = new ucBallance();
      this.panel1.SuspendLayout();
      this.SuspendLayout();
      this.armLabel1.AutoSize = true;
      this.armLabel1.CharacterCasing = CharacterCasing.Normal;
      this.armLabel1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armLabel1.Location = new Point(5, 9);
      this.armLabel1.Name = "armLabel1";
      this.armLabel1.Size = new Size(164, 16);
      this.armLabel1.TabIndex = 1;
      this.armLabel1.Text = "Всего к оплате по чеку:";
      this.armLabel2.AutoSize = true;
      this.armLabel2.CharacterCasing = CharacterCasing.Normal;
      this.armLabel2.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armLabel2.Location = new Point(5, 49);
      this.armLabel2.Name = "armLabel2";
      this.armLabel2.Size = new Size(148, 16);
      this.armLabel2.TabIndex = 1;
      this.armLabel2.Text = "Сумма для списания:";
      this.armLabel3.AutoSize = true;
      this.armLabel3.CharacterCasing = CharacterCasing.Normal;
      this.armLabel3.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armLabel3.Location = new Point(5, 82);
      this.armLabel3.Name = "armLabel3";
      this.armLabel3.Size = new Size(172, 16);
      this.armLabel3.TabIndex = 1;
      this.armLabel3.Text = "Максимально возможно:";
      this.chequeTotal.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.chequeTotal.DecimalPlaces = 2;
      this.chequeTotal.DecimalSeparator = ',';
      this.chequeTotal.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.chequeTotal.Location = new Point(181, 6);
      this.chequeTotal.MaxLength = 18;
      this.chequeTotal.Name = "chequeTotal";
      this.chequeTotal.Nullable = false;
      this.chequeTotal.Positive = true;
      this.chequeTotal.ReadOnly = true;
      this.chequeTotal.Size = new Size(468, 22);
      this.chequeTotal.TabIndex = 3;
      this.chequeTotal.Text = "0,00";
      this.chequeTotal.TextAlign = HorizontalAlignment.Right;
      this.chequeTotal.ThousandSeparator = ' ';
      this.chequeTotal.TypingMode = TypingMode.Replace;
      this.chequeTotal.Value = new Decimal(new int[4]
      {
        0,
        0,
        0,
        131072
      });
      this.maxAllowSum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.maxAllowSum.DecimalPlaces = 2;
      this.maxAllowSum.DecimalSeparator = ',';
      this.maxAllowSum.Font = new Font("Arial", 9.75f, FontStyle.Bold);
      this.maxAllowSum.Location = new Point(181, 79);
      this.maxAllowSum.MaxLength = 18;
      this.maxAllowSum.Name = "maxAllowSum";
      this.maxAllowSum.Nullable = false;
      this.maxAllowSum.Positive = true;
      this.maxAllowSum.ReadOnly = true;
      this.maxAllowSum.Size = new Size(468, 22);
      this.maxAllowSum.TabIndex = 3;
      this.maxAllowSum.Text = "0,00";
      this.maxAllowSum.TextAlign = HorizontalAlignment.Right;
      this.maxAllowSum.ThousandSeparator = ' ';
      this.maxAllowSum.TypingMode = TypingMode.Replace;
      this.maxAllowSum.Value = new Decimal(new int[4]
      {
        0,
        0,
        0,
        131072
      });
      this.txtSum.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.txtSum.DecimalPlaces = 0;
      this.txtSum.DecimalSeparator = ',';
      this.txtSum.Font = new Font("Arial", 20.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.txtSum.Location = new Point(181, 34);
      this.txtSum.MaxLength = 18;
      this.txtSum.Name = "txtSum";
      this.txtSum.Nullable = false;
      this.txtSum.Positive = true;
      this.txtSum.Size = new Size(468, 39);
      this.txtSum.TabIndex = 3;
      this.txtSum.Text = "0";
      this.txtSum.TextAlign = HorizontalAlignment.Right;
      this.txtSum.ThousandSeparator = ' ';
      this.txtSum.TypingMode = TypingMode.Replace;
      this.txtSum.Value = new Decimal(new int[4]);
      this.panel1.AutoSize = true;
      this.panel1.Controls.Add((Control) this.armLabel1);
      this.panel1.Controls.Add((Control) this.maxAllowSum);
      this.panel1.Controls.Add((Control) this.armLabel2);
      this.panel1.Controls.Add((Control) this.chequeTotal);
      this.panel1.Controls.Add((Control) this.armLabel3);
      this.panel1.Controls.Add((Control) this.txtSum);
      this.panel1.Dock = DockStyle.Top;
      this.panel1.Location = new Point(0, 90);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(665, 104);
      this.panel1.TabIndex = 4;
      this.ucBallance1.AutoSize = true;
      this.ucBallance1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.ucBallance1.Dock = DockStyle.Top;
      this.ucBallance1.Location = new Point(0, 0);
      this.ucBallance1.Name = "ucBallance1";
      this.ucBallance1.Size = new Size(665, 90);
      this.ucBallance1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.ucBallance1);
      this.Name = nameof (ucDebit);
      this.Size = new Size(665, 232);
      this.Load += new EventHandler(this.ucDebit_Load);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
