using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Web;
using System.Web;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Helper class to create page-load analytics values for CTS pages.
    /// </summary>
    class CTSWebAnalyticsHelper
    {
        /**
         * TODO: Create unit tests
         *     - Add all available params
         *     - Get better names for delegate 
         *     - Clean up
         */

        ///Delegate definition so we can more cleanly list the parsers we will call.
        private delegate void AnalyticsParamsDelegate(List<string> paramsList, CTSSearchParams searchParams);
        private static AnalyticsParamsDelegate _analyticsParams;

        /// <summary>
        /// Static constructor to initialize.
        /// </summary>
        static CTSWebAnalyticsHelper() {
            _analyticsParams =
                (AnalyticsParamsDelegate)AddAnalyticsCancerType + //First param needs the cast.
                AddAnalyticsSubTypes +
                AddAnalyticsStages +
                AddAnalyticsFindings +
                AddAnalyticsAge +
                AddAnalyticsKeyword +
                AddAnalyticsGender +

                AddAnalyticsInvestigator +
                AddAnalyticsLeadOrg;
        }

        /// <summary>
        /// Get a list of search parameters
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns>A list of search parameter value</returns>
        public static List<String> GetAnalyticsParamsList(CTSSearchParams searchParams)
        {
            List<string> waParamsList = new List<string>();

            // Call each of our delegate methods to build out the parameter list
            _analyticsParams(waParamsList, searchParams);

            // Return the assembled list of params
            return waParamsList;
        }


        #region Analytics param adders

        //Parameter t (Main Cancer Type)
        private static void AddAnalyticsCancerType(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.MainType))
            {
                value = searchParams.MainType.Label;
            }
            waList.Add(value);
        }

        //Parameter st (SubTypes)
        private static void AddAnalyticsSubTypes(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.SubTypes))
            {
                value = AddAnalyticsMultiTermFields(searchParams.SubTypes);
            }
            waList.Add(value);
        }

        //Parameter stg (Stages)
        private static void AddAnalyticsStages(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Stages))
            {
                value = AddAnalyticsMultiTermFields(searchParams.Stages);
            }
            waList.Add(value);
        }

        //Parameter fin (Findings)
        private static void AddAnalyticsFindings(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Findings))
            {
                value = AddAnalyticsMultiTermFields(searchParams.Findings);
            }
            waList.Add(value);
        }

        // Parameter a (Age)
        private static void AddAnalyticsAge(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Age))
            {
                value = searchParams.Age.ToString();
            }
            waList.Add(value);
        }

        //Parameter q (Keyword/Phrase)
        private static void AddAnalyticsKeyword(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Phrase))
            {
                value = HttpUtility.UrlEncode(searchParams.Phrase);
            }
            waList.Add(value);
        }

        // Parameter g (Gender)
        private static void AddAnalyticsGender(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Gender))
            {
                value = HttpUtility.UrlEncode(searchParams.Gender);
            }
            waList.Add(value);
        }








        // Parameter in (Investigator)
        private static void AddAnalyticsInvestigator(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Investigator))
            {
                value = HttpUtility.UrlEncode(searchParams.Investigator);
            }
            waList.Add(value);
        }

        // Parameter lo (Lead Org)
        private static void AddAnalyticsLeadOrg(List<string> waList, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.LeadOrg))
            {
                value = HttpUtility.UrlEncode(searchParams.LeadOrg);
            }
            waList.Add(value);
        }

        #endregion


        #region Util methods

        /// <summary>
        /// Converts a TerminologyFieldSearchParam[] to a string
        /// </summary>
        /// <param name="fieldValues">An array of TerminologyFieldSearchParam[]</param>
        /// <returns></returns>
        private static string AddAnalyticsMultiTermFields(TerminologyFieldSearchParam[] fieldValues)
        {
            List<string> labels = new List<string>();

            foreach (TerminologyFieldSearchParam termField in fieldValues)
            {
                labels.Add(string.Join("/", termField.Label));
            }

            return string.Join(",", labels.ToArray());
        }

        #endregion

    }
}
