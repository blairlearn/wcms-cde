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
    public class SiteWideSearchAPIClient
    {
        static ILog log = LogManager.GetLogger(typeof(SiteWideSearchAPIClient));

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
                string basepath = ConfigurationManager.AppSettings["SiteWideSearchAPIBasepath"].ToString();
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

        public SiteWideSearchAPIClient(string host)
        {
            this.Host = host;
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

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                string url = string.Format(
                    "{0}/search/{1}/{2}/{3}?from={4}&size={5}&site={6}", 
                    BasePath,
                    collection,
                    language,
                    term,
                    size.ToString(),
                    from.ToString(),
                    site
                ); 

                // We want this to be synchronus, so call Result right away.
                response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    rtnResults = response.Content.ReadAsAsync<SiteWideSearchResults>().Result;
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
