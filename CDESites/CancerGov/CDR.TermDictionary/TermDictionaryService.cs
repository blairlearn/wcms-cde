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

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SearchGeneticsJSON/{language}?searchTerm={criteria}&MaxRows={maxRows}&Contains={contains}")]
        [OperationContract]
        public TermDictionaryServiceCollection SearchGeneticsJSON(string language, string criteria, int maxRows, bool contains)
        {
            return Search(language, criteria, maxRows, contains,"Genetics", "Health professional");
        }



        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SuggestJSON/{language}?term={criteria}")]
        [OperationContract]
        public TermDictionaryServiceCollection SuggestJSON(string language, string criteria)
        {
            // The below values not used by the system. Retained so it can used in the future for 
            // customization.
            int maxRows = 10;
            bool contains = false;

            return Search(language, criteria, maxRows, contains);
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SuggestGeneticsStartsJSON/{language}?term={criteria}")]
        [OperationContract]
        public TermDictionaryServiceCollection SuggestGeneticsStartsJSON(string language, string criteria)
        {
            // The below values not used by the system. Retained so it can used in the future for 
            // customization.
            int maxRows = 10;
            bool contains = false;

            return Search(language, criteria, maxRows, contains, "Genetics", "Health professional");
        }

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SuggestGeneticsContainsJSON/{language}?term={criteria}")]
        [OperationContract]
        public TermDictionaryServiceCollection SuggestGeneticsContainsJSON(string language, string criteria)
        {
            // The below values not used by the system. Retained so it can used in the future for 
            // customization.
            int maxRows = 10;
            bool contains = true;

            return Search(language, criteria, maxRows, contains, "Genetics", "Health professional");
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

        #region GetDictionary Serviche 

        /// <summary>
        /// This method is used to return a single TermDictionaryItem as XML output based on a termId which is CDRId of the
        /// term dictionary. 
        /// </summary>
        /// <param name="language">This is either English or Spanish</param>
        /// <param name="termId">The CDRID of the term</param>
        /// <param name="audience">This value can be Patient or HealthProfessional</param>
        /// <returns>Returns the object if found in the database else TermDictionaryServiceItem object with 
        /// null TermDictionaryDetail property</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "GetTermDictionaryByIdXML/{language}?TermId={termId}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByIdXML(string language, int termId, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ById, language, termId, String.Empty, audience);
        }

        /// <summary>
        /// This method is used to return a single TermDictionaryItem as JSON output based on a termId which is CDRId of the
        /// term dictionary. 
        /// </summary>
        /// <param name="language">This is either English or Spanish</param>
        /// <param name="termId">The CDRID of the term</param>
        /// <param name="audience">This value can be Patient or HealthProfessional</param>
        /// <returns>Returns the object if found in the database else TermDictionaryServiceItem object with 
        /// null TermDictionaryDetail property</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetTermDictionaryByIdJSON/{language}?TermId={termId}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByIdJSON(string language, int termId, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ById, language, termId, String.Empty, audience);
        }


        /// <summary>
        /// This method is used to return a single TermDictionaryItem as XML output based on a termName
        /// </summary>
        /// <param name="language">This is either English or Spanish</param>
        /// <param name="termName">The name of the term</param>
        /// <param name="audience">This value can be Patient or HealthProfessional</param>
        /// <returns>Returns the TermDictionaryServiceItem object if found in the database else TermDictionaryServiceItem object with 
        /// null TermDictionaryDetail property</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "GetTermDictionaryByNameXML/{language}?TermName={termName}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByNameXML(string language, string termName, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ByName, language, 0, termName, audience);
        }


        /// <summary>
        /// This method is used to return a single TermDictionaryItem as JSON output based on a termName
        /// </summary>
        /// <param name="language">This is either English or Spanish</param>
        /// <param name="termName">The name of the term</param>
        /// <param name="audience">This value can be Patient or HealthProfessional</param>
        /// <returns>Returns the TermDictionaryServiceItem object if found in the database else TermDictionaryServiceItem object with 
        /// null TermDictionaryDetail property</returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetTermDictionaryByNameJSON/{language}?TermName={termName}&Audience={audience}")]
        [OperationContract]
        public TermDictionaryServiceItem GetTermDictionaryByNameJSON(string language, string termName, string audience)
        {
            return getTermDictionary(TermDefinitionByType.ByName, language, 0, termName, audience);
        }

        #endregion

        #region GetTermDictionaryList Service
        /// <summary>
        /// Returns a TermDictionaryServiceList as xml output which contains a collection of Term Dictionary items and the total number of records.
        /// </summary> 
        /// <param name="language">This English or Spanish</param>
        /// <param name="criteria">The search string.</param>
        /// <param name="contains">If this is true the search string is searched to match anywhere in the sentence. If false it 
        /// search string is searched to match the beginning</param>
        /// <param name="maxRows">Max number of records that is returned per request.</param>
        /// <param name="pageNumber">If pageNumber is > 1 then maxrows is used for records per page. If -1 then result 
        /// just contains the topN records which is equal to MaxRows</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Xml,
            UriTemplate = "GetTermDictionaryListXml/{language}?searchTerm={criteria}&Contains={contains}&MaxRows={maxRows}&PageNumber={pageNumber}")]
        [OperationContract]
        public TermDictionaryServiceList GetTermDictionaryListXml(string language, string criteria, bool contains, int maxRows, int pageNumber)
        {
            return getTermDictionaryList(language, criteria, contains, maxRows, pageNumber);
        }

        /// <summary>
        /// Returns a TermDictionaryServiceList as JSON output which contains a collection of Term Dictionary items and the total number of records.
        /// </summary> 
        /// <param name="language">This English or Spanish</param>
        /// <param name="criteria">The search string.</param>
        /// <param name="contains">If this is true the search string is searched to match anywhere in the sentence. If false it 
        /// search string is searched to match the beginning</param>
        /// <param name="maxRows">Max number of records that is returned per request.</param>
        /// <param name="pageNumber">If pageNumber is > 1 then maxrows is used for records per page. If -1 then result 
        /// just contains the topN records which is equal to MaxRows</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetTermDictionaryListJson/{language}?searchTerm={criteria}&Contains={contains}&MaxRows={maxRows}&PageNumber={pageNumber}")]
        [OperationContract]
        public TermDictionaryServiceList GetTermDictionaryList(string language, string criteria, bool contains, int maxRows, int pageNumber)
        {
            return getTermDictionaryList(language, criteria, contains, maxRows, pageNumber);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Returns a TermDictionaryServiceList which contains a collection of Term Dictionary 
        /// items and the total number of records.
        /// </summary> 
        /// <param name="language"></param>
        /// <param name="criteria"></param>
        /// <param name="contains"></param>
        /// <param name="maxRows">Maxrows is treated as recordPerPage or the topN records.
        /// Used for records per page when pagenumber is 0 or greater than 0. 
        /// If pagenumber is -1 number records returned is specified by maxRows. 
        /// If maxRows=0 and pageNumber=-1 all records are returned.</param>
        /// <param name="pageNumber">Specifies the pagenumber for which the records should be returned.
        /// If pagenumber is -1 number records returned is specified by maxRows. 
        /// If maxRows=0 and pageNumber=-1 all records are returned.</param>
        /// <returns></returns>
        private TermDictionaryServiceList getTermDictionaryList(string language, string criteria, bool contains, int maxRows, int pageNumber)
        {
            TermDictionaryServiceList sc = new TermDictionaryServiceList();
            sc.TermDictionaryServiceCollection = new TermDictionaryServiceCollection();
            sc.TotalRecordCount = 0;

            try
            {
                int totalRecordCount = 0;

                // No criteria specified
                if (string.IsNullOrEmpty(criteria))
                    return sc;

                // if maxrows is 0 and pagenumber is > 0 then return empty.
                if ((maxRows == 0 && pageNumber >= 0) || (maxRows < 0))
                    return sc;

                if (pageNumber == 0)
                    pageNumber = 1;

                // language passed to an enum
                DisplayLanguage displayLanguage =
                    (DisplayLanguage)Enum.Parse(typeof(DisplayLanguage), language);

                // Call the database query
                TermDictionaryCollection dc =
                    TermDictionaryManager.GetTermDictionaryList(language, criteria, contains, maxRows, pageNumber, ref totalRecordCount);

                // Use Linq to extract the data from the business layer and create 
                // the service data objects
                sc.TermDictionaryServiceCollection.AddRange(
                    from entry in dc
                    select createTermDictionarySvcItem(entry)
                );

                sc.TotalRecordCount = totalRecordCount;
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
            return Search(language, criteria, maxRows, contains, "Cancer.gov", "Patient");
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
        /// <param name="dictionary">Which Term dicitonary to search - Cancer.gov or Genetics</param>
        /// <param name="audience">Definition audience - Patient or Health professional</param>
        /// <returns>Returns the search results</returns>
        private TermDictionaryServiceCollection Search(string language, string criteria, int maxRows, bool contains, string dictionary, string audience)
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
                    TermDictionaryManager.Search(language, criteria, maxRows, contains,dictionary, audience);

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

        /// <summary>
        /// A private method that is used by both GetTermDictionary by id and Name.
        /// </summary>
        /// <param name="byType">The  value that specifies the type of search to be performed.</param>
        /// <param name="language">This is English or Spanish</param>
        /// <param name="termId">The CDRID of the term</param>
        /// <param name="termName">The term name</param>
        /// <param name="audience">This Patient or HealthProfessional</param>
        /// <returns>TermDictionaryServiceItem instance</returns>
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

                if (byType == TermDefinitionByType.ById)
                    termDicDataItem = TermDictionaryManager.GetDefinitionByTermID(language, termId.ToString(), audience, 1);
                else
                    termDicDataItem = TermDictionaryManager.GetDefinitionByTermName(displayLanguage, termName, audience, 1);

                if (termDicDataItem != null)
                    termDicSvcItem = createTermDictionarySvcItem(termDicDataItem);
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
