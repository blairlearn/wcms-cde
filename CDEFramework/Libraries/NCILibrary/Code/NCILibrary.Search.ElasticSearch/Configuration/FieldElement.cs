using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    public class FieldElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string FieldName
        {
            get { return (string)base["name"]; }
        }

    }
}
