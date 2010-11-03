using System;
using NCI.Text;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// Provides access to a keyed collection of delegates that create markup extension handlers 
    /// related to Cancer.gov.
    /// </summary>
    public class CancerGovLoader : MarkupExtensionLoader
    {
        protected override void LoadHandlers()
        {
            Add(() => new ViewDateHandler());
            Add(() => new ViewLinkHandler());
            Add(() => new ViewPropertyHandler());
            Add(() => new ViewTitleHandler());
        }
    }
}