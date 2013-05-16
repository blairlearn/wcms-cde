using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.HttpHeaders.Configuration
{
    /// <summary>
    /// Contains a list of http header definitions.
    /// Corresponds to the headers element in
    /// 
    /// <httpHeaders>
    ///    <headers>
    ///      <add name="foo" value="bar" />
    ///      <add name="X-UA-Compatible" value="IE=8" />
    ///    </headers>
    /// </httpHeaders>
    /// 
    /// </summary>
    public class HttpHeaderListElement : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates the new object when the underlying ConfigurationElementCollection
        /// object adds a new element.
        /// </summary>
        /// <returns>
        /// A new HttpHeaderElement, cast as a ConfigurationElement.
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new HttpHeaderElement();
        }

        /// <summary>
        /// Returns the value to be used as the unique key for looking up a particular element
        /// in the list of HttpHeaders.
        /// </summary>
        /// <param name="element">The HttpHeaderElement object to return the key for.</param>
        /// <returns>
        /// An Object that acts as the key for the specified CommunityListElement.
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HttpHeaderElement)element).Name;
        }

        /// <summary>
        /// Looks up a HttpHeaderElement object from its unique key. 
        /// </summary>
        /// <value></value>
        public new HttpHeaderElement this[String key]
        {
            get { return (HttpHeaderElement)BaseGet(key); }
        }
    }
}
