using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class MobileRedirectorElement : ConfigurationElement
    {
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)base["value"]; }
        }
    }
}
