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

namespace NCILibrary.Search.Client
{
    public class SiteWideSearchAPIClient
    {
        static ILog log = LogManager.GetLogger(typeof(SiteWideSearchAPIClient));

        private HttpClient client { get; set; }
        private string SitewideSearchApiUrl { get; set; }
        

        public SiteWideSearchAPIClient(string apiUrl, HttpClient httpClient)
        {
            this.client = httpClient;
            this.SitewideSearchApiUrl = apiUrl;
        }

        /// <summary>
        /// Performs a GET request to the index endpoint (/) of the SiteWideSearch API
        /// </summary>
        /// <param name="collection">The search collection to hit</param>
        /// <param name="language">The language of the search collection</param>
        /// <param name="term">The Search Term</param>
        /// <param name="size"># of results to return (optional)</param>
        /// <param name="from">Beginning index for results (optional)</param>
        /// <param name="site">The site to constrain results to (optional)</param>
        /// <returns>Collection of SiteWideSearchResults</returns>
        public SiteWideSearchResults Search(
            string collection,
            string language,
            string term,
            int size = 10,
            int from = 0,
            string site = "all")
        {
            SiteWideSearchResults rtnResults = null;

            HttpResponseMessage response = null;
            String notFound = "NotFound";

            client.BaseAddress = new Uri(SitewideSearchApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            string url = string.Format(
                "{0}/search/{1}/{2}/{3}?from={4}&size={5}&site={6}",
                SitewideSearchApiUrl,
                collection,
                language,
                term,                
                from.ToString(),
                size.ToString(),
                site
            );

            // We want this to be synchronus, so call Result right away.
            response = client.GetAsync("http://ncias-s1786-v.nci.nih.gov:5008/search/cgov/en/cancer?from=0&size=20&site=all").Result;

            if (response.IsSuccessStatusCode)
            {
                rtnResults = response.Content.ReadAsAsync<SiteWideSearchResults>().Result;
            }
            else
            {
                //TODO: fix this {0} value to be something useful, 
                //response.Content.ReadAsStringAsync().Result returns null on Content.
                string errorMessage = string.Format(
                    "Response: {0} \nAPI path: {1}",
                    response.ReasonPhrase,
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