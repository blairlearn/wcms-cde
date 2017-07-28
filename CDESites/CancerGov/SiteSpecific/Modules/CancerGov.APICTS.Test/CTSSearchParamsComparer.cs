using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Test
{


    /// <summary>
    /// Comparer for a CTSSearchParams class.
    /// </summary>
    public class CTSSearchParamsComparer : IEqualityComparer<CTSSearchParams>
    {

        private TerminologyFieldSearchParamComparer _termComp = new TerminologyFieldSearchParamComparer();
        private LabelledSearchParamComparer _labelledParamComp = new LabelledSearchParamComparer();

        #region IEqualityComparer<CTSSearchParams> Members
         
        public bool Equals(CTSSearchParams x, CTSSearchParams y)
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

            //This should compare every single property.
            bool isEqual =
                _termComp.Equals(x.MainType, y.MainType) &&
                this.AreTermParamListsEqual(x.SubTypes, y.SubTypes) &&
                this.AreTermParamListsEqual(x.Stages, y.Stages) &&
                this.AreTermParamListsEqual(x.Findings, y.Findings) &&
                _labelledParamComp.Equals(x.State, y.State) &&
                //this.AreLabelledParamListsEqual(x.TrialPhases, y.TrialPhases) &&
                x.Age == y.Age &&
                x.Phrase == y.Phrase && // Keyword
                x.City == y.City &&
                x.Country == y.Country &&
                x.Hospital == y.Hospital &&
                x.Investigator == y.Investigator &&
                x.LeadOrg == y.LeadOrg; 

            //ADD A FIELD TO SearchParams, NEED to add here.

            return isEqual;
        }

        /// <summary>
        /// Helper function to determine if two synonym lists are equal, order does not matter.
        /// </summary>
        /// <param name="x">Synonym list 1</param>
        /// <param name="y">Synonym list 2</param>
        /// <returns></returns>
        private bool AreTermParamListsEqual(TerminologyFieldSearchParam[] x, TerminologyFieldSearchParam[] y)
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

            //Generate a set of those values that are not in both lists.
            //if this is not 0, then there is an error.
            var diffxy = x.Except(y, new TerminologyFieldSearchParamComparer());

            return diffxy.Count() == 0;
        }

        /// <summary>
        /// Helper function to determine if two labelled objects are equal.
        /// </summary>
        /// <param name="x">Labelled object 1</param>
        /// <param name="y">Labelled object 2</param>
        /// <returns></returns>
        private bool AreLabelledParamListsEqual(LabelledSearchParam[] x, LabelledSearchParam[] y)
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

            //Generate a set of those values that are not in both lists.
            //if this is not 0, then there is an error.
            var diffxy = x.Except(y, new LabelledSearchParamComparer());

            return diffxy.Count() == 0;
        }

        public int GetHashCode(CTSSearchParams obj)
        {
            int hash = 0;
            hash ^= _termComp.GetHashCode(obj.MainType);
            hash ^= obj.SubTypes.GetHashCode();
            hash ^= obj.Phrase.GetHashCode();
            hash ^= obj.Country.GetHashCode();
            hash ^= obj.City.GetHashCode();
            hash ^= obj.State.GetHashCode();
            hash ^= obj.Hospital.GetHashCode();
            hash ^= obj.Investigator.GetHashCode();
            hash ^= obj.LeadOrg.GetHashCode();

            //ADD A FIELD TO SearchParams, NEED to add here.

            return hash;
        }



        #endregion
    }
}
