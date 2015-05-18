using System;
using System.Web;

namespace CancerGov.HttpModules
{
    /// <summary>
    /// This module is to map old clinical trial URLs that were pre-Devon Rex launch
    /// to post devon rex URLs.  The ProtocolSearchIDs have to be remapped for these
    /// searches.
    /// 
    /// The URLs below should be put in configuration and maybe that will be a CTS Phase-1
    /// tasks.
    /// </summary>
    public class LegacyCTSResultRedirector : IHttpModule
    {
        /// <summary>
        /// 
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            return;
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
        }

        #endregion

        void OnBeginRequest(object sender, EventArgs e)
        {
            //First we need to load the URL map.
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);

            //If this is the homepage, then exit.
            if (url == "/")
                return;

            //Chop off any trailing slashes
            if (url.EndsWith("/"))
                url = url.Substring(0, url.Length - 1);

            //Check if it is one of the URLs to remap
            if (
                url.Equals("/clinicaltrials/search/results", StringComparison.OrdinalIgnoreCase) ||
                url.Equals("/clinicaltrials/search/printresults", StringComparison.OrdinalIgnoreCase)
                )
            {
                string psid = context.Request.QueryString["protocolsearchid"];

                //Check if the PSID is empty or not - if so, this is not something
                //we can redirect, so just return.
                if (string.IsNullOrEmpty(psid))
                    return;

                //if redirect map contains A, then redirect.
                CachableProtocolSearchIDMap map = CachableProtocolSearchIDMap.GetMap();

                if (map.Contains(psid))
                {
                    //redirect 
                    DoPermanentRedirect(context.Response, string.Format("/about-cancer/treatment/clinical-trials/search/results?protocolsearchid={0}", map[psid]));
                }

            }

        }

        /// <summary>
        /// Clears the Response text, issues an HTTP redirect using status 301, and ends
        /// the current request.
        /// </summary>
        /// <param name="Response">The current response object.</param>
        /// <param name="url">The redirection's target URL.</param>
        /// <remarks>Response.Redirect() issues its redirect with a 301 (temporarily moved) status code.
        /// We want these redirects to be permanent so search engines will link to the new
        /// location. Unfortunately, HttpResponse.RedirectPermanent() isn't implemented until
        /// at version 4.0 of the .NET Framework.</remarks>
        /// <exception cref="ThreadAbortException">Called when the redirect takes place and the current
        /// request is ended.</exception>
        private void DoPermanentRedirect(HttpResponse Response, String url)
        {
            Response.Clear();
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", url);
            Response.End();
        }
    }
}
