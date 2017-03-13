using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using NCI.Data;
using System.Configuration;

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
            try
            {
                string printPageHtml = "";

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
                            while (reader.Read())
                            {
                                //SqlFieldValueReader sqlFVReader = new SqlFieldValueReader(reader);
                                //printPageHtml = reader.GetString("content");
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Configuration Missing: Connection string is null, update the web config with connection string");
                }

                return printPageHtml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
