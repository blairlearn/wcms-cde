using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
    public class WURFL_Wrapper
    {
       private WurflLoader _WurflLoader = new WurflLoader();
        private WURFL.IDevice _device = null;
        private Dictionary<string, string> _capabilityDictionary = null;
        private bool _loaded = false;
        private HttpRequest _httpRequest = null;

        public HttpRequest HttpRequest
        {
            get { return _httpRequest; }
        }

        public bool DetectCapabilities(HttpRequest httpRequest)
        {
            if ((_device = WurflLoader.GetManager().GetDeviceForRequest(httpRequest)) != null)
            {
                _capabilityDictionary = new Dictionary<string, string>(_device.GetCapabilities());

                _httpRequest = httpRequest;
                _loaded = true;
                return true;
            }
            else
                return false;
        }
               
        public string GetCapability(string property)
        {
            
            if (_loaded)
            {
                if (_capabilityDictionary.ContainsKey(property))
                    return _capabilityDictionary[property];
                else
                    return null;
            }
            else
                return null;
        }

        public Dictionary<string, string> GetCapabilitiesDictionary()
        {
            if (_loaded)
                return _capabilityDictionary;
            else
                return null;
        }
        public WURFL.IDevice GetDevice()
        {
            if (_loaded)
                return _device;
            else
                return null;
        }
    }
}
