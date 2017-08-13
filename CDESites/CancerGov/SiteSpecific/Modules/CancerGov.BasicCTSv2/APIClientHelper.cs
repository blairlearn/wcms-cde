using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    /// <summary>
    /// Helper class get getting an API client instance.
    /// </summary>
    public static class APIClientHelper
    {
        /// <summary>
        /// Gets an instance of a v1 CTAPI client
        /// </summary>
        /// <returns></returns>
        public static ClinicalTrialsAPIClient GetV1ClientInstance()
        {
            string baseApiPath = BasicClinicalTrialSearchAPISection.GetAPIUrl();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseApiPath);

            return new ClinicalTrialsAPIClient(client);
        }   
    }
}
