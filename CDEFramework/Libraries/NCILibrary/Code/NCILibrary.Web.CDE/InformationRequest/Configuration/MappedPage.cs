using System;
using System.Configuration;

namespace NCI.Web.CDE.InformationRequest.Configuration
{
    public class ApplicationElement : ConfigurationElement
    {
       [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
        }

        [ConfigurationProperty("desktopUrl", IsRequired = true)]
        public string DesktopUrl
        {
            get
            {
                return (string)base["desktopUrl"];
            }
        }

        [ConfigurationProperty("mobileUrl", IsRequired = true)]
        public string MobileUrl
        {
            get
            {
                return (string)base["mobileUrl"];
            }
        }

    }
}
