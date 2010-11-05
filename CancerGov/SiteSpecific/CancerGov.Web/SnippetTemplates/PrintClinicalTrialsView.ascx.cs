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

namespace CancerGov.Web.SnippetTemplates
{
    public partial class PrintClinicalTrialsView : AppsBaseUserControl
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

        private string strEmailUrl = "";

        public string EmailUrl
        {
            get { return strEmailUrl; }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Set base page to display the print verison of the banner.
            //SetDisplayVersion(DisplayVersion.Print);

            // PageHtmlHead.Title = "Clinical Trials Results - National Cancer Institute";

            
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

                string title = String.Empty;
                // string title = (PageHtmlHead.Title != null ? PageHtmlHead.Title : "");
                string url = System.Web.HttpUtility.UrlEncode(this.Request.Url.ToString().Replace("&", "__amp;"));
                string url2 = "/common/popUps/PopEmail.aspx?title="
                    //  + title
                    + "&docurl="
                    + url
                    + "&invokedFrom="
                    + EmailPopupInvokedBy.ClinicalTrialPrintableSearchResults.ToString("d")                     
                    + NCI.Core.HashMaster.SaltedHashURL(HttpUtility.UrlDecode(title) + this.Request.Url, "PSRV1");
                    

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
        

    }
}