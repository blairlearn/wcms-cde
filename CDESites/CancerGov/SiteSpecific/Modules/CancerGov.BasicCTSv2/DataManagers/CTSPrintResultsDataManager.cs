using System.Web;
using System;
using System.Collections;
using System.Web.Script.Serialization;
using System.Collections.Generic;
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
        /// <summary>
        /// Connects to the database, and executes the stored proc with the required parameters. The 
        /// resulting content is the guid associated with the cached print content.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="searchParams"></param>
        /// <param name="isLive"></param>
        /// <returns>A guid.</returns>
        public static Guid SavePrintResult(string content, IEnumerable<String> trialIDs, CTSPrintSearchParams searchParams, bool isLive)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
            {
                Guid printResultGuid = Guid.Empty;

                SqlParameter[] parameters = {
                    new SqlParameter("@printid", SqlDbType.UniqueIdentifier),
                    new SqlParameter("@content", SqlDbType.NVarChar, content.Length),
                    new SqlParameter("@searchparams", SqlDbType.NVarChar),
                    new SqlParameter("@isLive", SqlDbType.Bit),
                    new SqlParameter("@trialids", SqlDbType.Structured)
                };
                parameters[0].Direction = ParameterDirection.Output;
                parameters[1].Value = content;
                parameters[2].Value = new JavaScriptSerializer().Serialize(searchParams);
                parameters[3].Value = isLive;
                parameters[4].Value = CreatePrintIdDataTable(trialIDs);

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

        private static DataTable CreatePrintIdDataTable(IEnumerable<String> trialIDs)
        {
            // This datatable must be structured like the datatable in the stored proc, 
            // in order to be passed in correctly as a parameter.
            DataTable dt = new DataTable();

            // Second column, "trialid", is an varchar(124)
            DataColumn dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.MaxLength = 124;
            dc.ColumnName = "trialid";
            dt.Columns.Add(dc);

            foreach (var id in trialIDs)
            {
                DataRow row = dt.NewRow();
                row["trialid"] = id;
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// Connects to the database , and executes the stored proc with the required parameters. The 
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