using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NCI.Logging;
using NCI.Web.Dictionary.BusinessObjects;
using NCI.Services.Dictionary;

namespace NCI.Web.Dictionary
{
    public class DictionaryAppManager
    {
        static Log log = new Log(typeof(DictionaryAppManager));


        

        /// <summary>
        /// Get Term from Dictionary Service to be deserialized and returned to the app module
        /// </summary>
        /// <param name="termId">int of the CDR ID</param>
        /// <param name="dictionary">which dictionary is being passed through</param>
        /// <param name="language">English/Spanish term</param>
        /// <param name="version">the version of the dictionary service</param>
        /// <returns>the term deserialized and the meta data from the database</returns>
        public DictionaryTerm GetTerm(int termId, DictionaryType dictionary, Language language, String version)
        {
            
            
            //sets up Dictionary Service so methods can be called
            DictionaryService service = new DictionaryService();

            //Sets up the services return type so that the meta data can be transfered to this term return.

            NCI.Services.Dictionary.BusinessObjects.TermReturn termRet = new NCI.Services.Dictionary.BusinessObjects.TermReturn();
            try
            {
                termRet = service.GetTerm(termId, dictionary, language);
            }
            catch (Exception ex)
            {
                log.error("Error in Dictionary Web Service for Get Term: " + ex);
            }

            //String of JSON returned from the Database to be deserialized.
            DictionaryTerm dicTerm = null;
            
            try
            {
                //deserialize term details as returned by the service layer (termret.term)
                dicTerm = JsonConvert.DeserializeObject<DictionaryTerm>(termRet.Term);
            }
            catch (JsonReaderException ex)
            {
                log.error("Error in Json string from service: " + ex.ToString());
            }

           return dicTerm;


            
        }


        /// <summary>
        /// Dictionary search that will return a list of dictionary terms based on the dictionary type passed in
        /// and what kind of search is desired
        /// </summary>
        /// <param name="searchText">the text that has been typed into the search box</param>
        /// <param name="searchType">the type of search being executed</param>
        /// <param name="offset">how many to offset the first return result</param>
        /// <param name="maxResults">the max results to return<remarks>int.maxvalue should be used for retrieving all unless we have more</remarks></param>
        /// <param name="dictionary">the dictionary type (cancert term, drug, genetic)</param>
        /// <param name="language">English/Spanish</param>
        /// <returns>returns a list of dictioanry terms and related metadata</returns>
        public IEnumerable<DictionarySearchResult> Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language)
        {
            DictionaryService service = new DictionaryService();

            //sets up SearchReturn from Web Service
            NCI.Services.Dictionary.BusinessObjects.SearchReturn searchRet = null;

            try
            {
                searchRet = service.Search(searchText, searchType, offset, maxResults, dictionary, language);
            }
            catch (Exception ex)
            {
                log.error("There is an error in the Search Method of the Dictionary Service: " + ex);
            }

            List<DictionarySearchResult> resultList = DeserializeList(searchRet.Result);
            
            return resultList.AsEnumerable();

            /*if (srchReturn.Result.Length > 0)
            {

                //set meta data
                srchReturn.Meta = new SearchReturnMeta();
                srchReturn.Meta.Audience = searchRet.Meta.Audience;
                srchReturn.Meta.Language = searchRet.Meta.Language;
                srchReturn.Meta.Offset = searchRet.Meta.Offset;
                srchReturn.Meta.ResultCount = searchRet.Meta.ResultCount;
                srchReturn.Meta.Messages = searchRet.Meta.Messages;

            }*/



        }

        /// <summary>
        /// Term suggestions from what is being typed into the search box.  Used for autosuggest
        /// </summary>
        /// <param name="searchText">the string being typed</param>
        /// <param name="searchType">Type of search being done (contains, starts with, etc.)</param>
        /// <param name="dictionary">Which dictionary is being searched</param>
        /// <param name="language">Language</param>
        /// <returns>returns list of suggestions</returns>
        public SuggestReturn SearchSuggest(String searchText, SearchType searchType, DictionaryType dictionary, Language language)
        {
           

            SuggestReturn sugRet = new SuggestReturn();

            DictionarySuggestion[] results = new DictionarySuggestion[] { };
            DictionaryService service = new DictionaryService();
            int count=0;
            SuggestReturnMeta meta = new SuggestReturnMeta();
            NCI.Services.Dictionary.BusinessObjects.SuggestReturn suggestRet = service.SearchSuggest(searchText, searchType, dictionary, language);

            foreach (NCI.Services.Dictionary.BusinessObjects.DictionarySuggestion m in suggestRet.Result)
            {
                results[count].ID = m.ID;
                results[count].Term = m.Term;
                count++;
            }

            sugRet.Result = results;

            sugRet.Meta.ResultCount = suggestRet.Meta.ResultCount;
            sugRet.Meta.Messages = suggestRet.Meta.Messages;

            return sugRet;

        }

        /// <summary>
        /// Expand Search is used for A-Z Lists on dictionaries
        /// </summary>
        /// <param name="searchText">search text</param>
        /// <param name="includeTypes">types to include</param>
        /// <param name="offset">how many results to offset</param>
        /// <param name="numResults"># of results to return</param>
        /// <param name="dictionary">The dictionary</param>
        /// <param name="language">which language</param>
        /// <param name="version">version of dictionary service</param>
        /// <returns>Collection of Dictionary Search Results</returns>
        public IEnumerable<DictionarySearchResult> Expand(String searchText, String includeTypes, int offset, int maxResults, DictionaryType dictionary, Language language, String version)
        {

            DictionaryService service = new DictionaryService();
            NCI.Services.Dictionary.BusinessObjects.SearchReturn expandRet = null;
            try
            {
                expandRet = service.Expand(searchText, includeTypes, offset, maxResults, dictionary, language);
            }
            catch (Exception ex)
            {
                log.error("Error in Expand Method in Dictionary Web Service: " + ex);
            }

            List<DictionarySearchResult> expansionList = DeserializeList(expandRet.Result);

            

            return expansionList.AsEnumerable();
            /*if (exRet.Result.Length > 0)
            {
              
                //set up meta data
                exRet.Meta = new SearchReturnMeta();
                exRet.Meta.ResultCount = expandRet.Meta.ResultCount;
                exRet.Meta.Messages = expandRet.Meta.Messages;
            }


            return exRet;*/
        }

        /// <summary>
        /// this is to utilize shared code between search and expand as they return the same objects and do the same action
        /// </summary>
        /// <param name="list">the string list that the dictionary service would return</param>
        /// <returns>deserialized list of DictionarySearchResults</returns>
        private List<DictionarySearchResult> DeserializeList(NCI.Services.Dictionary.BusinessObjects.DictionaryExpansion[] list)
        {
            List<DictionarySearchResult> returnList = new List<DictionarySearchResult>(); ;

            foreach (NCI.Services.Dictionary.BusinessObjects.DictionaryExpansion m in list)
            {
                try
                {
                    int id  = m.ID;
                    string termName  = m.MatchedTerm;
                    DictionaryTerm term = JsonConvert.DeserializeObject<DictionaryTerm>(m.TermDetail);

                    DictionarySearchResult expansion = new DictionarySearchResult(id, termName, term);
                    returnList.Add(expansion);
                }
                catch (JsonReaderException ex)
                {
                    log.error("Error in Json string from service: " + ex.ToString());
                }

            }

            return returnList;
        }   
    }
}
