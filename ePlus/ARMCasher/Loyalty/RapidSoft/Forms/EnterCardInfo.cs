// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.RapidSoft.Forms.EnterCardInfo
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.Database;
using ePlus.ARMCasher.Loyalty.Forms;
using ePlus.ARMUtils;
using ePlus.Loyalty;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.RapidSoft.Forms
{
  internal class EnterCardInfo : Form
  {
    private RapidSoftLoyaltyProgram _rapidSoft;
    private CHEQUE _cheque;
    public string PosId;
    public string TerminalId;
    private Decimal _chequeSumm;
    private Decimal _maxDiscountSum;
    public Guid ChequeNumber;
    private Decimal _pointsBalance;
    private Decimal _moneyBalance;
    private Decimal _discountSum;
    private CardStatus _cardStatus;
    private Guid _originalRequestId;
    private string _clientId;
    private IContainer components;
    private Button InfoButton;
    public TextBox numberTb;
    public Label Label47;
    public Label label1;
    private TextBox balancePointTb;
    private TextBox balanceMoneyTb;
    public Label label2;
    private TextBox statusTb;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;
    private Label label3;
    private TextBox chequeSumTb;
    private GroupBox groupBox1;
    private GroupBox groupBox2;
    private StatusStrip clientMessageStrip;
    private ToolStripStatusLabel clientMessageLabel;
    private TextBox minCashSumTb;
    private TextBox maxDiscountTb;
    private Label label5;
    private Label label4;
    private Label label6;
    private Button getDiscountButton;
    private TextBox discountSumTb;
    private Button rollbackDiscountButton;
    private TextBox last8charsTb;

    public Decimal ChequeSumm
    {
      get => this._chequeSumm;
      set
      {
        this._chequeSumm = value;
        this.chequeSumTb.Text = this._chequeSumm.ToString("#,#0.00#");
      }
    }

    public Decimal DiscountSum => this._discountSum;

    public Decimal SumNotRapidDiscount { get; set; }

    public Decimal SumWithDiscount => this._chequeSumm - this.SumNotRapidDiscount;

    public EnterCardInfo(RapidSoftLoyaltyProgram rapidSoftProgram, CHEQUE cheque)
    {
      this.InitializeComponent();
      this._rapidSoft = rapidSoftProgram;
      this._cheque = cheque;
    }

    private string CardStatusText()
    {
      switch (this._cardStatus)
      {
        case CardStatus.NotFound:
          return "Карта не найдена";
        case CardStatus.Active:
          return "Карта активна";
        case CardStatus.Limited:
          return "Карта ограничена";
        case CardStatus.Locked:
          return "Карта заблокирована";
        case CardStatus.NotActivated:
          return "Карта не активирована";
        case CardStatus.Expired:
          return "Карта просрочена";
        default:
          return string.Empty;
      }
    }

    private void ClearForm()
    {
      this._originalRequestId = Guid.Empty;
      this._discountSum = 0M;
      this._pointsBalance = 0M;
      this._moneyBalance = 0M;
      this.clientMessageLabel.Text = string.Empty;
      this.minCashSumTb.Text = string.Empty;
      this.maxDiscountTb.Text = string.Empty;
      this.discountSumTb.Text = "0.00";
    }

    private void OnInfoClick()
    {
            /*if (string.IsNullOrEmpty(this.numberTb.Text))
              return;
            this.ClientId = this.numberTb.Text;
            if (this.ClientId.Length > 8)
              this.last8charsTb.Text = this.ClientId.Substring(this.ClientId.Length - 8);
            if (this.CheckExistingDiscount())
              return;
            this.discountSumTb.ReadOnly = false;
            this.rollbackDiscountButton.Enabled = false;
            LoyaltyCardInfo? loyaltyInfo = new LoyaltyCardInfo?();
            using (FrmWaiting frmWaiting = new FrmWaiting())
            {
              frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) =>
              {
                try
                {
                  loyaltyInfo = new LoyaltyCardInfo?(this._rapidSoft.GetLoyaltyCardInfo(false));
                }
                catch (Exception ex)
                {
                  this.Invoke((Delegate) (() =>
                  {
                    int num = (int) UtilsArm.ShowMessageErrorOK(ex.Message);
                    this.ClearForm();
                    this.statusLabel.Text = "Ошибка получения баланса.";
                  }));
                }
              });
              int num1 = (int) frmWaiting.ShowDialog();
            }*/
            if (string.IsNullOrEmpty(this.numberTb.Text))
            {
                return;
            }
            this.ClientId = this.numberTb.Text;
            if (this.ClientId.Length > 8)
            {
                this.last8charsTb.Text = this.ClientId.Substring(this.ClientId.Length - 8);
            }
            if (this.CheckExistingDiscount())
            {
                return;
            }
            this.discountSumTb.ReadOnly = false;
            this.rollbackDiscountButton.Enabled = false;
            LoyaltyCardInfo? nullable = null;
            using (FrmWaiting frmWaiting = new FrmWaiting())
            {
                frmWaiting.BkWorker.DoWork += new DoWorkEventHandler((object param0, DoWorkEventArgs param1) => {
                    try
                    {
                        nullable = new LoyaltyCardInfo?(this._rapidSoft.GetLoyaltyCardInfo(false));
                    }
                    catch (Exception exception1)
                    {
                        Exception exception = exception1;
                        base.Invoke(new Action(() => {
                            UtilsArm.ShowMessageErrorOK(exception.Message);
                            this.ClearForm();
                            this.statusLabel.Text = "Ошибка получения баланса.";
                        }));
                    }
                });
                frmWaiting.ShowDialog();
            }

    }

        private bool CheckExistingDiscount()
    {
      this.ClientId = this.numberTb.Text;
      this._originalRequestId = Guid.Empty;
      RapidCheque rapidCheque = new RapidChequeDatabase().Load(this.ClientId, this.ChequeNumber.ToString());
      if (rapidCheque == null)
        return false;
      this.rollbackDiscountButton.Enabled = true;
      this.getDiscountButton.Enabled = false;
      this._originalRequestId = rapidCheque.RequestId;
      this.discountSumTb.Text = rapidCheque.Summ.ToString("#,#0.00#");
      this.discountSumTb.ReadOnly = true;
      return true;
    }

    private void InfoButton_Click(object sender, EventArgs e) => this.OnInfoClick();

    private void numberTb_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      this.OnInfoClick();
    }

    private void discountSumTb_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        e.Handled = true;
      if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
      {
        e.Handled = true;
      }
      else
      {
        if (e.KeyChar != '\r')
          return;
        e.Handled = true;
        this.CloseApplayAndApplayDiscount();
      }
    }

    private void getDiscountButton_Click(object sender, EventArgs e) => this.CloseApplayAndApplayDiscount();

    private void CloseApplayAndApplayDiscount()
    {
      Decimal result = 0M;
      this.discountSumTb.Text = this.discountSumTb.Text.Replace(".", ",");
      Decimal.TryParse(this.discountSumTb.Text, out result);
      if (result > 0M && result > this._maxDiscountSum)
        result = this._maxDiscountSum;
      if (result <= 0M)
        return;
      this.ApplyDiscount(result);
    }

    private void ApplyDiscount(Decimal discountSum)
    {
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) =>
        {
          try
          {
            this._rapidSoft.Charge(this._cheque, discountSum);
          }
          catch (Exception ex)
          {
            this.statusLabel.Text = "Ошибка получения скидки.";
            int num = (int) UtilsArm.ShowMessageErrorOK(ex.Message);
          }
        });
        int num1 = (int) frmWaiting.ShowDialog();
      }
    }

    private void rollbackDiscountButton_Click(object sender, EventArgs e) => this.OnRollbackDiscount();

    private void OnRollbackDiscount()
    {
      using (FrmWaiting frmWaiting = new FrmWaiting())
      {
        frmWaiting.BkWorker.DoWork += (DoWorkEventHandler) ((param0, param1) =>
        {
          try
          {
            this._rapidSoft.Rollback(out string _);
          }
          catch (Exception ex)
          {
            int num = (int) UtilsArm.ShowMessageErrorOK(ex.Message);
          }
        });
        int num1 = (int) frmWaiting.ShowDialog();
      }
    }

    public string ClientId
    {
      get => this._clientId;
      set
      {
        this._clientId = value;
        if (string.IsNullOrEmpty(this._clientId))
          return;
        if (this.numberTb.Text != this._clientId)
          this.numberTb.Text = this._clientId;
        if (this._clientId.Length <= 8)
          return;
        this.last8charsTb.Text = this.ClientId.Substring(this.ClientId.Length - 8);
      }
    }

    public Guid RequestId { get; set; }

    private void EnterCardInfo_Load(object sender, EventArgs e)
    {
    }

    internal string RapidSoftName { get; set; }

    private void numberTb_TextChanged(object sender, EventArgs e) => this.OnInfoClick();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.InfoButton = new Button();
      this.numberTb = new TextBox();
      this.Label47 = new Label();
      this.label1 = new Label();
      this.balancePointTb = new TextBox();
      this.balanceMoneyTb = new TextBox();
      this.label2 = new Label();
      this.statusTb = new TextBox();
      this.statusStrip = new StatusStrip();
      this.statusLabel = new ToolStripStatusLabel();
      this.label3 = new Label();
      this.chequeSumTb = new TextBox();
      this.groupBox1 = new GroupBox();
      this.rollbackDiscountButton = new Button();
      this.discountSumTb = new TextBox();
      this.label6 = new Label();
      this.getDiscountButton = new Button();
      this.minCashSumTb = new TextBox();
      this.maxDiscountTb = new TextBox();
      this.label5 = new Label();
      this.label4 = new Label();
      this.groupBox2 = new GroupBox();
      this.last8charsTb = new TextBox();
      this.clientMessageStrip = new StatusStrip();
      this.clientMessageLabel = new ToolStripStatusLabel();
      this.statusStrip.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.clientMessageStrip.SuspendLayout();
      this.SuspendLayout();
      this.InfoButton.Location = new Point(233, 120);
      this.InfoButton.Name = "InfoButton";
      this.InfoButton.Size = new Size(148, 23);
      this.InfoButton.TabIndex = 9;
      this.InfoButton.Text = "Обновить баланс";
      this.InfoButton.UseVisualStyleBackColor = true;
      this.InfoButton.Click += new EventHandler(this.InfoButton_Click);
      this.numberTb.AcceptsReturn = true;
      this.numberTb.BackColor = SystemColors.Window;
      this.numberTb.Cursor = Cursors.IBeam;
      this.numberTb.ForeColor = SystemColors.WindowText;
      this.numberTb.Location = new Point(112, 23);
      this.numberTb.MaxLength = 0;
      this.numberTb.Name = "numberTb";
      this.numberTb.RightToLeft = RightToLeft.No;
      this.numberTb.Size = new Size(203, 20);
      this.numberTb.TabIndex = 4;
      this.numberTb.UseSystemPasswordChar = true;
      this.numberTb.TextChanged += new EventHandler(this.numberTb_TextChanged);
      this.numberTb.KeyDown += new KeyEventHandler(this.numberTb_KeyDown);
      this.Label47.Cursor = Cursors.Default;
      this.Label47.ForeColor = SystemColors.ControlText;
      this.Label47.Location = new Point(12, 26);
      this.Label47.Name = "Label47";
      this.Label47.RightToLeft = RightToLeft.No;
      this.Label47.Size = new Size(77, 20);
      this.Label47.TabIndex = 167;
      this.Label47.Text = "Номер:";
      this.label1.Cursor = Cursors.Default;
      this.label1.ForeColor = SystemColors.ControlText;
      this.label1.Location = new Point(12, 54);
      this.label1.Name = "label1";
      this.label1.RightToLeft = RightToLeft.No;
      this.label1.Size = new Size(86, 20);
      this.label1.TabIndex = 175;
      this.label1.Text = "Баланс:";
      this.balancePointTb.Location = new Point(112, 54);
      this.balancePointTb.Name = "balancePointTb";
      this.balancePointTb.ReadOnly = true;
      this.balancePointTb.Size = new Size(120, 20);
      this.balancePointTb.TabIndex = 6;
      this.balanceMoneyTb.Location = new Point(261, 54);
      this.balanceMoneyTb.Name = "balanceMoneyTb";
      this.balanceMoneyTb.ReadOnly = true;
      this.balanceMoneyTb.Size = new Size(120, 20);
      this.balanceMoneyTb.TabIndex = 7;
      this.label2.Cursor = Cursors.Default;
      this.label2.ForeColor = SystemColors.ControlText;
      this.label2.Location = new Point(12, 90);
      this.label2.Name = "label2";
      this.label2.RightToLeft = RightToLeft.No;
      this.label2.Size = new Size(86, 20);
      this.label2.TabIndex = 181;
      this.label2.Text = "Статус:";
      this.statusTb.Location = new Point(112, 87);
      this.statusTb.Name = "statusTb";
      this.statusTb.ReadOnly = true;
      this.statusTb.Size = new Size(269, 20);
      this.statusTb.TabIndex = 8;
      this.statusStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.statusLabel
      });
      this.statusStrip.Location = new Point(0, 402);
      this.statusStrip.Name = "statusStrip";
      this.statusStrip.Size = new Size(420, 22);
      this.statusStrip.SizingGrip = false;
      this.statusStrip.TabIndex = 183;
      this.statusStrip.Text = "statusStrip";
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new Size(0, 17);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(12, 16);
      this.label3.Name = "label3";
      this.label3.Size = new Size(84, 13);
      this.label3.TabIndex = 185;
      this.label3.Text = "Сумма по чеку:";
      this.chequeSumTb.Location = new Point(188, 13);
      this.chequeSumTb.Name = "chequeSumTb";
      this.chequeSumTb.ReadOnly = true;
      this.chequeSumTb.Size = new Size(195, 20);
      this.chequeSumTb.TabIndex = 10;
      this.groupBox1.Controls.Add((Control) this.rollbackDiscountButton);
      this.groupBox1.Controls.Add((Control) this.discountSumTb);
      this.groupBox1.Controls.Add((Control) this.label6);
      this.groupBox1.Controls.Add((Control) this.getDiscountButton);
      this.groupBox1.Controls.Add((Control) this.minCashSumTb);
      this.groupBox1.Controls.Add((Control) this.maxDiscountTb);
      this.groupBox1.Controls.Add((Control) this.label5);
      this.groupBox1.Controls.Add((Control) this.label4);
      this.groupBox1.Controls.Add((Control) this.label3);
      this.groupBox1.Controls.Add((Control) this.chequeSumTb);
      this.groupBox1.Location = new Point(15, 187);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new Size(393, 184);
      this.groupBox1.TabIndex = 187;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Расчёт скидки";
      this.rollbackDiscountButton.Enabled = false;
      this.rollbackDiscountButton.Location = new Point(233, 145);
      this.rollbackDiscountButton.Name = "rollbackDiscountButton";
      this.rollbackDiscountButton.Size = new Size(148, 23);
      this.rollbackDiscountButton.TabIndex = 3;
      this.rollbackDiscountButton.Text = "Отменить скидку";
      this.rollbackDiscountButton.UseVisualStyleBackColor = true;
      this.rollbackDiscountButton.Click += new EventHandler(this.rollbackDiscountButton_Click);
      this.discountSumTb.Location = new Point(188, 103);
      this.discountSumTb.Name = "discountSumTb";
      this.discountSumTb.ReadOnly = true;
      this.discountSumTb.Size = new Size(193, 20);
      this.discountSumTb.TabIndex = 1;
      this.discountSumTb.KeyPress += new KeyPressEventHandler(this.discountSumTb_KeyPress);
      this.label6.AutoSize = true;
      this.label6.Location = new Point(12, 106);
      this.label6.Name = "label6";
      this.label6.Size = new Size(88, 13);
      this.label6.TabIndex = 192;
      this.label6.Text = "Размер скидки:";
      this.getDiscountButton.Enabled = false;
      this.getDiscountButton.Location = new Point(15, 145);
      this.getDiscountButton.Name = "getDiscountButton";
      this.getDiscountButton.Size = new Size(148, 23);
      this.getDiscountButton.TabIndex = 2;
      this.getDiscountButton.Text = "Получить скидку";
      this.getDiscountButton.UseVisualStyleBackColor = true;
      this.getDiscountButton.Click += new EventHandler(this.getDiscountButton_Click);
      this.minCashSumTb.Location = new Point(188, 71);
      this.minCashSumTb.Name = "minCashSumTb";
      this.minCashSumTb.ReadOnly = true;
      this.minCashSumTb.Size = new Size(195, 20);
      this.minCashSumTb.TabIndex = 12;
      this.maxDiscountTb.Location = new Point(188, 41);
      this.maxDiscountTb.Name = "maxDiscountTb";
      this.maxDiscountTb.ReadOnly = true;
      this.maxDiscountTb.Size = new Size(195, 20);
      this.maxDiscountTb.TabIndex = 11;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(12, 74);
      this.label5.Name = "label5";
      this.label5.Size = new Size(157, 13);
      this.label5.TabIndex = 188;
      this.label5.Text = "Минимальная сумма оплаты:";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 44);
      this.label4.Name = "label4";
      this.label4.Size = new Size(126, 13);
      this.label4.TabIndex = 187;
      this.label4.Text = "Максимальная скидка:";
      this.groupBox2.Controls.Add((Control) this.last8charsTb);
      this.groupBox2.Controls.Add((Control) this.Label47);
      this.groupBox2.Controls.Add((Control) this.numberTb);
      this.groupBox2.Controls.Add((Control) this.label1);
      this.groupBox2.Controls.Add((Control) this.balancePointTb);
      this.groupBox2.Controls.Add((Control) this.balanceMoneyTb);
      this.groupBox2.Controls.Add((Control) this.label2);
      this.groupBox2.Controls.Add((Control) this.statusTb);
      this.groupBox2.Controls.Add((Control) this.InfoButton);
      this.groupBox2.Location = new Point(15, 12);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new Size(393, 157);
      this.groupBox2.TabIndex = 188;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Информация по карте";
      this.last8charsTb.Location = new Point(321, 23);
      this.last8charsTb.Name = "last8charsTb";
      this.last8charsTb.ReadOnly = true;
      this.last8charsTb.Size = new Size(59, 20);
      this.last8charsTb.TabIndex = 5;
      this.clientMessageStrip.Items.AddRange(new ToolStripItem[1]
      {
        (ToolStripItem) this.clientMessageLabel
      });
      this.clientMessageStrip.Location = new Point(0, 380);
      this.clientMessageStrip.Name = "clientMessageStrip";
      this.clientMessageStrip.Size = new Size(420, 22);
      this.clientMessageStrip.SizingGrip = false;
      this.clientMessageStrip.TabIndex = 189;
      this.clientMessageStrip.Text = "statusStrip1";
      this.clientMessageLabel.Name = "clientMessageLabel";
      this.clientMessageLabel.Size = new Size(0, 17);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(420, 424);
      this.Controls.Add((Control) this.clientMessageStrip);
      this.Controls.Add((Control) this.groupBox2);
      this.Controls.Add((Control) this.groupBox1);
      this.Controls.Add((Control) this.statusStrip);
      this.MaximumSize = new Size(436, 462);
      this.MinimumSize = new Size(436, 462);
      this.Name = nameof (EnterCardInfo);
      this.Text = "Система лояльности RapidSoft";
      this.Load += new EventHandler(this.EnterCardInfo_Load);
      this.statusStrip.ResumeLayout(false);
      this.statusStrip.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.clientMessageStrip.ResumeLayout(false);
      this.clientMessageStrip.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
