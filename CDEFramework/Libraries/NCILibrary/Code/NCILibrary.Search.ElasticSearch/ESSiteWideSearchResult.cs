using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NCI.Search;

namespace NCI.Search
{
    /// <summary>
    /// This class a single search results
    /// </summary>
    public class ESSiteWideSearchResult : ISiteWideSearchResult
    {
        /// <summary>
        /// Title of the result
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL of the result
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// URL of the result
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Not being used at the moment but may be used in a future release
        /// </summary>
        public string WrappedUrl { get; set; }

        /// <summary>
        /// Not being used at the moment but may be used in a future release
        /// </summary>
        public string ContentType { get; set; }

        public ESSiteWideSearchResult()
        {
        }

        public ESSiteWideSearchResult(string title, string url, string description)
        {
            this.Title = title;
            this.Url = url;
            this.Description = description;
        }
    }
}
