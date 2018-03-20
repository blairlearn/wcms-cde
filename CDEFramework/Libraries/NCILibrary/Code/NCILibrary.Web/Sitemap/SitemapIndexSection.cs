using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NCI.Web.Sitemap
{
    /// <summary>
    /// Each SitemapIndexSection section is made up of collection of Sitemaps 
    /// </summary>
    public class SitemapIndexSection : ConfigurationSection
    {
        /// <summary>
        /// Collection of Sitemaps
        /// </summary>
        [ConfigurationProperty("Sitemaps")]
        public SitemapIndexProviderConfiguration Sitemaps
        {
            get { return (SitemapIndexProviderConfiguration)base["Sitemaps"]; }
        }
    }
}
