using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    [ConfigurationCollection(typeof(ClusterElement),
        AddItemName = "cluster",
        CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ClusterElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ClusterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClusterElement)element).Name;
        }

    }
}
