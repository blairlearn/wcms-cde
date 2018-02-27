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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DictionariesInfo", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DictionariesInfo
    {
        /// <summary>
        /// A collection of DictionaryInfo items for the dictionaries.
        /// </summary>
        [XmlArray(ElementName = "Dictionaries", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Dictionary", typeof(DictionaryInfo), Form = XmlSchemaForm.Unqualified)]
        public List<DictionaryInfo> DictionaryInfos { get; set; }
    }
}
