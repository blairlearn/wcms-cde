using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using NCI.Web;
using NCI.Web.CDE;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public class DynamicTrialListingPageDiseaseControl : DynamicTrialListingPageControl
    {
        /// <summary>
        /// Replaces the Placeholder Text
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override string ReplacePlaceholderText(string input)
        {
            //TODO: Replace text
            return input;
        }

        protected override string GetCurrentPatternKey()
        {
            throw new NotImplementedException();
        }

        protected override string GetTypeSpecificQueryParameters()
        {
            throw new NotImplementedException();
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

            //Has Disease
            if (urlParams.Length >= 1)
            {
                this.DiseaseIDs = urlParams[0];
            }

            //Has Type of Trial
            if (urlParams.Length >= 2)
            {
                this.TrialType = urlParams[1];
            }

            //Has Intervention
            if (urlParams.Length >= 3)
            {
                this.InterventionIDs = urlParams[2];
            }
        }
    }
}
