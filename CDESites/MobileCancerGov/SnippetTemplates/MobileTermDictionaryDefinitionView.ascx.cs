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
using CancerGov.Text;
using CancerGov.CDR.TermDictionary;
using MobileCancerGov.Web.SnippetTemplates;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;

namespace MobileCancerGov.Web.SnippetTemplates
{
    public partial class MobileTermDictionaryDefinitionView : SnippetControl
    {
        private String _dictionaryURL = "";
        private TermDictionaryDataItem di = null;

        //FOR TESTING 
        public String cdrId = "";
        public String language = "";

        public string DefinitionHTML
        {
            get { return (di == null ? "" : di.DefinitionHTML); }
        }

        public string TermName
        {
            get { return (di == null ? "" : di.TermName); }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            _dictionaryURL = Page.Request.Url.LocalPath;
            azLink.HRef = Page.Request.Url.LocalPath;
            cdrId = Strings.Clean(Request.QueryString["cdrid"]);
            language = Strings.Clean(Request.QueryString["language"]);
            if(String.IsNullOrEmpty(language))
                language = "english"; //default to English


            if (!String.IsNullOrEmpty(cdrId))
            {
                di = TermDictionaryManager.GetDefinitionByTermID(language, cdrId, "patient", 1);
                int i = 0;
             
            }
            

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}