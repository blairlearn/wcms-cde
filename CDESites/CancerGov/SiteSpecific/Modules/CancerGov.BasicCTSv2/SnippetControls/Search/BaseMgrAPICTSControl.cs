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
    /// This is the base control for anything that needs to interact with the CTAPI (e.g. Results and Details view)
    /// </summary>
    public abstract class BaseMgrAPICTSControl : BaseAPICTSControl
    {
        static ILog log = LogManager.GetLogger(typeof(BaseMgrAPICTSControl));

        private int _pageNum = 1;
        private int _itemsPerPage = 10;
        //We may need to know if pagenum or ipp were actually set in the URL or not.  Specifically for the details page.

        public int PageNum { get { return _pageNum; } }
        public int ItemsPerPage { get { return _itemsPerPage; } }        

        //An instance of the BasicCTSManager for interacting with the CTSAPI
        protected BasicCTSManager CTSManager { get; private set; }
        protected CTSSearchParams SearchParams { get; private set; }
        protected NciUrl ParsedReqUrlParams { get; private set; }

        /// <summary>
        /// Initializes the CTSManager and Parses the Query Params
        /// </summary>
        protected override void Init()
        {
            base.Init();

            ParsedReqUrlParams = new NciUrl(true, true, true);  //We need this to be lowercase and collapse duplicate params. (Or not use an NCI URL)
            ParsedReqUrlParams.SetUrl(this.Request.Url.Query);

            //////////////////////////////
            // Create an instance of a BasicCTSManager.
            string apiURL = BasicClinicalTrialSearchAPISection.GetAPIUrl();
            if (string.IsNullOrEmpty(apiURL))
            {
                string err = String.Format("Could not load APIURL for {0}", this.GetType().ToString());
                log.Error(err);
                throw new Exception(err);
            }
            CTSManager = new BasicCTSManager(new ClinicalTrialsAPIClient(apiURL));

            /////////////////////////////
            // Parse the Query to get the search params.
            try
            {
                CTSSearchParamFactory factory = new CTSSearchParamFactory(DynamicTrialListingMapping.Instance);
                SearchParams = factory.Create(ParsedReqUrlParams);
            }
            catch (Exception ex)
            {
                log.Error("could not parse the CTS search parameters", ex);
                throw ex;
            }

            ///////////////////////////
            // Parse the page specific parameters            
            if (IsInUrl(ParsedReqUrlParams, "pn"))
            {
                this._pageNum = ParamAsInt(ParsedReqUrlParams.QueryParameters["pn"], 1);
            }

            _itemsPerPage = Config.DefaultItemsPerPage;
            if (IsInUrl(ParsedReqUrlParams, "ni"))
            {
                this._itemsPerPage = ParamAsInt(ParsedReqUrlParams.QueryParameters["ni"], _itemsPerPage);
            }
        }

        /// <summary>
        /// Helper function to check if a param is used. (And not just set with an empty string.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        protected bool IsInUrl(NciUrl url, string paramName)
        {
            return url.QueryParameters.ContainsKey(paramName) && !String.IsNullOrWhiteSpace(url.QueryParameters[paramName]);
        }

        /// <summary>
        /// Converts a query param to an int; returns 0 if unable to parse
        /// </summary>
        protected int ParamAsInt(string paramVal, int def)
        {
            if (string.IsNullOrWhiteSpace(paramVal))
            {
                return def;
            }
            else
            {
                int tmpInt = 0;
                if (int.TryParse(paramVal.Trim(), out tmpInt))
                {
                    return tmpInt;
                }
                else
                {
                    return def;
                }
            }
        }

        #region Common Velocity Helpers

        /// <summary>
        /// Gets the search form URL for Start Over
        /// </summary>
        /// <returns></returns>
        public string GetSearchFormUrl()
        {
            if (SearchParams.ResultsLinkFlag == ResultsLinkType.Advanced)
            {
                return Config.AdvSearchPagePrettyUrl;
            }
            else
            {
                return Config.BasicSearchPagePrettyUrl;
            }
        }

        #endregion

    }
}
