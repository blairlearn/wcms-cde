using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using NCI.Web.CDE.Modules;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NCI.DataManager;
using NCI.Web.UI.WebControls;

namespace NCI.Web.CDE.UI.SnippetControls
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:DynamicListSnippet runat=server></{0}:DynamicListSnippet>")]
    public class DynamicListSnippet : SnippetControl
    {
        #region Private Members
        DynamicList _dynamicList = null; 

        private string KeyWords
        {
            get 
            { 
                return string.IsNullOrEmpty(this.Page.Request.Params["KeyWords"]) ? DynamicList.SearchParameters.Keyword : this.Page.Request.Params["KeyWords"];
            }
        }

        private DateTime StartDate
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["startdate"]))
                    return DateTime.Parse(DynamicList.SearchParameters.StartDate);
                return DateTime.Parse(this.Page.Request.Params["startdate"]);
            }
        }

        private DateTime EndDate
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["enddate"]))
                    return DateTime.Parse(DynamicList.SearchParameters.EndDate);
                return DateTime.Parse(this.Page.Request.Params["enddate"]);
            }
        }

        private int CurrentPage
        {
            get
            {
                if (string.IsNullOrEmpty(this.Page.Request.Params["page"]))
                    return 1;
                return Int32.Parse(this.Page.Request.Params["page"]);
            }
        }

        private DynamicList DynamicList
        {
            get 
            {
                if( _dynamicList == null )
                    _dynamicList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);
                return _dynamicList;
            }
        }

        #endregion

        public void Page_Load(object sender, EventArgs e)
        {
            processData();
        }

        private void processData()
        {
            try
            {
                if (DynamicList != null)
                {
                    Validate(DynamicList);

                    int actualMaxResult = DynamicList.MaxResults;
                    // Call the  datamanger to perform the search
                    ICollection<SearchResult> searchResults =
                                SearchDataManager.Execute(CurrentPage, StartDate, EndDate, KeyWords,
                                    DynamicList.RecordsPerPage, DynamicList.MaxResults, DynamicList.SearchFilter,
                                    DynamicList.ExcludeSearchFilter, DynamicList.Language, DynamicList.SearchType, out actualMaxResult);

                    DynamicSearch dynamicSearch = new DynamicSearch();
                    dynamicSearch.Results = searchResults;
                    dynamicSearch.StartDate = String.Format("{0:MM/dd/yyyy}", StartDate);
                    dynamicSearch.EndDate = String.Format("{0:MM/dd/yyyy}", EndDate);
                    dynamicSearch.KeyWord = KeyWords;

                    if (CurrentPage > 1)
                        dynamicSearch.StartCount = DynamicList.RecordsPerPage * CurrentPage - 1;
                    else
                        dynamicSearch.StartCount = 1;

                    if (CurrentPage == 1)
                        dynamicSearch.EndCount = DynamicList.RecordsPerPage;
                    else
                    {
                        dynamicSearch.EndCount = dynamicSearch.StartCount + DynamicList.RecordsPerPage - 1;
                        if (searchResults.Count < DynamicList.RecordsPerPage)
                            dynamicSearch.EndCount = actualMaxResult;
                    }

                    int recCount=0;
                    foreach (SearchResult sr in searchResults)
                        sr.RecNumber = dynamicSearch.StartCount + recCount++;

                    dynamicSearch.ResultCount = actualMaxResult;

                    LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResults(DynamicList.ResultsTemplate, dynamicSearch));
                    Controls.Add(ltl);
                    SetupPager(DynamicList.RecordsPerPage, actualMaxResult);
                }
            }
            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("DynamicListSnippet:processData", NCI.Logging.NCIErrorLevel.Error, ex);
            }
        }

        /// <summary>
        /// Helper method to setup the pager
        /// </summary>
        private void SetupPager(int recordsPerPage, int totalRecordCount)
        {
            SimplePager pager = new SimplePager();
            pager.RecordCount = totalRecordCount;
            pager.RecordsPerPage = recordsPerPage;
            pager.CurrentPage = CurrentPage;
            pager.PageParamName = "page";
            pager.PagerStyleSettings.SelectedIndexCssClass = "pager-SelectedPage";
            pager.BaseUrl = PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString();
            Controls.Add(pager);
        }
        /// <summary>
        /// Validates the data received from the xml, throws an exception if some of the required 
        /// fields are null or empty.
        /// </summary>
        /// <param name="dynamicList">The object whose properties are being validated.</param>
        private void Validate(DynamicList dynamicList)
        {
            if (string.IsNullOrEmpty(dynamicList.SearchFilter) ||
                string.IsNullOrEmpty(dynamicList.ResultsTemplate) ||
                string.IsNullOrEmpty(dynamicList.SearchType))
                throw new Exception("One or more of these fields SearchFilter,ResultsTemplate,SearchType cannot be empty, correct the xml data.");

            if (dynamicList.SearchParameters == null ||
                (string.IsNullOrEmpty(dynamicList.SearchParameters.Keyword) &&
                    (string.IsNullOrEmpty(dynamicList.SearchParameters.StartDate) ||
                string.IsNullOrEmpty(dynamicList.SearchParameters.EndDate))))
            {
                throw new Exception("SearchParameters.Keyword,SearchParameters.StartDate,SearchParameters.EndDate cannot be empty, correct the xml data.");
            }
        }
    }
}
