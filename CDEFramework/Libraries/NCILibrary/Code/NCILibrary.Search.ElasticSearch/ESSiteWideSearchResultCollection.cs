using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search
{
    public class ESSiteWideSearchResultCollection : ISiteWideSearchResultCollection
    {
        private IEnumerable<ESSiteWideSearchResult> searchResults;

        public int ResultCount { get; set; }

        List<ESSiteWideSearchResult> SearchResults { get; set; }

        //protected ESSearchResultCollection(IEnumerable<ESSearchResult> results)
        //{
        //    this.searchResults = results;

        //}

        public List<ESSiteWideSearchResult> ESSearchCollection()
        {
            SearchResults = (List<ESSiteWideSearchResult>)searchResults;
            return SearchResults;
        }

        public ESSiteWideSearchResultCollection(IEnumerable<ESSiteWideSearchResult> results)
        {
            this.searchResults = results;
        }


        #region IEnumerable<ESSearchResult> Members
        /// <summary>
        /// Get Enumerator object
        /// </summary>
        /// <returns>Enumerable list</returns>


        public IEnumerator<ISiteWideSearchResult> GetEnumerator()
        {

            return (IEnumerator<ISiteWideSearchResult>)searchResults.GetEnumerator();
        }

        #endregion

        //#region IEnumerable Members
        ///// <summary>
        ///// Get IEnumerable
        ///// </summary>
        ///// <returns>the Enumerable list</returns>
        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return searchResults.GetEnumerator();
        //}

        //#endregion

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)(searchResults)).GetEnumerator();
        }
    }

}
