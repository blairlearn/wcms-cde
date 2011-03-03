using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.UI.WebControls.JSLibraries;   // In order to reference Prototype.

namespace NCI.Web.UI.WebControls.FormControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:SmartCheckBoxList runat=server></{0}:SmartCheckBoxList>")]
    public class SmartCheckBoxList : CheckBoxList
    {
        protected override void OnPreRender(EventArgs e)
        {
            /// Set up JavaScript resources. Order is important.  Because the control's script uses prototype, we need
            /// to register that one first.
            PrototypeManager.Load(this.Page);
            JSManager.AddResource(this.Page, typeof(HelpTextInput), "CancerGovUIControls.Resources.SmartCheckBoxList.js");

            base.OnPreRender(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write(string.Format("<script type=\"text/javascript\">SmartCheckBoxList.Create(\"{0}\");</script>", this.ClientID));
        }
    }
}
