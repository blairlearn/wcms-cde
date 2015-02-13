using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Util;

namespace CancerGov.EmergencyAlert
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ServerControl1 runat=server></{0}:ServerControl1>")]
    public class EmergencyAlertBanner : WebControl
    {
        private EmergencyContext eContext = null;

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Visible = false;

            this.eContext = EmergencyContext.Current;

            //We should not show this on the print version.
            bool isPrint = Strings.ToBoolean(Page.Request.Params["print"]);

            if (this.eContext != null)
            {
                if (!isPrint && this.eContext.InEmergency && !string.IsNullOrEmpty(this.eContext.BannerText) && !IsEmergencyPage())
                {
                    //Should not show this if it is the emergency page also...
                    //Turn on only in an emergency && only if there is some text for the banner.
                    this.Visible = true;
                }
            }

            this.CssClass = "EmergencyAlertBanner";
        }

        private bool IsEmergencyPage()
        {
            return false;
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            if (this.eContext != null)
            {
                output.AddAttribute(HtmlTextWriterAttribute.Class, "notification");
                output.RenderBeginTag(HtmlTextWriterTag.Span);
                output.AddAttribute(HtmlTextWriterAttribute.Href, this.eContext.EmergencyUrl);
                output.RenderBeginTag(HtmlTextWriterTag.A);
                output.Write("Emergency Information");
                output.Write(this.eContext.BannerText);
                output.RenderEndTag();
                output.RenderEndTag();
            }
        }
    }
}
