using System;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using System.IO;
//using NCI.Web.CDE.InformationRequest;
using NCI.Web.CDE;
using NCI.Web.CDE.Configuration;
//using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Logging;
using System.Reflection;

namespace NCI.Web.CDE
{
    public class MobileRedirectCookieDropper : IHttpModule
    {
        // This modile is used on the mobile site to drop a cookie 
        // so mobile redirect will not take place on the desktop site
        // since we are already on the mobile site

        private static readonly string _cookieName =
            System.Configuration.ConfigurationManager.AppSettings["MobileRedirectCookieName"];
        
        /// <summary>
        /// You will need to configure this module in the web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>

        public void Dispose()
        {
            //clean-up code here.
        }

        /// <summary>
        /// Instantiated  event OnBeginRequest
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            //drop a cookie so mobile redirect will never happen in this session 
            //since we are already on the mobile site

            if (string.IsNullOrEmpty(_cookieName))
                Logger.LogError("CDE:MobileRedirectCookieDropper.cs", "App Setting MobileRedirectCookieName was not found", NCIErrorLevel.Error);
            else
            {
                try
                {
                    HttpContext context = ((HttpApplication)sender).Context;
                    String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture));

                    if (url.IndexOf(".ico") != -1 ||
                        url.IndexOf(".css") != -1 ||
                        url.IndexOf(".gif") != -1 ||
                        url.IndexOf(".jpg") != -1 ||
                        url.IndexOf(".js") != -1 ||
                        url.IndexOf(".axd") != -1 ||
                        url.IndexOf(".png") != -1)
                        return;

                    HttpCookie redirectCookie = new HttpCookie(_cookieName, "true");
                    redirectCookie.Expires = DateTime.MinValue; // Make this a session cookie
                    redirectCookie.Domain = "cancer.gov";
                    context.Response.Cookies.Add(redirectCookie);
                }
                catch (Exception ex)
                {
                    Logger.LogError("CDE:MobileRedirectCookieDropper.cs", "Error dropping mobile redirect cookie", NCIErrorLevel.Error,ex);
                    return;
                }
            }
        }
    }
}
