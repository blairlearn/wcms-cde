using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using CancerGov.UI;
using CancerGov.Common.ErrorHandling;

using NCI.Util;
using CancerGov.Search.AutoSuggest;
using NCI.Web.CDE;

namespace CancerGov.Search.AutoSuggest
{
    /// <summary>
    /// This Class is the business layer that interfaces between the user interface and
    /// the database layer. 
    /// </summary>
    public static class AutoSuggestSearchManager
    {
        /// <summary>
        /// This methods filters the information passed to it in order to refine the query
        /// that will be called in the database layer.
        /// </summary>
        /// <param name="language">enumeration indicating language</param>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">indicator as to whether the text is to be searched starting from the beginning or anywhere
        ///                        in the string</param>
        /// <returns>Returns the search results</returns>
        public static AutoSuggestSearchCollection Search(string language, string criteria, int maxRows, bool contains)
        {
            AutoSuggestSearchCollection dc = new AutoSuggestSearchCollection();

            // Find out how we should search for the string
            if (Strings.Clean(criteria) != null)
            {
                // input criteria cleaup
                // 1. replace any ',' or ';' with space
                // 2. Trim whitespace.
                criteria = criteria.Trim();
                criteria = criteria.Replace(",", " ");
                criteria = criteria.Replace(";", " ");

                // Find out the field we need to get to build our list
                string fieldName = "TermName";
                if (language == "Spanish")
                {
                    fieldName = language.ToString() + fieldName;
                }

                try
                {
                    // Call the database layer and get data
                    DataTable dt = AutoSuggestSearchQuery.Search(
                            language.ToString(),
                            criteria,
                            maxRows);

                    // use Linq to move information from the dataTable
                    // into the AutoSuggestSearchCollection
                    dc.AddRange(
                        from entry in dt.AsEnumerable()
                        select GetEntryFromDR(entry)
                    );

                }
                catch (Exception ex)
                {
                    CancerGovError.LogError("TermDictionatyManager", 2, ex);
                    throw ex;
                }
            }

            return dc;
        }

        /// <summary>
        /// Extracts information from a DataRow and builds a new
        /// AutoSuggestSearchDataItem
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>AutoSuggestSearchDataItem</returns>
        private static AutoSuggestSearchDataItem GetEntryFromDR(DataRow dr)
        {
            // return the new object
            AutoSuggestSearchDataItem ddi = new AutoSuggestSearchDataItem(
                0,
                Strings.Clean(dr["termname"]),
                String.Empty,
                String.Empty
             );

            return ddi;
        }
    }
}
