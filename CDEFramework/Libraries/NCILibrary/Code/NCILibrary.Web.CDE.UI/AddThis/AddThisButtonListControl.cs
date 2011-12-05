using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.WebControls.AddThis
{

    [ToolboxData("<{0}:AddThisButtonList runat=server></{0}:AddThisButtonList>")]
    public class AddThisButtonListControl : WebControl
    {

        AddThisLanguageCollection _itemsCollection;
        
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
            if (_itemsCollection.Count > 0)
                base.Render(writer);
        }
        
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "addthis_toolbox addthis_default_style addthis_32x32_style");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            //output.Write(Text);
            //Check the page data.
            foreach (AddThisButtonCollection items in _itemsCollection )
            {
                foreach (AddThisButtonItem button in items.ButtonCollection)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "addthis_button_" + button.Service);
                    writer.RenderBeginTag(HtmlTextWriterTag.A);
                }
            }

        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.RenderEndTag();
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
