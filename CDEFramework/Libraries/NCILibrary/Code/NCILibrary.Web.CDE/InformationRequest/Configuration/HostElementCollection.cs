using System;
using System.Configuration;

namespace NCI.Web.CDE.InformationRequest.Configuration
{
    [ConfigurationCollection(typeof(HostElement),
     AddItemName = "host",
     CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class HostElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HostElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HostElement)element).Type;
        }
    }
}
