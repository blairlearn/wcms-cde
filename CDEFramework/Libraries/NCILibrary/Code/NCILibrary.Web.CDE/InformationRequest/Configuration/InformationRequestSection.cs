using System;
using System.Configuration;

namespace NCI.Web.CDE.InformationRequest.Configuration
{

    public class InformationRequestSection : ConfigurationSection
    {
        [ConfigurationProperty("hosts")]
        public HostElementCollection Hosts
        {
            get { return (HostElementCollection)base["hosts"]; }
        }

        [ConfigurationProperty("mappedPages")]
        public MappedPageCollection MappedPages
        {
            get { return (MappedPageCollection)base["mappedPages"]; }
        }
    }
}
