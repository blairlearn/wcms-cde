using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
namespace NCI.Web.CDE
{
    public class Translations
    {
        /// <summary>
        /// A collection of zero or more meta tags containing metadata for translated versions of the content
        /// </summary>
        [XmlArray(ElementName = "TranslationData", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("TranslationMetaTag", typeof(TranslationMetaTag), Form = XmlSchemaForm.Unqualified)]
        public TranslationMetaTag[] Tags { get; set; }
    }
}