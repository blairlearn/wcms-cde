using System;
using System.Configuration;

namespace NCI.Web.ProductionHost.Configuration
{
    public class StringProductionHostElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; } 
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return (string)base["value"]; }
        }
    }
}