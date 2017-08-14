using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Helper class to create page-load analytics values for CTS pages.
    /// </summary>
    class CTSWebAnalyticsHelper
    {

        /**TODO:
         * Create unit tests
         * Clean up static.cancer.gov files
         */
        /// See serialize methods from param factory as example:
        ///
        //private static void SerializeTrialTypes(NciUrl url, CTSSearchParams searchParams)
        //{
        //    if (searchParams.IsFieldSet(FormFields.TrialTypes))
        //    {
        //        url.QueryParameters.Add(
        //            "tt",
        //            string.Join(",", searchParams.TrialTypes.Select(tp => tp.Key))
        //        );
        //    }
        //}
    }
}
