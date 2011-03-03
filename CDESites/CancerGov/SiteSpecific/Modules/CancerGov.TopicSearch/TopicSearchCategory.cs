using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Xml.Schema;
using NCI.Web.CDE.PageAssembly;
using System.Collections.ObjectModel;

namespace NCI.Web.CDE
{

    /// <summary>
    /// This class encapsulates all the information related to TopicSearch and 
    /// individual items in the topic search.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_TopicSearchCategory", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class TopicSearchCategory
    {
        /// <summary>
        /// The text that will be used for title of the topic search.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CategoryName { get; set; }

        /// <summary>
        /// A collection of all page options item.
        /// </summary>
        [XmlArray(ElementName = "TopicSearches", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("TopicSearch", typeof(TopicSearch), Form = XmlSchemaForm.Unqualified)]
        public Collection<TopicSearch> TopicSearches { get; set; }
    }
}
