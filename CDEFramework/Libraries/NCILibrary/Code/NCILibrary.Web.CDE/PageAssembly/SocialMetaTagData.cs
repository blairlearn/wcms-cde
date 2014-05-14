using System;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Configuration;
using NCI.Logging;
namespace NCI.Web.CDE
{
    /// <summary>
    /// Defines a single meta tag for use with social media standards.
    /// </summary>
    public class SocialMetaTagData
    {
        /// <summary>
        /// The type of the meta tag (name or property)
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public SocialMetaTagTypes Type { get; set; }

        /// <summary>
        /// The name or property of the mime tag
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        /// <summary>
        /// The source of the tag's content.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public SocialMetaTagSources Source { get; set; }

        /// <summary>
        /// Either the content to use for the meta tag directly, or the name of the PageAssembly field to read, based on the Source attribute.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Content { get; set; }

        /// <summary>
        /// Flag to add the hostname to the beginning of the field, only used for url and literalUrl types.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public bool PrependHost { get; set; }

        /// <summary>
        /// Flag to allow empty content to generate a resulting meta tag.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public bool AllowEmptyContent { get; set; }

        public SocialMetaTagData()
        {
            this.Type = SocialMetaTagTypes.Unknown;
            this.Id = String.Empty;
            this.Source = SocialMetaTagSources.Unknown;
            this.Content = String.Empty;
            this.PrependHost = true;
            this.AllowEmptyContent = false;
        }
    }
}