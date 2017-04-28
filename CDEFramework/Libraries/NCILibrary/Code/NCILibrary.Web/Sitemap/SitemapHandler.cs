using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Common.Logging;

namespace NCI.Web.Sitemap
{
    public class SitemapHandler : IHttpHandler
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
            // Check if the exception is already in the cache; if so, go to error page
            else if (HttpContext.Current.Cache["sitemap_ex"] != null)
            {
                response.Status = "500";
                response.End();
            }
            // If it isn't, get the current sitemap, save that in the cache, and output
            else
            {
                // Create a stopwatch object for timing of sitemap retrieval
                Stopwatch stopwatch = new Stopwatch();
                TimeSpan timeSpan;

                try
                {
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(string.Empty, "http://www.sitemaps.org/schemas/sitemap/0.9");
                    XmlSerializer ser = new XmlSerializer(typeof(SitemapUrlSet), "http://www.sitemaps.org/schemas/sitemap/0.9");

                    using (MemoryStream memStream = new MemoryStream())
                    using (XmlWriter writer = XmlWriter.Create(memStream))
                    {
                        stopwatch.Start();
                        ser.Serialize(writer, Sitemaps.GetSitemap(), ns);
                        utf8 = memStream.ToArray();
                        HttpContext.Current.Cache.Add("sitemap", utf8, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                        response.OutputStream.Write(utf8, 0, utf8.Length);
                        stopwatch.Stop();
                    }

                    // Execution timeout is 110±15 seconds, so we'll log an error if the process takes more than 90 seconds. 
                    // This will allow us to track instances where sitemap generation is slow, even if we don't hit a ThreadAbortException. 
                    // See https://blogs.msdn.microsoft.com/pedram/2007/10/02/how-the-execution-timeout-is-managed-in-asp-net/)
                    timeSpan = stopwatch.Elapsed;
                    if (timeSpan.TotalSeconds > 90)
                    {
                        log.Error("Warning: XML sitemap is taking longer than expected to retrieve. Check page and file instruction XML files.\nTime Elapsed for Sitemap Retrieval: " + timeSpan.ToString());
                    }
                    else
                    {
                        log.Debug("Time Elapsed for Sitemap Retrieval: " + timeSpan.ToString());
                    }
                }
                // Save the exception in the cache and send an error email
                catch (Exception ex)
                {
                    if (stopwatch.IsRunning)
                    {
                        stopwatch.Stop();
                    }
                    timeSpan = stopwatch.Elapsed;

                    HttpContext.Current.Cache.Add("sitemap_ex", ex, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    log.Fatal("Error generating sitemap. Check page and file instruction XML files. \nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host
                              + "\nTime Elapsed for Sitemap Retrieval: " + timeSpan.ToString() + " \nSitemapHandler.cs:ProcessRequest()", ex);
                    response.Status = "500";
                    response.End();
                }
            }
        }

        #endregion
    }
}