using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDR.Common;
using System.Data;
using NCI.Util;

namespace CDR.Summary
{
    public class SummaryManager : CDRManagerBase
    {
        /// <summary>
        /// Retrieves a list of new and/or updated Summary documents.
        /// </summary>
        /// <param name="maxReturnCount">Maximum number of items to return</param>
        /// <param name="audience">Patient or HealthProfessional</param>
        /// <param name="type">All, Treatment, SupportiveCare, CAM, Genetics,
        /// Prevention or Screening</param>
        /// <param name="newOrUpdateFlag">New, Update or Both</param>
        /// <param name="language">English or Spanish</param>
        /// <returns>A list of SummaryInfo objects.</returns>

        public List<SummaryInfo> LoadNewOrUpdatedSummaries(
            int maxReturnCount,
            TargetAudience audience,
            SummaryType type,
            NewOrUpdateStatus newOrUpdateFlag,
            CDRLanguage language)
        {
            List<SummaryInfo> results = null;

            // Convert the enums to strings.
            string audienceCriterion = CriterionToSearchString(audience);
            string typeCriterion = CriterionToSearchString(type);
            string newOrUpdateCriterion = CriterionToSearchString(newOrUpdateFlag);
            string languageCriterion = CriterionToSearchString(language);

            // Get the list of summaries.
            SummaryQuery query = new SummaryQuery();
            DataTable data =
                query.LoadNewOrUpdatedSummaries(maxReturnCount, audienceCriterion, typeCriterion, newOrUpdateCriterion, languageCriterion);

            if (data != null && data.Rows.Count > 0)
            {
                results = new List<SummaryInfo>();

                // LINQ magic to convert the DataTable object into a collection
                // of SummaryInfo objects.
                results.AddRange(
                    from row in data.AsEnumerable()
                    select new SummaryInfo(
                        Strings.Clean(row["Title"]),
                        Strings.Clean(row["description"]),
                        Strings.ToDateTime(row["PubDate"]),
                        Strings.Clean(row["Category"]),
                        Strings.Clean(row["link"]))
                );
            }

            return results;
        }

        #region Helpers

        /// <summary>
        /// Helper method to convert a TargetAudience critierion value to one of
        /// the string values recognized by the database.
        /// </summary>
        /// <param name="critierion">A TargetAudience value</param>
        /// <returns>String value to be passed to SummaryQuery.LoadNewOrUpdatedSummaries().</returns>
        private string CriterionToSearchString(TargetAudience critierion)
        {
            string searchString;

            if (critierion == TargetAudience.Patient)
                searchString = "Patients";
            else
                searchString = "Health professionals";

            return searchString;
        }

        /// <summary>
        /// Helper method to convert a SummaryType critierion value to one of
        /// the string values recognized by the database.
        /// </summary>
        /// <param name="critierion">A SummaryType value</param>
        /// <returns>String value to be passed to SummaryQuery.LoadNewOrUpdatedSummaries().</returns>
        private string CriterionToSearchString(SummaryType critierion)
        {
            string searchString;

            switch (critierion)
            {
                case SummaryType.All:
                    searchString = "All";
                    break;
                case SummaryType.Treatment:
                    searchString = "Treatment";
                    break;
                case SummaryType.SupportiveCare:
                    searchString = "Supportive care";
                    break;
                case SummaryType.CAM:
                    searchString = "Complementary and alternative medicine";
                    break;
                case SummaryType.Genetics:
                    searchString = "Genetics";
                    break;
                case SummaryType.Prevention:
                    searchString = "Prevention";
                    break;
                case SummaryType.Screening:
                    searchString = "Screening";
                    break;
                default:
                    searchString = "All";
                    break;
            }

            return searchString;
        }

        #endregion


    }
}
