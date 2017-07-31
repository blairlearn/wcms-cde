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

namespace CancerGov.ClinicalTrialsAPI
{
    public class ClinicalTrialsAPIClient : IClinicalTrialsAPIClient
    {
        static ILog log = LogManager.GetLogger(typeof(ClinicalTrialsAPIClient));

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
                string basepath = ConfigurationManager.AppSettings["ClinicalTrialsAPIBasepath"];
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

        public ClinicalTrialsAPIClient(string host)
        {
            this.Host = host;
        }

        /// <summary>
        /// Calls the listing endpoint (/clinical-trials) of the clinical trials API
        /// </summary>
        /// <param name="size"># of results to return (optional)</param>
        /// <param name="from">Beginning index for results (optional)</param>
        /// <param name="includeFields">Fields to include (optional)</param>
        /// <param name="excludeFields">Fields to exclude (optional)</param>
        /// <param name="searchParams">Search parameters (optional)</param>
        /// <returns>Collection of Clinical Trials</returns>
        public ClinicalTrialsCollection List(
            int size = 10, 
            int from = 0, 
            string[] includeFields = null, 
            string[] excludeFields = null,
            Dictionary<string, object> searchParams = null
            )
        {
            ClinicalTrialsCollection rtnResults = null;

            //Handle Null include/exclude field
            includeFields = includeFields ?? new string[0];
            excludeFields = excludeFields ?? new string[0];
            searchParams = searchParams ?? new Dictionary<string, object>();

            JObject requestBody = new JObject();
            requestBody.Add(new JProperty("size", size));
            requestBody.Add(new JProperty("from", from));

            if (includeFields.Length > 0)
            {
                requestBody.Add(new JProperty("include", includeFields));
            }

            if (excludeFields.Length > 0)
            {
                requestBody.Add(new JProperty("exclude", includeFields));
            }

            foreach (KeyValuePair<string, object> sp in searchParams)
            {
                requestBody.Add(new JProperty(sp.Key, sp.Value));
            }

            //Get the HTTP response content from POST request
            HttpContent httpContent = ReturnPostRespContent("clinical-trials", requestBody);
            rtnResults = httpContent.ReadAsAsync<ClinicalTrialsCollection>().Result;

            return rtnResults;

        }

        /// <summary>
        /// Calls the listing endpoint (/clinical-trials) of the clinical trials API using dynamically
        /// created search params (see Trial Listing pages)
        /// </summary>
        /// <param name="size"># of results to return</param>
        /// <param name="from">Beginning index for results</param>
        /// <param name="searchParams">Default search parameters</param>
        /// <param name="dynamicSearchParams">Dynamic search parameters</param>
        /// <returns>Collection of Clinical Trials</returns>
        public ClinicalTrialsCollection GetTrialsList(int size, int from, Dictionary<string, object> searchParams, JObject dynamicSearchParams)
        {
            ClinicalTrialsCollection rtnResults = null;

            //Handle null fields
            searchParams = searchParams ?? new Dictionary<string, object>();
            dynamicSearchParams = dynamicSearchParams ?? new JObject();

                JObject requestBody = new JObject();
                requestBody.Add(new JProperty("size", size));
                requestBody.Add(new JProperty("from", from));

                //Add common filter criteria to request
                foreach (KeyValuePair<string, object> sp in searchParams)
                {
                    requestBody.Add(new JProperty(sp.Key, sp.Value));
                }

                //Add dynamic filter criteria to request
                if (dynamicSearchParams != null)
                {
                    //Merge dynamic and common filters (dynamic values override common)
                    requestBody.Merge(dynamicSearchParams, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Concat });
                }

                //Get the HTTP response content from POST request
                HttpContent httpContent = ReturnPostRespContent("clinical-trials", requestBody);
                rtnResults = httpContent.ReadAsAsync<ClinicalTrialsCollection>().Result;

            return rtnResults;

        }

