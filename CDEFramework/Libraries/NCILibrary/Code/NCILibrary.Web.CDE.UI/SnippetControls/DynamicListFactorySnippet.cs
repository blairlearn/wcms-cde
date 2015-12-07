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
    public class DynamicListFactorySnippet : SnippetControl
    {
        /// <summary>
        /// Gets the snippet info for a Dynamic List content item and selects the correct control
        /// based on the ResultsTemplate element in the page instructions. If the selected control
        /// cannot be found, a template with no description or image is selected.
        /// </summary>
        /// <param name="e"></param>
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            DynamicList info = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);
            String defaultTemplate = ("~/DynamicListTemplates/DynamicListNoDescNoImgDate.ascx");
            SnippetControl localControl;

            try
            {
                localControl = (SnippetControl)Page.LoadControl(info.ResultsTemplate);
            }
            catch (HttpException ex)
            {
                localControl = (SnippetControl)Page.LoadControl(defaultTemplate);
            }
            catch (ArgumentNullException ex)
            {
                localControl = (SnippetControl)Page.LoadControl(defaultTemplate);
            }
            localControl.SnippetInfo = this.SnippetInfo;
            this.Controls.Add(localControl);
        }

        override protected void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}