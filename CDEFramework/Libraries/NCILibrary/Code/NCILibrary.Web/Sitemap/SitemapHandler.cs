using System;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace NCI.Web.Sitemap
{
    public class SitemapHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "application/xml";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            byte[] utf8;

            // Check if sitemap is already in the cache; if so, print the cached sitemap
            if (HttpContext.Current.Cache["sitemap"] != null)
            {
                utf8 = (byte[])HttpContext.Current.Cache["sitemap"];
                response.OutputStream.Write(utf8, 0, utf8.Length);
            }
            // If it isn't, get the current sitemap, save that in the cache, and output
            else
            {
                XmlSerializer ser = new XmlSerializer(typeof(SitemapUrlSet));
                using (MemoryStream memStream = new MemoryStream())
                using (XmlWriter writer = XmlWriter.Create(memStream))
                {
                    ser.Serialize(writer, Sitemaps.GetSitemap());
                    utf8 = memStream.ToArray();
                    HttpContext.Current.Cache.Add("sitemap", utf8, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    response.OutputStream.Write(utf8, 0, utf8.Length);
                }
            }
        }

        #endregion
    }
}