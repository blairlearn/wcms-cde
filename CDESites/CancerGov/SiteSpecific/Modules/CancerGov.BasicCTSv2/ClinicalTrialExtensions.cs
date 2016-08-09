using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2
{
    public static class ClinicalTrialExtensions
    {
        /// <summary>
        /// Determines if this trial has eligibility criteria
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public static bool HasEligibilityCriteria(this ClinicalTrial trial)
        {
            return (trial.EligibilityInfo != null && trial.EligibilityInfo.UnstructuredCriteria != null);
        }

        /// <summary>
        /// Gets the eligibility criteria for inclusion in this trial
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public static string[] GetInclusionCriteria(this ClinicalTrial trial) {

            if (trial.EligibilityInfo != null && trial.EligibilityInfo.UnstructuredCriteria != null)
            {
                return (
                    from criterion in trial.EligibilityInfo.UnstructuredCriteria
                    where criterion.IsInclusionCriterion
                    select criterion.Description
                ).ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Gets the eligibility criteria for exclusion form this trial
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public static string[] GetExclusionCriteria(this ClinicalTrial trial) {

            if (trial.EligibilityInfo != null && trial.EligibilityInfo.UnstructuredCriteria != null)
            {
                return (
                    from criterion in trial.EligibilityInfo.UnstructuredCriteria
                    where !criterion.IsInclusionCriterion
                    select criterion.Description
                ).ToArray();
            }
            else
            {
                return new string[0];
            }
        }


        /// <summary>
        /// Determines if a study site has Contact Information
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static bool HasContact(this ClinicalTrial.StudySite site)
        {
            return !String.IsNullOrWhiteSpace(site.ContactEmail) 
                || !String.IsNullOrWhiteSpace(site.ContactName) 
                || !String.IsNullOrWhiteSpace(site.ContactPhone);
        }

        /// <summary>
        /// Gets the phase number only
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - phase</returns>
        public static String GetTrialPhase(this ClinicalTrial trial)
        {
            string rtnPhase = String.Empty;
            if (trial.TrialPhase != null)
            {
                if (!String.IsNullOrEmpty(trial.TrialPhase.PhaseNumber))
                {
                    rtnPhase = trial.TrialPhase.PhaseNumber;
                }
            }
            return rtnPhase;
        }

        /// <summary>
        /// Gets the Primary Purpose
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - purpose</returns>
        public static String GetPrimaryPurpose(this ClinicalTrial trial)
        {
            string purpose = String.Empty;
            if (trial.PrimaryPurpose != null)
            {
                if (!String.IsNullOrEmpty(trial.PrimaryPurpose.Code))
                {
                    purpose = trial.PrimaryPurpose.Code;
                }
            }
            return purpose;
        }

        /// <summary>
        /// Gets the Lead Organization
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public static string GetLeadOrg(this ClinicalTrial trial)
        {
            string leadOrg = String.Empty;
            if (!String.IsNullOrEmpty(trial.LeadOrganizationName))
            {
                leadOrg = trial.LeadOrganizationName;
            }
            return leadOrg;
        }

        /// <summary>
        /// Gets an array of all of the collaborator names
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>string[] collaborators</returns>
        public static string[] GetCollaborators(this ClinicalTrial trial)
        {
            List<String> collaborators = new List<String>();
            if (trial.Collaborators != null)
            {
                collaborators.AddRange(
                    from collab in trial.Collaborators
                    select collab.Name
                );
            }
            return collaborators.ToArray();
        }

        /// <summary>
        /// Gets the Principal Investigator
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>string - principal</returns>
        public static string GetPrincipalInvestigator(this ClinicalTrial trial)
        {
            string principal = String.Empty;
            if (!String.IsNullOrEmpty(trial.PrincipalInvestigator))
            {
                principal = trial.PrincipalInvestigator;
            }
            return principal;
        }

        /// <summary>
        /// Gets an array of all of the secondary IDs, which are those that are not the NCT ID or the Primary ID
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public static string[] GetSecondaryIDs(this ClinicalTrial trial)
        {
            List<String> rtnIds = new List<String>();
            String[] secIds = { trial.NCIID, trial.CCRID, trial.CTEPID, trial.DCPID };
            String dupe = "";

            // Add secondary IDs (NCI, CCR, CTEP, DCP) to list
            foreach(String sid in secIds)
            {
                if(!String.IsNullOrEmpty(sid)) 
                {
                    rtnIds.Add(sid);
                }
            }

            // Add other secondary IDs to list
            if (trial.OtherTrialIDs != null)
            {
                rtnIds.AddRange(
                    from id in trial.OtherTrialIDs
                    where !String.IsNullOrEmpty(id.Value)
                    select id.Value
                );
            }

            // Remove any duplicate found in Primary or Secondary IDs 
            foreach (String rtnid in rtnIds.ToList())
            {
                if(rtnid == trial.NCTID || rtnid == trial.ProtocolID || rtnid == dupe)
                {
                    rtnIds.Remove(rtnid);
                }
                dupe = rtnid;
            }

            return rtnIds.ToArray();
        }

        /// <summary>
        /// Gets all the locations sorted and separated by USA, Canada and Other
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public static object GetAllSortedLocations(this ClinicalTrial trial)
        {
            return new
            {
                USA = GetUSASortedLocations(trial),
                Canada = GetCanadianSortedLocations(trial),
                Other = GetOtherSortedLocations(trial)
            };
        }

        private static object GetUSASortedLocations(ClinicalTrial trial)
        {
            return GetNASortedLocations(trial, "United States");
        }

        private static object GetCanadianSortedLocations(ClinicalTrial trial)
        {
            return GetNASortedLocations(trial, "Canada");
        }

        private static object GetNASortedLocations(ClinicalTrial trial, string country)
        {
            OrderedDictionary locations = new OrderedDictionary();

            if (trial.Sites != null)
            {
                var sortedSites = from site in trial.Sites
                                  where site.Org.Country == country
                                  orderby site.Org.StateOrProvince, site.Org.Name
                                  select site;

                foreach (ClinicalTrial.StudySite site in sortedSites)
                {
                    if (!locations.Contains(site.Org.StateOrProvince))
                        locations.Add(site.Org.StateOrProvince, new List<ClinicalTrial.StudySite>());

                    ((List<ClinicalTrial.StudySite>)locations[site.Org.StateOrProvince]).Add(site);
                }
            }

            return locations;
        }


        private static object GetOtherSortedLocations(ClinicalTrial trial)
        {
            OrderedDictionary locations = new OrderedDictionary();

            if (trial.Sites != null)
            {
                var sortedSites = from site in trial.Sites
                                  where (site.Org.Country != "United States"
                                        && site.Org.Country != "Canada")
                                  orderby site.Org.Country, site.Org.StateOrProvince, site.Org.Name
                                  select site;

                foreach (ClinicalTrial.StudySite site in sortedSites)
                {
                    if (!locations.Contains(site.Org.Country))
                        locations.Add(site.Org.Country, new List<ClinicalTrial.StudySite>());

                    ((List<ClinicalTrial.StudySite>)locations[site.Org.Country]).Add(site);
                }
            }
            return locations;

        }

    }
}
