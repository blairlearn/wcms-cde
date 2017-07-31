using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrialsAPI
{
    interface IClinicalTrialsAPIClient
    {
        /// <summary>
        /// Calls the listing endpoint (/clinical-trials) of the clinical trials API
        /// </summary>
        /// <param name="size"># of results to return (optional)</param>
        /// <param name="from">Beginning index for results (optional)</param>
        /// <param name="includeFields">Fields to include (optional)</param>
        /// <param name="excludeFields">Fields to exclude (optional)</param>
        /// <param name="searchParams">Search parameters (optional)</param>
        /// <returns>Collection of Clinical Trials</returns>
        ClinicalTrialsCollection List(
            int size = 10,
            int from = 0,
            string[] includeFields = null,
            string[] excludeFields = null,
            Dictionary<string, object> searchParams = null
            );
        
        /// <summary>
        /// Calls the listing endpoint (/clinical-trials) of the clinical trials API using dynamically
        /// created search params (see Trial Listing pages)
        /// </summary>
        /// <param name="size"># of results to return</param>
        /// <param name="from">Beginning index for results</param>
        /// <param name="searchParams">Default search parameters</param>
        /// <param name="dynamicSearchParams">Dynamic search parameters</param>
        /// <returns>Collection of Clinical Trials</returns>
        ClinicalTrialsCollection GetTrialsList(int size, int from, Dictionary<string, object> searchParams, JObject dynamicSearchParams);

        /// <summary>
        /// Gets a clinical trial from the API via its ID.
        /// </summary>
        /// <param name="id">Either the NCI ID or the NCT ID</param>
        /// <returns>The clinical trial</returns>
        ClinicalTrial Get(string id);

        /// <summary>
        /// Gets a collection of terms from the API.
        /// </summary>
        /// <param name="size"># of results to return (optional)</param>
        /// <param name="from">Beginning index for results (optional)</param>
        /// <param name="searchParams">Default search parameters (optional)</param>
        /// <returns>Collection of terms</returns>
        TermCollection Terms(
            int size = 10,
            int from = 0,
            //string[] includeFields = null, 
            //string[] excludeFields = null,
            Dictionary<string, object> searchParams = null
        );

        /// <summary>
        /// Gets a term from the API via its key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>The term</returns>
        Term GetTerm(string key);


    }
}
