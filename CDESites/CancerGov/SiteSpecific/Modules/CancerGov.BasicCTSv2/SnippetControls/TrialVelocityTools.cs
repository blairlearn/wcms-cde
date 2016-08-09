using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return trial.DetailedDescription.Replace("\r\n", "<br>");
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

        /// <summary>
        /// Wraper around site Extension method
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public bool SiteHasContact(ClinicalTrial.StudySite site)
        {
            return site.HasContact();
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
            return glossifiedPhase;
        }

        /// <summary>
        /// Gets the Primary Purpose and formats text
        /// </summary>
        /// <param name="trial"></param>
        /// <returns>String - purpose</returns>
        public string GetPrimaryPurpose(ClinicalTrial trial)
        {
            ///TODO: Verify if we need to add other_text and additioncal_qualifier_code to this text
            string purpose = trial.GetPrimaryPurpose();
            return char.ToString(purpose[0]) + purpose.Substring(1).ToLower();
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
        /// Gets the Principal Investigator string
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
