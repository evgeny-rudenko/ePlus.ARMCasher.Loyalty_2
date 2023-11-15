// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Mindbox.MindboxLoyaltyFactory
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMCasher.Loyalty.Forms;
using System;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.Mindbox
{
  internal class MindboxLoyaltyFactory
  {
    private IWin32Window parentWindow;

    public LoyaltyCard CreateLoyaltyCard() => throw new NotImplementedException();

    private CardReader GetCardReader(IWin32Window parent)
    {
      this.parentWindow = parent;
      return new CardReader(this.ReadLoyaltyMindboxCard);
    }

    private bool ReadLoyaltyMindboxCard(out CustomerCardInfo customerInfo)
    {
      customerInfo = (CustomerCardInfo) null;
      FrmScanBarcodeEx frmScanBarcodeEx1 = new FrmScanBarcodeEx(true);
      frmScanBarcodeEx1.Text = "Поиск клиента в программе лояльности";
      using (FrmScanBarcodeEx frmScanBarcodeEx2 = frmScanBarcodeEx1)
      {
        if (frmScanBarcodeEx2.ShowDialog(this.parentWindow) == DialogResult.OK)
        {
          customerInfo = new CustomerCardInfo()
          {
            ClientId = frmScanBarcodeEx2.Barcode,
            Last4Digit = (string) null,
            Promocode = frmScanBarcodeEx2.Promocode
          };
          return true;
        }
      }
      return false;
    }
  }
}
