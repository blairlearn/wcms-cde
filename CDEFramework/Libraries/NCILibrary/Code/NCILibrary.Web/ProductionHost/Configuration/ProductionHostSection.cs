using System;
using System.Configuration;

namespace NCI.Web.ProductionHost.Configuration
{

    public class ProductionHostSection : ConfigurationSection
    {
        [ConfigurationProperty("stringConditions")]
        public StringProductionHostElementCollection StringConditions
        {
            get { return (StringProductionHostElementCollection)base["stringConditions"]; }
        }
    }
}
