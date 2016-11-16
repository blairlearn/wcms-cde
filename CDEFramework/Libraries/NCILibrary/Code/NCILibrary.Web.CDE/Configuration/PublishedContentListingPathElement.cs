using System;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    /// <summary>
    /// Represents an allowed path to be exposed by the PublishedContent Listing Handler
    /// </summary>
    public class PublishedContentListingPathElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the name/key of this path.  No Spaces or special chars please!
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        /// <summary>
        /// Gets the name of this allowed path to be displayed when no path (e.g. name query param)
        /// is passed into the PublishedContent Listing Handler.
        /// </summary>
        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get
            {
                return (string)base["displayName"];
            }
        }

        /// <summary>
        /// Gets the root path for this path.  This should be something like ~/PublishedContent/PageInstructions
        /// </summary>
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get
            {
                return (string)base["path"];
            }
        }

        /// <summary>
        /// Gets the allowed file types for this path.  Should look like ".xml,.csv,.blah"
        /// </summary>
        [ConfigurationProperty("allowedFileTypes", IsRequired = true)]
        public string AllowedFileTypes
        {
            get
            {
                return (string)base["allowedFileTypes"];
            }
        }

        /// <summary>
        /// Gets the allowed file types as an array.
        /// </summary>
        /// <returns></returns>
        public string[] GetAllowedTypesAsArray()
        {
            if (!string.IsNullOrWhiteSpace(this.AllowedFileTypes))
            {
                return this.AllowedFileTypes.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                return new string[] { };
            }
        }

    }
}
