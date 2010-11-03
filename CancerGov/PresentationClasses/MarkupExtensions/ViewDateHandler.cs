using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CancerGov.UI.Pages;
using CancerGov.MarkupExtensions;
using NCI.Text;
using NCI.Util;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// TODO-v.NEXT: convert strings to enums?
    /// 
    /// Index   Param                   Requirements
    /// 0       DateType                String, must be one of Posted, Update, Expiration, or Review.
    /// 1       DateFormat              Optional
    /// </summary>
    [MarkupExtensionHandler("Returns a formatted date such as the view's posted, update, expiration, or review date.",
        Usage = "{mx:CancerGov.ViewDate(DateType)} or {mx:CancerGov.ViewDate(DateType|DateFormat)} where DateType is posted, update, expiration, or review.  Returns the specified date for the current view formatted according to the format string \"MonthNum1, YearNum4\";")]
    public class ViewDateHandler : CancerGovViewPageHandler
    {
        private const string _posted = "POSTED";
        private const string _update = "UPDATE";
        private const string _expiration = "EXPIRATION";
        private const string _review = "REVIEW";
        private string _dateType = null;
        private string _dateFormat = "MonthWordFull DayNum1, YearNum4"; // Default format.


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
            int maxParameterCount = 2;
            if (parameters.Length > maxParameterCount)
            {
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.TooManyParametersError, this.Name, maxParameterCount, parameters.Length));
            }

            // Get the required parameters.

            // DateType
            int parameterIndex = -1;
            _dateType = Strings.Clean(parameters[++parameterIndex]);
            if (_dateType == null)
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.RequiredParameterNotSpecifiedOrInvalidError, this.Name, "Date Type", parameterIndex, typeof(string)));

            _dateType = _dateType.ToUpper();
            List<String> validDateTypeValues = new List<String>(new String[] { _posted, _update, _expiration, _review });
            if (validDateTypeValues.Contains(_dateType) == false)
            {
                throw new MarkupExtensionException(String.Format("The Date Type was set to {0} but must be one of the following values: {1}, {2}, {3}, {4}", _dateType, _posted, _update, _expiration, _review));
            }

            // Get optional parameters.

            // DateFormat
            if (parameters.Length > ++parameterIndex)
            {
                string dateFormatParameter = Strings.Clean(parameters[parameterIndex]);
                if (dateFormatParameter != null)
                    _dateFormat = dateFormatParameter;
            }
        }


        public override string Name
        {
            get { return "CancerGov.ViewDate"; }
        }


        public override string Process(string[] parameters)
        {
            ParseParams(parameters);

            DateTime date = DateTime.Today;

            // Set the date value depending on the requested date type.
            switch (_dateType)
            {
                case "POSTED":
                    date = ViewPage.CurrentView.PostedDate;
                    break;

                case "UPDATE":
                    // Note: when they specify "UPDATE" they actually want "Release" from the DB.  This is not a bug.
                    date = ViewPage.CurrentView.ReleaseDate;
                    break;

                case "EXPIRATION":
                    date = ViewPage.CurrentView.ExpirationDate;
                    break;

                case "REVIEW":
                    date = ViewPage.CurrentView.ReviewedDate;
                    break;
            }

            string formattedDate = NCI.Web.DateHandler.GetFormattedDate(date, _dateFormat);

            return formattedDate;
        }
    }
}
