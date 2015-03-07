using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Util;

namespace NCI.Web.UI.WebControls.FormControls
{
    //[DefaultProperty("Text")]
    [ToolboxData("<{0}:DeleteList runat=server></{0}:DeleteList>")]
    public class DeleteList : ListControl, INamingContainer, IPostBackDataHandler
    {
        #region Fields

        private const string _valueListName = "ValueList";      // Hidden list of values
        private const string _nameListName = "NameList";        // Hidden list of item names.
        private const string _deletedListName = "DeletedList";  // Hidden list of flags for which item to delete.
        private const string _deleteButtonName = "DeleteButton";
        private const string _emptyMessageName = "EmptyMessage";
        private const string _displayListName = "DisplayList";  // The visible list of items.

        private const string _selectionSeparatorChar = "|";

        #endregion

        #region Properties

        /// <summary>
        /// The text to display when the control's list is empty.
        /// </summary>
        [
        Bindable(true),
        Category("Layout"),
        DefaultValue(""),
        Description("Text to display when the control's list is empty."),
        Localizable(true),
        PersistenceMode(PersistenceMode.Attribute)
        ]
        public virtual string EmptyListText
        {
            get
            {
                return (string)(ViewState["EmptyListText"] ?? "(no entries selected)");
            }
            set { ViewState["EmptyListText"] = value; }
        }

        #endregion


        #region Protected Properties

        /// <summary>
        /// Override the control's default outermost tag.
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #endregion


        #region Methods supporting IPostBackDataHandler

        /// <summary>
        /// Implementation of IPostBackDataHandler.LoadPostData().
        /// This method examines the postback data and updates the control's state.
        /// </summary>
        /// <param name="postDataKey">Provides the dictionary key for the control instance.</param>
        /// <param name="postCollection">A dictionary collection of all incoming form values.</param>
        /// <returns>True if the control's state changes.</returns>
        /// <remarks>If this method returns true, the .NET framework will call RaisePostDataChangedEvent().
        /// 
        /// Because this is effectively a composite control with the potential for multiple fields
        /// being returned, the postDataKey parameter is ignored.</remarks>
        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            // Order is important.  Because an item might be added and subsequently deleted, we have
            // to process insertions before deletions.
            bool itemsHaveBeenAdded = ProcessInsertions(postCollection);
            bool itemsHaveBeenDeleted = ProcessDeletions(postCollection);

            return itemsHaveBeenAdded || itemsHaveBeenDeleted;
        }

        /// <summary>
        /// Determines whether any items have been added to the list of selections
        /// This method relies on the client-side component marking items as deleted,
        /// but not removing them from the list.
        /// </summary>
        /// <param name="postCollection">A dictionary collection of all incoming form values.</param>
        /// <returns></returns>
        private bool ProcessInsertions(NameValueCollection postCollection)
        {
            bool itemsWereAdded = false;

            string[] selectionValue = Strings.ToStringArray(postCollection[BuildUniqueControlName(_valueListName)], _selectionSeparatorChar);
            string[] selectionText = Strings.ToStringArray(postCollection[BuildUniqueControlName(_nameListName)], _selectionSeparatorChar);

            // Determine whether there were insertions by comparing the number of items returned
            // if form data against the number of Items in the original selection.
            if (selectionValue != null &&
                selectionValue.Length > Items.Count)
            {
                itemsWereAdded = true;

                // Append only the new items.
                int startOffset = Items.Count;
                for (int i = startOffset; i < selectionValue.Length; i++)
                {
                    Items.Add(new ListItem(selectionText[i], selectionValue[i]));
                }
            }

            return itemsWereAdded;
        }

        /// <summary>
        /// Determines whether any list items have been marked for deletion.
        /// This method relies on the client-side component marking items as deleted,
        /// but not removing them from the list.
        /// </summary>
        /// <param name="postCollection">A dictionary collection of all incoming form values.</param>
        /// <returns></returns>
        private bool ProcessDeletions(NameValueCollection postCollection)
        {
            bool itemsWereDeleted = false;

            // Check for use of the delete buttons.
            string fieldName = BuildUniqueControlName(_deleteButtonName);
            if (postCollection[fieldName] != null)
            {
                if (Items.Count > 1)
                {
                    int deleteIndex = Strings.ToInt(postCollection[fieldName]);
                    Items.RemoveAt(deleteIndex);
                }
                else
                    Items.Clear();
                itemsWereDeleted = true;
            }

            // Check for items marked for deletion by asynchronous code.
            bool[] deleteFlags = null;
            string deleteFieldValues = postCollection[BuildUniqueControlName(_deletedListName)];

            if(!string.IsNullOrEmpty(deleteFieldValues))
                deleteFlags = ToBooleanArray(Strings.ToStringArray(deleteFieldValues, _selectionSeparatorChar));

            // Removing items from the ListItemCollection causes items with larger offsets
            // to have their offset decreased (using Remove against an iterator isn't supported
            // at all).  Rather than maintain separate indexes for the delete flags vs. the
            // ListItemCollection, it's simple rto count down from the top.
            for (int index = Items.Count - 1; index >= 0; index--)
            {
                if (deleteFlags[index])
                {
                    Items.RemoveAt(index);
                    itemsWereDeleted = true;
                }
            }

            return itemsWereDeleted;
        }

