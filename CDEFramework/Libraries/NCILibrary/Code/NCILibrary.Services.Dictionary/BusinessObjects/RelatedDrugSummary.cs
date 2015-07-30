﻿using System;
using System.Runtime.Serialization;

namespace NCI.Services.Dictionary.BusinessObjects
{
    /// <summary>
    /// Represents a link to a Drug Info Summary
    /// </summary>
    [DataContract()]
    public class RelatedDrugSummary
    {
        /// <summary>
        /// the summary link
        /// </summary>
        [DataMember(Name = "id")]
        public int ID { get; set; }

        /// <summary>
        /// link text
        /// </summary>
        [DataMember(Name = "text")]
        public String Text { get; set; }
    }
}
