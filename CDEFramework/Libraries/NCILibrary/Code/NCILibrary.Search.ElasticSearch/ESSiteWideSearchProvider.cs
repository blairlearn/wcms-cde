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



namespace NCI.Search
{
    /// <summary>
    /// 
    /// </summary>
    public class ESSiteWideSearchProvider : NCI.Search.SiteWideSearchProviderBase
    {
        static Log log = new Log(typeof(ESSiteWideSearchProvider));

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

            var connectionPool = new SniffingConnectionPool(searchCollConfig.ClusterNodes);
            var config = new ConnectionConfiguration(connectionPool);
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

            ElasticsearchResponse<DynamicDictionary> results = esClient.SearchTemplate(searchCollConfig.Index, _body);

            var stringResponse = results.Response;
            List<ESSiteWideSearchResult> foundTerms = new List<ESSiteWideSearchResult>(pageSize);

            try
            {


                foreach (var hit in results.Response["hits"].hits)
                {

                    string title = "";
                    if (hit.fields.title && hit.fields.title.length > 0)
                    {
                        title = hit.fields.title[0];
                    }

                    string url = "";
                    if (hit.fields.url && hit.fields.url.length > 0)
                    {
                        url = hit.fields.url[0];
                    }

                    string description = hit.fields.metatag[0];
                    if (hit.fields.metatag && hit.fields.metatag.length > 0)
                    {
                        description = hit.fields.metatag[0];
                    }

                    foundTerms.Add(new ESSiteWideSearchResult(title, url, description));


                }


            }

            catch (Exception ex)
            {
                log.error("Error retrieving search results.", ex);
            }


            //Return the results
            return new ESSiteWideSearchResultCollection(foundTerms.AsEnumerable())
            {
                ResultCount = results.Response["hits"].total
            };
        }
    }
}
