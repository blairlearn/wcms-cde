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
using NCI.Web.CDE.WebAnalytics;
using CancerGov.Text;
using CancerGov.Common;
using CancerGov.CDR.TermDictionary;
using CancerGov.Web.SnippetTemplates;
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class GeneticsTermDictionaryHome : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string languageParam = ""; //disable language selection by query parameter 

            //Determine Language - set language related values
            string pageTitle;
            string buttonText;
            string language;
            string reDirect;
            GeneticsTermDictionaryHelper.DetermineLanguage(languageParam, out language, out pageTitle, out buttonText, out reDirect);
            litPageUrl.Text = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentUrl").ToString();
            litSearchBlock.Text = GeneticsTermDictionaryHelper.SearchBlock(litPageUrl.Text, "", language, pageTitle, buttonText, false);

        }
    }
}