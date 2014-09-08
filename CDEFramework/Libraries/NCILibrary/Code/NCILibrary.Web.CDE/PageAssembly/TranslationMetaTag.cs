using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines a single meta tag for use with translation data.
    /// </summary>
    public class TranslationMetaTag
    {

        /// <summary>
        /// The translation item's country and culture code
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Locale { get; set; }

        /// <summary>
        /// The translation item's content ID
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        /// <summary>
        /// The translation item's URL
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Url { get; set; }


        public TranslationMetaTag()
        {
            this.Locale = String.Empty;
            this.Id = String.Empty;
            this.Url = String.Empty;
        }
    }
}