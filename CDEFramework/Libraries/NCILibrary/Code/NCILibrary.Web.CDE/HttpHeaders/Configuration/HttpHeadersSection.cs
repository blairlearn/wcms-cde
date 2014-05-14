using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using NCI.Web.CDE.HttpHeaders.Configuration;

namespace NCI.Web.CDE.HttpHeaders
{
    /// <summary>
    /// 
    /// </summary>
    public class HttpHeadersSection : ConfigurationSection
    {
        /// <summary>
        /// Provides the list of custom HTTP header names and values from
        /// web.config in nci/web/httpHeaders/headers.
        /// </summary>
        [ConfigurationProperty("headers", IsRequired = false, IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(HttpHeaderListElement), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public HttpHeaderListElement HttpHeaders
        {
            get { return (HttpHeaderListElement)base["headers"]; }
        }

        /// <summary>
        /// Private property to encapsulate the logic of loading an HttpHeadersSection object
        /// and making sure the reference is non-null.
        /// </summary>
        internal static HttpHeadersSection Instance
        {
            get
            {
                HttpHeadersSection section = (HttpHeadersSection)ConfigurationManager.GetSection("nci/web/httpHeaders");

                if (section == null)
                    section = new HttpHeadersSection();

                return section;
            }
        }
    }
}
