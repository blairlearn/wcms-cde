using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NCI.Web.CDE.UI;
using NCI.Web.CDE;
using NCI.Web.Dictionary;

namespace CancerGov.Dictionaries.SnippetControls.TermDictionary
{
    public class TermDictionaryHome : UserControl
    {
        protected System.Web.UI.WebControls.PlaceHolder pnlIntroEnglish;

        protected System.Web.UI.WebControls.PlaceHolder pnlIntroSpanish;

        public string TotalCount = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            DictionaryAppManager _dictionaryAppManager = new DictionaryAppManager();

            // Check if dictionary terms list size is cached; if so, set to TotalCount
            if (HttpContext.Current.Cache.Get("totalcount") != null)
            {
                TotalCount = (string)HttpContext.Current.Cache.Get("totalcount");
            }
            // If it isn't, get the current size, save that in the cache, and set to TotalCount
            else
            {
                DictionarySearchResultCollection resultCollection = _dictionaryAppManager.Search("%", SearchType.Begins, 0, int.MaxValue, NCI.Web.Dictionary.DictionaryType.term, PageAssemblyContext.Current.PageAssemblyInstruction.Language);

                if (resultCollection != null)
                    TotalCount = resultCollection.ResultsCount.ToString("N0");

                HttpContext.Current.Cache.Add("totalcount", TotalCount, null, DateTime.Now.AddMinutes(60), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
            }

            //set visibilty for the English versus Spanish text
            if (PageAssemblyContext.Current.PageAssemblyInstruction.Language == "es")
            {
                pnlIntroEnglish.Visible = false;
                pnlIntroSpanish.Visible = true;
            }
            else
            {
                pnlIntroEnglish.Visible = true;
                pnlIntroSpanish.Visible = false;
            }

        }
    }
}