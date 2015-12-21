using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    public class NodeElement : ConfigurationElement
    {
        [ConfigurationProperty("name")]
        public string NodeName
        {
            get { return (string)base["name"]; }
        }

        [ConfigurationProperty("ip", IsKey = true, IsRequired = true)]
        public string NodeIP
        {
            get { return (string)base["ip"]; }
        }

        [ConfigurationProperty("port")]
        public string Port
        {
            get { return (string)base["port"]; }
        }
    }

}
