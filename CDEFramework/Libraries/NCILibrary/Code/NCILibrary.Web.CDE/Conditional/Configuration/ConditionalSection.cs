using System;
using System.Configuration;

namespace NCI.Web.CDE.Conditional.Configuration
{

    public class ConditionalSection : ConfigurationSection
    {
        [ConfigurationProperty("booleanConditions")]
        public BooleanConditionElementCollection BooleanConditions
        {
            get { return (BooleanConditionElementCollection)base["booleanConditions"]; }
        }
    }
}
