using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CancerGov.Common.ErrorHandling;
using NCI.Data;

namespace CancerGov.CDR.TermDictionary
{
    /// <summary>
    /// This is the database layer that calls the stored procedure based on the
    /// users search criteria
    /// </summary>
    public static class TermDictionaryQuery
    {
        /// <summary>
        /// This is a method that will query the database and return a list of glossary terms
        /// based on the criteria and the language needed
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="language"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        [UsesSProc("usp_GetGlossary")]
        public static DataTable Search(string language, string criteria, int rows)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@Criteria", SqlDbType.VarChar),
                                       new SqlParameter("@Language", SqlDbType.VarChar),
                                       new SqlParameter("@topN", SqlDbType.Int)
                                   };

            // Set the values on the parameters
            parms[0].Value = criteria;
            parms[1].Value = language;
            parms[2].Value = rows;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.AppSettings["CDRDbConnectionString"],
                    CommandType.StoredProcedure,
                    "usp_GetGlossary",
                    parms);

            }
            catch (Exception ex)
            {
                CancerGovError.LogError("TermDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }

        /// <summary>
        /// This method calls the GetDefinition method and does not provide a termID
        /// to the GetDefintion method. This assures that the user doesn't accidentally
        /// send two values TermName and TermID
        /// </summary>
        /// <param name="language"></param>
        /// <param name="termID"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public static DataTable GetDefinitionByTermName(string language, string termName, string audience)
        {
            return GetDefinition(language, termName, null, audience);
        }

        /// <summary>
        /// This method calls the GetDefinition method and does not provide a termName
        /// to the GetDefintion method. This assures that the user doesn't accidentally
        /// send two values TermName and TermID
        /// </summary>
        /// <param name="language"></param>
        /// <param name="termID"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        public static DataTable GetDefinitionByTermID(string language, string termID, string audience)
        {
            return GetDefinition(language, null, termID, audience);
        }

        /// <summary>
        /// Query to return all terms whose name is close to the term we are looking for
        /// </summary>
        /// <param name="language"></param>
        /// <param name="termName"></param>
        /// <param name="nMatches"></param>
        /// <returns></returns>
        [UsesSProc("usp_GetGlossaryTermNeighbors")]
        public static DataTable GetTermNeighbors(string language, string termName, int nMatches)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@name", SqlDbType.VarChar),
                                       new SqlParameter("@num", SqlDbType.Int),
                                       new SqlParameter("@language", SqlDbType.VarChar)
                                   };

            // Set the values on the parameters
            parms[0].Value = termName;
            parms[1].Value = nMatches;
            parms[2].Value = language;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.AppSettings["CDRDbConnectionString"],
                    CommandType.StoredProcedure,
                    "usp_GetGlossaryTermNeighbors",
                    parms);

                // Add the columns we don't have to make this work at the manager level
                dt.Columns.Add(new DataColumn("MediaHTML", System.Type.GetType("System.String")));
                //if (dt.Rows.Count > 0)
                //{
                //if (language == "Spanish")
                //{
                //    dt.Columns.Add(new DataColumn("TermName", System.Type.GetType("System.String")));
                //}
                //else
                //{
                dt.Columns.Add(new DataColumn("OLTermName", System.Type.GetType("System.String")));
                //}
                //}

            }
            catch (Exception ex)
            {
                CancerGovError.LogError("TermDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }

        /// <summary>
        /// Query the GlossaryDefinition stored procedure. This method is called by 
        /// two other methods. A DataTable is returned if the information is found
        /// in the database.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="termName"></param>
        /// <param name="termID"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        [UsesSProc("usp_GetGlossaryDefinition")]
        private static DataTable GetDefinition(string language, string termName, string termID, string audience)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@Term", SqlDbType.VarChar),
                                       new SqlParameter("@ID", SqlDbType.VarChar),
                                       new SqlParameter("@Audience", SqlDbType.VarChar),
                                       new SqlParameter("@language", SqlDbType.VarChar)
                                   };

            // Set the values on the parameters
            parms[0].Value = termName;
            parms[1].Value = termID;
            parms[2].Value = audience;
            parms[3].Value = language;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.AppSettings["CDRDbConnectionString"],
                    CommandType.StoredProcedure,
                    "usp_GetGlossaryDefinition",
                    parms);

                // Let's add the SpanishTermName so we don't break in the manager
                //if (dt.Rows.Count > 0)
                //{
                //if (language == "Spanish")
                //{
                //    dt.Columns.Add(new DataColumn("TermName", System.Type.GetType("System.String")));
                //}
                //else
                //{
                dt.Columns.Add(new DataColumn("OLTermName", System.Type.GetType("System.String")));
                //}
                //}
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("TermDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }

    }
}
