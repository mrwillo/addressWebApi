using System;
using System.Configuration;
using System.Data.SqlClient;

namespace AddressWebAPI.Providers
{
  public class DBProvider
  {
    public static SqlConnection DatabaseConnection
    {
      get
      {
        SqlConnection dbConnection = null;
        try
        {
          dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["AddressDBConnection"].ToString());
          dbConnection.Open();
        }
        catch (Exception ex)
        {
          Console.WriteLine("Cannot open connection " + ex.Message);
        }
        return dbConnection;
      }
    }
  }
}