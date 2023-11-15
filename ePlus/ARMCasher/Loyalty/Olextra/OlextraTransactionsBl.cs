// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.Olextra.OlextraTransactionsBl
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.ARMBusinessLogic;
using ePlus.CommonEx;
using ePlus.MetaData.Server;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ePlus.ARMCasher.Loyalty.Olextra
{
  public class OlextraTransactionsBl
  {
    private SqlLoaderEx<CHEQUE_ITEM_TRANSACTION> loader = new SqlLoaderEx<CHEQUE_ITEM_TRANSACTION>();
    private DataService_BL dataService = new DataService_BL(MultiServerBL.ClientConnectionString);

    public void SaveEx(IEnumerable<CHEQUE_ITEM_TRANSACTION> transactions)
    {
      string tableName = "#CHEQUE_ITEM_TRANSACTION_OLEXTRA";
      using (SqlConnection connection = new SqlConnection(MultiServerBL.ClientConnectionString))
      {
        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
        {
          using (DataTable dataTable = this.loader.CreateDataTable(transactions))
          {
            using (SqlCommand command1 = connection.CreateCommand())
            {
              using (SqlCommand command2 = connection.CreateCommand())
              {
                command1.CommandText = this.loader.GenerateCreateTableScript(tableName);
                connection.Open();
                command1.ExecuteNonQuery();
                sqlBulkCopy.DestinationTableName = tableName;
                sqlBulkCopy.WriteToServer(dataTable);
                command2.CommandText = "EXEC USP_CHEQUE_ITEM_TRANSACTION_OLEXTRA_SAVE";
                command2.ExecuteNonQuery();
              }
            }
          }
        }
      }
    }

    public IEnumerable<CHEQUE_ITEM_TRANSACTION> GetListUnconfirmed() => (IEnumerable<CHEQUE_ITEM_TRANSACTION>) (this.loader.GetList(this.dataService.Execute(string.Format("EXEC USP_CHEQUE_ITEM_TRANSACTION_OLEXTRA_LOAD")).Tables[0]) ?? new List<CHEQUE_ITEM_TRANSACTION>());
  }
}
