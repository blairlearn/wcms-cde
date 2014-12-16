using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.UI.SnippetControls
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("SublayoutInfo", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class SublayoutInfo
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Title { get; set; }
    }
}
