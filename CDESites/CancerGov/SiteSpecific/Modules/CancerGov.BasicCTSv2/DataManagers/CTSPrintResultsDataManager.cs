using System.Web;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using NCI.Util;
using NCI.Data;
using NCI.DataManager;
using Common.Logging;
using CancerGov.Common.ErrorHandling;
using NCI.Web.CDE.Application;

namespace CancerGov.ClinicalTrials.Basic.v2.DataManagers
{
    public static class CTSPrintResultsDataManager
    {
        //Getting the List of years/months from the DB
        public static Guid SavePrintResult(string content, string searchParams, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
            {
                Guid printResultGuid = Guid.Empty;

                SqlParameter[] parameters = {
                    new SqlParameter("@printid", SqlDbType.UniqueIdentifier),
                    new SqlParameter("@content", SqlDbType.NVarChar, content.Length),
                    new SqlParameter("@searchparams", SqlDbType.NVarChar, searchParams.Length),
                    new SqlParameter("@isLive", SqlDbType.Bit)
                };
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = content;
                parameters[2].Value = searchParams;
                parameters[3].Value = isLive;

                try
                {
                    SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "dbo.ct_insertPrintresultcache", parameters);
                    printResultGuid = (Guid)parameters[0].Value;
                }
                catch (SqlException ex)
                {
                     CancerGovError.LogError("Unable to save data. Search Params: " + searchParams + "isLive: " + isLive, 2, ex);
                }

                return printResultGuid;       
            }
        }

        /// <summary>
        /// Connects to the database , and executes the stored proc with the required parameter. The 
        /// resulting content is the print page HTML.
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="isLive"></param>
        /// <returns>A string.</returns>
        public static string RetrieveResult(Guid printID, bool isLive)
        {
            string printPageHtml = null;
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

                if (!string.IsNullOrEmpty(connString))
                {
                    using (SqlConnection conn = SqlHelper.CreateConnection(connString))
                    {
                        using (SqlDataReader reader =
                                    SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "dbo.ct_getPrintResultCache",
                                    new SqlParameter("@PrintId ", printID),
                                    new SqlParameter("@isLive", isLive ? 1 : 0)
                            ))
                        {
                            if (reader.Read())
                            {
                                SqlFieldValueReader sqlFVReader = new SqlFieldValueReader(reader);
                                printPageHtml = sqlFVReader.GetString("content");
                            }
                            else
                            {
                                ErrorPageDisplayer.RaisePageByCode("CTSPrintDataManager", 404);
                                throw new PrintIDNotFoundException("The given printID did not match any cache values in the database");
                            }
                        }
                    }
                }
                else
                {
                    ErrorPageDisplayer.RaisePageByCode("CTSPrintDataManager", 500);
                    throw new DbConnectionException("Configuration Missing: Connection string is null, update the web config with connection string");
                }

                if (!string.IsNullOrWhiteSpace(printPageHtml))
                {
                    return printPageHtml;
                }
                else
                {
                    throw new PrintFetchFailureException("Failed in CTSPrintDataManager - Cannot return null page HTML");
                }
            }
            catch
            {
                throw new PrintFetchFailureException("Failed in CTSPrintDataManager");
            }
        }
    }
}