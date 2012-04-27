using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    /// <summary>
    /// </summary>
    [MarkupExtensionHandler("Returns the title of the current page.",
        Usage = "{mx:PageInfo.Title()}  No parameters are required or allowed.")]
    public class PageInfoTitleHandler : MarkupExtensionHandler
    {
        public override string Name
        {
            get { return "PageInfo.Title"; }
        }

        public override string Process(string[] parameters)
        {
            // Process and validate parameters.
            int maxParameterCount = 0;
            if (parameters.Length > maxParameterCount)
            {
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.TooManyParametersError, this.Name, maxParameterCount, parameters.Length));
            }

            string result = String.Empty;

            if (PageAssemblyContext.Current != null)
            {
                //TODO: Is this what we actually want?  What about for booklets???
                result = PageAssemblyContext.Current.PageAssemblyInstruction.GetField("long_title");
            }

            return result;
        }
    }
}
