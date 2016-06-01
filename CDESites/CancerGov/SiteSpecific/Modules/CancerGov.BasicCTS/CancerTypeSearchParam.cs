using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents the Parameters of a Cancer Type Search
    /// </summary>
    public class CancerTypeSearchParam : BaseCTSSearchParam
    {
        public override Nest.SearchDescriptor<T> ModifySearchParams<T>(Nest.SearchDescriptor<T> descriptor)
        {
            return base.ModifySearchParams<T>(descriptor);
        }
    }
}
