using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Logging;
using NCI.Text;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This is a web control for the IntenseDebate javascript to be inserted on webpages if needed
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:IntenseDebateWebControl runat=server></{0}:IntenseDebateWebControl>")]
    public class IntenseDebateControl : WebControl
    {
        public IntenseDebateControl()
        {
            Account = string.Empty;
            Identifier = string.Empty;
            URL = string.Empty;
            Title = string.Empty;
            CommentPolicyText = string.Empty;
        }

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Account
        {
            get { return (String)ViewState["Account"] ?? string.Empty; }
            set { ViewState["Account"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Identifier
        {
            get { return (String)ViewState["Identifier"] ?? string.Empty; }
            set { ViewState["Identifier"] = value; }
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string URL
        {
            get { return (String)ViewState["URL"] ?? string.Empty; }
            set { ViewState["URL"] = value; }
        }


        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get { return (String)ViewState["Title"] ?? string.Empty; }
            set { ViewState["Title"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string CommentPolicyText
        {
            get { return (String)ViewState["CommentPolicyText"] ?? string.Empty; }
            set { ViewState["CommentPolicyText"] = value; }
        }


        protected override void RenderContents(HtmlTextWriter output)
        {
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.Write(this.CommentPolicyText);
            output.RenderEndTag();
 
            output.AddAttribute(HtmlTextWriterAttribute.Id, "intensedebate-thread");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.Write("<!-- IntenseDebate Content Here -->");
            output.RenderEndTag();

            string pattern = 
                @"var idcomments_acct =  '{0}';
                var idcomments_post_id = '{1}';
                var idcomments_post_url = '{2}';
                ";

            string intenseDebateScript = string.Format(pattern, this.Account, this.Identifier, this.URL);
 
            output.RenderBeginTag(HtmlTextWriterTag.Script);
            output.Write(intenseDebateScript);
            output.RenderEndTag();
 
            output.AddAttribute(HtmlTextWriterAttribute.Id, "IDCommentsPostTitle");
            output.AddAttribute(HtmlTextWriterAttribute.Style, "display: none");
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.RenderEndTag();
 
            output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            output.AddAttribute(HtmlTextWriterAttribute.Src, "http://www.intensedebate.com/js/genericCommentWrapperV2.js");
            output.RenderBeginTag(HtmlTextWriterTag.Script);
            output.RenderEndTag();//ends script tag
        }
    }
}
