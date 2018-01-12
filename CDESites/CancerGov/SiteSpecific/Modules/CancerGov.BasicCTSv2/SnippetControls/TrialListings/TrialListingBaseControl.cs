using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using CancerGov.ClinicalTrialsAPI;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    public abstract class TrialListingBaseControl : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(TrialListingBaseControl));

        /// <summary>
        /// basic CTS query parameters
        /// </summary>
        protected const string PAGENUM_PARAM = "pn";
        protected const string ITEMSPP_PARAM = "ni";
        protected const string REDIRECTED_FLAG = "r";
        protected const string RESULTS_LINK_FLAG = "rl";

        protected TrialListingPageInfo _trialListingPageInfo = null;

        protected bool hasInvalidSearchParam;

        // This control still shares BasicCTSManager with the CTS controls
        protected BasicCTSManager _basicCTSManager = null;

        private string _APIURL = string.Empty;

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        protected abstract String WorkingUrl { get; }

        /// <summary>
        /// Assemble the default (non-dynamic) search parameters for a listing page.
        /// </summary>
        /// <returns>Search params object</returns>
        protected BaseCTSSearchParam GetSearchParamsForListing()
        {
            //Parse Parameters
            int pageNum = this.ParmAsInt(PAGENUM_PARAM, 1);
            int itemsPerPage = this.ParmAsInt(ITEMSPP_PARAM, TrialListingPageInfo.DefaultItemsPerPage);

            //BaseCTSSearchParam searchParams = null;
            BaseCTSSearchParam searchParams = new ListingSearchParam();

            // Set Page and Items Per Page
            if (pageNum < 1)
            {
                searchParams.Page = 1;
            }
            else
            {
                searchParams.Page = pageNum;
            }
            searchParams.ItemsPerPage = itemsPerPage;

            return searchParams;
        }

        /// <summary>
        /// Gets a query parameter as a string or uses a default
        /// </summary>
        /// <param name="param"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        protected string ParmAsStr(string param, string def)
        {
            string paramval = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramval))
                return def;
            else
                return paramval.Trim();
        }

        /// <summary>
        /// Gets a query parameter as an int or uses a default
        /// </summary>
        /// <param name="param"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        protected int ParmAsInt(string param, int def)
        {
            string paramval = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramval))
            {
                return def;
            }
            else
            {
                int tmpInt = 0;
                if (int.TryParse(paramval.Trim(), out tmpInt))
                {
                    if (tmpInt == 0)
                        hasInvalidSearchParam = true;

                    return tmpInt;
                }
                else
                {
                    hasInvalidSearchParam = true;
                    return def;
                }
            }
        }

        // Property for TrialListingPageInfo object
        protected TrialListingPageInfo TrialListingPageInfo
        {
            get
            {
                if (_trialListingPageInfo != null)
                { 
                    return _trialListingPageInfo;
                }

                // Read the basic CTS page information JSON
                string spidata = this.SnippetInfo.Data;

                try
                {
                    if (string.IsNullOrWhiteSpace(spidata))
                    {
                        throw new Exception("TrialListingPageInfo not present in JSON, associate an Application Module item with this page in Percussion");
                    }
                    spidata = spidata.Trim();
                    
                    // Get our TrialListingPageInfo object this is JSON data that includes template and URL paths and result count parameters.
                    // It also includes a nested, JSON-formatted string "RequestFilters", which represents the JSON passed in with the API body request - this is
                    // deserialized in TrialListingPageControl.
                    // TODO: handle all deserialization in once place, if possible. This will avoid having to go through the process twice
                    TrialListingPageInfo trialListingPageInfo = ModuleObjectFactory<TrialListingPageInfo>.GetJsonObject(spidata);

                    return _trialListingPageInfo = trialListingPageInfo;
                }
                catch (Exception ex)
                {
                    log.Error("Could not load the TrialListingPageInfo; check the config info of the Application Module item in Percussion", ex);
                    throw ex;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e); 
            _basicCTSManager = new BasicCTSManager(APIClientHelper.GetV1ClientInstance());

        } 

    }
}
