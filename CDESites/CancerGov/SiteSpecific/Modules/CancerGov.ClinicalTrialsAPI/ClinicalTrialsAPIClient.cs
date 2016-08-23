using System;
using System.Collections.Generic;
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
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

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
                //TODO: refactor version as string to pass into Post
                HttpResponseMessage response = client.PostAsync("/v1/clinical-trials", new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")).Result;
                
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
                HttpResponseMessage response = client.GetAsync("/v1/clinical-trial/" + id).Result;

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
                client.BaseAddress = new Uri(this.Host);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                JObject requestBody = new JObject();
                requestBody.Add(new JProperty("size", size));
                requestBody.Add(new JProperty("from", from));

                foreach (KeyValuePair<string, object> sp in searchParams)
                {
                    requestBody.Add(new JProperty(sp.Key, sp.Value));
                }


                //We want this to be synchronus, so call Result right away.
                HttpResponseMessage response = client.PostAsync("/terms", new StringContent(requestBody.ToString(), Encoding.UTF8, "application/json")).Result;

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
                HttpResponseMessage response = client.GetAsync("/term/" + key).Result;

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

        


    }
}
