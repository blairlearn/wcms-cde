using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using System.Configuration;
using CancerGov.Web.UI.SnippetControls;
using NCI.Logging;

namespace NCI.Web.CDE.UI.Modules
{
    /// <summary>
    /// This Snippet Template is for inserting a Disqus comment thread onto the page.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DisqusComments runat=server></{0}:DisqusComments>")]
    public class DisqusComments : SnippetControl
    {
        protected DisqusWebControl theControl = null;

        public void Page_Load(object sender, EventArgs e)
        {
            // check if comments are enabled
            BasePageAssemblyInstruction basePage = ((BasePageAssemblyInstruction)PageAssemblyContext.Current.PageAssemblyInstruction);
            
            bool isCommentingAvailable = false;

            if (basePage is SinglePageAssemblyInstruction)
            {                
                // convert string to boolean for IsCommentingEnabled
                isCommentingAvailable = ((SinglePageAssemblyInstruction)basePage).SocialMetadata.IsCommentingAvailable;
            }
            else if (basePage is MultiPageAssemblyInstruction)
            {
                // convert string to boolean for IsCommentingEnabled
                isCommentingAvailable = ((MultiPageAssemblyInstruction)basePage).SocialMetadata.IsCommentingAvailable;
            }

            Logger.LogError("CDE:DisqusComments.cs:Page_Load", "SocialMetadata isCommentingAvailable value is " + isCommentingAvailable, NCIErrorLevel.Debug);

            // pseudocode

            // skip everything above if not showing comments

            // end pseudocode

            // initialize the control
            theControl = new DisqusWebControl();
            this.Controls.Add(theControl);

            // load the shortname from settings
            DisqusCommentsSettings disqusCommentsSettings = ModuleObjectFactory<DisqusCommentsSettings>.GetModuleObject(SnippetInfo.Data);
            if (disqusCommentsSettings != null)
            {
                theControl.Shortname = disqusCommentsSettings.Shortname;
            }

            // begin setting the control's properties

            // identifer
            string contentType = basePage.ContentItemInfo.ContentItemType;
            string contentId = basePage.ContentItemInfo.ContentItemID;
            theControl.Identifier = contentType + "-" + contentId;

            // split based on multipage or singlepage
            if (basePage is SinglePageAssemblyInstruction)
            {
                theControl.Title = ((SinglePageAssemblyInstruction)basePage).PageMetadata.ShortTitle;
                theControl.URL = HttpContext.Current.Request.Url.Scheme
                + "://"
                + HttpContext.Current.Request.Url.Authority 
                + ((SinglePageAssemblyInstruction)basePage).PrettyUrl;
            }
            else if (basePage is MultiPageAssemblyInstruction)
            {
                theControl.Title = ((MultiPageAssemblyInstruction)basePage).PageMetadata.ShortTitle;
                theControl.URL = HttpContext.Current.Request.Url.Scheme
                + "://"
                + HttpContext.Current.Request.Url.Authority 
                + ((MultiPageAssemblyInstruction)basePage).PrettyUrl;
            }
        }

    }
}