using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class SuggestReturnMeta : MetaCommon
    {
        /// <summary>
        /// The total number of terms matching the request
        /// </summary>
        public int ResultCount { get; set; }
    }
}
