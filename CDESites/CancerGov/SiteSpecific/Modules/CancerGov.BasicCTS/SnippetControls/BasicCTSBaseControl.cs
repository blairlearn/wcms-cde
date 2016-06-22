using System;
using System.Text.RegularExpressions;
using NCI.Logging;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSBaseControl : SnippetControl
    {
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


        protected BasicCTSPageInfo _basicCTSPageInfo = null;
        public bool invalidSearchParam = false;

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
            string cancerTypeDisplayName = null;

            BaseCTSSearchParam searchParams = null;

            #region Set Cancer Type or Phrase

            if (cancerType != string.Empty)
            {
                //cancerTypeIDAndHash = cancerType;
                string[] ctarr = cancerType.Split(new Char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                if (ctarr.Length >= 1)
                {
                    if (ctarr.Length > 1)
                        cancerTypeDisplayName = _basicCTSManager.GetCancerTypeDisplayName(ctarr[0], ctarr[1]);
                    else if (ctarr.Length == 1)
                        cancerTypeDisplayName = _basicCTSManager.GetCancerTypeDisplayName(ctarr[0], null);

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
                        invalidSearchParam = true;
                        searchParams = new CancerTypeSearchParam()
                        {
                            ESTemplateFile = BasicCTSPageInfo.ESTemplateCancerType
                        };
                    }


                }

            }
            else
            {
                searchParams = new PhraseSearchParam()
                {
                    Phrase = phrase,
                    ESTemplateFile = BasicCTSPageInfo.ESTemplateFullText
                };

                if (!string.IsNullOrWhiteSpace(phrase))
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
                        invalidSearchParam = true;
                    }
                }
                else
                {
                    invalidSearchParam = true;
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
                    invalidSearchParam = true;
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
                        invalidSearchParam = true;

                    return tmpInt;
                }
                else
                {
                    invalidSearchParam = true;
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
                    NCI.Logging.Logger.LogError("BasicCTSBaseControl", "could not load the BasicCTSPageInfo, check the config info of the application module in percussion", NCIErrorLevel.Error, ex);
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
