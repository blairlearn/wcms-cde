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
    public class LanguageToggleControl : WebControl
    {

        private LanguageToggleCollection _langToggleCollection;

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets and sets the collection of LanguageToggle items
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false), DefaultValue((string)null), Description("DataControls_Columns"), Category("Default")]
        public virtual LanguageToggleCollection LanguageToggles
        {
            get
            {
                if (this._langToggleCollection == null)
                {
                    this._langToggleCollection = new LanguageToggleCollection();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)this._langToggleCollection).TrackViewState();
                    }
                }
                return this._langToggleCollection;
            }
            set
            {
                _langToggleCollection = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //If the page has no AltLanguage URL
            try
            {
                PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrls");
            }
            catch (PageAssemblyException)
            {
                this.Visible = false;
                return;
            }
            if (String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrls").ToString()))
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

            LanguageToggle langToggle = _langToggleCollection[CultureInfo.CurrentUICulture];
            string lang = CultureInfo.CurrentUICulture.ToString();

            if (langToggle != null)
            {
                if (!String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrls").ToString()))
                {
                    string formattedUrl;

                    if (lang != "en")
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsEn").ToString()))
                            {
                                formattedUrl = String.Format(langToggle.Template, "/", "English");
                            }
                            else
                            {
                                formattedUrl = String.Format(langToggle.Template, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsEn").ToString(), "English");
                            }
                        }
                        catch (ArgumentNullException)
                        {
                            throw;
                        }
                        catch (FormatException)
                        {
                            throw;
                        }
                        writer.Write(formattedUrl);
                    }

                    if (lang != "es")
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsEs").ToString()))
                            {
                                formattedUrl = String.Format(langToggle.Template, "/espanol", "Español");
                            }
                            else
                            {
                                formattedUrl = String.Format(langToggle.Template, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsEs").ToString(), "Español");
                            }
                        }
                        catch (ArgumentNullException)
                        {
                            throw;
                        }
                        catch (FormatException)
                        {
                            throw;
                        }
                        writer.Write(formattedUrl);
                    }

                    if (!String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsPt").ToString()))
                    {

                        try
                        {
                            formattedUrl = String.Format(langToggle.Template, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsPt").ToString(), "Português");
                        }
                        catch (ArgumentNullException)
                        {
                            throw;
                        }
                        catch (FormatException)
                        {
                            throw;
                        }
                        writer.Write(formattedUrl);
                    }


                    if (!String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsZh").ToString()))
                    {
                        try
                        {
                            formattedUrl = String.Format(langToggle.Template, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("TranslationUrlsZh").ToString(), "中文");
                        }
                        catch (ArgumentNullException)
                        {
                            throw;
                        }
                        catch (FormatException)
                        {
                            throw;
                        }
                        writer.Write(formattedUrl);
                    }
                }
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


