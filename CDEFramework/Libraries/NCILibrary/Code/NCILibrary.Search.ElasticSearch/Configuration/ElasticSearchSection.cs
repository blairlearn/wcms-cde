using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// Each ElasticSearch section is made up of collection of clusters and SiteWideSearchElements 
    /// </summary>
    public class ElasticSearchSection : ConfigurationSection
    {
        /// <summary>
        /// Collection of clusters
        /// </summary>
        [ConfigurationProperty("clusters")]
        public ClusterElementCollection Clusters
        {
            get { return (ClusterElementCollection)base["clusters"]; }
        }

        /// <summary>
        /// Collection of siteWideSearchCollections
        /// </summary>
        [ConfigurationProperty("siteWideSearchCollections")]
        public ESSiteWideSearchCollectionElementCollection SiteWideSearchCollections
        {
            get { return (ESSiteWideSearchCollectionElementCollection)base["siteWideSearchCollections"]; }
        }

    }
}
