// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmBallance
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.Loyalty.Cotrols;
using ePlus.ARMCommon;
using ePlus.ARMCommon.Controls;
using ePlus.Loyalty;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmBallance : Form, IBallanceView, IBaseView
  {
    private IContainer components;
    private ucBallance ucBallance1;
    private ARMButton armButton1;

    public FrmBallance() => this.InitializeComponent();

    public void Bind(LoyaltyCardInfo obj, string caption)
    {
      this.ucBallance1.Bind(obj);
      this.Text = caption;
    }

    private void FrmBallance_Shown(object sender, EventArgs e) => this.armButton1.Focus();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.armButton1 = new ARMButton();
      this.ucBallance1 = new ucBallance();
      this.SuspendLayout();
      this.armButton1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.armButton1.DialogResult = DialogResult.OK;
      this.armButton1.Font = new Font("Arial", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.armButton1.Location = new Point(131, 141);
      this.armButton1.Name = "armButton1";
      this.armButton1.Size = new Size(105, 28);
      this.armButton1.TabIndex = 1;
      this.armButton1.Text = "ОК";
      this.armButton1.UseVisualStyleBackColor = true;
      this.ucBallance1.AutoSize = true;
      this.ucBallance1.Dock = DockStyle.Top;
      this.ucBallance1.Location = new Point(0, 0);
      this.ucBallance1.Name = "ucBallance1";
      this.ucBallance1.Size = new Size(374, 90);
      this.ucBallance1.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new Size(374, 181);
      this.Controls.Add((Control) this.armButton1);
      this.Controls.Add((Control) this.ucBallance1);
      this.MaximizeBox = false;
      this.MaximumSize = new Size(390, 220);
      this.MinimizeBox = false;
      this.MinimumSize = new Size(390, 189);
      this.Name = nameof (FrmBallance);
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "Баланс";
      this.Shown += new EventHandler(this.FrmBallance_Shown);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    DialogResult IBaseView.ShowDialog() => this.ShowDialog();

    void IBaseView.Close() => this.Close();
  }
}
