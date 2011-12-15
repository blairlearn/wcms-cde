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
using NCI.Web.CDE;
namespace CancerGov.CDR.TermDictionary
{
    /// <summary>
    /// This class is the user interface and provides the connection between 
    /// the WCF service and the business layer. A ServiceContract decoration is 
    /// required in order for it to be used with the WCF
    /// </summary>
    [ServiceContract]
    public class TermDictionaryService
    {
        private enum TermDefinitionByType { ById, ByName };

        #region Search Service
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
            UriTemplate = "SearchJSON/{language}?searchTerm={criteria}&MaxRows={maxRows}&Contains={contains}")]
        [OperationContract]
        public TermDictionaryServiceCollection SearchJSON(string language, string criteria, int maxRows, bool contains)
        {
            return Search(language, criteria, maxRows, contains);
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
            UriTemplate = "SearchXML/{language}?searchTerm={criteria}&MaxRows={maxRows}&Contains={contains}")]
        [OperationContract]
        public TermDictionaryServiceCollection SearchXML(string language, string criteria, int maxRows, bool contains)
        {
            return Search(language, criteria, maxRows, contains);
        }
        #endregion

        #region GetDictionary Service
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "GetTermDictionaryByIdXML/{language}?TermId={termId}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByIdXML(string language, int termId, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ById, language, termId, String.Empty, audience);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetTermDictionaryByIdJSON{language}?TermId={termId}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByIdJSON(string language, int termId, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ById, language, termId, String.Empty, audience);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "GetTermDictionaryByNameXML/{language}?TermName={termName}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByNameXML(string language, string termName, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ByName, language, 0, termName, audience);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetTermDictionaryByNameJSON/{language}?TermName={termName}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByNameJSON(string language, string termName, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ByName, language, 0, termName, audience);
        }

        #endregion

        #region GetTermDictionaryList Service
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "GetTermDictionaryListXml/{language}?searchTerm={criteria}&Contains={contains}&MaxRows={maxRows}&PageNumber=pageNumber")]
        [OperationContract]
        public TermDictionaryServiceCollection GetTermDictionaryListXml(string language, string criteria, bool contains, int maxRows, int pageNumber)
        {
            return getTermDictionaryList(language, criteria, contains, maxRows, pageNumber);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetTermDictionaryListJson/{language}?searchTerm={criteria}&Contains={contains}&MaxRows={maxRows}&PageNumber=pageNumber")]
        [OperationContract]
        public TermDictionaryServiceCollection GetTermDictionaryList(string language, string criteria, bool contains, int maxRows, int pageNumber)
        {
            return getTermDictionaryList(language, criteria, contains, maxRows, pageNumber);
        }
        #endregion

        #region Private Methods
        public TermDictionaryServiceCollection getTermDictionaryList(string language, string criteria, bool contains, int maxRows, int pageNumber)
        {
            TermDictionaryServiceCollection sc = new TermDictionaryServiceCollection();

            try
            {
                if (maxRows > 0 && pageNumber > 0)
                {
                    // language passed to an enum
                    DisplayLanguage displayLanguage =
                        (DisplayLanguage)Enum.Parse(typeof(DisplayLanguage), language);

                    // Call the database query
                    TermDictionaryCollection dc =
                        TermDictionaryManager.Search(language, criteria, contains, maxRows, pageNumber);

                    // Use Linq to extract the data from the business layer and create 
                    // the service data objects
                    sc.AddRange(
                        from entry in dc
                        select createTermDictionarySvcItem(entry)
                    );
                }
            }
            catch (Exception ex)
            {
                // Log the error that occured
                CancerGovError.LogError("TermDictionary", 2, ex);
            }

            return sc;
        }


        private TermDictionaryServiceItem createTermDictionarySvcItem(TermDictionaryDataItem termDictItem)
        {
            TermDictionaryServiceItem termDictionaryServiceItem = new TermDictionaryServiceItem(termDictItem.GlossaryTermID, termDictItem.TermName, null);
            termDictionaryServiceItem.TermDictionaryDetail = termDictItem;
            return termDictionaryServiceItem;
        }

        /// <summary>
        /// Method used to query database for English language results. This will return the
        /// data in XML format. There is no way to create an additional WebGet so that we
        /// can return the same data in JSON format.
        /// </summary>
        /// <param name="language">The language needed to do the lookup</param>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">Indicator on whether the text is to be search from the beginning of the text or anywhere in the string</param>
        /// <returns>Returns the search results</returns>
        private TermDictionaryServiceCollection Search(string language, string criteria, int maxRows, bool contains)
        {
            // create the collection variable
            TermDictionaryServiceCollection sc = new TermDictionaryServiceCollection();

            try
            {
                // language passed to an enum
                DisplayLanguage displayLanguage =
                    (DisplayLanguage)Enum.Parse(typeof(DisplayLanguage), language);

                // Call the database query
                TermDictionaryCollection dc =
                    TermDictionaryManager.Search(language, criteria, contains , maxRows, 0);

                // Use Linq to extract the data from the business layer and create 
                // the service data objects
                var collection = dc.ConvertAll(entry => new TermDictionaryServiceItem(
                    entry.GlossaryTermID,
                    entry.TermName,
                    string.Empty
                    ));

                sc.AddRange(collection);
            }
            catch (Exception ex)
            {
                // Log the error that occured
                CancerGovError.LogError("TermDictionary", 2, ex);
            }

            return sc;
        }

        private TermDictionaryServiceItem getTermDictionary(TermDefinitionByType byType, string language, int termId, string termName, string audience)
        {
            TermDictionaryServiceItem termDicSvcItem = null;

            try
            {
                // language passed to an enum calling this also validates the language value passed is meaningful DisplayLanguage
                DisplayLanguage displayLanguage =
                    (DisplayLanguage)Enum.Parse(typeof(DisplayLanguage), language);

                // Call the database query
                TermDictionaryDataItem termDicDataItem = null;
                
                if( byType == TermDefinitionByType.ById )
                    termDicDataItem = TermDictionaryManager.GetDefinitionByTermID(language, termId.ToString(), audience, 0);
                else
                    termDicDataItem = TermDictionaryManager.GetDefinitionByTermName(displayLanguage, termName, audience, 0);

                if (termDicDataItem != null)
                    termDicSvcItem = createTermDictionarySvcItem( termDicDataItem );
            }
            catch (Exception ex)
            {
                // Log the error that occured
                CancerGovError.LogError("TermDictionary", 2, ex);
            }

            return termDicSvcItem;
        }

        #endregion
    }
}
