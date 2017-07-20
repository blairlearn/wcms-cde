using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using Common.Logging;

using Newtonsoft.Json;

namespace NCILibrary.Search.Client
{
    public class BestBetsAPIClient
    {
        static ILog log = LogManager.GetLogger(typeof(BestBetsAPIClient));

        private HttpClient HttpClient { get; set; }
        private string BestBetsApiUrl { get; set; }
        

        public BestBetsAPIClient(string apiUrl, HttpClient httpClient)
        {
            this.BestBetsApiUrl = apiUrl;
            this.HttpClient = httpClient;
        }

        /// <summary>
        /// Performs a GET request to the index endpoint (/) of the BestBets API
        /// </summary>
        /// <param name="language">The language of the search collection</param>
        /// <param name="term">The Search Term</param>
        /// <returns>Collection of SiteWideSearchResults</returns>
        public BestBetResult[] Search(
            string language,
            string term)
        {
            BestBetResult[] rtnResults = { };

            HttpResponseMessage response = null;
            String notFound = "NotFound";


            HttpClient.BaseAddress = new Uri(BestBetsApiUrl);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string url = string.Format(
                "{0}/bestbets/{1}/{2}",
                BestBetsApiUrl,
                language,
                term
            );

            // We want this to be synchronus, so call Result right away.
            response = HttpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                rtnResults = response.Content.ReadAsAsync<BestBetResult[]>().Result;
            }
            else
            {
                string errorMessage = string.Format(
                    "Response: {0} \nAPI path: {1}",
                    response.Content.ReadAsStringAsync().Result,
                    url);

                if (response.StatusCode.ToString() == notFound)
                {
                    // If results are not found, log 404 message and return content as null
                    log.Debug(errorMessage);
                }
                else
                {
                    // If response is other error message, log and throw exception
                    log.Error(errorMessage);
                    throw new Exception(errorMessage);
                }
            }
            

            return rtnResults;

        }
    }
}