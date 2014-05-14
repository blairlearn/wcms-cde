using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NCI.Web.CDE.Configuration
{
    public class MobileRedirectorInformationElement : ConfigurationElement
    {
        [ConfigurationProperty("cookieName")]
        public MobileRedirectorElement CookieName
        {
            get { return (MobileRedirectorElement)base["cookieName"]; }
        }

        [ConfigurationProperty("timeoutMinutes")]
        public MobileRedirectorElement TimeoutMinutes
        {
            get { return (MobileRedirectorElement)base["timeoutMinutes"]; }
        }

        [ConfigurationProperty("refreshOnPageView")]
        public MobileRedirectorElement RefreshOnPageView
        {
            get { return (MobileRedirectorElement)base["refreshOnPageView"]; }
        }

        [ConfigurationProperty("cookieDomain")]
        public MobileRedirectorElement CookieDomain 
        {
            get { return (MobileRedirectorElement)base["cookieDomain"]; }
        }


    }
}
