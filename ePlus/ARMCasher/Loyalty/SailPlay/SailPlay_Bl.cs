// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.SailPlay.SailPlay_Bl
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.CommonEx;
using System.Data;
using System.Data.SqlClient;

namespace ePlus.ARMCasher.Loyalty.SailPlay
{
  public class SailPlay_Bl
  {
    public void SaveRegisterCardMyHealth(long idUser)
    {
      using (SqlConnection connection = new SqlConnection(MultiServerBL.ClientConnectionString))
      {
        SqlCommand sqlCommand1 = new SqlCommand("USP_REGISTRY_MZ_SAVE", connection);
        sqlCommand1.CommandType = CommandType.StoredProcedure;
        using (SqlCommand sqlCommand2 = sqlCommand1)
        {
          sqlCommand2.Parameters.AddWithValue("ID_USER_DATA", (object) idUser);
          connection.Open();
          sqlCommand2.ExecuteNonQuery();
        }
      }
    }
  }
}
