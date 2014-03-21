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
    public class SocialMetaTag
    {
        /// <summary>
        /// The type of the meta tag (name or property)
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Type { get; set; }

        /// <summary>
        /// The name or property of the mime tag
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Id { get; set; }

        /// <summary>
        /// The source of the tag's content.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Source { get; set; }

        /// <summary>
        /// Either the content to use for the meta tag directly, or the name of the PageAssembly field to read, based on the Source attribute.
        /// </summary>
        [XmlAttribute(Form = XmlSchemaForm.Unqualified)]
        public string Content { get; set; }

        /// <summary>
        /// Adds an appropriate FieldFilter to the given PageAssemblyInstruction object, based on the Tag's
        /// current settings.
        /// </summary>
        /// <param name="pai">The IPageAssemblyInstruction object to receive the new FieldFilter.</param>
        public void InitializeFieldFilter(IPageAssemblyInstruction pai)
        {
            // add a field filter for each tag
            pai.AddFieldFilter(Id, (name, data) =>
            {
                data.Value = ResolveContent(pai);
            });
        }

        /// <summary>
        /// Resolves the actual content for the meta tag, based on the values of the source and content fields.
        /// </summary>
        /// <returns>A string representation of the meta tag content.</returns>
        private string ResolveContent(IPageAssemblyInstruction pai)
        {
            string content = String.Empty;
            switch (Source)
            {
                case "field":
                    content = pai.GetField(Content);
                    break;
                case "url":
                    content = pai.GetUrl(Content).ToString();
                    break;
                default:
                    content = Content;
                    break;
            }

            if (String.IsNullOrEmpty(content))
            {
                Logger.LogError("CDE:SocialMetadata.cs:SocialMetaTag.ResolveContent()",
                    Type + " " + Content + " resolved to a null or empty value.",
                    NCIErrorLevel.Warning);
            }

            return content;
        }
    }

    public class SocialMetadata
    {
        public SocialMetadata()
        {
            // default to false in the case of no entry in XML
            IsCommentingAvailable = false;

            Tags = new SocialMetaTag[0];
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
        [XmlArrayItem("SocialMetaTag", typeof(SocialMetaTag), Form = XmlSchemaForm.Unqualified)]
        public SocialMetaTag[] Tags { get; set; }

        /// <summary>
        /// Creates field filters for all member fields.
        /// </summary>
        /// <param name="pai">An available PageAssemblyInstruction object to receive the field filters.</param>
        public void InitializeFieldFilters(IPageAssemblyInstruction pai)
        {

            try
            {
                // check provided PageAssemblyInstruction
                if (pai == null)
                {
                    Logger.LogError("CDE:SocialMetadata.cs:InitializeFieldFilters()",
                        "null PageAssemblyInstruction provided.",
                        NCIErrorLevel.Warning);

                    return;
                }

                // add commenting available field filter
                pai.AddFieldFilter("is_commenting_available", (name, data) =>
                {
                    data.Value = IsCommentingAvailable.ToString();
                });

                // add field filters for any tags
                if (Tags != null)
                {
                    foreach (SocialMetaTag tag in Tags)
                    {
                        tag.InitializeFieldFilter(pai);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("CDE:SocialMetadata.cs:InitializeFieldFilters()",
                       "Exception encountered while initializing field filters.",
                       NCIErrorLevel.Error, e);
            }
        }
    }
}