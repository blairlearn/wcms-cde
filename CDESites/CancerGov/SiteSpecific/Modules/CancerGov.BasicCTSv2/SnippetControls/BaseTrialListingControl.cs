using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Configs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Web.UI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls
{
    /// <summary>
    /// Serves as the base class for the different types of trial listing pages.
    /// </summary>
    public abstract class BaseTrialListingControl : SnippetControl
    {
        /// <summary>
        /// basic CTS query parameters
        /// </summary>
        protected const string PAGENUM_PARAM = "pn";
        protected const string ITEMSPP_PARAM = "ni";
        protected const string REDIRECTED_FLAG = "r";
        protected const string RESULTS_LINK_FLAG = "rl";

        // This control still shares BasicCTSManager with the CTS controls
        private BasicCTSManager _basicCTSManager = null;
        private string _APIURL = string.Empty;
        private bool hasInvalidSearchParam;

        static ILog log = LogManager.GetLogger(typeof(BaseTrialListingControl));

        /// <summary>
        /// Gets the configuration type of the derrieved class
        /// </summary>
        /// <returns></returns>
        protected abstract Type GetConfigType();

        /// <summary>
        /// Gets the base query for the trial listing.  
        /// <remarks>This should not have pagination, nor should it have additional filters added from Query Parameters</remarks>
        /// </summary>
        /// <returns></returns>
        protected abstract JObject GetTrialQuery();

        /// <summary>
        /// Gets the the configuration 
        /// </summary>
        protected BaseTrialListingConfig Config { get; private set; }


        /// <summary>
        /// Gets the URL for the ClinicalTrials API from BasicClinicalTrialSearchAPISection:GetAPIUrl()
        /// </summary>
        protected string APIURL
        {
            get
            {
                if (String.IsNullOrWhiteSpace(_APIURL))
                {
                    this._APIURL = BasicClinicalTrialSearchAPISection.GetAPIUrl();
                }

                return this._APIURL;
            }
        }

        protected sealed override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.LoadConfig();

            _basicCTSManager = new BasicCTSManager(APIURL);
        }

        protected sealed override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            //Get Trial Query, inject pager stuff, call manager
            JObject query = new JObject(this.GetTrialQuery());
            
            //add in URL filters to query

            //Setup the pager.
            BaseCTSSearchParam searchParams = this.GetSearchParamsForListing();

            //fetch results
            var results = _basicCTSManager.Search(searchParams, query);
            

            //Load VM File
            //SARINA TODO: Load VM FIle & bind results.

            //Set Analytics
        }

        /// <summary>
        /// Loads the JSON configuration from the SnippetInfo's Data
        /// </summary>
        private void LoadConfig()
        {
            Type configType = this.GetConfigType();

            // Read the basic CTS page information JSON
            string sidata = this.SnippetInfo.Data;

            try
            {
                if (string.IsNullOrWhiteSpace(sidata))
                {
                    throw new Exception("TrialListingConfig not present in JSON, associate an Application Module item with this page in Percussion");
                }
                sidata = sidata.Trim();

                // Get our TrialListingPageInfo object this is JSON data that includes template and URL paths and result count parameters.
                // It also includes a nested, JSON-formatted string "RequestFilters", which represents the JSON passed in with the API body request - this is
                // deserialized in TrialListingPageControl.
                // TODO: handle all deserialization in once place, if possible. This will avoid having to go through the process twice
                this.Config = (BaseTrialListingConfig)JsonConvert.DeserializeObject(sidata, configType);

            }
            catch (Exception ex)
            {
                log.Error("Could not load the TrialListingPageInfo; check the config info of the Application Module item in Percussion", ex);
                throw ex;
            }
        }

        /// <summary>
        /// Assemble the default (non-dynamic) search parameters for a listing page.
        /// </summary>
        /// <returns>Search params object</returns>
        private BaseCTSSearchParam GetSearchParamsForListing()
        {
            //Parse Parameters
            int pageNum = this.ParmAsInt(PAGENUM_PARAM, 1);
            int itemsPerPage = this.ParmAsInt(ITEMSPP_PARAM, this.Config.DefaultItemsPerPage);

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
        private string ParmAsStr(string param, string def)
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
        private int ParmAsInt(string param, int def)
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
    }
}
