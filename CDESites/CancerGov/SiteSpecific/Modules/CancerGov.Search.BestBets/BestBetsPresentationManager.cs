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

            string twoCharLang = string.Empty; //To search all, pass in string.empty
            if (lang == DisplayLanguage.Spanish)
            {
                twoCharLang = "es";
            } 

            //TODO: Language!!!!
            //NOTE: This can throw an exception - in the past we left it unhandled.  That is stupid, because
            //we log other errors.  Put that logging here!
            BestBetResult[] rawResults = BestBetsManager.Search(searchTerm, twoCharLang);
           

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

    }
}
