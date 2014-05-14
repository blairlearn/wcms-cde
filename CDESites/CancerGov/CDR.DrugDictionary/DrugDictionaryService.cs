using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using CancerGov.UI;
using CancerGov.Common.ErrorHandling;

namespace CancerGov.CDR.DrugDictionary
{
    /// <summary>
    /// This class is the user interface and provides the connection between 
    /// the WCF service and the business layer. A ServiceContract decoration is 
    /// required in order for it to be used with the WCF
    /// </summary>
    [ServiceContract]
    public class DrugDictionaryService
    {
        /// <summary>
        /// Method to interface through the WCF do get results from a database query
        /// This will return the data in JSON format
        /// </summary>
        /// <param name="language">The language used to query the database</param>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">Indicator on whether the text is to be search from the beginning of the text or anywhere in the string</param>
        /// <returns>Returns the search results</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SearchJSON?searchTerm={criteria}&MaxRows={maxRows}&Contains={contains}")]
        [OperationContract]
        public DrugDictionaryServiceCollection SearchJSON(string criteria, int maxRows, bool contains)
        {
            return Search(criteria, maxRows, contains);
        }

        /// <summary>
        /// Method to interface through the WCF do get results from a database query.
        /// This will return the data in XML format
        /// </summary>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">Indicator on whether the text is to be search from the beginning of the text or anywhere in the string</param>
        /// <returns>Returns the search results</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "SearchXML?searchTerm={criteria}&MaxRows={maxRows}&Contains={contains}")]
        [OperationContract]
        public DrugDictionaryServiceCollection SearchXML(string criteria, int maxRows, bool contains)
        {
            return Search(criteria, maxRows, contains);
        }

        /// <summary>
        /// Method used to query database for English language results. This will return the
        /// data in XML format. There is no way to create an additional WebGet so that we
        /// can return the same data in JSON format.
        /// </summary>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">Indicator on whether the text is to be search from the beginning of the text or anywhere in the string</param>
        /// <returns>Returns the search results</returns>
        private DrugDictionaryServiceCollection Search(string criteria, int maxRows, bool contains)
        {
            // create the collection variable
            DrugDictionaryServiceCollection sc = new DrugDictionaryServiceCollection();

            try
            {
                // Call the database query
                DrugDictionaryCollection dc =
                    DrugDictionaryManager.SearchNameOnly(criteria, maxRows, contains);

                // Use Linq to extract the data from the business layer and create 
                // the service data objects
                var collection = dc.ConvertAll(entry => new DrugDictionaryServiceItem(
                    entry.TermID,
                    entry.PreferredName,
                    string.Empty
                    ));

                sc.AddRange(collection);
            }
            catch (Exception ex)
            {
                // Log the error that occured
                CancerGovError.LogError("DrugDictionary", 2, ex);
            }

            return sc;
        }
    }
}
