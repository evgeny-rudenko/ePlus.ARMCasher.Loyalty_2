// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasherNew.Controls.ARMPCXBarcodeTextBox
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.CommonEx.Controls;
using System.Windows.Forms;

namespace ePlus.ARMCasherNew.Controls
{
  public class ARMPCXBarcodeTextBox : ePlusBarcodeTextBox
  {
    public ARMPCXBarcodeTextBox() => this.AutoValidating = false;

    protected override void SetText(string text, string base64, byte[] bytes)
    {
      Form form = this.FindForm();
      if (form == null || !form.ContainsFocus)
        return;
      this.Text = text;
      ePlusBarcodeTextBox.LastBarcode_Set(this.Text);
      this.OnDataReceived(new ScannerDataReceivedEventArgs(this.Text, BarcodeBelongType.None, base64, bytes));
    }
  }
}
