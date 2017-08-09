using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class TrialTermLookupService: ITerminologyLookupService
    {
        private TrialTermLookupConfig _config = null;

        public TrialTermLookupService(TrialTermLookupConfig config)
        {
            _config = config;
            LoadMappings();
        }

        /// <summary>
        /// Build Dictionaries Here.
        /// </summary>
        private void LoadMappings()
        {
            //BIG ASSUMPTION: A Ccode will only ever exist in one Label definition.  (e.g. C1234, C6789 | Label)
        }

        #region ITerminologyLookupService Members

        public string GetTitleCase(string value)
        {
            //Break up value on comma
            //find any entry with that value or all of them.

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets lower case name.
        /// <remarks>This does not lower case, but returns TitleCase</remarks>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string Get(string value)
        {
            return GetTitleCase(value);            
        }

        public bool MappingContainsKey(string key)
        {
            //Break up value on comma
            //find any entry with that value or all of them.

            //Break 
            throw new NotImplementedException();
        }

        #endregion
    }
}
