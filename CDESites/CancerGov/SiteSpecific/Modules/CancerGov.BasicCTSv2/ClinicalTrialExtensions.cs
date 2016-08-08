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
                if(sid != null) 
                {
                    rtnIds.Add(sid);
                }
            }

            // Add other secondary IDs to list
            if (trial.OtherTrialIDs != null)
            {
                rtnIds.AddRange(
                    from id in trial.OtherTrialIDs
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
