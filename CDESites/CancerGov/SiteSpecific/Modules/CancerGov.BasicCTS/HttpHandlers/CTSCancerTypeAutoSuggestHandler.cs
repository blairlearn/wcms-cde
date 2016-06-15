using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CancerGov.ClinicalTrials.Basic.HttpHandlers
{
    public class CTSCancerTypeAutoSuggestHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string query = context.Request.Params["q"];

            //Handle this as a 404.
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentException("Query Must Not be Null or Empty");

            query = query.ToLower();

            BasicCTSManager manager = new BasicCTSManager();

            Regex startsPattern = new Regex("^" + query, RegexOptions.IgnoreCase);
            Regex containsPattern = new Regex("\\b" + query, RegexOptions.IgnoreCase);

            var response = from suggestion in manager.GetCancerTypeSuggestions(query)
                           where startsPattern.IsMatch(suggestion.Name) || containsPattern.IsMatch(suggestion.Name) 
                           orderby startsPattern.IsMatch(suggestion.Name) descending
                           select new {
                               term = suggestion.Name,
                               id = suggestion.CDRID + "|" + suggestion.Hash
                           };

            context.Response.ContentType = "application/json";            
            context.Response.Write(JsonConvert.SerializeObject(response));

        }

        #endregion
    }
}
