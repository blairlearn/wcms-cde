using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
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
        /// Enumeration representing a bitmap for the fields that are set.
        /// </summary>
        [Flags]
        protected enum SetFields
        {
            None = 0,
            Age = 1,
            Gender = Age << 1,
            ZipCode = Gender << 1,
            ZipProximity = ZipCode << 1,
            Phrase = ZipProximity << 1,
            CancerType = Phrase << 1
        }

        /// <summary>
        /// basic CTS query parameters
        /// </summary>
        protected const string PAGENUM_PARAM = "pn";
        protected const string ITEMSPP_PARAM = "ni";
        protected const string PRASE_PARAM = "q";
        protected const string ZIP_PARAM = "z";
        protected const string ZIPPROX_PARAM = "zp";
        protected const string AGE_PARAM = "a";
        protected const string GENDER_PARAM = "g";
        protected const string CANCERTYPE_PARAM = "t";
        protected const string CANCERTYPEASPHRASE_PARAM = "ct";
        protected const string REDIRECTED_FLAG = "r";

        protected BasicCTSPageInfo _basicCTSPageInfo = null;
        protected TrialListingPageInfo _trialListingPageInfo = null;

        protected bool hasInvalidSearchParam;

        protected SetFields _setFields = SetFields.None;
        protected BasicCTSManager _basicCTSManager = null;
        protected string cancerTypeIDAndHash = null;

        private string _APIURL = string.Empty;

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        protected abstract String WorkingUrl { get; }

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


        /// <summary>
        /// Assemble the default (non-dynamic) search parameters for a listing page.
        /// </summary>
        /// <returns>Search params object</returns>
        protected BaseCTSSearchParam GetSearchParamsForListing()
        {

            //Parse Parameters
            int pageNum = this.ParmAsInt(PAGENUM_PARAM, 1);
            int itemsPerPage = this.ParmAsInt(ITEMSPP_PARAM, TrialListingPageInfo.DefaultItemsPerPage);
            string phrase = this.ParmAsStr(PRASE_PARAM, string.Empty);
            string zip = this.ParmAsStr(ZIP_PARAM, string.Empty);
            int age = this.ParmAsInt(AGE_PARAM, 0);
            int gender = this.ParmAsInt(GENDER_PARAM, 0); //0 = decline, 1 = female, 2 = male, 
            string cancerType = this.ParmAsStr(CANCERTYPE_PARAM, string.Empty);
            string cancerTypeAsPhrase = this.ParmAsStr(CANCERTYPEASPHRASE_PARAM, string.Empty); // if autosuggest is broken, the cancer type field will be parsed as a phrase search

            //BaseCTSSearchParam searchParams = null;
            BaseCTSSearchParam searchParams = new ListingSearchParam();

            // Set Page and Items Per Page
            if (pageNum < 1)
                searchParams.Page = 1;
            else
                searchParams.Page = pageNum;

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
                    return _trialListingPageInfo;
                // Read the basic CTS page information xml
                string spidata = this.SnippetInfo.Data;
                try
                {
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("TrialListingPageInfo not present in JSON, associate an application module item with this page in Percussion");

                    spidata = spidata.Trim();
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("TrialListingPageInfo not present in JSON, associate an application module item with this page in Percussion");

                    TrialListingPageInfo trialListingPageInfo = ModuleObjectFactory<TrialListingPageInfo>.GetModuleObject(spidata);

                    return _trialListingPageInfo = trialListingPageInfo;
                }
                catch (Exception ex)
                {
                    log.Error("could not load the TrialListingPageInfo, check the config info of the application module in percussion", ex);
                    throw ex;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Removed due to concerns over differing search results for CDRID vs. conept ID.
            //HandleLegacyCancerTypeID(); // Redirect for URLs containing "t=CDRXXXX"

            _basicCTSManager = new BasicCTSManager(APIURL);

        }

        /// <summary>
        /// Clears the Response text, issues an HTTP redirect using status 301, and ends
        /// the current request.
        /// </summary>
        /// <param name="Response">The current response object.</param>
        /// <param name="url">The redirection's target URL.</param>
        /// <remarks>Response.Redirect() issues its redirect with a 301 (temporarily moved) status code.
        /// We want these redirects to be permanent so search engines will link to the new
        /// location. Unfortunately, HttpResponse.RedirectPermanent() isn't implemented until
        /// at version 4.0 of the .NET Framework.</remarks>
        /// <exception cref="ThreadAbortException">Called when the redirect takes place and the current
        /// request is ended.</exception>
        protected void DoPermanentRedirect(HttpResponse Response, String url)
        {
            Response.Clear();
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", url);
            Response.End();
        }
    }
}
