using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using NCI.Web.CDE.UI;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE;
using NCI.Web;

namespace CancerGov.ClinicalTrials.Basic.SnippetControls
{
    public class BasicCTSResultsControl : BasicCTSBaseControl
    {
        /// <summary>
        /// Enumeration representing a bitmap for the fields that are set.
        /// </summary>
        [Flags]
        private enum SetFields
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
        /// Gets the Search Parameters for the current request.
        /// </summary>
        public BaseCTSSearchParam SearchParams { get; private set; }
        public PhraseSearchParam PhraseSearchParams { get; set; }
        public CancerTypeSearchParam CancerTypeSearchParams { get; set; }

        private SetFields _setFields = SetFields.None;
        private BasicCTSManager _basicCTSManager = null;

        private void SetSearchParams()
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
                string[] ctarr = cancerType.Split(new Char[]{'|'}, StringSplitOptions.RemoveEmptyEntries);

                if (ctarr.Length >= 1)
                {
                    if(ctarr.Length > 1)    
                        cancerTypeDisplayName = _basicCTSManager.GetCancerTypeDisplayName(ctarr[0], ctarr[1]);


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

            #endregion

            #region Set Page and Items Per Page
            searchParams.Page = pageNum;
            searchParams.ItemsPerPage = itemsPerPage;
            #endregion

            #region Set Age

            //Handle Age
            if (age > 0)
            {
                searchParams.Age = age;
                _setFields |= SetFields.Age;
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

            SearchParams = searchParams;
        }

        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _basicCTSManager = new BasicCTSManager();

            SetSearchParams();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            //Do the search
            var results = _basicCTSManager.SearchTemplate(SearchParams);

            // Copying the Title & Short Title logic from Advanced Form
            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                data.Value = "Results of Your Search";
 
                if (results.TotalResults == 0)
                {
                    data.Value = "No Trials Matched Your Search";
                }
                else if (invalidSearchParam)
                {
                    data.Value = "No Results";
                }
            });

            // Show Results

            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                BasicCTSPageInfo.ResultsPageTemplatePath, 
                new
                {
                    Results = results,
                    Control = this
                }
            ));
            Controls.Add(ltl);
        }

        #region Velocity Helpers

        /// <summary>
        /// Gets the Starting Number for the Results Being Displayed
        /// </summary>
        /// <returns></returns>
        public string GetStartItemNum()
        {            
            return (((SearchParams.Page - 1) * SearchParams.ItemsPerPage) + 1).ToString();
        }

        /// <summary>
        /// Gets the ending number for the results being displayed
        /// </summary>
        /// <param name="totalResults"></param>
        /// <returns></returns>
        public string GetEndItemNum(long totalResults)
        {
            long possibleLast = (SearchParams.Page * SearchParams.ItemsPerPage);
            if (possibleLast > totalResults)
                return totalResults.ToString();
            else
                return possibleLast.ToString();
        }

        public string GetParamsList()
        {
            List<string> plist = new List<string>();

            if (SearchParams is CancerTypeSearchParam)
            {
                CancerTypeSearchParams = (CancerTypeSearchParam)SearchParams;

                if (!string.IsNullOrWhiteSpace(CancerTypeSearchParams.CancerTypeDisplayName))
                    plist.Add("Type/Condition \"" + CancerTypeSearchParams.CancerTypeDisplayName + "\"");
            }

            if (SearchParams is PhraseSearchParam)
            {
                PhraseSearchParams = (PhraseSearchParam)SearchParams;
                if (!string.IsNullOrWhiteSpace(PhraseSearchParams.Phrase))
                    plist.Add("Keyword \"" + PhraseSearchParams.Phrase + "\"");
            }

            if (SearchParams.Age != null && SearchParams.Age > 0)
                plist.Add("Age \"" + SearchParams.Age + "\"");

            if (!string.IsNullOrWhiteSpace(SearchParams.Gender))
                plist.Add("Gender \"" + SearchParams.Gender + "\"");

            if (HasZip())
                plist.Add("ZIP \"" + SearchParams.ZipLookup.PostalCode_ZIP + "\"");

            if ((this.invalidSearchParam == false) && (_setFields == SetFields.None))
            {
                return "all trials\"";
            }

            return string.Join(", ", plist);
        }

        public bool HasInvalidParams()
        {
            return this.invalidSearchParam;
        }

        /// <summary>
        /// Determines if the current search has a Zip or not.
        /// </summary>
        /// <returns></returns>
        public bool HasZip()
        {
            return SearchParams.ZipLookup != null;
        }

        /// <summary>
        /// Gets the View URL for an ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDetailedViewUrl(string id)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(BasicCTSPageInfo.DetailedViewPagePrettyUrl);

            url.QueryParameters.Add("id", id);

            if ((_setFields & SetFields.ZipCode) != 0)
                url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

            if ((_setFields & SetFields.ZipProximity) != 0)
                url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());

            return url.ToString();
        }

        /// <summary>
        /// Gets URL for a Single Page of Results
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public string GetPageUrl(int pageNum)
        {
            NciUrl url = this.PageInstruction.GetUrl("CurrentURL");
            url.QueryParameters.Add("pn", pageNum.ToString());

            if ((_setFields & SetFields.Age) != 0)
                url.QueryParameters.Add(AGE_PARAM, SearchParams.Age.ToString());

            if ((_setFields & SetFields.Gender) != 0)
            {
                if (SearchParams.Gender == BaseCTSSearchParam.GENDER_FEMALE)
                    url.QueryParameters.Add(GENDER_PARAM, "1");
                else if (SearchParams.Gender == BaseCTSSearchParam.GENDER_MALE)
                    url.QueryParameters.Add(GENDER_PARAM, "2");
            }            
            
            if ((_setFields & SetFields.ZipCode) != 0)
                url.QueryParameters.Add(ZIP_PARAM, SearchParams.ZipLookup.PostalCode_ZIP);

            if ((_setFields & SetFields.ZipProximity) != 0)
                url.QueryParameters.Add(ZIPPROX_PARAM, SearchParams.ZipRadius.ToString());


            //Phrase and type are based on the type of object

            //Items Per Page

            return url.ToString();
        }

        /// <summary>
        /// Gets the Urls and Labels for all the pages of results from Curr Page - numLeft to Curr Page + numRight
        /// </summary>
        /// <param name="numLeft">The number of pages to display left of the selected page</param>
        /// <param name="numRight">The number of pages to display to the right of the selected page</param>
        /// <param name="totalResults">The total number of results </param>
        /// <returns></returns>
        public IEnumerable<object> GetPagerItems(int numLeft, int numRight, long totalResults)
        {
            int startPage = (SearchParams.Page - numLeft) >= 1 ? SearchParams.Page - numLeft : 1;
            int maxPage = (int)Math.Ceiling((double)(totalResults / SearchParams.ItemsPerPage));
            int endPage = (SearchParams.Page + numRight) <= maxPage ? SearchParams.Page + numRight : maxPage;

            List<object> items = new List<object>();

            if (SearchParams.Page != 1)
                items.Add(
                    new
                    {
                        Text = "&lt; Previous",
                        PageUrl = GetPageUrl(SearchParams.Page - 1)
                    });

            for (int i = startPage; i <= endPage; i++)
            {
                items.Add(
                    new {
                        Text = i.ToString(),
                        PageUrl = GetPageUrl(i)
                    }
                );
            }

            if (SearchParams.Page != endPage)
                items.Add(
                    new
                    {
                        Text = "Next &gt;",
                        PageUrl = GetPageUrl(SearchParams.Page + 1)
                    });



            return items;
        }

        #endregion

    }
}
