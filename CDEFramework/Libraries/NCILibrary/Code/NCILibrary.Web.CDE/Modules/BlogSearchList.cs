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
        /// This class represents search parameter fields.
        /// </summary>
        public class BlogSearchParameters
        {
            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string SiteName { get; set; }

        }

        /// <summary>
        /// This class represents parameter fields, results template used in search.
        /// </summary>
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
        [System.Xml.Serialization.XmlRootAttribute("Module_DynamicList", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
        public class BlogSearchList
        {
            /// <summary>
            /// Search Filter
            /// </summary>
            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string Filter { get; set; }
 
            /// <summary>
            ///Content ID of Item
            /// </summary>
            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string ContentID { get; set; }
            
            /// <summary>
            /// Language for which the search will be performed.
            /// </summary>
            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string Language { get; set; }

            /// <summary>
            /// Search parameters like SiteName
            /// </summary>
            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public SearchParameters SearchParameters { get; set; }

            /// <summary>
            /// The template which will be used to render the results on the CDE
            /// </summary>
            [XmlElement(Form = XmlSchemaForm.Unqualified)]
            public string ResultsTemplate { get; set; }
        }
    }

