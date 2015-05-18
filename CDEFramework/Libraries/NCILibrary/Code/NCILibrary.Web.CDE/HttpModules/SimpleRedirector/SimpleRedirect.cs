using System;
using System.Web;

using NCI.Web.CDE.SimpleRedirector;
using NCI.Web.CDE.SimpleRedirector.Configuration;

namespace NCI.Web.CDE
{
    /// <summary>
    /// Simple tool for redirecting URLs based on a comma-delimited list of old and new URLs.
    /// </summary>
    public class SimpleRedirect : IHttpModule
    {
        static Log log = new Log(typeof(SimpleRedirect));

        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
        }

        #endregion

        /// <summary>
        /// The guts of the HTTP module.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);
            String query= context.Server.UrlDecode(context.Request.Url.Query);
            // Can we ignore this file?
            if (Utility.IgnoreWebResource(url))
                return;

            // If the PageAssemblyInstruction is not null, then a page is being served
            // and we can ignore this request.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            String redirectUrl = GetRedirectUrl(url, context);

            if (query != "")
            {
                redirectUrl = redirectUrl + query;
            }

            if(!String.IsNullOrEmpty(redirectUrl))
                DoPermanentRedirect(context.Response, redirectUrl);
        }

        /// <summary>
        /// Private helper to find the redirection URL mapped to a given input URL.
        /// </summary>
        /// <param name="url">The URL to examine for a replacement</param>
        /// <param name="context"></param>
        /// <returns></returns>
        private String GetRedirectUrl(String url, HttpContext context)
        {
            String redirect = null;

            if (url.LastIndexOf("/") == url.Length-1)
            {
                url = url.Substring(0, url.Length - 1);
            }

            SimpleRedirectorConfigurationSection config = SimpleRedirectorConfigurationSection.Get();

            String datafile = HttpContext.Current.Server.MapPath(config.DataSource.DataFile);
            RedirectionMap urlMap = RedirectionMap.GetMap(datafile, context);

            if (urlMap.Contains(url))
            {
                if (urlMap.ContainsMultiple(url))
                {
                    log.debug(String.Format("Url: '{0}' has multiple instances in redirect map.", url));
                }
                redirect = urlMap[url];
                log.debug(String.Format("Url '{0}' found; redirects to '{1}'.", url, redirect));
            }
            else
            {
                log.debug(String.Format("No match found for url '{0}.", url));
            }
            return redirect;
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
