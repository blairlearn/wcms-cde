using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls.AddThis
{
    public class AddThisLanguageCollection : StateManagedCollection
    {
        //These are the types of AddThis items that can be in this collection.
        //If there needs to be a new type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(AddThisButtonCollection)
        };

        public event EventHandler LanguageCollectionChanged;

        [Browsable(false)]
        public AddThisButtonCollection this[int index]
        {
            get
            {
                return (this[index] as AddThisButtonCollection);
            }
        }

        public void Add(AddThisButtonCollection languageItem)
        {
            ((IList)this).Add(languageItem);
        }

        public AddThisLanguageCollection CloneFields()
        {
            AddThisLanguageCollection languageItems = new AddThisLanguageCollection();
            foreach (AddThisButtonCollection buttonCollection in this)
            {
                languageItems.Add(buttonCollection.CloneAddThisButtonItems());
            }
            return languageItems;
        }

        public bool Contains(AddThisButtonCollection buttonItem)
        {
            return ((IList)this).Contains(buttonItem);
        }

        public void CopyTo(AddThisButtonCollection[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(AddThisButtonCollection buttonItem)
        {
            return ((IList)this).IndexOf(buttonItem);
        }

        public void Insert(int index, AddThisButtonCollection buttonItem)
        {
            ((IList)this).Insert(index, buttonItem);
        }

        public void Removed(AddThisButtonCollection buttonItem)
        {
            ((IList)this).Remove(buttonItem);
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
                    return new AddThisButtonCollection();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            AddThisButtonCollection buttonItem = value as AddThisButtonCollection;
            if (buttonItem != null)
            {
                buttonItem.ItemsChanged += new EventHandler(this.LanguageCollectionChanged);
            }

            this.OnLanguageCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is AddThisButtonCollection))
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
            ((AddThisButtonCollection)b).SetDirty();
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
