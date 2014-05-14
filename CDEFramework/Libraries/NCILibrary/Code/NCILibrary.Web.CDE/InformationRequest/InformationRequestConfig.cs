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
using System.Collections.Generic;
using NCI.Web.CDE.InformationRequest.Configuration;
using NCI.Web.CDE.WebAnalytics;

namespace NCI.Web.CDE.InformationRequest
{
    public static class InformationRequestConfig
    {
        private static string _mobileHost = "";
        private static string _desktopHost = "";
        private static Dictionary<string, string> _applicationUrls = new Dictionary<string, string>();

        public static string MobileHost
        {
            get { return _mobileHost; }
        }

        public static string DesktopHost
        {
            get { return _desktopHost; }
        }

        public static string GetMobileUrl(string desktopUrl)
        {
            string searchValue = desktopUrl.Trim().ToLower();

            if(_applicationUrls.ContainsKey(searchValue))
                return _applicationUrls[searchValue];
            else
                return "";
        }

        public static string GetDesktop(string mobileUrl)
        {
            string searchValue = mobileUrl.Trim().ToLower();

            if(_applicationUrls.ContainsKey(searchValue))
                return _applicationUrls[searchValue];
            else
                return "";
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

                if (elem.Type == InformationRequestConstants.DesktopHost)
                {
                    _desktopHost = elem.Url;
                }
            }

            section = (InformationRequestSection)ConfigurationManager.GetSection("nci/web/informationRequest");

            foreach (ApplicationElement elem in section.MappedPages)
                _applicationUrls.Add(elem.DesktopUrl.Trim().ToLower(),elem.MobileUrl.Trim().ToLower());
        }
    }
}
