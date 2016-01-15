using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Elasticsearch.Net.ConnectionPool;

using NCI.Search.Configuration;
using NCI.Search;
using NCI.Logging;
using Elasticsearch.Net.Exceptions;
using System.Threading;



namespace NCI.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class ESSiteWideSearchProvider : NCI.Search.SiteWideSearchProviderBase
    {       
        /// <summary>
        /// Gets the search results from this SiteWideSearch provider.
        /// </summary>
        /// <param name="searchCollection">The search collection.</param>
        /// <param name="term">The term.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public override ISiteWideSearchResultCollection GetSearchResults(string searchCollection, string term, int pageSize, int offset)
        {

            ESSiteWideSearchCollectionElement searchCollConfig = ElasticSearchConfig.GetSearchCollectionConfig(searchCollection);
            ClusterElement clusterConfig = searchCollConfig.ClusterElementDetails;
            var connectionPool = new SniffingConnectionPool(searchCollConfig.ClusterNodes);
            var config = new ConnectionConfiguration(connectionPool)
                            .UsePrettyResponses()
                            .MaximumRetries(clusterConfig.MaximumRetries)//try 5 times if the server is down
                            .ExposeRawResponse()
                            .ThrowOnElasticsearchServerExceptions();
                                       
            var esClient = new ElasticsearchClient(config);

            string[] fieldList = ElasticSearchConfig.GetFields(searchCollection);


            //do search_template 
            var _body = new
            {
                template = new
                {
                    id = searchCollConfig.Template
                },
                @params = new
                {
                    my_value = term,
                    my_size = pageSize.ToString(),
                    my_from = offset.ToString(),
                    my_site = searchCollConfig.Site,
                    my_fields = fieldList
                }
            };

            ElasticsearchResponse<DynamicDictionary> results = null;

            try
            {
                results = esClient.SearchTemplate(searchCollConfig.Index, _body);
            }
            catch (MaxRetryException ex)
            {
                try 
                {
                    //log the maximum retry excpetion
                    Logger.LogError(this.GetType().ToString(), "Error using the ESClient Search Template method. Maximum retry exception: ", NCIErrorLevel.Error, ex);
                    //sleep for 5 seconds 
                    Thread.Sleep(clusterConfig.ConnectionTimeoutDelay);
                    //try to fetch results again
                    results = esClient.SearchTemplate(searchCollConfig.Index, _body);
                }
                catch (Exception e)
                {
                    Logger.LogError(this.GetType().ToString(), "Error using the ESClient Search Template method.", NCIErrorLevel.Error, e);
                    throw e;
                }


            }//retry catch log error sleep retry
            
            List<ESSiteWideSearchResult> foundTerms = new List<ESSiteWideSearchResult>(pageSize);

            if (results.Success)
            {
                var stringResponse = results.Response;
                try
                {

                    foreach (var hit in results.Response["hits"].hits)
                    {

                        string title = SetFieldValue(hit, "title");
                        if (string.IsNullOrEmpty(title))
                            title = "Untitled";

                        string url = SetFieldValue(hit, "url");

                        string description = SetFieldValue(hit, "metatag.description");
                                          

                        foundTerms.Add(new ESSiteWideSearchResult(title, url, description));


                    }


                }

                catch (Exception ex)
                {
                    Logger.LogError(this.GetType().ToString(), "Error retrieving search results.", NCIErrorLevel.Error, ex);
                    throw ex;

                }

            }

            else
            {
                string prettyResponse = "";
                //log the raw response message 
                //if there is not response them log with message - "No message in response"
                if (!string.IsNullOrEmpty(System.Text.Encoding.Default.GetString(results.ResponseRaw)))
                    prettyResponse = System.Text.Encoding.Default.GetString(results.ResponseRaw);
                else
                    prettyResponse = "No message in response";

                Logger.LogError(this.GetType().ToString(), "Search failed. Http Status Code:" + results.HttpStatusCode.Value.ToString() + ". Message: " + prettyResponse, NCIErrorLevel.Error);

                throw (new Exception("Search failed. Http Status Code:" + results.HttpStatusCode.Value.ToString() + ". Message: " + prettyResponse));
            }
                                
            //Return the results
            return new ESSiteWideSearchResultCollection(foundTerms.AsEnumerable())
            {
                ResultCount = results.Response["hits"].total
            };
        }

        private string SetFieldValue(dynamic hit, string field)
        {
            string fieldValue = "";

            try
            {
                if (hit.fields[field][0] != null)
                {
                    fieldValue = hit.fields[field][0];
                }
            }

            catch (KeyNotFoundException keynotfound)
            {
                fieldValue = "";
            }

            catch (Exception e)
            {
                Logger.LogError(this.GetType().ToString(), "Error when setting field values for field:" + field, NCIErrorLevel.Error, e);
                throw e;
            }
            return fieldValue;
        }

      
    }
}
