using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CancerGov.ClinicalTrials.Basic
{
    /// <summary>
    /// Represents the _source view for a Trial document.
    /// </summary>
    public class TrialDescription
    {
        public string CDRID { get; set; }
        public string OrgStudyID { get; set; }
        public string[] SecondaryIDs { get; set; }
        public string NCTID { get; set; }
        public string BriefTitle { get; set; }
        public string OfficialTitle { get; set; }
        public string BriefSummary { get; set; }
        public string DetailedDescription { get; set; }
        public string CTEntryCriteria { get; set; }
        public string CTGovDisclaimer { get; set; }
        public string CurrentProtocolStatus { get; set; }
        public string[] ProtocolPhases { get; set; }
        public string[] StudyCategoryNames { get; set; }
        public TrialEligibility Eligibility { get; set; }
        public TrialLocation[] Locations { get; set; }

        //Helpers that really should be done in velocity instead.
        public string JoinedPhases { 
            get { 
                //Should probably sort...
                return string.Join(",", ProtocolPhases);  
            } 
        }
        public string JoinedTrialTypes { 
            get { 
                return string.Join(",", StudyCategoryNames); 
            } 
        }

        public string SecondaryIDList
        {
            get
            {
                return string.Join(",", SecondaryIDs, NCTID);
            }
        }

        #region Supporting Classes

        public class TrialLocation
        {
            public string FacilityName { get; set; }
            public LocationPostalAddress PostalAddress { get; set; }
            /** need to add:
            "CTGovContact": { "type": "object"},
            "Investigator": { "type": "object"}
            **/


            public class LocationPostalAddress
            {
                public string CountryName { get; set; }
                public string City { get; set; }
                public string PoliticalSubUnitName { get; set; }
                public string PostalCode_ZIP { get; set; }
                public string PostalCodePosition { get; set; }
                //GeoCode *IS* important here.
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

        #endregion

    }
}
