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
using Newtonsoft.Json.Linq;

namespace NCI.Search.Client
{
    public class BestBetsAPIClient
    {
        static ILog log = LogManager.GetLogger(typeof(BestBetsAPIClient));

        /// <summary>
        /// Property for the hostname that requests will be sent to.
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// Base path (API version) set in the web.config.
        /// This can also be an empty string
        /// </summary>
        protected string BasePath
        {
            get
            {
                string basepath = ConfigurationManager.AppSettings["BestBetsAPIBasepath"].ToString();
                if (basepath == null)
                {
                    basepath = String.Empty;
                }
                else
                {
                    basepath = "/" + basepath;
                }
                return basepath;
            }
        }

        public BestBetsAPIClient(string host)
        {
            this.Host = host;
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
            BestBetResult[] rtnResults = {};

            HttpResponseMessage response = null;
            String notFound = "NotFound";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string url = string.Format(
                    "{0}/bestbets/{1}/{2}", 
                    BasePath, 
                    language, 
                    term
                ); 

                // We want this to be synchronus, so call Result right away.
                response = client.GetAsync(url).Result;
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
            }

            return rtnResults;

        }
    }
}
