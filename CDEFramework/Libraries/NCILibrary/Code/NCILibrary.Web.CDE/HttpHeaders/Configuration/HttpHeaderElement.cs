using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.HttpHeaders.Configuration
{
    /// <summary>
    /// Configuration element to specify a community in a community list.
    /// This corresponds to the innermost nodes in the list
    /// 
    /// <httpHeaders>
    ///    <headers>
    ///      <add name="foo" value="bar" />
    ///      <add name="X-UA-Compatible" value="IE=8" />
    ///    </headers>
    /// </httpHeaders>
    /// 
    public class HttpHeaderElement : ConfigurationElement
    {
        /// <summary>
        /// The Http header's name.
        /// </summary>
        /// <value>The name.</value>
        [ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
            get { return (String)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// The Http header's value.
        /// </summary>
        /// <value>The value.</value>
        [ConfigurationProperty("value", IsRequired = true)]
        public String Value
        {
            get { return (String)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
