using System;
using System.Web;
using NCI.Text;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    /// <summary>
    /// Provides access to a keyed collection of delegates that create generic markup extension 
    /// handlers.
    /// </summary>
    public class CancerGovLegacyExtensionsLoader : MarkupExtensionLoader
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
