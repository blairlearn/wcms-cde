using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Services.Dictionary.BusinessObjects;


namespace NCI.Services.Dictionary
{
    /// <summary>
    /// Provides support for converting the results of a Dictionary Service
    /// method to a JSON string.
    /// 
    /// Individual classes in the dictionary return type hierarchy implment the
    /// IJsonizable with a Jsonize() method which passes the class' data members to
    /// an instance of Jsonizer.
    /// </summary>
    public class Jsonizer
    {
        // Used by the individual AddMember methods to store the values making
        // up the data structure.
        private StringBuilder builder = new StringBuilder();

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="rootItem">The object hierarchy's root item.</param>
        internal Jsonizer(IJsonizable rootItem)
        {
            builder.Append("{");
            rootItem.Jsonize(this);
            builder.Append("}");
        }

        /// <summary>
        /// Creates a string containing a JSON representation of the object hierarchy.
        /// </summary>
        /// <returns></returns>
        internal string ToJsonString()
        {
            return builder.ToString();
        }

        /// <summary>
        /// Called by an implementation of IJsonizable to store a data member which implements IJsonizable.
        /// </summary>
        /// <param name="name">The object member's name</param>
        /// <param name="item">The object to store</param>
        /// <param name="isFinal">Is this the last member the caller will add?</param>
        internal void AddMember(string name, IJsonizable item, bool isFinal)
        {
            Jsonizer subJsonizer = new Jsonizer(item);

            builder.AppendFormat("\"{0}\": {1}", name, subJsonizer.ToJsonString());
            if (!isFinal)
                builder.Append(",");
        }

        /// <summary>
        /// Called by an implementation of IJsonizable to store a data member containing an array
        /// of objects, each of which implements IJsonizable.
        /// </summary>
        /// <param name="name">The object member's name</param>
        /// <param name="item">The object to store</param>
        /// <param name="isFinal">Is this the last member the caller will add?</param>
        internal void AddMember(string name, IJsonizable[] itemArray, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": ", name);

            List<Jsonizer> subJsonizers = new List<Jsonizer>(itemArray.Length);
            foreach (IJsonizable item in itemArray)
            {
                Jsonizer subItem = new Jsonizer(item);
                subJsonizers.Add(subItem);
            }

            // Output a possibly empty array of IJsonizable objects.
            int length = subJsonizers.Count;
            int lastIndex = length - 1;
            builder.Append("[");
            for (int i = 0; i < itemArray.Length; i++)
            {
                builder.Append(subJsonizers[i].ToJsonString());
                if (i != lastIndex)
                    builder.Append(",");
            }
            builder.Append("]");

            if (!isFinal)
                builder.Append(",");
        }

        /// <summary>
        /// Called by an implementation of IJsonizable to store a data member of type string.
        /// </summary>
        /// <param name="name">The object member's name</param>
        /// <param name="item">The object to store</param>
        /// <param name="isFinal">Is this the last member the caller will add?</param>
        internal void AddMember(string name, String item, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": \"{1}\"", name, item);
            if (!isFinal)
                builder.Append(",");
        }

        /// <summary>
        /// Called by an implementation of IJsonizable to store a data member of type string.
        /// </summary>
        /// <param name="name">The object member's name</param>
        /// <param name="item">The object to store</param>
        /// <param name="isFinal">Is this the last member the caller will add?</param>
        internal void AddMember(string name, String[] item, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": ", name);

            // Output a possibly empty array of strings.
            int length = item.Length;
            int lastIndex = length - 1;
            builder.Append("[");
            for (int i = 0; i < length; i++)
            {
                builder.AppendFormat("\"{0}\"", item[i]);
                if(i !=lastIndex)
                    builder.Append(",");
            }
            builder.Append("]");

            if (!isFinal)
                builder.Append(",");
        }

        /// <summary>
        /// Called by an implementation of IJsonizable to store a data member which is a JSON string.
        /// </summary>
        /// <param name="name">The object member's name</param>
        /// <param name="item">The object to store. <remarks>item is assumed to be a
        /// syntactically correct, fully-formed JSON object.</remarks></param>
        /// <param name="isFinal">Is this the last member the caller will add?</param>
        internal void AddJsonString(string name, String item, bool isFinal)
        {
            // If item is empty, there is no structure to output. In this case,
            // output the literal "null" to note the absence of any value.
            // Otherwise, this would output invalid JSON.
            if(String.IsNullOrEmpty(item))
                item = "null";

            builder.AppendFormat("\"{0}\": {1}", name, item);
            if (!isFinal)
                builder.Append(",");
        }

        /// <summary>
        /// Called by an implementation of IJsonizable to store an integer data member.
        /// </summary>
        /// <param name="name">The object member's name</param>
        /// <param name="item">The object to store</param>
        /// <param name="isFinal">Is this the last member the caller will add?</param>
        internal void AddMember(string name, int item, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": {1}", name, item);
            if (!isFinal)
                builder.Append(",");
        }
    }
}
