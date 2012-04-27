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

    public class MobileLanguageToggleCollection : StateManagedCollection
    {
        //These are the types of AddThis items that can be in this collection.
        //If there needs to be a new type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(MobileLanguageToggle)
        };

        public event EventHandler LanguageToggleCollectionChanged;

        [Browsable(false)]
        public MobileLanguageToggle this[int index]
        {
            get
            {
                return (this[index] as MobileLanguageToggle);
            }
        }

        [Browsable(false)]
        public MobileLanguageToggle this[String index]
        {
            get
            {
                foreach (MobileLanguageToggle langToggle in this)
                {
                    if (langToggle.Language.Equals(index))
                    {
                        return langToggle;
                    }
                }
                return null;
            }
        }

        [Browsable(false)]
        public MobileLanguageToggle this[CultureInfo culture]
        {
            get
            {
                foreach (MobileLanguageToggle langToggle in this)
                {
                    if (langToggle.Language.Equals(culture.TwoLetterISOLanguageName))
                    {
                        return langToggle;
                    }
                }
                return null;
            }
        }

        public void Add(MobileLanguageToggle langToggle)
        {
            ((IList)this).Add(langToggle);
        }

        public MobileLanguageToggleCollection CloneFields()
        {
            MobileLanguageToggleCollection langTogglesItems = new MobileLanguageToggleCollection();
            foreach (MobileLanguageToggle langToggle in this)
            {
                langTogglesItems.Add(langToggle.CloneMobileLanguageToggle());
            }
            return langTogglesItems;
        }

        public bool Contains(MobileLanguageToggle langToggle)
        {
            return ((IList)this).Contains(langToggle);
        }

        public void CopyTo(MobileLanguageToggle[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(MobileLanguageToggle langToggle)
        {
            return ((IList)this).IndexOf(langToggle);
        }

        public void Insert(int index, MobileLanguageToggle langToggle)
        {
            ((IList)this).Insert(index, langToggle);
        }

        public void Removed(MobileLanguageToggle langToggle)
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
                    return new MobileLanguageToggle();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            MobileLanguageToggle langToggle = value as MobileLanguageToggle;
            if (langToggle != null)
            {
                langToggle.LangToggleChanged += new EventHandler(this.LanguageToggleCollectionChanged);
            }

            this.OnLanguageToggleCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is MobileLanguageToggle))
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
            ((MobileLanguageToggleCollection)b).SetDirty();
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
