using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCI.Search.Configuration
{
    public class SiteWideSearchConfiguration : ConfigurationSection
    {

        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get
            {
                return (ProviderSettingsCollection)base["providers"];
            }
        }

        [ConfigurationProperty("defaultProvider")]
        public string DefaultProvider
        {
            get
            {
                return base["defaultProvider"] as string;
            }
        }

    }
}
