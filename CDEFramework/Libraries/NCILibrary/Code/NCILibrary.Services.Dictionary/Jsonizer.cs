using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Services.Dictionary.BusinessObjects;


namespace NCI.Services.Dictionary
{
    public class Jsonizer
    {
        private StringBuilder builder = new StringBuilder();

        internal Jsonizer(IJsonizable rootItem)
        {
            builder.Append("{");
            rootItem.Jsonize(this);
            builder.Append("}");
        }

        internal string ToJsonString()
        {
            return builder.ToString();
        }

        internal void AddMember(string name, IJsonizable item, bool isFinal)
        {
            Jsonizer subJsonizer = new Jsonizer(item);

            builder.AppendFormat("\"{0}\": {1}", name, subJsonizer.ToJsonString());
            if (!isFinal)
                builder.Append(",");
        }

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

        internal void AddMember(string name, String item, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": \"{1}\"", name, item);
            if (!isFinal)
                builder.Append(",");
        }

        internal void AddJsonString(string name, String item, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": {{{1}}}", name, item);
            if (!isFinal)
                builder.Append(",");
        }

        internal void AddMember(string name, int item, bool isFinal)
        {
            builder.AppendFormat("\"{0}\": {1}", name, item);
            if (!isFinal)
                builder.Append(",");
        }
    }
}
