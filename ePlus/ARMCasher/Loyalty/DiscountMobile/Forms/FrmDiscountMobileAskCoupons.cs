// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.DiscountMobile.Forms.FrmDiscountMobileAskCoupons
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.Loyalty.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.DiscountMobile.Forms
{
  internal class FrmDiscountMobileAskCoupons : Form
  {
    public List<long> SelectedCoupons = new List<long>();
    private IContainer components;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private CheckedListBox checkedBoxCoupons;
    private System.Windows.Forms.Label label1;

    public void SetCoupons(List<DiscountMobileCouponItem> coupons)
    {
      this.checkedBoxCoupons.Items.Clear();
      foreach (DiscountMobileCouponItem coupon in coupons)
        this.checkedBoxCoupons.Items.Add((object) new ListItem(coupon.Number + " " + coupon.OfferName + " " + coupon.CouponCondition, coupon.Id.ToString((IFormatProvider) CultureInfo.InvariantCulture)), false);
    }

    public FrmDiscountMobileAskCoupons()
    {
      this.InitializeComponent();
      this.SelectedCoupons.Clear();
    }

    private void OkButtonClick(object sender, EventArgs e)
    {
      CheckedListBox.CheckedItemCollection checkedItems = this.checkedBoxCoupons.CheckedItems;
      this.SelectedCoupons.Clear();
      foreach (ListItem listItem in checkedItems)
      {
        long result;
        long.TryParse(listItem.Value, out result);
        this.SelectedCoupons.Add(result);
      }
      this.Close();
    }

    private void CancelButtonClick(object sender, EventArgs e)
    {
      this.SelectedCoupons.Clear();
      this.Close();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.okButton = new System.Windows.Forms.Button();
      this.cancelButton = new System.Windows.Forms.Button();
      this.checkedBoxCoupons = new CheckedListBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      this.okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.okButton.Location = new Point(711, 398);
      this.okButton.Name = "okButton";
      this.okButton.Size = new Size(75, 23);
      this.okButton.TabIndex = 0;
      this.okButton.Text = "ОК";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new EventHandler(this.OkButtonClick);
      this.cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.cancelButton.Location = new Point(609, 398);
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new Size(75, 23);
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "Отмена";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new EventHandler(this.CancelButtonClick);
      this.checkedBoxCoupons.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.checkedBoxCoupons.FormattingEnabled = true;
      this.checkedBoxCoupons.HorizontalScrollbar = true;
      this.checkedBoxCoupons.Location = new Point(16, 36);
      this.checkedBoxCoupons.Name = "checkedBoxCoupons";
      this.checkedBoxCoupons.ScrollAlwaysVisible = true;
      this.checkedBoxCoupons.Size = new Size(801, 349);
      this.checkedBoxCoupons.TabIndex = 2;
      this.checkedBoxCoupons.ThreeDCheckBoxes = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new Size(115, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Использовать купон:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(837, 434);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.checkedBoxCoupons);
      this.Controls.Add((Control) this.cancelButton);
      this.Controls.Add((Control) this.okButton);
      this.Name = nameof (FrmDiscountMobileAskCoupons);
      this.Text = "Найдены цифровые купоны";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
