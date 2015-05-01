using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Reflection;

namespace NCI.Web.UI.WebControls.FormControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:AccessibleCheckBoxList runat=server></{0}:AccessibleCheckBoxList>")]
    public class AccessibleCheckBoxList : DataBoundControl, INamingContainer, IPostBackDataHandler
    {

        private static readonly object EventSelectedIndexChanged = new object();
        private static readonly object EventTextChanged = new object();
        private ListItemCollection items;
        private int cachedSelectedIndex;
        private string cachedSelectedValue;
        private ArrayList cachedSelectedIndices;
        private bool _stateLoaded;

        #region Properties

        [Themeable(false)]
        [Category("Behavior")]
        [DefaultValue(false)]
        public virtual bool AppendDataBoundItems
        {
            get
            {
                object obj = this.ViewState["AppendDataBoundItems"];
                if (obj != null)
                    return (bool)obj;
                else
                    return false;
            }
            set
            {
                this.ViewState["AppendDataBoundItems"] = (object)(bool)(value ? true : false);
                if (!this.Initialized)
                    return;
                this.RequiresDataBinding = true;
            }
        }

        //We will not support an AutoPostBack property as it does not make sense for this control

        [Category("Behavior")]
        [DefaultValue(false)]
        [Themeable(false)]
        public virtual bool CausesValidation
        {
            get
            {
                object obj = this.ViewState["CausesValidation"];
                if (obj != null)
                    return (bool)obj;
                else
                    return false;
            }
            set
            {
                this.ViewState["CausesValidation"] = (object)(bool)(value ? true : false);
            }
        }

        [Themeable(false)]
        [Category("Data")]
        [DefaultValue("")]
        public virtual string DataTextField
        {
            get
            {
                return (string)this.ViewState["DataTextField"] ?? string.Empty;
            }
            set
            {
                this.ViewState["DataTextField"] = (object)value;
                if (!this.Initialized)
                    return;
                this.RequiresDataBinding = true;
            }
        }

        [DefaultValue("")]
        [Themeable(false)]
        [Category("Data")]
        public virtual string DataTextFormatString
        {
            get
            {
                return (string)this.ViewState["DataTextFormatString"] ?? string.Empty;
            }
            set
            {
                this.ViewState["DataTextFormatString"] = (object)value;
                if (!this.Initialized)
                    return;
                this.RequiresDataBinding = true;
            }
        }

        [DefaultValue("")]
        [Category("Data")]
        [Themeable(false)]
        public virtual string DataValueField
        {
            get
            {
                return (string)this.ViewState["DataValueField"] ?? string.Empty;
            }
            set
            {
                this.ViewState["DataValueField"] = (object)value;
                if (!this.Initialized)
                    return;
                this.RequiresDataBinding = true;
            }
        }

        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [Category("Default")]
        [DefaultValue(null)]
        [Editor("System.Web.UI.Design.WebControls.ListItemsCollectionEditor,System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [MergableProperty(false)]
        public virtual ListItemCollection Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new ListItemCollection();
                    if (this.IsTrackingViewState)
                        
                        ((IStateManager)this.items).TrackViewState();
                }
                return this.items;
            }
        }

        /// <devdoc>
        ///    <para>Indicates the ordinal index of the first selected item within the
        ///       list.</para>
        /// </devdoc>
        [
        Bindable(true),
        Browsable(false),
        DefaultValue(0),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Themeable(false),
        Category("Behavior")
        ]
        public virtual int SelectedIndex
        {
            get
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Selected)
                        return i;
                }
                return -1;
            }
            set
            {
                if (value < -1)
                {
                    if (Items.Count == 0)
                    {
                        // VSW 540083: If there are no items, setting SelectedIndex < -1 is the same as setting it to -1.  Don't throw.
                        value = -1;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("value", String.Format("'{0}' has a {1} which is invalid because it does not exist in the list of items.", ID, "SelectedIndex"));
                    }
                }

                if ((Items.Count != 0 && value < Items.Count) || value == -1)
                {
                    ClearSelection();
                    if (value >= 0)
                    {
                        Items[value].Selected = true;
                    }
                }
                else
                {
                    // if we're in a postback and our state is loaded but the selection doesn't exist in the list of items,
                    // throw saying we couldn't find the selected item.
                    if (_stateLoaded)
                    {
                        throw new ArgumentOutOfRangeException("value", String.Format("'{0}' has a {1} which is invalid because it does not exist in the list of items.", ID, "SelectedIndex"));
                    }
                }
                // always save the selectedindex
                // When we've databound, we'll have items from viewstate on the next postback.
                // If we don't cache the selected index and reset it after we databind again,
                // the selection goes away.  So we always have to save the selectedIndex for restore
                // after databind.
                cachedSelectedIndex = value;
            }
        }


        //SelectedIndex
        /// <devdoc>
        ///    <para>Indicates the ordinal index of the default item within the
        ///       list.  This will be the item that unselects all other values.</para>
        /// </devdoc>
        [
        Bindable(true),
        Browsable(false),
        DefaultValue(0),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Themeable(false),
        Category("Behavior")
        ]
        public virtual int DefaultIndex
        {
            get
            {
                object i = ViewState["DefaultIndex"];
                return (int)(i ?? -1);
            }
            set
            {
                ViewState["DefaultIndex"] = value;
            }
        }

        /// <devdoc>
        ///    <para>A protected property. Gets an array of selected
        ///       indexes within the list. This property is read-only.</para>
        /// </devdoc>
        internal virtual ArrayList SelectedIndicesInternal
        {
            get
            {
                cachedSelectedIndices = new ArrayList(3);
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Selected)
                    {
                        cachedSelectedIndices.Add(i);
                    }
                }
                return cachedSelectedIndices;
            }
        }

        /// <devdoc>
        ///    <para>Indicates the first selected item within the list.
        ///       This property is read-only.</para>
        /// </devdoc>
        [
        Category("Behavior"),
        Browsable(false),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
        ]
        public virtual ListItem SelectedItem
        {
            get
            {
                int i = SelectedIndex;
                return (i < 0) ? null : Items[i];
            }
        }

        [
        Category("Behavior"),
        Themeable(false),
        DefaultValue("")
        ]
        public virtual string ValidationGroup
        {
            get
            {
                string s = (string)ViewState["ValidationGroup"];
                return ((s == null) ? string.Empty : s);
            }
            set
            {
                ViewState["ValidationGroup"] = value;
            }
        }

        /// <devdoc>
        ///    <para>Indicates the value of the first selected item within the
        ///       list.</para>
        /// </devdoc>
        [
        Bindable(true, BindingDirection.TwoWay),
        Browsable(false),
        DefaultValue(""),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Themeable(false),
        Category("Behavior")
        ]
        public virtual string SelectedValue
        {
            get
            {
                int i = SelectedIndex;
                return (i < 0) ? String.Empty : Items[i].Value;
            }
            set
            {
                if (Items.Count != 0)
                {
                    // at design time, a binding on SelectedValue will be reset to the default value on OnComponentChanged
                    if (value == null || (DesignMode && value.Length == 0))
                    {
                        ClearSelection();
                        return;
                    }

                    ListItem selectItem = Items.FindByValue(value);
                    // if we're in a postback and our state is loaded or the page isn't a postback but all persistance is loaded
                    // but the selection doesn't exist in the list of items,
                    // throw saying we couldn't find the selected value.
                    bool loaded = Page != null && Page.IsPostBack && _stateLoaded;

                    if (loaded && selectItem == null)
                    {
                        throw new ArgumentOutOfRangeException("value", String.Format("'{0}' has a {1} which is invalid because it does not exist in the list of items.", ID, "SelectedValue"));
                    }

                    if (selectItem != null)
                    {
                        ClearSelection();
                        selectItem.Selected = true;
                    }
                }
                // always save the selectedvalue
                // for later databinding in case we have viewstate items or static items
                cachedSelectedValue = value;
            }
        }

        //Not implementing a text property since we can have multiple selections
        //public virtual string Text {}
        
        /// <summary>
        /// Gets or sets the Legend of this check box list
        /// </summary>
        [
        Browsable(false),
        Themeable(false),
        DefaultValue(""),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
        Category("Behavior")
        ]
        public virtual string Legend {
            get
            {
                string s = (string)ViewState["Legend"];
                return ((s == null) ? string.Empty : s);
            }
            set
            {
                ViewState["Legend"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the Empty text to be displayed when there are no items
        /// </summary>
        [
            Localizable(true),
            Category("Appearance"),
            DefaultValue(""),
            PersistenceMode(PersistenceMode.EncodedInnerDefaultProperty)
        ]
        public virtual string EmptyText
        {
            get
            {
                string s = (string)ViewState["EmptyText"];
                return ((s == null) ? string.Empty : s);
            }
            set
            {
                ViewState["EmptyText"] = value;
            }
        }



        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                //We will make this item output a field set.
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion

        #region Events

        /// <devdoc>
        ///    Occurs when the list selection is changed upon server
        ///    postback.
        /// </devdoc>
        [
        Category("Action")
        ]
        public event EventHandler SelectedIndexChanged
        {
            add
            {
                Events.AddHandler(EventSelectedIndexChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventSelectedIndexChanged, value);
            }
        }

        /// <devdoc>
        ///    <para>Occurs when the content of the text box is
        ///       changed upon server postback.</para>
        /// </devdoc>
        [
        Category("Action")
        ]
        public event EventHandler TextChanged
        {
            add
            {
                Events.AddHandler(EventTextChanged, value);
            }
            remove
            {
                Events.RemoveHandler(EventTextChanged, value);
            }
        }

        #endregion

        //Ok, let's see what this needs to do...
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // Make sure we are in a form tag with runat=server.
            if (Page != null)
            {
                Page.VerifyRenderingInServerForm(this);
            }

            //Determine if these are needed - most likely the answer is yes.

            PostBackOptions options = new PostBackOptions(this, String.Empty);

            //    // ASURT 98368
            //    // Need to merge the autopostback script with the user script
            if (CausesValidation)
            {
                options.PerformValidation = true;
                options.ValidationGroup = ValidationGroup;
            }

            //onChange = System.Web.Util.MergeScript(onChange, Page.ClientScript.GetPostBackEventReference(options, true));

            //    writer.AddAttribute(HtmlTextWriterAttribute.Onchange, onChange);
            //    if (EnableLegacyRendering)
            //    {
            //        writer.AddAttribute("language", "javascript", false);
            //    }
            //}

            //if (Enabled && !IsEnabled & SupportsDisabledAttribute)
            //{
            //    // We need to do the cascade effect on the server, because the browser
            //    // only renders as disabled, but doesn't disable the functionality.
            //    writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            //}

            base.AddAttributesToRender(writer);
        }


        /// <devdoc>
        ///    <para> Clears out the list selection and sets the
        ///    <see cref='System.Web.UI.WebControls.ListItem.Selected'/> property
        ///       of all items to false.</para>
        /// </devdoc>
        public virtual void ClearSelection()
        {
            for (int i = 0; i < Items.Count; i++)
                Items[i].Selected = false;
            // Don't clear cachedSelectedIndices here because some databound controls (such as SiteMapPath)
            // call databind on all child controls when restoring from viewstate.  We need to preserve the
            // cachedSelectedIndices and restore them again for the second databinding.
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            
            //This is scary as it is aschronous.  The original .NET code uses an internal method that
            // is synchronous.
            GetData().Select(DataSourceSelectArguments.Empty, data => {
                PerformDataBinding(data);
            });
        }


        #region Post Back Data Handling

        /// <internalonly/>
        /// <devdoc>
        /// <para>Processes posted data for the <see cref='System.Web.UI.WebControls.CheckBoxList'/> control.</para>
        /// </devdoc>
        bool IPostBackDataHandler.LoadPostData(String postDataKey, NameValueCollection postCollection)
        {
            return LoadPostData(postDataKey, postCollection);
        }

        /// <internalonly/>
        /// <devdoc>
        /// <para>Processes posted data for the <see cref='System.Web.UI.WebControls.CheckBoxList'/> control.</para>
        /// </devdoc>
        protected virtual bool LoadPostData(String postDataKey, NameValueCollection postCollection)
        {

            if (IsEnabled == false)
            {
                // When a CheckBoxList is disabled, then there is no postback
                // data for it. Any checked state information has been loaded
                // via view state.
                return false;
            }

            //The original .NET code was looking for oddthings since Checkboxes are sub controls
            //in the original implementation.  So we do not need to worry about "effective" ids and
            //such.

            EnsureDataBound();

            //The value of the postCollection for this item will be a comma separated list of
            //indicies.  If empty, nothing is selected.
            if (postCollection[postDataKey] != null) {

                int[] selectedIndicies = ParseSelectedIndicies(postCollection[postDataKey]);
                bool hasChanged = false;

                //Loop through the items
                for (int i = 0; i < Items.Count; i++)
                {
                    bool isSelected = selectedIndicies.Contains(i);

                    if (!isSelected && Items[i].Selected)
                    {
                        hasChanged = true;
                        Items[i].Selected = false;
                    }
                    else if (isSelected && !Items[i].Selected)
                    {
                        hasChanged = true;
                        Items[i].Selected = true;
                    }
                }

                if (hasChanged)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Helper function to parse the selected indicies from post back.
        /// </summary>
        /// <param name="rawSelectedIndicies"></param>
        /// <returns></returns>
        private int[] ParseSelectedIndicies(string rawSelectedIndicies)
        {
            string[] indicies = rawSelectedIndicies.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            List<int> selectedIndicies = new List<int>();

            foreach (string ind in indicies)
            {
                int index = 0;
                if (int.TryParse(ind, out index))
                {
                    if (index >= Items.Count)
                    {
                        throw new IndexOutOfRangeException("The index, " + index.ToString() + ", is not within the range of the items.");
                    }
                }
                else
                {
                    throw new ArgumentException("The index, " + ind + ", is not a valid integer.");
                }

                //By this point in time we know that index is actually a valid parameter.
                selectedIndicies.Add(index);
            }

            return selectedIndicies.ToArray();
        }

        /// <internalonly/>
        /// <devdoc>
        /// <para>Raises when posted data for a control has changed.</para>
        /// </devdoc>
        void IPostBackDataHandler.RaisePostDataChangedEvent()
        {
            RaisePostDataChangedEvent();
        }

        /// <internalonly/>
        /// <devdoc>
        /// <para>Raises when posted data for a control has changed.</para>
        /// </devdoc>
        protected virtual void RaisePostDataChangedEvent()
        {
            OnSelectedIndexChanged(EventArgs.Empty);
        }

        #endregion
        /// <internalonly/>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (Page != null && IsEnabled)
            {

                if (SaveSelectedIndicesViewState == false)
                {
                    // Store a client-side array of enabled control, so we can re-enable them on
                    // postback (in case they are disabled client-side)
                    // Postback is needed when the SelectedIndices are not stored in view state.


                    Type pageType = typeof(System.Web.UI.Page);
                    MethodInfo method = pageType.GetMethod("RegisterEnabledControl", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    method.Invoke(Page, new object[] { this });

                }
            }
        }

        /// <devdoc>
        ///    <para> A protected method. Raises the
        ///    <see langword='SelectedIndexChanged'/> event.</para>
        /// </devdoc>
        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            EventHandler onChangeHandler = (EventHandler)Events[EventSelectedIndexChanged];
            if (onChangeHandler != null) onChangeHandler(this, e);

            OnTextChanged(e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            EventHandler onChangeHandler = (EventHandler)Events[EventTextChanged];
            if (onChangeHandler != null) onChangeHandler(this, e);
        }

        /// <internalonly/>
        /// <devdoc>
        /// </devdoc>
        protected override void PerformDataBinding(IEnumerable dataSource)
        {
            base.PerformDataBinding(dataSource);

            if (dataSource != null)
            {
                bool fieldsSpecified = false;
                bool formatSpecified = false;

                string textField = DataTextField;
                string valueField = DataValueField;
                string textFormat = DataTextFormatString;

                if (!AppendDataBoundItems)
                {
                    Items.Clear();
                }

                ICollection collection = dataSource as ICollection;
                if (collection != null)
                {
                    Items.Capacity = collection.Count + Items.Count;
                }

                if ((textField.Length != 0) || (valueField.Length != 0))
                {
                    fieldsSpecified = true;
                }
                if (textFormat.Length != 0)
                {
                    formatSpecified = true;
                }

                foreach (object dataItem in dataSource)
                {
                    ListItem item = new ListItem();

                    if (fieldsSpecified)
                    {
                        if (textField.Length > 0)
                        {
                            item.Text = DataBinder.GetPropertyValue(dataItem, textField, textFormat);
                        }
                        if (valueField.Length > 0)
                        {
                            item.Value = DataBinder.GetPropertyValue(dataItem, valueField, null);
                        }
                    }
                    else
                    {
                        if (formatSpecified)
                        {
                            item.Text = String.Format(CultureInfo.CurrentCulture, textFormat, dataItem);
                        }
                        else
                        {
                            item.Text = dataItem.ToString();
                        }
                        item.Value = dataItem.ToString();
                    }

                    Items.Add(item);
                }
            }

            // try to apply the cached SelectedIndex and SelectedValue now
            if (cachedSelectedValue != null)
            {
                int cachedSelectedValueIndex = -1;

                //Original .NET code was FindByValueInternal, which calls FindByValue with the same 
                //params.  (Although returning an int, instead of an item. thus requiring us to be
                //slightly slower.  Booo microsoft.)
                ListItem selectedItem = Items.FindByValue(cachedSelectedValue);
                cachedSelectedValueIndex = selectedItem == null ? -1 : Items.IndexOf(selectedItem);


                if (-1 == cachedSelectedValueIndex)
                {
                    throw new ArgumentOutOfRangeException("value", String.Format("'{0}' has a {1} which is invalid because it does not exist in the list of items.", ID, "SelectedValue"));
                }

                if ((cachedSelectedIndex != -1) && (cachedSelectedIndex != cachedSelectedValueIndex))
                {
                    throw new ArgumentException(String.Format("The '{0}' and '{1}' attributes are mutually exclusive.", "SelectedIndex", "SelectedValue"));
                }

                SelectedIndex = cachedSelectedValueIndex;
                cachedSelectedValue = null;
                cachedSelectedIndex = -1;
            }
            else
            {
                if (cachedSelectedIndex != -1)
                {
                    SelectedIndex = cachedSelectedIndex;
                    cachedSelectedIndex = -1;
                }
            }
        }

        protected override void PerformSelect()
        {
            // Override PerformSelect and call OnDataBinding because in V1 OnDataBinding was the function that
            // performed the databind, and we need to maintain backward compat.  OnDataBinding will retrieve the
            // data from the view synchronously and call PerformDataBinding with the data, preserving the OM.
            OnDataBinding(EventArgs.Empty);
            RequiresDataBinding = false;
            MarkAsDataBound();
            OnDataBound(EventArgs.Empty);
        }

        



        /// <devdoc>
        ///    Sets items within the
        ///    list to be selected according to the specified array of indexes.
        /// </devdoc>
        internal void SelectInternal(ArrayList selectedIndices)
        {
            ClearSelection();
            for (int i = 0; i < selectedIndices.Count; i++)
            {
                int n = (int)selectedIndices[i];
                if (n >= 0 && n < Items.Count)
                    Items[n].Selected = true;
            }
            cachedSelectedIndices = selectedIndices;
        }

        /// <devdoc>
        ///    Sets items within the list to be selected from post data.
        ///    The difference is that these items won't be cached and reset after a databind.
        /// </devdoc>
        protected void SetPostDataSelection(int selectedIndex)
        {
            if (Items.Count != 0)
            {
                if (selectedIndex < Items.Count)
                {
                    ClearSelection();
                    if (selectedIndex >= 0)
                    {
                        Items[selectedIndex].Selected = true;
                    }
                }
            }
        }

        #region Rendering Methods

        /// <devdoc>
        /// <para>This method is used by controls and adapters
        /// to render the options inside a select statement.</para>
        /// </devdoc>
        protected override void RenderContents(HtmlTextWriter writer)
        {

            //Draw all the checkboxes in the list of items
            ListItemCollection liCollection = Items;
            int n = liCollection.Count;

            if (n > 0)
            {
                for (int i = 0; i < n; i++)
                {
                    ListItem li = liCollection[i];
                    RenderItem(writer, i, li);

                    if (Page != null)
                    {
                        Page.ClientScript.RegisterForEventValidation(UniqueID, li.Value);
                    }
                }
            }
            else
            {
                //Since there are no items, render the empty text.
                if (!string.IsNullOrEmpty(this.EmptyText))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.Write(EmptyText);
                    writer.RenderEndTag();
                }
            }
        }

        protected virtual void RenderItem(HtmlTextWriter writer, int i, ListItem li)
        {
            string liID = ClientID + ClientIDSeparator + i.ToString();

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "checkbox");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            //---------Begin Rendering opening Input tag-----------
            //Each item needs a unique id.
            writer.AddAttribute(HtmlTextWriterAttribute.Id, liID);
            //Name should be the same for all since this listing should act as a group.
            writer.AddAttribute(HtmlTextWriterAttribute.Name, UniqueID);
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");

            //Make it disabled or not?
            if (li.Enabled == false)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");
            }

            //Is this the default item?
            if (DefaultIndex == i)
            {
                //We are using our jquery.groupedCheckBoxList for this.  so we will
                //make the default item group a, and leave everything else to the other
                //group.
                writer.AddAttribute("data-cbgroup", "a");
            }

            if (li.Selected)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
            }
            //Track the index of the selected item.
            writer.AddAttribute(HtmlTextWriterAttribute.Value, i.ToString(), true);

            //Output any addtional attributes that the list item may have.
            foreach (string attr in li.Attributes.Keys)
            {
                writer.AddAttribute(attr, li.Attributes[attr], true);
            }


            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            //---------End Rendering opening Input Tag----------------
            writer.RenderEndTag(); // /input

            //========= Label =================
            writer.AddAttribute(HtmlTextWriterAttribute.For, liID);
            writer.RenderBeginTag(HtmlTextWriterTag.Label);
            HttpUtility.HtmlEncode(li.Text, writer); //Label Text
            writer.RenderEndTag();
            //========= End Label =============

            writer.RenderEndTag(); // /div
        }

        #endregion

        #region View State Management

        /// <internalonly/>
        /// <devdoc>
        /// </devdoc>
        protected override void TrackViewState()
        {
            base.TrackViewState();
            ((IStateManager)Items).TrackViewState();
        }

        /// <internalonly/>
        /// <devdoc>
        /// </devdoc>
        protected override object SaveViewState()
        {
            object baseState = base.SaveViewState();
            object items = ((IStateManager)Items).SaveViewState();
            object selectedIndicesState = null;

            if (SaveSelectedIndicesViewState)
            {
                selectedIndicesState = SelectedIndicesInternal;
            }

            if (selectedIndicesState != null || items != null || baseState != null)
            {
                return new Triplet(baseState, items, selectedIndicesState);
            }
            return null;
        }

        /// <internalonly/>
        /// <devdoc>
        ///    Load previously saved state.
        ///    Overridden to restore selection.
        /// </devdoc>
        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                Triplet stateTriplet = (Triplet)savedState;
                base.LoadViewState(stateTriplet.First);

                // restore state of items
                ((IStateManager)Items).LoadViewState(stateTriplet.Second);

                // restore selected indices
                ArrayList selectedIndices = stateTriplet.Third as ArrayList;
                if (selectedIndices != null)
                {
                    SelectInternal(selectedIndices);
                }
            }
            else
            {
                base.LoadViewState(null);
            }

            _stateLoaded = true;
        }

        /// <devdoc>
        ///    Determines whether the SelectedIndices must be stored in view state, to
        ///    optimize the size of the saved state.
        /// </devdoc>
        internal bool SaveSelectedIndicesViewState
        {
            get
            {
                // FROM MICROSOFT:::
                // Must be saved when
                // 1. There is a registered event handler for SelectedIndexChanged or TextChanged.  
                //    For our controls, we know for sure that there is no event handler registered for 
                //    SelectedIndexChanged or TextChanged so we can short-circuit that check.
                // 2. Control is not enabled or visible, because the browser's post data will not include this control
                // 3. [REMOVED: The class is a derrived class condition is not applicable]
                // 4. [REMOVED: AutoPostBack Condition is not applicable]
                // 5. The control is paginated.
                // 6. The control contains items that are disabled.  The browser's post data will not
                //    include this data for disabled items, so we need to save those selected indices.
                //
                if ((this.Events[AccessibleCheckBoxList.EventSelectedIndexChanged] != null) ||
                    (this.Events[AccessibleCheckBoxList.EventTextChanged] != null) ||
                    (IsEnabled == false) ||
                    (Visible == false))
                {
                    return true;
                }

                foreach (ListItem listItem in this.Items)
                {
                    if (!listItem.Enabled)
                        return true;
                }

                return true;
            }
        }

        #endregion

        //Don't need because we are always multiselect
        //protected internal virtual void VerifyMultiSelect()
    }
}
