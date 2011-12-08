using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Globalization;

namespace NCI.Web.CDE.UI.Configuration
{
    [ConfigurationCollection(typeof(MobileLanguageToggleConfigElement),
        AddItemName = "mobileLangToggleConfig",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class MobileLanguageToggleConfigElementCollection : ConfigurationElementCollection
    {

        [ConfigurationProperty("defaultLang", IsRequired = true)]
        public string DefaultLanguage
        {
            get
            {
                return (string)base["defaultLang"];
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MobileLanguageToggleConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MobileLanguageToggleConfigElement)element).Language;
        }
    }
}
