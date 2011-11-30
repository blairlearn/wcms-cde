using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using WURFL;
using Wurfl.AspNet.DeviceBrowser.Helpers;


namespace NCI.Web.CDE.CapabilitiesDetection
{
    public class CapablitiesDetector
    {
        private bool _isDeskTop = false;
        private bool _isTablet = false;
        private bool _isAdvancedMobile = false;
        private bool _isBasicMobile = false;
        private int _height = -1;
        private int _width = -1;
        private Dictionary<string, string> _capabilitiesList = null;

        //Properties 
        public bool IsDeskTop { get { return _isDeskTop; } }
        public bool IsTablet { get { return _isTablet; } }
        public bool IsAdvancedMobile { get { return _isAdvancedMobile; } }
        public bool IsBasicMobile { get { return _isBasicMobile; } }
        public int Height { get { return _height; } }
        public int Width { get { return _width; } }
        public Dictionary<string, string> CapabilitiesList { get { return _capabilitiesList; } }

        public CapablitiesDetector()
        {
            WURFL_Wrapper wurfl = new WURFL_Wrapper();
            wurfl.DetectCapabilities(HttpContext.Current.Request);

            _capabilitiesList = wurfl.GetCapabilitiesDictionary();

            _height = Convert.ToInt32(wurfl.GetCapability("physical_screen_height"));
            _width = Convert.ToInt32(wurfl.GetCapability("physical_screen_width"));

            if (wurfl.GetCapability("mobile_browser") == "")  // This is a not a mobile device 
                _isDeskTop = true;
            else // This is a mobile device 
                if (wurfl.GetCapability("is_tablet") == "true")
                    _isTablet = true;
                else if ((wurfl.GetCapability("ajax_support_javascript") == "true")&&
                        Convert.ToInt32(wurfl.GetCapability("xhtml_support_level")) > 3)
                    _isAdvancedMobile = true;
                else
                    _isBasicMobile = true;
        }
    }
}
