using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using System.Configuration;
using System.Collections;

using CancerGov.Common.ErrorHandling;
using CancerGov.Text;

//11-23-2004 BryanP: SCR1002 Changed the database object fetching from adhoc to stored proc. 

namespace CancerGov.CDR.DataManager
{
    /// <summary>
    /// Summary description for ProtocolStates.
    /// </summary>
    [Obsolete("Move these methods into CTSearchManager/CTSearchQuery.")]
    public class ProtocolSearchFormData
    {
        public static DataTable GetProtocolInstitutions(string keyword, string lookupMethod)
        {
            keyword = keyword.Replace("'", "''");

            switch (lookupMethod.Trim().ToUpper())
            {
                case ProtocolSearchLookupMethods.AlphaNumIndex:
                    keyword = keyword + "%";
                    break;
                case ProtocolSearchLookupMethods.Keyword:
                    keyword = "%" + keyword + "%";
                    break;
            }

            DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = null;

            try
            {
                dbAdapter = new SqlDataAdapter("usp_GetProtocolInstitutions", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.Parameters.Add("@Keyword", keyword);

                dbAdapter.Fill(dbTable);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "ProtocolSearchFormData.GetProtocolInstitutions", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbTable;
        }

        public static DataTable GetProtocolLeadOrganizations(string keyword, string lookupMethod)
        {
            keyword = keyword.Replace("'", "''");

            switch (lookupMethod.Trim().ToUpper())
            {
                case ProtocolSearchLookupMethods.AlphaNumIndex:
                    keyword = keyword + "%";
                    break;
                case ProtocolSearchLookupMethods.Keyword:
                    keyword = "%" + keyword + "%";
                    break;
            }

            DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = null;

            try
            {
                dbAdapter = new SqlDataAdapter("usp_GetProtocolLeadOrganizations", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.Parameters.Add("@Keyword", keyword);

                dbAdapter.Fill(dbTable);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "ProtocolSearchFormData.GetProtocolLeadOrganizations", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbTable;
        }

        public static DataTable GetProtocolInvestigators(string keyword, string lookupMethod)
        {
            keyword = keyword.Replace("'", "''");
            keyword += "%";

            DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = null;

            try
            {
                dbAdapter = new SqlDataAdapter("usp_GetProtocolInvestigators", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.Parameters.Add("@Keyword", keyword);
                dbAdapter.SelectCommand.Parameters.Add("@LookupMethod", lookupMethod);
                dbAdapter.Fill(dbTable);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "ProtocolSearchFormData.GetProtocolInvestigators", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbTable;
        }

        public static DataTable GetProtocolInterventions(string keyword, string lookupMethod)
        {
            keyword = keyword.Replace("'", "''");
            keyword += "%";

            DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = null;

            try
            {
                dbAdapter = new SqlDataAdapter("usp_GetInterventions", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.Parameters.Add("@Match", keyword);
                //dbAdapter.SelectCommand.Parameters.Add("@LookupMethod", lookupMethod);
                dbAdapter.Fill(dbTable);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "ProtocolSearchFormData.GetProtocolInvestigators", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbTable;
        }

        public static DataTable GetProtocolDrugs(string keyword, string lookupMethod)
        {
            keyword = keyword.Replace("'", "''");

            switch (lookupMethod.Trim().ToUpper())
            {
                case ProtocolSearchLookupMethods.AlphaNumIndex:
                    keyword = keyword + "%";
                    break;
                case ProtocolSearchLookupMethods.Keyword:
                    keyword = "%" + keyword + "%";
                    break;
            }

            DataTable dbTable = new DataTable();
            SqlDataAdapter dbAdapter = null;

            try
            {
                dbAdapter = new SqlDataAdapter("usp_GetProtocolDrugs", ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
                dbAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dbAdapter.SelectCommand.Parameters.Add("@Keyword", keyword);

                dbAdapter.Fill(dbTable);
            }
            catch (SqlException sqlE)
            {
                CancerGovError.LogError("", "ProtocolSearchFormData.GetProtocolDrugs", ErrorType.DbUnavailable, sqlE);
            }
            finally
            {
                if (dbAdapter != null)
                {
                    dbAdapter.Dispose();
                }
            }

            return dbTable;
        }

    }
}
