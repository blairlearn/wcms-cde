using System;
using System.Configuration;

namespace NCI.Web.CDE.InformationRequest.Configuration
{
    public class HostElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return (string)base["type"];
            }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)base["url"];
            }
        }
    }
}
