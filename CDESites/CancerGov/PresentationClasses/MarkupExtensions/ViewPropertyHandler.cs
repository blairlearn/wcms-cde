using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    [MarkupExtensionHandler("Returns a property of the current view.",
        Usage = "{mx:CancerGov.ViewProperty(PropertyName)} which returns the value of the view property with name PropertyName for the current view.")]
    public class ViewPropertyHandler : PageInfoFieldHandler
    {
        public override string Name
        {
            get { return "CancerGov.ViewProperty"; }
        }
    }
}
