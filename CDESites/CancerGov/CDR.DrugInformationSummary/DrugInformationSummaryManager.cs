using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CDR.Common;
using System.Data;
using NCI.Util;

namespace CDR.DrugInformationSummary
{
    public class DrugInformationSummaryManager : CDRManagerBase 
    {
        /// <summary>
        /// Retrieves a list of new and/or updated DrugInformationSummary documents.
        /// </summary>
        /// <param name="maxReturnCount">Maximum number of items to return</param>
        /// <param name="newOrUpdateFlag">New, Update or Both</param>
        /// <returns>A list of DrugInformationSummaryInfo objects.</returns>


        public List<DrugInformationSummaryInfo> LoadNewOrUpdatedDrugInformationSummaries(
            int maxReturnCount,
            NewOrUpdateStatus newOrUpdateFlag)
        {
            List<DrugInformationSummaryInfo> results = null;

            // Convert the enums to strings.
            string newOrUpdateCriterion = CriterionToSearchString(newOrUpdateFlag);

            // Get the list of summaries.
            DrugInformationSummaryQuery query = new DrugInformationSummaryQuery();
            DataTable data =
                query.LoadNewOrUpdatedDrugInformationSummaries(maxReturnCount, newOrUpdateCriterion);

            if (data != null && data.Rows.Count > 0)
            {
                results = new List<DrugInformationSummaryInfo>();

                // LINQ magic to convert the DataTable object into a collection
                // of SummaryInfo objects.
                results.AddRange(
                    from row in data.AsEnumerable()
                    select new DrugInformationSummaryInfo(
                        Strings.Clean(row["Title"]),
                        Strings.Clean(row["description"]),
                        Strings.ToDateTime(row["PubDate"]),
                        Strings.Clean(row["link"]))
                );
            }

            return results;
        }
    }
}
