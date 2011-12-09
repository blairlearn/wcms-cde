using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.InformationRequest.Configuration
{

    public class InformationRequestSection : ConfigurationSection
    {
        [ConfigurationProperty("enabled", IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)base["enabled"]; }
        }

        [ConfigurationProperty("reportingSuites")]
        public ReportingSuitesElementCollection ReportingSuites
        {
            get { return (ReportingSuitesElementCollection)base["reportingSuites"]; }
        }

        [ConfigurationProperty("enableNonJavaScriptTagging", IsRequired = false, DefaultValue = false)]
        public bool EnableNonJavaScriptTagging
        {
            get { return (bool)base["enableNonJavaScriptTagging"]; }
        }

        [ConfigurationProperty("urlPathChannelMappings")]
        public UrlPathChannelElementCollection UrlPathChannelMappings
        {
            get { return (UrlPathChannelElementCollection)base["urlPathChannelMappings"]; }
        }
    }
}
