using System;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.IO;
using NCI.Logging;
using NCI.Web.CDE;

namespace NCI.Web.CDE
{
    public class PromoUrlMappingLoader : IHttpModule
    {
        #region IHttpModule Members

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(OnBeginRequest);
            // Handle LogRequest event and provide custom logging implementation
            context.LogRequest += new EventHandler(OnLogRequest);
        }

        void OnLogRequest(object sender, EventArgs e)
        {
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);

            if (url.ToLower().IndexOf(".ico") != -1 || url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1 || url.IndexOf(".axd") != -1)
                return;

            //Check if the PageAssemblyInstruction is not null then it was processed as pretty url.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            try
            {
                //Check if Promo Url information is in the Application State
                PromoUrlMapping promoUrlMapping = null;
                bool reLoad = false;
                if (context.Application["reloadPromoUrlMappingInfo"] != null)
                    reLoad = (bool)context.Application["reloadPromoUrlMappingInfo"];

                if (context.Application["PromoUrlMapping"] != null && !reLoad)
                    promoUrlMapping = (PromoUrlMapping)context.Application["PromoUrlMapping"];
                else
                {
                    context.Application.Lock();
                    PromoUrlMapping freshPromoUrlMapping = PromoUrlMappingInfoFactory.GetPromoUrlMapping("/");
                    if (freshPromoUrlMapping != null)
                    {
                        context.Application["reloadPromoUrlMappingInfo"] = false;
                        context.Application["PromoUrlMapping"] = freshPromoUrlMapping;
                    }
                    context.Application.UnLock();
                }

                promoUrlMapping = (PromoUrlMapping)context.Application["PromoUrlMapping"];

                if (promoUrlMapping != null)
                {
                    PromoUrl promoUrl = null;
                    if (promoUrlMapping.PromoUrls.ContainsKey(url.ToLower()))
                    {
                        promoUrl = promoUrlMapping.PromoUrls[url.ToLower()];
                        string mappedToUrl = promoUrl.MappedTo + (string.IsNullOrEmpty(context.Request.Url.Query) ? String.Empty : context.Request.Url.Query);

                        // If the original request is post then save target promo url
                        // for use in the page instructions assembly loader.
                        if (context.Request.RequestType == "POST")
                        {
                            context.RewritePath(mappedToUrl);
                        }
                        else
                            context.Response.Redirect(mappedToUrl, true);
                    }
                    else
                    {
                        //1. Remove last part of path, e.g. /cancertopics/wyntk/bladder/page10 becomes /cancertopics/wyntk/bladder
                        string truncUrl = url.Substring(0, url.LastIndexOf('/'));
                        string appendUrl = url.Substring(url.LastIndexOf('/'));

                        if (truncUrl != string.Empty)
                        {
                            if (promoUrlMapping.PromoUrls.ContainsKey(truncUrl.ToLower()))
                            {
                                promoUrl = promoUrlMapping.PromoUrls[truncUrl.ToLower()];
                                string mappedToUrl = promoUrl.MappedTo + appendUrl + (string.IsNullOrEmpty(context.Request.Url.Query) ? String.Empty : context.Request.Url.Query);
                                if (context.Request.RequestType == "POST")
                                {
                                    context.RewritePath(mappedToUrl);
                                }
                                else
                                    context.Response.Redirect(mappedToUrl, true);
                            }

                        }
                        else
                        {
                            Logger.LogError("CDE:PromoUrlMappingLoader.cs:OnBeginRequest", "Promo Url Mapping information not found for " + url, NCIErrorLevel.Debug);
                        }

                    }
                }
                else
                {
                    Logger.LogError("CDE:PromoUrlMappingLoader.cs:OnBeginRequest", "No Promo Url Mapping information", NCIErrorLevel.Warning);
                }
            }
            catch (System.Threading.ThreadAbortException)
            { }
            catch (Exception ex)
            {
                Logger.LogError("CDE:PromoUrlMappingLoader.cs:OnBeginRequest", "\nFailed to Process Promo URL - " + url, NCIErrorLevel.Error, ex);
            }

        }

        #endregion


    }
}
