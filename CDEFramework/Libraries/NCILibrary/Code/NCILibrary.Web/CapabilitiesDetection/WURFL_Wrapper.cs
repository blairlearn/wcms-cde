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

using NCILibrary.Web.CapabilitiesDetection;

using WURFL;
using Wurfl.AspNet.DeviceBrowser.Helpers;


namespace NCI.Web
{
    public class WURFL_Wrapper
    {
        private WURFL.IDevice _device = null;
        private IWURFLManager _manager = null;
        private Dictionary<string, string> _capabilityDictionary = null;
        private bool _loaded = false;
        private HttpRequest _httpRequest = null;

        /// <summary>
        /// One-time setup for the wrapper.
        /// </summary>
        static WURFL_Wrapper()
        {
            // Load configuration data.  If a config section exists, set the location of the WURFL
            // data and patch files.
            //
            // WurflLoader is a static class with the sole exception of the
            // SetDataPath method.  Because it was created externally, we don't
            // want to refactor it, but because we only reference it via WURFL_Wrapper,
            // we can safely initialize it in a static constructor and override the
            // default data and patch file locations.
            CapabilitiesDetectionSection config = (CapabilitiesDetectionSection)ConfigurationManager.GetSection("nci/web/capabilities");
            if (config != null)
            {
                WurflLoader loader = new WurflLoader();
                loader.SetDataPath(config.DeviceData.dataFile, config.DeviceData.patchFile);
            }
        }

        public HttpRequest HttpRequest
        {
            get { return _httpRequest; }
        }

        public bool DetectCapabilities(HttpRequest httpRequest)
        {
            _manager = WurflLoader.GetManager();
            _device = _manager.GetDeviceForRequest(httpRequest);

            if (_device  != null)
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
                    return _capabilityDictionary[property].Trim().ToLower();
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
