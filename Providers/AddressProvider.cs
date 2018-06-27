using AddressWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace AddressWebAPI.Providers
{
  public class AddressProvider
  {
    public static IList<ContactModel> searchContact(int tagId, string keyword, out string processMessage)
    {
      processMessage = String.Empty;
      var listContact = new List<ContactModel>();
      try
      {
        using (var connection = DBProvider.DatabaseConnection)
        {
          var command = new SqlCommand("[address].[select_contact]", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@Keyword", keyword);
          command.Parameters.AddWithValue("@TagId", tagId);

          var reader = command.ExecuteReader();
          while (reader.Read())
          {
            listContact.Add(new ContactModel()
            {
              id = Convert.ToInt32(reader["ID"]),
              name = Convert.ToString(reader["name"]),
              email = Convert.ToString(reader["email"]),
              phone = Convert.ToString(reader["phone"]),
              linkedIn = Convert.ToString(reader["linkedIn"]),
              company = Convert.ToString(reader["company"]),
              tags = Convert.ToString(reader["tags"])
            });
          }
        }
      }
      catch (Exception exp)
      {
        processMessage = exp.Message;
        return null;
      }

      return listContact;
    }

    public static ContactModel getContact(int contactId, out string processMessage)
    {
      processMessage = String.Empty;
      ContactModel contact = null;

      try
      {
        using (var connection = DBProvider.DatabaseConnection)
        {
          var command = new SqlCommand("[address].[get_contact]", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@ContactId", contactId);
          var reader = command.ExecuteReader();
          while (reader.Read())
          {
            contact = new ContactModel()
            {
              id = Convert.ToInt32(reader["ID"]),
              name = Convert.ToString(reader["name"]),
              email = Convert.ToString(reader["email"]),
              phone = Convert.ToString(reader["phone"]),
              linkedIn = Convert.ToString(reader["linkedIn"]),
              company = Convert.ToString(reader["company"]),
              skype = Convert.ToString(reader["skype"])
            };
          }
        }
      }
      catch (Exception exp)
      {
        processMessage = exp.Message;
        return null;
      }

      return contact;
    }

    public static IList<ContactTagModel> getTags(out string processMessage)
    {
      processMessage = String.Empty;
      var listTag = new List<ContactTagModel>();
      try
      {
        using (var connection = DBProvider.DatabaseConnection)
        {
          var command = new SqlCommand("[address].[select_tag]", connection);
          command.CommandType = CommandType.StoredProcedure;
          var reader = command.ExecuteReader();
          while (reader.Read())
          {
            listTag.Add(new ContactTagModel()
            {
              id = Convert.ToInt32(reader["ID"]),
              name = Convert.ToString(reader["name"]),
            });
          }
        }
      }
      catch (Exception exp)
      {
        processMessage = exp.Message;
        return null;
      }

      return listTag;
    }


    public static ContactTagModel mergeTag(ContactTagModel tagModel, out int idOut, out string processMessage)
    {
      processMessage = String.Empty;
      idOut = 0;
      try
      {
        using (var connection = DBProvider.DatabaseConnection)
        {
          var command = new SqlCommand("[address].[merge_tag]", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@TagName", tagModel.name);
          command.Parameters.Add(new SqlParameter
          {
            ParameterName = "@IDOut",
            DbType = DbType.Int32,
            Direction = ParameterDirection.Output
          });
          command.Parameters.Add(new SqlParameter
          {
            ParameterName = "@ProcessMessage",
            DbType = DbType.String,
            Size = 200,
            Direction = ParameterDirection.Output
          });
          command.ExecuteNonQuery();

          idOut = int.Parse(command.Parameters["@IDOut"].Value.ToString());
          processMessage = command.Parameters["@ProcessMessage"].Value.ToString();
          tagModel.id = idOut;
        }
      }
      catch (Exception exp)
      {
        processMessage = exp.Message;
        return null;
      }
      return tagModel;
    }

    public static void deleteTag(int id, out int idOut, out string processMessage)
    {
      idOut = 0;
      processMessage = String.Empty;
      try
      {
        using (var connection = DBProvider.DatabaseConnection)
        {
          var command = new SqlCommand("[address].[delete_tag]", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@TagId", id);

          command.Parameters.Add(new SqlParameter
          {
            ParameterName = "@IDOut",
            DbType = DbType.Int32,
            Direction = ParameterDirection.Output
          });
          command.Parameters.Add(new SqlParameter
          {
            ParameterName = "@ProcessMessage",
            DbType = DbType.String,
            Size = 200,
            Direction = ParameterDirection.Output
          });
          command.ExecuteNonQuery();

          idOut = int.Parse(command.Parameters["@IDOut"].Value.ToString());
          processMessage = command.Parameters["@ProcessMessage"].Value.ToString();
        }
      }
      catch (Exception exp)
      {
        processMessage = exp.Message;
      }
    }


    public static void changeTagAssign(int contactId, int tagId, out int idOut, out string processMessage)
    {
      idOut = 0;
      processMessage = String.Empty;
      try
      {
        using (var connection = DBProvider.DatabaseConnection)
        {
          var command = new SqlCommand("[address].[change_tag_assign]", connection);
          command.CommandType = CommandType.StoredProcedure;
          command.Parameters.AddWithValue("@TagId", tagId);
          command.Parameters.AddWithValue("@ContactId", contactId);

          command.Parameters.Add(new SqlParameter
          {
            ParameterName = "@IDOut",
            DbType = DbType.Int32,
            Direction = ParameterDirection.Output
          });
          command.Parameters.Add(new SqlParameter
          {
            ParameterName = "@ProcessMessage",
            DbType = DbType.String,
            Size = 200,
            Direction = ParameterDirection.Output
          });
          command.ExecuteNonQuery();

          idOut = int.Parse(command.Parameters["@IDOut"].Value.ToString());
          processMessage = command.Parameters["@ProcessMessage"].Value.ToString();
        }
      }
      catch (Exception exp)
      {
        processMessage = exp.Message;
      }
    }

  }
}