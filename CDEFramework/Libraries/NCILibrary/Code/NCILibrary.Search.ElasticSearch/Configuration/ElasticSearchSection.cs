using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    public class ElasticSearchSection : ConfigurationSection
    {

        [ConfigurationProperty("clusters")]
        public ClusterElementCollection Clusters
        {
            get { return (ClusterElementCollection)base["clusters"]; }
        }

        [ConfigurationProperty("siteWideSearchCollections")]
        public ESSiteWideSearchCollectionElementCollection SiteWideSearchCollections
        {
            get { return (ESSiteWideSearchCollectionElementCollection)base["siteWideSearchCollections"]; }
        }

    }
}
