using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public class CTSSearchParams : BaseCTSSearchParam
    {
        public String AgeOfEligibility { get; set; }
        public String ZipCode { get; set; }
        public String CancerType { get; set; }
        public GeoLocation GeoCode { get; set; }
    }
}
