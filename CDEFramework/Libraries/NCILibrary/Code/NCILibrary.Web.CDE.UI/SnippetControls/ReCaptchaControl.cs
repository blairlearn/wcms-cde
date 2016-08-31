using System.ComponentModel;
using System.Web.UI;

using NCI.Web.CDE.Configuration;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This control renders the needed HTML for a reCAPTCHA widget.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:ReCaptchaControl runat=server></{0}:ReCaptchaControl>")]
    public class ReCaptchaControl : SnippetControl
    {
        private static string scriptTag = 
            "<script src='https://www.google.com/recaptcha/api.js' async defer></script>";

        private static string recaptchaTag =
            "<div class='g-recaptcha' data-sitekey='" + ReCaptchaConfig.PublicKey + "'></div>";

        public override void RenderControl(HtmlTextWriter writer)
        {
            base.RenderControl(writer);
            
            LiteralControl scriptLit = new LiteralControl(scriptTag);
            scriptLit.RenderControl(writer);

            LiteralControl recaptchaLit = new LiteralControl(recaptchaTag);
            recaptchaLit.RenderControl(writer);
        }
    }
}
