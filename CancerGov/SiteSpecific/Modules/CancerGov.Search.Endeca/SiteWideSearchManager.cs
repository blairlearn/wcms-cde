using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Search.Endeca;

namespace CancerGov.Modules.Search.Endeca
{
    /// <summary>
    /// 
    /// </summary>
    public static class SiteWideSearchManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="offSet"></param>
        /// <param name="dimIDs"></param>
        /// <returns></returns>
        public static SiteWideSearchResults GetSiteWideSearchResults(string searchTerm, long itemsPerPage, long offSet, DisplayLanguage lang)
        {
            string dimIDs = Strings.Clean(ConfigurationManager.AppSettings["EndecaAllDocs"]);

            if (lang == DisplayLanguage.Spanish)
            {
                dimIDs = Strings.Clean(ConfigurationManager.AppSettings["EndecaSpanishDocs"]);
            }

            if (dimIDs == null)
            {
                throw new Exception("The site wide search result doc types are not set correctly.");
            }

            //Create a search object and execute it.
            EndecaSearch eSearch = new EndecaSearch(searchTerm, itemsPerPage, offSet, dimIDs);
            eSearch.ExecuteSearch();

            //I do not want to mess with the EndecaSearch class since it is used by more than the site wide
            //search results.  Unfortunately FillSearchResults does not take in an IList, but an ArrayList.
            //The SiteWideSearchResults is a generic list, so I still need to use an ArrayList temporarily.
            ArrayList searchResults = new ArrayList();
            eSearch.FillSearchResults(searchResults);

            if (searchResults.Count == 0)
            {
                return new SiteWideSearchResults(0);
            }
            else
            {
                SiteWideSearchResults results = new SiteWideSearchResults(eSearch.TotalSearchResults);

                foreach (EndecaResult res in searchResults)
                    results.Add(res);

                //DYM Keyword
                string didYouMeanStr = eSearch.DidYouMean();
                if (didYouMeanStr != null)
                {
                    results.DidYouMeanText = didYouMeanStr;
                }

                return results;
            }
        }
    }
}
