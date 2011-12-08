using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.UI.Configuration
{
    public class MobileLanguageToggleConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("mobileLangToggleConfigs")]
        public MobileLanguageToggleConfigElementCollection Configs
        {
            get { return (MobileLanguageToggleConfigElementCollection)base["mobileLangToggleConfigs"]; }
        }
    }
}
