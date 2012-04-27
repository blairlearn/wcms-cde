using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace CancerGovCommon.Modules
{
    /// <summary>
    /// This class will contain all the cancerbulletin configuration data that can be used across all cancerbulletin
    /// applications.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_CancerBulletinConfigData", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class CancerBulletinConfigData
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CommentsUrl { get; set; }
    }
}
