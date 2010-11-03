using System;
using System.Text.RegularExpressions;
using CancerGov.UI.Pages;
using CancerGov.MarkupExtensions;
using NCI.Text;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// </summary>
    [MarkupExtensionHandler("Returns the title of the current view.",
        Usage = "{mx:CancerGov.ViewTitle()}  No parameters are required or allowed.")]
    public class ViewTitleHandler : CancerGovViewPageHandler
    {
        public override string Name
        {
            get { return "CancerGov.ViewTitle"; }
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

            if (this.ViewPage.CurrentView != null)
            {
                result = this.ViewPage.CurrentView.Title;
            }

            return result;
        }
    }
}
