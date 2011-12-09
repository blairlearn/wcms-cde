using System;
using System.Configuration;

namespace NCI.Web.CDE.InformationRequest.Configuration
{

    public class WebAnalyticsSection : ConfigurationSection
    {
        [ConfigurationProperty("hosts")]
        public HostElementCollection Hosts
        {
            get { return (HostElementCollection)base["hosts"]; }
        }
    }
}
