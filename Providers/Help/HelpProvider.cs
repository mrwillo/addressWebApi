using CESJapanDataServices.Models.CustomerCare;
using CESJapanDataServices.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CESJapanDataServices.Providers.Help
{
  public class HelpProvider
  {
    internal static IList<BlueDocument> BlueDocumentSelect(int userID, int documentCategoryID, string documentName)
    {
      List<BlueDocument> ls = new List<BlueDocument>();
      DataTable dt = new DataTable();

      string host = string.Empty;
      string fileInS3 = string.Empty;

      //30 min
      long expirationInSeconds = long.Parse(ConfigurationManager.AppSettings["AWSTemperorayUrlExpirationInSeconds"]);
      expirationInSeconds = (long)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds + expirationInSeconds;
      string awsSecretKey = ConfigurationManager.AppSettings["AWSSecretKey"];
      string awsAccessID = ConfigurationManager.AppSettings["AWSAccessID"];
      byte[] awsSecretKeyBytes = Encoding.ASCII.GetBytes(awsSecretKey);

      try
      {
        using (SqlConnection connection = DBProvider.DatabaseConnection)
        {
          SqlCommand cmd = new SqlCommand("blu.usp_document_client_select", connection);
          cmd.CommandTimeout = 300;
          cmd.CommandType = CommandType.StoredProcedure;
          cmd.Parameters.AddWithValue("@ClientIntID", 0);
          cmd.Parameters.AddWithValue("@UserID", userID);
          cmd.Parameters.AddWithValue("@DocumentCategoryID", documentCategoryID);
          cmd.Parameters.AddWithValue("@Name", documentName.Trim());
          SqlDataAdapter adp = new SqlDataAdapter();
          adp.SelectCommand = cmd;
          adp.Fill(dt);

          foreach (DataRow dr in dt.Rows)
          {
            host = dr["UrlHost"].ToString();
            fileInS3 = dr["UrlPath"].ToString();

            string[] stringToSignParts = new string[] { "GET", "", "", expirationInSeconds.ToString(), fileInS3 };
            string stringToSign = String.Join("\n", stringToSignParts);
            byte[] hashValue = null;
            using (HMACSHA1 hmac = new HMACSHA1(awsSecretKeyBytes))
            {
              byte[] inStream = Encoding.ASCII.GetBytes(stringToSign);
              hashValue = hmac.ComputeHash(inStream);
            }

            string signatureString = System.Convert.ToBase64String(hashValue);
            //string signatureStringURLEncode = System.Net.WebUtility.HtmlEncode(signatureString);
            string signatureStringURLEncode = System.Web.HttpUtility.UrlEncode(signatureString);

            BlueDocument hm = new BlueDocument()
            {
              DocumentID = int.Parse(dr["DocumentID"].ToString()),
              DocumentName = dr["Name"].ToString(),
              DocumentCategoryID = int.Parse(dr["DocumentCategoryID"].ToString()),
              ClientIntID = int.Parse(dr["ClientIntID"].ToString()),
              CategoryName = dr["CategoryName"].ToString(),
              Note = dr["Note"].ToString(),
              SignedUrl = host + fileInS3 + "?AWSAccessKeyId=" + awsAccessID +
                                       "&Expires=" + expirationInSeconds +
                                        "&Signature=" + signatureStringURLEncode
            };
            ls.Add(hm);
          }
        }
      }
      catch (Exception ex)
      {
        CommonUtility.HandleUnexpectedException(ex);
      }

      return ls;
    }
  }
}