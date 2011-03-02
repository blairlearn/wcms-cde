using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    [MarkupExtensionHandler("Returns a formatted date such as the view's posted, update, expiration, or review date.",
    Usage = "{mx:CancerGov.ViewDate(DateType)} or {mx:CancerGov.ViewDate(DateType|DateFormat)} where DateType is posted, update, expiration, or review.  Returns the specified date for the current view formatted according to the format string \"MonthNum1, YearNum4\";")]
    public class ViewDateHandler : PageInfoDateHandler
    {
        public override string Name
        {
            get { return "CancerGov.ViewDate"; }
        }
    }
}
