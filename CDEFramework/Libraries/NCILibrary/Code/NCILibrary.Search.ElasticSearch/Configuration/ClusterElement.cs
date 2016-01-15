using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    /// <summary>
    /// This class is used for the details of the cluster
    /// </summary>
    public class ClusterElement : ConfigurationElement
    {
        /// <summary>
        /// Name of the cluster - in this case SearchCluster
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        /// <summary>
        /// MaximumRetries - How many times should the client try to connect to other nodes before returning a ConnectionFault error. 
        /// </summary>
        [ConfigurationProperty("maximumretries", IsRequired = true, DefaultValue = "5")]
        public int MaximumRetries
        {
            get
            {
                return (int)base["maximumretries"];
            }
        }

        /// <summary>
        /// ConnectionTimeoutDelay - Milliseconds that a dead connection will wait before attempting to revive itself. 
        /// </summary>
        [ConfigurationProperty("connectiontimeoutdelay", IsRequired = true, DefaultValue = "5000")]
        public int ConnectionTimeoutDelay
        {
            get
            {
                return (int)base["connectiontimeoutdelay"];
            }
        }

        /// <summary>
        /// Collection of nodes in the cluster
        /// </summary>
        [ConfigurationProperty("nodes")]
        public NodeElementCollection Nodes
        {
            get { return (NodeElementCollection)base["nodes"]; }
        }
    }
}
