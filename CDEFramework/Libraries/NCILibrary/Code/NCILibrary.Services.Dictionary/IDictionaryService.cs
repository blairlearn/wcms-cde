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
    // NOTE: If you change the interface name here, you must also update the reference to it in Web.config.
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
        /// <param name="language">The term's desired language.</param>
        /// <param name="version">API version number</param>
        /// <returns></returns>
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{version}/{language}/{dictionary}/GetTerm?termID={termId}")]
        [OperationContract]
        TermReturn GetTerm(String termId, String dictionary, String language, String version);

        // Placeholder.  We really want to return something which *contains* an array.
        // Search method for calling via GET requests.
        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{version}/{language}/{dictionary}/search?param1={param1}&param2={param2}")]
        [OperationContract]
        DictionaryTerm[] SearchGet(String param1, String param2, String dictionary, String language, String version);

        // Placeholder.  We really want to return something which *contains* an array..
        // Search method for calling via POST requests.
        [WebInvoke(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "{version}/{language}/{dictionary}/SearchPost")]
        [OperationContract]
        DictionaryTerm[] SearchPost(SearchInputs paramBlock, String dictionary, String language, String version);
       
    }

}
