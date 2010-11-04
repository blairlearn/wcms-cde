using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    /// <summary>
    /// 
    /// TODO-v.NEXT: convert strings to enums?
    /// 
    /// Index   Param                   Requirements
    /// 0       LinkType                String, one of Print, Email, AllPages
    /// 1       LinkUrl                 Optional.  If specified must be String, one of MainPage, AllPages.  Only valid when Link Type is Print or Email.
    /// </summary>
    [MarkupExtensionHandler("Returns the URL for a link.",
        Usage = "{mx:CancerGov.ViewLink(LinkType|LinkUrl)} which returns the URL for a print, email, or all pages link for the current view.  LinkType must be one of Print, Email, AllPages and LinkUrl must be one of MainPage, AllPages.  Specifying a LinkUrl value is not allowed if LinkType is AllPages.")]
    public class ViewLinkHandler : PageInfoLinkHandler
    {
        public override string Name
        {
            get { return "CancerGov.ViewLink"; }
        }
    }
}
