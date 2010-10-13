using System;

namespace CancerGov.Web
{
    /// <summary>
    /// Used with PopEmail.aspx to specify where the "Email this Page"
    /// pop up was invoked from.
    /// </summary>
    public enum EmailPopupInvokedBy
    {
        Unspecified = 0,
        ClinicalTrialSearchResults = 1,
        ClinicalTrialPrintableSearchResults = 2
    }

    public enum DisplayLanguage
    {
        English = 1,
        Spanish = 2
    }
}