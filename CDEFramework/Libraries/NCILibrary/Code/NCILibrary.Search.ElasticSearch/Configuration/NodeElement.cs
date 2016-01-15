using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// This class represents a single node
    /// </summary>
    public class NodeElement : ConfigurationElement
    {
        /// <summary>
        /// Name of the node
        /// </summary>
        [ConfigurationProperty("name")]
        public string NodeName
        {
            get { return (string)base["name"]; }
        }

        /// <summary>
        /// IP address of the node
        /// </summary>
        [ConfigurationProperty("ip", IsKey = true, IsRequired = true)]
        public string NodeIP
        {
            get { return (string)base["ip"]; }
        }

        /// <summary>
        /// port of the node
        /// </summary>
        [ConfigurationProperty("port")]
        public string Port
        {
            get { return (string)base["port"]; }
        }
    }

}
