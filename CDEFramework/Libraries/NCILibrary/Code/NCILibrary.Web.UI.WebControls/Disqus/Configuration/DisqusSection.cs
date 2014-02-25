using System;
using System.Configuration;

namespace NCI.Web.UI.WebControls.Disqus.Configuration
{

    public class DisqusSection : ConfigurationSection
    {
        [ConfigurationProperty("booleanConditions")]
        public BooleanDisqusElementCollection BooleanConditions
        {
            get { return (BooleanDisqusElementCollection)base["booleanConditions"]; }
        }
    }
}
