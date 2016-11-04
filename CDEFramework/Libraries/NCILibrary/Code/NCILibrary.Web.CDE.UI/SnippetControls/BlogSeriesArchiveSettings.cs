using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace NCI.Web.CDE.UI.Modules
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_BlogSeriesArchive", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class BlogSeriesArchiveSettings
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Years { get; set; }
    }
}
