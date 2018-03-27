using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CancerGov.Dictionaries.Configuration
{
    public class DictionaryInfo
    {
        /// <summary>
        /// The Name of this dictionary.
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// The Language of this dictionary.
        /// </summary>
        [XmlAttribute]
        public string Language { get; set; }

        /// <summary>
        /// The URL for definitions for this dictionary.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DefinitionUrl { get; set; }

        /// <summary>
        /// The location of the friendly name mapping file for this dictionary.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string FriendlyNameMapping { get; set; }
    }
}
