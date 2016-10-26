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
    public abstract class BasicCTSBaseControl : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(BasicCTSBaseControl));

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

        protected bool hasInvalidSearchParam;

        protected SetFields _setFields = SetFields.None;
        protected BasicCTSManager _basicCTSManager = null;
        protected string cancerTypeIDAndHash = null;

        private static readonly string CONFIG_SECTION_NAME = "nci/search/basicClinicalTrialSearchAPI";

        private string _APIURL = string.Empty;

        /// <summary>
        /// Retrieve the working URL of this control from the page XML.
        /// </summary>
        protected abstract String WorkingUrl { get; }

        /// <summary>
        /// Gets the URL for the ClinicalTrials API from the configuration
        /// </summary>
        protected string APIURL {
            get {
                if (String.IsNullOrWhiteSpace(_APIURL))
                {
                    string url = "";

                    BasicClinicalTrialSearchAPISection config = (BasicClinicalTrialSearchAPISection)ConfigurationManager.GetSection(CONFIG_SECTION_NAME);

                    if (config == null)
                        throw new ConfigurationErrorsException("The configuration section, " + CONFIG_SECTION_NAME + ", cannot be found");

                    if (string.IsNullOrWhiteSpace(config.APIProtocol))
                        throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: apiProtocol cannot be null or empty");

                    if (string.IsNullOrWhiteSpace(config.APIHost))
                        throw new ConfigurationErrorsException(CONFIG_SECTION_NAME + "error: apiHost cannot be null or empty");

                    url = string.Format("{0}://{1}", config.APIProtocol, config.APIHost);

                    if (!string.IsNullOrWhiteSpace(config.APIPort))
                    {
                        url += ":" + config.APIPort;
                    }

                    _APIURL = url;
                }

                return this._APIURL;
            }
        }


        protected BaseCTSSearchParam GetSearchParams()
        {
            //Parse Parameters
            int pageNum = this.ParmAsInt(PAGENUM_PARAM, 1);
            int itemsPerPage = this.ParmAsInt(ITEMSPP_PARAM, BasicCTSPageInfo.DefaultItemsPerPage);
            string phrase = this.ParmAsStr(PRASE_PARAM, string.Empty);
            string zip = this.ParmAsStr(ZIP_PARAM, string.Empty);
            int zipProximity = this.ParmAsInt(ZIPPROX_PARAM, BasicCTSPageInfo.DefaultZipProximity); //In miles
            int age = this.ParmAsInt(AGE_PARAM, 0);
            int gender = this.ParmAsInt(GENDER_PARAM, 0); //0 = decline, 1 = female, 2 = male, 
            string cancerType = this.ParmAsStr(CANCERTYPE_PARAM, string.Empty);
            string cancerTypeAsPhrase = this.ParmAsStr(CANCERTYPEASPHRASE_PARAM, string.Empty); // if autosuggest is broken, the cancer type field will be parsed as a phrase search
            string cancerTypeDisplayName = null;

            BaseCTSSearchParam searchParams = null;

            #region Set Cancer Type or Phrase

            if (cancerType != string.Empty)
            {
                //cancerTypeIDAndHash = cancerType;

                //Ok, with the new clinical trials API & EVS terms, an autosuggestion can have multiple ids.  So what the front end will do is produce a t= paramater such as:
                //C12345,C78904|cleaned_up_term_key                

                // The cancerType param may not always contain a pipe. If it does, split the param and key into an array.
                // Otherwise, make the whole parameter string the first item in an array 
                String[] ctarr = (cancerType.Contains("|") ? cancerType.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { cancerType });
                {

                    //split up the disease ids
                    string[] diseaseIDs = ctarr[0].Split(',');

                    // Determine cancer type display name from CIDs and key (if there is no match with the key,
                    // then first term with matching ids is used)
                    string termKey = ctarr.Length > 1 ? ctarr[1] : null;
                    cancerTypeDisplayName = _basicCTSManager.GetCancerTypeDisplayName(diseaseIDs, termKey);
                    

                    if (cancerTypeDisplayName != null)
                    {
                        cancerTypeIDAndHash = cancerType;

                        //Test id to match ^CDR\d+$
                        searchParams = new CancerTypeSearchParam()
                        {
                            //get cancer type.
                            CancerTypeIDs = diseaseIDs,

                            CancerTypeDisplayName = cancerTypeDisplayName
                        };

                        _setFields |= SetFields.CancerType;
                    }
                    else
                    {
                        hasInvalidSearchParam = true;
                        searchParams = new CancerTypeSearchParam();
                    }


                }

            }
            else
            {
                /*bool brokenCTParam = false;
                if (string.IsNullOrWhiteSpace(phrase) && !string.IsNullOrWhiteSpace(cancerTypeAsPhrase))
                    brokenCTParam = true;*/

                // If phrase is set, use phrase for search. Otherwise use cancerTypeAsPhrase (which will be
                // empty string if not set, and work the same as an empty phrase search). Also only set
                // Is BrokenCTSearchParam if just cancerTypeAsPhrase (and not phrase) is set.
                searchParams = new PhraseSearchParam()
                {
                    Phrase = !string.IsNullOrWhiteSpace(phrase) ? phrase : cancerTypeAsPhrase,
                    IsBrokenCTSearchParam = (string.IsNullOrWhiteSpace(phrase) && !string.IsNullOrWhiteSpace(cancerTypeAsPhrase)) ? true : false
                };

                if (!string.IsNullOrWhiteSpace(phrase) || !string.IsNullOrWhiteSpace(cancerTypeAsPhrase))
                {
                    _setFields |= SetFields.Phrase;
                }
            }

            #endregion

            // Fill in common parameters

            #region Set Zip Code + GeoLocation
            if (!string.IsNullOrWhiteSpace(zip))
            {
                string pattern = @"^[0-9]{5}$";

                if (Regex.IsMatch(zip, pattern))
                {
                    searchParams.ZipLookup = _basicCTSManager.GetZipLookupForZip(zip);
                    if (searchParams.ZipLookup != null)
                    {
                        _setFields |= SetFields.ZipCode;
                        if (zipProximity != BasicCTSPageInfo.DefaultZipProximity)
                        {
                            searchParams.ZipRadius = zipProximity;
                            _setFields |= SetFields.ZipProximity;
                        }
                    }
                    else
                    {
                        hasInvalidSearchParam = true;
                    }
                }
                else
                {
                    hasInvalidSearchParam = true;
                }
            }

            #endregion

            #region Set Page and Items Per Page
            if (pageNum < 1)
                searchParams.Page = 1;
            else
                searchParams.Page = pageNum;

            searchParams.ItemsPerPage = itemsPerPage;
            #endregion

            #region Set Age

            //Handle Age
            if (age > 0)
            {
                if (age > 120)
                {
                    hasInvalidSearchParam = true;
                }
                else
                {
                    searchParams.Age = age;
                    _setFields |= SetFields.Age;
                }
            }

            #endregion

            #region Set Gender

            //Handle Gender if specified
            switch (gender)
            {
                case 1:
                    searchParams.Gender = BaseCTSSearchParam.GENDER_FEMALE;
                    _setFields |= SetFields.Gender;
                    break;
                case 2:
                    searchParams.Gender = BaseCTSSearchParam.GENDER_MALE;
                    _setFields |= SetFields.Gender;
                    break;
            }

            #endregion

            return searchParams;
        }

        /// Tentatively Deprecated.
        /// 
        /// <summary>
        /// if the cancer type id passed in begins with "CDR":
        /// - Lookup the appropriate thesaurus ID from the mapping table
        /// - Get the current url, replace the 't' param in the current URL
        ///   with the appropriate thesaurus ID, then do a response.redirect 
        ///   to the new page
        /// </summary>
        /// <params>NciUrl url</params>
        protected void HandleLegacyCancerTypeID()
        {
            // Pattern for a CDRID with appended hash.  The capture group ([1-9][0-9]*) will
            // contain the actual numeric portion of the ID without any leading zeros.
            Regex cdrIDRegex = new Regex("CDR0+([1-9][0-9]*)\\|?.*", RegexOptions.IgnoreCase);

            string cancerTypeID = this.ParmAsStr(CANCERTYPE_PARAM, string.Empty);

            if (!String.IsNullOrWhiteSpace(cancerTypeID) &&
                cancerTypeID.ToLower().StartsWith("cdr"))
            {
                // Strip the legacy ID of its leading "CDR0" and trailing pipe and hash
                Match matches = cdrIDRegex.Match(cancerTypeID);
                if (matches.Success && matches.Groups.Count > 1)
                {
                    cancerTypeID = matches.Groups[1].Value;

                    // Check to see if the CDR cancer type ID has an NCI Thesarus concept ID.
                    string conceptID = LegacyIDLookup.MapLegacyCancerTypeID(cancerTypeID);

                    // If a concept ID was found, then redirect the search to use it.
                    if (!String.IsNullOrWhiteSpace(conceptID))
                    {
                        NciUrl redirectURL = new NciUrl();

                        // Get the page's URL path from the page XML.
                        redirectURL.SetUrl(this.WorkingUrl);

                        // Copy querystring parameters from the request.
                        foreach (string key in Request.QueryString.AllKeys)
                            redirectURL.QueryParameters.Add(key, Request.QueryString[key]);

                        redirectURL.QueryParameters.Remove(CANCERTYPE_PARAM);

                        redirectURL.QueryParameters.Add(CANCERTYPE_PARAM, conceptID);

                        redirectURL.QueryParameters.Add(REDIRECTED_FLAG, String.Empty);

                        // Force page reload with an HTTP 301 resposne code.
                        DoPermanentRedirect(Response, redirectURL.ToString());
                    }
                }
            }
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

        protected BasicCTSPageInfo BasicCTSPageInfo
        {
            get
            {
                if (_basicCTSPageInfo != null)
                    return _basicCTSPageInfo;
                // Read the basic CTS page information xml
                string spidata = this.SnippetInfo.Data;
                try
                {
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("BasicCTSPageInfo not present in xml, associate an application module item  with this page in percussion");

                    spidata = spidata.Trim();
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("BasicCTSPageInfo not present in xml, associate an application module item  with this page in percussion");

                    BasicCTSPageInfo basicCTSPageInfo = ModuleObjectFactory<BasicCTSPageInfo>.GetModuleObject(spidata);

                    return _basicCTSPageInfo = basicCTSPageInfo;
                }
                catch (Exception ex)
                {
                    log.Error("could not load the BasicCTSPageInfo, check the config info of the application module in percussion", ex);
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
