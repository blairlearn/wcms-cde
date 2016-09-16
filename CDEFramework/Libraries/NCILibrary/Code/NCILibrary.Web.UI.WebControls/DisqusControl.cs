using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.UI.WebControls
{
    /// <summary>
    /// This is a web control for the Disqus javascript to be inserted on webpages if needed
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DisqusWebControl runat=server></{0}:DisqusWebControl>")]
    public class DisqusControl : WebControl
    {
        public DisqusControl()
        {
            Shortname = string.Empty;
            Identifier = string.Empty;
            Title = string.Empty;
            URL = string.Empty;
            Category = int.MinValue;
            DisableMobile = false;
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
        [DefaultValue(int.MinValue)]
        [Localizable(true)]
        public int Category
        {
            get { return (int)(ViewState["Category"] ?? int.MinValue); }
            set { ViewState["Category"] = value; }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool DisableMobile
        {
            get { return (bool)(ViewState["DisableMobile"] ?? false); }
            set { ViewState["DisableMobile"] = value; }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            output.AddAttribute(HtmlTextWriterAttribute.Id, "disqus_thread");
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.Write("<!-- Disqus Content Here -->");
            output.RenderEndTag();//end div

            string catId = String.Empty;
            if (this.Category > 0)
            {
                catId = @"var disqus_category_id = '" + this.Category + @"';
    ";
            }

            string disableMobile = String.Empty;
            if (this.DisableMobile)
            {
                disableMobile = @"var disqus_disable_mobile = true;
    ";
            }

            string disqusScript =
                @"
    var disqus_shortname = '" + this.Shortname +@"';
    var disqus_identifier = '" + this.Identifier + @"';
    var disqus_url = '" + this.URL + @"';
    var disqus_title = '" + this.Title + @"';
    " + catId + 
   disableMobile + @"

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
            // ADDED CLASS TO AVOID EXIT LINK NOTIFICATION
            output.AddAttribute(HtmlTextWriterAttribute.Class, "dsq-brlink no-exit-notification");
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
