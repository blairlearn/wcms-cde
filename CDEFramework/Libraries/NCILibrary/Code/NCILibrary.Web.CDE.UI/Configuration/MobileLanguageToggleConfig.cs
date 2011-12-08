using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NCI.Web.CDE.UI.Configuration
{
    public static class MobileLanguageToggleConfig
    {
        private static readonly string mobileLangToggleConfigPath = "nci/web/mobileLanguageToggle";

        public static MobileLanguageToggleConfigElement GetByCultureLanguage(CultureInfo culture)
        {
            MobileLanguageToggleConfigElement rtnElem = null;
            MobileLanguageToggleConfigElement defaultElem = null;

            MobileLanguageToggleConfigSection section = (MobileLanguageToggleConfigSection)ConfigurationManager.GetSection(mobileLangToggleConfigPath);

            foreach (MobileLanguageToggleConfigElement elem in section.Configs)
            {
                if (elem.CultureInfo.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName)
                {
                    rtnElem = elem;
                    break;
                }

                if (elem.CultureInfo.TwoLetterISOLanguageName == culture.TwoLetterISOLanguageName)
                {
                    defaultElem = elem;
                }

            }

            if (rtnElem == null)
                rtnElem = defaultElem;

            return rtnElem;
        }

    }
}
