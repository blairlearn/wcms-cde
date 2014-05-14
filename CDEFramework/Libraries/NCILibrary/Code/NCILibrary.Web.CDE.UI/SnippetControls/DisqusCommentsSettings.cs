using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.UI.Modules
{
    /// <summary>
    /// The class which defines all configurable properties.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DisqusComments", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DisqusCommentsSettings
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
         public string Shortname{get;set;}
    }
}
