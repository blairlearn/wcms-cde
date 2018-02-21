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

            SitemapIndexSection section = (SitemapIndexSection)ConfigurationManager.GetSection("SitemapIndex");
            SitemapIndexProviderConfiguration indexConfig = section.Sitemaps;
            SitemapProviderConfiguration config = indexConfig[sitemapName + ".xml"];
            string path = config.SitemapStores[0].Parameters.Get("path");
            String file = HttpContext.Current.Server.MapPath(path);

            List<String> errorMessages = new List<String>();

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SitemapUrlSet));
                StreamReader reader = new StreamReader(file);
                sitemapUrls = (SitemapUrlSet)serializer.Deserialize(reader);
                reader.Close();
            }
            // If the file is malformed XML, create an error message.
            catch (XmlException ex)
            {
                log.Error("An XML file has failed parsing in FileSitemapUrlStore:GetSitemapUrls().\nFile: " + file + "\nEnvironment: " + System.Environment.MachineName + "\nRequest Host: " + HttpContext.Current.Request.Url.Host + "\n" + ex.ToString() + "\n");
                throw (ex);
            }
            
            return new SitemapUrlSet(sitemapUrls);
        }
    }
}
