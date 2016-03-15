using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// This class is a collection of all the ElasticSearchCollectionElements
    /// </summary>
    [ConfigurationCollection(typeof(ESSiteWideSearchCollectionElement),
        AddItemName = "siteWideSearchCollection",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ESSiteWideSearchCollectionElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new ConfigurationElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ESSiteWideSearchCollectionElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element 
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ESSiteWideSearchCollectionElement)element).Name;
        }
    }
}
