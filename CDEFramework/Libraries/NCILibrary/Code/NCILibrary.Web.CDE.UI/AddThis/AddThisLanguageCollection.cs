using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls
{
    public class AddThisLanguageCollection : StateManagedCollection
    {
        //These are the types of AddThis items that can be in this collection.
        //If there needs to be a new type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(AddThisButtonLanguageItem)
        };

        public event EventHandler LanguageCollectionChanged;

        [Browsable(false)]
        public AddThisButtonLanguageItem this[int index]
        {
            get
            {
                return (this[index] as AddThisButtonLanguageItem);
            }
        }

        [Browsable(false)]
        public AddThisButtonLanguageItem this[String index]
        {
            get
            {
                foreach (AddThisButtonLanguageItem languageItem in this)
                {
                    if (languageItem.Language.Equals(index))
                    {
                        return languageItem;
                    }
                }
                return null;
            }
        }

        public void Add(AddThisButtonLanguageItem languageItem)
        {
            ((IList)this).Add(languageItem);
        }

        public AddThisLanguageCollection CloneFields()
        {
            AddThisLanguageCollection languageItems = new AddThisLanguageCollection();
            foreach (AddThisButtonLanguageItem languageItem in this)
            {
                languageItems.Add(languageItem);
            }
            return languageItems;
        }

        public bool Contains(AddThisButtonLanguageItem languageItem)
        {
            return ((IList)this).Contains(languageItem);
        }

        public void CopyTo(AddThisButtonLanguageItem[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(AddThisButtonLanguageItem languageItem)
        {
            return ((IList)this).IndexOf(languageItem);
        }

        public void Insert(int index, AddThisButtonLanguageItem languageItem)
        {
            ((IList)this).Insert(index, languageItem);
        }

        public void Removed(AddThisButtonLanguageItem languageItem)
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
                case 0 :
                    return new AddThisButtonLanguageItem();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            AddThisButtonLanguageItem buttonItem = value as AddThisButtonLanguageItem;
            if (buttonItem != null)
            {
                buttonItem.LanguageItemChanged += new EventHandler(this.LanguageCollectionChanged);
            }

            this.OnLanguageCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is AddThisButtonLanguageItem))
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
            ((AddThisButtonLanguageItem)b).SetDirty();
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
