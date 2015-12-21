using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    [ConfigurationCollection(typeof(ESSiteWideSearchCollectionElement),
        AddItemName = "siteWideSearchCollection",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ESSiteWideSearchCollectionElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ESSiteWideSearchCollectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ESSiteWideSearchCollectionElement)element).Name;
        }
    }
}
