using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace CancerGov.ClinicalTrialsAPI
{
    public class ClinicalTrialsAPIClient
    {
        public string Host { get; private set; }

        public ClinicalTrialsAPIClient(string host)
        {
            this.Host = host;
        }

        /// <summary>
        /// When Performing a Search we Need to Limit the Fields We Retrieve.
        /// </summary>
        private string[] GetListOfResultsFields()
        {
            return new string[] {
                "nct_id",
                "brief_title",
                "sites.org.name",
                "sites.org.postal_code",
                "eligibility.structured",
                "current_trial_status"
            };
        }

        /// <summary>
        /// When Performing a Search we Need to Limit the Statuses we will search.
        /// </summary>
        /*
        private string[] GetListOfActiveStatuses()
        {
 Closed to Accrual and Intervention
 In Review
 Temporarily Closed to Accrual and Intervention
 Administratively Complete
 Temporarily Closed to Accrual
 Enrolling by Invitation
 Closed to Accrual
 Active
 Complete
 Withdrawn
 Approved
        }
         */

        /// <summary>
        /// Calls the listing endpoint (/clinical-trials) of the clinical trials API
        /// </summary>
        /// <param name="size"></param>
        /// <param name="from"></param>
        /// <param name="includeFields"></param>
        /// <param name="excludeFields"></param>
        /// <returns></returns>
        public ClinicalTrialsCollection List(int size = 10, int from = 0, string[] includeFields = null, string[] excludeFields = null)
        {
            ClinicalTrialsCollection rtnResults = null;

            //Handle Null include/exclude fields
            includeFields = includeFields ?? new string[0];
            excludeFields = excludeFields ?? new string[0];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://" + this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = client.GetAsync("/clinical-trials").Result;

                if (response.IsSuccessStatusCode)
                {
                    rtnResults = response.Content.ReadAsAsync<ClinicalTrialsCollection>().Result;
                }
            }

            return rtnResults;


        }

        /// <summary>
        /// Gets a clinical trial from the API via its ID.
        /// </summary>
        /// <param name="id">Either the NCI ID or the NCT ID</param>
        /// <returns>The clinical trial</returns>
        public ClinicalTrial Get(string id) {

            ClinicalTrial rtnTrial = null;

            if (String.IsNullOrWhiteSpace(id)) {
                throw new ArgumentNullException("The trial identifier is null or an empty string");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://" + this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = client.GetAsync("/clinical-trial/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    rtnTrial = response.Content.ReadAsAsync<ClinicalTrial>().Result;                    
                }
            }

            return rtnTrial;
        }

        


    }
}
