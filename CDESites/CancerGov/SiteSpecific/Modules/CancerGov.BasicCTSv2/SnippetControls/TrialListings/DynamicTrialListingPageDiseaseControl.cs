﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NCI.Web;
using NCI.Web.CDE;
using Newtonsoft.Json.Linq;
using NCI.Web.CDE.WebAnalytics;
using Common.Logging;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public class DynamicTrialListingPageDiseaseControl : DynamicTrialListingPageControl
    {
        /// <summary>
        /// Replaces the Placeholder Text
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A string with the placeholder text</returns>
        protected override string ReplacePlaceholderText(string input)
        {
            // Replace all disease IDs with overrides
            if (!string.IsNullOrWhiteSpace(this.DiseaseIDs))
            {
                input = input.Replace("${disease_name}", GetOverride(this.DiseaseIDs, true));
                input = input.Replace("${disease_name_lower}", GetOverride(this.DiseaseIDs, false));
            }

            // Replace all trial types with overrides
            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                input = input.Replace("${type_of_trial}", GetOverride(this.TrialType, true));
                input = input.Replace("${type_of_trial_lower}", GetOverride(this.TrialType, false));
            }

            // Replace all intervention IDs with overrides
            if (!string.IsNullOrWhiteSpace(this.InterventionIDs))
            {
                input = input.Replace("${intervention}", GetOverride(this.InterventionIDs, true));
                input = input.Replace("${intervention_lower}", GetOverride(this.InterventionIDs, false));
            }

            return input;
        }

        /// <summary>
        /// Gets the current PatternKey based on URL parameters
        /// </summary>
        /// <returns>A string with the current pattern key</returns>
        protected override string GetCurrentPatternKey()
        {
            // If intervention is present, pattern includes all three params
            if (!string.IsNullOrWhiteSpace(this.InterventionIDs))
            {
                return "DiseaseTypeIntervention";
            }
            // If trial type is present and intervention is not, pattern includes first two params
            else if (!string.IsNullOrWhiteSpace(this.TrialType) && string.IsNullOrWhiteSpace(this.InterventionIDs))
            {
                return "DiseaseType";
            }
            // If only disease is present, pattern includes just that param
            else {
                return "DiseaseOnly";
            }
        }

        /// <summary>
        /// Gets the type specific query parameters to be sent to the API from the given URL params
        /// </summary>
        /// <returns>A JObject with all of the parameters converted to those needed by API</returns>
        protected override JObject GetTypeSpecificQueryParameters()
        {
            JObject queryParams = new JObject();
            string[] diseaseIDsarr = this.DiseaseIDs.Split(new char[] { ',' });

            queryParams.Add("diseases.nci_thesaurus_concept_id", new JArray(diseaseIDsarr));

            if (!string.IsNullOrWhiteSpace(this.TrialType))
            {
                queryParams.Add("primary_purpose.primary_purpose_code", this.TrialType);
            }

            if (!string.IsNullOrWhiteSpace(this.InterventionIDs))
            {
                string[] interventionIDsarr = this.InterventionIDs.Split(new char[] { ',' });
                queryParams.Add("arms.interventions.intervention_code", new JArray(interventionIDsarr));
            }

            return queryParams;
        }

        /// <summary>
        /// Replaces the Placeholder Codes (or text) with Override Labels
        /// </summary>
        /// <param name="codes"></param>
        /// <returns>A string with the override text</returns>
        private string GetOverride(string valToOverride, bool needsTitleCase)
        {
            // Get label mappings
            var labelMapping = DynamicTrialListingMapping.Instance;
            string overrideText = "";
            
            // If combination of codes is in label mappings, set override
            if (labelMapping.MappingContainsKey(valToOverride))
            {
                if(needsTitleCase)
                {
                    overrideText = labelMapping.GetTitleCase(valToOverride);
                }
                else
                {
                    overrideText = labelMapping.Get(valToOverride);
                }
            }
            // Raise 404 error if overrides aren't found
            else
            {
                Response.Headers.Add("X-CTSMap", "ID not found");
                LogManager.GetLogger(typeof(DynamicTrialListingPageDiseaseControl)).ErrorFormat("Invalid parameter in dynamic listing page: {0} does not have override", valToOverride);
                NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("DynamicTrialListingPageDiseaseControl", 404, "Invalid parameter in dynamic listing page: value given does not have override");
            }

            return overrideText;
        }

        /// <summary>
        /// Gets and sets the disease IDs for this listing
        /// </summary>
        private string DiseaseIDs { get; set; }

        /// <summary>
        /// Gets and sets the trial type for this listing
        /// </summary>
        private string TrialType { get; set; }

        /// <summary>
        /// Gets and sets the intervention ids for this listing
        /// </summary>
        private string InterventionIDs { get; set; }


        protected override string[] GetParametersForNoTrials()
        {
            //TODO: Make this logic less sketchy. Use DiseaseIDs and Patterns, etc. 
            return this.CurrAppPath.Split(new char[] { '/' });
        }

        /// <summary>
        /// Parses the URL for all of the parameters for a disease dynamic listing page
        /// </summary>
        protected override void ParseURL()
        {

            if (this.CurrAppPath == "/")
            {
                throw new HttpException(400, "Invalid Parameters");
            }


            List<string> rawParams = new List<string>();

            //TODO: Only do this check in one place and it should be in SetupURLs in the DynamicTrialListingPageControl
            if (this.CurrAppPath == "/notrials")
            {
                NciUrl ParsedReqUrlParams = new NciUrl(true, true, true);  //We need this to be lowercase and collapse duplicate params. (Or not use an NCI URL)
                ParsedReqUrlParams.SetUrl(this.Request.Url.Query);

                //TODO: handle new /notrials link when there are 0 query parameters.
                if ("No p1" == "true")
                {
                    throw new HttpException(400, "Invalid Parameters");
                }
            }
            else
            {
                //Setup rawParams for /a/b/c structure
            }

            //

            

            string[] urlParams = this.CurrAppPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (urlParams.Length >= 4)
            {
                throw new HttpException(400, "Invalid Parameters");
            }

            //Has Disease
            if (urlParams.Length >= 1)
            {
                if (urlParams[0].Contains(","))
                {
                    string[] split = urlParams[0].Split(',');
                    // Sort c-codes in alphanumerical order for comparison to items in mapping file
                    Array.Sort(split);
                    // Lowercase all c-codes for comparison to items in mapping file
                    split = split.Select(s => s.ToLower()).ToArray();
                    this.DiseaseIDs = string.Join(",", split);
                }
                else if(urlParams[0].ToLower().Trim() == "notrials")
                {
                    SetParametersForNoTrialsPage();
                }
                else
                {
                    // Lowercase all c-codes for comparison to items in mapping file
                    this.DiseaseIDs = urlParams[0].ToLower();
                }
            }

            //Has Type of Trial
            if (urlParams.Length >= 2)
            {
                // Lowercase all c-codes for comparison to items in mapping file
                this.TrialType = urlParams[1].ToLower();
            }

            //Has Intervention
            if (urlParams.Length >= 3)
            {
                if (urlParams[2].Contains(","))
                {
                    string[] split = urlParams[2].Split(',');
                    // Sort c-codes in alphanumerical order for comparison to items in mapping file
                    Array.Sort(split);
                    // Lowercase all c-codes for comparison to items in mapping file
                    split = split.Select(s => s.ToLower()).ToArray();
                    this.InterventionIDs = string.Join(",", split);
                }
                else
                {
                    // Lowercase all c-codes for comparison to items in mapping file
                    this.InterventionIDs = urlParams[2].ToLower();
                }
            }
        }


        private void SetParametersForNoTrialsPage()
        {
            try
            {
                if(HttpContext.Current.Request.QueryString.Count >= 1)
                {
                    this.DiseaseIDs = HttpContext.Current.Request.QueryString[0].ToLower();
                }

                if (HttpContext.Current.Request.QueryString.Count >= 2)
                {
                    this.TrialType = HttpContext.Current.Request.QueryString[1].ToLower();
                }

                if (HttpContext.Current.Request.QueryString.Count >= 3)
                {
                    if (HttpContext.Current.Request.QueryString[2].Contains(","))
                    {
                        string[] split = HttpContext.Current.Request.QueryString[2].Split(',');
                        // Sort c-codes in alphanumerical order for comparison to items in mapping file
                        Array.Sort(split);
                        // Lowercase all c-codes for comparison to items in mapping file
                        split = split.Select(s => s.ToLower()).ToArray();
                        this.InterventionIDs = string.Join(",", split);
                    }
                    else
                    {
                        // Lowercase all c-codes for comparison to items in mapping file
                        this.InterventionIDs = HttpContext.Current.Request.QueryString[2].ToLower();
                    }
                }
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                string stackTrace = ex.StackTrace;
            }
        }

        /// <summary>
        /// Set default pageLoad analytics for this page
        /// </summary>
        protected override void SetAnalytics()
        {
            string val = "clinicaltrials_custom";
            string desc = "Clinical Trials: Custom";

            // Format string for analytics params: Intervention|Disease IDs|Trial Type|Intervention IDs|Total Results
            string[] analyticsParams = new string[5];
            analyticsParams[0] = "Disease";
            analyticsParams[1] = (!string.IsNullOrWhiteSpace(this.DiseaseIDs)) ? this.DiseaseIDs : "none";
            analyticsParams[2] = (!string.IsNullOrWhiteSpace(this.TrialType)) ? this.TrialType : "none";
            analyticsParams[3] = (!string.IsNullOrWhiteSpace(this.InterventionIDs)) ? this.InterventionIDs : "none";
            analyticsParams[4] = this.TotalSearchResults.ToString();
            string dynamicAnalytics = string.Join("|", analyticsParams);

            string resultsPerPage;
            if (this.TotalSearchResults < this.GetItemsPerPage())
            {
                resultsPerPage = this.TotalSearchResults.ToString();
            }
            else
            {
                resultsPerPage = this.GetItemsPerPage().ToString();
            }

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
                wbField.Value = resultsPerPage;
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
    }
}
