using System;
using Common.Logging;
using NCI.Web.CancerGov.Apps;
using NCI.Web.CDE.Modules;

namespace CancerGov.Web.SnippetTemplates
{
    public class SearchBaseUserControl : AppsBaseUserControl
    {
        static ILog log = LogManager.GetLogger(typeof(SearchBaseUserControl));

        private SearchResultPageInfo _searchPageInfo = null;

        protected SearchResultPageInfo SearchPageInfo
        {
            get
            {
                if (_searchPageInfo != null)
                    return _searchPageInfo;
                // Read the search page information xml , to determine the 
                // search results pretty url
                string spidata = this.SnippetInfo.Data;
                try
                {
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("searchResultPageInfo not present in xml, associate an application module item  with this page in percussion");

                    spidata = spidata.Trim();
                    if (string.IsNullOrEmpty(spidata))
                        throw new Exception("searchResultPageInfo not present in xml, associate an application module item  with this page in percussion");

                    SearchResultPageInfo searchResultPageInfo = ModuleObjectFactory<SearchResultPageInfo>.GetModuleObject(spidata);

                    return _searchPageInfo = searchResultPageInfo;
                }
                catch (Exception ex)
                {
                    log.Error("could not load the SearchResultPageInfo, check the config info of the application module in percussion", ex);
                    throw ex;
                }
            }
        }

        protected string SearchHelpPrettyUrl
        {
            get
            {
                return string.IsNullOrEmpty(SearchPageInfo.SearchHelpPrettyUrl) ? String.Empty : SearchPageInfo.SearchHelpPrettyUrl;
            }
        }
    }
}