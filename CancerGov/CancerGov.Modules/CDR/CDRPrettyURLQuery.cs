using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NCI.Data;
using NCI.Logging;
using NCI.Text;

namespace CancerGov.Modules.CDR
{
    internal class CDRPrettyURLQuery
    {
        public static string GetProtocolByOldId(string oldId)
        {

            try
            {
                string cdrID = String.Empty;

                string connString = ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString;

                if (!string.IsNullOrEmpty(connString))
                {
                    using (SqlConnection conn = SqlHelper.CreateConnection(connString))
                    {
                        using (SqlDataReader reader =
                                    SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "dbo.usp_GetProtocolIdByOldID",
                                    new SqlParameter("@oldID", string.IsNullOrEmpty(oldId) ? null : oldId)))
                        {
                            while (reader.Read())
                            {
                                cdrID = (reader.IsDBNull(reader.GetOrdinal("ProtocolID")))
                               ? null : reader["ProtocolID"].ToString();
                            }
                        }
                    }
                }
                else
                    throw new Exception("CDRPrettyURLQuery:Configuration Missing:Connection string is null, update CDRDbConnectionString in the web config with connection string");

                return cdrID;
            }
            catch (Exception ex)
            {
                Logger.LogError("CDRPrettyURLQuery:GetProtocolByOldId", "Failed in DataManager", NCIErrorLevel.Error);
                throw ex;
            }
        }

        public static DataTable GetOldProtocolAndAlternateIDs()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter dbAdapter = null;

            try
            {
                dbAdapter = new SqlDataAdapter("usp_GetProtocolOldAndAlternateIDs", ConfigurationManager.AppSettings["CDRDbConnectionString"]);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.Fill(dt);
            }
            catch (SqlException sqlE)
            {
                //throw InvalidGuidException;
                Logger.LogError("CDRPrettyURLQuery:GetOldProtocolAndAlternateIDs", NCIErrorLevel.Error, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dt;
        }
    }
}
