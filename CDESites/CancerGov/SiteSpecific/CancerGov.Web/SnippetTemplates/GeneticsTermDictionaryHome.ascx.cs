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

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GeneticsTermDictionaryHome : System.Web.UI.UserControl
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
                string reDirect = "";

                //litSearchBlock.Text = GeneticsTermDictionaryHelper.SearchBlock();
                    //MobileTermDictionary.SearchBlock(MobileTermDictionary.RawUrlClean(Page.Request.RawUrl), "", language, pageTitle, buttonText, false);
            }
            catch (Exception ex)
            {
                // Insert Error Trapping 

                //Logger.LogError("GeneticsTermDictionaryHome", NCIErrorLevel.Error, ex);
            }
        }
    }
}