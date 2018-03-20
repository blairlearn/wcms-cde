using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NCI.Web.Sitemap
{
    /// <summary>
    /// This class is a collection of all the SitemapProviderConfigurations
    /// </summary>
    [ConfigurationCollection(typeof(SitemapProviderConfiguration),
        AddItemName = "Sitemap",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class SitemapIndexProviderConfiguration : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new ConfigurationElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new SitemapProviderConfiguration();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element 
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SitemapProviderConfiguration)element).Name;
        }

        public SitemapProviderConfiguration this[int index]
        {
            get
            {
                return base.BaseGet(index) as SitemapProviderConfiguration;
            }
        }

        public new SitemapProviderConfiguration this[string name]
        {
            get
            {
                return base.BaseGet(name) as SitemapProviderConfiguration;
            }
        }
    }
}