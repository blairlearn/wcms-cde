using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    [ConfigurationCollection(
        typeof(NodeElement),
        AddItemName = "node",
        CollectionType = ConfigurationElementCollectionType.BasicMap
        )]
    public class NodeElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new NodeElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((NodeElement)element).NodeIP;
        }

    }
}
