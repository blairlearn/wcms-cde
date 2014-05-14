using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
namespace NCI.Web.CDE
{
    public class SocialMetadata
    {
        public SocialMetadata()
        {
            // default to false in the case of no entry in XML
            IsCommentingAvailable = false;

            Tags = new SocialMetaTagData[0];
        }

        /// <summary>
        /// Gets the enabled state of comments on the page.
        /// </summary>
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public bool IsCommentingAvailable { get; set; }

        /// <summary>
        /// A collection of zero or more meta tags for implementing various social media standards.
        /// </summary>
        [XmlArray(ElementName = "SocialMetaTags", Form = XmlSchemaForm.Unqualified)]
        [XmlArrayItem("SocialMetaTag", typeof(SocialMetaTagData), Form = XmlSchemaForm.Unqualified)]
        public SocialMetaTagData[] Tags { get; set; }
    }
}