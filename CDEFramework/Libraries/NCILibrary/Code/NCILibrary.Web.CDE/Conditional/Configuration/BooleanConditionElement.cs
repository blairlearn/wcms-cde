using System;
using System.Configuration;

namespace NCI.Web.CDE.Conditional.Configuration
{
    public class BooleanConditionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base["name"]; } 
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public bool Value
        {
            get { return (bool)base["value"]; }
        }
    }
}