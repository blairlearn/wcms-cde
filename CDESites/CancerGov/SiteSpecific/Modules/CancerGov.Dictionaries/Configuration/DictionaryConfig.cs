using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CancerGov.Dictionaries.Configuration
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DictionaryConfig", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DictionaryConfig
    {
        /// <summary>
        /// The type of dictionary.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DictionaryType { get; set; }

        /// <summary>
        /// A collection of zero or more CDRID to friendly name mapping files for dictionaries.
        /// </summary>
        [XmlArray(ElementName = "CDRFriendlyNameMappingFiles", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("CDRFriendlyNameMappingFile", typeof(CDRFriendlyNameMappingFile), Form = XmlSchemaForm.Unqualified)]
        public CDRFriendlyNameMappingFile[] Files { get; set; }
    }
}
