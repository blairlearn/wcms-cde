using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class PageOptionsButtonItem : IStateManager
    {

        private bool _trackViewState = false;

        public string Service
        {
            get
            {
                return (string)this.ViewState["Service"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Service"]))
                {
                    this.ViewState["Service"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

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
                    this.OnButtonItemChanged();
                }
            }
        }

        public string CssClass
        {
            get
            {
                return (string)this.ViewState["CssClass"] ?? string.Empty;
            }
            set
            {
                if (!object.Equals(value, this.ViewState["CssClass"]))
                {
                    this.ViewState["CssClass"] = value;
                    this.OnButtonItemChanged();
                }
            }
        }

        public string AlternateContentVersionKey
        {
            get
            {
                return (string)this.ViewState["AlternateContentVersionKey"] ?? string.Empty;
            }
            set
            {
                if (!object.Equals(value, this.ViewState["AlternateContentVersionKey"]))
                {
                    this.ViewState["AlternateContentVersionKey"] = value;
                    this.OnButtonItemChanged();
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

        internal event EventHandler ButtonItemChanged;

        protected internal void OnButtonItemChanged()
        {
            if (this.ButtonItemChanged != null)
            {
                this.ButtonItemChanged(this, EventArgs.Empty);
            }
        }

        protected internal PageOptionsButtonItem CloneAddThisButtonItem()
        {
            PageOptionsButtonItem newAddThisButtonItem = this.CreateAddThisButtonItem();
            this.CopyProperties(newAddThisButtonItem);
            return newAddThisButtonItem;
        }

        protected void CopyProperties(PageOptionsButtonItem newAddThisButtonItem)
        {
            ((PageOptionsButtonItem)newAddThisButtonItem).Service = this.Service;
            ((PageOptionsButtonItem)newAddThisButtonItem).Title = this.Title;
            ((PageOptionsButtonItem)newAddThisButtonItem).CssClass = this.CssClass;
        }

        protected PageOptionsButtonItem CreateAddThisButtonItem()
        {
            return new PageOptionsButtonItem();
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

        public virtual string WebAnalytics
        {
            get
            {
                return (string)this.ViewState["WebAnalytics"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["WebAnalytics"]))
                {
                    this.ViewState["WebAnalytics"] = value;
                    this.OnButtonItemChanged();
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

        /// <summary>
        /// When implemented by a class, loads the server control's previously saved view state to the control.
        /// </summary>
        /// <param name="state">An <see cref="T:System.Object"/> that contains the saved view state values for the control.</param>
        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        /// <summary>
        /// When implemented by a class, saves the changes to a server control's view state to an <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> that contains the view state changes.
        /// </returns>
        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        /// <summary>
        /// When implemented by a class, instructs the server control to track changes to its view state.
        /// </summary>
        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        #endregion


    }
}
