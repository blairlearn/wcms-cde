using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CancerGov.Common.ErrorHandling;
using NCI.Data;

namespace CancerGov.Search.AutoSuggest
{
    /// <summary>
    /// This is the database layer that calls the stored procedure based on the
    /// users search criteria
    /// </summary>
    public static class AutoSuggestSearchQuery
    {
        /// <summary>
        /// This is a method that will query the database and return a list of glossary terms
        /// based on the criteria and the language needed
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="language"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [UsesSProc("autosuggest_en_search")]
        public static DataTable Search(string language, string criteria, int rows)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {

                                       new SqlParameter("@t", SqlDbType.VarChar)
                                   };

            // Set the values on the parameters
            parms[0].Value = criteria;

            string storedProcName = "autosuggest_en_search";
            if( language.ToLower() == "spanish" ) 
                storedProcName = "autosuggest_es_search";

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    storedProcName,
                    parms);

            }
            catch (Exception ex)
            {
                CancerGovError.LogError("AutoSuggestSearchQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }
    }
}
