using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using Wurfl.AspNet.DeviceBrowser.Helpers;

namespace NCILibrary.Web.CapabilitiesDetection
{
    public class CapabilitiesDetectionSection : ConfigurationSection
    {
        [ConfigurationProperty("deviceData", IsRequired = false)]
        public WurflDataElement DeviceData
        {
            get { return (WurflDataElement)base["deviceData"]; }
        }
    }
}
