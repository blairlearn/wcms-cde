using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    public class ESSiteWideSearchCollectionElement : ConfigurationElement
    {

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        [ConfigurationProperty("template", IsRequired = true)]
        public string Template
        {
            get { return (string)base["template"]; }
        }

        [ConfigurationProperty("index", IsRequired = true)]
        public string Index
        {
            get { return (string)base["index"]; }
        }

        [ConfigurationProperty("site", IsRequired = true)]
        public string Site
        {
            get { return (string)base["site"]; }
        }

        [ConfigurationProperty("cluster", IsRequired = true)]
        public string Cluster
        {
            get { return (string)base["cluster"]; }
        }

        public Uri[] ClusterNodes
        {
            get
            {
                return ElasticSearchConfig.GetClusterNodes(this.Cluster);
            }
        }

        public ClusterElement ClusterElementDetails
        {
            get
            {
                return ElasticSearchConfig.GetCluster(this.Cluster);
            }
        }

        [ConfigurationProperty("fields")]
        public FieldElementCollection Fields
        {
            get { return (FieldElementCollection)base["fields"]; }
        }

    }
}
