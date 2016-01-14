using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    public class ClusterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        [ConfigurationProperty("maximumretries", IsRequired = true, DefaultValue = "5")]
        public int MaximumRetries
        {
            get
            {
                return (int)base["maximumretries"];
            }
        }
        
        //default value - 5000 milliseconds = 5 seconds
        [ConfigurationProperty("connectiontimeoutdelay", IsRequired = true, DefaultValue = "5000")]
        public int ConnectionTimeoutDelay
        {
            get
            {
                return (int)base["connectiontimeoutdelay"];
            }
        }

        [ConfigurationProperty("nodes")]
        public NodeElementCollection Nodes
        {
            get { return (NodeElementCollection)base["nodes"]; }
        }
    }
}
