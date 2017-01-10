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

        [ConfigurationProperty("SitemapErrorCount")]
        public SitemapErrorCount ErrorCount
        {
            get
            {
                return ((SitemapErrorCount)base["SitemapErrorCount"]);
            }
        }
    }

    /// <summary>
    /// Class representing the "SitemapErrorCount" configuration element.
    /// </summary>
    public class SitemapErrorCount : ConfigurationElement
    {
        [ConfigurationProperty("max", DefaultValue = "5", IsRequired = true)]
        public int Max
        {
            get
            { return (int)this["max"]; }
            set
            { this["max"] = value; }
        }
    }
}