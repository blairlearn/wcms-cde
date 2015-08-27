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

        // These values should come from the GateKeeper backend processing.
        const String AUDIENCE_PATIENT = "Patient";
        const String AUDIENCE_HEALTHPROF = "Health Professional";


        

        /// <summary>
        /// Get Term from Dictionary Service to be deserialized and returned to the app module
        /// </summary>
        /// <param name="termId">int of the CDR ID</param>
        /// <param name="dictionary">which dictionary is being passed through</param>
        /// <param name="language">English/Spanish term</param>
        /// <param name="version">the version of the dictionary service</param>
        /// <returns>the term deserialized and the meta data from the database</returns>
        public TermReturn GetTerm(int termId, DictionaryType dictionary, Language language, String version)
        {
            
            
            //sets up Dictionary Service so methods can be called
            DictionaryService service = new DictionaryService();

            //Sets up the services return type so that the meta data can be transfered to this term return.
            NCI.Services.Dictionary.BusinessObjects.TermReturn termRet = service.GetTerm(termId, dictionary, language);

            //String of JSON returned from the Database to be deserialized.
            String jsonObject =termRet.Term;
            String newJsonObject = "{" + jsonObject + "}";
            DictionaryTerm dicTerm = new DictionaryTerm();
            TermReturn term = new TermReturn();
            term.Meta = new TermReturnMeta();
            try
            {
                dicTerm = JsonConvert.DeserializeObject<DictionaryTerm>(newJsonObject);
            }
            catch (JsonReaderException ex)
            {
                log.error("Error in Json string from service: " + ex.ToString());
            }

            term.Term = dicTerm;

            if (term.Term != null)
            {
                //set Meta Data from Database
                term.Meta.Language = termRet.Meta.Language;
                term.Meta.Audience = termRet.Meta.Audience;
                term.Meta.Messages = termRet.Meta.Messages;
            }

            return term;
        }


        /// <summary>
        /// Dictionary search that will return a list of dictionary terms based on the dictionary type passed in
        /// and what kind of search is desired
        /// </summary>
        /// <param name="searchText">the text that has been typed into the search box</param>
        /// <param name="searchType">the type of search being executed</param>
        /// <param name="offset">how many to offset the first return result</param>
        /// <param name="maxResults">the max results to return</param>
        /// <param name="dictionary">the dictionary type (cancert term, drug, genetic)</param>
        /// <param name="language">English/Spanish</param>
        /// <returns>returns a list of dictioanry terms and related metadata</returns>
        public SearchReturn Search(String searchText, SearchType searchType, int offset, int maxResults, DictionaryType dictionary, Language language)
        {
            log.debug(string.Format("Enter Search( {0}, {1}, {2}, {3}, {4}, {5}).", searchText, searchType, offset, maxResults, dictionary, language));
            DictionaryService service = new DictionaryService();



            SearchReturn srchReturn = new SearchReturn();
            
            NCI.Services.Dictionary.BusinessObjects.SearchReturn searchRet = service.Search(searchText, searchType, offset, maxResults, dictionary, language);


            List<DictionaryExpansion> resultList = new List<DictionaryExpansion>(searchRet.Meta.ResultCount);

            foreach (NCI.Services.Dictionary.BusinessObjects.DictionaryExpansion m in searchRet.Result)
            {
                try
                {
                    int id = m.ID;
                    string termName = m.MatchedTerm;
                    DictionaryTerm term = JsonConvert.DeserializeObject<DictionaryTerm>("{" + m.TermDetail + "}");

                    DictionaryExpansion expansion = new DictionaryExpansion(id, termName, term);
                    resultList.Add(expansion);
                }
                catch (JsonReaderException ex)
                {
                    log.error("Error in Json string from service: " + ex.ToString());
                }

            } 
            
            srchReturn.Result = resultList.ToArray();

            if (srchReturn.Result.Length > 0)
            {

                //set meta data
                srchReturn.Meta = new SearchReturnMeta();
                srchReturn.Meta.Audience = searchRet.Meta.Audience;
                srchReturn.Meta.Language = searchRet.Meta.Language;
                srchReturn.Meta.Offset = searchRet.Meta.Offset;
                srchReturn.Meta.ResultCount = searchRet.Meta.ResultCount;
                srchReturn.Meta.Messages = searchRet.Meta.Messages;

            }


            return srchReturn;

        }

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

        public SearchReturn Expand(String searchText, String includeTypes, int offset, int maxResults, DictionaryType dictionary, Language language, String version)
        {

            SearchReturn exRet = new SearchReturn();
            DictionaryService service = new DictionaryService();
            NCI.Services.Dictionary.BusinessObjects.SearchReturn expandRet = service.Expand(searchText, includeTypes, offset, maxResults, dictionary, language);


            List<DictionaryExpansion> expansionList = new List<DictionaryExpansion>(expandRet.Meta.ResultCount);

            foreach (NCI.Services.Dictionary.BusinessObjects.DictionaryExpansion m in expandRet.Result)
            {
                try
                {
                    int id  = m.ID;
                    string termName  = m.MatchedTerm;
                    DictionaryTerm term = JsonConvert.DeserializeObject<DictionaryTerm>("{" + m.TermDetail + "}");

                    DictionaryExpansion expansion = new DictionaryExpansion(id, termName, term);
                    expansionList.Add(expansion);
                }
                catch (JsonReaderException ex)
                {
                    log.error("Error in Json string from service: " + ex.ToString());
                }

            }

            exRet.Result = expansionList.ToArray();
            if (exRet.Result.Length > 0)
            {
              
                //set up meta data
                exRet.Meta = new SearchReturnMeta();
                exRet.Meta.ResultCount = expandRet.Meta.ResultCount;
                exRet.Meta.Messages = expandRet.Meta.Messages;
            }


            return exRet;
        }

        
        
    }
}
