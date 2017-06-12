using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Configuration
{
    /// <summary>
    /// This class is a collection of all the term mapping files
    /// </summary>
    [ConfigurationCollection(
        typeof(TermMappingFileElement),
        AddItemName = "add",
        CollectionType = ConfigurationElementCollectionType.BasicMap
        )]
    public class TermMappingFileElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new ConfigurationElement
        /// </summary>
        protected override ConfigurationElement CreateNewElement()
        {
            return new TermMappingFileElement();
        }

        /// <summary>
        /// Gets the element key for a specified configuration element 
        /// </summary>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TermMappingFileElement)element).Name;
        }

        /// <summary>
        /// Looks up a HttpHeaderElement object from its unique key. 
        /// </summary>
        /// <value></value>
        public new TermMappingFileElement this[String key]
        {
            get { return (TermMappingFileElement)BaseGet(key); }
        }
    }
}
