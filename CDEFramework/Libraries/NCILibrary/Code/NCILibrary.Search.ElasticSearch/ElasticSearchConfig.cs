using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search.Configuration;
using System.Configuration;

namespace NCI.Search
{

    public static class ElasticSearchConfig
    {
        private static string _esconfigPath = "nci/search/elasticsearch";

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
