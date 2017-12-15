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
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string EnglishCDRFriendlyNameMapFilepath { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SpanishCDRFriendlyNameMapFilepath { get; set; }
    }
}
