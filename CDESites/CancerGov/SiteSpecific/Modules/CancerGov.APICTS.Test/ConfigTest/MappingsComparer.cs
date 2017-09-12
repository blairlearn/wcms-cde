using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test.ConfigTest
{
    public class MappingsComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            // If the items are both null, or if one or the other is null, return 
            // the correct response right away.
            if (x == null && y == null)
            {
                return true;
            }
            else if (x == null || y == null)
            {
                return false;
            }

            // This compares the strings.
            bool isEqual = x == y;

            return isEqual;
        }

        /// <summary>
        /// Return a hash code for a specifed object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(string obj)
        {
            int hash = 0;
            hash ^= obj.GetHashCode();

            return hash;
        }
    }
}
