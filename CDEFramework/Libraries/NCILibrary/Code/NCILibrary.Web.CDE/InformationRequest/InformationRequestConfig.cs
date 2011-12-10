using System;
using System.Web;
using System.Web.SessionState;
using System.Globalization;
using System.Configuration;
using System.IO;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Web.CDE.InformationRequest;
using NCI.Logging;
using System.Reflection;
using NCI.Web.CDE.InformationRequest.Configuration;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE.InformationRequest
{
    public static class InformationRequestConfig
    {
        private static string _mobileHost = "";
        private static string _desktopHost = "";

        public static string MobileHost
        {
            get { return _mobileHost; }
        }

        public static string DesktopHost
        {
            get { return _desktopHost; }
        }
        
        
        static InformationRequestConfig()
        {
            InformationRequestSection section = (InformationRequestSection)ConfigurationManager.GetSection("nci/web/informationRequest");

            foreach (HostElement elem in section.Hosts)
            {
                if (elem.Type == InformationRequestConstants.MobileHost)
                {
                    _mobileHost = elem.Url;
                }

                if (elem.Type == InformationRequestConstants.DecktopHost)
                {
                    _desktopHost = elem.Url;
                }
            }
        }



    }
}
