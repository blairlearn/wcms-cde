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
using CancerGov.CDR.TermDictionary;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileDictionaryHome : SnippetControl
    {
        ArrayList azListLettersWithData; 

        protected void Page_Load(object sender, EventArgs e)
        {
            azListLettersWithData = TermDictionaryManager.GetAZListLettersWithData("english");
        }

        protected string AnchorTagCreator(char letter)
        {
            string output="";

            if (azListLettersWithData.IndexOf(letter.ToString().ToUpper()) > -1)
            {
                output = "<a href=\"" +
                         Page.Request.Url.LocalPath +
                          "?expand=" +  Server.UrlEncode(letter.ToString()) +
                          "\">" + letter +
                          "</a>";
            }
            else
                output = letter.ToString();

            return output;
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.LocalPath + "?search=" + searchString.Value.Trim().ToString());
        }
    }
}