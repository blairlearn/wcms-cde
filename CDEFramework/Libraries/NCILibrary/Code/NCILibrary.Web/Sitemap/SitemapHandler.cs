using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private void OutputSitemapGenHeaders(string method, Stopwatch stopwatch, HttpResponse response)
        {
            stopwatch.Stop();
            response.Headers.Add("X-SitemapGen-Handler", "Sync");
            response.Headers.Add("X-SitemapGen-Method", method);
            response.Headers.Add("X-SitemapGen-Time", stopwatch.ElapsedMilliseconds.ToString());

        }

        private void OutputSitemapXML(HttpContext context, string method, Stopwatch stopwatch, Action<XmlWriter, XmlSerializer> outputUrlsFunc)
        {
            //Prep the serializer used within loop.
            XmlSerializer ser = new XmlSerializer(typeof(SitemapUrl), string.Empty);

            //NOTE: this is not in a using block since we *don't* want to close the 
            //underlying stream as that would close the response's stream
            XmlWriter writer = XmlWriter.Create(context.Response.OutputStream);

            //Write preamble.
            writer.WriteStartDocument();
            writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");

            //THIS IS CALLING THE ANON FUNCTION THAT WAS PASSED IN
            //this is so we can share code between cached and on-the-fly
            outputUrlsFunc(writer, ser);


            //Do take note of generation time.
            stopwatch.Stop();
            writer.WriteComment(String.Format("{0} Elaspsed Time: {1}", method, stopwatch.ElapsedMilliseconds));

            // Write postamble & close out. (even if there was an error)
            writer.WriteEndDocument();
            writer.Flush();
            writer.Close(); //This closes the output stream.                
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;            
            response.ContentType = "application/xml";
            response.ContentEncoding = System.Text.Encoding.UTF8;
            //Let's try and force the connection to keep pushing out data.
            response.AddHeader("Connection", "close");
            response.BufferOutput = false;


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Check if sitemap is already in the cache; if so, print the cached sitemap
            if (HttpContext.Current.Cache["sitemap"] != null)
            {
                List<SitemapUrl> sitemapUrls = (List<SitemapUrl>)HttpContext.Current.Cache["sitemap"];

                OutputSitemapXML(context, "Cached", stopwatch, (writer, ser) =>
                {
                    // Loop over the sitemap urls.  A proper implementation of 
                    // a sitmap store should yield URLs as soon as they are encountered,
                    // and as such we would wite these to the response as soon as we encounter
                    // them.  This is to avoid http timeout issues.
                    //
                    // As such we should also store off the sitemapUrls encountered while iterating
                    // for storing in the cache when done.
                    int count = 0;
                    foreach (SitemapUrl url in sitemapUrls)
                    {
                        //ser.Serialize(writer, url); //Write out URL
                        writer.WriteStartElement("url");
                        writer.WriteElementString("loc", url.pageUrl);
                        writer.WriteElementString("priority", url.priority.ToString());
                        writer.WriteElementString("changefreq", url.changeFreqXml);
                        writer.WriteFullEndElement();

                        // Flush the buffer on the very first URL, OR
                        // every 100 urls (or so), so that we are streaming
                        // data back to the client.
                        if (count == 0)
                        {
                            writer.Flush();
                        }
                        else if (count == 100)
                        {
                            writer.Flush();
                            count = 0;
                        }
                        count++;
                    }
                });
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
                //This will be the collection of sitemap urls we store in the cache.
                List<SitemapUrl> sitemapUrls = new List<SitemapUrl>();

                OutputSitemapXML(context, "On-the-fly", stopwatch, (writer, ser) =>
                {
                    try
                    {

                        // Loop over the sitemap urls.  A proper implementation of 
                        // a sitmap store should yield URLs as soon as they are encountered,
                        // and as such we would wite these to the response as soon as we encounter
                        // them.  This is to avoid http timeout issues.
                        //
                        // As such we should also store off the sitemapUrls encountered while iterating
                        // for storing in the cache when done.
                        //
                        // One last thing, GetSitemap can throw an error during loop, so we are 
                        // wrapping this whole foreach block in a try/catch.  Also, the final 
                        //
                        int count = 0;
                        foreach (SitemapUrl url in Sitemaps.GetSitemap())
                        {
                            writer.WriteStartElement("url");
                            writer.WriteElementString("loc", url.pageUrl);
                            writer.WriteElementString("priority", url.priority.ToString());
                            writer.WriteElementString("changefreq", url.changeFreqXml);
                            writer.WriteFullEndElement();

                            sitemapUrls.Add(url); //Save URL for caching later.

                            // Flush the buffer on the very first URL, OR
                            // every 100 urls (or so), so that we are streaming
                            // data back to the client.
                            if (count == 0)
                            {
                                writer.Flush();
                            }
                            else if (count == 100)
                            {
                                writer.Flush();
                                count = 0;
                            }
                            count++;
                        }

                        //Succeeded without errors.  Saving.
                        HttpContext.Current.Cache.Add("sitemap", sitemapUrls, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    }
                    catch (Exception ex)
                    {
                        //NOTE: We cannot return an error message upon failure.  We can save the exception so that
                        //we do not generate a bad file for the next request.  This is because we have already 
                        //started streaming the response.

                        // Save the exception in the cache and send an error email
                        HttpContext.Current.Cache.Add("sitemap_ex", ex, null, DateTime.Now.AddMinutes(5), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                        log.Fatal("Error generating sitemap. Check page and file instruction XML files. \nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + " \nSitemapHandler.cs:ProcessRequest()", ex);

                        //To help debugging, let's make sure we know when there was an error generating 
                        //the file, as it may look all fine and dandy
                        writer.WriteComment("An error occurred while generating file");
                    }
                });
            }
        }

        #endregion
    }
}