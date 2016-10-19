using System;
using System.Globalization;
using System.Web;
using Common.Logging;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public class MobileRedirectCookieDropper : IHttpModule
    {
        static ILog log = LogManager.GetLogger(typeof(MobileRedirectCookieDropper));

        // This modile is used on the mobile site to drop a cookie 
        // so mobile redirect will not take place on the desktop site
        // since we are already on the mobile site

        private string _cookieName = "";
        private string _cookieDomain = "";
        private bool _refreshOnPageView = true;
         
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

            _cookieName = ContentDeliveryEngineConfig.MobileRedirector.CookieName.Value;
            _cookieDomain = ContentDeliveryEngineConfig.MobileRedirector.CookieDomain.Value;
            
            string refreshOnPageView = ContentDeliveryEngineConfig.MobileRedirector.RefreshOnPageView.Value;
            bool result; 
            if (bool.TryParse(refreshOnPageView, out result))
                _refreshOnPageView = result;
            else
                _refreshOnPageView = true; //default to true if refreshOnPageView is not a boolean
       
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            //if refreshOnPageView is true, drop a cookie so mobile redirect will never 
            //happen in this session since we are already on the mobile site

            if (string.IsNullOrEmpty(_cookieName))
                log.Error("nci/web/cde/mobileRedirector/cookieName was not found");
            else
            {
                try
                {
                    if (_refreshOnPageView)
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

                        HttpCookie redirectCookie = new HttpCookie(_cookieName, DateTime.Now.Ticks.ToString());
                        redirectCookie.Expires = DateTime.MinValue; // Make this a session cookie
                        redirectCookie.Domain = _cookieDomain;
                        context.Response.Cookies.Add(redirectCookie);
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error dropping mobile redirect cookie");
                    return;
                }
            }
        }
    }
}
