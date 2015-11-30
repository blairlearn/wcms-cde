using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.BestBets.Configuration
{
    public class BestBetsSection : ConfigurationSection
    {
        [ConfigurationProperty("pathConfigurationClass")]
        public string PathConfigurationClass
        {
            get { return (string)base["pathConfigurationClass"]; }
        }

    }
}
