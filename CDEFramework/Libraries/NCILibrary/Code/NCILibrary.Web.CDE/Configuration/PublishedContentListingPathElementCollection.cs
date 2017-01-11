using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    /// <summary>
    /// Represents a collection of paths that will be permitted by the Published Content 
    /// Listing Handler.
    /// </summary>
    [ConfigurationCollection(typeof(PublishedContentListingPathElement),
        AddItemName = "publishedContentListingPath",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class PublishedContentListingPathElementCollection : ConfigurationElementCollection
    {
        
        public new PublishedContentListingPathElement this[string pathName]
        {
            get
            {
                return (PublishedContentListingPathElement)base.BaseGet(pathName);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PublishedContentListingPathElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PublishedContentListingPathElement)element).Name;
        }
    }
}
