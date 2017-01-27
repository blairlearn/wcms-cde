using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class ClientSideButtonItem : PageOptionsButtonItem
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
                    this.OnClientSideButtonItemChanged();
                }
            }
        }

        internal event EventHandler ClientSideButtonItemChanged;

        protected internal void OnClientSideButtonItemChanged()
        {
            if (this.ClientSideButtonItemChanged != null)
            {
                this.ClientSideButtonItemChanged(this, EventArgs.Empty);
            }
        }

        protected internal ClientSideButtonItem CloneClientSideButtonItem()
        {
            ClientSideButtonItem newClientSideButtonItem = this.CreateClientSideButtonItem();
            this.CopyProperties(newClientSideButtonItem);
            return newClientSideButtonItem;
        }

        protected void CopyProperties(ClientSideButtonItem newClientSideButtonItem)
        {
            ((ClientSideButtonItem)newClientSideButtonItem).Title = this.Title;
        }

        protected ClientSideButtonItem CreateClientSideButtonItem()
        {
            return new ClientSideButtonItem();
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
                    this.OnClientSideButtonItemChanged();
                }
            }
        }
    }
}
