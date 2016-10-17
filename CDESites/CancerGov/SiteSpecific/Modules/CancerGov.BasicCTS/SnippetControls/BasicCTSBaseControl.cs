using System;
using System.Text.RegularExpressions;
using Common.Logging;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSBaseControl : SnippetControl
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

        protected BasicCTSPageInfo _basicCTSPageInfo = null;

        protected bool hasInvalidSearchParam;

        protected SetFields _setFields = SetFields.None;
        protected BasicCTSManager _basicCTSManager = null;
        protected string cancerTypeIDAndHash = null;

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
                string[] ctarr = cancerType.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (ctarr.Length >= 1)
                {
                    // Determine cancer type display name from CDRID and Hash (if there is no match with the Hash,
                    // the first cancer type display name is chosen using the CDRID)
                    string hash = ctarr.Length >= 1 ? ctarr[1] : null;
                    cancerTypeDisplayName = _basicCTSManager.GetCancerTypeDisplayName(ctarr[0], hash);

                    if (cancerTypeDisplayName != null)
                    {
                        cancerTypeIDAndHash = cancerType;

                        //Test id to match ^CDR\d+$
                        searchParams = new CancerTypeSearchParam()
                        {
                            //get cancer type.
                            CancerTypeID = ctarr[0],

                            CancerTypeDisplayName = cancerTypeDisplayName,

                            //Add in the label which is go to ElasticSearch, fetch ctarr[1] (the hash) and get the text
                            ESTemplateFile = BasicCTSPageInfo.ESTemplateCancerType
                        };

                        _setFields |= SetFields.CancerType;
                    }
                    else
                    {
                        hasInvalidSearchParam = true;
                        searchParams = new CancerTypeSearchParam()
                        {
                            ESTemplateFile = BasicCTSPageInfo.ESTemplateCancerType
                        };
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
                    IsBrokenCTSearchParam = (string.IsNullOrWhiteSpace(phrase) && !string.IsNullOrWhiteSpace(cancerTypeAsPhrase)) ? true : false,
                    ESTemplateFile = BasicCTSPageInfo.ESTemplateFullText
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
                            _setFields |= SetFields.ZipProximity;
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

            _basicCTSManager = new BasicCTSManager();

        }
    }
}
