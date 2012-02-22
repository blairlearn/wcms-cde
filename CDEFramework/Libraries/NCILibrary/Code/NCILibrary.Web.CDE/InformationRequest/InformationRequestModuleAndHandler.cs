using System;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using System.IO;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Web.CDE.InformationRequest;
using NCI.Logging;
using System.Reflection;
using NCI.Web.CDE.InformationRequest.Configuration;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE
{
    public class InformationRequestModuleAndHandler : IHttpModule, IHttpHandler
    {

        //This functions as both http Module and Handler for information request 


        #region IHttpModule Members
        /// <summary>
        /// Intit attaches module to OnMapRequestHandler
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new System.EventHandler(OnMapRequestHandler);
        }

        public void OnMapRequestHandler(object sender, EventArgs args)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath.ToLower(CultureInfo.InvariantCulture)).ToLower();

            if (url.IndexOf(".ico") != -1 ||
                url.IndexOf(".css") != -1 ||
                url.IndexOf(".gif") != -1 ||
                url.IndexOf(".jpg") != -1 ||
                url.IndexOf(".js") != -1 ||
                url.IndexOf(".axd") != -1 ||
                url.IndexOf(".png") != -1)
                return;

            
            string[] request = { "" };
            String command = (string)context.Request.QueryString[InformationRequestConstants.InformationRequestToken];

            if (!string.IsNullOrEmpty(command))
            {
                context.RemapHandler(new InformationRequestModuleAndHandler());
            }
        }
            
        public void Dispose()
        {
            //clean-up code here.
        }

        #endregion

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string command = (string)context.Request.QueryString[InformationRequestConstants.InformationRequestToken];

            string notFoundMessage = "";

            switch (command.ToLower())
            {
                case "mobileurl":
                    if (PageAssemblyContext.Current.PageAssemblyInstruction == null)
                        context.Response.Write(InformationRequestMessages.FileNotFound);
                    else
                    {
                        string[] alternateContentVersionsKeys = PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys;
                        if (Array.Find(alternateContentVersionsKeys,
                            element => element.StartsWith("mobileurl", StringComparison.Ordinal)) == "mobileurl")
                        {
                            NciUrl mobileUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("mobileurl");
                            context.Response.Write(InformationRequestMessages.MobileUrlFound + " [" + mobileUrl.ToString() + "]");
                        }
                        else
                            context.Response.Write(InformationRequestMessages.MobileUrlNotFound);
                    }
                    break;

                case "canonicalurl":
                    notFoundMessage = InformationRequestMessages.CanonicalUrlNotFound;
                    if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                    {
                        string canonicalUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl(PageAssemblyInstructionUrls.CanonicalUrl).ToString();
                        canonicalUrl = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName + canonicalUrl;
                        context.Response.Write(InformationRequestMessages.CanonicalUrlFound + " [" + canonicalUrl + "]");
                    }
                    else
                        context.Response.Write(notFoundMessage);
                    break;

                default:
                    context.Response.Write("Unknown Command:[" + command + "]");
                    break;

            }
        }

        #endregion
    }
}
