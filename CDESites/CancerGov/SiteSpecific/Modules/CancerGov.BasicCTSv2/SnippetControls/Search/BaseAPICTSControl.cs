using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using CancerGov.ClinicalTrials.Basic.v2.Configuration;
using Common.Logging;
using NCI.Web;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using CancerGov.ClinicalTrialsAPI;

namespace CancerGov.ClinicalTrials.Basic.v2.SnippetControls.Search
{
    /// <summary>
    /// This is the base class for all API-Based Clinical Trial Search controls.
    /// <remarks>This replaces BasicCTSBaseControl</remarks>
    /// </summary>
    public class BaseAPICTSControl: SnippetControl
    {
        static ILog log = LogManager.GetLogger(typeof(BaseAPICTSControl));
    

    }
}
