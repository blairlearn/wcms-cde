using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Common.Logging;

namespace NCI.Web.CDE.Application
{
    /// <summary>
    /// This is a helper class for doing all permanent redirects
    /// </summary>
    public class PermanentRedirector
    {
        static ILog log = LogManager.GetLogger(typeof(PermanentRedirector));

        /// <summary>
        /// Display the "ErrorPage" page and a status of 500.
        /// </summary>
        /// <param name="callingClass">The name of the class that called this - for logging.</param>
        public static void DoPermanentRedirect(HttpResponse response, string redirectUrl, string redirectReason)
        {
            // Set the cache control to not store - needed for analytics
            if (HttpContext.Current.Response.Headers["Cache-Control"] != null)
            {
                if(!HttpContext.Current.Response.Headers["Cache-Control"].Contains("no-store"))
                {
                    HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
                }
            }
            else
            {
                HttpContext.Current.Response.AddHeader("Cache-Control", "no-store");
            }

            // Set the redirect reason header
            HttpContext.Current.Response.AddHeader("X-Redirect-Reason", redirectReason);

            // Do permanent redirect
            response.RedirectPermanent(redirectUrl, true);
        }
    }
}
