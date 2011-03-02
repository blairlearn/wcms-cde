using System;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using CancerGov.Common.ErrorHandling;
using NCI.Data;

namespace CancerGov.CDR.DrugDictionary
{
    /// <summary>
    /// This is the database layer that calls the stored procedure based on the
    /// users search criteria
    /// </summary>
    public static class DrugDictionaryQuery
    {
        /// <summary>
        /// This is a method that will query the database and return a list of drug terms
        /// based on the criteria. It allows the user to paginate by passing in paging info
        /// </summary>
        /// <param name="criteria">String used to query the sp</param>
        /// <param name="rows">Number of rows per page</param>
        /// <param name="curPage">Current page data is needed for</param>
        /// <param name="otherNames">Boolean indicating whether otherName should be include in the query</param>
        /// <returns></returns>
        [UsesSProc("usp_TermDD_GetMatchingTerms")]
        public static DataTable Search(string criteria, int rows, int curPage, char otherNames, out int matchCount)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@Match", SqlDbType.NVarChar),
                                       new SqlParameter("@PageSize", SqlDbType.Int),
                                       new SqlParameter("@CurPage", SqlDbType.Int),
                                       new SqlParameter("@OtherNamesP", SqlDbType.Char),
                                       new SqlParameter("@NumMatches", SqlDbType.Int)
                                   };

            // Set the values on the parameters
            parms[0].Value = criteria;
            parms[1].Value = rows;
            parms[2].Value = curPage;
            parms[3].Value = otherNames;
            parms[4].Direction = ParameterDirection.Output;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_TermDD_GetMatchingTerms",
                    parms);

                // Add the column so that the manager doesn't get upset
                dt.Columns.Add(new DataColumn("PrettyURL", System.Type.GetType("System.String")));

                // Get the total number of matches we found. This value may be different than
                // the actual number of rows returned
                matchCount = (int)parms[4].Value;
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }

        /// <summary>
        /// This is a method that will query the database and return a list of drug termID and
        /// Preferred names based on the criteria. The user may specify the number of rows to return.
        /// </summary>
        /// <param name="criteria">String used to query the sp</param>
        /// <param name="rows">Number of rows per page</param>
        /// <param name="curPage">Current page data is needed for</param>
        /// <param name="otherNames">Boolean indicating whether otherName should be include in the query</param>
        /// <returns></returns>
        [UsesSProc("usp_getDrugTermName")]
        public static DataTable SearchNameOnly(string criteria, int rows)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@criteria", SqlDbType.NVarChar),
                                       new SqlParameter("@TopN", SqlDbType.Int),
                                   };

            // Set the values on the parameters
            parms[0].Value = criteria;
            parms[1].Value = rows;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_getDrugTermName",
                    parms);

                // Add the column so that the manager doesn't get upset
                dt.Columns.Add(new DataColumn("otherName", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("definitionHTML", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("PrettyURL", System.Type.GetType("System.String")));
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }

        /// <summary>
        /// This method will query the database to get a definition for a single
        /// term.
        /// </summary>
        /// <param name="termID"></param>
        /// <returns></returns>
        [UsesSProc("usp_TermDD_GetTermDefinition")]
        public static DataSet GetDefinitionByTermID(int termID)
        {
            // create our null object
            DataSet ds = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@TermID", SqlDbType.Int)
                                   };

            // Set the values on the parameters
            parms[0].Value = termID;

            try
            {
                // Query the database and get the results
                ds = SqlHelper.ExecuteDataset(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_TermDD_GetTermDefinition",
                    parms);

                // Add the columns so that the manager doesn't get upset
                if (ds.Tables.Count > 0)
                {
                    ds.Tables[0].Columns.Add(new DataColumn("TermID", System.Type.GetType("System.Int32")));
                    ds.Tables[0].Columns.Add(new DataColumn("OtherName", System.Type.GetType("System.Int32")));
                }
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return ds;
        }

        /// <summary>
        /// This method will query the database for term names that match close
        /// to the term name provided. The number of rows passed indicates how
        /// many names should be found before and after the term name provided.
        /// </summary>
        /// <param name="termName"></param>
        /// <param name="nRows"></param>
        /// <returns></returns>
        [UsesSProc("usp_TermDD_GetTermNeighbors")]
        public static DataTable GetTermNeighbors(string termName, int nRows)
        {
            // create our null object
            DataTable dt = null;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@name", SqlDbType.NVarChar),
                                       new SqlParameter("@num", SqlDbType.Int)
                                   };

            // Set the values on the parameters
            parms[0].Value = termName;
            parms[1].Value = nRows;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_TermDD_GetTermNeighbors",
                    parms);

                // Add the columns so that the manager doesn't get upset
                dt.Columns.Add(new DataColumn("OtherName", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("DefinitionHTML", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("PrettyURL", System.Type.GetType("System.String")));
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionaryQuery", 2, ex);
                throw ex;
            }

            // return the DataTable
            return dt;
        }
    }
}
