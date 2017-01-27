using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class EmailButtonItem : PageOptionsButtonItem
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
                    this.OnEmailButtonItemChanged();
                }
            }
        }

        internal event EventHandler EmailButtonItemChanged;

        protected internal void OnEmailButtonItemChanged()
        {
            if (this.EmailButtonItemChanged != null)
            {
                this.EmailButtonItemChanged(this, EventArgs.Empty);
            }
        }

        protected internal EmailButtonItem CloneEmailButtonItem()
        {
            EmailButtonItem newEmailButtonItem = this.CreateEmailButtonItem();
            this.CopyProperties(newEmailButtonItem);
            return newEmailButtonItem;
        }

        protected void CopyProperties(EmailButtonItem newEmailButtonItem)
        {
            ((EmailButtonItem)newEmailButtonItem).Title = this.Title;
        }

        protected EmailButtonItem CreateEmailButtonItem()
        {
            return new EmailButtonItem();
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
                    this.OnEmailButtonItemChanged();
                }
            }
        }
    }
}
