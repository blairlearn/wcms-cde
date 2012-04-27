using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.ComponentModel;

namespace NCI.Web.CDE.UI.WebControls
{
    public class AddThisButtonCollection : StateManagedCollection
    {
        //These are the types of AddThis items that can be in this collection.
        //If there needs to be a new type, then add a class and then
        //add it to this list.
        private static readonly Type[] knownTypes = new Type[] { 
            typeof(AddThisButtonItem),
            typeof(CustomAddThisButtonItem),
            typeof(GoogleAddThisButtonItem)
        };

        public event EventHandler ButtonItemCollectionChanged;

        [Browsable(false)]
        public AddThisButtonItem this[int index]
        {
            get
            {
                return (this[index] as AddThisButtonItem);
            }
        }

        [Browsable(false)]
        public AddThisButtonItem this[String index]
        {
            get
            {
                foreach(AddThisButtonItem buttonItem in this){
                    if (buttonItem.Service.Equals(index))
                    {
                        return buttonItem;
                    }
                }
                return null;
            }
        }

        public void Add(AddThisButtonItem buttonItem)
        {
            ((IList)this).Add(buttonItem);
        }

        public AddThisButtonCollection CloneFields()
        {
            AddThisButtonCollection buttonItems = new AddThisButtonCollection();
            foreach (AddThisButtonItem buttonItem in this)
            {
                buttonItems.Add(buttonItem.CloneAddThisButtonItem());
            }
            return buttonItems;
        }

        public bool Contains(AddThisButtonItem buttonItem)
        {
            return ((IList)this).Contains(buttonItem);
        }

        public void CopyTo(AddThisButtonItem[] array, int index)
        {
            this.CopyTo(array, index);
        }

        public int IndexOf(AddThisButtonItem buttonItem)
        {
            return ((IList)this).IndexOf(buttonItem);
        }

        public void Insert(int index, AddThisButtonItem buttonItem)
        {
            ((IList)this).Insert(index, buttonItem);
        }

        public void Removed(AddThisButtonItem buttonItem)
        {
            ((IList)this).Remove(buttonItem);
        }

        public void RemoveAt(int index)
        {
            ((IList)this).RemoveAt(index);
        }

        protected override void OnClearComplete()
        {
            this.OnButtonItemCollectionChanged();
        }

        protected override object CreateKnownType(int index)
        {
            switch (index)
            {
                case 0:
                    return new AddThisButtonItem();
                case 1:
                    return new CustomAddThisButtonItem();
                case 2:
                    return new GoogleAddThisButtonItem();
            }

            throw new ArgumentOutOfRangeException("Incorrect type");
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            AddThisButtonItem buttonItem = value as AddThisButtonItem;
            if (buttonItem != null)
            {
                buttonItem.ButtonItemChanged += new EventHandler(this.ButtonItemCollectionChanged);
            }

            this.OnButtonItemCollectionChanged();
        }

        protected override void OnValidate(object value)
        {
            base.OnValidate(value);
            if (!(value is AddThisButtonItem))
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

        private void OnButtonItemCollectionChanged(object sender, EventArgs e)
        {
            this.OnButtonItemCollectionChanged();
        }

        private void OnButtonItemCollectionChanged()
        {
            if (this.ButtonItemCollectionChanged != null)
            {
                this.ButtonItemCollectionChanged(this, EventArgs.Empty);
            }
        }

    }
}
