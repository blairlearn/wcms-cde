using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using NCI.Web.CDE;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.Modules;
using NCI.Search.BestBets;



/*
using NCI.Util;
using NCI.Search.Endeca;
*/

namespace CancerGov.Search.BestBets
{
    public static class BestBetsPresentationManager
    {
        public static BestBetUIResult[] GetBestBets(string searchTerm, DisplayLanguage lang)
        {
            List<BestBetUIResult> rtnResults = new List<BestBetUIResult>();

            //Note, new NCI.Search.BestBets.BestBetsManager will clean terms for us, so we 
            //do not have to worry about that.

            /* LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE
             * --------------------------------------------------------------------------------
            string dimIDs = "0";
            if (lang == DisplayLanguage.Spanish)
            {
                dimIDs = Strings.Clean(ConfigurationSettings.AppSettings["EndecaSpanishBestBets"]) ?? dimIDs;
            }
             ----------------------------------------------------------------------------------------
             LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE LANGUAGE */
              
             
            //TODO: Language!!!!
            //NOTE: This can throw an exception - in the past we left it unhandled.  That is stupid, because
            //we log other errors.  Put that logging here!
            BestBetResult[] rawResults = BestBetsManager.Search(searchTerm);
           

            //Loop through the cats and get the list items.
            foreach (BestBetResult res in rawResults)
            {
                try
                {
                    if (string.IsNullOrEmpty(res.CategoryID))
                    {
                        NCI.Logging.Logger.LogError("GetBestBets", "category id is null/empty", NCI.Logging.NCIErrorLevel.Warning);
                        continue;
                    }

                    string bbResFileName = String.Format(ContentDeliveryEngineConfig.PathInformation.BestBetsResultPath.Path, res.CategoryID);

                    BestBetUIResult bbResult = ModuleObjectFactory<BestBetUIResult>.GetObjectFromFile(bbResFileName);

                    if (bbResult != null && bbResult.Display)
                        rtnResults.Add(bbResult);
                }
                catch (Exception ex)
                { 
                    // The bestbet result xml file may not always be there, so catch the exception and log the error
                    // and ignore the exception
                    NCI.Logging.Logger.LogError("GetBestBets", "could not find bb result for category id " + res.CategoryID + " Category name " + res.CategoryName, NCI.Logging.NCIErrorLevel.Warning, ex);

                }
            }

            return rtnResults.ToArray();
        }

        /// <summary>
        /// This is so the LINQ query that builds up the list items does not look too crazy.
        /// </summary>
        /// <param name="prettyUrl"></param>
        /// <param name="url"></param>
        /// <param name="urlArguments"></param>
        /// <returns>The prettyUrl if it is not null, otherwise the url, and arguments if there are any.</returns>
        //private static string GetListItemUrl(string prettyUrl, string url, string urlArguments)
        //{
        //    string rtnUrl = Strings.Clean(prettyUrl);

        //    if (string.IsNullOrEmpty(rtnUrl))
        //    {
        //        rtnUrl = Strings.Clean(url);
        //        if (!string.IsNullOrEmpty(rtnUrl))
        //        {
        //            string args = Strings.Clean(urlArguments);
        //            if (!string.IsNullOrEmpty(args))
        //                rtnUrl += "?" + args;
        //        }
        //        else
        //        {
        //            rtnUrl = "#"; //This should NEVER happen, but it is much better than an exception being thrown.
        //        }
        //    }

        //    return rtnUrl;
        //}
    }
}
