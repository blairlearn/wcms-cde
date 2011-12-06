using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace NCI.Web.CDE.CapabilitiesDetection
{
    public static class DisplayDeviceDetector
    {

        public static int ScreenHeight
        {
            get
            {
                CapabilitiesDetector detecto = new CapabilitiesDetector();
                return detecto.ScreenHeight;
            }
        }
        
        public static int ScreenWidth
        {
            get
            {
                CapabilitiesDetector detecto = new CapabilitiesDetector();
                return detecto.ScreenWidth;
            }
        }
        
        public static DisplayDevices DisplayDevice 
        { 
            get 
            {
                if (HttpContext.Current.Request.UserAgent == null)
                {
                    // If Request UserAgent is null - default to desktop
                    return DisplayDevices.Decktop;
                }
                else
                {
                   CapabilitiesDetector detecto = new CapabilitiesDetector();

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
        }
        
        public static string DisplayDeviceString
        {
            get
            {

                if (HttpContext.Current.Request.UserAgent == null)
                {
                    // If Request UserAgent is null - default to desktop
                    return "Decktop";
                }
                else
                {
                    CapabilitiesDetector detecto = new CapabilitiesDetector();

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
        }

        public static Dictionary<string, string> DisplayDeviceCapabilitiesList
        {
            get
            {
                if (HttpContext.Current.Request.UserAgent == null)
                {
                    return null;
                }
                else
                {
                    CapabilitiesDetector detecto = new CapabilitiesDetector();
                    return detecto.CapabilitiesList;
                }
            }
        }
    }
}
