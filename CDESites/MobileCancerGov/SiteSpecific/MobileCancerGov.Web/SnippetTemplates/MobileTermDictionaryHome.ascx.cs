using System;
using System.Web.UI;
using Common.Logging;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.UI.SnippetControls;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileDictionaryHome : SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(MobileDictionaryHome));

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //string languageParam = Strings.Clean(Request.QueryString["language"]);
                string languageParam = ""; // disable langauge selection by query parameter
                string pageTitle = "";
                string buttonText = "";
                string language = "";
                string reDirect = "";
                MobileTermDictionary.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText, out reDirect);
                litSearchBlock.Text = MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), "", language, pageTitle, buttonText,false);
                litAZList.Text = MobileTermDictionary.AZBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), language);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}