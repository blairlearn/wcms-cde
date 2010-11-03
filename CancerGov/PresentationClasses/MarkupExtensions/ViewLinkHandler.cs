using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CancerGov.Text;
using CancerGov.UI.Pages;
using CancerGov.MarkupExtensions;
using NCI.Text;

namespace CancerGov.MarkupExtensions
{
    /// <summary>
    /// 
    /// TODO-v.NEXT: convert strings to enums?
    /// 
    /// Index   Param                   Requirements
    /// 0       LinkType                String, one of Print, Email, AllPages
    /// 1       LinkUrl                 Optional.  If specified must be String, one of MainPage, AllPages.  Only valid when Link Type is Print or Email.
    /// </summary>
    [MarkupExtensionHandler("Returns the URL for a link.",
        Usage = "{mx:CancerGov.ViewLink(LinkType|LinkUrl)} which returns the URL for a print, email, or all pages link for the current view.  LinkType must be one of Print, Email, AllPages and LinkUrl must be one of MainPage, AllPages.  Specifying a LinkUrl value is not allowed if LinkType is AllPages.")]
    public class ViewLinkHandler : CancerGovViewPageHandler
    {
        private const string _linkType_Print = "PRINT";
        private const string _linkType_Email = "EMAIL";
        private const string _linkType_AllPages = "ALLPAGES";
        private const string _linkUrl_MainPage = "MAINPAGE";
        private const string _linkUrl_AllPages = "ALLPAGES";
        string _linkType = null;
        string _linkUrl = null;


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

            // LinkType
            int parameterIndex = -1;
            _linkType = Strings.Clean(parameters[++parameterIndex]);
            if (_linkType == null)
                throw new MarkupExtensionException(String.Format(MarkupExtensionHandler.RequiredParameterNotSpecifiedOrInvalidError, this.Name, "Link Type", parameterIndex, typeof(string)));

            _linkType = _linkType.ToUpper();
            List<String> validLinkTypeValues = new List<String>(new String[] { _linkType_Print, _linkType_Email, _linkType_AllPages });
            if (validLinkTypeValues.Contains(_linkType) == false)
            {
                throw new MarkupExtensionException(String.Format("The Link Type was set to {0} but must be one of the following values: {1}, {2}, {3}", _linkType, _linkType_Print, _linkType_Email, _linkType_AllPages));
            }

            // Get optional parameters.

            // LinkUrl
            if (parameters.Length > ++parameterIndex)
            {
                // The second LinkUrl parameter is only valid if the first parameter is _linkType_Print or _linkType_Email.
                // So, if they passed link url parameter when link type was link type, that is an error.
                if (_linkType == _linkType_AllPages)
                    throw new MarkupExtensionException(String.Format("The Link URL parameter is not valid if the Link Type is {0}.", _linkType_AllPages));

                // Validate the Link URL.
                _linkUrl = Strings.Clean(parameters[parameterIndex]);
                if (_linkUrl != null)
                {
                    _linkUrl = _linkUrl.ToUpper();
                    List<String> validLinkUrlValues = new List<String>(new String[] { _linkUrl_MainPage, _linkUrl_AllPages });
                    if (validLinkUrlValues.Contains(_linkUrl) == false)
                    {
                        throw new MarkupExtensionException(String.Format("The Link URL was set to {0} but must be one of the following values: {1}, {2}", _linkUrl, _linkUrl_MainPage, _linkUrl_AllPages));
                    }
                }
            }
        }


        public override string Name
        {
            get { return "CancerGov.ViewLink"; }
        }


        /// <summary>
        /// params 
        /// 0   linkType
        /// 1   whichPage
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override string Process(string[] parameters)
        {
            ParseParams(parameters);

            string result = String.Empty;

            switch (_linkType)
            {
                case _linkType_Print:
                    {
                        if (_linkUrl != null)
                        {
                            switch (_linkUrl)
                            {
                                case _linkUrl_MainPage:
                                    result = ViewPage.CurrentView.GetPrintUrl();
                                    break;
                                case _linkUrl_AllPages:
                                    result = ViewPage.CurrentView.GetAllPagesPrintUrl();
                                    break;
                            }
                        }
                        else
                        {
                            result = ViewPage.CurrentView.GetCurrentPagePrintUrl();
                        }
                        break;
                    }
                case _linkType_Email:
                    {
                        if (_linkUrl != null)
                        {
                            switch (_linkUrl)
                            {
                                case _linkUrl_MainPage:
                                    result = ViewPage.CurrentView.GetEmailUrl();
                                    break;
                                case _linkUrl_AllPages:
                                    result = ViewPage.CurrentView.GetAllPagesEmailUrl();
                                    break;
                            }
                        }
                        else
                        {
                            result = ViewPage.CurrentView.GetCurrentPageEmailUrl();
                        }
                        break;
                    }
                case _linkType_AllPages:
                    {
                        result = ViewPage.CurrentView.GetAllPagesUrl();
                        break;
                    }
            }

            return result;
        }
    }
}
