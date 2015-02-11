using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Globalization;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.HttpHeaders;
using NCI.Util;
using NCI.Logging;
using NCI.Web.Extensions;
using NCI.Web.CDE;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.Modules;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public class DynamicListFactorySnippet : BaseSearchSnippet
    {

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DynamicList info = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);

            // Need to add exception handling here
            SnippetControl localControl = (SnippetControl)Page.LoadControl(info.ResultsTemplate);

            localControl.SnippetInfo = this.SnippetInfo;
            this.Controls.Add(localControl);
        }

        override protected void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}