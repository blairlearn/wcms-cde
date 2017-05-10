using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NCI.Web.CancerGov.Apps;
using System.Data.SqlClient;
using CancerGov.Common;
using NCI.Web.CDE;
using NCI.Util;
using CancerGov.Common.ErrorHandling;
using NCI.Web.CDE.WebAnalytics;
using CancerGov.CDR.DataManager;
using NCI.Web.CDE.UI;
using System.Collections.Specialized;
using NCI.Search;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ResultsGeneticsServices : SearchBaseUserControl
    {
        protected class ResultsGeneticServicesPager
        {
            private int showPages = 10;
            private int currentPage = 0;
            private int recordsPerPage = 10;
            private int recordCount = 0;
            private string pageBaseUrlFormat = "javascript:page('{0}');";

            #region Properties

            /// <summary>
            /// Property sets index of current page view
            /// </summary>
            public int CurrentPage
            {
                get { return currentPage; }
                set { currentPage = value; }
            }

            /// <summary>
            /// Property sets number of records per page view
            /// </summary>
            public int RecordsPerPage
            {
                get { return recordsPerPage; }
                set { recordsPerPage = value; }
            }

            /// <summary>
            /// Property sets total number of records
            /// </summary>
            public int RecordCount
            {
                get { return recordCount; }
                set { recordCount = value; }
            }

            public int ShowPages
            {
                get { return showPages; }
                set { showPages = value; }
            }

            #endregion

            /// <summary>
            /// Default class constructor
            /// </summary>
            public ResultsGeneticServicesPager() { }

            public ResultsGeneticServicesPager(string pageBaseUrl, int pageIndex, int pageSize, int pageCount, int itemCount)
            {
                this.currentPage = pageIndex;
                this.recordCount = itemCount;
                this.recordsPerPage = pageSize;
                this.showPages = pageCount;
                this.pageBaseUrlFormat = pageBaseUrl + "&first={0}&page={1}";
            }

            /// <summary>
            /// Method that builds HTML paging constructs based on class properties
            /// </summary>
            /// <returns>Paging HTML links</returns>
            public string RenderPager()
            {
                string result = "";
                int startIndex = 0;
                int endIndex = 0;
                int pages = 0;

                //Get number of pages
                if (recordsPerPage > 0)
                {
                    pages = recordCount / recordsPerPage;
                    if (recordCount % recordsPerPage > 0)
                    {
                        pages += 1;
                    }
                }

                if (pages > 1)
                {
                    startIndex = currentPage - showPages > 0 ? currentPage - showPages : 1;
                    endIndex = currentPage + showPages > pages ? pages : currentPage + showPages;

                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        if (currentPage != i)
                        {
                            result += "<li><a href=\"" + String.Format(pageBaseUrlFormat, (((i - 1) * this.recordsPerPage) + 1).ToString(), i) + "\">" + i.ToString() + "</a></li>";
                        }
                        else
                        {
                            result += "<li class='current'>" + i.ToString() + "</li>";
                        }
                    }

                    if (currentPage > 1)
                    {
                        result = "<li class='previous'><a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage - 2) * this.recordsPerPage) + 1).ToString(), (currentPage - 1).ToString()) + "\">Previous</a></li>" + result;
                    }
                    if (currentPage < pages)
                    {
                        result += "<li class='next'><a href=\"" + String.Format(pageBaseUrlFormat, (((currentPage) * this.recordsPerPage) + 1).ToString(), (currentPage + 1).ToString()) + "\">Next</a></li>";
                    }

                    result = "<div class='pagination'><ul class='no-bullets'>" + result + "</ul></div>";
                }

                return result;
            }
        }
               
        private string resultLabel;
        private string resultCount;
        private string searchSummary;
        private int firstRec;
        private int lastRec;
        private int recordsPerPage = 10;

        #region Page properties

        /// <summary>
        /// Gets HTML phrase description search results
        /// </summary>
        public string ResultLabel
        {
            get { return resultLabel; }
            set { resultLabel = value; }
        }

        /// <summary>
        /// Gets the number of search results
        /// </summary>
        public string ResultCount
        {
            get { return resultCount; }
            set { resultCount = value; }
            }

        /// <summary>
        /// Gets the number of records displayed per page view
        /// </summary>
        public int RecordsPerPage
        {
            get { return recordsPerPage; }
            set { recordsPerPage = value; }
        }

        /// <summary>
        /// Gets the HTML source for the summary of search parameters
        /// </summary>
        public string SearchSummary
        {
            get { return searchSummary; }
            set { searchSummary = value; }
            }

        /// <summary>
        /// Gets the index of the first result displayed on the page view
        /// </summary>
        public int FirstRec
        {
            get { return firstRec; }
            set { firstRec = value; }
        }

        /// <summary>
        /// Gets the index of the last result displayed on the page view
        /// </summary>
        public int LastRec
        {
            get { return lastRec; }
            set { lastRec = value; }
        }

        #endregion

        #region protected properties
        protected string CancerType = string.Empty;
        protected string CancerFamily = string.Empty;
        protected string State = string.Empty;
        protected string Country = string.Empty;
        #endregion
        /// <summary>
        /// Default web form class constructor
        /// </summary>       
        public ResultsGeneticsServices()
        {
           //Page.Unload += new System.EventHandler(Page_Unload);
        }

        /// <summary>
        /// Event method sets content version and templates and user control properties<br/>
        /// <br/>
        /// [1] Input parameters:<br/>
        ///			selCancerType {* name(string);value(integer), comma-delimited : genetic professional cancer type names and identifiers},<br/>
        ///			selCancerFamily {* string, comma-delimited: cancer family search parameters},<br/>
        ///			txtCity {string: city search parameter},<br/>
        ///			selState {* string, comma-delimited: postal abbreviations},<br/>
        ///			selCountry {string: country names},<br/>
        ///			txtLastName {string: last name search parameter},<br/>
        ///			selectedPage {integer: indicates page view to display}<br/>
        ///	[2] Uses usp_GetCancerGeneticProfessionals to get results table<br/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        protected override void OnLoad(EventArgs e)
        {

            NameValueCollection postedValues = Request.Form;
            String nextKey;
            for (int i = 0; i < postedValues.AllKeys.Length; i++)
            {
                nextKey = postedValues.AllKeys[i];
                if (nextKey.Substring(0, 2) != "__")
                {
                    if (nextKey.Contains("selCancerType"))
                    {
                        CancerType=postedValues[i];
                    }
                    else if (nextKey.Contains("selCancerFamily"))
                    {
                        CancerFamily = postedValues[i];
                    }
                    else if (nextKey.Contains("selState"))
                    {
                        State = postedValues[i];

                    }

                    else if (nextKey.Contains("selCountry"))
                    {
                        Country = postedValues[i];

                    }
                }
            }


            base.OnLoad(e);
			string city = Request.Form["txtCity"];
			string lastName = Request.Form["txtLastName"];

            GeneticProfessional gp = new GeneticProfessional();
            DataTable dbTable;
            dbTable = gp.GetCancerGeneticProfessionals(CancerType, CancerFamily, city, State, Country, lastName);

            resultCount = dbTable.Rows.Count.ToString();

            searchSummary = BuildSearchSummary(Functions.ParseNameValue(CancerType, 0).Replace(",", ", "), Functions.ParseNameValue(CancerFamily, 0).Replace(",", ", "), lastName, city, Functions.ParseNameValue(State, 0).Replace(",", ", "), Functions.ParseNameValue(Country, 0), resultCount);

            if (resultCount == "0")
            {
                resultLabel = "Results: 0 - 0 of " + resultCount;
                this.submit.Visible = false;
                resultGrid.Visible = false;
            }
            else 
            {
                int selectedPage = Strings.ToInt(Request.Form["selectedPage"], 0);
                selectedPage = selectedPage <= 0 ? 1 : selectedPage;

                firstRec = selectedPage;
                lastRec = (selectedPage + (recordsPerPage - 1)) < Convert.ToInt32(resultCount) ? (selectedPage + (recordsPerPage - 1)) : Convert.ToInt32(resultCount);

                ResultsGeneticServicesPager objPager = new ResultsGeneticServicesPager();
                objPager.CurrentPage = (selectedPage / recordsPerPage) + ((selectedPage % recordsPerPage) > 0 ? 1 : 0);
                objPager.RecordCount = Convert.ToInt32(resultCount);
                objPager.RecordsPerPage = recordsPerPage;
                objPager.ShowPages = 2;
                ulPager.Text = objPager.RenderPager();

                DataTable resultPage = dbTable.Clone();
                for (int i = firstRec - 1; i < lastRec; i++)
                {
                    resultPage.ImportRow(dbTable.Rows[i]);
                }

                resultLabel = "Results: " + firstRec.ToString() + " - " + lastRec.ToString() + " of " + resultCount;
                resultGrid.Visible = true;
                resultGrid.DataSource = resultPage;
                resultGrid.DataBind();

                if (resultPage != null)
                {
                    resultPage.Dispose();
                }
            }

            if (dbTable != null)
            {
                dbTable.Dispose();
            }

            if (WebAnalyticsOptions.IsEnabled)
            {
                // Add page name to analytics
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar1, wbField =>
                {
                    wbField.Value = ConfigurationManager.AppSettings["HostName"] + SearchPageInfo.DetailedViewSearchResultPagePrettyUrl;
                });

            }
 
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            var headers = HttpContext.Current.Response.Headers;
            if (headers.Get("Cache-Control") != null)
            {
                headers.Set("Cache-Control", "private");
            }
        }
        

        public void Page_Unload(object sender, System.EventArgs e)
        {

            if (resultGrid != null)
            {
                resultGrid.Dispose();
            }
        }

        #region BuildSearchSummary method

        /// <summary>
        /// Method builds HTML summary of initial search criteria
        /// </summary>
        /// <param name="cancerType">Types of cancer selected</param>
        /// <param name="cancerFamily">Families of cancer selected</param>
        /// <param name="lastName">Last name search criteria</param>
        /// <param name="city">City search criteria</param>
        /// <param name="stateName">States criteria selected</param>
        /// <param name="countryName">Country criteria selected</param>
        /// <returns>HTML table cells containing a summary of initial search criteria</returns>
        private string BuildSearchSummary(string cancerType, string cancerFamily, string lastName, string city, string stateName, string countryName, string resultCount)
        {
            string result = "";

            result += "<h3>" + resultCount + " cancer genetics professionals found for:</h3>\n";
            result += "<dl class='directory-search-terms'>";

            result += "<dt>Type of cancer</dt>\n";
            result += "<dd>" + cancerType.Replace(",", "</dd><dd>") + "</dd>\n";

            result += "<dt>Family Cancer Syndrome</dt>\n";
            result += "<dd>" + cancerFamily.Replace(",", "</dd><dd>") + "</dd>\n";

            result += "<dt>Last name</dt>\n";
            result += "<dd>" + lastName + "</dd>\n";

            result += "<dt>City</dt>\n";
            result += "<dd>" + city + "</dd>\n";

            result += "<dt>State:</dt>\n";
            result += "<dd>" + stateName.Replace(",", "</dd><dd>") + "</dd>\n";

            result += "<dt>Country:</dt>\n";
            result += "<dd>" + countryName.Replace(",", "</dd><dd>") + "</dd>\n";

            result += "</dl>";
            
            return result;
        }
        
        #endregion
        
    }
}