using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;

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
            PageNotFoundHandler.RaisePageNotFound();
        }

        /// <summary>
        /// Raise a Page Not Found Page.  This can't be in the error page displayer as it would 
        /// be a circular reference.  
        /// </summary>
        /// <param name="callingClass"></param>
        public static void RaisePageNotFound()
        {
            string systemMessagePageUrl;

            systemMessagePageUrl = ConfigurationSettings.AppSettings["NotFoundPage"];

            //Bypass the custom error messages and return our content
            HttpContext.Current.Response.TrySkipIisCustomErrors = true;

            //Setup the 404 response code.
            HttpContext.Current.Response.StatusCode = 404;

            //System.IO.StringWriter writer = new System.IO.StringWriter();
            HttpContext.Current.Server.Execute(systemMessagePageUrl);

            HttpContext.Current.Response.End();
        }

    }
}
