﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using Common.Logging;
using NCI.DataManager;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI.Configuration;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// This class is the base implemenation for Dynamic Search & display control and Content Search & 
    /// display control.
    /// </summary>
    public abstract class BaseSearchSnippet : SnippetControl
    {
        #region Private Members
        static ILog log = LogManager.GetLogger(typeof(BaseSearchSnippet));

        SearchList _searchList = null;
        private DataTable _taxonomyFilters = null;

        /// <summary>
        /// The current page that is being used.
        /// </summary>
        private int CurrentPage
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["page"]))
                    return 1;
                return Int32.Parse(this.Page.Request.Params["page"]);
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// SitePath is a search criteria used in searching
        /// </summary>

        protected string SiteName
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return string.Empty;
                return this.SearchList.SearchParameters.SiteName;
            }
        }

        /// <summary>
        /// Keyword is a search criteria used in searching
        /// </summary>
        virtual protected string KeyWords
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return string.Empty;
                return this.SearchList.SearchParameters.Keyword;
            }
        }

        /// <summary>
        /// Startdate is a search criteria used in searching.if 
        /// StartDate value is present then both StartDate and 
        /// EndDate value should exist.
        /// </summary>
        virtual protected DateTime StartDate
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return DateTime.MinValue;
                return string.IsNullOrEmpty(this.SearchList.SearchParameters.StartDate) ? DateTime.MinValue : DateTime.Parse(this.SearchList.SearchParameters.StartDate);
            }
        }

        /// <summary>
        /// Enddate is a search criteria used in searching, if 
        /// StartDate value is present then both StartDate and 
        /// EndDate value should exist.
        /// </summary>
        virtual protected DateTime EndDate
        {
            get
            {
                if (this.SearchList.SearchParameters == null)
                    return DateTime.MaxValue;
                return string.IsNullOrEmpty(this.SearchList.SearchParameters.EndDate) ? DateTime.MaxValue : DateTime.Parse(this.SearchList.SearchParameters.EndDate);
            }
        }

        /// <summary>
        /// TaxonomyFilters is a DataTable consisting of TaxonomyName and TaxonID 
        /// for each taxonomy filter selected on a dynamic list.
        /// If there are no filters selected, then send null.
        /// </summary>
        /*virtual protected DataTable TaxonomyFilters
        {
            get
            {
                if (_taxonomyFilters == null)
                {
                    _taxonomyFilters = ReturnTaxonomySqlParam(this.SearchList.SearchFilters.TaxonomyFilters.OfType<TaxonomyFilter>().ToList());
                }
                return _taxonomyFilters;
            }
        }*/

        protected virtual SearchList SearchList
        { get; set; }

        #endregion

        #region Public
        public void Page_Load(object sender, EventArgs e)
        {
            processData();
        }

        #endregion

        #region Private Methods
        private void processData()
        {
            try
            {
                if (this.SearchList != null)
                {
                    // Validate();

                    int actualMaxResult = this.SearchList.MaxResults;

                    DateTime startDate = StartDate, endDate = EndDate;
                    string keyWord = KeyWords;
                    string siteName = SiteName;
                    if (this.SearchList.SearchType == "keyword")
                    {
                        startDate = DateTime.MinValue;
                        endDate = DateTime.MaxValue;
                    }
                    else if (this.SearchList.SearchType == "date")
                    {
                        keyWord = string.Empty;
                    }

                    int year = 0;
                    Dictionary<string, string> filters = GetUrlFilters();
                    if (startDate == DateTime.MinValue && endDate == DateTime.MaxValue && filters.ContainsKey("year"))
                    {
                        try
                        {
                            year = Int32.Parse(filters["year"]);
                            if (filters.ContainsKey("month"))
                            {
                                try
                                {
                                    int month = Int32.Parse(filters["month"]);
                                    int lastDay = DateTime.DaysInMonth(year, month);
                                    startDate = new DateTime(year, month, 1);
                                    endDate = new DateTime(year, month, lastDay);
                                }
                                catch
                                {
                                    NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("BaseSearchSnippet", 404, "Invalid month parameter in dynamic list filter");
                                }
                            }
                            else
                            {
                                startDate = new DateTime(year, 1, 1);
                                endDate = new DateTime(year, 12, 31);
                            }
                        }
                        catch
                        {
                            NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("BaseSearchSnippet", 404, "Invalid year parameter in dynamic list filter");
                        }
                    }

                    if (startDate == DateTime.MinValue && endDate == DateTime.MaxValue && filters.ContainsKey("month") && !filters.ContainsKey("year"))
                    {
                        NCI.Web.CDE.Application.ErrorPageDisplayer.RaisePageByCode("BaseSearchSnippet", 404, "Invalid parameter in dynamic list filter: cannot have month without year");
                    }

                    List<TaxonomyFilter> filtersForSql = new List<TaxonomyFilter>(this.SearchList.SearchFilters.TaxonomyFilters.Where(filter => filter.Taxons.Count() > 0));
                    foreach (KeyValuePair<string, string> entry in filters)
                    {
                        if (entry.Key != "year" && entry.Key != "month")
                        {
                            bool contains = filtersForSql.Any(filter => filter.TaxonomyName == entry.Key);
                            if(!contains)
                            {
                                TaxonomyFilter newFilter = new TaxonomyFilter();
                                newFilter.TaxonomyName = entry.Key;
                                List<int> taxonIDs = entry.Value.Split(',').Select(Int32.Parse).ToList();
                                List<Taxon> newTaxons = new List<Taxon>();
                                foreach (int ID in taxonIDs)
                                {
                                    Taxon newTaxon = new Taxon();
                                    newTaxon.ID = ID;
                                    newTaxons.Add(newTaxon);
                                }
                                newFilter.Taxons = newTaxons.ToArray<Taxon>();
                                filtersForSql.Add(newFilter);
                            }
                        }
                    }


                    // Call the  datamanger to perform the search
                    ICollection<SearchResult> searchResults =
                                SearchDataManager.Execute(CurrentPage,
                                    startDate,
                                    endDate, 
                                    keyWord, 
                                    ReturnTaxonomySqlParam(filtersForSql),
                                    this.SearchList.RecordsPerPage, 
                                    this.SearchList.MaxResults, 
                                    this.SearchList.SearchFilter,
                                    this.SearchList.ExcludeSearchFilter, 
                                    this.SearchList.ResultsSortOrder, 
                                    this.SearchList.Language, 
                                    Settings.IsLive, 
                                    out actualMaxResult, siteName);

                    DynamicSearch dynamicSearch = new DynamicSearch();
                    dynamicSearch.Results = searchResults;
                    dynamicSearch.StartDate = String.Format("{0:MM/dd/yyyy}", startDate);
                    dynamicSearch.EndDate = String.Format("{0:MM/dd/yyyy}", endDate);
                    dynamicSearch.KeyWord = keyWord;
                    dynamicSearch.SiteName = siteName;
                    dynamicSearch.DisqusShortname = this.SearchList.DisqusShortname;
                    dynamicSearch.SearchTitle = this.SearchList.SearchTitle;

                    // check if the site is in production
                    bool isProd = PageAssemblyContext.Current.IsProd;
                    // append a shortname prefix based on the production state
                    dynamicSearch.DisqusShortname = dynamicSearch.DisqusShortname + (isProd ? "-prod" : "-dev");

                    this.PageInstruction.AddUrlFilter("Print", (name, url) =>
                    {

                        if (url.QueryParameters.ContainsKey("keyword") == false)
                        {
                            url.QueryParameters.Add("keyword", keyWord);
                        }
                        if (!((dynamicSearch.StartDate == "01/01/0001") || (dynamicSearch.EndDate == "12/31/9999")))
                        {
                            url.QueryParameters.Add("startmonth", startDate.Month.ToString());
                            url.QueryParameters.Add("startyear", startDate.Year.ToString());
                            url.QueryParameters.Add("endmonth", endDate.Month.ToString());
                            url.QueryParameters.Add("endyear", endDate.Year.ToString());
                        }
                    });

                    if (actualMaxResult > 0)
                    {
                        if (CurrentPage > 1)
                            dynamicSearch.StartCount = (this.SearchList.RecordsPerPage * (CurrentPage - 1)) + 1;
                        else
                        {
                            dynamicSearch.StartCount = 1;
                        }

                        if (CurrentPage == 1)
                        {
                            dynamicSearch.EndCount = this.SearchList.RecordsPerPage;
                            if (searchResults.Count < this.SearchList.RecordsPerPage)
                                dynamicSearch.EndCount = actualMaxResult;
                        }
                        else
                        {
                            dynamicSearch.EndCount = dynamicSearch.StartCount + this.SearchList.RecordsPerPage - 1;
                            if (searchResults.Count < this.SearchList.RecordsPerPage)
                                dynamicSearch.EndCount = actualMaxResult;
                        }
                    }

                    int recCount = 0;
                    foreach (SearchResult sr in searchResults)
                        sr.RecNumber = dynamicSearch.StartCount + recCount++;

                    int validCount = this.SearchList.MaxResults;

                    if (actualMaxResult < this.SearchList.MaxResults || this.SearchList.MaxResults == 0)
                        validCount = actualMaxResult;
                    else
                        validCount = this.SearchList.MaxResults;

                    dynamicSearch.ResultCount = validCount;
                    LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(this.SearchList.ResultsTemplate, dynamicSearch));
                    Controls.Add(ltl);

                    if(this.SearchList.ResultsTemplate.Contains("Blog"))
                    {
                        SetupBlogPager(this.SearchList.RecordsPerPage, validCount, filters);
                    }
                    else
                    {
                        SetupPager(this.SearchList.RecordsPerPage, validCount, filters);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (Exception ex)
            {
                log.Error("this.SearchListSnippet:processData", ex);
            }
        }
        /// <summary>
        /// Helper method to setup the pager
        /// </summary>
        protected virtual void SetupPager(int recordsPerPage, int totalRecordCount, Dictionary<string, string> urlFilters)
        {
            SimplePager pager = new SimplePager();
            pager.RecordCount = totalRecordCount;
            pager.RecordsPerPage = recordsPerPage;
            pager.CurrentPage = CurrentPage;
            pager.PageParamName = "page";
            pager.PagerStyleSettings.SelectedIndexCssClass = "pager-SelectedPage";
            pager.BaseUrl = PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();

            string searchQueryParams = string.Empty;
            if (this.SearchList.SearchType.ToLower() == "keyword" || this.SearchList.SearchType.ToLower() == "keyword_with_date")
                searchQueryParams = "?keyword=" + Server.HtmlEncode(KeyWords);
            if (this.SearchList.SearchType.ToLower() == "date" || this.SearchList.SearchType.ToLower() == "keyword_with_date")
            {
                if (string.IsNullOrEmpty(searchQueryParams))
                    searchQueryParams = "?";
                else
                    searchQueryParams += "&";
                if (StartDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                    searchQueryParams += string.Format("startMonth={0}&startyear={1}&endMonth={2}&endYear={3}", StartDate.Month, StartDate.Year, EndDate.Month, EndDate.Year);
                else
                    searchQueryParams += "startMonth=&startyear=&endMonth=&endYear=";
            }

            foreach (KeyValuePair<string, string> entry in urlFilters)
            {
                if (string.IsNullOrEmpty(searchQueryParams))
                    searchQueryParams = "?";
                else
                    searchQueryParams += "&";

                searchQueryParams += string.Format("filter[{0}]={1}", entry.Key, entry.Value);
            }
            

            pager.BaseUrl += searchQueryParams;

            Controls.Add(pager);

            // check for existence of previous and and next urls
            string prevUrl = pager.GetPrevLinkUrl();
            string nextUrl = pager.GetNextLinkUrl();

            if (prevUrl != null)
            {
                this.PageInstruction.AddUrlFilter("RelPrev", (name, url) =>
                {
                    url.SetUrl(prevUrl);
                });
            }

            if (nextUrl != null)
            {
                this.PageInstruction.AddUrlFilter("RelNext", (name, url) =>
                {
                    url.SetUrl(nextUrl);
                });
            }
        }

        protected virtual void SetupBlogPager(int recordsPerPage, int totalRecordCount, Dictionary<string, string> urlFilters)
        {
            BlogPager blogLandingPager = new BlogPager();
            int currentPage = 0;

            string olderText = "< Older Posts";
            string newerText = "Newer Posts >";
            if (PageAssemblyContext.Current.PageAssemblyInstruction.GetField("Language") == "es")
            {
                olderText = "< Artículos anteriores";
                newerText = "Artículos siguientes >";
            }

            blogLandingPager.RecordCount = totalRecordCount;
            blogLandingPager.RecordsPerPage = recordsPerPage;
            if (string.IsNullOrEmpty(this.Page.Request.Params["page"]))
                currentPage = 1;
            else { currentPage = Int32.Parse(this.Page.Request.Params["page"]); }

            blogLandingPager.CurrentPage = currentPage;
            blogLandingPager.BaseUrl = PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();
            blogLandingPager.PageParamName = "page";
            blogLandingPager.CssClass = "blog-pager clearfix";
            blogLandingPager.PagerStyleSettings.NextPageCssClass = "older";
            blogLandingPager.PagerStyleSettings.NextPageText = olderText;
            blogLandingPager.PagerStyleSettings.PrevPageCssClass = "newer";
            blogLandingPager.PagerStyleSettings.PrevPageText = newerText;


            string searchQueryParams = string.Empty;
            if (this.SearchList.SearchType.ToLower() == "keyword" || this.SearchList.SearchType.ToLower() == "keyword_with_date")
                searchQueryParams = "?keyword=" + Server.HtmlEncode(KeyWords);
            if (this.SearchList.SearchType.ToLower() == "date" || this.SearchList.SearchType.ToLower() == "keyword_with_date")
            {
                if (string.IsNullOrEmpty(searchQueryParams))
                    searchQueryParams = "?";
                else
                    searchQueryParams += "&";
                if (StartDate != DateTime.MinValue && EndDate != DateTime.MaxValue)
                    searchQueryParams += string.Format("startMonth={0}&startyear={1}&endMonth={2}&endYear={3}", StartDate.Month, StartDate.Year, EndDate.Month, EndDate.Year);
                else
                    searchQueryParams += "startMonth=&startyear=&endMonth=&endYear=";
            }

            foreach (KeyValuePair<string, string> entry in urlFilters)
            {
                if (string.IsNullOrEmpty(searchQueryParams))
                    searchQueryParams = "?";
                else
                    searchQueryParams += "&";

                searchQueryParams += string.Format("filter[{0}]={1}", entry.Key, entry.Value);
            }

            blogLandingPager.BaseUrl += searchQueryParams;

            Controls.Add(blogLandingPager);

            // check for existence of previous and and next urls
            string prevUrl = blogLandingPager.GetPrevLinkUrl();
            string nextUrl = blogLandingPager.GetNextLinkUrl();

            if (prevUrl != null)
            {
                this.PageInstruction.AddUrlFilter("RelPrev", (name, url) =>
                {
                    url.SetUrl(prevUrl);
                });
            }

            if (nextUrl != null)
            {
                this.PageInstruction.AddUrlFilter("RelNext", (name, url) =>
                {
                    url.SetUrl(nextUrl);
                });
            }
        }

        /// <summary>
        /// Validates the data received from the xml, throws an exception if the required 
        /// fields are null or empty.
        /// </summary>
        /// <param name="this.SearchList">The object whose properties are being validated.</param>
        private void Validate()
        {
            if (string.IsNullOrEmpty(this.SearchList.SearchFilter) ||
                string.IsNullOrEmpty(this.SearchList.ResultsTemplate) ||
                string.IsNullOrEmpty(this.SearchList.SearchType))
                throw new Exception("One or more of these fields SearchFilter,ResultsTemplate,SearchType cannot be empty, correct the xml data.");

        }

        private Dictionary<string, string> GetUrlFilters()
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            Regex pattern = new Regex(@"filter\[([^]]*)\]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (string key in HttpContext.Current.Request.QueryString.AllKeys)
            {
                if (pattern.IsMatch(key))
                {
                    Match match = pattern.Match(key);
                    urlParams.Add(match.Groups[1].Value, HttpContext.Current.Request.QueryString[match.Value]);
                }
            }

            return urlParams;
        }

        private DataTable GetTaxonomyDataTable()
        {
            // This datatable must be structured like the datatable in the stored proc, 
            // in order to be passed in correctly as a parameter.
            DataTable dt = new DataTable();

            // First column, "taxonomyName", is a varchar of 250 characters
            DataColumn dc = new DataColumn();
            dc.DataType = System.Type.GetType("System.String");
            dc.ColumnName = "taxonomyName";
            dc.MaxLength = 250;
            dt.Columns.Add(dc);

            // Second column, "taxonID", is an int
            DataColumn dc1 = new DataColumn();
            dc1.DataType = System.Type.GetType("System.Int32");
            dc1.ColumnName = "taxonID";
            dt.Columns.Add(dc1);

            return dt;
        }

        private DataTable ReturnTaxonomySqlParam(List<TaxonomyFilter> taxonomyFiltersList)
        {
            // This datatable must be structured like the datatable in the stored proc, 
            // in order to be passed in correctly as a parameter.
            DataTable dt = GetTaxonomyDataTable();

            // Loop through each of the different TaxonomyFilters (for different taxonomies)
            foreach (TaxonomyFilter filter in taxonomyFiltersList)
            {
                // Loop through each Taxon within each TaxonomyFilter
                foreach (Taxon taxon in filter.Taxons)
                {
                    // Create a new row in the datatable for each Taxon, and assign the
                    // values to the columns accordingly.
                    DataRow row = dt.NewRow();
                    row["taxonomyName"] = filter.TaxonomyName;
                    row["taxonID"] = taxon.ID;
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }

        #endregion

    }
}
