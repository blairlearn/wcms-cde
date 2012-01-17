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
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Logging;
using CancerGov.CDR.TermDictionary;
using CancerGov.Text;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileDictionaryHome : SnippetControl
    {


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string languageParam = Strings.Clean(Request.QueryString["language"]);
                string languageParam = ""; // disable langauge selection by query parameter
                string pageTitle = "";
                string buttonText = "";
                string language = "";
                MobileTermDictionary.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText);
                litSearchBlock.Text = MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), "", language, pageTitle, buttonText,false);
                litAZList.Text = MobileTermDictionary.AZBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), language);
            }
            catch (Exception ex)
            {
                Logger.LogError("MobileTermDictionaryHome", NCIErrorLevel.Error, ex);
            }
        }
    }
}