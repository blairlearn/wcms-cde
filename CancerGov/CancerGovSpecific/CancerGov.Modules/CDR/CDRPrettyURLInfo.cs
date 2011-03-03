using System;
using System.Collections.Generic;
using System.Text;

namespace CancerGov.Modules.CDR
{
   public class CDRPrettyURLInfo : ICGPrettyUrlInfo
    {
        #region Member Variables
        private Guid _summaryID = Guid.Empty;
        private string _realURL = string.Empty;
        private string _prettyURL = string.Empty;
        #endregion

        #region Class Properties

        public Guid SummaryID
        {
            get { return _summaryID; }
            set { _summaryID = value; }
        }

        Guid ICGPrettyUrlInfo.ObjectID
        {
            get { return _summaryID; }
        }

        public string PrettyUrl
        {
            get { return _prettyURL; }
            set { _prettyURL = value; }
        }        

        public string RealUrl
        {
            get { return _realURL; }
            set { _realURL = value; }
        }


        string ICGPrettyUrlInfo.RedirectUrl
        {
            get { return null; }
        }

        #endregion
        public CDRPrettyURLInfo(string prettyURL, string realURL)
        {
            _prettyURL = prettyURL;
            _realURL = realURL;
        }

        public CDRPrettyURLInfo(Guid summaryID, string prettyURL, string realURL)
        {
            _summaryID = summaryID;
            _prettyURL = prettyURL;
            _realURL = realURL;
        }
    }
}
