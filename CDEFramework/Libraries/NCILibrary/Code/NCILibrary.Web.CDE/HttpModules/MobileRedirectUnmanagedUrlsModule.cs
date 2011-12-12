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
using NCI.Logging;
using NCI.Web.CDE.CapabilitiesDetection;

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
            String mobilePage = "";
            bool mobileDevice = false;

            if ((DisplayDeviceDetector.DisplayDevice == DisplayDevices.BasicMobile) ||
                (DisplayDeviceDetector.DisplayDevice == DisplayDevices.AdvancedMobile))
                mobileDevice = true;

            //For TESTING 
            mobileDevice = true;
            
            //If mobile device
            if(mobileDevice)
            {

                if (context.Response.StatusCode == 404) // Page not found
                {
                    if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                        return;

                    try
                    {

                        //Is this a mapped application 
                        mobilePage = InformationRequestConfig.GetMobileUrl(context.Request.Url.PathAndQuery);
                        if (mobilePage != "")
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


                if (context.Response.StatusCode == 302) // a redirect was returned 
                {
                    if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                        return;

                    try
                    {
                        //Is this a mapped page - see web.config: nci/web/informationRequest/mappedPages 
                        mobilePage = InformationRequestConfig.GetMobileUrl(context.Request.Url.LocalPath);
                        if (mobilePage != "")
                        {
                            //page mapping found - redirect to mapped page
                            string redirectUrl = InformationRequestConfig.MobileHost + mobilePage + context.Request.Url.Query;
                            context.Response.Redirect(redirectUrl, true);
                        }
                        else
                        {
                            //page mapping not found - see if page exist on desktop
                            string redirectUrl = InformationRequestConfig.DesktopHost + context.Request.Url.AbsolutePath + context.Request.Url.Query;
                            InformationRequestProcessor irPro = new InformationRequestProcessor(redirectUrl);
                            //if page not found (404), exception will be thrown - else redirect to the page on desktop
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
                        {
                            //redirect to page not found error page
                            context.Response.Redirect("/PublishedContent/ErrorMessages/pagenotfound.html", true);
                        }
                    }
                }
            }
        }
        
        public void Dispose(){} 

    }
}
