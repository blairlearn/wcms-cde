using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Client
{
    public class SiteWideSearchResult
    {
        /// <summary>
        /// The title of this item 
        /// </summary>
        /// <returns></returns>
        public string Title { get; set; }

        /// <summary>
        /// The URL for this result
        /// </summary>
        /// <returns></returns>
        public string URL { get; set; }

        /// <summary>
        /// Gets the content type of this result if there is one
        /// </summary>
        /// <returns></returns>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets the description of this result
        /// </summary>
        /// <returns></returns>
        public string Description { get; set; }
    }
}
