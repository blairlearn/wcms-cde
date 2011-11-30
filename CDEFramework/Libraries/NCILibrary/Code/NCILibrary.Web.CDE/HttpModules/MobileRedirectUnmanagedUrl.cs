using System;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using System.IO;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Logging;
using System.Reflection;

namespace NCI.Web.CDE
{
    public class MobileRedirectUnmanagedUrl : IHttpModule
    {
        
 
        /// <summary>
        /// Instantiated  event OnBeginRequest
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.Error += new System.EventHandler(OnError);

        }

        public void OnError(object obj, EventArgs args)
        {
            HttpContext ctx = HttpContext.Current;
            HttpResponse response = ctx.Response;
            HttpRequest request = ctx.Request;

            Exception exception = ctx.Server.GetLastError();
        }

        public void Dispose()
        {
            //clean-up code here.
        }

    }
}
