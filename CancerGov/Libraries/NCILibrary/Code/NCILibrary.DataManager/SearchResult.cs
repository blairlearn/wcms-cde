using System;
using System.Collections.Generic;

namespace NCI.DataManager
{
    /// <summary>
    /// This class represents a single reuslt obtained after performing the search.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// The record number for use in nvelocity template.
        /// </summary>
        public int RecNumber { get; set; }


        /// <summary>
        /// Long descritpion of the content item
        /// </summary>
        public string LongTitle { get; set; }

        /// <summary>
        /// Short Description of the content item.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Long descritpion of the content item
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// The link to the complete content.
        /// </summary>
        public string HRef { get; set; }

        /// <summary>
        /// Updated date of the content
        /// </summary>
        public string UpdatedDate { get; set; }

        /// <summary>
        /// Posted date of the content.
        /// </summary>
        public string PostedDate { get; set; }
        public SearchResult()
        {
            ShortDescription = string.Empty;
            LongDescription = string.Empty;
            UpdatedDate = string.Empty;
            PostedDate = string.Empty;
            HRef = string.Empty;
        }
    }
}
