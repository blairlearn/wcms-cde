using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using NCI.Logging;
using NCI.Web.CDE.Modules;
using NCI.Web.UI.WebControls;
using NCI.Util;

namespace NCI.Web.CDE.UI.Modules
{
    /// <summary>
    /// This Snippet Template is for inserting a Disqus comment thread onto the page.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DisqusComments runat=server></{0}:DisqusComments>")]
    public class DisqusComments : SnippetControl
    {
        protected DisqusControl theControl = null;

        public void Page_Load(object sender, EventArgs e)
        {
            // FIX THIS COMMENT
            BasePageAssemblyInstruction basePage = PageAssemblyContext.Current.PageAssemblyInstruction as BasePageAssemblyInstruction;
            if (basePage == null)
                return;

            bool isCommentingAvailable = Strings.ToBoolean(PageAssemblyContext.Current.PageAssemblyInstruction.GetField("is_commenting_available"), false, false);

            Logger.LogError("CDE:DisqusComments.cs:Page_Load", "SocialMetadata isCommentingAvailable value is " + isCommentingAvailable, NCIErrorLevel.Debug);

            // if commenting is not available, then done with processing
            if (!isCommentingAvailable)
                return;

            // initialize the control
            theControl = new DisqusControl();

            // load the shortname from settings
            DisqusCommentsSettings disqusCommentsSettings = ModuleObjectFactory<DisqusCommentsSettings>.GetModuleObject(SnippetInfo.Data);
            if (disqusCommentsSettings != null)
            {
                string shortname = disqusCommentsSettings.Shortname;

                // do not configure the control if no shortname available
                if (String.IsNullOrEmpty(shortname))
                {
                    theControl.Dispose();
                    theControl = null;
                    return;
                }

                // check if the site is in production
                bool isProd = PageAssemblyContext.Current.IsProd;
                // append a shortname prefix based on the production state
                theControl.Shortname = disqusCommentsSettings.Shortname + (isProd ? "-prod" : "-dev");
            }

            // add the control
            this.Controls.Add(theControl);

            // begin setting the control's properties

            // identifer
            string contentType = basePage.ContentItemInfo.ContentItemType;
            string contentId = basePage.ContentItemInfo.ContentItemID;
            theControl.Identifier = contentType + "-" + contentId;

            // split based on multipage or singlepage

            theControl.Title = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("short_title");

            theControl.URL = HttpContext.Current.Request.Url.Scheme
                + "://"
                + HttpContext.Current.Request.Url.Authority
                + PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("PrettyUrl");

        }

    }
}