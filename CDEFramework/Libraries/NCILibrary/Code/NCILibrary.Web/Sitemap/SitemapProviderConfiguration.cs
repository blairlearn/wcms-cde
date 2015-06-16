using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NCI.Web.Sitemap
{
    public class SitemapProviderConfiguration : ConfigurationSection
    {
        public static SitemapProviderConfiguration Get()
        {
            SitemapProviderConfiguration config = (SitemapProviderConfiguration)ConfigurationManager.GetSection("Sitemap");
            return config;
        }

        [ConfigurationProperty("SitemapStores")]
        public ProviderSettingsCollection SitemapStores
        {
            get
            {
                return ((ProviderSettingsCollection)base["SitemapStores"]);
            }
        }
    }
}