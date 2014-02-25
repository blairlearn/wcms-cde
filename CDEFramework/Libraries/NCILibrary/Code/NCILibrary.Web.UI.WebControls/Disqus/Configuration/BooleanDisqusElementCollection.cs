using System;
using System.Configuration;

namespace NCI.Web.UI.WebControls.Disqus.Configuration
{
    [ConfigurationCollection(typeof(BooleanDisqusElement),
     AddItemName = "booleanCondition",
     CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class BooleanDisqusElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BooleanDisqusElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BooleanDisqusElement)element).Name;
        }
    }
}
