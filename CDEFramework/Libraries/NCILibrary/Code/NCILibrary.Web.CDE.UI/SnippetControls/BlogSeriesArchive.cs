using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCI.Util;
using NCI.DataManager;
using NCI.Web.CDE.Modules;
using NCI.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using Common.Logging;
using System.Web;

namespace NCI.Web.CDE.UI.Modules
{
    /// <summary>
    /// This Snippet Template is for inserting a Disqus comment thread onto the page.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BlogSeriesArchive runat=server></{0}:BlogSeriesArchive>")]
    public class BlogSeriesArchive : SnippetControl
    {
        //String blogSeriesID = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("BlogSeriesId");

        protected BlogSeriesArchiveControl theControl = null;
        public void Page_Load(object sender, EventArgs e)
        {
            // FIX THIS COMMENT
            SinglePageAssemblyInstruction basePage = PageAssemblyContext.Current.PageAssemblyInstruction as SinglePageAssemblyInstruction;
            if (basePage == null)
                return;
            
            // initialize the control
            theControl = new BlogSeriesArchiveControl();

            // load the shortname from settings
            BlogSeriesArchiveSettings blogSeriesArchiveSettings = ModuleObjectFactory<BlogSeriesArchiveSettings>.GetModuleObject(SnippetInfo.Data);
            string years = "";
            if (blogSeriesArchiveSettings != null)
            {
                years = blogSeriesArchiveSettings.Years;

                // do not configure the control if no shortname available
                if (String.IsNullOrEmpty(years))
                {
                    return;
                }
            }
            
            var results = BlogArchiveDataManager.Execute("Blog Series-828591", Int32.Parse(years));

            theControl.results = results;

            // add the control
            this.Controls.Add(theControl);

        }
    }
}