using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.BasicCTSv2
{
    public static class ClinicalTrialExtensions
    {
        public static string[] SecondaryIDs(this ClinicalTrial trial)
        {
            List<String> rtnIds = new List<String>();
            // logic goes here
            // if primary id == ctep id, don't include, etc

            // Add trials IDs in the listing of other_ids
            rtnIds.AddRange(
                from id in trial.OtherTrialIDs
                select id.Value 
            );
            return rtnIds.ToArray();
        }
    }
}
