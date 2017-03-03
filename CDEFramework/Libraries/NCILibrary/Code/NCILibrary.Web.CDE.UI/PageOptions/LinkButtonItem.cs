using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class LinkButtonItem : PageOptionsButtonItem
    {
        public new string Title
        {
            get
            {
                return (string)this.ViewState["Title"] ?? string.Empty;
            }
            set
            {
                if (!object.Equals(value, this.ViewState["Title"]))
                {
                    this.ViewState["Title"] = value;
                    this.OnLinkButtonItemChanged();
                }
            }
        }

        internal event EventHandler LinkButtonItemChanged;

        protected internal void OnLinkButtonItemChanged()
        {
            if (this.LinkButtonItemChanged != null)
            {
                this.LinkButtonItemChanged(this, EventArgs.Empty);
            }
        }

        protected internal LinkButtonItem CloneLinkButtonItem()
        {
            LinkButtonItem newLinkButtonItem = this.CreateLinkButtonItem();
            this.CopyProperties(newLinkButtonItem);
            return newLinkButtonItem;
        }

        protected void CopyProperties(LinkButtonItem newLinkButtonItem)
        {
            ((LinkButtonItem)newLinkButtonItem).Title = this.Title;
        }

        protected LinkButtonItem CreateLinkButtonItem()
        {
            return new LinkButtonItem();
        }

        public virtual string OnClick
        {
            get
            {
                return (string)this.ViewState["OnClick"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["OnClick"]))
                {
                    this.ViewState["OnClick"] = value;
                    this.OnLinkButtonItemChanged();
                }
            }
        }
    }
}
