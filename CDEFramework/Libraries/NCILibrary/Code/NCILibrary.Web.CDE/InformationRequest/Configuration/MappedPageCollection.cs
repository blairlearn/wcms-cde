using System;
using System.Configuration;

namespace NCI.Web.CDE.InformationRequest.Configuration
{
    [ConfigurationCollection(typeof(ApplicationElement),
     AddItemName = "mappedPage",
     CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class MappedPageCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ApplicationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ApplicationElement)element).Name;
        }
    }
}
