// Decompiled with JetBrains decompiler
// Type: ePlus.ARMCasher.Loyalty.GoldenMiddle.GoldenMiddle_Bl
// Assembly: ePlus.ARMCasher.Loyalty, Version=3.8.2.0, Culture=neutral, PublicKeyToken=40f8aa27bb7bfe46
// MVID: 1D8C7E7A-C2D0-4F1C-8268-75DE0CDC8368
// Assembly location: C:\temp\ARM\ePlus.ARMCasher.Loyalty.dll

using ePlus.CommonEx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace ePlus.ARMCasher.Loyalty.GoldenMiddle
{
  public class GoldenMiddle_Bl
  {
    public Dictionary<long, string> GetGoodsGroups(IEnumerable<long> goodsIds)
    {
      Dictionary<long, string> goodsGroups = new Dictionary<long, string>();
      XDocument xdocument = new XDocument(new object[1]
      {
        (object) new XElement((XName) "XML", (object) goodsIds.Select<long, XElement>((System.Func<long, XElement>) (g => new XElement((XName) "ID_GOODS", (object) g.ToString()))))
      });
      using (SqlConnection connection = new SqlConnection(MultiServerBL.ClientConnectionString))
      {
        SqlCommand sqlCommand1 = new SqlCommand("USP_GM_GET_GOODS_GROUPS", connection);
        sqlCommand1.CommandType = CommandType.StoredProcedure;
        using (SqlCommand sqlCommand2 = sqlCommand1)
        {
          sqlCommand2.Parameters.AddWithValue("XML_DATA", (object) xdocument.ToString());
          connection.Open();
          using (SqlDataReader sqlDataReader = sqlCommand2.ExecuteReader())
          {
            int ordinal1 = sqlDataReader.GetOrdinal("ID_GOODS");
            int ordinal2 = sqlDataReader.GetOrdinal("GROUP_NAME");
            while (sqlDataReader.Read())
            {
              long int64 = sqlDataReader.GetInt64(ordinal1);
              string str1 = sqlDataReader.GetString(ordinal2);
              int num1 = str1.IndexOf('[');
              int num2 = str1.IndexOf(']');
              string str2;
              try
              {
                str2 = str1.Substring(num1 + 1, num2 - num1 - 1);
              }
              catch
              {
                throw new Exception(string.Format("Не удалось получить категорию товара из группы\"{0}\"", (object) str1));
              }
              goodsGroups.Add(int64, str2);
            }
          }
        }
      }
      return goodsGroups;
    }
  }
}
