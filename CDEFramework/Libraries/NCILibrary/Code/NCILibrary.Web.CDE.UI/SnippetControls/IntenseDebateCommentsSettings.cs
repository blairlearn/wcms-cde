using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.UI.Modules
{
    /// <summary>
    /// The class which defines all configurable properties.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_IntenseDebateComments", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class IntenseDebateCommentsSettings
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string LiveAccount{get;set;}
  
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string DevAccount{get;set; }
    }
}
