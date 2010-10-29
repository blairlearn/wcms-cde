using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.UI.CDR
{
    /// <summary>
    /// Utility class for passing option information into the ProtocolRenderer.
    /// Use this class instead of adding arguments to the ProtocolRenderer renderer.
    /// (Don't even think about adding properties to the ProtocolRenderer.)
    /// </summary>
    public class ProtocolRendererOptions
    {
        private string _TrialSitesSeeAllUrl = "";
        private string _TrialSitesSeeAllText = "";

        public string TrialSitesSeeAllUrl
        {
            get { return _TrialSitesSeeAllUrl; }
            set { _TrialSitesSeeAllUrl = value; }
        }
        public string TrialSitesSeeAllText
        {
            get { return _TrialSitesSeeAllText; }
            set { _TrialSitesSeeAllText = value; }
        }
    }
}
