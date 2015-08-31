﻿using System.Runtime.Serialization;

namespace NCI.Web.Dictionary.BusinessObjects
{
    public class SuggestReturnMeta : MetaCommon
    {
        /// <summary>
        /// The total number of terms matching the request
        /// </summary>
        public int ResultCount { get; set; }
    }
}
