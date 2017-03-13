using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using NCI.Data;
using NCI.DataManager;
using CancerGov.ClinicalTrials.Basic.v2;
using NCI.Web.CDE.Application;

namespace CancerGov.DataManager
{
    public class CTSPrintDataManager
    {
        //static ILog log = LogManager.GetLogger(typeof(CTSPrintDataManager));

        /// <summary>
        /// Connects to the database , and executes the stored proc with the required parameter. The 
        /// resulting content is the print page HTML.
        /// </summary>
        /// <param name="printId"></param>
        /// <param name="isLive"></param>
        /// <returns>A string.</returns>
        public static string Execute(Guid printID, bool isLive)
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
                                ErrorPageDisplayer.RaisePageByCode("CTSPrintDataManager", 500);
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
