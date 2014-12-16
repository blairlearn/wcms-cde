using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls
{
    public class PageOptionsButtonLanguageItem : IStateManager
    {

        private PageOptionsButtonCollection _buttonsCollection;
        [PersistenceMode(PersistenceMode.InnerProperty), MergableProperty(false), DefaultValue((string)null), Description("DataControls_Columns"), Category("Default")]
        public PageOptionsButtonCollection ButtonsCollection
        {
            get
            {
                if (this._buttonsCollection == null)
                {
                    this._buttonsCollection = new PageOptionsButtonCollection();
                    if (this.IsTrackingViewState)
                    {
                        ((IStateManager)this._buttonsCollection).TrackViewState();
                    }
                }
                return this._buttonsCollection;
            }
            set
            {
                _buttonsCollection = value;
            }
        }

        private bool _trackViewState = false;

        private string _language;
        public string Language
        {
            get
            {
                return (string)this.ViewState["Language"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Language"]))
                {
                    this.ViewState["Language"] = value;
                    this.OnLanguageItemChanged();
                }
            }
        }

        private string _compact;
        public string Compact
        {
            get
            {
                return (string)this.ViewState["Compact"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Compact"]))
                {
                    this.ViewState["Compact"] = value;
                    this.OnLanguageItemChanged();
                }
            }
        }

        private string _expanded;
        public string Expanded
        {
            get
            {
                return (string)this.ViewState["Expanded"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Expanded"]))
                {
                    this.ViewState["Expanded"] = value;
                    this.OnLanguageItemChanged();
                }
            }
        }

        private string _account;
        public string Account
        {
            get
            {
                return (string)this.ViewState["Account"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Account"]))
                {
                    this.ViewState["Account"] = value;
                    this.OnLanguageItemChanged();
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

        internal event EventHandler LanguageItemChanged;

        protected internal void OnLanguageItemChanged()
        {
            if (this.LanguageItemChanged != null)
            {
                this.LanguageItemChanged(this, EventArgs.Empty);
            }
        }

        protected internal PageOptionsButtonLanguageItem CloneAddThisButtonLanguageItem()
        {
            PageOptionsButtonLanguageItem newAddThisItem = this.CreateAddThisLanguageItem();
            this.CopyProperties(newAddThisItem);
            return newAddThisItem;
        }

        protected void CopyProperties(PageOptionsButtonLanguageItem newAddThisItems)
        {
            ((PageOptionsButtonLanguageItem)newAddThisItems).ButtonsCollection = this.ButtonsCollection;
            ((PageOptionsButtonLanguageItem)newAddThisItems).Language = this.Language;
        }

        protected PageOptionsButtonLanguageItem CreateAddThisLanguageItem()
        {
            return new PageOptionsButtonLanguageItem();
        }

        protected void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] objArray = (object[])savedState;
                //check to see that viewstate is an array with 2 elements
                if (objArray[0] != null)
                    ((IStateManager)this.ViewState).LoadViewState(((object[])savedState)[0]);

                if ((objArray.Length > 1) && (objArray[1] != null))
                {
                    ((IStateManager)ButtonsCollection).LoadViewState(objArray[1]);
                }
            }
        }

        protected object SaveViewState()
        {
            object obj1 = ((IStateManager)this.ViewState).SaveViewState();
            object obj2 = ((IStateManager)this.ButtonsCollection).SaveViewState();

            if (obj1 == null)
            {
                return null;
            }
            return new object[] { obj1, obj2 }; 

        }

        protected void TrackViewState()
        {
            this._trackViewState = true;
            ((IStateManager)this.ViewState).TrackViewState();

            if (ButtonsCollection != null)
            {
                ((IStateManager)ButtonsCollection).TrackViewState();
            }
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
                    this.OnLanguageItemChanged();
                }
            }
        }

        internal void SetDirty()
        {
            this._statebag.SetDirty(true);

            //If there are any sub IStateManager items, set them dirty as well.
            if (_buttonsCollection != null)
            {
                _buttonsCollection.SetDirty();
            }
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
