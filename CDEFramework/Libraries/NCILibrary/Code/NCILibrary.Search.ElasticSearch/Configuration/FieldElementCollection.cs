using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    [ConfigurationCollection(
        typeof(FieldElement),
        AddItemName = "field",
        CollectionType = ConfigurationElementCollectionType.BasicMap
        )]
    public class FieldElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FieldElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FieldElement)element).FieldName;
        }

    }
}
