using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using NCI.Data;
using System.Configuration;

namespace CancerGov.DataManager
{
    static public class GeneralCommentsDataManager
    {
        static public void AddComments(string comments, string commetType)
        {
            // Code goes here for calling the stored proc.  

            string connString = ConfigurationSettings.AppSettings["DbConnectionString"];

            if (!string.IsNullOrEmpty(connString))
            {
                using (SqlConnection conn = SqlHelper.CreateConnection(connString))
                {
                    using (SqlDataReader reader =
                                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "dbo.usp_GeneralComments_Add",
                                new SqlParameter("@Comment", comments),
                                new SqlParameter("@CommentType", commetType)
                                ))
                               
                        



                
            }
        }
    }
}

