// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.SailPlay.FormSailPlayUserRegister
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCasher.Loyalty.Properties;
using ePlus.ARMUtils;
using ePlus.Loyalty.SailPlay;
using ePlus.Loyalty.SailPlay.Wpf;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace ePlus.ARMCasher.Loyalty.SailPlay
{
  public class FormSailPlayUserRegister : Form
  {
    private MaskedTextProvider _maskedProvider = new MaskedTextProvider("+7 (###) ### ## ##");
    private IContainer components;
    private ElementHost elementHost;
    private ElementHost elementHostButtons;
    private OkCancelButtonsControl okCancelButtonsControl;
    private Label label1;
    private SailPlayUserRegControl sailPlayUserRegControl;

    public bool OnlyEmailEdit { get; set; }

    public FormSailPlayUserRegister()
    {
      this.InitializeComponent();
      this.FormBorderStyle = FormBorderStyle.None;
      this.Width = this.BackgroundImage.Width;
      this.Height = this.BackgroundImage.Height;
      this.RefreshImage();
      this.TransparencyKey = Color.FromArgb(0, 0, 0);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.BackColor = Color.Black;
      this.elementHost.BackColorTransparent = true;
      this.elementHostButtons.BackColorTransparent = true;
      this.okCancelButtonsControl.OkClick += new EventHandler(this.OkClick);
      this.okCancelButtonsControl.CancelClick += new EventHandler(this.CancelClick);
    }

    public void RefreshImage()
    {
      Bitmap bitmap = new Bitmap(this.BackgroundImage);
      for (int x = 0; x < bitmap.Width; ++x)
      {
        for (int y = 0; y < bitmap.Height; ++y)
        {
          Color pixel = bitmap.GetPixel(x, y);
          if (pixel.B != (byte) 0 && pixel.B < (byte) 229)
            bitmap.SetPixel(x, y, Color.Black);
        }
      }
      this.BackgroundImage = (Image) bitmap;
    }

    public UserInfoResult UserInfo
    {
      get => (UserInfoResult) this.sailPlayUserRegControl.DataContext;
      set => this.sailPlayUserRegControl.DataContext = (object) value;
    }

    public DialogResult ShowDialog(UserInfoResult userInfo)
    {
      this.UserInfo = userInfo;
      return this.ShowDialog();
    }

    protected override void OnMouseClick(MouseEventArgs e)
    {
      base.OnMouseClick(e);
      if (e.Button != MouseButtons.Left || e.X <= 530 || e.Y >= 60)
        return;
      this.Close();
    }

    private void OkClick(object sender, EventArgs e)
    {
      if (!this.IsValid())
        return;
      if (!this.OnlyEmailEdit)
      {
        string str = this._maskedProvider.ToString(false, false);
        this.UserInfo.Phone = (str.StartsWith("7") ? str : "7" + str).Replace("+", "");
      }
      this.DialogResult = DialogResult.OK;
    }

    private bool IsValid()
    {
      if (!this.OnlyEmailEdit)
      {
        StringBuilder stringBuilder = new StringBuilder();
        if (string.IsNullOrEmpty(this.UserInfo.FirstName))
          stringBuilder.AppendLine("Необходимо задать имя клиента");
        if (!this._maskedProvider.Set(this.UserInfo.Phone) || !this._maskedProvider.MaskCompleted)
          stringBuilder.AppendLine("Необходимо задать телефон клиента");
        if (stringBuilder.Length > 0)
        {
          int num = (int) System.Windows.Forms.MessageBox.Show(stringBuilder.ToString(), "Не указаны обязательные данные", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
          return false;
        }
      }
      if (string.IsNullOrEmpty(this.UserInfo.EMail) || UtilsArm.IsValidMailAddress(this.UserInfo.EMail))
        return true;
      int num1 = (int) System.Windows.Forms.MessageBox.Show("Задан неправильный формат электронной почты");
      return false;
    }

    private void CancelClick(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FormSailPlayUserRegister));
      this.elementHost = new ElementHost();
      this.sailPlayUserRegControl = new SailPlayUserRegControl();
      this.elementHostButtons = new ElementHost();
      this.okCancelButtonsControl = new OkCancelButtonsControl();
      this.label1 = new Label();
      this.SuspendLayout();
      this.elementHost.Location = new Point(66, 102);
      this.elementHost.Name = "elementHost";
      this.elementHost.Size = new Size(430, 410);
      this.elementHost.TabIndex = 0;
      this.elementHost.Text = "elementHost1";
      this.elementHost.Child = (UIElement) this.sailPlayUserRegControl;
      this.elementHostButtons.Location = new Point(125, 515);
      this.elementHostButtons.Name = "elementHostButtons";
      this.elementHostButtons.Size = new Size(324, 48);
      this.elementHostButtons.TabIndex = 1;
      this.elementHostButtons.Text = "elementHost1";
      this.elementHostButtons.Child = (UIElement) this.okCancelButtonsControl;
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.Transparent;
      this.label1.Font = new Font("Microsoft Sans Serif", 11f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte) 204);
      this.label1.ForeColor = Color.FromArgb(1, 1, 1);
      this.label1.Location = new Point(148, 566);
      this.label1.Name = "label1";
      this.label1.Size = new Size(273, 18);
      this.label1.TabIndex = 2;
      this.label1.Text = "* поля обязательные для заполнения";
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) Resources.UserRegForm;
      this.ClientSize = new Size(600, 618);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.elementHostButtons);
      this.Controls.Add((Control) this.elementHost);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximumSize = new Size(600, 618);
      this.MinimumSize = new Size(600, 618);
      this.Name = nameof (FormSailPlayUserRegister);
      this.Text = "Регистрация нового пользователя";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
