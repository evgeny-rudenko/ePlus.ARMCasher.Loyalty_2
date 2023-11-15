// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.Forms.DialogRollback
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint.Forms
{
  internal class DialogRollback : Form
  {
    private IContainer components;
    internal TextBox ECROpIdOrig;
    internal Button Cancel_Button;
    internal Button OK_Button;
    internal TableLayoutPanel TableLayoutPanel1;
    internal TextBox BpSIdOrig;
    internal Label Label2;
    internal Label Label1;

    public DialogRollback() => this.InitializeComponent();

    private void OK_Button_Click(object sender, EventArgs e)
    {
      int num1 = Strings.Len(this.BpSIdOrig.Text);
      if (num1 != 20)
      {
        int num2 = (int) Interaction.MsgBox((object) "Длина BpSIdOrig должна быть равна 20. Проверьте правильность ввода BpSIdOrig!");
        if (num1 >= 20)
          return;
        this.BpSIdOrig.Text = Strings.Mid("00000000000000000000", 1, 20 - num1) + this.BpSIdOrig.Text;
      }
      else if (Strings.Len(this.ECROpIdOrig.Text) == 0)
      {
        int num3 = (int) Interaction.MsgBox((object) "Введите номер оригинальной операции (ECROpIdOrig)!");
      }
      else
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void Cancel_Button_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
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
      this.ECROpIdOrig = new TextBox();
      this.Cancel_Button = new Button();
      this.OK_Button = new Button();
      this.TableLayoutPanel1 = new TableLayoutPanel();
      this.BpSIdOrig = new TextBox();
      this.Label2 = new Label();
      this.Label1 = new Label();
      this.TableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      this.ECROpIdOrig.Location = new Point(87, 31);
      this.ECROpIdOrig.MaxLength = 5;
      this.ECROpIdOrig.Name = "ECROpIdOrig";
      this.ECROpIdOrig.Size = new Size(131, 20);
      this.ECROpIdOrig.TabIndex = 8;
      this.Cancel_Button.Anchor = AnchorStyles.None;
      this.Cancel_Button.DialogResult = DialogResult.Cancel;
      this.Cancel_Button.Location = new Point(76, 3);
      this.Cancel_Button.Name = "Cancel_Button";
      this.Cancel_Button.Size = new Size(67, 22);
      this.Cancel_Button.TabIndex = 1;
      this.Cancel_Button.Text = "Cancel";
      this.Cancel_Button.Click += new EventHandler(this.Cancel_Button_Click);
      this.OK_Button.Anchor = AnchorStyles.None;
      this.OK_Button.Location = new Point(3, 3);
      this.OK_Button.Name = "OK_Button";
      this.OK_Button.Size = new Size(67, 22);
      this.OK_Button.TabIndex = 0;
      this.OK_Button.Text = "OK";
      this.OK_Button.Click += new EventHandler(this.OK_Button_Click);
      this.TableLayoutPanel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.TableLayoutPanel1.ColumnCount = 2;
      this.TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.TableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.TableLayoutPanel1.Controls.Add((Control) this.Cancel_Button, 1, 0);
      this.TableLayoutPanel1.Controls.Add((Control) this.OK_Button, 0, 0);
      this.TableLayoutPanel1.Location = new Point(85, 64);
      this.TableLayoutPanel1.Name = "TableLayoutPanel1";
      this.TableLayoutPanel1.RowCount = 1;
      this.TableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
      this.TableLayoutPanel1.Size = new Size(146, 28);
      this.TableLayoutPanel1.TabIndex = 7;
      this.BpSIdOrig.Location = new Point(87, 4);
      this.BpSIdOrig.MaxLength = 20;
      this.BpSIdOrig.Name = "BpSIdOrig";
      this.BpSIdOrig.Size = new Size(131, 20);
      this.BpSIdOrig.TabIndex = 11;
      this.Label2.AutoSize = true;
      this.Label2.Location = new Point(7, 34);
      this.Label2.Name = "Label2";
      this.Label2.Size = new Size(74, 13);
      this.Label2.TabIndex = 10;
      this.Label2.Text = "ECROpIdOrig:";
      this.Label1.AutoSize = true;
      this.Label1.Location = new Point(7, 7);
      this.Label1.Name = "Label1";
      this.Label1.Size = new Size(58, 13);
      this.Label1.TabIndex = 9;
      this.Label1.Text = "BpSIdOrig:";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(238, 96);
      this.Controls.Add((Control) this.ECROpIdOrig);
      this.Controls.Add((Control) this.TableLayoutPanel1);
      this.Controls.Add((Control) this.BpSIdOrig);
      this.Controls.Add((Control) this.Label2);
      this.Controls.Add((Control) this.Label1);
      this.Name = nameof (DialogRollback);
      this.Text = "Rollback";
      this.TableLayoutPanel1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
