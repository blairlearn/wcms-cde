using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.ComponentModel;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class represents the Module data for DocumentTitle Block.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DocTitleBlock", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DockTitleBlock
    {
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string Title { get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string TitleDisplay{ get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string ImageUrl { get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string TableColor{ get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string ContentField{ get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string LinkTitle{ get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string LinkUrl{ get; set; }
         [XmlElement(Form = XmlSchemaForm.Unqualified)] public string SubTitle { get; set; }
    }
}
