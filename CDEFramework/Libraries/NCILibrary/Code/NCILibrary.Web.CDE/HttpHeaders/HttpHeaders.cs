using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using NCI.Web.CDE.HttpHeaders.Configuration;

namespace NCI.Web.CDE.HttpHeaders
{
    public static class HttpHeaders
    {
        /// <summary>
        /// Sets custom headers in the HTTP response stream.
        /// </summary>
        /// <param name="context">An HttpContext object.</param>
        public static void SetCustomHeaders(HttpContext context)
        {
            HttpHeadersSection config = HttpHeadersSection.Instance;
            HttpResponse resp = context.Response;

            foreach (HttpHeaderElement item in config.HttpHeaders)
            {
                resp.AddHeader(item.Name, item.Value);
            }
        }

    }
}
