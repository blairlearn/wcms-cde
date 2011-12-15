using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CDE.CapabilitiesDetection;
using NCI.Web.CDE;

namespace MobileCancerGov.Web.UserControls
{
    public partial class TermDictionary : System.Web.UI.UserControl
    {
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if((DisplayDeviceDetector.DisplayDevice == DisplayDevices.AdvancedMobile) ||
               (DisplayDeviceDetector.DisplayDevice == DisplayDevices.Desktop) ||
               (DisplayDeviceDetector.DisplayDevice == DisplayDevices.Tablet)) 
            {
                advanced.Visible = true;
                basic.Visible = false;
            }
            else
            {
                advanced.Visible = false;
                basic.Visible = true;
            }




        }

        protected void btnToggle_Click(object sender, EventArgs e)
        {

        }

    }
}