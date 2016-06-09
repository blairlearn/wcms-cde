using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents Basic Trial Information
    /// </summary>
    public abstract class TrialBase
    {
        public string CDRID { get; set; }
        public string OrgStudyID { get; set; }
        public string[] SecondaryIDs { get; set; }
        public string NCTID { get; set; }
        public string BriefTitle { get; set; }
        public string OfficialTitle { get; set; }
        public string CurrentProtocolStatus { get; set; }
        public string[] ProtocolPhases { get; set; }
        public string[] StudyCategoryNames { get; set; }
        public TrialEligibility Eligibility { get; set; }

        //Helpers that really should be done in velocity instead.
        public string JoinedPhases
        {
            get
            {
                //Should probably sort...
                return string.Join(", ", ProtocolPhases);
            }
        }
        public string JoinedTrialTypes
        {
            get
            {
                return string.Join(", ", StudyCategoryNames);
            }
        }

        public string SecondaryIDList
        {
            get
            {
                return string.Join(", ", String.Join(", ", SecondaryIDs), NCTID);
            }
        }


        public class TrialEligibility
        {

            public string HealthyVolunteers { get; set; }
            public string Gender { get; set; }
            // These are numbers and I don't want to experience the magic of JSON -> "number" yet.
            //"LowAge": UNSEARCHABLE_STR, ## Should be number
            //"HighAge": UNSEARCHABLE_STR, ## Should be number
            public string AgeText { get; set; }
            public string[] Diagnoses { get; set; }

        }

    }
}
