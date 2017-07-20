using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using NCI.Search;
using NCI.Search.Configuration;
using NCI.Util;
using NCI.Web.CDE.Configuration;
using NCI.Web.CDE.Modules;
using NCI.Web.UI.WebControls;
using NCILibrary.Search.Client;
using System.Net.Http;
using NCI.Web.CDE.Modules;

namespace NCI.Web.CDE.UI.SnippetControls
{
    public partial class SiteWideSearch_v2 : AppsBaseSnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(SiteWideSearch_v2));
        private IPageAssemblyInstruction pgInstruction = PageAssemblyContext.Current.PageAssemblyInstruction;
        SiteWideSearchAPIClient searchClient = new SiteWideSearchAPIClient(SitewideSearchBasePath, new HttpClient());
        SiteWideSearchConfig _searchPageInfo;
        BestBetsAPIClient bestBetsClient = new BestBetsAPIClient(BestBetsBasePath, new HttpClient());

        private int _PageNumber = 1;
        private int _PageOffset;
        private int _ItemsPerPage;
        private const int ITEMS_PER_PAGE_DEFAULT = 10;

        protected SiteWideSearchConfig SearchConfig
        {
            get
            {
                if (_searchPageInfo != null)
                    return _searchPageInfo;
                // Read the basic CTS page information xml
                string spidata = this.SnippetInfo.Data;
                try
                {
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("SiteWideSearchConfig not present in xml, associate an application module item  with this page in percussion");

                    spidata = spidata.Trim();
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("SiteWideSearchConfig not present in xml, associate an application module item  with this page in percussion");

                    SiteWideSearchConfig basicCTSPageInfo = ModuleObjectFactory<SiteWideSearchConfig>.GetModuleObject(spidata);

                    return _searchPageInfo = basicCTSPageInfo;
                }
                catch (Exception ex)
                {
                    log.Error("could not load the BasicCTSPageInfo, check the config info of the application module in percussion", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Base path (API version) set in the web.config.
        /// This can also be an empty string
        /// </summary>
        static protected string SitewideSearchBasePath
        {
            get
            {
                string basepath = ConfigurationManager.AppSettings["SiteWideSearchAPIBasepath"].ToString();
                if (basepath == null)
                {
                    basepath = String.Empty;
                }

                return basepath;
            }
        }


        protected string SitewideSearchCollection
        {
            get
            {
                if(SearchConfig.SearchCollection.IndexOf("DocSearch") > -1)
                    return "doc";
                else
                    return "cgov";
            }
        }

        /// <summary>
        /// Base path (API version) set in the web.config.
        /// This can also be an empty string
        /// </summary>
        static protected string BestBetsBasePath
        {
            get
            {
                string basepath = ConfigurationManager.AppSettings["BestBetsAPIBasepath"].ToString();
                if (basepath == null)
                {
                    basepath = String.Empty;
                }

                return basepath;
            }
        }
       

        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            var searchTerm = HttpContext.Current.Request.Params.Get("swKeyword");
            if (searchTerm == null)
                searchTerm = "";

            bool parsedResultCount = Int32.TryParse(Request.QueryString["pageunit"], out _ItemsPerPage);
            // If the parsedResultCount is false, there was no url param, so the default value should be used.
            if (parsedResultCount == false)
                _ItemsPerPage = ITEMS_PER_PAGE_DEFAULT;

            bool parsedCurrentPage = Int32.TryParse(Request.QueryString["page"], out _PageNumber);

            Int32.TryParse(Request.QueryString["Offset"], out _PageOffset);

            // Page 3 with 10 per page has an offset of 20 and user views 21-30
            // (page - 1) x offset + 1
            if (_PageNumber > 1)
            {
                _PageOffset = (_PageNumber - 1) * _ItemsPerPage;
            }
            var rangeFrom = _PageOffset + 1;
            var rangeTo = _PageOffset + _ItemsPerPage;


            // We will prioritize any fields in the url over the current language.
            var language = Request.QueryString["Language"];
            if (String.IsNullOrEmpty(language))
            {
                language = pgInstruction.GetField("Language");
            }


            // Only show best bets when on the first page of results
            string bestBetsName = null;
            string bestBetsHtml = null;
            if (_PageNumber == 0)
            {
                var bestBetsResult = bestBetsClient.Search(language, searchTerm);               

                if (bestBetsResult.Length > 0)
                {
                    bestBetsHtml = bestBetsResult[0].HTML;
                    bestBetsName = bestBetsResult[0].Name;
                }
            }

            SiteWideSearchResults results = null;
            try
            {
                results = searchClient.Search(SitewideSearchCollection, language, searchTerm, _ItemsPerPage, _PageOffset);
            }
            catch (Exception ex)
            {
                var test = "Invalid results";
            }

            // Show Results
            LiteralControl ltl = new LiteralControl(VelocityTemplate.MergeTemplateWithResultsByFilepath(
                SearchConfig.ResultsPageTemplatePath,
                new
                {
                    Results = results,
                    SearchTerm = searchTerm,
                    BestBetsHtml = bestBetsHtml,
                    BestBetsName = bestBetsName,
                    RangeFrom = rangeFrom,
                    RangeTo = rangeTo,
                    Control = this,
                }
            ));
            Controls.Add(ltl);
           
        }


        /// <summary>
        /// Gets URL for a Single Page of Results
        /// </summary>
        /// <param name="pageNum"></param>
        /// <returns></returns>
        public string GetPageUrl(int pageNum)
        {
            NciUrl url = new NciUrl();
            url.SetUrl(HttpContext.Current.Request.RawUrl.ToString());

            if (!url.QueryParameters.ContainsKey("page"))
            {
                url.QueryParameters.Add("page", pageNum.ToString());
            }
            else
            {
                url.QueryParameters["page"] = pageNum.ToString();
            }

            return url.ToString();
        }

        /// <summary>
        /// Gets the Urls and Labels for all the pages of results from Curr Page - numLeft to Curr Page + numRight
        /// </summary>
        /// <param name="numLeft">The number of pages to display left of the selected page</param>
        /// <param name="numRight">The number of pages to display to the right of the selected page</param>
        /// <param name="totalResults">The total number of results </param>
        /// <returns></returns>
        public IEnumerable<object> GetPagerItems(int numLeft, int numRight, long totalResults, string language)
        {
            int startPage = (_PageNumber - numLeft) >= 1 ? _PageNumber - numLeft : 1; // Current page minus left limit (numLeft)
            int maxPage = (int)Math.Ceiling((double)totalResults / (double)_ItemsPerPage); // Highest available page
            int endPage = (_PageNumber + numRight) <= maxPage ? _PageNumber + numRight : maxPage; // Current page plus right limit (numRight)

            // The maximum number of elements that can be drawn in this pager. his would be:
            // ("< Previous" + 1 + 2/ellipsis + numLeft + current + numRight + next-to-last/ellipsis + "Next >") 
            int itemsTotalMax = 7 + numLeft + numRight;

            // If the pageNumber parameter is set above the highest available page number, set the start page to the endPage value.
            if (_PageNumber > endPage)
            {
                startPage = (endPage - numLeft) >= 1 ? endPage - numLeft : 1;
            }

            // If maxPage == 1, then only one page of results is found. Therefore, return null for the pager items.
            // Otherwise, set up the pager accordingly.
            if (maxPage > 1)
            {
                // Create a list of pager item objects:
                //  Text (string) = link text
                //  PageUrl (string) = href value for item 
                //  IsLink (bool) - whether or not this item will be used as a link
                List<object> items = new List<object>();

                // Draw text and links for first & previous pages
                if (_PageNumber != 1)
                {
                    // Draw link to previous page
                    string PrevUrl = GetPageUrl(_PageNumber - 1);
                    items.Add(
                    new
                    {
                        Text = "&lt; Previous",
                        PageUrl = PrevUrl
                    });

                    // add previous url to filter
                    this.PageInstruction.AddUrlFilter("RelPrev", (name, url) =>
                    {
                        url.SetUrl(PrevUrl);
                    });

                    // Draw first page links and text
                    if (_PageNumber > (numLeft + 1))
                    {
                        // Draw link to first page
                        items.Add(
                        new
                        {
                            Text = "1",
                            PageUrl = GetPageUrl(1)
                        });

                        // Draw elipses to delimit first page. 
                        // If the ellipses only represent a single digit, draw that instead (in this case it will always be 2).
                        if (_PageNumber > (numLeft + 2))
                        {
                            if (_PageNumber == (numLeft + 3))
                            {
                                items.Add(
                                new
                                {
                                    Text = "2",
                                    PageUrl = GetPageUrl(2)
                                });
                            }
                            else
                            {
                                items.Add(
                                new
                                {
                                    Text = "...",
                                    IsLink = false
                                });
                            }
                        }
                    }
                }

                // Draw links before and after current
                for (int i = startPage; i <= endPage; i++)
                {
                    items.Add(
                        new
                        {
                            Text = i.ToString(),
                            PageUrl = GetPageUrl(i)
                        }
                    );
                }

                // Draw last page links and text
                if (_PageNumber < endPage)
                {
                    // Draw iink to last page
                    if (_PageNumber < (maxPage - numRight))
                    {
                        // Draw elipses to delimit last page. 
                        // If the ellipses only represent a single digit, draw that instead (in this case it will always maxmimum page minus one).
                        if (_PageNumber < (maxPage - numRight - 1))
                        {
                            if (_PageNumber == (maxPage - numRight - 2))
                            {
                                items.Add(
                                new
                                {
                                    Text = (maxPage - 1).ToString(),
                                    PageUrl = GetPageUrl(maxPage - 1)
                                });
                            }
                            else
                            {
                                items.Add(
                                new
                                {
                                    Text = "...",
                                    IsLink = false
                                });
                            }
                        }

                        // Draw link to last page
                        items.Add(
                        new
                        {
                            Text = maxPage.ToString(),
                            PageUrl = GetPageUrl(maxPage)
                        });
                    }

                    // Draw link to next page
                    string NextUrl = GetPageUrl(_PageNumber + 1);
                    items.Add(
                    new
                    {
                        Text = language == "en" ? "Next &gt;" : "Siguiente &gt;",
                        PageUrl = NextUrl
                    });

                    this.PageInstruction.AddUrlFilter("RelNext", (name, url) =>
                    {
                        url.SetUrl(NextUrl);
                    });

                }

                // Remove any duplicate links that may have slipped though. This only occurs in cases where the URL query param 
                // is greater than the last available page.
                // Doing this after the fact to prevent mucking up the code above. This is an edge case and should be handled outside of
                // the general logic. 
                if (_PageNumber > maxPage)
                {
                    items = items.Distinct().ToList();
                    // Remove ellipsis if it shows betweeen two consecutive numbers
                    if (items.Count - 2 == maxPage)
                    {
                        items.RemoveAt(2);
                    }
                }

                return items;
            }
            else
            {
                return null;
            }
        }
        
    }
}
