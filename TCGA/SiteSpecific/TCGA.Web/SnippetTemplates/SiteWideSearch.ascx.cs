using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using TCGA.Apps;
using NCI.Web.CDE;
using NCI.Web.UI.WebControls;
using NCI.Logging;
using NCI.Util;
using NCI.Search.Endeca;
using NCI.Search;

namespace TCGA.Web.SnippetTemplates
{
    public partial class SiteWideSearch : AppsBaseUserControl
    {
        private int _currentPage = 1;
        private int _offSet = 0;
        private int _recordsPerPage = 10;
        private bool _didDDLChange = false;
        private string _results = string.Empty;
        private const string swKeywordQuery = "swKeywordQuery";
        private const string swKeyword = "swKeyword";
        private bool _resultsFound = false;

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Get Settings
            if (Request.RequestType == "POST")
            {
            }
            else
            {
                //The method was a GET, therfore we must be paging.
                _currentPage = Strings.ToInt(Request.Params["PageNum"], 1);
                _recordsPerPage = Strings.ToInt(Request.Params["RecordsPerPage"], 10);
                _offSet = Strings.ToInt(Request.Params["OffSet"], 0);
            }

            long dimFilter = Strings.ToLong(ConfigurationManager.AppSettings["EndecaSWSearchDimFilter"], 0);

            if (PerformSearch)
            {
                if (Keyword != null)
                {
                    //Store keyword in viewstate (This does not check if it is not there already)
                    if (ViewState[swKeyword] == null)
                        ViewState.Add(swKeyword, Keyword);
                    else
                        ViewState[swKeyword] = Keyword;

                    ISiteWideSearchResultCollection results = GenericSiteWideSearchManager.GetSearchResults(Keyword, _currentPage, _recordsPerPage, dimFilter);

                    rptSearchResults.DataSource = results;
                    rptSearchResults.DataBind();

                    if (results.TotalNumResults == 0)
                    {
                        ResultsText = "No results found";
                        rptSearchResults.Visible = false;
                    }
                    else
                    {
                        int startRecord = 0;
                        int endRecord = 0;
                        _resultsFound = true;
                        SimplePager.GetFirstItemLastItem(_currentPage, _recordsPerPage, (int)results.TotalNumResults, out startRecord, out endRecord);

                        //phNoResultsLabel.Visible = false;
                        rptSearchResults.Visible = true;
                        string resultsCount = String.Format("{0}-{1} of {2}", startRecord.ToString(), endRecord.ToString(), results.TotalNumResults.ToString());
                        ResultsText = "Results " + resultsCount;
                    }

                    spPager.RecordCount = (int)results.TotalNumResults;
                    spPager.RecordsPerPage = _recordsPerPage;
                    spPager.CurrentPage = _currentPage;
                    spPager.BaseUrl = PrettyUrl + "?swKeywordQuery=" + Keyword;
                }
                else
                {
                    ResultsText = "No results found";
                    rptSearchResults.Visible = false;
                    spPager.Visible = false;
                }
            }
            else
            {
                ResultsText = String.Empty;
            }
        }

        protected void ddlPageUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If we change then set page to page 1
            _recordsPerPage = 10;
            _currentPage = 1;
            _offSet = 0;
            _didDDLChange = true;
        }

        protected string ResultsText
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
            }
        }

        protected bool PerformSearch
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.Params["PageNum"]) ||
                    !string.IsNullOrEmpty(Strings.Clean(Request.Params["swKeywordQuery"])) ||
                    IsPostBack
                    )
                    return true;
                return false;
            }
        }

        protected bool ResultsFound
        {
            get { return _resultsFound; }
        }

        protected string Keyword
        {
            get
            {
                string keyword = Strings.Clean(Request.Params["txtKeyword1"]);
                if (string.IsNullOrEmpty(keyword))
                    keyword = Strings.Clean(Request.Params["swKeywordQuery"]);
                return keyword;
            }
        }

        /// <summary>
        /// Number of items that will be displayed per page.
        /// </summary>
        protected int ItemsPerPage
        {
            get
            {
                if (itemsPerPage == null)
                    return 10;
                return Int32.Parse(itemsPerPage.Value);
            }
        }

    }
}