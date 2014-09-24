using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls
{
    public class LanguageToggleLanguageCollection : StateManagedCollection
    {
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(LanguageToggleLanguageItem)
        };

        public event EventHandler LanguageCollectionChanged;

        [Browsable(false)]
        public LanguageToggleLanguageItem this[int index]
        {
            get
            {
                return (this[index] as LanguageToggleLanguageItem);
            }
        }

        [Browsable(false)]
        public LanguageToggleLanguageItem this[String index]
        {
            get
            {
                foreach (LanguageToggleLanguageItem languageItem in this)
                {
                    if (languageItem.Language.Equals(index))
                    {
                        return languageItem;
                    }
                }
                return null;
            }
        }

        public void Add(LanguageToggleLanguageItem languageItem)
        {
            ((IList)this).Add(languageItem);
        }

        public LanguageToggleLanguageCollection CloneFields()
        {
            LanguageToggleLanguageCollection languageItems = new LanguageToggleLanguageCollection();
            foreach (LanguageToggleLanguageItem languageItem in this)
            {
                languageItems.Add(languageItem);
            }
            return languageItems;
        }

        public bool Contains(LanguageToggleLanguageItem languageItem)
        {
            return ((IList)this).Contains(languageItem);
        }

        public void CopyTo(LanguageToggleLanguageItem[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(LanguageToggleLanguageItem languageItem)
        {
            return ((IList)this).IndexOf(languageItem);
        }

        public void Insert(int index, LanguageToggleLanguageItem languageItem)
        {
            ((IList)this).Insert(index, languageItem);
        }

        public void Removed(LanguageToggleLanguageItem languageItem)
        {
            ((IList)this).Remove(languageItem);
        }

        public void RemoveAt(int index)
        {
            ((IList)this).RemoveAt(index);
        }

        protected override void OnClearComplete()
        {
            this.OnLanguageCollectionChanged();
        }

        protected override object CreateKnownType(int index)
        {
            switch (index)
            {
                case 0:
                    return new LanguageToggleLanguageItem();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            LanguageToggleLanguageItem langItem = value as LanguageToggleLanguageItem;
            if (langItem != null)
            {
                langItem.LanguageItemChanged += new EventHandler(this.LanguageCollectionChanged);
            }

            this.OnLanguageCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is LanguageToggleLanguageItem))
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
            ((LanguageToggleLanguageItem)b).SetDirty();
        }

        private void OnLanguageCollectionChanged(object sender, EventArgs e)
        {
            this.OnLanguageCollectionChanged();
        }

        private void OnLanguageCollectionChanged()
        {
            if (this.LanguageCollectionChanged != null)
            {
                this.LanguageCollectionChanged(this, EventArgs.Empty);
            }
        }

    }
}
