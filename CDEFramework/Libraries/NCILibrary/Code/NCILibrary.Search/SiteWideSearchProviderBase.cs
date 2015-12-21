using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Provider;

namespace NCI.Search
{
    /// <summary>
    /// Defines the base for all SiteWideSearch providers
    /// </summary>
    public abstract class SiteWideSearchProviderBase : ProviderBase
    {
        /// <summary>
        /// Gets the search results from this SiteWideSearch provider.
        /// </summary>
        /// <param name="searchCollection">The search collection.</param>
        /// <param name="term">The term.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public abstract ISiteWideSearchResultCollection GetSearchResults(string searchCollection, string term, int pageSize, int offset);
    }

}
