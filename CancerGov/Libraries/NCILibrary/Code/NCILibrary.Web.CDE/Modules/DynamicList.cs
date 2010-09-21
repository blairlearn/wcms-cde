using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace NCI.Web.CDE.Modules
{
    public class SearchParameters
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Keyword { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string StartDate { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string EndDate { get; set; }
    }

    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("Module_DynamicList", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class DynamicList
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchTitle { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int RecordsPerPage { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public int MaxResults { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchFilter { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ExcludeSearchFilter { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResultsSortOrder { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string Language { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string SearchType { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public SearchParameters SearchParameters { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string ResultsTemplate { get; set; }
    }
}
