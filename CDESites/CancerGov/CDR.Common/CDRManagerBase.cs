using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDR.Common
{
    /// <summary>
    /// Abstract base class to provide common functionality to Manager classes in the
    /// CancerGov.CDR namespace.
    /// </summary>
    public class CDRManagerBase
    {
        /// <summary>
        /// Converts a NewOrUpdateStatus critierion value to one of
        /// the string values recognized by the database.
        /// </summary>
        /// <param name="critierion">A NewOrUpdateStatus value</param>
        /// <returns>String value to be passed to SummaryQuery.LoadNewOrUpdatedSummaries().</returns>
        protected string CriterionToSearchString(NewOrUpdateStatus critierion)
        {
            string searchString;

            if (Enum.IsDefined(typeof(NewOrUpdateStatus), critierion))
                searchString = critierion.ToString();
            else
                searchString = "Both";

            return searchString;
        }

        /// <summary>
        /// Helper method to convert a CDRLanguage critierion value to one of
        /// the string values recognized by the database.
        /// </summary>
        /// <param name="critierion">A CDRLanguage value</param>
        /// <returns>String value to be passed to SummaryQuery.LoadNewOrUpdatedSummaries().</returns>
        protected string CriterionToSearchString(CDRLanguage critierion)
        {
            string searchString;

            if (Enum.IsDefined(typeof(CDRLanguage), critierion))
                searchString = critierion.ToString();
            else
                searchString = "English";

            return searchString;
        }

    }
}
