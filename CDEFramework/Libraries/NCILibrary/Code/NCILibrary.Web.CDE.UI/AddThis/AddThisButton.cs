using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class AddThisButtonItem : IStateManager
    {

        private bool _trackViewState = false;

        private string _service;
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

        private string _description;
        public string Description
        {
            get
            {
                return (string)this.ViewState["Description"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Description"]))
                {
                    this.ViewState["Description"] = value;
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

        protected internal AddThisButtonItem CloneAddThisButtonItem()
        {
            AddThisButtonItem newAddThisButtonItem = this.CreateAddThisButtonItem();
            this.CopyProperties(newAddThisButtonItem);
            return newAddThisButtonItem;
        }

        protected void CopyProperties(AddThisButtonItem newAddThisButtonItem)
        {
            ((AddThisButtonItem)newAddThisButtonItem).Service = this.Service;
            ((AddThisButtonItem)newAddThisButtonItem).Url = this.Url;
            ((AddThisButtonItem)newAddThisButtonItem).Description = this.Description;
        }

        protected AddThisButtonItem CreateAddThisButtonItem()
        {
            return new AddThisButtonItem();
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
