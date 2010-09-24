using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using NCI.Data;
using NCI.Logging;

namespace NCI.DataManager
{
    /// <summary>
    /// This class is a datamanager class and deals with all data that resides 
    /// in the database and used maily for content and dynamic searches.
    /// </summary>
    public class SearchDataManager
    {
        /// <summary>
        /// Connects to the database , and executes the stored proc with the required parameter. The 
        /// results are processed as SearchResult object.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="keyWords"></param>
        /// <param name="recordsPerPage"></param>
        /// <param name="maxResults"></param>
        /// <param name="searchFilter"></param>
        /// <param name="excludeSearchFilter"></param>
        /// <param name="language"></param>
        /// <param name="searchType"></param>
        /// <returns>A collection which contains Search Result objects.</returns>
        public static ICollection<SearchResult> Execute(int currentPage, DateTime startDate, DateTime endDate, string keyWords, int recordsPerPage,
            int maxResults, string searchFilter, string excludeSearchFilter, string language, string searchType, out int actualMaxResult)
        {
            try
            {
                ICollection<SearchResult> searchResults = new Collection<SearchResult>();

                #region Testing only remove once the real stored proc is available
                SearchResult searchResult1 = new SearchResult();
                searchResult1.PostedDate = "1/1/2009";
                searchResult1.UpdatedDate = "1/1/2009";
                searchResult1.ShortDescription = "The impact of HPV vaccines and screening tests on cervical cancer prevention: A National Cancer Institute Science Writers' Seminar";
                searchResult1.LongDescription = "science writers' seminar to discuss new research findings and future directions in HPV-related cancer research.  Among the topics discussed will be the natural history of HPV and related cancers, advances in screening techniques and tools, the role of vaccines and microbicides in prevention, both nationally and internationally, and future research directions";
                searchResult1.HRef = "http://www.cancer.gov/newscenter/pressreleases/HPVseminar";
                if (currentPage == 1 || currentPage == 2 || currentPage == 3)
                    searchResults.Add(searchResult1);

                searchResult1 = new SearchResult();
                searchResult1.PostedDate = "1/1/2009";
                searchResult1.UpdatedDate = "1/1/2009";
                searchResult1.ShortDescription = "The impact of HPV vaccines and screening tests on cervical cancer prevention: A National Cancer Institute Science Writers' Seminar";
                searchResult1.LongDescription = "science writers' seminar to discuss new research findings and future directions in HPV-related cancer research.  Among the topics discussed will be the natural history of HPV and related cancers, advances in screening techniques and tools, the role of vaccines and microbicides in prevention, both nationally and internationally, and future research directions";
                searchResult1.HRef = "http://www.cancer.gov/newscenter/pressreleases/HPVseminar";
                if( currentPage == 1 || currentPage == 2 )
                    searchResults.Add(searchResult1);

                actualMaxResult = 5;
                return searchResults;
                
                #endregion

                string connString = ConfigurationSettings.AppSettings["DbConnectionString"];

                if (!string.IsNullOrEmpty(connString))
                {
                    using (SqlConnection conn = SqlHelper.CreateConnection(connString))
                    {
                        SqlCommand command = new SqlCommand();
                        command.Connection = conn;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "uspDynamicSearch";

                        // Create output parameter for actualMaxResult
                        SqlParameter paramActualMaxResult = new SqlParameter("@actualMaxResult", SqlDbType.Int);
                        paramActualMaxResult.Direction = ParameterDirection.Output;

                        using (SqlDataReader reader =
                                    SqlHelper.ExecuteReader(conn, CommandType.StoredProcedure, "uspDynamicSearch",
                                    new SqlParameter("@currentPage", currentPage),
                                    new SqlParameter("@startDate", startDate),
                                    new SqlParameter("@endDate", endDate),
                                    new SqlParameter("@keyWords", keyWords),
                                    new SqlParameter("@recordsPerPage", recordsPerPage),
                                    new SqlParameter("@maxResults", maxResults),
                                    new SqlParameter("@searchFilter", searchFilter),
                                    new SqlParameter("@excludeSearchFilter", excludeSearchFilter),
                                    new SqlParameter("@language", language),
                                    paramActualMaxResult
                            ))
                        {
                            SqlFieldValueReader sqlFVReader = new SqlFieldValueReader(reader);
                            while (reader.Read())
                            {
                                SearchResult searchResult = new SearchResult();
                                searchResult.HRef = sqlFVReader.GetString("HRef");
                                searchResult.LongDescription = sqlFVReader.GetString("LongDescription");
                                searchResult.ShortDescription = sqlFVReader.GetString("ShortDescription");
                                DateTime dt = sqlFVReader.GetDateTime("PostedDate");
                                if (dt != DateTime.MinValue)
                                    searchResult.PostedDate = String.Format("{0:MM/dd/yyyy}", dt);

                                dt = sqlFVReader.GetDateTime("UpdatedDate");
                                if (dt != DateTime.MinValue)
                                    searchResult.UpdatedDate = String.Format("{0:MM/dd/yyyy}", dt);

                                searchResults.Add(searchResult);
                            }

                            actualMaxResult = Convert.ToInt32(paramActualMaxResult.Value);
                        }
                    }
                }
                else
                    throw new Exception("Configuration Missing:Connection string is null, update the web config with connection string");

                return searchResults;
            }
            catch (Exception ex)
            {
                Logger.LogError("SearchDataManager:Execute", "Failed in DataManager", NCIErrorLevel.Error);
                throw ex;
            }
        }
    }
}
