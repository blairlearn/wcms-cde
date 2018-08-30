using System;
using System.IO;
using System.Web;
using Common.Logging;
using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE
{
    public class PromoUrlMappingLoader : IHttpModule
    {
        static ILog log = LogManager.GetLogger(typeof(PromoUrlMappingLoader));

        HttpContext context = null;
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
            context = ((HttpApplication)sender).Context;

            // Get absolute path of the request URL as pretty URL
            String url = context.Server.UrlDecode(context.Request.Url.AbsolutePath);

            if (Utility.IgnoreWebResource(url))
                return;

            //Check if the PageAssemblyInstruction is not null then it was processed as pretty url.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            try
            {
                //Check if Promo Url information is in the Application State
                PromoUrlMapping promoUrlMapping = null;
                bool reLoad = (bool)context.Application["reloadPromoUrlMappingInfo"];
                bool promoUrlMappingFileChanged = IsPromoUrlMappingFileChanged("/");

                if (context.Application["PromoUrlMapping"] != null && !reLoad && !promoUrlMappingFileChanged)
                    promoUrlMapping = (PromoUrlMapping)context.Application["PromoUrlMapping"];
                else
                {
                    context.Application.Lock();
                    FileInfo fileInfo = null;
                    PromoUrlMapping freshPromoUrlMapping = PromoUrlMappingInfoFactory.GetPromoUrlMapping("/", ref fileInfo);
                    if (freshPromoUrlMapping != null)
                    {
                        context.Application["reloadPromoUrlMappingInfo"] = false;
                        context.Application["PromoUrlMapping"] = freshPromoUrlMapping;
                        CachedPromoMappingFileInfo = fileInfo;
                    }
                    context.Application.UnLock();

                    // Log an event saying the promo url mapping file was loaded
                    log.DebugFormat("Promo Url Mapping file successfully loaded reLoad: {0}, promoUrlMappingFileChanged: {1}", 
                        reLoad, promoUrlMappingFileChanged);
                }

                promoUrlMapping = (PromoUrlMapping)context.Application["PromoUrlMapping"];

                if (promoUrlMapping != null)
                {
                    PromoUrl promoUrl = null;
                    if (promoUrlMapping.PromoUrls.ContainsKey(url.ToLower()))
                    {
                        promoUrl = promoUrlMapping.PromoUrls[url.ToLower()];
                        string mappedToUrl = promoUrl.MappedTo + (string.IsNullOrEmpty(context.Request.Url.Query) ? "?redirect=true" : context.Request.Url.Query);

                        // Add redirect parameter for analytics (if not previously redirected)
                        if(!string.IsNullOrEmpty(context.Request.Url.Query))
                        {
                            mappedToUrl += (!context.Request.Url.Query.Contains("redirect=true") ? "&redirect=true" : String.Empty);
                        }


                        // If the original request is post then save target promo url
                        // for use in the page instructions assembly loader.
                            if (context.Request.RequestType == "POST")
                        {
                            context.RewritePath(mappedToUrl);
                        }
                        else
                            NCI.Web.CDE.Application.PermanentRedirector.DoPermanentRedirect(context.Response, mappedToUrl, "Promo Url");
                    }
                    else if (!url.EndsWith("/") && promoUrlMapping.PromoUrls.ContainsKey(url.ToLower() + "/"))
                    {
                        promoUrl = promoUrlMapping.PromoUrls[url.ToLower() + "/"];
                        string mappedToUrl = promoUrl.MappedTo + (string.IsNullOrEmpty(context.Request.Url.Query) ? "?redirect=true" : context.Request.Url.Query);

                        // Add redirect parameter for analytics (if not previously redirected)
                        if (!string.IsNullOrEmpty(context.Request.Url.Query))
                        {
                            mappedToUrl += (!context.Request.Url.Query.Contains("redirect=true") ? "&redirect=true" : String.Empty);
                        }

                        // If the original request is post then save target promo url
                        // for use in the page instructions assembly loader.
                        if (context.Request.RequestType == "POST")
                        {
                            context.RewritePath(mappedToUrl);
                        }
                        else
                            NCI.Web.CDE.Application.PermanentRedirector.DoPermanentRedirect(context.Response, mappedToUrl, "Promo Url");
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
                                string mappedToUrl = promoUrl.MappedTo + appendUrl + (string.IsNullOrEmpty(context.Request.Url.Query) ? "?redirect=true" : context.Request.Url.Query);

                                // Add redirect parameter for analytics (if not previously redirected)
                                if (!string.IsNullOrEmpty(context.Request.Url.Query))
                                {
                                    mappedToUrl += (!context.Request.Url.Query.Contains("redirect=true") ? "&redirect=true" : String.Empty);
                                }

                                if (context.Request.RequestType == "POST")
                                {
                                    context.RewritePath(mappedToUrl);
                                }
                                else
                                    NCI.Web.CDE.Application.PermanentRedirector.DoPermanentRedirect(context.Response, mappedToUrl, "Promo Url multi-page");                                    
                            }

                        }
                        else
                        {
                            log.DebugFormat("OnBeginRequest(): Promo Url Mapping information not found for {0}", url);
                        }

                    }
                }
                else
                {
                    log.Warn("OnBeginRequest(): No Promo Url Mapping information");
                }
            }
            catch (System.Threading.ThreadAbortException)
            { }
            catch (Exception ex)
            {
                log.Error("OnBeginRequest(): Failed to Process Promo URL - " + url, ex);
            }

        }

        #endregion

        #region Private Members

        private FileInfo CachedPromoMappingFileInfo
        {
            get
            {
                return context.Application["PromoMappingFileInfo"] as FileInfo;
            }
            set
            {
                context.Application["PromoMappingFileInfo"] = value;
            }
        }

        /// <summary>
        /// The file watcher on the promourlmapping file does not seem to work at all when the file is
        /// managed using DFS even when the contents of the file have changed, but since we need to load 
        /// promo url as soon as it is changed, we are going to do out own checking. Check if the file 
        /// time stamp since the last time it was recorded. If it got updated it is time to reload the mapping 
        /// file.
        /// </summary>
        /// <returns></returns>
        private bool IsPromoUrlMappingFileChanged(string path)
        {
            bool changed = false;
            try
            {
                string pmFile = String.Format(ContentDeliveryEngineConfig.PathInformation.PromoUrlMappingPath.Path, (path == "/" ? String.Empty : path));
                pmFile = HttpContext.Current.Server.MapPath(pmFile);
                FileInfo promoUrlFileInfo = new FileInfo(pmFile);
                if (CachedPromoMappingFileInfo != null && promoUrlFileInfo != null)
                {
                    return  DateTime.Compare( CachedPromoMappingFileInfo.CreationTime, promoUrlFileInfo.CreationTime) != 0 || 
                            DateTime.Compare( CachedPromoMappingFileInfo.LastAccessTime, promoUrlFileInfo.LastAccessTime ) != 0 ||
                            DateTime.Compare(CachedPromoMappingFileInfo.LastWriteTime, promoUrlFileInfo.LastWriteTime) != 0;
                }
            }
            catch{}

            return changed;
        }


        #endregion
    }
}
