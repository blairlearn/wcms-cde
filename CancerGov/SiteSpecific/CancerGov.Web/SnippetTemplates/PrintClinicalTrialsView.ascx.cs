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

using NCI.Web.CDE;
using NCI.Util;
using NCI.Core;
using CancerGov.Common.ErrorHandling;
using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.Common.HashMaster;
using NCI.Web.CancerGov.Apps;
using NCI.Web.CDE.Modules;
using NCI.Logging;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class PrintClinicalTrialsView : AppsBaseUserControl
    {

        private SearchResultPageInfo _searchPageInfo = null;
        enum PageRenderingState
        {
            None = 0,
            Results = 1,
            CustomSelections = 2
        };
        PageRenderingState _pageRenderingState = PageRenderingState.Results;
        
   
        private string strEmailUrl = "";

        public string EmailUrl
        {
            get { return strEmailUrl; }
        }

        //protected int GetProtocolSearchID()
        //{
        //    int protocolSearchID = Strings.ToInt(Strings.Clean(Request.Params["protocolsearchid"]));
        //    if (protocolSearchID == -1)
        //    {
        //        CancerGovError.LogError("ClinicalTrialsResultsAdvanced.aspx", 1, "Invalid Protocol Search ID");
        //        //this.RaiseErrorPage();
        //        this.RaiseErrorPage("InvalidSearchID");
        //    }

        //    return protocolSearchID;
        //}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // Let's find out what they want to see...
            int protocolSearchID = Strings.ToInt(Strings.Clean(Request.QueryString["protocolsearchid"]));
            int cacheID = Strings.ToInt(Strings.Clean(Request.QueryString["cid"]));

            // Handle missing cacheID.
            if (cacheID == -1)
            {
                CancerGovError.LogError("PSRV", 1, "No Protocols Specified");
                this.RaiseErrorPage("InvalidSearchID");
            }

            try
            {
                CTCachedPrintPage cachedPage = CTSearchManager.LoadCachedPage(cacheID);
                litPageContent.Text = cachedPage.PageHtml;
                cacheDate.Text = cachedPage.CacheDate.ToString("d");

                string title = PageInstruction.GetField("long_title");
                string url = System.Web.HttpUtility.UrlEncode(PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString().Replace("&", "__amp;"));
                string url2 = "/common/popUps/PopEmail.aspx?title="
                    + title
                    + "&docurl="
                    + url
                    + "&invokedFrom="
                    + EmailPopupInvokedBy.ClinicalTrialPrintableSearchResults.ToString("d")
                    + NCI.Core.HashMaster.SaltedHashURL(HttpUtility.UrlDecode(title) + PageInstruction.GetUrl(PageAssemblyInstructionUrls.PrettyUrl).ToString(), "PSRV1");
                 
                

                EmailResults.NavigateUrl = url2;
                EmailResults.Attributes.Add("onclick", "javascript: dynPopWindow('" + url2 + "' "
                    + ", 'emailPopUp', 'height=365,width=525'); return false;");

            }
            catch (ProtocolPrintCacheFailureException cacheFailure)
            {
                CancerGovError.LogError("PSRV", 1, "Failed to get cache for id : " + cacheID.ToString() + " Error: " + cacheFailure.Message);
                this.RaiseErrorPage("InvalidSearchID");
            }
            catch (ProtocolNullPrintCacheIDException cacheFailure)
            {
                CancerGovError.LogError("PSRV", 1, "Invalid cacheID : " + cacheID.ToString());
                this.RaiseErrorPage("InvalidSearchID");
            }
        }


        protected SearchResultPageInfo SearchPageInfo
        {
            get
            {
                if (_searchPageInfo != null)
                    return _searchPageInfo;
                // Read the search page information xml , to determine the 
                // search results pretty url
                string spidata = this.SnippetInfo.Data;
                try
                {
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("searchResultPageInfo not present in xml, associate an application module item  with this page in percussion");

                    spidata = spidata.Trim();
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("searchResultPageInfo not present in xml, associate an application module item  with this page in percussion");

                    SearchResultPageInfo searchResultPageInfo = ModuleObjectFactory<SearchResultPageInfo>.GetModuleObject(spidata);

                    return _searchPageInfo = searchResultPageInfo;
                }
                catch (Exception ex)
                {
                    NCI.Logging.Logger.LogError("ClinicalTrialsResults", "could not load the SearchResultPageInfo, check the config info of the application module in percussion", NCIErrorLevel.Error, ex);
                    throw ex;
                }
            }
        }

        #region Event Handlers

        //protected void refineSearch_ServerClick(object sender, System.Web.UI.ImageClickEventArgs e)
        //{
        //    Response.Redirect(SearchPageInfo.SearchPagePrettyUrl + "?protocolsearchid=" + GetProtocolSearchID());
        //}

        #endregion

    }
}