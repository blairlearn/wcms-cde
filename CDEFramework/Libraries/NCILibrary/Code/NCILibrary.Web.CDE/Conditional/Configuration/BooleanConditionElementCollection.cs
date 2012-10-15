using System;
using System.Configuration;

namespace NCI.Web.CDE.Conditional.Configuration
{
    [ConfigurationCollection(typeof(BooleanConditionElement),
     AddItemName = "booleanCondition",
     CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class BooleanConditionElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BooleanConditionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BooleanConditionElement)element).Name;
        }
    }
}
