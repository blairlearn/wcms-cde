using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using NCI.Web.CDE.Application;

namespace NCI.Web.CDE.HttpHandlers
{
    public class PageNotFoundHandler : IHttpHandler
    {

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            ErrorPageDisplayer.RaisePageNotFound(this.GetType().ToString());
        }
    }
}
