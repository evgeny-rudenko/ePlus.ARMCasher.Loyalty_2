// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Forms.FrmWaiting
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMCommon.Log;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Forms
{
  public class FrmWaiting : Form
  {
    private IContainer components;
    private Label label1;
    private System.Timers.Timer timer = new System.Timers.Timer();
    private int secondsCounter;
    private bool callbackFileCheck;
    private static int waitingTimeout = 1000;
    private BackgroundWorker bkWorker = new BackgroundWorker();
    private Exception exception;

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmWaiting));
      this.label1 = new Label();
      this.SuspendLayout();
      this.label1.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 204);
      this.label1.Location = new Point(12, 20);
      this.label1.Name = "label1";
      this.label1.Size = new Size(419, 23);
      this.label1.TabIndex = 1;
      this.label1.Text = "label1";
      this.label1.TextAlign = ContentAlignment.TopCenter;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(443, 66);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmWaitingPCX";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Ожидание";
      this.FormClosing += new FormClosingEventHandler(this.FrmScanIncomingFolder_FormClosing);
      this.Load += new EventHandler(this.FrmScanIncomingFolder_Load);
      this.ResumeLayout(false);
    }

    public Exception Exception => this.exception;

    public BackgroundWorker BkWorker => this.bkWorker;

    public int WaitingTimeout
    {
      get => FrmWaiting.waitingTimeout;
      set => FrmWaiting.waitingTimeout = value;
    }

    public FrmWaiting()
    {
      this.InitializeComponent();
      this.timer.Interval = 1000.0;
      this.timer.Elapsed += new ElapsedEventHandler(this.timer_Elapsed);
      this.HandleDestroyed += new EventHandler(this.FrmWaiting_HandleDestroyed);
    }

    private void FrmWaiting_HandleDestroyed(object sender, EventArgs e)
    {
      if (!this.timer.Enabled)
        return;
      this.timer.Stop();
    }

    private void timer_Elapsed(object sender, ElapsedEventArgs e)
    {
      if (this.secondsCounter > 0 && this.timer.Enabled && !this.IsDisposed && this.IsHandleCreated)
        this.Invoke((Delegate) new TextSetter(this.SetLabelText), (object) string.Format("До окончания времени ожидания осталось {0} секунд.", (object) --this.secondsCounter));
      if (this.callbackFileCheck)
        return;
      try
      {
        this.callbackFileCheck = true;
        if (this.secondsCounter > 0)
          return;
        this.timer.Stop();
      }
      catch (Exception ex)
      {
        ARMLogger.InfoException("Ошибка при работе таймера", ex);
      }
      finally
      {
        this.callbackFileCheck = false;
      }
    }

    private void SetLabelText(string text) => this.label1.Text = text;

    private void FrmScanIncomingFolder_Load(object sender, EventArgs e)
    {
      this.secondsCounter = FrmWaiting.waitingTimeout;
      this.bkWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bkWorker_RunWorkerCompleted);
      this.timer.Start();
      this.bkWorker.RunWorkerAsync();
      this.label1.Text = string.Format("До окончания времени ожидания осталось {0} секунд.", (object) this.secondsCounter);
    }

    private void bkWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) => this.StopTimeCounter(e.Error);

    private void FrmScanIncomingFolder_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult == DialogResult.Yes || this.DialogResult == DialogResult.No)
        return;
      e.Cancel = true;
    }

    public DialogResult ShowChildDialod(Form frm)
    {
      if (this.InvokeRequired)
        return (DialogResult) this.Invoke((Delegate) new FrmWaiting.FormArgVoidReturnDelegate(this.ShowChildDialod), (object) frm);
      this.timer.Stop();
      DialogResult dialogResult = frm.ShowDialog((IWin32Window) this);
      this.timer.Start();
      return dialogResult;
    }

    private void StopTimeCounter(Exception ex)
    {
      this.timer.Stop();
      if (ex != null)
      {
        this.exception = ex;
        this.DialogResult = DialogResult.No;
      }
      else
        this.DialogResult = DialogResult.Yes;
    }

    private delegate DialogResult FormArgVoidReturnDelegate(Form frm);
  }
}
