﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NCI.Services.Dictionary.BusinessObjects
{
    public class RelatedItems
    {
        /// <summary>
        /// Possibly empty list of relatedTerm structures
        /// </summary>
        public RelatedTerm[] Term { get; set; }

        /// <summary>
        /// Possibly empty list of externalLink structures
        /// </summary>
        public RelatedExternalLink[] External { get; set; }

        /// <summary>
        /// Possibly empty list of summaryRef structures
        /// </summary>
        public RelatedSummary[] Summary { get; set; }

        /// <summary>
        /// Possibly empty list of drugSummaryRef structures
        /// </summary>
        public RelatedDrugSummary[] DrugSummary { get; set; }
    }
}
