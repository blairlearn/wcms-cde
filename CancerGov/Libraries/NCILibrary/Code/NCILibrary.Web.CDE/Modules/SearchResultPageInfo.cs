using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.Modules
{
    /// <summary>
    /// This class defines the properties of search result. Like the prettyUrl of the 
    /// search results page. This information should be made avaliable in the instruction 
    /// that defines the search page.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_SearchResultPageInfo", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class SearchResultPageInfo
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchResultsPrettyUrl { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchPagePrettyUrl { get; set; }

    }
}
    