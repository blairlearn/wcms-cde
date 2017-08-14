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
        private delegate void ParameterAnalyticsDelegate(List<String> arr, CTSSearchParams searchParams);
        private static ParameterAnalyticsDelegate _paramAnalytics;

        /// <summary>
        /// Static constructor to initialize 
        /// </summary>
        static CTSWebAnalyticsHelper() {
            _paramAnalytics =
                (ParameterAnalyticsDelegate)SerializeCancerType + //First param needs the cast.
                SerializeAge;
        }

        /// <summary>
        /// Serialize the parameters to a URL
        /// </summary>
        /// <param name="searchParams">The search parameters to serialize</param>
        /// <returns>A URL with query params.</returns>
        public static List<String> GetAnalyticsArray(CTSSearchParams searchParams)
        {
            NciUrl url = new NciUrl();
            List<string> aarr = new List<string>();

            _paramAnalytics(aarr, searchParams);

            return aarr;
            // for imeplementation, see GetPageUrl() in results control
        }



        #region Param Serializers



        //Parameter t (Main Cancer Type)
        private static void SerializeCancerType(List<String> arr, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.MainType))
            {
                value = searchParams.MainType.Label;
            }
            arr.Add(value);
        }



        // Parameter a (Age)
        private static void SerializeAge(List<String> arr, CTSSearchParams searchParams)
        {
            string value = "none";
            if (searchParams.IsFieldSet(FormFields.Age))
            {
                value = searchParams.Age.ToString();
            }
            arr.Add(value);

        }

        


        #endregion


        #region util methods

        /// <summary>
        /// Logs an error in the ParseErrors array on the Search Params
        /// </summary>
        private void LogParseError(FormFields field, string errorMessage, CTSSearchParams searchParams)
        {
            CTSSearchFieldParamError error = new CTSSearchFieldParamError();
            error.Field = field;
            error.ErrorMessage = errorMessage;
            searchParams.ParseErrors.Add(error);
        }

        /// <summary>
        /// Logs an error in the ParseErrors array on the Search Params
        /// </summary>
        private void LogParseError(string param, string errorMessage, CTSSearchParams searchParams)
        {
            CTSSearchParamError error = new CTSSearchParamError();
            error.Param = param;
            error.ErrorMessage = errorMessage;
            searchParams.ParseErrors.Add(error);
        }

        #endregion









    }
}
