using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.InformationRequest
{
    /// <summary>
    /// Information Request Messages 
    /// </summary>    
    public sealed class InformationRequestMessages
    {
        private InformationRequestMessages() {}

        public static readonly string CanonicalUrlNotFound ="CanonicalUrl Not Found:";
        public static readonly string MobileUrlNotFound = "MobileUrl Not Found:";
        public static readonly string FileNotFound = "File Not Found(404):";
        public static readonly string MobileUrlFound = "MobileUrl Found:";
        public static readonly string CanonicalUrlFound = "CanonicalUrl Found:";
        public static readonly string MobileHostNotFound = "Mobile Host Not Found:";
        public static readonly string DesktopHostNotFound = "Desktop Host Not Found:";
        
    }

    /// <summary>
    /// Information Request Constants 
    /// </summary> 
    public sealed class InformationRequestConstants
    {
        private InformationRequestConstants() { }

        public static readonly string MobileHost = "mobile";
        public static readonly string DesktopHost = "desktop";
        public static readonly string InformationRequestToken = "?Information__Request=";
        public static readonly string MobileUrlRequest = "?Information__Request=mobileurl";

    }
}
