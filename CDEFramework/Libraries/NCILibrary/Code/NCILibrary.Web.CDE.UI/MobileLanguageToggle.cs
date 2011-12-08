using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using NCI.Web.CDE.UI.Configuration;

namespace NCI.Web.CDE.UI
{
    public class MobileLanguageToggle : WebControl
    {

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //If the page has no AltLanguage URL
            try
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("AltLanguage");
            }
            catch (PageAssemblyException)
            {
                this.Visible = false;
                return;
            }
            if (String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("AltLanguage").ToString()))
            {
                this.Visible = false;
                return;
            }
             base.Render(writer);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            MobileLanguageToggleConfigElement elem = MobileLanguageToggleConfig.GetByCultureLanguage(CultureInfo.CurrentUICulture);
            if( !String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("AltLanguage").ToString()) ){
                String formattedUrl;
                try
                {
                    formattedUrl = String.Format(elem.Template, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("AltLanguage").ToString());
                }
                catch (ArgumentNullException)
                {
                    throw;
                }
                catch (FormatException)
                {
                    throw;
                }
                writer.Write(HttpUtility.UrlDecode(formattedUrl));
            }
            
            
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
        }

        /// <summary>
        /// Saves any state that was modified after the <see cref="M:System.Web.UI.WebControls.Style.TrackViewState"/> method was invoked.
        /// </summary>
        /// <returns>
        /// An object that contains the current view state of the control; otherwise, if there is no view state associated with the control, null.
        /// </returns>
        protected override object SaveViewState()
        {
            object obj1 = base.SaveViewState();

            return new object[] { obj1 };
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();
        }

        /// <summary>
        /// Restores view-state information from a previous request that was saved with the <see cref="M:System.Web.UI.WebControls.WebControl.SaveViewState"/> method.
        /// </summary>
        /// <param name="savedState">An object that represents the control state to restore.</param>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;

                base.LoadViewState(objArray[0]);
            }
            else
            {
                base.LoadViewState(null);
            }
        }
    }
}

