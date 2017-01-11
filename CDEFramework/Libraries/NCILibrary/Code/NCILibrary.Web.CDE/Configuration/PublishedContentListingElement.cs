using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    /// <summary>
    /// Configuration for the PublishedContent Listing Handler.  Defines what paths are
    /// permitted and what content types (extensions) are allowed.
    /// </summary>
    public class PublishedContentListingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the collection of paths that are allowed.
        /// </summary>
        [ConfigurationProperty("publishedContentListingPaths")]
        public PublishedContentListingPathElementCollection ListablePaths
        {
            get { return (PublishedContentListingPathElementCollection)base["publishedContentListingPaths"]; }
        }
    }
}
