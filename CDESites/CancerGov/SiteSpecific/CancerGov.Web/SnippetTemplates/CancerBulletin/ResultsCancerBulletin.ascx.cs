using System;
using System.Collections;
using System.Configuration;
using System.Web.UI;
using CancerGov.Common;
using NCI.Logging;
using NCI.Search.Endeca;
using NCI.Util;
using NCI.Web.CDE.WebAnalytics;
using NCI.Search;

namespace CancerGov.Web.SnippetTemplates.CancerBulletin
{
    /// <summary>
    /// Summary description for ResultsCancerBulletin.
    /// </summary>
    public partial class ResultsCancerBulletin : SearchBaseUserControl
    {
        protected ArrayList eSearchResults = new ArrayList();

        private string pager;
        private string keyword;
        private int firstRecord = 0;
        private int recordIndex = 0;
        private int lastRecord = 0;
        private int totalItems = 0;
        private string resultsHtml = "";
        private string dateLabel = "";


        #region Properties
        public string Pager
        {
            get { return pager; }
            set { pager = value; }
        }

        public string Keyword
        {
            get { return keyword; }
        }

        public int FirstRecord
        {
            get { return firstRecord; }
        }

        public int LastRecord
        {
            get { return lastRecord; }
        }

        public string WebAnalytics
        {
            get
            {
                if (WebAnalyticsOptions.IsEnabled)
                    return "NCIAnalytics.SearchResults(this, " + (recordIndex++).ToString() + ");";
                else
                    return "";
            }
        }
        public int TotalItems
        {
            get { return totalItems; }
        }

        public string DateLabel
        {
            get { return dateLabel; }
        }

        public string ResultsHtml
        {
            get { return resultsHtml; }
        }

        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string searchType = Strings.Clean(Request.Params["cbsubmit"]);
            int startMonth = Strings.ToInt(Strings.Clean(Request.Params["startMonth"]));
            int startYear = Strings.ToInt(Strings.Clean(Request.Params["startYear"]));
            int endMonth = Strings.ToInt(Strings.Clean(Request.Params["endMonth"]));
            int endYear = Strings.ToInt(Strings.Clean(Request.Params["endYear"]));
            keyword = Strings.Clean(Request.Params["cbkeyword"]);
            int currentPage = Strings.ToInt(Strings.Clean(Request.Params["page"]));

            //Updating this logic for SCR 30608 where a previous release introduced an
            //error where when the initial post for a date range search works, but on
            //subsequent searches bSearchRange is always false. (I.E. forgets that a
            //date range search is happening.)
            bool bSearchRange = false;

            if (Page.Request.HttpMethod == "POST")
            {
                bSearchRange = (Request.Params["searchRangeButton.x"] != null);
            }
            else
            {
                bSearchRange = (searchType != null && string.Compare(searchType, "range", true) == 0);
            }
           
            
                                
            firstRecord = 0;
            lastRecord = 0;
            totalItems = 0;

            if (currentPage < 0)
                currentPage = 1;
            int pageSize = 10;

            if (keyword == null)
            {
                resultsHtml = "<br><br>Please enter a search phrase.<br><br><br>";
            }
            else
            {

                firstRecord = (currentPage - 1) * pageSize + 1;
                recordIndex = firstRecord;

                //Get Endeca Search Results
                string doc_type = ConfigurationSettings.AppSettings["EndecaCancerBulletinDocs"];
                //CancerGov.Search.EndecaSearching.EndecaSearch eSearch;
                EndecaSearch eSearch;
                if (bSearchRange)
                {
                    if (startMonth < 0 || startMonth > 12 || endMonth < 0 || endMonth > 12 || startYear < 0 || endYear < 0)
                    {
                        NCI.Logging.Logger.LogError("Invalid date range parameters","ResultCancerBulletin", NCIErrorLevel.Error);
                        this.RaiseErrorPage("Invalid date range parameters");
                    }

                    string startRange = getTimeStamp(startYear, startMonth, 1);
                    int endLastDay = getLastDayOfTheMonth(endMonth, endYear);
                    string endRange = getTimeStamp(endYear, endMonth, endLastDay);

                    string startDate = startMonth.ToString() + "/1/" + startYear.ToString();
                    string endDate = endMonth.ToString() + "/" + endLastDay.ToString() + "/" + endYear.ToString();

                    dateLabel = "<b>for items between</b> \"" + startDate + "\" <b>and</b> \"" + endDate + "\"";

                    eSearch = new EndecaSearch(keyword, pageSize, firstRecord - 1, startRange, endRange, doc_type);
                }
                else
                {
                    eSearch = new EndecaSearch(keyword, pageSize, firstRecord - 1, null, null, doc_type);
                }

                try
                {
                    eSearch.ExecuteSearch();
                }
                catch (Exception ex)
                {
                    NCI.Logging.Logger.LogError( "ResultsCancerBulletin" ,PrettyUrl, NCIErrorLevel.Error, ex);
                    this.RaiseErrorPage();
                }

                
                eSearch.FillSearchResults(eSearchResults);

                if (eSearchResults.Count != 0)
                {

                    lastRecord = firstRecord + eSearchResults.Count - 1;
                    totalItems = (int)eSearch.TotalSearchResults;

                    ResultRepeater.DataSource = eSearchResults;
                    ResultRepeater.DataBind();

                    //pager code
                    string urlFormat;
                    string pagerUrl;
                    if (bSearchRange)
                    {
                        urlFormat = PrettyUrl + "?cbsubmit=range&cbkeyword={0}&startMonth={1}&startYear={2}&endMonth={3}&endYear={4}";
                        pagerUrl = String.Format(urlFormat, Server.UrlEncode(keyword), startMonth.ToString(), startYear.ToString(), endMonth.ToString(), endYear.ToString());
                    }
                    else
                    {
                        urlFormat = PrettyUrl + "?cbkeyword={0}";
                        pagerUrl = String.Format(urlFormat, Server.UrlEncode(keyword));
                    }

                    ResultPager objPager = new ResultPager(pagerUrl, currentPage, pageSize, 2, totalItems);
                    pager = objPager.RenderPager();

                }
                else
                {
                    firstRecord = 0;
                }
            }
            
            this.PageInstruction.AddFieldFilter("channelName", (fieldName, data) =>
            {
                data.Value = "Bulletin";
            });

            
        }

        private string getTimeStamp(int year, int month, int day)
        {
            string tm = year.ToString();
            if (month <= 9)
                tm = tm + "0" + month.ToString();
            else
                tm = tm + month.ToString();
            if (day <= 9)
                tm = tm + "0" + day.ToString();
            else
                tm = tm + day.ToString();
            return tm;
        }

        private int getLastDayOfTheMonth(int month, int year)
        {

            int nextMonth = month + 1;
            int nextYear = year;

            if (month == 12)
            {
                nextMonth = 1;
                nextYear = year + 1;
            }
            DateTime d1 = new DateTime(nextYear, (nextMonth), 1);
            DateTime d2 = new DateTime(year, month, 1);
            TimeSpan ts = d1 - d2;
            return ts.Days;

        } 
    }
}