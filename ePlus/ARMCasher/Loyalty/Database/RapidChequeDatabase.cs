// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Database.RapidChequeDatabase
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.CommonEx;
using ePlus.MetaData.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace ePlus.ARMCasher.Loyalty.Database
{
  internal class RapidChequeDatabase
  {
    private readonly SqlLoader<RapidCheque> _loader = new SqlLoader<RapidCheque>();

    public void DeleteDiscount(Guid chequeId)
    {
      SqlCommand cmd = new SqlCommand("\r\n                delete from DISCOUNT2_MAKE_ITEM\r\n                where ID_CHEQUE_ITEM_GLOBAL in \r\n                (\r\n\t                select ID_CHEQUE_ITEM_GLOBAL from CHEQUE_ITEM where ID_CHEQUE_GLOBAL = '{0}'\r\n                )\r\n                and ID_DISCOUNT2_CARD_GLOBAL = '{1}'\r\n            ");
      cmd.CommandType = CommandType.Text;
      MultiServerBL.RunSqlCommand(ref cmd, ServerType.Local);
    }

    public RapidCheque Load(string clientId, string chequeId)
    {
      SqlCommand cmd = new SqlCommand(string.Format("SELECT [RequestId], [ChequeId], [clientId], [Operation], [Summ], [Date] from [RAPID_CHEQUE] where [ChequeId] = '{0}' and [ClientId] = '{1}' order by [Date] desc ", (object) chequeId.ToUpper(), (object) clientId.ToUpper()));
      cmd.CommandType = CommandType.Text;
      List<RapidCheque> list = this._loader.GetList(MultiServerBL.RunSqlCommand(ref cmd).Tables[0]);
      return list.Count > 0 ? list[0] : (RapidCheque) null;
    }

    public RapidCheque Load(Guid chequeId)
    {
      SqlCommand cmd = new SqlCommand(string.Format("SELECT [RequestId], [ChequeId], [clientId], [Operation], [Summ], [Date] from [RAPID_CHEQUE] where [ChequeId] = '{0}' order by [Date] desc ", (object) chequeId.ToString().ToUpper()));
      cmd.CommandType = CommandType.Text;
      List<RapidCheque> list = this._loader.GetList(MultiServerBL.RunSqlCommand(ref cmd).Tables[0]);
      return list.Count > 0 ? list[0] : (RapidCheque) null;
    }

    public void Save(RapidCheque item)
    {
      SqlCommand sqlCommand = new SqlCommand(string.Format("INSERT INTO [RAPID_CHEQUE] ([ChequeId], [ClientId], [Operation], [Summ], [Date], [RequestId]) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", (object) item.ChequeId, (object) item.ClientId, (object) item.Operation, (object) item.Summ.ToString((IFormatProvider) new NumberFormatInfo()
      {
        NumberDecimalSeparator = "."
      }), (object) item.Date, (object) item.RequestId));
      sqlCommand.CommandType = CommandType.Text;
      SqlCommand cmd = sqlCommand;
      MultiServerBL.RunSqlCommand(ref cmd);
    }

    public void Update(RapidCheque item)
    {
      if (item.Operation == "ROLLBACK")
      {
        this.Delete(item);
      }
      else
      {
        SqlCommand cmd1 = new SqlCommand(string.Format("\r\n                select 1 from [RAPID_CHEQUE]\r\n                where [ClientId] = '{0}' and [ChequeId] = '{1}' and [RequestId] = '{2}' \r\n            ", (object) item.ClientId, (object) item.ChequeId, (object) item.RequestId));
        cmd1.CommandType = CommandType.Text;
        DataSet dataSet = MultiServerBL.RunSqlCommand(ref cmd1);
        string cmdText;
        if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
          cmdText = string.Format("UPDATE [RAPID_CHEQUE] set [Operation] = '{0}', [Date] = '{1}' where [ClientId] = '{2}' and [ChequeId] = '{3}' and [RequestId] = '{4}'", (object) item.Operation, (object) item.Date, (object) item.ClientId, (object) item.ChequeId, (object) item.RequestId);
        else
          cmdText = string.Format("\r\n                    insert into [RAPID_CHEQUE] ([Operation], [Date], [ClientId], [ChequeId], [RequestId])\r\n                    values ('{0}', '{1}', '{2}', '{3}', '{4}')\r\n                ", (object) item.Operation, (object) item.Date, (object) item.ClientId, (object) item.ChequeId, (object) item.RequestId);
        SqlCommand cmd2 = new SqlCommand(cmdText);
        MultiServerBL.RunSqlCommand(ref cmd2);
      }
    }

    private void Delete(RapidCheque item)
    {
      SqlCommand sqlCommand = new SqlCommand(string.Format("DELETE from [RAPID_CHEQUE] where [ClientId] = '{0}' and [ChequeId] = '{1}' and [RequestId] = '{2}'", (object) item.ClientId, (object) item.ChequeId, (object) item.RequestId));
      sqlCommand.CommandType = CommandType.Text;
      SqlCommand cmd = sqlCommand;
      MultiServerBL.RunSqlCommand(ref cmd);
    }
  }
}
