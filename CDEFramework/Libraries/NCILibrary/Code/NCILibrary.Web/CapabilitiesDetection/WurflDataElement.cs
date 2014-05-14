using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

using Wurfl.AspNet.DeviceBrowser.Helpers;

namespace NCILibrary.Web.CapabilitiesDetection
{
    /// <summary>
    /// Represents a configuration element with a single attribute named value.
    /// </summary>
    public class WurflDataElement : ConfigurationElement
    {
        [ConfigurationProperty("dataFile", IsRequired = false, DefaultValue = WurflLoader.DefaultWurflDataFilePath)]
        public String dataFile
        {
            get { return (String)this["dataFile"]; }
            set { this["dataFile"] = value; }
        }

        [ConfigurationProperty("patchFile", IsRequired = false, DefaultValue = WurflLoader.DefaultWurflPatchFilePath)]
        public String patchFile
        {
            get { return (String)this["patchFile"]; }
            set { this["patchFile"] = value; }
        }

    }
}
