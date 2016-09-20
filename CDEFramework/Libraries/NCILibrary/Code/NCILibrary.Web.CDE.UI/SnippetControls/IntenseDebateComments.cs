using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using Common.Logging;
using NCI.Util;
using NCI.Web.CDE.Modules;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.Modules
{
    /// <summary>
    /// This Snippet Template is for inserting an IntenseDebate comment thread onto the page.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:IntenseDebateComments runat=server></{0}:IntenseDebateComments>")]
    public class IntenseDebateComments : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(IntenseDebateComments));

        protected IntenseDebateControl theControl = null;
        public void Page_Load(object sender, EventArgs e)
        {
            // FIX THIS COMMENT
            BasePageAssemblyInstruction basePage = PageAssemblyContext.Current.PageAssemblyInstruction as BasePageAssemblyInstruction;
            if (basePage == null)
                return;
            bool isCommentingAvailable = Strings.ToBoolean(PageAssemblyContext.Current.PageAssemblyInstruction.GetField("is_commenting_available"), false, false);
            log.DebugFormat("Page_Load(): SocialMetadata isCommentingAvailable value is {0}", isCommentingAvailable);
            // if commenting is not available, then done with processing
            if (!isCommentingAvailable)
                return;
            // initialize the control
            theControl = new IntenseDebateControl();
            // load the account from settings
            IntenseDebateCommentsSettings intenseDebateCommentsSettings = ModuleObjectFactory<IntenseDebateCommentsSettings>.GetModuleObject(SnippetInfo.Data);
            if (intenseDebateCommentsSettings != null)
            {
                string account = intenseDebateCommentsSettings.DevAccount;
                // do not configure the control if no account available
                if (String.IsNullOrEmpty(intenseDebateCommentsSettings.DevAccount) || String.IsNullOrEmpty(intenseDebateCommentsSettings.LiveAccount))
                {
                    theControl.Dispose();
                    theControl = null;
                    return;
                }
                // check if the site is in production
                bool isProd = PageAssemblyContext.Current.IsProd;
                // append a shortname prefix based on the production state
                theControl.Account = (isProd ? intenseDebateCommentsSettings.LiveAccount : intenseDebateCommentsSettings.DevAccount);
                
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
            theControl.CommentPolicyText = intenseDebateCommentsSettings.CommentPolicy;
        }
    }
}
