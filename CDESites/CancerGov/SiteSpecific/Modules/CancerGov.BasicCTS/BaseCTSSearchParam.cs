using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents the shared parameters for any Basic CTS Search
    /// </summary>
    public abstract class BaseCTSSearchParam
    {
        /// <summary>
        /// Gets/Sets the position to start results from
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Gets/Sets the Number of Records to Fetch
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// ZipCode to use for filtering.  NOTE: This may be some sort of LocationFilter object if we need more than Zip.
        /// </summary>
        public string ZipCode { get; set; }
        

        //We may need some sorting options here too...

        /// <summary>
        /// Gets the body for the ElasticSearch Search/SearchTemplate request.
        /// </summary>
        /// <returns>A JSON string to be used in the Search/SearchTemplate request</returns>
        public abstract object GetBody();
    }
}