        private bool[] ToBooleanArray(string[] flagList)
        {
            if (flagList == null)
                throw new ArgumentNullException("flagList");

            bool[] flags = new bool[flagList.Length];
            for (int i = 0; i < flagList.Length; i++)
            {
                flags[i] = Strings.ToBoolean(flagList[i]);
            }

            return flags;
        }

        /// <summary>
        /// Implementation of IPostBackDataHandler.RaisePostDataChangedEvent().
        /// 
        /// The .NET framework calls this method in response to LoadPostData() returning true,
        /// allowing other controls to be notified that this one's selection has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            base.OnSelectedIndexChanged(EventArgs.Empty);
        }
        
        #endregion


        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "deletelist");
            base.AddAttributesToRender(writer);
        }


        /// <summary>
        /// Register the control to be notified of postback events.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            /// Set up JavaScript resources.
            JSManager.AddResource(this.Page, typeof(DeleteList), "NCI.Web.UI.WebControls.FormControls.Resources.deleteList.js");

            Page.RegisterRequiresPostBack(this);
            base.OnPreRender(e);
        }


        protected override void RenderContents(HtmlTextWriter writer)
        {
            // Outside the UL, add the "The List is Empty" text.  Show if the list is empty, hide otherwise.
            if (Items.Count > 0)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, BuildUniqueControlName(_emptyMessageName));
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write(EmptyListText);
            writer.RenderEndTag();

            //writer.AddStyleAttribute(HtmlTextWriterStyle.ListStyleType, "none");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, BuildUniqueControlName(_displayListName));
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            //foreach (ListItem item in Items)
            for (int i = 0; i < Items.Count; i++)
            {
                ListItem item = Items[i];

                writer.RenderBeginTag(HtmlTextWriterTag.Li);

                writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pseudo-icon-deletelist");
                writer.RenderBeginTag(HtmlTextWriterTag.Button);

                writer.AddAttribute(HtmlTextWriterAttribute.Class, "hidden");
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
                writer.Write("Delete");
                writer.RenderEndTag();

                writer.RenderEndTag();
                writer.Write(item.Text);

                writer.RenderEndTag();
            }

            writer.RenderEndTag();

            DrawHiddenSelectionList(writer);

            // Setup Javascript.
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.Write("$('#" + this.ClientID + "').deletelist();");
            writer.RenderEndTag();
        }

        private void DrawHiddenSelectionList(HtmlTextWriter writer)
        {
            string valueListName = BuildUniqueControlName(_valueListName);
            string textListName = BuildUniqueControlName(_nameListName);
            string deletedListName = BuildUniqueControlName(_deletedListName);

            StringBuilder valueList = new StringBuilder();
            StringBuilder textList = new StringBuilder();
            StringBuilder deletedList = new StringBuilder();
            bool first = true;
            foreach (ListItem  item in Items)
            {
                if (first)
                    first = false;
                else
                {
                    valueList.Append(_selectionSeparatorChar);
                    textList.Append(_selectionSeparatorChar);
                    deletedList.Append(_selectionSeparatorChar);
                }
                valueList.Append(item.Value);
                textList.Append(item.Text);
                deletedList.Append("false");
            }
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, valueList.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Id, valueListName);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, valueListName);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, textList.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Id, textListName);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, textListName);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();

            writer.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, deletedList.ToString());
            writer.AddAttribute(HtmlTextWriterAttribute.Id, deletedListName);
            writer.AddAttribute(HtmlTextWriterAttribute.Name, deletedListName);
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
        }

        #region Rendering Helper Methods

        /// <summary>
        /// Helper function to create a unique name for a UI item based on the
        /// unique Id of the overall control.
        /// </summary>
        /// <param name="controlId">String containing the name of the UI element.</param>
        /// <returns>String value containing a unique Identifier for the UI element.</returns>
        private string BuildUniqueControlName(string controlId)
        {
            return this.ClientID + this.ClientIDSeparator + controlId;
        }

        #endregion
    }
}
