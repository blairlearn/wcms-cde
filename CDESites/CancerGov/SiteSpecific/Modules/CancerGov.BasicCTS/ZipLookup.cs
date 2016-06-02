using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nest;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents a ZIP Code Lookup from ElasticSearch
    /// </summary>
    [ElasticType(IdProperty = "PostalCode_ZIP")]
    public class ZipLookup
    {
        /// <summary>
        /// Gets and Sets the Zip Code
        /// </summary>
        [ElasticProperty(Name = "_id", Index = FieldIndexOption.NotAnalyzed, Type = FieldType.String)]
        public string PostalCode_ZIP { get; set; }

        /// <summary>
        /// Gets and Sets the GeoCode for that Zip Code
        /// </summary>
        public GeoLocation GeoCode { get; set; }
    }
}
