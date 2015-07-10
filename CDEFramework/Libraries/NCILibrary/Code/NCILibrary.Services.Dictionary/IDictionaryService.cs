using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using NCI.Services.Dictionary.BusinessObjects;

namespace NCI.Services.Dictionary
{
    /* NOTE: If you change the interface name here, you must also update the reference to it in Web.config.
     * 
     * A possibly controversial implementation detail: These methods take enums as parameters.
     * 
     * The plus side is, unlike strings, WCF will guarantee that the parameter values are valid,
     * thus simplifying the validation code.
     * 
     * There are some recommendations against using enums in web services,
     * http://www.25hoursaday.com/weblog/2005/08/31/WhyYouShouldAvoidUsingEnumeratedTypesInXMLWebServices.aspx
     * but these revolve around using them as outputs (A calling routine might not handle a newly added
     * enum value correctly).
     * 
     * These parameters are input-only.     * 
     */

    /// <summary>
    /// Declares the public methods for the dictionary service and defines the rules for
    /// mapping the parts of the dictionary URLs into method parameters.
    /// </summary>
    [ServiceContract]
    public interface IDictionaryService
    {
        /// <summary>
        /// Retrieves a single dictionary term based on its specific term ID.
        /// </summary>
        /// <param name="termId">The ID of the term to be retrieved</param>
        /// <param name="dictionary">The dictionary to retreive the term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/GetTerm?termID={termId}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        TermReturn GetTerm(String termId, DictionaryType dictionary, Language language);

        /// <summary>
        /// Performs a search for terms with names matching searchText.
        /// </summary>
        /// <param name="searchText">text to search for.</param>
        /// <param name="searchType">The type of search to perform.
        ///     Valid values are:
        ///         Begins - Search for terms beginning with searchText.
        ///         Contains - Search for terms containing searchText.
        ///         Magic - Search for terms beginning with searchText, followed by those containing searchText.
        /// </param>
        /// <param name="offset">Offset into the list of matches for the first result to return.</param>
        /// <param name="maxResults">The maximum number of results to return. Must be at least 10.</param>
        /// <param name="dictionary">The dictionary to retreive the term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/search?searchText={searchText}&searchType={searchType}&offset={offset}&maxResults={maxResults}&language={language}&dictionary={dictionary}")]
        [OperationContract]
        SearchReturn Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language);


        /// <summary>
        /// Performs a search for terms with names matching searchText.  This alternate version is
        /// invoked by a POST request.
        /// </summary>
        /// <param name="paramBlock"></param>
        /// <param name="dictionary">The dictionary to retreive the term from.
        ///     Valid values are
        ///        term - Dictionary of Cancer Terms
        ///        drug - Drug Dictionary
        ///        genetic - Dictionary of Genetics Terms
        /// </param>
        /// <param name="language">The term's desired language.
        ///     Supported values are:
        ///         en - English
        ///         es - Spanish
        /// </param>
        /// <returns></returns>
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "v1/SearchPost")]
        [OperationContract]
        SearchReturn SearchPost(SearchInputs paramBlock);
       
    }

}
