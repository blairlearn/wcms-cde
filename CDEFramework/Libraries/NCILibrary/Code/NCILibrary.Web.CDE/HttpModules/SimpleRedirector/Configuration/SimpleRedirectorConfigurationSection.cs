using System;
using System.Configuration;

namespace NCILibrary.Web.SimpleRedirector.Configuration
{
    public class SimpleRedirectorConfigurationSection : ConfigurationSection
    {
        public static SimpleRedirectorConfigurationSection Get()
        {
            SimpleRedirectorConfigurationSection config = (SimpleRedirectorConfigurationSection)ConfigurationManager.GetSection("nci/web/simpleRedirection");
            return config;
        }

        [ConfigurationProperty("dataSource")]
        public DataSourceConfigurationElement DataSource
        {
            get { return (DataSourceConfigurationElement)base["dataSource"]; }
        }
    }
}
