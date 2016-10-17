using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using Common.Logging;
using NCI.Search.Endeca;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.Modules;

namespace CancerGov.Modules.Search.Endeca
{
    public static class BestBetsManager
    {
        public static BestBetsResults GetBestBets(string searchTerm, DisplayLanguage lang)
        {
            ILog log = LogManager.GetLogger(typeof(BestBetsResults));

            BestBetsResults rtnResults = new BestBetsResults();

            //Remove those characters that are not valid
            searchTerm = Regex.Replace(searchTerm, "[-':;\"\\./]", "");

            string dimIDs = "0";
            if (lang == DisplayLanguage.Spanish)
            {
                dimIDs = Strings.Clean(ConfigurationManager.AppSettings["EndecaSpanishBestBets"]) ?? dimIDs;
            }

            EndecaBestBetsSearch bbs = new EndecaBestBetsSearch(searchTerm, dimIDs);

            //If there are problems, that is ok, I want the page that gets the best bets to handle any
            //exceptions.
            bbs.ExecuteSearch();
            ArrayList tmpBBCats = new ArrayList();
            bbs.FillSearchResults(tmpBBCats);

            //Loop through the cats and get the list items.
            foreach (EndecaBestBetResult res in tmpBBCats)
            {
                try
                {
                    if (string.IsNullOrEmpty(res.CategoryID))
                    {
                        log.Warn("GetBestBets(): category id is null/empty");
                        continue;
                    }

                    string bbResFileName = String.Format(ContentDeliveryEngineConfig.PathInformation.BestBetsResultPath.Path, res.CategoryID);


                    BestBetResult bbResult = ModuleObjectFactory<BestBetResult>.GetObjectFromFile(bbResFileName);
                    if (bbResult.Display.ToLower() == "true")
                    {
                        if (bbResult != null)
                            rtnResults.Add(bbResult);
                    }
                }
                catch (Exception ex)
                { 
                    // The bestbet result xml file may not always be there, so catch the exception and log the error
                    // and ignore the exception
                    log.WarnFormat("GetBestBets(): could not find bb result for category id {0} Category name {1}", ex, res.CategoryID, res.CategoryName);

                }
            }

            return rtnResults;
        }

        /// <summary>
        /// This is so the LINQ query that builds up the list items does not look too crazy.
        /// </summary>
        /// <param name="prettyUrl"></param>
        /// <param name="url"></param>
        /// <param name="urlArguments"></param>
        /// <returns>The prettyUrl if it is not null, otherwise the url, and arguments if there are any.</returns>
        private static string GetListItemUrl(string prettyUrl, string url, string urlArguments)
        {
            string rtnUrl = Strings.Clean(prettyUrl);

            if (string.IsNullOrEmpty(rtnUrl))
            {
                rtnUrl = Strings.Clean(url);
                if (!string.IsNullOrEmpty(rtnUrl))
                {
                    string args = Strings.Clean(urlArguments);
                    if (!string.IsNullOrEmpty(args))
                        rtnUrl += "?" + args;
                }
                else
                {
                    rtnUrl = "#"; //This should NEVER happen, but it is much better than an exception being thrown.
                }
            }

            return rtnUrl;
        }

        //So if we actually had some kind of list manager and query, we would go to that.  Seeing as how we
        //don't I am moving this out of the presentation layer and into here.  A BestBetsQuery would be overkill
        //especially since it goes to the database to get someone else's data...
        private static DataTable GetListFromDB(Guid listID)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("dbo.usp_GetBestBetsByListId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ListID", SqlDbType.UniqueIdentifier);
                    cmd.Parameters["@ListID"].Value = listID;
                    cmd.Parameters.Add("@Where", SqlDbType.VarChar);
                    cmd.Parameters["@Where"].Value = ""; //What is this???

                    using (SqlDataAdapter dbAdapter = new SqlDataAdapter(cmd))
                    {
                        dbAdapter.Fill(dt);
                    }
                }
            }

            return dt;
        }
    }
}
