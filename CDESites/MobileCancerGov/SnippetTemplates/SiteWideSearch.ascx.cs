using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using NCI.Logging;
using NCI.Util;
using NCI.Search.Endeca;
using NCI.Search;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.InformationRequest;
using NCI.Web.CDE.WebAnalytics;


namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class SiteWideSearch : SnippetControl
    {
        private int _currentPage = 1;
        private int _offSet = 0;
        private int _recordsPerPage = 10;
        private int _resultOffset = 1;
            
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

            if (PerformSearch)
            {
                if (Keyword != null)
                {
                    //Store keyword in viewstate (This does not check if it is not there already)
                    if (ViewState[swKeyword] == null)
                        ViewState.Add(swKeyword, Keyword);
                    else
                        ViewState[swKeyword] = Keyword;

                    try
                    {
                        long dimFilter = 0;

                        if( PageDisplayInformation.Language == DisplayLanguage.Spanish )
                            dimFilter = Strings.ToLong(ConfigurationManager.AppSettings["EndecaSpanishDocs"], 0);

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
                            _resultOffset = startRecord;

                            //phNoResultsLabel.Visible = false;
                            rptSearchResults.Visible = true;
                            string resultsCount = String.Format("{0}-{1} of {2}", startRecord.ToString(), endRecord.ToString(), results.TotalNumResults.ToString());
                            ResultsText = "Results " + resultsCount;
                        }

                        spPager.RecordCount = (int)results.TotalNumResults;
                        spPager.RecordsPerPage = _recordsPerPage;
                        spPager.CurrentPage = _currentPage;
                        spPager.BaseUrl = PrettyUrl + "?swKeywordQuery=" + Server.UrlEncode(Keyword);

                        if (PageDisplayInformation.Language == DisplayLanguage.Spanish)
                        {
                            spPager.PagerStyleSettings.NextPageText = "Siguiente&nbsp;&gt;";
                            spPager.PagerStyleSettings.PrevPageText = "&lt;&nbsp;Anterior";
                            lnkSearchInDeskTop.Text = "Buscar en la versión completa de Cancer.gov/español";
                        }


                        //// Web Analytics *************************************************
                        this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.NumberOfSearchResults, wbField =>
                        {
                            wbField.Value = results.TotalNumResults.ToString();
                        });

                        this.PageInstruction.AddFieldFilter("channelName", (name, data) =>
                        {
                            data.Value = "NCI Home";
                        });


                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("SiteWideSearch", NCIErrorLevel.Error, ex);
                    }

                    // Set the link for desktop search or full site search
                    string prettyUrl = ConfigurationManager.AppSettings["CGovEnglishSiteWideSearchResultPage"];

                    if (PageDisplayInformation.Language == DisplayLanguage.Spanish)
                        prettyUrl = ConfigurationManager.AppSettings["CGovSpanishSiteWideSearchResultPage"];

                    if (string.IsNullOrEmpty(prettyUrl))
                        prettyUrl = PrettyUrl;

                    lnkSearchInDeskTop.NavigateUrl = InformationRequestConfig.DesktopHost + prettyUrl + "?swKeyword=" + Server.UrlEncode(Keyword);
                    lnkSearchInDeskTop.Visible = true;
                }
                else
                {
                    _resultsFound = false;
                    ResultsText = "No results found";
                    rptSearchResults.Visible = false;
                }
            }
            else
            {
                ResultsText = String.Empty;
            }
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
                    !string.IsNullOrEmpty(Strings.Clean(Request.Params["swKeyword"])) ||
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

        protected string PrettyUrl
        {
            get
            {
                return this.PageInstruction.GetUrl("PrettyUrl").UriStem;
            }
        }

        protected string Keyword
        {
            get
            {
                string keyword = Strings.Clean(Server.HtmlDecode(Request.Params["swKeyword"]));
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

        protected string ResultsHyperlinkOnclick(RepeaterItem result)
        {
            if (WebAnalyticsOptions.IsEnabled)
                return "NCIAnalytics.SiteWideSearchResults(this," + "false" + ",'" + (result.ItemIndex + _resultOffset).ToString() + "');"; // Load results onclick script
            else
                return "";
        }

        protected DisplayInformation PageDisplayInformation
        {
            get
            {
                NCI.Web.CDE.DisplayInformation pageDisplayInformation = new NCI.Web.CDE.DisplayInformation();

                switch (PageInstruction.Language)
                {
                    case "en":
                        pageDisplayInformation.Language = DisplayLanguage.English;
                        break;
                    case "es":
                        pageDisplayInformation.Language = DisplayLanguage.Spanish;
                        break;
                    default:
                        pageDisplayInformation.Language = DisplayLanguage.English;
                        break;
                }

                pageDisplayInformation.Version = PageAssemblyContext.Current.DisplayVersion;

                return pageDisplayInformation;
            }

        }

        protected string LimitText(RepeaterItem item)
        {
            int numberOfCharacters = Strings.ToInt(ConfigurationManager.AppSettings["LimitTextChar"], 235);
            int MAX_WALKBACK = Strings.ToInt(ConfigurationManager.AppSettings["MaxWalkBack"], 30);

            string description = string.Empty;

            if (item.DataItem != null && DataBinder.Eval(item.DataItem, "Description") != null )
            {
                description = DataBinder.Eval(item.DataItem, "Description").ToString();

                if (description.Length > numberOfCharacters)
                {
                    if (description.Substring(numberOfCharacters - 1, 1) == " ")
                        description = description.Substring(0, numberOfCharacters - 1) + "...";
                    else //walk back to next space
                    {
                        int i;
                        for (i = numberOfCharacters; i > 0; i--)
                        {
                            if (i == MAX_WALKBACK)
                            {
                                i = 0;
                                break;
                            }
                            if (description.Substring(i, 1) == " ")
                                break;
                        }
                        if (i <= 0)
                            description = description.Substring(0, numberOfCharacters - 1) + "...";
                        else
                            description = description.Substring(0, i) + "...";
                    }
                }
            }
            return description;
        }
    }
}