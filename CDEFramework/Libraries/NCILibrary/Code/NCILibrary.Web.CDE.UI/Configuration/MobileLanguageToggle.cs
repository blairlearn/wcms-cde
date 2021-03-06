﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.Configuration
{
    public class MobileLanguageToggle : IStateManager
    {

        private bool _trackViewState = false;

        private string _language;
        private string _template;

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
                    this.OnLangToggleChanged();
                }
            }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public string Template
        {
            get
            {
                return (string)this.ViewState["Template"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Template"]))
                {
                    this.ViewState["Template"] = value;
                    this.OnLangToggleChanged();
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

        internal event EventHandler LangToggleChanged;

        protected internal void OnLangToggleChanged()
        {
            if (this.LangToggleChanged != null)
            {
                this.LangToggleChanged(this, EventArgs.Empty);
            }
        }

        protected internal MobileLanguageToggle CloneMobileLanguageToggle()
        {
            MobileLanguageToggle newLangToggle = this.CreateMobileLanguageToggle();
            this.CopyProperties(newLangToggle);
            return newLangToggle;
        }

        protected void CopyProperties(MobileLanguageToggle newLangToggle)
        {
            ((MobileLanguageToggle)newLangToggle).Language = this.Language;
            ((MobileLanguageToggle)newLangToggle).Template = this.Template;
        }

        protected MobileLanguageToggle CreateMobileLanguageToggle()
        {
            return new MobileLanguageToggle();
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
                    this.OnLangToggleChanged();
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
