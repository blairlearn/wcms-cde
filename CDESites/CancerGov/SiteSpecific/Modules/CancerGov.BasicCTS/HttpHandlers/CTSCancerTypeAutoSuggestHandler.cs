using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

            BasicCTSManager manager = new BasicCTSManager();

            context.Response.ContentType = "application/json";    
        }

        #endregion
    }
}
