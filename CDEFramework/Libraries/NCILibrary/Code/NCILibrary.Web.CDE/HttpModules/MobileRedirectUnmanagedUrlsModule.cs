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
            //app.LogRequest += new System.EventHandler(OnPostLogRequest);
           app.PreSendRequestHeaders += new System.EventHandler(OnPreSendRequestHeaders);
        }

        public void OnPreSendRequestHeaders(object source, EventArgs e)
        {
            HttpApplication app = (HttpApplication)source;
            HttpContext context = app.Context;
            Exception exception = context.Server.GetLastError();
            String key = context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture);
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture));
            String mobileApplication = "";

            if (context.Response.StatusCode == 404)
            {
                if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                    return;

                try
                {
                    //Is this a mapped application 
                    mobileApplication = InformationRequestConfig.GetMobileUrl(context.Request.Url.PathAndQuery);
                    if (mobileApplication != "")
                    {
                        //redirect to mobile application 
                    }
                    else
                    {

                        string InformationRequestCommand = InformationRequestConfig.DesktopHost + context.Request.Url.AbsolutePath + InformationRequestConstants.MobileUrlRequest;
                        InformationRequestProcessor irPro = new InformationRequestProcessor(InformationRequestCommand);
                        if (irPro.ReturnMessage == InformationRequestMessages.MobileUrlFound)
                        {
                            //Mobile Url found - redirect 
                            url = irPro.ReturnValue;
                            if (url != "")
                                context.Response.Redirect(url + context.Request.Url.Query, true);
                        }
                        else if (irPro.ReturnMessage == InformationRequestMessages.MobileUrlNotFound)
                        {
                            //Page found on desktop - no mobile version - redirect to desktop site
                            context.Response.Redirect(InformationRequestConfig.DesktopHost + context.Request.Url.AbsolutePath + context.Request.Url.Query, true);
                        }
                    }
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


            if (context.Response.StatusCode == 302)
            {
                if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                    return;

                try
                {
                    //Is this a mapped application - see web.config: nci/web/informationRequest/mappedPages 
                    mobileApplication = InformationRequestConfig.GetMobileUrl(context.Request.Url.LocalPath);
                    if (mobileApplication != "")
                    {
                        //redirect to mobile application 
                        string redirectUrl = InformationRequestConfig.MobileHost + mobileApplication + context.Request.Url.Query;
                        context.Response.Redirect(redirectUrl, true);
                    }
                    else
                    {
                        //Application redirect
                        string redirectUrl = InformationRequestConfig.DesktopHost + context.Request.Url.AbsolutePath + context.Request.Url.Query;
                        context.Response.Redirect(redirectUrl, true);
                    }
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
