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
    public class PageOptionsControl : WebControl
    {

        private PageOptionsLanguageCollection _itemsCollection;

        private IPageAssemblyInstruction pgInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;

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
        public virtual PageOptionsLanguageCollection PageOptionsButtonLanguages
        {
            get
            {
                if (this._itemsCollection == null)
                {
                    this._itemsCollection = new PageOptionsLanguageCollection();
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
            String pageLanguage = pgInstruction.GetField("Language");
            if (_itemsCollection[pageLanguage] == null && _itemsCollection["en"] == null)
            {
                this.Visible = false;
                return;
            }

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
            if (!string.IsNullOrEmpty(pgInstruction.GetField("add_this_title")))
            {
                if (multipleConfigItems)
                {
                    writer.Write(",\n");
                }
                string titleField = pgInstruction.GetField("add_this_title").Replace("\"", "\\\"");
                writer.Write("title: \"" + titleField + "\"");
                multipleConfigItems = true;
            }
            //description
            if (!string.IsNullOrEmpty(pgInstruction.GetField("add_this_description")))
            {
                if (multipleConfigItems)
                {
                    writer.Write(",\n");
                }
                writer.Write("description: \"" + pgInstruction.GetField("add_this_description") + "\"");
                multipleConfigItems = true;
            }
            //url
            /**if (!pgInstruction.GetUrl("add_this_url").Equals(""))
            {
                if (multipleConfigItems)
                {
                    writer.Write(",\n");
                }
                writer.Write("url: \"" + pgInstruction.GetUrl("add_this_url") + "\"");
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
            String language = pgInstruction.GetField("Language");
            if (!language.Equals(""))
            {
                PageOptionsButtonLanguageItem langItem = _itemsCollection[language];
                if (langItem != null)
                {
                    //Custom Button Definitions
                    PageOptionsButtonCollection buttonCollection = langItem.ButtonsCollection;
                    if (buttonCollection != null)
                    {
                        bool hasCustomButtons = false;
                        /*foreach (PageOptionsAddThisButtonItem currButton in buttonCollection)
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
                        }*/
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
            PageOptionsButtonLanguageItem buttonLanguageItem = _itemsCollection["en"];

            string language = pgInstruction.GetField("Language");
            if (language == "es")
            {
                buttonLanguageItem = _itemsCollection["es"];
            }

            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            foreach (PageOptionsButtonItem button in buttonLanguageItem.ButtonsCollection)
            {

                if (pgInstruction.AlternateContentVersionsKeys.Contains(button.AlternateContentVersionKey))
                {
                    string liClass = string.Empty;
                    liClass += button.CssClass;
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, liClass);
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);

                    if (button is PageOptionsAddThisButtonItem)
                    {
                        RenderPageOptionsAddThisButton(writer, (PageOptionsAddThisButtonItem)button);
                    }

                    if (button is EmailButtonItem)
                    {
                        RenderEmailButtonItem(writer, (EmailButtonItem)button);
                    }

                    if (button is LinkButtonItem)
                    {
                        RenderLinkButtonItem(writer, (LinkButtonItem)button);
                    }
                    writer.RenderEndTag(); // li
                }
            }

            writer.RenderEndTag(); // ul

        }

        private static void RenderPageOptionsAddThisButton(HtmlTextWriter writer, PageOptionsAddThisButtonItem button)
        {
            string btnClass = string.Empty;
            string btnAlt = string.Empty;
            btnClass += "addthis_button_" + button.Service;
            btnClass += " add_this_btn";
            writer.AddAttribute(HtmlTextWriterAttribute.Class, btnClass);
            if (!string.IsNullOrEmpty(button.Title.Trim()))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, button.Title);
                btnAlt = button.Title;
            }
            if (!string.IsNullOrEmpty(button.WebAnalytics.Trim()))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, button.WebAnalytics);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            writer.AddAttribute(HtmlTextWriterAttribute.Alt, btnAlt);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/spacer.gif");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);

            writer.RenderEndTag(); // img
            writer.RenderEndTag(); // a
        }

        private void RenderEmailButtonItem(HtmlTextWriter writer, EmailButtonItem button)
        {
            string btnAlt = string.Empty;
            if (!string.IsNullOrEmpty(button.Title.Trim()))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, button.Title);
                btnAlt = button.Title;
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Href, pgInstruction.GetUrl("email").ToString());
            string emailOnClick = string.Empty;
            if (!string.IsNullOrEmpty(button.WebAnalytics.Trim()))
            {
                emailOnClick += button.WebAnalytics;
            }
            emailOnClick += " " + "dynPopWindow('" + pgInstruction.GetUrl("email").ToString().Replace("'", "%27").Replace("(", "%28").Replace(")", "%29") + "', 'emailPopUp', 'height=525,width=492'); return false;";
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, emailOnClick);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, btnAlt);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/spacer.gif");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag(); // img
            writer.RenderEndTag(); // a
        }

        private void RenderLinkButtonItem(HtmlTextWriter writer, LinkButtonItem button)
        {
            string btnAlt = string.Empty;
            if (!string.IsNullOrEmpty(button.Title.Trim()))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, button.Title);
                btnAlt = button.Title;
            }
            if (!string.IsNullOrEmpty(button.WebAnalytics.Trim()))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, button.WebAnalytics);
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Href, pgInstruction.GetUrl(button.AlternateContentVersionKey).ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, btnAlt);
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "/publishedcontent/images/images/spacer.gif");
            writer.RenderBeginTag(HtmlTextWriterTag.Img);
            writer.RenderEndTag(); // img
            writer.RenderEndTag(); // a
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            base.RenderEndTag(writer);
            writer.AddAttribute("async", "async");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "//s7.addthis.com/js/250/addthis_widget.js#pubid=xa-4ed7cc9f006efaac");
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
