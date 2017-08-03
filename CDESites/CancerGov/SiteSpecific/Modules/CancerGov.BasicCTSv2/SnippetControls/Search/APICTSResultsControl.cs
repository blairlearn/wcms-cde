using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Represents the new CTAPI Driven Results page for Basic & Advanced.
    /// </summary>
    public class APICTSResultsControl : BaseMgrAPICTSControl
    {
        private ClinicalTrialsCollection _results = null;
        

        /// <summary>
        /// Gets the path to the template
        /// </summary>
        /// <returns></returns>
        protected override string GetTemplatePath()
        {
            return this.Config.ResultsPageTemplatePath;
        }

        /// <summary>
        /// Goes and fetches the data from the API & Returns the results to base class to be bound to the template.
        /// </summary>
        /// <returns></returns>
        protected override object GetDataForTemplate()
        {

            //TODO: Get the page number & items per page

            _results = CTSManager.Search(SearchParams, this.PageNum, this.ItemsPerPage);

            //Let's setup some helpful items for the template, so they do not need to be helper functions
            //The start is either 0 if there are no results, or a 1 based offset based on Page number and items per page.
            int startItemNumber = _results.TotalResults == 0 ? _results.TotalResults : ((this.PageNum - 1) * this.ItemsPerPage) + 1;

            //Determine the last item.
            long lastItemNumber = (this.PageNum * this.ItemsPerPage);
            if (lastItemNumber > _results.TotalResults)
                lastItemNumber = _results.TotalResults;

            //Determine the max page
            int maxPage = (int)Math.Ceiling((double)_results.TotalResults / (double)this.ItemsPerPage);

            //TODO: Setup field filters

            //TODO: Add Analytics Filters?

            //Return the object for binding.
            return new
            {
                Results = _results,
                Control = this,
                Parameters = SearchParams,
                PageInfo = new {
                    CurrentPage = this.PageNum.ToString(),
                    MaxPage = maxPage.ToString(),
                    ItemsPerPage = this.ItemsPerPage.ToString(),
                    StartItemNumber = startItemNumber.ToString(),
                    LastItemNumber = lastItemNumber.ToString()
                },
                TrialTools = new TrialVelocityTools()
            };
        }

        #region Velocity Helpers 

        /// <summary>
        /// Gets a detailed view URL
        /// </summary>
        /// <param name="nciID"></param>
        /// <returns></returns>
        public string GetDetailedViewUrl(string nciID)
        {
            //Create a new url for the current details page.
            NciUrl url = new NciUrl();
            url.SetUrl(this.Config.DetailedViewPagePrettyUrl);

            //Convert the current search parameters into a NciUrl
            NciUrl paramsUrl = CTSSearchParamFactory.ConvertParamsToUrl(this.SearchParams);

            //Copy the params 
            url.QueryParameters = paramsUrl.QueryParameters;

            //Add the NCI URL param.
            url.QueryParameters.Add("id", nciID);

            return url.ToString();
        }
        #endregion

    }
}
