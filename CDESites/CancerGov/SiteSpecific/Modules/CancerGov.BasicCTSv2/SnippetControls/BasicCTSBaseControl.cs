using System;
using System.Linq;
using System.Collections.Generic;
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
        protected enum QueryFieldsSetByUser
        {
            None = 0,
            CancerType          = 1 << 0,
            CancerSubtype       = 1 << 1,
            CancerStage         = 1 << 2,
            CancerFindings      = 1 << 3,
            Age                 = 1 << 4,
            Gender              = 1 << 5,
            Phrase              = 1 << 6,
            ZipCode             = 1 << 7,
            ZipProximity        = 1 << 8,
            Country             = 1 << 9,
            City                = 1 << 10,
            State               = 1 << 11,
            Hospital            = 1 << 12,
            AtNIH               = 1 << 13,
            TrialType           = 1 << 14,
            DrugCode            = 1 << 15,
            DrugName            = 1 << 16,
            TreatmentCode       = 1 << 17,
            TreatmentName       = 1 << 18,
            TrialPhase          = 1 << 19,
            NewTrialsOnly       = 1 << 20,
            TrialIDs            = 1 << 21,
            TrialInvestigator   = 1 << 22,
            LeadOrganization    = 1 << 23
        }

        #region CTS query parameters
        // Navigation & display parameters
        protected const string PAGENUM_PARAM = "pn";
        protected const string ITEMSPP_PARAM = "ni";
        protected const string REDIRECTED_FLAG = "r";
        protected const string RESULTS_LINK_FLAG = "rl";

        // Type/phrase search parameters
        protected const string CANCERTYPE_PARAM = "t";
        protected const string CANCERTYPEASPHRASE_PARAM = "ct";
        protected const string PHRASE_PARAM = "q";
        protected const string CANCERTYPE_SUBTYPE = "st";
        protected const string CANCERTYPE_STAGE = "stg";
        protected const string CANCERTYPE_FINDINGS = "fin";

        // Location search parameters
        protected const string LOCATION_COUNTRY = "lcnty";
        protected const string LOCATION_CITY = "lcty";
        protected const string LOCATION_STATE = "lst";
        protected const string HOSPITAL_INSTITUTION = "hos";
        protected const string AT_NIH = "nih";
        protected const string NIH_ZIP_CODE = "20892";
        protected const string ZIP_PARAM = "z";
        protected const string ZIPPROX_PARAM = "zp";
        protected const string LOCATION_ALL = "all";

        // Other search parameters
        protected const string AGE_PARAM = "a";
        protected const string GENDER_PARAM = "g";
        protected const string TRIAL_TYPE = "tt";
        protected const string DRUG_CODE = "d";
        protected const string DRUG_NAME = "ds";
        protected const string TREATMENT_CODE = "i";
        protected const string TREATMENT_NAME = "is";
        protected const string TRIAL_PHASE = "tp";
        protected const string NEW_TRIALS_ONLY = "new";
        protected const string NCT_ID = "id";
        protected const string TRIAL_IDS = "tid";
        protected const string TRIAL_INVESTIGATOR = "in";
        protected const string LEAD_ORGANIZATION = "lo";
        #endregion

        protected BasicCTSPageInfo _basicCTSPageInfo = null;

        protected bool hasInvalidSearchParam;

        protected QueryFieldsSetByUser _setFields = QueryFieldsSetByUser.None;
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
        protected string APIURL {
            get {
                if (String.IsNullOrWhiteSpace(_APIURL))
                {
                    this._APIURL = BasicClinicalTrialSearchAPISection.GetAPIUrl();
                }

                return this._APIURL;
            }
        }


        protected BaseCTSSearchParam GetSearchParams()
        {
            //Parse Parameters
            int pageNum = this.ParamAsInt(PAGENUM_PARAM, 1);
            int itemsPerPage = this.ParamAsInt(ITEMSPP_PARAM, BasicCTSPageInfo.DefaultItemsPerPage);
            string phrase = this.ParamAsStr(PHRASE_PARAM);
            string zip = this.ParamAsStr(ZIP_PARAM);
            int zipProximity = this.ParamAsInt(ZIPPROX_PARAM, BasicCTSPageInfo.DefaultZipProximity); //In miles
            int age = this.ParamAsInt(AGE_PARAM, 0);
            int gender = this.ParamAsInt(GENDER_PARAM, 0); //0 = decline, 1 = female, 2 = male, 
            string cancerType = this.ParamAsStr(CANCERTYPE_PARAM);
            string cancerTypeAsPhrase = this.ParamAsStr(CANCERTYPEASPHRASE_PARAM); // if autosuggest is broken, the cancer type field will be parsed as a phrase search
            string cancerTypeDisplayName = null;
            string cancerSubtype = this.ParamAsStr(CANCERTYPE_SUBTYPE);
            string cancerStage = this.ParamAsStr(CANCERTYPE_STAGE);
            string cancerFindings = this.ParamAsStr(CANCERTYPE_FINDINGS);

            string country = ParamAsStr(LOCATION_COUNTRY);
            string city = ParamAsStr(LOCATION_CITY);
            string state = ParamAsStr(LOCATION_STATE);
            string hospital = ParamAsStr(HOSPITAL_INSTITUTION);

            bool atNIH_isSet = false;
            bool atNIH = ParamAsBool(AT_NIH, ref atNIH_isSet);

            bool newTrialsOnly_IsSet = false;
            bool newTrialsOnly = ParamAsBool(NEW_TRIALS_ONLY, ref newTrialsOnly_IsSet);

            string trialType = ParamAsStr(TRIAL_TYPE);
            string drugIDs = ParamAsStr(DRUG_CODE);
            string drugName = ParamAsStr(DRUG_NAME);
            string treatmentCodes = ParamAsStr(TREATMENT_CODE);
            string treatmentName = ParamAsStr(TREATMENT_NAME);
            // "phase"
            string trialPhase = ParamAsStr(TRIAL_PHASE);
            string trialIDs = ParamAsStr(TRIAL_IDS);
            // "principal_investigator"
            string principalInvestigator = ParamAsStr(TRIAL_INVESTIGATOR);
            // "lead_org_"
            string leadOrganization = ParamAsStr(LEAD_ORGANIZATION);

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

                        _setFields |= QueryFieldsSetByUser.CancerType;
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
                    _setFields |= QueryFieldsSetByUser.Phrase;
                }
            }

            // cancerTypeDisplayName = _basicCTSManager.GetCancerTypeDisplayName(diseaseIDs, termKey);
            // Set query field for cancer subtype if exists
            //searchParams.CancerSubtype = _basicCTSManager.GetCancerTypeDisplayName(cancerSubtype, cancerSubtype);
            searchParams.CancerSubtype = GetDisplayNameFromCCode(cancerSubtype);
            if (!String.IsNullOrEmpty(searchParams.CancerSubtype))
            {
                _setFields |= QueryFieldsSetByUser.CancerSubtype;
            }

            // Set query field for cancer stage if exists
            searchParams.CancerStage = GetDisplayNameFromCCode(cancerStage);
            if (!String.IsNullOrEmpty(searchParams.CancerStage))
            {
                _setFields |= QueryFieldsSetByUser.CancerStage;
            }

            // Set query field for cancer findings if exists
            searchParams.CancerFindings = GetDisplayNameFromCCode(cancerFindings);
            if (!String.IsNullOrEmpty(searchParams.CancerFindings))
            {
                _setFields |= QueryFieldsSetByUser.CancerFindings;
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
                    
                    if(searchParams.ZipLookup == null)
                    {
                        hasInvalidSearchParam = true;
                    }
                }
                else
                {
                    hasInvalidSearchParam = true;
                }
            }
            else if (atNIH)
            {
                searchParams.ZipLookup = _basicCTSManager.GetZipLookupForZip(NIH_ZIP_CODE);
            }
            if (searchParams.ZipLookup != null)
            {
                _setFields |= QueryFieldsSetByUser.ZipCode;
                if (zipProximity != BasicCTSPageInfo.DefaultZipProximity)
                {
                    searchParams.ZipRadius = zipProximity;
                    _setFields |= QueryFieldsSetByUser.ZipProximity;
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
                    _setFields |= QueryFieldsSetByUser.Age;
                }
            }

            #endregion

            #region Set Trial phase
            trialPhase = trialPhase.ToUpper();
            searchParams.TrialPhase = trialPhase.Equals("ALL") ? "" : trialPhase;
            if (!String.IsNullOrWhiteSpace(trialPhase))
            {
                var phase = trialPhase.ToUpper();
                var phaseArray = phase.Split(',');
                var allPhasesSelected = false;
                HashSet<String> phaseRange = new HashSet<String>();


                foreach (var p in phaseArray)
                {                       
                    // phases are searched based on the following rules
                    // Phase 1 finds trials in phase 1 or 1-2
                    // Phase 2 finds trials in phase 1-2, 2, or 2-3
                    // Phase 3 finds trials in phase 2-3, or 3
                    // Phase 4 finds trials in phase 4
                    switch (p)
                    {
                        case "I":
                            phaseRange.Add("I");
                            phaseRange.Add("I_II");
                            break;
                        case "II":
                            phaseRange.Add("I_II");
                            phaseRange.Add("II");
                            phaseRange.Add("II_III");
                            break;
                        case "III":
                            phaseRange.Add("II_III");
                            phaseRange.Add("III");
                            break;
                        case "IV":
                            phaseRange.Add("IV");
                            break;
                        case "ALL":
                            allPhasesSelected = true;
                            break;
                        default:
                            hasInvalidSearchParam = true;
                            break;
                    }
                }
                
                var range = phaseRange.ToArray();
                searchParams.TrialPhaseArray = range.Length > 1 ? range : null;
                // If all phases are selected, this is the equivalent of not having this search paramin use.
                if(!allPhasesSelected)
                    _setFields |= QueryFieldsSetByUser.TrialPhase;
            }

            #endregion

            // Set advanced CTS properties.
            searchParams.Country = country;
            if (!String.IsNullOrEmpty(searchParams.Country))
            {                
                _setFields |= QueryFieldsSetByUser.Country;
            }

            searchParams.City = city;
            if (!String.IsNullOrEmpty(searchParams.City))            
                _setFields |= QueryFieldsSetByUser.City;
            

            searchParams.State = state;
            if (!String.IsNullOrEmpty(searchParams.State))             
                _setFields |= QueryFieldsSetByUser.State;
            

            searchParams.HospitalOrInstitution = hospital;
            if (!String.IsNullOrEmpty(searchParams.HospitalOrInstitution))
                _setFields |= QueryFieldsSetByUser.Hospital;
            

            searchParams.AtNIH = atNIH;
            if (atNIH_isSet)
            {
                // This property is neded to check within the velocity template if this is used.
                searchParams.AtNIH_IsSet = true; 

                _setFields |= QueryFieldsSetByUser.AtNIH;
            }

            // If the value is "all" then the parameter is essentially not used
            trialType = trialType.ToUpper();
            searchParams.TrialType = trialType.Equals("ALL") ? "" : trialType;
            searchParams.TrialTypeArray = !String.IsNullOrEmpty(searchParams.TrialType) ? trialType.Split(',') : null;
            if (trialType != null)
                _setFields |= QueryFieldsSetByUser.TrialType;

            if (!String.IsNullOrEmpty(drugIDs))
            {
                string[] idArray = drugIDs.Split(',').Select(id => id.Trim()).ToArray();
                searchParams.DrugIDs = idArray;
                _setFields |= QueryFieldsSetByUser.DrugCode;
            }               
            

            searchParams.DrugName = drugName;
            if (!String.IsNullOrEmpty(searchParams.DrugName))
                _setFields |= QueryFieldsSetByUser.DrugName;


            if (!String.IsNullOrEmpty(treatmentCodes))
            {
                string[] idArray = treatmentCodes.Split(',').Select(id => id.Trim()).ToArray();
                searchParams.TreatmentInterventionCodes = idArray;
                _setFields |= QueryFieldsSetByUser.TreatmentCode;
            }  
            

            searchParams.TreatmentInterventionTerm = treatmentName;
            if (!String.IsNullOrEmpty(searchParams.TreatmentInterventionTerm))
                _setFields |= QueryFieldsSetByUser.TreatmentName;
            

            searchParams.NewTrialsOnly = newTrialsOnly;
            if (newTrialsOnly_IsSet)
            {
                // This property is needed to check within the velocity template if this is used.
                searchParams.NewTrialsOnly_IsSet = true;

                _setFields |= QueryFieldsSetByUser.NewTrialsOnly;
            }

            searchParams.PrincipalInvestigator = principalInvestigator;
            if (!String.IsNullOrEmpty(searchParams.PrincipalInvestigator))
                _setFields |= QueryFieldsSetByUser.TrialInvestigator;

            searchParams.LeadOrganization = leadOrganization;
            if (!String.IsNullOrEmpty(searchParams.LeadOrganization))
                _setFields |= QueryFieldsSetByUser.LeadOrganization;


            if (!String.IsNullOrEmpty(trialIDs))
            {
                string[] idArray = trialIDs.Split(',').Select(id => id.Trim()).ToArray();
                searchParams.TrialIDs = idArray;
                _setFields |= QueryFieldsSetByUser.TrialIDs;
            }
            
            
            

            #region Set Gender

            //Handle Gender if specified
            switch (gender)
            {
                case 1:
                    searchParams.Gender = BaseCTSSearchParam.GENDER_FEMALE;
                    _setFields |= QueryFieldsSetByUser.Gender;
                    break;
                case 2:
                    searchParams.Gender = BaseCTSSearchParam.GENDER_MALE;
                    _setFields |= QueryFieldsSetByUser.Gender;
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

            string cancerTypeID = this.ParamAsStr(CANCERTYPE_PARAM);

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
        /// Returns pretty display name for CXXXX value.
        /// </summary>
        /// <param name="ccode"></param>
        /// <returns></returns>
        protected String GetDisplayNameFromCCode(string ccode)
        {
            string prettyDisplayName = string.Empty;
            // The cancerType/subtype/stage/findings param may not always contain a pipe. If it does, split the param and key into an array.
            // Otherwise, make the whole parameter string the first item in an array 
            String[] ctarr = (ccode.Contains("|") ? ccode.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries) : new string[] { ccode });
            {
                //split up the disease ids
                string[] diseaseIDs = ctarr[0].Split(',');

                // Determine display name from CIDs and key (if there is no match with the key,
                // then first term with matching ids is used)
                string termKey = ctarr.Length > 1 ? ctarr[1] : null;
                prettyDisplayName = _basicCTSManager.GetCancerTypeDisplayName(diseaseIDs, termKey);
            }

            return prettyDisplayName;
        }


        /// <summary>
        /// Gets a query parameter as a string
        /// </summary>
        /// <param name="param"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        protected string ParamAsStr(string param)
        {
            string paramVal = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramVal))
                return String.Empty;
            else
                return paramVal.Trim();
        }

        // Returns the bool param value or false if not present or invalid
        protected bool ParamAsBool(string param, ref bool propertyIsUsed)
        {
            string paramVal = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramVal))
            {
                propertyIsUsed = false;
                return false;
            }
            else
            {
                propertyIsUsed = true;
                bool tempVal = false;
                if (bool.TryParse(paramVal.Trim(), out tempVal))
                {
                    return tempVal;
                }
                else
                {
                    hasInvalidSearchParam = true;
                    return false;
                }
            }
        }

        protected int ParamAsInt(string param, int def)
        {
            string paramVal = Request.QueryString[param];

            if (string.IsNullOrWhiteSpace(paramVal))
            {
                return def;
            }
            else
            {
                int tmpInt = 0;
                if (int.TryParse(paramVal.Trim(), out tmpInt))
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
