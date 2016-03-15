using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search.Configuration;
using System.Configuration;

namespace NCI.Search
{

    /// <summary>
    /// Encapsulates methods needed for the ElasticSearchConfig
    /// </summary>
    public static class ElasticSearchConfig
    {
        /// <summary>
        /// elastic search configuration path
        /// </summary>
        private static string _esconfigPath = "nci/search/elasticsearch";

        /// <summary>
        /// Gets all the fields for a given search collection
        /// </summary>
        /// <param name="searchCollectionName">The name of the search collection</param>
        public static string[] GetFields(string searchCollectionName)
        {
            List<string> fieldList = new List<string>();

            ESSiteWideSearchCollectionElement searchCollConfig = GetSearchCollectionConfig(searchCollectionName);
            if (searchCollConfig == null)
                throw new ConfigurationErrorsException("ElasticSearch SiteWideSearch collection, " + searchCollectionName + ", cannot be found in configuration.");

            FieldElementCollection fieldConfigs = searchCollConfig.Fields;

            foreach (FieldElement fieldElement in fieldConfigs)
            {
                fieldList.Add(fieldElement.FieldName);

            }

            return fieldList.ToArray();
        }

        /// <summary>
        /// Gets all the nodes in a given cluster for a given search collection
        /// </summary>
        /// <param name="clusterName">The name of the cluster</param>
        public static Uri[] GetClusterNodes(string clusterName)
        {
            List<Uri> listOfNodes = new List<Uri>();

            ClusterElementCollection clusterConfigs = ESSection.Clusters;

            if (clusterConfigs == null)
                throw new ConfigurationErrorsException("ElasticSearch cluster configuration cannot be found");

            ClusterElement requestedCluster = null;

            //Find the cluster
            foreach (ClusterElement cluster in clusterConfigs)
            {
                if (cluster.Name == clusterName)
                {
                    requestedCluster = cluster;
                    break;
                }
            }

            if (requestedCluster == null)
                throw new ConfigurationErrorsException("ElasticSearch cluster, " + clusterName + ", cannot be found in configuration.");


            foreach (NodeElement node in requestedCluster.Nodes)
            {
                listOfNodes.Add(new Uri(node.NodeIP + ":" + node.Port));
            }

            return listOfNodes.ToArray();
        }

        /// <summary>
        /// Gets a cluster object with the name
        /// </summary>
        /// <param name="clusterName">The name of the cluster</param>
        public static ClusterElement GetCluster(string clusterName)
        {
            List<string> fieldList = new List<string>();

            ClusterElementCollection clusterConfigs = ESSection.Clusters;

            if (clusterConfigs == null)
                throw new ConfigurationErrorsException("ElasticSearch cluster configuration cannot be found");

            ClusterElement requestedCluster = null;

            //Find the cluster
            foreach (ClusterElement cluster in clusterConfigs)
            {
                if (cluster.Name == clusterName)
                {
                    requestedCluster = cluster;
                    break;
                }
            }

            if (requestedCluster == null)
                throw new ConfigurationErrorsException("ElasticSearch cluster, " + clusterName + ", cannot be found in configuration.");


            return requestedCluster;
        }

        /// <summary>
        /// Gets the ESSiteWideSearchCollection elements based on the name of the collection
        /// </summary>
        /// <param name="searchCollectionName">The name of the search collection</param>
        public static ESSiteWideSearchCollectionElement GetSearchCollectionConfig(string searchCollectionName)
        {
            ESSiteWideSearchCollectionElementCollection searchCollections = ESSection.SiteWideSearchCollections;

            if (searchCollections == null)
                throw new ConfigurationErrorsException("ElasticSearch siteWideSearchCollections configuration cannot be found");

            //Find the cluster
            foreach (ESSiteWideSearchCollectionElement searchCollection in searchCollections)
            {
                if (searchCollection.Name == searchCollectionName)
                    return searchCollection;
            }

            throw new ConfigurationErrorsException("ElasticSearch SiteWideSearch collection, " + searchCollectionName + ", cannot be found in configuration.");
        }

        /// <summary>
        /// Details of the Elastic Search Section
        /// </summary>
        private static ElasticSearchSection ESSection
        {
            get
            {
                ElasticSearchSection section = (ElasticSearchSection)ConfigurationManager.GetSection(_esconfigPath);
                if (section == null)
                    throw new ConfigurationErrorsException("ElasticSearch configuration section cannot be found");

                return section;
            }
        }

    }
}
