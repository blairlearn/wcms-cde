using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    [MarkupExtensionHandler("Returns the title of the current view.",
    Usage = "{mx:CancerGov.ViewTitle()}  No parameters are required or allowed.")]
    public class ViewTitleHandler : PageInfoTitleHandler
    {
        public override string Name
        {
            get { return "CancerGov.ViewTitle"; }
        }
    }
}
