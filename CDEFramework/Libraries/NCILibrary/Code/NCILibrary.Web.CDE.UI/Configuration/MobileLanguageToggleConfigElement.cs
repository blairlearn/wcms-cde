using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NCI.Web.CDE.UI.Configuration
{
    public class MobileLanguageToggleConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("lang", IsKey = true, IsRequired = true)]
        public string Language
        {
            get
            {
                return (string)base["lang"];
            }
        }

        [ConfigurationProperty("template", IsRequired = true)]
        public String Template
        {
            get
            {
                return (String)base["template"];
            }
        }

        public CultureInfo CultureInfo
        {
            get
            {
                CultureInfo culture = CultureInfo.CurrentUICulture;

                try
                {
                    culture = new CultureInfo(Language);
                }
                catch { }

                return culture;
            }
        }
    }
}
