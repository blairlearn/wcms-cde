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
using CancerGov.CDR.TermDictionary;


namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileDictionaryHome : SnippetControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            litPageUrl.Text = Page.Request.Url.LocalPath;
            litSearchBlock.Text = MobileTermDictionary.SearchBlock("","");
            litAZList.Text = MobileTermDictionary.AZBlock(Page.Request.Url.LocalPath);


        }

        //protected string AnchorTagCreator(char letter)
        //{
        //    string output = "";

        //    if (azListLettersWithData.IndexOf(letter.ToString().ToUpper()) > -1)
        //    {
        //        output = "<a href=\"" +
        //                 Page.Request.Url.LocalPath +
        //                  "?expand=" + Server.UrlEncode(letter.ToString()) +
        //                  "\">" + letter +
        //                  "</a>";
        //    }
        //    else
        //        output = letter.ToString();

        //    return output;
        //}


    }
}