using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using NCI.Logging;
using NCI.Text;
using NCI.Web.UI.WebControls.Disqus;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This is a web control for the Disqus javascript to be inserted on webpages if needed
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DisqusWebControl runat=server></{0}:DisqusWebControl>")]
    public class DisqusWebControl : WebControl
    {
        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Shortname
        {
            get { return (String)ViewState["Shortname"] ?? string.Empty; }
            set { ViewState["Shortname"] = value; }
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
        public string Title
        {
            get { return (String)ViewState["Title"] ?? string.Empty; }

            set { ViewState["Title"] = value; }
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
        [DefaultValue("General")]
        [Localizable(true)]
        public string Category
        {
            get { return (String)ViewState["Category"] ?? string.Empty; }
            set { ViewState["Category"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("false")]
        [Localizable(true)]
        public string Disable_mobile
        {
            get { return (string)ViewState["Disable_mobile"] ?? null; }
            set { ViewState["Disable_mobile"] = value; }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Id, "disqus_thread");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderEndTag();//end div

            bool isProd = DisqusConfig.IsProd;
            String snSuffix = isProd ? "-prod" : "-dev";

            string disqusScript =
@"    var disqus_shortname = '" + this.Shortname + snSuffix +@"';
    var disqus_identifier = '" + this.Identifier + @"';
    var disqus_url = '" + this.URL + @"';
    var disqus_title = '" + this.Title + @"';
    var disqus_category_id = '" + this.Category + @"';
    var disqus_disable_mobile = '" + this.Disable_mobile + @"';

    /* * * DON'T EDIT BELOW THIS LINE * * */
    (function() {
        var dsq = document.createElement('script'); dsq.type = 'text/javascript'; dsq.async = true;
        dsq.src = '//' + disqus_shortname + '.disqus.com/embed.js';
        (document.getElementsByTagName('head')[0] || document.getElementsByTagName('body')[0]).appendChild(dsq);
    })();";
            output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            output.RenderBeginTag(HtmlTextWriterTag.Script);
            output.Write(disqusScript);
            output.RenderEndTag();//ends script tag


            output.RenderBeginTag(HtmlTextWriterTag.Noscript);
            output.Write("Please enable JavaScript to view the ");
            output.AddAttribute(HtmlTextWriterAttribute.Href, "http://disqus.com/?ref_noscript");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write("comments powered by Disqus.");
            output.RenderEndTag();//end A tag
            output.RenderEndTag();//end noscript

            output.AddAttribute(HtmlTextWriterAttribute.Href, "http://disqus.com");
            output.AddAttribute(HtmlTextWriterAttribute.Class, "dsq-brlink");
            output.RenderBeginTag(HtmlTextWriterTag.A);
            output.Write("blog comments powered by ");
            output.AddAttribute(HtmlTextWriterAttribute.Class, "logo-disqus");
            output.RenderBeginTag(HtmlTextWriterTag.Span);
            output.Write("Disqus");
            output.RenderEndTag();//end span
            output.RenderEndTag();//end A
        }
    }
}
