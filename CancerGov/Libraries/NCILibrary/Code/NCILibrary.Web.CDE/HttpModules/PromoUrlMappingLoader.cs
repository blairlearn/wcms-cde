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
    public class PromoUrlMappingLoader:IHttpModule
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

            if (url.IndexOf(".css") != -1 || url.IndexOf(".gif") != -1 || url.IndexOf(".jpg") != -1 || url.IndexOf(".js") != -1)
                return;

            //Check if the PageAssemblyInstruction is not null then it was processed as pretty url.
            if (PageAssemblyContext.Current.PageAssemblyInstruction != null)
                return;

            try
            {
                PromoUrlMapping promoUrlMapping = PromoUrlMappingInfoFactory.GetPromoUrlMapping("/");
                PromoUrl promoUrl = promoUrlMapping.PromoUrls[url];
                if (promoUrl != null)
                    context.Response.Redirect(promoUrl.MappedTo,true);
            }
            catch (Exception ex)
            { 
            }

            
        }

        #endregion
    }
}
