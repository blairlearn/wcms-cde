using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using NCI.Util;
using NCI.Web.CDE.WebAnalytics;
using NCI.Web.CDE.UI.WebControls;
using NCI.Web.CDE.Modules;
using NCI.Web.UI.WebControls;
using NCI.Logging;
using NCI.Search.Endeca;
//using CancerGov.Modules.Search.Endeca;
using NCI.Web.CDE;
//using NCI.Web.CancerGov.Apps;

namespace DCEG.Web.Apps
{
    public class SearchBaseUserControl:AppsBaseUserControl
    {
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
                    NCI.Logging.Logger.LogError("ClinicalTrialsResults", "could not load the SearchResultPageInfo, check the config info of the application module in percussion", NCIErrorLevel.Error, ex);
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
