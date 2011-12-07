using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.UI.WebControls
{
    public class CustomAddThisButtonItem : AddThisButtonItem
    {
        private string _name;
        public string Name
        {
            get
            {
                return (string)this.ViewState["Name"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Name"]))
                {
                    this.ViewState["Name"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

        private string _icon;
        public string Icon
        {
            get
            {
                return (string)this.ViewState["Icon"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Icon"]))
                {
                    this.ViewState["Icon"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

        private string _url;
        public string Url
        {
            get
            {
                return (string)this.ViewState["Url"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Url"]))
                {
                    this.ViewState["Url"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

        protected void CopyProperties(CustomAddThisButtonItem newAddThisButtonItem)
        {
            ((CustomAddThisButtonItem)newAddThisButtonItem).Service = this.Service;
            ((CustomAddThisButtonItem)newAddThisButtonItem).Url = this.Url;
            ((CustomAddThisButtonItem)newAddThisButtonItem).Name = this.Name;
            ((CustomAddThisButtonItem)newAddThisButtonItem).Icon = this.Icon;
        }

        protected internal CustomAddThisButtonItem CloneAddThisButtonItem()
        {
            CustomAddThisButtonItem newAddThisButtonItem = this.CreateAddThisButtonItem();
            this.CopyProperties(newAddThisButtonItem);
            return newAddThisButtonItem;
        }

        protected CustomAddThisButtonItem CreateAddThisButtonItem()
        {
            return new CustomAddThisButtonItem();
        }


    }
}
