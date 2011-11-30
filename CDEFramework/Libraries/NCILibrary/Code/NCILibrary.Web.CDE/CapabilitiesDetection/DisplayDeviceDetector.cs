using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.CapabilitiesDetection
{
    public static class DisplayDeviceDetector
    {
        public static DisplayDevices DisplayDevice 
        { 
            get 
            { 
                CapablitiesDetector detecto = new CapablitiesDetector();

                if (detecto.IsTablet)
                    return DisplayDevices.Tablet;
                else if (detecto.IsAdvancedMobile)
                    return DisplayDevices.AdvancedMobile;
                else if (detecto.IsBasicMobile)
                    return DisplayDevices.BasicMobile;
                else
                    return DisplayDevices.Decktop;


            } 
        }
        public static string DisplayDeviceString
        {
            get
            {
                CapablitiesDetector detecto = new CapablitiesDetector();

                if (detecto.IsTablet)
                    return "Tablet";
                else if (detecto.IsAdvancedMobile)
                    return "AdvancedMobile";
                else if (detecto.IsBasicMobile)
                    return "BasicMobile";
                else
                    return "Decktop";


            }
        }
        public static Dictionary<string, string> DisplayDeviceCapabilitiesList
        {
            get
            {
                CapablitiesDetector detecto = new CapablitiesDetector();
                return detecto.CapabilitiesList;
            }
        }
    }
}
