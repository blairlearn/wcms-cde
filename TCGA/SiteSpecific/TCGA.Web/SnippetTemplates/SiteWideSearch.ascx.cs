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
        private string _keyword = string.Empty;
        private int _currentPage = 1;
        private int _offSet = 0;
        private int _recordsPerPage = 10;
        private bool _didDDLChange = false;

        private const string swKeywordQuery = "swKeywordQuery";
        private const string swKeyword = "swKeyword";

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Get Settings
            if (Request.RequestType == "POST")
            {
                //If it is a post then we take the defaults.
                if (_didDDLChange)
                {
                    //We are changing options this is a true postback.
                    //So we get the keyword from the view state
                    _keyword = (string)ViewState[swKeyword];
                }
                else
                {
                    _keyword = Strings.Clean(Request.Params[swKeyword]);
                }
            }
            else
            {
                //The method was a GET, therfore we must be paging.
                _keyword = Strings.Clean(Request.Params[swKeywordQuery]);
                _currentPage = Strings.ToInt(Request.Params["PageNum"], 1);
                _recordsPerPage = Strings.ToInt(Request.Params["RecordsPerPage"], 10);
                _offSet = Strings.ToInt(Request.Params["OffSet"], 0);
            }

            lblSearchTerm.Text = _keyword;

            //Set Page Unit selected item
            ddlPageUnit.Text = _recordsPerPage.ToString();

            long dimFilter = Strings.ToLong(ConfigurationManager.AppSettings["EndecaSWSearchDimFilter"], 0);

            if (_keyword != null)
            {
                //Store keyword in viewstate (This does not check if it is not there already)
                if (ViewState[swKeyword] == null)
                    ViewState.Add(swKeyword, _keyword);
                else
                    ViewState[swKeyword] = _keyword;

                ISiteWideSearchResultCollection results = GenericSiteWideSearchManager.GetSearchResults(_keyword, _currentPage, _recordsPerPage, dimFilter);

                rptSearchResults.DataSource = results;
                rptSearchResults.DataBind();

                if (results.TotalNumResults == 0)
                {
                    lblResults.Text = "No results found";
                    tblPager.Visible = false;
                    rptSearchResults.Visible = false;
                }
                else
                {
                    int startRecord = 0;
                    int endRecord = 0;

                    SimplePager.GetFirstItemLastItem(_currentPage, _recordsPerPage, (int)results.TotalNumResults, out startRecord, out endRecord);

                    //phNoResultsLabel.Visible = false;
                    phResultsLabel.Visible = true;
                    rptSearchResults.Visible = true;
                    tblPager.Visible = true;

                    //long startRecord = _offSet + 1;
                    //long endRecord = _offSet + _recordsPerPage;
                    //if (endRecord > results.TotalNumResults)
                    //{
                    //    endRecord = results.TotalNumResults;
                    //}

                    string resultsCount = String.Format("{0}-{1} of {2}", startRecord.ToString(), endRecord.ToString(), results.TotalNumResults.ToString());
                    lblResults.Text = "Results " + resultsCount;
                    lblResultsBottom.Text = resultsCount;
                }
                //"Results 1-10 of 1789"

                spPager.RecordCount = (int)results.TotalNumResults;
                spPager.RecordsPerPage = _recordsPerPage;
                spPager.CurrentPage = _currentPage;

                spPager.BaseUrl = "results.aspx" + "?swKeywordQuery=" + _keyword;
            }
            else
            {
                lblResults.Text = "No results found";
                lblResults.Visible = true;
                tblPager.Visible = false;
                rptSearchResults.Visible = false;
                phResultsLabel.Visible = true;
            }

        }

        protected void ddlPageUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If we change then set page to page 1
            _recordsPerPage = Strings.ToInt(ddlPageUnit.SelectedValue);
            _currentPage = 1;
            _offSet = 0;
            _didDDLChange = true;
        }

    }
}