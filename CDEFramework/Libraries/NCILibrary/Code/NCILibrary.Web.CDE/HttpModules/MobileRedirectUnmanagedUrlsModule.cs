using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using NCI.Web.CDE.InformationRequest;
using System.Threading;


namespace NCI.Web.CDE
{
    public class MobileRedirectUnmanagedUrlsModule : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.PostLogRequest += new System.EventHandler(OnEndRequest);
        }

        public void OnEndRequest(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;
            Exception exception = context.Server.GetLastError();
            String key = context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture);
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture));
            string host = "http://localhost:7001";

            if (context.Response.StatusCode == 404)
            {
                if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                    return;

                string urlout = host + context.Request.RawUrl + "?Information__Request=mobileurl";
                WebRequest wr = WebRequest.Create(urlout);
                wr.Timeout = 10000;

                try
                {
                    HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
                    
                    url = response.ResponseUri.OriginalString;
                    url = "http://www.ibm.com";
                    if (url != "" && url != InformationRequestMessages.MobileUrlNotFound)
                        context.Response.Redirect(url, true);
                }
                catch (ThreadAbortException)
                {
                    int i = 1;
                    
                    throw;
                }
                catch (Exception ex)
                {
                    int i = 1;
                    //If there is an error let this continue on as a 404
                }

                int d = 2;
            }
        }
        
        public void Dispose(){} 

    }
}
