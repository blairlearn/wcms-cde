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
            //app.PostLogRequest += new System.EventHandler(OnPostLogRequest);
            app.LogRequest += new System.EventHandler(OnPostLogRequest);
        }

        public void OnPostLogRequest(object source, EventArgs e)
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


                try
                {
                    //string urlout = host + context.Request.Url.LocalPath.ToString() + "?Information__Request=mobileurl";
                    //WebRequest wr = WebRequest.Create(urlout);
                    //wr.Timeout = 10000;
                    //HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
                    //url = response.ResponseUri.OriginalString;

                    WebRequest request = WebRequest.Create(
                      "http://localhost:7001/ZZ?Information__Request=mobile");
                    request.Credentials = CredentialCache.DefaultCredentials;
                    WebResponse response = request.GetResponse();

                    // Display the status
                    System.Diagnostics.Debug.WriteLine("**** " + ((HttpWebResponse)response).StatusDescription);

                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine("**** " + responseFromServer);
                    reader.Close();
                    response.Close();



                    url = "http://www.ibm.com";
                    if (url != "" && url != InformationRequestMessages.MobileUrlNotFound)
                        context.Response.Redirect(url, true);
                }
                catch (ThreadAbortException)
                {
                    // This exception is barfed up because of 
                    // the second parameter of the above context.Response.Redirect 
                    // is set to true for endResponse (which throws a ThreadAbortException error)
                    throw;
                }
                catch (Exception ex)
                {
                    if (ex.Message == "The remote server returned an error: (404) Not Found.")
                        return;
                }
            }
        }
        
        public void Dispose(){} 

    }
}
