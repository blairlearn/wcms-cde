using System;
using CancerGov.UI.Pages;
using CancerGov.MarkupExtensions;
using NCI.Text;
using NCI.Util;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionHandler("Returns a property of the current view.",
        Usage = "{mx:CancerGov.ViewProperty(PropertyName)} which returns the value of the view property with name PropertyName for the current view.")]
    public class ViewPropertyHandler : CancerGovViewPageHandler
    {
        private string _propertyName = null;


        private void ParseParams(string[] parameters)
        {
            if (ViewPage.CurrentView == null)
            {
                throw new MarkupExtensionException("ViewPage.CurrentView is null.");
            }

            // Make sure all required parameters were passed in.
            int minParameterCount = 1;
            if (parameters.Length < minParameterCount)
            {
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.TooFewParametersError, this.Name, minParameterCount, parameters.Length));
            }

            // Make sure they didn't pass in too many parameters.
            int maxParameterCount = 1;
            if (parameters.Length > maxParameterCount)
            {
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.TooManyParametersError, this.Name, maxParameterCount, parameters.Length));
            }

            // Get the required parameters.

            // PropertyName
            int parameterIndex = -1;
            _propertyName = Strings.Clean(parameters[++parameterIndex]);
            if (_propertyName == null)
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.RequiredParameterNotSpecifiedOrInvalidError, this.Name, "Property Name", parameterIndex, typeof(string)));
        }


        public override string Name
        {
            get { return "CancerGov.ViewProperty"; }
        }


        public override string Process(string[] parameters)
        {
            ParseParams(parameters);

            string propertyValue = ViewPage.CurrentView.Properties[_propertyName];

            return propertyValue;
        }
    }
}
