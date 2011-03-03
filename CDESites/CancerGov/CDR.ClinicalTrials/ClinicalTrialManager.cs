using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using NCI.Util;

namespace CancerGov.CDR.ClinicalTrials
{
    public class ClinicalTrialManager
    {
        // This wants to be a const, but const can only be value types, and
        // arrays are references.
        readonly static string[] CATEGORY_DELMITER = {","};

        /// <summary>
        /// Loads a list consisting of only new and active clinical trials.
        /// </summary>
        /// <param name="maxReturnCount">Controls the maximum number of results returned.</param>
        /// <returns>A list of clinical trials objects with the newest first.</returns>
        public List<ClinicalTrialInfo> LoadNewAndActiveProtocols(int maxReturnCount)
        {
            List<ClinicalTrialInfo> results = new List<ClinicalTrialInfo>();

            ClinicalTrialQuery query = new ClinicalTrialQuery();
            DataTable data = query.LoadNewAndActiveProtocols(maxReturnCount);

            // LINQ magic to convert the DataTable object into a collection
            // of ClinicalTrialInfo objects.
            results.AddRange(
                from row in data.AsEnumerable()
                select BuildClinicalTrialInfo(
                    Strings.Clean(row["HealthProfessionaltitle"]),
                    Strings.Clean(row["description"]),
                    Strings.ToDateTime(row["PubDate"]),
                    Strings.Clean(row["Category"]),
                    Strings.Clean(row["link"]))
                );

            return results;
        }

        /// <summary>
        /// Encapsulates the logic for creating a ClinicalTrialInfo by converting the
        /// delimited list of categories into a List of strings.
        /// </summary>
        /// <param name="healthProfessionalTitle">Title for the health professional version of the clinicial trial</param>
        /// <param name="description">Description of the clinical trial</param>
        /// <param name="publicationDate">Date the trial was first published</param>
        /// <param name="categories">Delimited list of categories for the trial.  The delimeter
        /// is defined as CATEGORY_DELMITER.</param>
        /// <param name="prettyUrlID">URL version of the protocol's primary ID.</param>
        /// <returns>ClinicalTrialInfo</returns>
        private static ClinicalTrialInfo BuildClinicalTrialInfo(
            string healthProfessionalTitle,
            string description,
            DateTime publicationDate,
            string categories,
            string prettyUrlID)
        {
            List<string> categoryList = null;

            // If there are categories, split them up, and trim the whitespace before
            // adding to the category list.
            if (!string.IsNullOrEmpty(categories))
            {
                categoryList = new List<string>();
                Array.ForEach(categories.Split(CATEGORY_DELMITER, StringSplitOptions.RemoveEmptyEntries),
                    entry => categoryList.Add( entry.Trim()));
            }

            return new ClinicalTrialInfo(
                    healthProfessionalTitle,
                    description,
                    publicationDate,
                    categoryList,
                    prettyUrlID
                );
        }
    }
}
