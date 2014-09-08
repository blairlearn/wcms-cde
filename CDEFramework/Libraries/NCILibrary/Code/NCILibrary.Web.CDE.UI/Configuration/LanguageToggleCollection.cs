using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Globalization;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.Configuration
{

    public class LanguageToggleCollection : StateManagedCollection
    {
        //These are the types of AddThis items that can be in this collection.
        //If there needs to be a new type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(LanguageToggle)
        };

        public event EventHandler LanguageToggleCollectionChanged;

        [Browsable(false)]
        public LanguageToggle this[int index]
        {
            get
            {
                return (this[index] as LanguageToggle);
            }
        }

        [Browsable(false)]
        public LanguageToggle this[String index]
        {
            get
            {
                foreach (LanguageToggle langToggle in this)
                {
                    return langToggle;
                }
                return null;
            }
        }

        [Browsable(false)]
        public LanguageToggle this[CultureInfo culture]
        {
            get
            {
                foreach (LanguageToggle langToggle in this)
                {
                    return langToggle;
                }
                return null;
            }
        }

        public void Add(LanguageToggle langToggle)
        {
            ((IList)this).Add(langToggle);
        }

        public LanguageToggleCollection CloneFields()
        {
            LanguageToggleCollection langTogglesItems = new LanguageToggleCollection();
            foreach (LanguageToggle langToggle in this)
            {
                langTogglesItems.Add(langToggle.CloneLanguageToggle());
            }
            return langTogglesItems;
        }

        public bool Contains(LanguageToggle langToggle)
        {
            return ((IList)this).Contains(langToggle);
        }

        public void CopyTo(LanguageToggle[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(LanguageToggle langToggle)
        {
            return ((IList)this).IndexOf(langToggle);
        }

        public void Insert(int index, LanguageToggle langToggle)
        {
            ((IList)this).Insert(index, langToggle);
        }

        public void Removed(LanguageToggle langToggle)
        {
            ((IList)this).Remove(langToggle);
        }

        public void RemoveAt(int index)
        {
            ((IList)this).RemoveAt(index);
        }

        protected override void OnClearComplete()
        {
            this.OnLanguageToggleCollectionChanged();
        }

        protected override object CreateKnownType(int index)
        {
            switch (index)
            {
                case 0:
                    return new LanguageToggle();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            LanguageToggle langToggle = value as LanguageToggle;
            if (langToggle != null)
            {
                langToggle.LangToggleChanged += new EventHandler(this.LanguageToggleCollectionChanged);
            }

            this.OnLanguageToggleCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is LanguageToggle))
            {
                throw new ArgumentException("Invalid type");
            }
        }

        protected override Type[] GetKnownTypes()
        {
            return knownTypes;
        }

        protected override void SetDirtyObject(object b)
        {
            ((LanguageToggleCollection)b).SetDirty();
        }

        private void OnLanguageToggleCollectionChanged(object sender, EventArgs e)
        {
            this.OnLanguageToggleCollectionChanged();
        }

        private void OnLanguageToggleCollectionChanged()
        {
            if (this.LanguageToggleCollectionChanged != null)
            {
                this.LanguageToggleCollectionChanged(this, EventArgs.Empty);
            }
        }

    }
}
