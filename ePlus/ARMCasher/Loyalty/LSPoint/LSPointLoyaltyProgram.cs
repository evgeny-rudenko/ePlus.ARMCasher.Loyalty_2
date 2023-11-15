// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.LSPoint.LSPointLoyaltyProgram
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.ARMBusinessLogic.RetailRecipe;
using ePlus.ARMCasher.BusinessLogic;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.ARMCasher.Loyalty.LSPoint.Forms;
using ePlus.CommonEx;
using ePlus.KKMWrapper;
using ePlus.LSPoint.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ePlus.ARMCasher.Loyalty.LSPoint
{
  internal class LSPointLoyaltyProgram
  {
    public IList<LSPointLoyaltyProgram.StockDetailInfo> NewItemsList = (IList<LSPointLoyaltyProgram.StockDetailInfo>) new List<LSPointLoyaltyProgram.StockDetailInfo>();
    private readonly LSPointSettings _lspointSettings;
    private readonly LSPointSettingsBl _lspointSettingsBl = new LSPointSettingsBl();
    private readonly STOCK_DETAIL_BL stockDetailBl = new STOCK_DETAIL_BL();
    private static Bel _belForm = new Bel();
    private string _cardNumber;
    private CHEQUE _cheque;
    public bool ChequePrinted;

    public Decimal IncomeBonus { get; private set; }

    public Decimal OutcomeBonus { get; private set; }

    public LSPointLoyaltyProgram()
    {
      if (!AppConfigurator.EnableLSPoint || MultiServerBL.OnlineState == OnlineState.Offline || MultiServerBL.OnlineState == OnlineState.Disconnected)
        return;
      this._lspointSettings = this._lspointSettingsBl.Load(ServerType.Server) ?? new LSPointSettings();
      if (!LSPointLoyaltyProgram._belForm.InitSuccess)
        return;
      LSPointLoyaltyProgram._belForm.Check_UseKLProtocol.Checked = true;
    }

    public Decimal PaidBonus => (Decimal) LSPointLoyaltyProgram._belForm.AlreadyPayBonus;

    public Decimal PaidCash => (Decimal) LSPointLoyaltyProgram._belForm.AlreadyPayCash;

    public Decimal PaidCard => (Decimal) LSPointLoyaltyProgram._belForm.AlreadyPayCard;

    public long BPRRN => LSPointLoyaltyProgram._belForm.BPRRN;

    public string BpSId => LSPointLoyaltyProgram._belForm.BpSId.Text;

    public string ECROpId => LSPointLoyaltyProgram._belForm.ECROpId.Text;

    public string CardNumber
    {
      get => this._cardNumber;
      set => this._cardNumber = value;
    }

    public bool EnterCardInfo(CHEQUE cheque)
    {
      this._cheque = cheque;
      using (EnterCardInfoForm enterCardInfoForm = new EnterCardInfoForm(this))
      {
        enterCardInfoForm.StartPosition = FormStartPosition.CenterParent;
        if (enterCardInfoForm.ShowDialog() == DialogResult.OK)
          return true;
      }
      return false;
    }

    public void Info()
    {
      while (LSPointLoyaltyProgram._belForm.Bpecr1.IsOperationRunning == 1)
        Thread.Sleep(100);
      LSPointLoyaltyProgram._belForm.Info();
      LSPointLoyaltyProgram.ProcessScreenPrinterMessages();
    }

    private static void ProcessScreenPrinterMessages()
    {
      if (!string.IsNullOrEmpty(Bel.Instance.ScreenText.Text))
      {
        int num = (int) MessageBox.Show(Bel.Instance.ScreenText.Text);
      }
      if (string.IsNullOrEmpty(Bel.Instance.PrnText.Text) || !AppConfigurator.KKMSettings.kkmEnable)
        return;
      KkmWrapper.Driver.PrintNonFiscalDoc(Bel.Instance.PrnText.Text);
    }

    public void AddGoods(GoodsInfo info)
    {
      if (!LSPointLoyaltyProgram._belForm.AddGoods(info))
        return;
      LSPointLoyaltyProgram._belForm.GoodsList.Add(info);
    }

    public bool PartialReturnPromo(CHEQUE baseCheque, CHEQUE cheque, PCX_CHEQUE pcxCheque)
    {
      this._cheque = baseCheque;
      LSPointLoyaltyProgram._belForm.MagTrack2Field.Text = pcxCheque.CLIENT_ID;
      Decimal num = 0M;
      if (this._cheque != null && this._cheque.CHEQUE_ITEMS != null && this._cheque.CHEQUE_ITEMS.Count > 0)
      {
        foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) this._cheque.CHEQUE_ITEMS)
        {
          GoodsInfo info = new GoodsInfo();
          info.BarCode = chequeItem.CODE;
          info.Flags = (short) 0;
          info.Name = chequeItem.GOODS_NAME;
          info.Price = chequeItem.LOT_PRICE_VAT;
          num += chequeItem.LOT_PRICE_VAT;
          info.Quantity = chequeItem.QUANTITY;
          this.AddGoods(info);
        }
      }
      LSPointLoyaltyProgram._belForm.BpRrnField.Text = pcxCheque.TRANSACTION_ID;
      LSPointLoyaltyProgram._belForm.AmountForCancel.Text = num.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      while (LSPointLoyaltyProgram._belForm.Bpecr1.IsOperationRunning == 1)
        Thread.Sleep(100);
      if (!LSPointLoyaltyProgram._belForm.IsPromoEnabled || !LSPointLoyaltyProgram._belForm.OnBpRrnCancel())
        return false;
      LSPointLoyaltyProgram.ProcessScreenPrinterMessages();
      if (cheque == null || cheque.CHEQUE_ITEMS.Count <= 0)
        return true;
      this._cheque = cheque;
      return this.Promo();
    }

    private bool ModifyCheque()
    {
      if (Bel.Instance.Bpecr1.goodCount <= (short) 0)
        return true;
      Bel.Instance.Bpecr1.GetFirstGood();
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < (int) Bel.Instance.Bpecr1.goodCount; ++index)
      {
        string goodCode = (string) Bel.Instance.Bpecr1.goodCode;
        bool flag = false;
        foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) this._cheque.CHEQUE_ITEMS)
        {
          if (chequeItem.CODE == goodCode)
          {
            Decimal num = (Decimal) Bel.Instance.Bpecr1.goodQuantity / 1000M;
            chequeItem.QUANTITY = (Decimal) (int) num;
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          if ((string) Bel.Instance.Bpecr1.goodCode == "100000")
          {
            Decimal num = (Decimal) Bel.Instance.Bpecr1.goodQuantity / 1000M;
            string goodName = (string) Bel.Instance.Bpecr1.goodName;
            stringBuilder.AppendFormat("1/1 {1}\n{0:0.00} x 1,00 = {0:0.00}\r\n", (object) num, (object) goodName);
          }
          else
          {
            IEnumerable<STOCK_DETAIL> source = this.stockDetailBl.List((string) null, 0M, (string) Bel.Instance.Bpecr1.goodCode, (string) null, (string) null, DateTime.MinValue, (RetailRecipeSelectLotInfo) null, true, (string) null, 0L);
            if (source.Any<STOCK_DETAIL>())
              this.NewItemsList.Add(new LSPointLoyaltyProgram.StockDetailInfo()
              {
                StockDetail = source.First<STOCK_DETAIL>(),
                Quantity = Bel.Instance.Bpecr1.goodQuantity
              });
          }
        }
        if (index < (int) Bel.Instance.Bpecr1.goodCount)
          Bel.Instance.Bpecr1.GetNextGood();
      }
      if (AppConfigurator.KKMSettings.kkmEnable)
        KkmWrapper.Driver.PrintNonFiscalDoc(stringBuilder.ToString());
      return true;
    }

    public bool Promo()
    {
      while (LSPointLoyaltyProgram._belForm.Bpecr1.IsOperationRunning == 1)
        Thread.Sleep(100);
      if (this._cheque != null && this._cheque.CHEQUE_ITEMS != null && this._cheque.CHEQUE_ITEMS.Count > 0)
      {
        foreach (CHEQUE_ITEM chequeItem in (List<CHEQUE_ITEM>) this._cheque.CHEQUE_ITEMS)
          this.AddGoods(new GoodsInfo()
          {
            BarCode = chequeItem.CODE,
            Flags = (short) 0,
            Name = chequeItem.GOODS_NAME,
            Price = chequeItem.LOT_PRICE_VAT,
            Quantity = chequeItem.QUANTITY
          });
      }
      while (LSPointLoyaltyProgram._belForm.Bpecr1.IsOperationRunning == 1)
        Thread.Sleep(100);
      if (!LSPointLoyaltyProgram._belForm.IsPromoEnabled || !LSPointLoyaltyProgram._belForm.Promo() || LSPointLoyaltyProgram._belForm.IsCancelled)
        return false;
      LSPointLoyaltyProgram.ProcessScreenPrinterMessages();
      PCX_CHEQUE_BL pcxChequeBl = new PCX_CHEQUE_BL();
      PCX_CHEQUE pcxCheque = new PCX_CHEQUE();
      pcxCheque.CLIENT_ID = LSPointLoyaltyProgram._belForm.MagTrack2Field.Text;
      pcxCheque.CLIENT_ID_TYPE = 10;
      pcxCheque.SCORE = 0M;
      pcxCheque.PARTNER_ID = string.Empty;
      pcxCheque.LOCATION = string.Empty;
      pcxCheque.TERMINAL = string.Empty;
      pcxCheque.TRANSACTION_ID = LSPointLoyaltyProgram._belForm.BPRRN.ToString((IFormatProvider) CultureInfo.InvariantCulture);
      pcxCheque.ID_CHEQUE_GLOBAL = this._cheque != null ? this._cheque.ID_CHEQUE_GLOBAL : Guid.Empty;
      pcxCheque.SUMM_SCORE = this.PaidBonus / 100M;
      pcxCheque.SUMM = (this.PaidCash + this.PaidCard + this.PaidBonus) / 100M;
      pcxCheque.SUMM_MONEY = this.PaidCash / 100M;
      pcxCheque.CARD_SCORE = this.PaidCard / 100M;
      pcxCheque.OPER_TYPE = this.PaidBonus == 0M ? "CHARGE" : "DEBIT";
      pcxCheque.STATUS = pcxOperationStatus.Online.ToString();
      pcxChequeBl.Save(pcxCheque);
      if (!this.ModifyCheque())
      {
        int num = (int) MessageBox.Show("Недостаточное количество товаров на складе для продажи в соответствии с условием акции LSPoint. Начисление бонусов будет отменено.");
        this.PartialReturnPromo(this._cheque, (CHEQUE) null, pcxCheque);
      }
      return true;
    }

    public bool PerformBprrnCancel(long BPRRN)
    {
      LSPointLoyaltyProgram._belForm.BpRrnField.Text = BPRRN.ToString();
      while (LSPointLoyaltyProgram._belForm.Bpecr1.IsOperationRunning == 1)
        Thread.Sleep(100);
      bool flag = LSPointLoyaltyProgram._belForm.OnBpRrnCancel();
      LSPointLoyaltyProgram.ProcessScreenPrinterMessages();
      return flag;
    }

    public bool PerformRollback(string BpSId, string ECROpId)
    {
      LSPointLoyaltyProgram._belForm.BpSId.Text = BpSId;
      LSPointLoyaltyProgram._belForm.ECROpId.Text = ECROpId;
      while (LSPointLoyaltyProgram._belForm.Bpecr1.IsOperationRunning == 1)
        Thread.Sleep(100);
      bool flag = LSPointLoyaltyProgram._belForm.OnPerformRollback(BpSId, ECROpId);
      LSPointLoyaltyProgram.ProcessScreenPrinterMessages();
      return flag;
    }

    public class StockDetailInfo
    {
      public STOCK_DETAIL StockDetail;
      public int Quantity;
    }
  }
}
