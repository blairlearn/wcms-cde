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

    [ToolboxData("<{0}:AddThisButtonList runat=server></{0}:AddThisButtonList>")]
    public class PageOptionsButtonListControl : WebControl
    {

        private AddThisLanguageCollection _itemsCollection;

        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        /// <summary>
        /// Gets and sets the collection of AddThisLanguageItems
        /// </summary>
        [PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false), DefaultValue((string)null), Description("DataControls_Columns"), Category("Default")]
        public virtual AddThisLanguageCollection AddThisButtonLanguages
        {
            get
            {
                if (this._itemsCollection == null)
                {
                    this._itemsCollection = new AddThisLanguageCollection();
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
            /*
            if (!PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys.Contains("mobileShare"))
            {
                this.Visible = false;
                return;
            }
            */
            base.Render(writer);
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            #region Var AddThis_Share
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write("var addthis_share = {\n");
            bool multipleConfigItems = false;
            //title
            if (!string.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetField("add_this_title")))
            {
                if (multipleConfigItems)
                {
                    writer.Write(",\n");
                }
                writer.Write("title: \"" + PageAssemblyContext.Current.PageAssemblyInstruction.GetField("add_this_title") + "\"");
                multipleConfigItems = true;
            }
            //description
            if (!string.IsNullOrEmpty(PageAssemblyContext.Current.PageAssemblyInstruction.GetField("add_this_description")))
            {
                if (multipleConfigItems)
                {
                    writer.Write(",\n");
                }
                writer.Write("description: \"" + PageAssemblyContext.Current.PageAssemblyInstruction.GetField("add_this_description") + "\"");
                multipleConfigItems = true;
            }
            //url
            /**if (!PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("add_this_url").Equals(""))
            {
                if (multipleConfigItems)
                {
                    writer.Write(",\n");
                }
                writer.Write("url: \"" + PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("add_this_url") + "\"");
                multipleConfigItems = true;
            }*/
            writer.Write("}");
            writer.RenderEndTag();
            #endregion

            #region Var AddThis_Config
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write("var addthis_config = {\n");
            multipleConfigItems = false;
            String language = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language");
            if (!language.Equals(""))
            {
                AddThisButtonLanguageItem langItem = _itemsCollection[language];
                if (langItem != null)
                {
                    //Custom Button Definitions
                    AddThisButtonCollection buttonCollection = langItem.ButtonsCollection;
                    if (buttonCollection != null)
                    {
                        bool hasCustomButtons = false;
                        foreach (AddThisButtonItem currButton in buttonCollection)
                        {
                            if (currButton.GetType().Name.Equals("CustomAddThisButtonItem"))
                            {
                                if (!hasCustomButtons)
                                {
                                    writer.Write("services_custom: {");
                                    hasCustomButtons = true;
                                }

                                bool hasMoreThenOneProperty = false;
                                //Name
                                if (!string.IsNullOrEmpty(((CustomAddThisButtonItem)currButton).Name))
                                {
                                    writer.Write("name: \"" + ((CustomAddThisButtonItem)currButton).Name + "\"");
                                    hasMoreThenOneProperty = true;
                                }
                                //Url
                                if (!string.IsNullOrEmpty(((CustomAddThisButtonItem)currButton).Url))
                                {
                                    if (hasMoreThenOneProperty)
                                    {
                                        writer.Write(",\n");
                                    }
                                    hasMoreThenOneProperty = true;
                                    writer.Write("url: \"" + ((CustomAddThisButtonItem)currButton).Url + "\"");
                                }
                                //Icon
                                if (!string.IsNullOrEmpty(((CustomAddThisButtonItem)currButton).Icon))
                                {
                                    if (hasMoreThenOneProperty)
                                    {
                                        writer.Write(",\n");
                                    }
                                    hasMoreThenOneProperty = true;
                                    writer.Write("icon: \"" + ((CustomAddThisButtonItem)currButton).Icon + "\"");
                                }

                            }
                        }
                        if (hasCustomButtons)
                        {
                            writer.Write("}");
                            multipleConfigItems = true;
                        }
                    }
                    //Account Field
                    if (!langItem.Account.Equals(""))
                    {
                        if (multipleConfigItems)
                        {
                            writer.Write(",\n");
                        }
                        writer.Write("username: \"" + langItem.Account + "\"");
                        multipleConfigItems = true;
                    }
                    //Language Field
                    if (!string.IsNullOrEmpty(langItem.Language))
                    {
                        if (multipleConfigItems)
                        {
                            writer.Write(",\n");
                        }
                        writer.Write("ui_language: \"" + langItem.Language + "\"");
                        multipleConfigItems = true;
                    }
                    //Compact Field
                    if (!string.IsNullOrEmpty(langItem.Compact))
                    {
                        if (multipleConfigItems)
                        {
                            writer.Write(",\n");
                        }
                        writer.Write("services_compact: \"" + langItem.Compact + "\"");
                        multipleConfigItems = true;
                    }
                    //Expanded Field
                    if (!string.IsNullOrEmpty(langItem.Expanded))
                    {
                        if (multipleConfigItems)
                        {
                            writer.Write(",\n");
                        }
                        writer.Write("services_expanded: \"" + langItem.Expanded + "\"");
                        multipleConfigItems = true;
                    }
                    //508 compliance
                    if (multipleConfigItems)
                    {
                        writer.Write(",\n");
                    }
                    writer.Write("ui_508_compliant: true");
                }
                writer.Write("}");
                writer.RenderEndTag();
            }
            #endregion

            //writer.AddAttribute(HtmlTextWriterAttribute.Class, "addthis_toolbox addthis_container addthis_default_style addthis_32x32_style");
            base.RenderBeginTag(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {

            if (PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys.Contains("viewall"))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("viewall").ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/design-elements/logos/viewall.png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            if (PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys.Contains("print") ||
                PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys.Contains("printall"))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("print").ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/design-elements/logos/print.png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            if (PageAssemblyContext.Current.PageAssemblyInstruction.AlternateContentVersionsKeys.Contains("email"))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Href, PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("email").ToString());
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/design-elements/logos/email.png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }


            foreach (AddThisButtonItem button in _itemsCollection[PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language")].ButtonsCollection)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "addthis_button_" + button.Service);

                // Support for GoogleAddThisButtonItem 
                if (button.GetType().Name.Equals("GoogleAddThisButtonItem"))
                {
                    if (!string.IsNullOrEmpty(((GoogleAddThisButtonItem)button).Size))
                    {
                        writer.AddAttribute("g:plusone:size", ((GoogleAddThisButtonItem)button).Size);
                    }

                    if (!string.IsNullOrEmpty(((GoogleAddThisButtonItem)button).Count))
                    {
                        writer.AddAttribute("g:plusone:count", ((GoogleAddThisButtonItem)button).Count);
                    }
                }

                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/design-elements/logos/" +
                    button.Service.ToString() + ".png");
                writer.RenderBeginTag(HtmlTextWriterTag.Img);
                writer.RenderEndTag();
                writer.RenderEndTag();
            }
        }
        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-4ed7cc9f006efaac");
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.RenderEndTag();
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
