using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;
using NCI.Util;

using NCI.Web.CDE.UI;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    [MarkupExtensionHandler("Returns a field of the current page.",
        Usage = "{mx:PageInfo.Field(FieldName)} which returns the value of the field with name FieldName for the current page.")]
    public class PageInfoFieldHandler : MarkupExtensionHandler
    {
        private string _propertyName = null;


        private void ParseParams(string[] parameters)
        {
            if (PageAssemblyContext.Current == null)
            {
                throw new MarkupExtensionException("There is no PageAssemblyContext.");
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
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.RequiredParameterNotSpecifiedOrInvalidError, this.Name, "Field Name", parameterIndex, typeof(string)));
        }


        public override string Name
        {
            get { return "PageInfo.Field"; }
        }


        public override string Process(string[] parameters)
        {
            ParseParams(parameters);
            string field = string.Empty;

            switch (_propertyName)
            {
                case "IsPDFAvailable":
                    {
                        field = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("application/pdf").UriStem;

                    }
                    break;

                case "VolumeNumber":
                    {
                        field = PageAssemblyContext.Current.PageAssemblyInstruction.GetField(_propertyName);

                    }
                    break;
            }

            if(string.IsNullOrEmpty(field))
                field = PageAssemblyContext.Current.PageAssemblyInstruction.GetField(_propertyName);

            //if (_propertyName == "IsPDFAvailable")
            //{
            //    //string field = PageAssemblyContext.Current.PageAssemblyInstruction.GetField(_propertyName);
            //    field = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("application/pdf").UriStem;
            //}

            return field;
        }

    }
}
