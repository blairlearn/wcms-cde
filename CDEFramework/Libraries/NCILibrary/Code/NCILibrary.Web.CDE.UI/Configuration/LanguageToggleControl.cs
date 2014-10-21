using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.WebControls
{

    [ToolboxData("<{0}:LanguageToggleList runat=server></{0}:LanguageToggleList>")]
    public class LanguageToggleControl : WebControl
    {

        private LanguageToggleLanguageCollection _itemsCollection;

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets and sets the collection of LanguageToggleLanguageItems
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false), DefaultValue((string)null), Description("DataControls_Columns"), Category("Default")]
        public virtual LanguageToggleLanguageCollection LanguageToggleLanguages
        {
            get
            {
                if (this._itemsCollection == null)
                {
                    this._itemsCollection = new LanguageToggleLanguageCollection();
                    if (base.IsTrackingViewState)
                    {
                        ((IStateManager)this._itemsCollection).TrackViewState();
                    }
                }
                return this._itemsCollection;
            }
            set
            {
                _itemsCollection = value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //Don't render the box if there is nothing to render.
            if (_itemsCollection.Count == 0)
            {
                this.Visible = false;
                return;
            }
            String pageLanguage = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language");
            if (_itemsCollection[pageLanguage] == null)
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



        /// <summary>
        /// Compare each template toggle item with the available translation keys for a content item; print 
        /// link if there is a match or default URL if no match exists for English or Spanish
        /// </summary>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string dictionaryUrl = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CanonicalUrl").ToString();

            if (!String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetTranslationUrl("TranslationUrls").ToString()))
            {
                foreach (LanguageToggle lang in _itemsCollection[PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language")].LangsCollection)
                {
                    if (PageAssemblyContext.Current.PageAssemblyInstruction.TranslationKeys.Length < 1)
                    {
                        if (!String.IsNullOrEmpty(lang.Url))
                        {
                            writer.Write("&nbsp;&nbsp;&nbsp;");
                            if (!string.IsNullOrEmpty(lang.OnClick.Trim()))
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, lang.OnClick);
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, lang.Url);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(lang.Title);
                            writer.RenderEndTag();
                        }
                    }

                    foreach (string key in PageAssemblyContext.Current.PageAssemblyInstruction.TranslationKeys)
                    {
                        if ((lang.Locale == "en-us" || lang.Locale == "es-us")&& String.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetTranslationUrl(lang.Locale).ToString()))
                        {
                            writer.Write("&nbsp;&nbsp;&nbsp;");
                            if (!string.IsNullOrEmpty(lang.OnClick.Trim()))
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, lang.OnClick);
                            }
                            writer.AddAttribute(HtmlTextWriterAttribute.Href, lang.Url);
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(lang.Title);
                            writer.RenderEndTag();
                        }

                        if (lang.Locale == key)
                        {
                            writer.Write("&nbsp;&nbsp;&nbsp;");
                            if (!string.IsNullOrEmpty(lang.OnClick.Trim()))
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, lang.OnClick);
                            }

                            // If this is a Cancer Terms Dictionary page, the language toggle should link to the selected dictionary item, not home
                            if ((lang.Locale == "es-us") && (dictionaryUrl.IndexOf("/dictionary?") > -1))
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Href, dictionaryUrl.Replace("/dictionary?","/diccionario?"));
                            }
                            else if ((lang.Locale == "en-us") && (dictionaryUrl.IndexOf("/diccionario?") > -1))
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Href, dictionaryUrl.Replace("/diccionario?", "/dictionary?"));
                            }
                            else
                            {
                                writer.AddAttribute(HtmlTextWriterAttribute.Href, PageAssemblyContext.Current.PageAssemblyInstruction.GetTranslationUrl(key).ToString());
                            }
                            writer.RenderBeginTag(HtmlTextWriterTag.A);
                            writer.Write(lang.Title);
                            writer.RenderEndTag();
                        }
                    } // foreach translation key
                } // foreach template language toggle item
            } // if translation keys exist 
        } // RenderContents()


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
            object obj2 = ((IStateManager)_itemsCollection).SaveViewState();

            return new object[] { obj1, obj2 };
        }

        /// <summary>
        /// Causes the control to track changes to its view state so they can be stored in the object's <see cref="P:System.Web.UI.Control.ViewState"/> property.
        /// </summary>
        protected override void TrackViewState()
        {
            base.TrackViewState();

            if (_itemsCollection != null)
            {
                ((IStateManager)_itemsCollection).TrackViewState();
            }
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

                if (objArray[1] != null)
                {
                    ((IStateManager)_itemsCollection).LoadViewState(objArray[1]);
                }
            }
            else
            {
                base.LoadViewState(null);
            }
        }
    }
}
