using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls
{
    public class PageOptionButtonItemCollection : StateManagedCollection
    {
        //These are the types of AddThis items that can be in this collection.
        //If there needs to be a new type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(PageOptionsButtonLanguageItem)
        };

        public event EventHandler LanguageCollectionChanged;

        [Browsable(false)]
        public PageOptionsButtonLanguageItem this[int index]
        {
            get
            {
                return (this[index] as PageOptionsButtonLanguageItem);
            }
        }

        [Browsable(false)]
        public PageOptionsButtonLanguageItem this[String index]
        {
            get
            {
                foreach (PageOptionsButtonLanguageItem languageItem in this)
                {
                    if (languageItem.Language.Equals(index))
                    {
                        return languageItem;
                    }
                }
                return null;
            }
        }

        public void Add(PageOptionsButtonLanguageItem languageItem)
        {
            ((IList)this).Add(languageItem);
        }

        public PageOptionButtonItemCollection CloneFields()
        {
            PageOptionButtonItemCollection languageItems = new PageOptionButtonItemCollection();
            foreach (PageOptionsButtonLanguageItem languageItem in this)
            {
                languageItems.Add(languageItem);
            }
            return languageItems;
        }

        public bool Contains(PageOptionsButtonLanguageItem languageItem)
        {
            return ((IList)this).Contains(languageItem);
        }

        public void CopyTo(PageOptionsButtonLanguageItem[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(PageOptionsButtonLanguageItem languageItem)
        {
            return ((IList)this).IndexOf(languageItem);
        }

        public void Insert(int index, PageOptionsButtonLanguageItem languageItem)
        {
            ((IList)this).Insert(index, languageItem);
        }

        public void Removed(PageOptionsButtonLanguageItem languageItem)
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
                    return new PageOptionsButtonLanguageItem();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            PageOptionsButtonLanguageItem buttonItem = value as PageOptionsButtonLanguageItem;
            if (buttonItem != null)
            {
                buttonItem.LanguageItemChanged += new EventHandler(this.LanguageCollectionChanged);
            }

            this.OnLanguageCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is PageOptionsButtonLanguageItem))
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
            ((PageOptionsButtonLanguageItem)b).SetDirty();
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
