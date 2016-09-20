using System;
using NCI.Web.CDE.Configuration;

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
        public SocialMetaTagTypes Type
        {
            get
            {
                if (SocialMetaTagData != null)
                {
                    return SocialMetaTagData.Type;
                }

                return SocialMetaTagTypes.Unknown;
            }
        }

        /// <summary>
        /// The name or property of the mime tag
        /// </summary>
        public string Key
        {
            get
            {
                if (SocialMetaTagData != null)
                {
                    return SocialMetaTagData.Id;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// The underlying socialmetadagdata object used to provide values and resulve content.
        /// </summary>
        private  SocialMetaTagData SocialMetaTagData { get; set; }

        public SocialMetaTag(SocialMetaTagData data)
        {
            SocialMetaTagData = data;
        }

        /// <summary>
        /// Generate the content string to place in the meta tag.  Can return null if no content exists.
        /// </summary>
        public string ResolveContent(IPageAssemblyInstruction IPageAssemblyInstruction)
        {
            string content = IPageAssemblyInstruction.GetField(Key);
            // for url or file types, append the latest field into the hostname.
            if (SocialMetaTagData.PrependHost &&
                (SocialMetaTagData.Source == SocialMetaTagSources.url || SocialMetaTagData.Source == SocialMetaTagSources.literalUrl))
            {
                content = ContentDeliveryEngineConfig.CanonicalHostName.CanonicalUrlHostName.CanonicalHostName
                    + content;
            }

            // if the content is null at this point...
            if (String.IsNullOrEmpty(content))
            {
                if (SocialMetaTagData.AllowEmptyContent)
                {
                    // ensure is a string if empty content is allowed
                    content = "";
                }
                else
                {
                    // else, provide null content
                    content = null;
                }
            }

            return content;
        }
    }
}