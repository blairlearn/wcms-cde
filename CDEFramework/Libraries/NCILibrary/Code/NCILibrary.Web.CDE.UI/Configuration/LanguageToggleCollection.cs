using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls
{
    public class LanguageToggleCollection : StateManagedCollection
    {
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(LanguageToggle),
        };

        public event EventHandler LangItemCollectionChanged;

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
                foreach (LanguageToggle langItem in this)
                {
                    if (langItem.Name.Equals(index))
                    {
                        return langItem;
                    }
                }
                return null;
            }
        }

        public void Add(LanguageToggle langItem)
        {
            ((IList)this).Add(langItem);
        }

        public LanguageToggleCollection CloneFields()
        {
            LanguageToggleCollection langItems = new LanguageToggleCollection();
            foreach (LanguageToggle langItem in this)
            {
                langItems.Add(langItem.CloneLanguageToggle());
            }
            return langItems;
        }

        public bool Contains(LanguageToggle langItem)
        {
            return ((IList)this).Contains(langItem);
        }

        public void CopyTo(LanguageToggle[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(LanguageToggle langItem)
        {
            return ((IList)this).IndexOf(langItem);
        }

        public void Insert(int index, LanguageToggle langItem)
        {
            ((IList)this).Insert(index, langItem);
        }

        public void Removed(LanguageToggle langItem)
        {
            ((IList)this).Remove(langItem);
        }

        public void RemoveAt(int index)
        {
            ((IList)this).RemoveAt(index);
        }

        protected override void OnClearComplete()
        {
            this.OnLangItemCollectionChanged();
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
            LanguageToggle langItem = value as LanguageToggle;
            if (langItem != null)
            {
                langItem.LangItemChanged += new EventHandler(this.LangItemCollectionChanged);
            }

            this.OnLangItemCollectionChanged();
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

        private void OnLangItemCollectionChanged(object sender, EventArgs e)
        {
            this.OnLangItemCollectionChanged();
        }

        private void OnLangItemCollectionChanged()
        {
            if (this.LangItemCollectionChanged != null)
            {
                this.LangItemCollectionChanged(this, EventArgs.Empty);
            }
        }

    }
}
