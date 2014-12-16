using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class LinkButtonItem : PageOptionsButtonItem
    {

        private bool _trackViewState = false;

        private string _title;
        public string Title
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

        private StateBag _statebag = new StateBag();
        protected StateBag ViewState
        {
            get
            {
                return this._statebag;
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

        protected void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;

                if (objArray[0] != null)
                    ((IStateManager)this.ViewState).LoadViewState(objArray[0]);
            }
        }

        protected object SaveViewState()
        {
            object obj1 = ((IStateManager)this.ViewState).SaveViewState();

            if (obj1 == null)
                return null;

            return new object[] { obj1 };

        }

        protected void TrackViewState()
        {
            this._trackViewState = true;
            ((IStateManager)this.ViewState).TrackViewState();
            //If there are any sub IStateManager items, start tracking view state for them as well.
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

        internal void SetDirty()
        {
            this._statebag.SetDirty(true);

            //If there are any sub IStateManager items, set them dirty as well.
        }
        #region IStateManager Members

        /// <summary>
        /// When implemented by a class, gets a value indicating whether a server control is tracking its view state changes.
        /// </summary>
        /// <value></value>
        /// <returns>true if a server control is tracking its view state changes; otherwise, false.
        /// </returns>
        public bool IsTrackingViewState
        {
            get { return this._trackViewState; }
        }


        #endregion


    }
}
