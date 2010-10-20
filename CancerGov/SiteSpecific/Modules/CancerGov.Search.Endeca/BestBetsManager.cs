using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Search.Endeca;

namespace CancerGov.Modules.Search.Endeca
{
    public static class BestBetsManager
    {
        public static BestBetsResults GetBestBets(string searchTerm, DisplayLanguage lang)
        {
            BestBetsResults rtnResults = new BestBetsResults();

            //Remove those characters that are not valid
            searchTerm = Regex.Replace(searchTerm, "[-':;\"\\./]", "");

            string dimIDs = "0";
            if (lang == DisplayLanguage.Spanish)
            {
                dimIDs = Strings.Clean(ConfigurationSettings.AppSettings["EndecaSpanishBestBets"]) ?? dimIDs;
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
                DataTable dt = GetListFromDB(res.ListID);

                //Only create a BestBetResult if there are list items
                if (dt.Rows.Count > 0)
                {                    
                    //Soo... this is weird, but it is in the old logic.  We seem
                    //to get the category name not from the EndecaBestBetResults, but
                    //from the datatable that is returned.  The reason for this is probably
                    //that the best bets in endeca are only updated when synonyms or
                    //categories are added.  So if someone were to change the name of a
                    //category then the best bets would have to be re-indexed in order to
                    //show the new category name.  However if we get the name from the
                    //database, then we will always have the most up to date category
                    //name for a best bets category.
                    //Since that is the way it currently works, and it would actually be
                    //a change in the process of updating best bets, we will keep the logic
                    //in here even though it is kind of hinky.
                    string catName = Strings.Clean((string)dt.Rows[0]["CatName"]);

                    if (!string.IsNullOrEmpty(catName))
                    {
                        BestBetResult bbr = new BestBetResult(catName);

                        //Note: the AsEnumerable comes from System.Data.DataSetExtensions.
                        var listItems =
                            from listItem in dt.AsEnumerable() 
                            select new BestBetListItem(
                                GetListItemUrl(
                                    listItem.Field<string>("PrettyUrl"),
                                    listItem.Field<string>("Url"),
                                    listItem.Field<string>("UrlArguments")),
                                Strings.Clean(listItem.Field<string>("Title")), 
                                Strings.Clean(listItem.Field<string>("Description")));

                        bbr.AddRange(listItems);
                        rtnResults.Add(bbr);
                    }                                        
                }
                if (dt != null)
                    dt.Dispose();
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
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DbConnectionString"]))
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
