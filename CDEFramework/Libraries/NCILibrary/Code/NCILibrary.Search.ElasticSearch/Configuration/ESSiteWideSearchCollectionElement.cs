using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// This class has the details of the cluster and the nodes that are part of a search collection
    /// </summary>
    
    public class ESSiteWideSearchCollectionElement : ConfigurationElement
    {
        /// <summary>
        /// Name of the search collection
        /// </summary>
    
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; }
        }

        /// <summary>
        /// The template used for search
        /// </summary>
    
        [ConfigurationProperty("template", IsRequired = true)]
        public string Template
        {
            get { return (string)base["template"]; }
        }

        /// <summary>
        /// The index used for search
        /// </summary>
       [ConfigurationProperty("index", IsRequired = true)]
        public string Index
        {
            get { return (string)base["index"]; }
        }

       /// <summary>
       /// The search website - Cancer.Gov, DCEG, etc.
       /// </summary>
    
        [ConfigurationProperty("site", IsRequired = true)]
        public string Site
        {
            get { return (string)base["site"]; }
        }

        /// <summary>
        /// The name of the search cluster
        /// </summary>
    
        [ConfigurationProperty("cluster", IsRequired = true)]
        public string Cluster
        {
            get { return (string)base["cluster"]; }
        }

        /// <summary>
        /// An array of all the cluster nodes
        /// </summary>
    
        public Uri[] ClusterNodes
        {
            get
            {
                return ElasticSearchConfig.GetClusterNodes(this.Cluster);
            }
        }

        /// <summary>
        /// Cluster details - Name, MaximumRetries and ConnectionTimeoutDelay
        /// See Cluster Element class for detailed description of all the properties
        /// </summary>
    
        public ClusterElement ClusterElementDetails
        {
            get
            {
                return ElasticSearchConfig.GetCluster(this.Cluster);
            }
        }

        /// <summary>
        /// Fields in the search collection - Title, URL, Description
        /// </summary>
    
        [ConfigurationProperty("fields")]
        public FieldElementCollection Fields
        {
            get { return (FieldElementCollection)base["fields"]; }
        }

    }
}
