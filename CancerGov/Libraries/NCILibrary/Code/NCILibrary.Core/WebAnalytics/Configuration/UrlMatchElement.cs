using System;
using System.Configuration;


namespace NCI.Web.CDE.WebAnalytics.Configuration
{
    public class UrlMatchElement : ConfigurationElement
    {
        [ConfigurationProperty("pathMatch", IsRequired = true)]
        public string PathMatch
        {
            get
            {
                return (string)base["pathMatch"];
            }
        }

        [ConfigurationProperty("channelName")]
        public string ChannelName
        {
            get
            {
                return (string)base["channelName"];
            }
        }

        [ConfigurationProperty("urlMatches")]
        public UrlMatchElementCollection UrlMatches
        {
            get { return (UrlMatchElementCollection)base["urlMatches"]; }
        }
    }    
}
