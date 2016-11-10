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


        protected BlogSeriesArchiveControl theControl = null;
        public void Page_Load(object sender, EventArgs e)
        {
            // FIX THIS COMMENT
            SinglePageAssemblyInstruction basePage = PageAssemblyContext.Current.PageAssemblyInstruction as SinglePageAssemblyInstruction;
            if (basePage == null)
                return;            
            
            BlogSeriesArchiveSettings blogSeriesArchiveSettings = ModuleObjectFactory<BlogSeriesArchiveSettings>.GetModuleObject(SnippetInfo.Data);

            // initialize the control
            var language = PageAssemblyContext.Current.PageAssemblyInstruction.Language;
            string years = blogSeriesArchiveSettings.Years;
            string groupBy = blogSeriesArchiveSettings.GroupBy;
            string blogSeriesId = "Blog Series-" + blogSeriesArchiveSettings.BlogSeriesId;

            theControl = new BlogSeriesArchiveControl(language, groupBy);

            // load the shortname from settings           

            if (blogSeriesArchiveSettings != null)
            {

                // do not configure the control if no shortname available
                if (String.IsNullOrEmpty(years))
                {
                    return;
                }
            }

            var results = BlogArchiveDataManager.Execute(blogSeriesId, Int32.Parse(years));

            theControl.results = results;

            // add the control
            this.Controls.Add(theControl);

        }
    }
}