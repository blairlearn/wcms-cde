using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common.Logging;
using Elasticsearch.Net;
using Elasticsearch.Net.Connection;
using Elasticsearch.Net.ConnectionPool;
using Elasticsearch.Net.Exceptions;
using NCI.Search.Configuration;

namespace NCI.Search
{
    /// <summary>
    /// This is the maanger class for Elastic Search. It is used to return search results.
    /// </summary>
    public class ESSiteWideSearchProvider : NCI.Search.SiteWideSearchProviderBase
    {       
        static ILog log = LogManager.GetLogger(typeof(ESSiteWideSearchProvider));

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
                    log.Error("Error using the ESClient Search Template method. Maximum retry exception: ", ex);
                    //sleep for 5 seconds 
                    Thread.Sleep(clusterConfig.ConnectionTimeoutDelay);
                    //try to fetch results again
                    results = esClient.SearchTemplate(searchCollConfig.Index, _body);
                }
                catch (Exception e)
                {
                    log.Error("Error using the ESClient Search Template method.", e);
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
                        //title is a special case and when it is empty the value needs to be Untitled
                        string title = SetFieldValue(hit, "title");
                        if (string.IsNullOrEmpty(title))
                            title = "Untitled";

                        string url = SetFieldValue(hit, "url");

                        string description = SetFieldValue(hit, "metatag.description");
                                          

                        string type = SetFieldValue(hit, "metatag.dcterms.type");

                      
                        foundTerms.Add(new ESSiteWideSearchResult(title, url, description, type));

                    }


                }

                catch (Exception ex)
                {
                    log.Error("Error retrieving search results.", ex);
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

                log.ErrorFormat("Search failed. Http Status Code: {0}. Message: {1}", results.HttpStatusCode.Value, prettyResponse);

                throw (new Exception("Search failed. Http Status Code:" + results.HttpStatusCode.Value.ToString() + ". Message: " + prettyResponse));
            }
                                
            //Return the results
            return new ESSiteWideSearchResultCollection(foundTerms.AsEnumerable())
            {
                ResultCount = results.Response["hits"].total
            };
        }

        /// <summary>
        /// Sets the value of a field
        /// </summary>
        /// <param name="hit">Each hit returned as part of the results</param>
        /// <param name="field">The name of the field e.g. Title, URL, Description</param>
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
                //specifically catch the keynotfound exception and set the fieldValue to empty string
                //just checking of not null values does not work
                fieldValue = "";
            }

            catch (Exception e)
            {
                log.ErrorFormat("Error when setting field values for field: {0}", e, field);
                throw e;
            }
            return fieldValue;
        }

      
    }
}