        /// <summary>
        /// Gets a clinical trial from the API via its ID.
        /// </summary>
        /// <param name="id">Either the NCI ID or the NCT ID</param>
        /// <returns>The clinical trial</returns>
        public ClinicalTrial Get(string id) {

            ClinicalTrial rtnTrial = null;

            if (String.IsNullOrWhiteSpace(id)) 
            {
                throw new ArgumentNullException("The trial identifier is null or an empty string");
            }

            //Get the HTTP response content from GET request
            HttpContent httpContent = ReturnGetRespContent("clinical-trial", id);
            rtnTrial = httpContent.ReadAsAsync<ClinicalTrial>().Result;

            return rtnTrial;
        }

        /// <summary>
        /// Gets a collection of terms from the API.
        /// </summary>
        /// <param name="size"># of results to return (optional)</param>
        /// <param name="from">Beginning index for results (optional)</param>
        /// <param name="searchParams">Default search parameters (optional)</param>
        /// <returns>Collection of terms</returns>
        public TermCollection Terms(
            int size = 10, 
            int from = 0, 
            //string[] includeFields = null, 
            //string[] excludeFields = null,
            Dictionary<string, object> searchParams = null
        )
        {
            TermCollection rtnResults = null;

            searchParams = searchParams ?? new Dictionary<string, object>();

            JObject requestBody = new JObject();
            requestBody.Add(new JProperty("size", size));
            requestBody.Add(new JProperty("from", from));

            foreach (KeyValuePair<string, object> sp in searchParams)
            {
                requestBody.Add(new JProperty(sp.Key, sp.Value));
            }

            //Get the HTTP response content from our Post request
            HttpContent httpContent = ReturnPostRespContent("terms", requestBody);
            rtnResults = httpContent.ReadAsAsync<TermCollection>().Result;

            return rtnResults;
        }

        /// <summary>
        /// Gets a term from the API via its key.
        /// </summary>
        /// <param name="key">the key</param>
        /// <returns>The term</returns>
        public Term GetTerm(string key)
        {
            Term rtnTerm = null;

            if (String.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("The term key is null or an empty string");
            }

            //Get the HTTP response content from GET request
            HttpContent httpContent = ReturnGetRespContent("term", key);
            rtnTerm = httpContent.ReadAsAsync<Term>().Result;

            return rtnTerm;
        }

        /// <summary>
        /// Gets the response content of a GET request.
        /// </summary>
        /// <param name="path">Path for client address</param>
        /// <param name="param">Param in URL</param>
        /// <returns>HTTP response content</returns>
        public HttpContent ReturnGetRespContent(String path, String param)
        {
            HttpResponseMessage response = null;
            HttpContent content = null;
            String notFound = "NotFound";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                // We want this to be synchronus, so call Result right away.
                response = client.GetAsync(BasePath + "/" + path + "/" + param).Result;
                if (response.IsSuccessStatusCode)
                {
                    content = response.Content;
                }
                else
                {
                    string errorMessage = "Response: " + response.Content.ReadAsStringAsync().Result + "\nAPI path: " + BasePath + "/" + path + "/" + param;
                    if (response.StatusCode.ToString() == notFound)
                    {
                        // If trial is not found, log 404 message and return content as null
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
            return content;
        }


        /// <summary>
        /// Gets the response content of a POST request.
        /// </summary>
        /// <param name="path">Path for client address</param>
        /// <param name="request">Params passed in with request body</param>
        /// <returns>HTTP response content</returns>
        public HttpContent ReturnPostRespContent(String path, JObject requestBody)
        {
            HttpResponseMessage response = null;
            HttpContent content = null;

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //We want this to be synchronus, so call Result right away.
                response = client.PostAsync(BasePath + "/" + path, new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    content = response.Content;
                }
                else
                {
                    //TODO: Add more checking here if the respone does not actually have any content
                    string errorMessage = "Response: " + response.Content.ReadAsStringAsync().Result + "\nAPI path: " + BasePath + "/" + path;
                    throw new Exception(errorMessage);
                }
            }
            return content;
        }


    }
}
