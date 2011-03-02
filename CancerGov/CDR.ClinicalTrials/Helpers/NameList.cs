using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.ClinicalTrials.Helpers
{
    /// <summary>
    /// Encapsulates common methods for manipulating a list of names.
    /// </summary>
    public class NameList : List<string>
    {
        /// <summary>
        /// Creates an empty NameList.
        /// </summary>
        public NameList()
            : base()
        {
        }

        /// <summary>
        /// Creates a NameList and populates it with values copied
        /// from the specified NameList object.
        /// </summary>
        /// <param name="list">A NameList object to copy into the new NameList.</param>
        public NameList(NameList list)
            : base(list)
        {
        }

        /// <summary>
        /// Creates a NameList and populates it with values copied
        /// from the specified IList.<string>.
        /// </summary>
        /// <param name="list">IList<string> to copy into the new NameList.</param>
        public NameList(IList<string> list)
            : base(list)
        {
        }

        /// <summary>
        /// Creates a NameList and populates it with name values copied
        /// from the specified KeyValuePair<string, int>.
        /// </summary>
        /// <param name="list">A list of KeyValuePair<string, int> objects to
        /// copy name values from when creating a new NameList.</param>
        public NameList(IList<KeyValuePair<string, int>> list)
            : base()
        {
            if (list != null)
            {
                foreach (KeyValuePair<string, int> item in list)
                {
                    base.Add(item.Key);
                }
            }
        }

        /// <summary>
        /// Creates a NameList with the specified initial capacity.
        /// </summary>
        /// <param name="capacity"></param>
        public NameList(Int32 capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Overridden.  Renders the list of names as a comma-separated list.
        /// </summary>
        /// <returns>A comma-separated list.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            int length = Count;

            for (int i = 0; i < length; i++)
            {
                if (i > 0)
                    sb.AppendFormat(", {0}", this[i]);
                else
                    sb.AppendFormat(this[i]);   // First pass only.
            }

            return sb.ToString();
        }
    }
}
