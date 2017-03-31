using System;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using Common.Logging;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NCI.Web.Sitemap
{
    public class AsyncSitemapHandler : HttpTaskAsyncHandler
    {
        static ILog log = LogManager.GetLogger(typeof(SitemapHandler));


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

        public override void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException("The ProcessRequest method has no implementation, see ProcessRequestAsync.");
        }

        private void OutputSitemapGenHeaders(string method, Stopwatch stopwatch, HttpResponse response) 
        {
            stopwatch.Stop();
            response.Headers.Add("X-SitemapGen-Handler", "Async");
            response.Headers.Add("X-SitemapGen-Method", method);
            response.Headers.Add("X-SitemapGen-Time", stopwatch.ElapsedMilliseconds.ToString());
        }

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "application/xml";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            byte[] utf8;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Check if sitemap is already in the cache; if so, print the cached sitemap
            if (HttpContext.Current.Cache["sitemap"] != null)
            {
                utf8 = (byte[])HttpContext.Current.Cache["sitemap"];                
                response.OutputStream.Write(utf8, 0, utf8.Length);              
                OutputSitemapGenHeaders("Cache", stopwatch, response);
            }
            // Check if the exception is already in the cache; if so, go to error page
            else if (HttpContext.Current.Cache["sitemap_ex"] != null)
            {                
                response.Status = "500";
                OutputSitemapGenHeaders("Error", stopwatch, response);
            }
            // If it isn't, get the current sitemap, save that in the cache, and output
            else
            {
                try
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, "http://www.sitemaps.org/schemas/sitemap/0.9");
                    XmlSerializer ser = new XmlSerializer(typeof(SitemapUrlSet), "http://www.sitemaps.org/schemas/sitemap/0.9");

                    using (MemoryStream memStream = new MemoryStream())
                    using (XmlWriter writer = XmlWriter.Create(memStream))
                    {

                        ser.Serialize(writer, Sitemaps.GetSitemap(), ns);
                        utf8 = memStream.ToArray();
                        HttpContext.Current.Cache.Add("sitemap", utf8, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                        response.OutputStream.Write(utf8, 0, utf8.Length);
                        OutputSitemapGenHeaders("OnTheFly", stopwatch, response);
                    }
                }
                // Save the exception in the cache and send an error email
                catch (Exception ex)
                {
                    HttpContext.Current.Cache.Add("sitemap_ex", ex, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    log.Fatal("Error generating sitemap. Check page and file instruction XML files. \nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + " \nSitemapHandler.cs:ProcessRequest()", ex);
                    response.Status = "500";
                    OutputSitemapGenHeaders("Error", stopwatch, response);
                }
            }
        }

        #endregion
    }
}