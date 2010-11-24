using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CancerGov.EmergencyAlert
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:EmergencyAlertMessage runat=server></{0}:EmergencyAlertMessage>")]
    public class EmergencyAlertMessage : WebControl
    {
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            EmergencyContext ctx = EmergencyContext.Current;

            if (ctx == null || !ctx.InEmergency)
            {
                //output.RenderBeginTag(HtmlTextWriterTag.H1);
                //output.Write("There is currently no emergency.");
                //output.RenderEndTag();
            }
            else
            {
                output.RenderBeginTag(HtmlTextWriterTag.H1);
                output.Write(ctx.Title);
                output.RenderEndTag();


                output.AddAttribute(HtmlTextWriterAttribute.Class, "message");
                output.RenderBeginTag(HtmlTextWriterTag.Div);
                output.Write(ctx.Information);
                output.RenderEndTag();
            }
        }
    }
}
