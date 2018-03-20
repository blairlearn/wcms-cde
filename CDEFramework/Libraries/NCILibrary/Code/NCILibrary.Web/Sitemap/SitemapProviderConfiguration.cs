using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NCI.Web.Sitemap
{
    public class SitemapProviderConfiguration : ConfigurationElement
    {
        /// <summary>
        /// Name of the search collection
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
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