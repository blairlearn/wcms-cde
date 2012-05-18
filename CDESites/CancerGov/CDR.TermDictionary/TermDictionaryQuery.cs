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
        public static DataTable Search(string language, string criteria, int rows)
        {
            return Search(language, criteria, rows, "Cancer.gov", "Patient");
        }
        
        /// <summary>
        /// This is a method that will query the database and return a list of glossary terms
        /// based on the criteria and the language needed
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="language"></param>
        /// <param name="rows"></param>
        /// <param name="dictionary"></param>
        /// <param name="audience"></param>
        /// <returns></returns>
        [UsesSProc("usp_GetGlossary2")]
        public static DataTable Search(string language, string criteria, int rows, string dictionary, string audience)
        {
            // create our null object
            DataTable dt = null;

            SqlParameter outputParam = new SqlParameter("@totalresult", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@Criteria", SqlDbType.VarChar),
                                       new SqlParameter("@Language", SqlDbType.VarChar),
                                       new SqlParameter("@topN", SqlDbType.Int),
                                       new SqlParameter("@dictionary", SqlDbType.VarChar),
                                       new SqlParameter("@audience", SqlDbType.VarChar),
                                       outputParam
                                   };

            // Set the values on the parameters
            parms[0].Value = criteria;
            parms[1].Value = language;
            parms[2].Value = rows;
            parms[3].Value = dictionary;
            parms[4].Value = audience;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_GetGlossary2",
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
        /// This is a method that will query the database and return a list of letters for which
        /// glossary terms start with
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        [UsesSProc("usp_getGlossaryFirstLetter")]
        public static DataTable AZListLettersWithData(string language)
        {
            // create our null object
            DataTable dt = null;

            SqlParameter outputParam = new SqlParameter("@totalresult", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            // create our parameter array
            SqlParameter[] parms = {   new SqlParameter("@Language", SqlDbType.VarChar)};

            // Set the values on the parameters
            parms[0].Value = language;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_getGlossaryFirstLetter",
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
        /// This is a method that will query the database and return a list of glossary terms
        /// based on the criteria and the language needed
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="language"></param>
        /// <param name="rows"></param>
        /// <param pageNumber="rows">The current page for which data needs to be retrived.
        /// If this value argument is -1 , the stored proc ignores the pageNumber</param>
        /// <returns></returns>
        [UsesSProc("usp_GetGlossary")]
        public static DataTable GetTermDictionaryList(string language, string criteria, int rows, int pageNumber, ref int totalRecordCount)
        {
            // create our null object
            DataTable dt = null;
            totalRecordCount = 0;

            SqlParameter outputParam = new SqlParameter("@totalresult", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;

            // create our parameter array
            SqlParameter[] parms = {
                                       new SqlParameter("@Criteria", SqlDbType.VarChar),
                                       new SqlParameter("@Language", SqlDbType.VarChar),
                                       new SqlParameter("@topN", SqlDbType.Int),
                                       new SqlParameter("@pagenumber", SqlDbType.Int),
                                       new SqlParameter("@recordsPerPage", SqlDbType.Int),
                                       outputParam
                                   };

            
            // Set the values on the parameters
            parms[0].Value = criteria;
            parms[1].Value = language;
            parms[2].Value = rows;
            parms[3].Value = pageNumber;
            parms[4].Value = rows;
            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_GetGlossary",
                    parms);

                totalRecordCount = (int)parms[5].Value;
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
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_GetGlossaryTermNeighbors",
                    parms);

                // Add the columns we don't have to make this work at the manager level
                dt.Columns.Add(new DataColumn("MediaHTML", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("MediaCaption", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("MediaID", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("AudioMediaHTML", System.Type.GetType("System.String")));
                dt.Columns.Add(new DataColumn("RelatedInformationHtml", System.Type.GetType("System.String")));
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
        [UsesSProc("usp_GetGlossaryDefinition2")]
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
            //parms[2].Value = audience;
            parms[2].Value = "Health professional";
            parms[3].Value = language;

            try
            {
                // Query the database and get the results
                dt = SqlHelper.ExecuteDatatable(
                    ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString,
                    CommandType.StoredProcedure,
                    "usp_GetGlossaryDefinition2",
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



        public static ArrayList GetPopDefinition(string type, string param, string pdqVersion, string language)
        {
            ArrayList returnvalue = new ArrayList(3);
            string version = (pdqVersion == "HealthProfessional") ? "Health Professional" : "Patient";
            SqlConnection dbh = new SqlConnection(ConfigurationManager.ConnectionStrings["CDRDbConnectionString"].ConnectionString);
            SqlCommand sth = new SqlCommand("usp_GetGlossaryDefinition", dbh);
            sth.CommandType = CommandType.StoredProcedure;

            if (type == "term")
            {
                SqlParameter param_term = new SqlParameter("@Term", SqlDbType.VarChar, 255);
                param_term.Value = param;
                sth.Parameters.Add(param_term);

                SqlParameter param_audience = new SqlParameter("@Audience", SqlDbType.VarChar, 50);
                param_audience.Value = version;
                sth.Parameters.Add(param_audience);

                SqlParameter param_language = new SqlParameter("@Language", SqlDbType.VarChar, 50);
                param_language.Value = language.ToString().ToUpper();
                sth.Parameters.Add(param_language);
            }
            else if (type == "id")
            {
                SqlParameter param_id = new SqlParameter("@ID", SqlDbType.VarChar, 50);
                param_id.Value = param;
                sth.Parameters.Add(param_id);

                SqlParameter param_audience = new SqlParameter("@Audience", SqlDbType.VarChar, 50);
                param_audience.Value = version;
                sth.Parameters.Add(param_audience);

                SqlParameter param_language = new SqlParameter("@Language", SqlDbType.VarChar, 50);
                param_language.Value = language.ToString().ToUpper();
                sth.Parameters.Add(param_language);
            }
            else
            {
                throw new Exception("Unknown type (" + type + ") in get_definition()");
            }

            dbh.Open();

            SqlDataReader rows = sth.ExecuteReader();
            if (rows.Read())
            {
                returnvalue.Add(rows[1]); // Name
                returnvalue.Add(rows[2]); // Pronounciation
                returnvalue.Add(rows[3]); // Definition
                returnvalue.Add(rows[4]); // MediaHtml
                returnvalue.Add(rows[7]); // AudioMediaHtml
                rows.Close();
                dbh.Close();
            }
            else
            {
                rows.Close();
                dbh.Close();
                return null;
            }

            return returnvalue;
        }

    }
}
