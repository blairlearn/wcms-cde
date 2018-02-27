using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Common.Logging;

namespace NCI.Web.Sitemap
{
    public static class SitemapConfig
    {
        static ILog log = LogManager.GetLogger(typeof(SitemapConfig));
        private static readonly string sitemapIndexSection = "SitemapIndex";

        public static SitemapProviderConfiguration GetProviderByName(string sitemapName)
        {
            SitemapProviderConfiguration rtnElem = null;

            try
            {
                SitemapIndexSection section = (SitemapIndexSection)ConfigurationManager.GetSection(sitemapIndexSection);
                SitemapIndexProviderConfiguration indexConfig = section.Sitemaps;
                rtnElem = indexConfig[sitemapName + ".xml"];
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error when retrieving Sitemap Provider Configuration for {0}.", sitemapName);
                throw ex;
            }

            return rtnElem;
        }

        public static string GetProviderPathByName(string sitemapName)
        {
            string rtnElem = null;

            try
            {
                SitemapIndexSection section = (SitemapIndexSection)ConfigurationManager.GetSection(sitemapIndexSection);
                SitemapIndexProviderConfiguration indexConfig = section.Sitemaps;
                SitemapProviderConfiguration provConfig = indexConfig[sitemapName + ".xml"];
                rtnElem = provConfig.SitemapStores[0].Parameters.Get("path");
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Error when retrieving Path in Sitemap Provider Configuration for {0}.", sitemapName);
                throw ex;
            }

            return rtnElem;
        }
    }
}
