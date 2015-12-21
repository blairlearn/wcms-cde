using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace CancerGov.Search.BestBets
{
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.example.org/CDESchema")]
    [System.Xml.Serialization.XmlRootAttribute("BestBetsCategory", Namespace = "http://www.example.org/CDESchema", IsNullable = false)]
    public class BestBetUIResult
    {
        private string _catName;

        /// <summary>
        /// Gets the Category Name of this BestBetResult
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CategoryName { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string CategoryDisplay { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool Display { get; set; }

    }
}
