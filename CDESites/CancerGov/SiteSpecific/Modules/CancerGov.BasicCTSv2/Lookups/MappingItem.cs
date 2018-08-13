using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic.v2.Lookups
{
    public class MappingItem
    {
        public List<string> Codes { get; set; }

        public string Text { get; set; }

        public bool IsOverride { get; set; }
    }
}
