using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NCI.Web;
using NCI.Web.CDE;
using Newtonsoft.Json.Linq;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public class DynamicTrialListingPageInterventionControl : DynamicTrialListingPageControl
    {
        /// <summary>
        /// Replaces the Placeholder Text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override string ReplacePlaceholderText(string input)
        {
            // Replace all intervention IDs with overrides
            if (!string.IsNullOrWhiteSpace(this.InterventionIDs))
            {
                input.Replace("$intervention_name", this.InterventionIDs);
            }

            // Replace all trial types with overrides
            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                input.Replace("${type_of_trial", this.TrialType);
            }

            return input;
        }

        protected override string GetCurrentPatternKey()
        {
            // If trial type is present, pattern includes both params
            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                return "InterventionType";
            }
            // If only intervention is present, pattern includes just that param
            else
            {
                return "InterventionOnly";
            }
        }

        protected override JObject GetTypeSpecificQueryParameters()
        {
            JObject queryParams = new JObject();
            string[] interventionIDsarr = this.InterventionIDs.Split(new char[] { ',' });

            queryParams.Add("arms.interventions.intervention_code", new JArray(interventionIDsarr));

            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                queryParams.Add("primary_purpose.primary_purpose_code", this.TrialType);
            }

            return queryParams;
        }

        /// <summary>
        /// Replaces the Placeholder Codes with Override Labels
        /// </summary>
        /// <param name="codes"></param>
        /// <returns>A string with the override text</returns>
        private string GetCodeOverride(string codes)
        {
            // Get label mappings
            var labelMapping = DynamicTrialListingMapping.Instance;
            string overrideText = "";

            // If combination of codes is in label mappings, set override
            if (labelMapping.MappingContainsKey(codes))
            {
                overrideText = labelMapping[codes];
            }
            // If specific combination isn't in label mappings, split them apart and look for
            // overrides for each individual code
            else if (codes.Contains(","))
            {
                string[] codeArr = codes.Split(new char[] { ',' });
                for (int i = 0; i < codeArr.Length; i++)
                {
                    if (labelMapping.MappingContainsKey(codeArr[i]))
                    {
                        // Replace code with override text
                        codeArr[i] = labelMapping[codeArr[i]];
                    }
                    else
                    {
                        // Raise 404 error if code doesn't have an override (regardless of whether other codes have them)
                        NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("DynamicTrialListingPageInterventionControl", 404, "Invalid parameter in dynamic listing page: c-code given does not have override");
                    }
                }
                if (codeArr.Length > 1)
                {
                    // If there are 3 or more codes, use commas, Oxford comma, and "and" to join string
                    if(codeArr.Length >= 3)
                    {
                        overrideText = string.Format("{0}, and {1}", string.Join(", ", codeArr, 0, codeArr.Length - 1), codeArr[codeArr.Length - 1]);

                    }
                    // If there are only two codes, just use "and"
                    else
                    {
                        overrideText = string.Format("{0} and {1}", codeArr[0], codeArr[1]);
                    }
                }
                else
                {
                    // There is only one code, so just use that override
                    overrideText = codeArr[0];
                }
            }
            // Raise 404 error if overrides aren't found
            else
            {
                NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("DynamicTrialListingPageInterventionControl", 404, "Invalid parameter in dynamic listing page: c-code given does not have override");
            }

            return overrideText;
        }

        /// <summary>
        /// Gets and sets the intervention ids for this listing
        /// </summary>
        private string InterventionIDs { get; set; }

        /// <summary>
        /// Gets and sets the trial type for this listing
        /// </summary>
        private string TrialType { get; set; }

        /// <summary>
        /// Parses the URL for all of the parameters for an intervention dynamic listing page
        /// </summary>
        protected override void ParseURL()
        {
            if (this.CurrAppPath == "/")
            {
                throw new HttpException(400, "Invalid Parameters");
            }

            string[] urlParams = this.CurrAppPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlParams.Length >= 4)
            {
                throw new HttpException(400, "Invalid Parameters");
            }

            //Has Intervention
            if (urlParams.Length >= 1)
            {
                if (urlParams[0].Contains(","))
                {
                    string[] split = urlParams[0].Split(',');
                    Array.Sort(split);
                    split = split.Select(s => s.ToLower()).ToArray();
                    this.InterventionIDs = string.Join(",", split);
                }
                else
                {
                    this.InterventionIDs = urlParams[0];
                }
            }

            //Has Type of Trial
            if (urlParams.Length >= 2)
            {
                this.TrialType = urlParams[1];
            }
        }

        #region Analytics methods
        /// <summary>
        /// Set default pageLoad analytics for this page
        /// </summary>
        protected override void SetAnalytics()
        {
            string val = "clinicaltrials_custom";
            string desc = "Clinical Trials: Custom";

            string[] analyticsParams = new string[4];
            analyticsParams[0] = "Intervention";
            analyticsParams[1] = (!string.IsNullOrWhiteSpace(this.InterventionIDs)) ? this.InterventionIDs : "none";
            analyticsParams[2] = (!string.IsNullOrWhiteSpace(this.TrialType)) ? this.TrialType : "none";
            analyticsParams[3] = this.TotalSearchResults.ToString();

            string dynamicAnalytics = string.Join("|", analyticsParams);

            // Set event
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event2, wbField =>
            {
                wbField.Value = WebAnalyticsOptions.Events.event2.ToString();
            });

            // Set props
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop11, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop20, wbField =>
            {
                wbField.Value = dynamicAnalytics;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Props.prop62, wbField =>
            {
                wbField.Value = desc;
            });

            // Set eVars
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar10, wbField =>
            {
                wbField.Value = this.Config.DefaultItemsPerPage.ToString();
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar11, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar20, wbField =>
            {
                wbField.Value = dynamicAnalytics;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar47, wbField =>
            {
                wbField.Value = val;
            });
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar62, wbField =>
            {
                wbField.Value = desc;
            });

        }
        #endregion
    }
}
