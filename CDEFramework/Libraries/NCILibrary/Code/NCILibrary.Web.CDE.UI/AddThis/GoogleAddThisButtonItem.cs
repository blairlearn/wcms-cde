using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCI.Web.CDE.UI.WebControls
{
    public class GoogleAddThisButtonItem : AddThisButtonItem
    {

        public string Size
        {
            get
            {
                return (string)this.ViewState["Size"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Size"]))
                {
                    this.ViewState["Size"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

        public string Count
        {
            get
            {
                return (string)this.ViewState["Count"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Count"]))
                {
                    this.ViewState["Count"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

        protected void CopyProperties(GoogleAddThisButtonItem newAddThisButtonItem)
        {
            ((GoogleAddThisButtonItem)newAddThisButtonItem).Service = this.Service;
            ((GoogleAddThisButtonItem)newAddThisButtonItem).Count = this.Count;
            ((GoogleAddThisButtonItem)newAddThisButtonItem).Size = this.Size;
        }

        protected internal GoogleAddThisButtonItem CloneAddThisButtonItem()
        {
            GoogleAddThisButtonItem newAddThisButtonItem = this.CreateAddThisButtonItem();
            this.CopyProperties(newAddThisButtonItem);
            return newAddThisButtonItem;
        }

        protected GoogleAddThisButtonItem CreateAddThisButtonItem()
        {
            return new GoogleAddThisButtonItem();
        }
    }
}

