using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CancerGov.ClinicalTrialsAPI
{
    public class ClinicalTrialsAPIClient
    {
        public string Host { get; private set; }

        /// <summary>
        /// Base path (API version) set in the web.config.
        /// This can also be an empty string
        /// </summary>
        protected string BasePath
        {
            get
            {
                string basepath = ConfigurationManager.AppSettings["ClinicalTrialsAPIBasepath"].ToString();
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
        /// <param name="size"></param>
        /// <param name="from"></param>
        /// <param name="includeFields"></param>
        /// <param name="excludeFields"></param>
        /// <returns></returns>
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

            using (var client = new HttpClient())
            {
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

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = Post(client, "clinical-trials", requestBody);
                if (response.IsSuccessStatusCode)
                {
                    rtnResults = response.Content.ReadAsAsync<ClinicalTrialsCollection>().Result;
                }
                else
                {
                    //TODO: Add more checking here if the respone does not actually have any content
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(errorMessage);
                }
            }

            return rtnResults;

        }

        /// <summary>
        /// Calls the listing endpoint (/clinical-trials) of the clinical trials API
        /// Overload for trial listing pages
        /// </summary>
        /// <param name="size"></param>
        /// <param name="from"></param>
        /// <param name="includeFields"></param>
        /// <param name="excludeFields"></param>
        /// <param name="dynamicSearchParams"></param>
        /// <returns></returns>
        public ClinicalTrialsCollection FilteredList(
            int size = 10, 
            int from = 0, 
            string[] includeFields = null, 
            string[] excludeFields = null,
            Dictionary<string, object> searchParams = null,
            string dynamicSearchParams = null
            )
        {
            ClinicalTrialsCollection rtnResults = null;

            //Handle null fields
            searchParams = searchParams ?? new Dictionary<string, object>();
            dynamicSearchParams = dynamicSearchParams ?? String.Empty;

            using (var client = new HttpClient())
            {
                JObject requestBody = new JObject();
                requestBody.Add(new JProperty("size", size));
                requestBody.Add(new JProperty("from", from));

                foreach (KeyValuePair<string, object> sp in searchParams)
                {
                    requestBody.Add(new JProperty(sp.Key, sp.Value));
                }

                //Add dynamic filter criteria
                if (!String.IsNullOrEmpty(dynamicSearchParams))
                {
                    Dictionary<string, object> dynFilters = JsonConvert.DeserializeObject<Dictionary<string, object>>(dynamicSearchParams);
                    foreach (KeyValuePair<string, object> dynFilter in dynFilters)
                    {
                        requestBody.Add(new JProperty(dynFilter.Key, dynFilter.Value));
                    }
                }

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = Post(client, "clinical-trials", requestBody);
                if (response.IsSuccessStatusCode)
                {
                    rtnResults = response.Content.ReadAsAsync<ClinicalTrialsCollection>().Result;
                }
                else
                {
                    //TODO: Add more checking here if the respone does not actually have any content
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(errorMessage);
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
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = client.GetAsync(BasePath + "/clinical-trial/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    rtnTrial = response.Content.ReadAsAsync<ClinicalTrial>().Result;                    
                }
                else
                {
                    //TODO: Add more checking here if the respone does not actually have any content
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(errorMessage);
                }
            }

            return rtnTrial;
        }


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

            using (var client = new HttpClient())
            {
                JObject requestBody = new JObject();
                requestBody.Add(new JProperty("size", size));
                requestBody.Add(new JProperty("from", from));

                foreach (KeyValuePair<string, object> sp in searchParams)
                {
                    requestBody.Add(new JProperty(sp.Key, sp.Value));
                }

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = Post(client, "terms", requestBody);
                if (response.IsSuccessStatusCode)
                {
                    rtnResults = response.Content.ReadAsAsync<TermCollection>().Result;
                }
                else
                {
                    //TODO: Add more checking here if the respone does not actually have any content
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(errorMessage);
                }
            }

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

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = client.GetAsync(BasePath + "/term/" + key).Result;

                if (response.IsSuccessStatusCode)
                {
                    rtnTerm = response.Content.ReadAsAsync<Term>().Result;
                }
                else
                {
                    //TODO: Add more checking here if the respone does not actually have any content
                    string errorMessage = response.Content.ReadAsStringAsync().Result;
                    throw new Exception(errorMessage);
                }
            }

            return rtnTerm;
        }

        /// <summary>
        /// Gets the Post request message 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="path"></param>
        /// <param name="requestBody"></param>
        /// <returns>reponse</returns>
        public HttpResponseMessage Post(HttpClient client, String path, JObject requestBody)
        {
            client.BaseAddress = new Uri(this.Host);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsync(BasePath + "/" + path, new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")).Result;
            return response;
        }

    }
}
