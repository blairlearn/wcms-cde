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
            context.Response.Write("");    
        }

        #endregion
    }
}
