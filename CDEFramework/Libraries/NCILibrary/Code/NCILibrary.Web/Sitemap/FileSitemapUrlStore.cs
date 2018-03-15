using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using Common.Logging;

namespace NCI.Web.Sitemap
{
    public class FileSitemapUrlStore : SitemapUrlStoreBase
    {
        static ILog log = LogManager.GetLogger(typeof(FileSitemapUrlStore));

        /// <summary>
        /// Create a collection of URL elements from XML files
        /// </summary>
        /// <returns>SitemapUrlSet</returns>
        public override SitemapUrlSet GetSitemapUrls(string sitemapName)
        {
            SitemapUrlSet sitemapUrls = new SitemapUrlSet();

            string path = SitemapConfig.GetProviderPathByName(sitemapName);

            if (path != null)
            {
                String file = HttpContext.Current.Server.MapPath(path);

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(SitemapUrlSet));
                    StreamReader reader = new StreamReader(file);
                    sitemapUrls = (SitemapUrlSet)serializer.Deserialize(reader);
                    
                    foreach(SitemapUrl url in sitemapUrls)
                    {
                        url.changeFreq = sitemapChangeFreq.weekly;
                    }

                    reader.Close();
                }
                // If the file is malformed XML, create an error message.
                catch (Exception ex)
                {
                    log.Error("An XML file has failed parsing in FileSitemapUrlStore:GetSitemapUrls().\nFile: " + file + "\nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + "\n" + ex.ToString() + "\n");
                    return new SitemapUrlSet();
                }

                return new SitemapUrlSet(sitemapUrls);
            }
            else
            {
                log.ErrorFormat("Could not load dictionary provider file located at {0}.", path);
                return new SitemapUrlSet();
            }
        }
    }
}
