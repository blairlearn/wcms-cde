using System;
using System.Configuration;

namespace NCI.Web.ProductionHost.Configuration
{
    [ConfigurationCollection(typeof(StringProductionHostElement),
     AddItemName = "stringCondition",
     CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class StringProductionHostElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StringProductionHostElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StringProductionHostElement)element).Name;
        }
    }
}
