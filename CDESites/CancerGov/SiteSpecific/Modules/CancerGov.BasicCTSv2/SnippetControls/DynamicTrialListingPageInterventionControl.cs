using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NCI.Web;
using NCI.Web.CDE;
using Newtonsoft.Json.Linq;

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
            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                input.Replace("${type_of_trial", this.TrialType);
            }
            if (!string.IsNullOrWhiteSpace(this.InterventionIDs))
            {
                input.Replace("$intervention_name", this.InterventionIDs);
            }
            return input;
        }

        protected override string GetCurrentPatternKey()
        {
            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                return "InterventionType";
            }
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
            var labelMapping = DynamicTrialListingMapping.Instance;
            string overrideText = "";

            if (labelMapping.MappingContainsKey(codes))
            {
                overrideText = labelMapping[codes];
            }
            else if (codes.Contains(","))
            {
                string[] codeArr = codes.Split(new char[] { ',' });
                for (int i = 0; i < codeArr.Length; i++)
                {
                    if (labelMapping.MappingContainsKey(codeArr[i]))
                    {
                        codeArr[i] = labelMapping[codeArr[i]];
                    }
                    else
                    {
                        NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("DynamicTrialListingPageInterventionControl", 404, "Invalid parameter in dynamic listing page: c-code given does not have override");
                    }
                }
                if (codeArr.Length > 1)
                {
                    overrideText = string.Format("{0} and {1}", string.Join(", ", codeArr, 0, codeArr.Length - 1), codeArr[codeArr.Length - 1]);
                }
                else
                {
                    overrideText = codeArr[0];
                }
            }
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
    }
}
