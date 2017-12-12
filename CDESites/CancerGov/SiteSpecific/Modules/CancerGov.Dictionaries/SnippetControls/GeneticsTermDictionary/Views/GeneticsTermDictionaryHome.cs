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
using NCI.Web.CDE.UI.SnippetControls;
using NCI.Web.UI.WebControls;
using NCI.Web.CDE;
using NCI.Web;
using NCI.Web.CDE.UI;

namespace CancerGov.Dictionaries.SnippetControls.GeneticsTermDictionary
{
    public class GeneticsTermDictionaryHome : SnippetControl
    {
        protected DictionarySearchBlock dictionarySearchBlock;

        protected void Page_Load(object sender, EventArgs e)
        {
            dictionarySearchBlock.Dictionary = NCI.Web.Dictionary.DictionaryType.genetic;
            dictionarySearchBlock.DictionaryURL = PageAssemblyContext.Current.requestedUrl.ToString();

        }
    }
}