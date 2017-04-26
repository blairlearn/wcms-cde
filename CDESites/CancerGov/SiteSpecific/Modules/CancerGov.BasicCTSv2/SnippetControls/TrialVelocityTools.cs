using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// This class houses tools that can be used in the velocity templates for Results and View
    /// </summary>
    public class TrialVelocityTools
    {
        /// <summary>
        /// Gets and formats the Trial Objectives and Outline
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - detailed description</returns>
        public string GetPrettyDescription(ClinicalTrial trial)
        {
            String rtn = "<p class='ctrp'>" + HttpUtility.HtmlEncode(trial.DetailedDescription) + "</p>";
            return rtn.Replace("\r\n", "</p><p class='ctrp'>");
        }

        /// <summary>
        /// Get a brief summary of the trial ("Description" accordion section on view page)
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - brief summary</returns>
        public string GetBriefSummary(ClinicalTrial trial)
        {
            String rtn = trial.BriefSummary;
            return rtn;
        }

        /// <summary>
        /// Determines if this trial has eligibility criteria
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public bool HasEligibilityCriteria(ClinicalTrial trial)
        {
            return trial.HasEligibilityCriteria();
        }

        /// <summary>
        /// Gets the inclusion criteria for a trial
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public string[] GetInclusionCriteria(ClinicalTrial trial)
        {
            return trial.GetInclusionCriteria();
        }

        /// <summary>
        /// Gets the exclusion criteria for a trial
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public string[] GetExclusionCriteria(ClinicalTrial trial)
        {
            return trial.GetExclusionCriteria();
        }

        /// <summary>
        /// Wrapper around trial Extension Method.
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public object GetAllSortedLocations(ClinicalTrial trial)
        {
            return trial.GetAllSortedLocations();
        }

        //$trial.GetUSLocations($SearchResults.Control.SearchParams.ZipLookup.GeoCode, $SearchResults.Control.SearchParams.ZipRadius).length

        /// <summary>
        /// Get all us Locations, but filtered by origin and radius in miles
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public IEnumerable<ClinicalTrial.StudySite> GetFilteredLocations(ClinicalTrial trial, GeoLocation origin, int radius)
        {
            return (trial.Sites.Where(site => site.Coordinates != null &&
                                              origin.DistanceBetween(new GeoLocation(site.Coordinates.Latitude, site.Coordinates.Longitude)) <= radius &&
                                              site.Country == "United States")
                    .OrderBy(loc => loc.Country)
                    .ThenBy(loc => loc.StateOrProvince)
                    .ThenBy(loc => loc.City).ToArray());
        }

        /// <summary>
        /// Returns a list of all active study sites. 
        /// </summary>
        /// <param name="trial">Clinical trial</param>
        /// <returns>List of active sites for a given trial</returns>
        public IEnumerable<ClinicalTrial.StudySite> GetAllStudySites(ClinicalTrial trial)
        {
            IEnumerable<ClinicalTrial.StudySite> sites = null;
            BasicCTSManager mgr = new BasicCTSManager();

            // Assemble list of trial sites to be printed by the Velocity template:
            // 1. Filter inactive study sites out of our sites list
            // 2. Make a list of USA sites, sorted by state, then city
            // 3. Make a list of international sites, sorted by country, then city 
            // 4. Join the lists with USA as the first group of items
            if (trial.Sites != null)
            {
                trial.Sites = new List<ClinicalTrial.StudySite>(trial.Sites.Where(site => mgr.ActiveRecruitmentStatuses.Any(status => status.ToLower() == site.RecruitmentStatus.ToLower())));
                var usaSites = trial.Sites.Where(s => s.Country == "United States").OrderBy(s => s.StateOrProvince).ThenBy(s => s.City).ToArray();
                var otherSites = trial.Sites.Where(s => s.Country != "United States").OrderBy(s => s.Country).ThenBy(s => s.City).ToArray();
                sites = usaSites.Concat(otherSites);
            }
            return sites;
        }

        /// <summary>
        /// Returns the number of locations for a given country sites collection
        /// </summary>
        /// <param name="trial">OrderedDictionary countryLocations</param>
        /// <returns>int - number of locations for given country</returns>
        public int GetLocCount(OrderedDictionary countryLocations)
        {
            int count = 0;

            foreach (string key in countryLocations.Keys)
            {
                count += ((List<ClinicalTrial.StudySite>)countryLocations[key]).Count;
            }

            return count;
        }

        /// <summary>
        /// Wrapper around site Extension method
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool SiteHasContact(ClinicalTrial.StudySite site)
        {
            return site.HasContact();
        }

        /// <summary>
        /// Capitalizes first letter and removes underscores
        /// </summary>
        public string GetFormattedString(String str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                str = char.ToString(str[0]).ToUpper() + str.Substring(1).ToLower();
                str = str.Replace("_", " ");
            }
            return str;
        }

        /// <summary>
        /// Get formatted age range string. Based on the max and min age for the trial, display the 
        /// resulting age in a pretty format and with units specified.
        /// TODO: Handle any edge cases with redundant units
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public String GetAgeString(ClinicalTrial trial)
        {
            int minAgeNum = trial.GetMinAgeNum();
            int maxAgeNum = trial.GetMaxAgeNum();
            string minAgeUnit = trial.GetMinAgeUnit().ToLower();
            string maxAgeUnit = trial.GetMaxAgeUnit().ToLower();
            string minAgeText = CleanAgeText(minAgeNum, minAgeUnit);
            string maxAgeText = CleanAgeText(maxAgeNum, maxAgeUnit);
            string ageRange = minAgeText + " to " + maxAgeText;

            // Set age range string for years if both max and min units are years
            if ((maxAgeUnit == "years") && (minAgeUnit == "years"))
            {
                ageRange = minAgeNum.ToString() + " to " + maxAgeNum.ToString() + " years";
                if (minAgeNum < 1 && maxAgeNum <= 120)
                {
                    ageRange = maxAgeText + " and under";
                }
                else if (minAgeNum > 0 && maxAgeNum > 120)
                {
                    ageRange = minAgeText + " and over";
                }
                else if (minAgeNum < 1 && maxAgeNum > 120)
                {
                    ageRange = "Not specified";
                }
            }

            // Set age range string for max and min if units match 
            else if (maxAgeUnit == minAgeUnit)
            {
                ageRange = minAgeNum.ToString() + " to " + maxAgeNum.ToString() + maxAgeUnit;
                if (minAgeNum < 1 && maxAgeNum <= 999)
                {
                    ageRange = maxAgeText + " and under";
                }
                else if (minAgeNum > 0 && maxAgeNum >= 999)
                {
                    ageRange = minAgeText + " and over";
                }
            }

            // Set age range string if units do not match
            else
            {
                if (maxAgeNum > 120 && maxAgeUnit == "years")
                {
                    ageRange = minAgeText + " and over";
                }
            }

            ageRange = ageRange.Replace("year to", "to").Replace("years to", "to");
            return GetFormattedString(ageRange);
        }

        /// <summary>
        /// Additional formatting for non-year age range increments
        ///  - Converts days/months into years if no remainder 
        ///  - Changes unit name to singular if we encounter a count of "1"
        /// </summary>
        /// <param name="number">int</param>
        /// <param name="unit">string</param>
        /// <returns>string - agerange</returns>
        protected String CleanAgeText(int number, string unit)
        {
            // Convert day/month values to years if needed
            if (number > 0)
            {
                if (number % 365 == 0 && unit == "days")
                {
                    number = (number / 365);
                    unit = "years";
                }
                if (number % 12 == 0 && unit == "months")
                {
                    number = (number / 12);
                    unit = "years";
                }
            }
            // Change plural units to singular if needed
            if (number == 1)
            {
                unit = unit.Remove(unit.LastIndexOf("s"));
            }

            return number.ToString() + " " + unit;
        }

        /// <summary>
        /// Get formatted gender string
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public string GetGenderString(ClinicalTrial trial)
        {
            String gender = trial.GetGender();
            if (gender.ToLower() == "both")
            {
                gender = "Male or Female";
                return gender;
            }
            return GetFormattedString(gender);
        }

        /// <summary>
        /// Gets the list of secondary IDs from extension method as comma separated string
        /// </summary>
        /// <param name="trial"></param>
        /// <returns></returns>
        public string GetSecondaryIDsString(ClinicalTrial trial)
        {
            string[] ids = trial.GetSecondaryIDs();
            if (ids.Length == 0)
                return string.Empty;
            else
                return string.Join(", ", ids);
        }

        /// <summary>
        /// Gets the Phase number and adds glossification markup
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String glossifedPhase</returns>
        public string GetGlossifiedPhase(ClinicalTrial trial)
        {
            string phase = trial.GetTrialPhase();
            string cdrID = "";
            string htmlBlobOpen;
            string htmlBlobClose;

            // Check the API trial values and assign to a CDR ID if valid
            switch (phase)
            {
                case "I": cdrID = "45830";
                    break;
                case "I_II": cdrID = "45832";
                    break;
                case "II": cdrID = "45831";
                    break;
                case "II_III": cdrID = "45834";
                    break;
                case "III": cdrID = "45833";
                    break;
                case "IV": cdrID = "45835";
                    break;
            }

            if (String.IsNullOrEmpty(cdrID))
            {
                phase = "No phase specified";
            }
            else
            {
                htmlBlobOpen = @"<a onclick=""javascript:popWindow('defbyid','CDR00000" + cdrID +
                               @"&amp;version=Patient&amp;language=English'); return false;"" href=""/Common/PopUps/popDefinition.aspx?id=CDR00000" + cdrID +
                               @"45830&amp;version=Patient&amp;language=English"" class=""definition"">";
                htmlBlobClose = @"</a>";
                phase = htmlBlobOpen + "Phase " + phase.Replace("_", "/") + htmlBlobClose;
            }
            return phase;
        }

        /// <summary>
        /// Gets the Phase number from the API and formats it for presentation
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String </returns>
        public string GetPhase(ClinicalTrial trial)
        {
            string phase = trial.GetTrialPhase();
            if (!String.IsNullOrWhiteSpace(phase))
            {
                phase = "Phase " + phase.Replace("_", "/");
            }
            return phase;
        }

        /// <summary>
        /// Gets the Primary Purpose and formats text
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - purpose</returns>
        public string GetTrialType(ClinicalTrial trial)
        {
            ///TODO: Verify if we need to add other_text and additioncal_qualifier_code to this text
            string purpose = trial.GetPrimaryPurpose();
            return GetFormattedString(purpose);
        }

        /// <summary>
        /// Gets the Lead Organization string
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - sponsor</returns>
        public string GetLeadSponsor(ClinicalTrial trial)
        {
            string sponsor = trial.GetLeadOrg();
            return sponsor;
        }

        /// <summary>
        /// Gets array of Collaborator name strings
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>string[] collaborators</returns>
        public string[] GetCollabsArray(ClinicalTrial trial)
        {
            return trial.GetCollaborators();
        }

        /// <summary>
        /// Gets Principal Investigator strings and joins them into an array
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>string - principal</returns>
        public string[] GetPrincipalArray(ClinicalTrial trial)
        {
            List<String> principal = new List<String>();
            if (!String.IsNullOrWhiteSpace(trial.GetPrincipalInvestigator()))
            {
                principal.Add(trial.GetPrincipalInvestigator());
            }
            /* TODO - Verify if there actually any instances where we 
             * have more than one Principal Investigator - OR if there
             * is another value from the API that we should combine with 
             * this
             */
            // principal.Add(trial.GetSomeOtherValue());
            return principal.ToArray();
        }

    }
}
