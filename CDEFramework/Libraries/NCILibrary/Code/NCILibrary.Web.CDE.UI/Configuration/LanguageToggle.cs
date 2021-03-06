﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace NCI.Web.CDE.UI.WebControls
{
    public class LanguageToggle : IStateManager
    {
        private string _name; // Name of translation item in English
        private string _locale; // 4-character country-culture code
        private string _url; // Default URL for translation item
        private string _class; // CSS class for language toggle
        private string _title; // Name of translation item in native language
        private string _onclick; // Analytics JS input
        private bool _trackViewState = false;

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
                    this.OnLangItemChanged();
                }
            }
        }

        public string Locale
        {
            get
            {
                return (string)this.ViewState["Locale"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Locale"]))
                {
                    this.ViewState["Locale"] = value;
                    this.OnLangItemChanged();
                }
            }
        }

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
                    this.OnLangItemChanged();
                }
            }
        }

        public string Class
        {
            get
            {
                return (string)this.ViewState["Class"] ?? string.Empty;
            }
            set
            {
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Class"]))
                {
                    this.ViewState["Class"] = value;
                    this.OnLangItemChanged();
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
                //If the text is the same, just ignore and don't set dirtyness
                //or fire any events.
                if (!object.Equals(value, this.ViewState["Title"]))
                {
                    this.ViewState["Title"] = value;
                    this.OnLangItemChanged();
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

        internal event EventHandler LangItemChanged;

        protected internal void OnLangItemChanged()
        {
            if (this.LangItemChanged != null)
            {
                this.LangItemChanged(this, EventArgs.Empty);
            }
        }

        protected internal LanguageToggle CloneLanguageToggle()
        {
            LanguageToggle newLanguageToggle = this.CreateLanguageToggle();
            this.CopyProperties(newLanguageToggle);
            return newLanguageToggle;
        }

        protected void CopyProperties(LanguageToggle newLanguageToggle)
        {
            ((LanguageToggle)newLanguageToggle).Name = this.Name;
        }

        protected LanguageToggle CreateLanguageToggle()
        {
            return new LanguageToggle();
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
                    this.OnLangItemChanged();
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
