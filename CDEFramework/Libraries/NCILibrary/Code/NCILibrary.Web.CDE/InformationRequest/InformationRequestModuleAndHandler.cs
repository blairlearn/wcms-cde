using System;
using System.Globalization;
using System.Web;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.InformationRequest;

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

            // If this a front-end asset URL, don't proceed 
            // TODO: make this a configuration setting 
            if (url.IndexOf(".axd") != -1 ||
                url.IndexOf(".css") != -1 ||
                url.IndexOf(".eot") != -1 ||
                url.IndexOf(".gif") != -1 ||
                url.IndexOf(".ico") != -1 ||
                url.IndexOf(".jpg") != -1 ||
                url.IndexOf(".js") != -1 ||
                url.IndexOf(".png") != -1 ||
                url.IndexOf(".svg") != -1 ||
                url.IndexOf(".ttf") != -1 ||
                url.IndexOf(".woff") != -1)
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
