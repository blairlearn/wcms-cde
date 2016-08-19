using System;
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

        public string GetPrettyDescription(ClinicalTrial trial)
        {
            String rtn = "<p class='ctrp'>" + HttpUtility.HtmlEncode(trial.DetailedDescription) + "</p>";
            return rtn.Replace("\r\n", "</p><p class='ctrp'>");
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
        public ClinicalTrial.StudySite[] GetFilteredLocations(ClinicalTrial trial, GeoLocation origin, int radius)
        {
                return (from location in trial.Sites
                                where location.Coordinates != null && origin.DistanceBetween(new GeoLocation(location.Coordinates.Latitude, location.Coordinates.Longitude)) <= radius
                                select location).ToArray();
        }

        /// <summary>
        /// Returns the number of locations for a given country sites collection
        /// </summary>
        /// <param name="trial">OrderedDictionary countryLocations</param>
        /// <returns>int - number of locations for given country</returns>
        public int GetLocCount(OrderedDictionary countryLocations)
        {
            return countryLocations.Count;
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
            str = char.ToString(str[0]).ToUpper() + str.Substring(1).ToLower();
            str = str.Replace("_", " ");
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
            string minAgeUnit = trial.GetMinAgeUnit();
            string maxAgeUnit = trial.GetMaxAgeUnit();
            string minAgeText = CleanAgeText(minAgeNum, minAgeUnit);
            string maxAgeText = CleanAgeText(maxAgeNum, maxAgeUnit);
            string ageRange = minAgeText + " to " + maxAgeText;

            // Set age range string for years if both max and min units are years
            if ((maxAgeUnit.ToLower() == "years") && (minAgeUnit.ToLower() == "years"))
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
            else if (maxAgeUnit.ToLower() == minAgeUnit.ToLower())
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
                if (maxAgeNum > 120 && maxAgeUnit.ToLower() == "years")
                {
                    ageRange = minAgeText + " and over";
                }
            }
            return ageRange.ToLower();
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
                if (number % 365 == 0 && unit.ToLower() == "days")
                {
                    number = (number / 365);
                    unit = "years";
                }
                if (number % 12 == 0 && unit.ToLower() == "months")
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
            if(gender.ToLower() == "both")
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
            ///TODO: add logic for glossification
            string glossifiedPhase = trial.GetTrialPhase();
            if(glossifiedPhase == "NA")
            {
                glossifiedPhase = "No phase specified";
            }
            else
            {
                glossifiedPhase = "Phase " + glossifiedPhase.Replace("_", "/");
            }
            return glossifiedPhase;
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
