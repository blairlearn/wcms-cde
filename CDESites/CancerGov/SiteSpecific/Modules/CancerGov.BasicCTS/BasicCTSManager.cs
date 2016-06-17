using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Elasticsearch.Net.ConnectionPool;

using Nest;

using NCI.Search;
using CancerGov.ClinicalTrials.Basic.Configuration;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// An interface between the Search Screens and the Elastic Search DBs.  (A controller if you will)
    /// </summary>
    public class BasicCTSManager
    {
        private static readonly string CONFIG_SECTION_NAME = "nci/search/basicClinicalTrialSearch";

        private string _clusterName = "";
        private string _trialIndexType = "";
        private string _menuTermIndexType = "";
        private string _geoLocIndexType = "";
        private string _indexName = "";

        /// <summary>
        /// Creates a new instance of a BasicCTSManager
        /// </summary>
        public BasicCTSManager()
        {
            BasicClinicalTrialSearchSection config = (BasicClinicalTrialSearchSection)ConfigurationManager.GetSection(CONFIG_SECTION_NAME);

            if (config == null)
                throw new ConfigurationErrorsException("The configuration section, " + CONFIG_SECTION_NAME + ", cannot be found");

            if (string.IsNullOrWhiteSpace(config.SearchIndex))
                throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: searchIndex cannot be null or empty");

            if (string.IsNullOrWhiteSpace(config.TrialIndexType))
                throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: trialIndexType cannot be null or empty");

            if (string.IsNullOrWhiteSpace(config.GeoLocIndexType))
                throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: geoLocIndexType cannot be null or empty");

            if (string.IsNullOrWhiteSpace(config.MenuTermIndexType))
                throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: menuTermIndexType cannot be null or empty");

            if (string.IsNullOrWhiteSpace(config.SearchCluster))
                throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: searchCluster cannot be null or empty");

            this._indexName = config.SearchIndex;
            this._trialIndexType = config.TrialIndexType;
            this._geoLocIndexType = config.GeoLocIndexType;
            this._menuTermIndexType = config.MenuTermIndexType;
            this._clusterName = config.SearchCluster;
        }

        /// <summary>
        /// Gets a trial by the trials NCTID
        /// </summary>
        /// <param name="nctID">The CT.gov ID</param>
        /// <returns></returns>
        public TrialDescription Get(string nctID)
        {
            ElasticClient client = GetESConnection();

            //Using the GenericVersion will magically map the JSON to a strongly typed object.
            var response = client.Get<TrialDescription>(g => g
                .Index(_indexName)
                .Type(_trialIndexType)
                .Id(nctID)
            );

            return response.Source;
        }

        /// <summary>
        /// Gets the Geo Location for a ZipCode
        /// </summary>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public ZipLookup GetZipLookupForZip(string zipCode)
        {
            ElasticClient client = GetESConnection();

            var response = client.Get<ZipLookup>(g => g
                .Index(_indexName)
                .Type(_geoLocIndexType)
                .Id(zipCode)
            );

            return response.Source;
        }

        /// <summary>
        /// Searches for Trials given a set of parameters
        /// </summary>
        /// <param name="searchParams">The Search Parameters to use</param>
        /// <returns></returns>
        public TrialSearchResults Search(BaseCTSSearchParam searchParams)
        {
            ElasticClient client = GetESConnection();

            var response = client.Search<TrialSearchResult>(sd => 
                    searchParams.ModifySearchParams<TrialSearchResult>(
                        sd
                            .Index(_indexName)
                            .Type(_trialIndexType)
                    )
                );

            // If no results / error / etc then raise error

            return new TrialSearchResults(
                response.Total,
                response.Documents
            );
        }

        /// <summary>
        /// Searches for Trials given a set of parameters using a search template.
        /// </summary>
        /// <param name="searchParams">The Search Parameters to use</param>
        /// <returns></returns>
        public TrialSearchResults SearchTemplate(BaseCTSSearchParam searchParams)
        {
            ElasticClient client = GetESConnection();

            var response = client.SearchTemplate<TrialSearchResult>(sd => {
                sd = sd
                    .Index(_indexName)
                    .Type(_trialIndexType);

                sd = searchParams.SetSearchParams<TrialSearchResult>(sd);

                return sd;
            });

            // If no results / error / etc then raise error

            return new TrialSearchResults(
                response.Total,
                response.Documents
            );
        }

        /// <summary>
        /// Get Cancer Type Suggestions for a query
        /// </summary>
        /// <param name="query"></param>
        public IEnumerable<MenuTerm> GetCancerTypeSuggestions(string query)
        {
            ElasticClient client = GetESConnection();

                //STRING_DELIMS = " (),-./:"
                //STOPWORDS = set(["of", "the", "and"])

            string[] query_terms = Regex.Split(query, "[ (),-./:]+", RegexOptions.IgnoreCase);

            var response = client.Search<MenuTerm>(sd => sd
                .Index(_indexName)
                .Type(_menuTermIndexType)
                .From(0)
                .Size(3000)
                .Filter(f =>
                        {
                            FilterContainer ff = f;
                            foreach (string term in query_terms)
                            {
                                ff &= f.Prefix("SplitNames", term);
                            }


                            return ff;
                        }                    
                 )
            );

            if (response.Total > 0)
                return response.Documents.OrderBy(doc => doc.Name, StringComparer.CurrentCultureIgnoreCase);
            else
                return new MenuTerm[] { };
        }

        public string GetCancerTypeDisplayName(string cancertypeid, string hashid)
        {
            ElasticClient client = GetESConnection();

            var response = client.Search<MenuTerm>(sd => sd
                .Index(_indexName)
                .Type(_menuTermIndexType)
                .From(0)
                .Size(1)
                .Filter(f => 
                    //.Bool(b => b.Should(
                    //        innerFilter =>
                            {
                                FilterContainer ff = f.Term("CDRID", cancertypeid);
                                if (!String.IsNullOrWhiteSpace(hashid))
                                    ff &= f.Term("Hash", hashid);

                                return ff;
                            }
                        //)
                    //)
                )
            );

            if(response.Total == 1)
            {
                return response.Documents.First().Name;
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(hashid))
                    return GetCancerTypeDisplayName(cancertypeid, null);
                else
                    return null;
            }
            
        }


        private ElasticClient GetESConnection()
        {
            //Get The Cluster configuration
            //TODO: Make the cluster name a configurable item
            Uri[] clusterNodes = ElasticSearchConfig.GetClusterNodes(_clusterName);
            NCI.Search.Configuration.ClusterElement clusterConfig = ElasticSearchConfig.GetCluster(_clusterName);

            var connectionPool = new SniffingConnectionPool(clusterNodes);
            var config = new ConnectionSettings(connectionPool)
                            .UsePrettyResponses()
                            .MaximumRetries(clusterConfig.MaximumRetries)//try 5 times if the server is down
                            .ExposeRawResponse()
                            .ThrowOnElasticsearchServerExceptions();
            return new ElasticClient(config);

        }
    }
}
