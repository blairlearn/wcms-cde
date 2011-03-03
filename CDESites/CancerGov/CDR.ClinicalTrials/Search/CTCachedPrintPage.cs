using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    public class CTCachedPrintPage
    {
        #region Fields

        int _cacheID;
        string _pageHtml;
        DateTime _cacheDate;

        #endregion


        #region Properties

        public int CacheID
        {
            get { return _cacheID; }
        }

        public string PageHtml
        {
            get { return _pageHtml; }
        }

        public DateTime CacheDate
        {
            get { return _cacheDate; }
        }

        #endregion

        public CTCachedPrintPage(int cacheID, string pageHtml, DateTime cacheDate)
        {
            _cacheID = cacheID;
            _pageHtml = pageHtml;
            _cacheDate = cacheDate;
        }
    }
}
