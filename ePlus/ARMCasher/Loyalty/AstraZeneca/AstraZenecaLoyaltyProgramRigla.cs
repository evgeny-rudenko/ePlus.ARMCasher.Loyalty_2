// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.AstraZeneca.AstraZenecaLoyaltyProgramRigla
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using Dapper;
using ePlus.ARMCasher.BusinessObjects;
using ePlus.CommonEx;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ePlus.ARMCasher.Loyalty.AstraZeneca
{
  internal class AstraZenecaLoyaltyProgramRigla : AstraZenecaLoyaltyProgram
  {
    private List<AllowedBarcode> barcodeCache;

    public AstraZenecaLoyaltyProgramRigla(string publicId, string posId)
      : base(publicId, posId)
    {
    }

    private void FillBarcodeCache()
    {
      this.barcodeCache = new List<AllowedBarcode>();
      using (SqlConnection cnn = new SqlConnection(MultiServerBL.ClientConnectionString))
      {
        string sql = " \r\n                    select \r\n\t                    ID_GOODS_GLOBAL,\r\n                        BARCODE\r\n                            FROM ALLOWED_BARCODE ab                            \r\n                        where DATE_DELETED IS NULL";
        List<AllowedBarcode> list = cnn.Query<AllowedBarcode>(sql).ToList<AllowedBarcode>();
        if (list == null)
          return;
        this.barcodeCache = list;
      }
    }

    protected override string GetBarcode(CHEQUE_ITEM item)
    {
      if (this.barcodeCache == null)
        this.FillBarcodeCache();
      AllowedBarcode allowedBarcode = this.barcodeCache.FirstOrDefault<AllowedBarcode>((Func<AllowedBarcode, bool>) (ab => ab.ID_GOODS_GLOBAL == item.idGoodsGlobal));
      return allowedBarcode == null ? (string) null : allowedBarcode.BARCODE;
    }
  }
}
