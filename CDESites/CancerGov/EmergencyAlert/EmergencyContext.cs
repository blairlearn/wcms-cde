using System;
using System.Configuration;
using System.Web;
using CancerGov.Common.ErrorHandling;
using NCI.EmergencyAlert;
using NCI.Util;

namespace CancerGov.EmergencyAlert
{
    public class EmergencyContext
    {
        //The key to get the context from the HttpContext.Items bag.
        private const string _itemsKey = "EmergencyContext";
        private const string _siteNameAppKey = "SiteName";
        private const string _emergencyUrlAppKey = "EmergencyUrl";

        private NCI.EmergencyAlert.EmergencyAlert _alert = null;
        private string _emergencyUrl = string.Empty;

        /// <summary>
        /// Gets whether or not we are currently in an emergency.
        /// </summary>
        /// <value><c>true</c> if the system is in an emergency state; otherwise, <c>false</c>.</value>
        public bool InEmergency
        {
            get { return (_alert != null); }
        }

        /// <summary>
        /// Gets the title of the emergency.
        /// </summary>
        public string Title
        {
            get
            {
                if (_alert == null)
                    return string.Empty;
                else
                    return _alert.EmergencyTitle;
            }
        }

        /// <summary>
        /// Gets the information about the emergency.
        /// </summary>
        public string Information
        {
            get
            {
                if (_alert == null)
                    return string.Empty;
                else
                    return _alert.Information;
            }
        }

        /// <summary>
        /// Gets a value indicating whether all requests should redirect to the emergency page.
        /// </summary>
        /// <value><c>true</c> if all requests should redirect to the emergency page; otherwise, <c>false</c>.</value>
        public bool RedirectAllPages
        {
            get
            {
                if (_alert == null)
                    return false;
                else
                    return _alert.ForceRedirect;
            }
        }

        /// <summary>
        /// Gets the text of the emergency banner.
        /// </summary>
        /// <value>The banner text.</value>
        public string BannerText
        {
            get
            {
                if (_alert == null)
                    return string.Empty;
                else
                    return _alert.EmergencyBanner;
            }
        }

        /// <summary>
        /// Gets the emergency page URL.
        /// </summary>
        /// <value>The emergency page URL.</value>
        public string EmergencyUrl
        {
            get { return _emergencyUrl; }
        }

        /// <summary>
        /// Gets the EmergencyContext object for this HttpApplication.
        /// </summary>
        public static EmergencyContext Current
        {
            get
            {
                EmergencyContext rtnContext = null;

                HttpContext current = HttpContext.Current;
                if (current != null)
                    if (current.Items[_itemsKey] != null)
                        rtnContext = (EmergencyContext)current.Items[_itemsKey];

                return rtnContext;
            }
        }

        /// <summary>
        /// Initializes a new instance of a EmergencyContext object.
        /// <remarks>This constructor should be hidden so that it is only ever called once.
        /// </remarks>
        /// </summary>
        private EmergencyContext()
        {
            InitProperties();
        }

        /// <summary>
        /// Initializes the EmergencyContext object and stores it in HttpContext.Items bag.
        /// </summary>
        public static EmergencyContext CreateContext()
        {
            EmergencyContext eContext = null;

            //make sure one does not exist first
            HttpContext current = HttpContext.Current;

            if (current != null)
            {
                if (current.Items[_itemsKey] != null)
                {
                    throw new Exception(_itemsKey + " Already Exists");
                }
                else
                {
                    eContext = new EmergencyContext();
                    current.Items.Add(_itemsKey, eContext);
                }
            }
            else
            {
                throw new Exception("HttpContext.Current does not exist");
            }

            return eContext;
        }

        /// <summary>
        /// Sets up the properties of the EmergencyContext.
        /// </summary>
        private void InitProperties()
        {
            string siteName = Strings.Clean(ConfigurationManager.AppSettings[_siteNameAppKey]);

            _emergencyUrl = Strings.Clean(ConfigurationManager.AppSettings[_emergencyUrlAppKey]);

            if (siteName == null)
                CancerGovError.LogError(this.GetType().ToString(), 4, "Could not load emergency. Missing siteName appSetting: " + _siteNameAppKey);

            if (siteName == null)
                return;

            //Go to manager and get the emergency info
            try
            {
                _alert = EmergencyManager.GetEmergency(siteName);
            }
            catch (Exception ex)
            {
                CancerGovError.LogError(this.GetType().ToString(), 4, "Could not load emergency. " + ex.ToString());
            }

            if (_alert != null && _emergencyUrl == null)
            {
                CancerGovError.LogError(this.GetType().ToString(), 4, "Could not load emergency. There is an emergency and missing EmergencyUrl appSetting: " + _emergencyUrlAppKey);
                _alert = null;
            }
            else if (_emergencyUrl == null)
            {
                CancerGovError.LogError(this.GetType().ToString(), 4, "Could not load emergency. Missing EmergencyUrl appSetting: " + _emergencyUrlAppKey);
            }
        }
    }
}
