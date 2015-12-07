using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.ComponentModel;
using NCI.Web.CDE.Modules;
using NCI.DataManager;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE.UI.Configuration;
using NCI.Text;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;


namespace NCI.Web.CDE.UI.SnippetControls
{
    /// <summary>
    /// pager Snippet Template is for creating buttons for previous/next posts for Blog Post Content Type.
    /// </summary>
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:BlogLandingDynamicList runat=server></{0}:BlogLandingDynamicList>")]
    public class BlogLandingDynamicList : BaseSearchSnippet
    {
        override protected SearchList SearchList
        {
            get
            {
                if (base.SearchList == null)
                {
                    DynamicListHelper helper = new DynamicListHelper();
                    base.SearchList = ModuleObjectFactory<DynamicList>.GetModuleObject(SnippetInfo.Data);
                    base.SearchList.ResultsTemplate = base.SearchList.ResultsTemplate =
                    helper.languageStrings() +
                    helper.blogBodyString();

                }
                return base.SearchList;
            }
        }

        /// <summary>
        /// Override the numerical pager used on other dynamic lists 
        /// Displays 'older' and 'newer' links
        /// </summary>
        protected override void SetupPager(int recordsPerPage, int totalRecordCount)
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
            else {currentPage= Int32.Parse(this.Page.Request.Params["page"]); }

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

            blogLandingPager.BaseUrl += searchQueryParams;

            Controls.Add(blogLandingPager);
        }
        
    }
}
