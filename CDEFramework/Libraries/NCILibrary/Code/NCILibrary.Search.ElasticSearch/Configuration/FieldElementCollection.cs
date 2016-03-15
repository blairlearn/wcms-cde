using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// This class is a collection of all the FieldElements - e.g. Title, URL, Description
    /// </summary>
    [ConfigurationCollection(
        typeof(FieldElement),
        AddItemName = "field",
        CollectionType = ConfigurationElementCollectionType.BasicMap
        )]
    public class FieldElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new ConfigurationElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new FieldElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element 
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FieldElement)element).FieldName;
        }

    }
}
