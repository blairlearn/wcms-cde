using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCI.Text;
using NCI.Util;

namespace NCI.Web.CDE.UI.MarkupExtensions
{
    //TODO: FIX THE URLS RETURNED
    /// <summary>
    /// 
    /// TODO-v.NEXT: convert strings to enums?
    /// 
    /// Index   Param                   Requirements
    /// 0       LinkType                String, one of Print, Email, AllPages
    /// 1       LinkUrl                 Optional.  If specified must be String, one of MainPage, AllPages.  Only valid when Link Type is Print or Email.
    /// </summary>
    [MarkupExtensionHandler("Returns the URL for a link.",
        Usage = "{mx:PageInfo.Link(LinkType|LinkUrl)} which returns the URL for a print, email, or all pages link for the current page.  LinkType must be one of Print, Email, AllPages and LinkUrl must be one of MainPage, AllPages.  Specifying a LinkUrl value is not allowed if LinkType is AllPages.")]
    public class PageInfoLinkHandler : MarkupExtensionHandler
    {
        private const string LINKTYPE_PRINT = "PRINT";
        private const string LINKTYPE_EMAIL = "EMAIL";
        private const string LINKTYPE_ALLPAGES = "ALLPAGES";
        private const string LINKURL_MAINPAGE = "MAINPAGE";
        private const string LINKURL_ALLPAGES = "ALLPAGES";
        string _linkType = null;
        string _linkUrl = null;


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
            List<String> validLinkTypeValues = new List<String>(new String[] { LINKTYPE_PRINT, LINKTYPE_EMAIL, LINKTYPE_ALLPAGES });
            if (validLinkTypeValues.Contains(_linkType) == false)
            {
                throw new MarkupExtensionException(String.Format("The Link Type was set to {0} but must be one of the following values: {1}, {2}, {3}", _linkType, LINKTYPE_PRINT, LINKTYPE_EMAIL, LINKTYPE_ALLPAGES));
            }

            // Get optional parameters.

            // LinkUrl
            if (parameters.Length > ++parameterIndex)
            {
                // The second LinkUrl parameter is only valid if the first parameter is _linkType_Print or _linkType_Email.
                // So, if they passed link url parameter when link type was link type, that is an error.
                if (_linkType == LINKTYPE_ALLPAGES)
                    throw new MarkupExtensionException(String.Format("The Link URL parameter is not valid if the Link Type is {0}.", LINKTYPE_ALLPAGES));

                // Validate the Link URL.
                _linkUrl = Strings.Clean(parameters[parameterIndex]);
                if (_linkUrl != null)
                {
                    _linkUrl = _linkUrl.ToUpper();
                    List<String> validLinkUrlValues = new List<String>(new String[] { LINKURL_MAINPAGE, LINKURL_ALLPAGES });
                    if (validLinkUrlValues.Contains(_linkUrl) == false)
                    {
                        throw new MarkupExtensionException(String.Format("The Link URL was set to {0} but must be one of the following values: {1}, {2}", _linkUrl, LINKURL_MAINPAGE, LINKURL_ALLPAGES));
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
                case LINKTYPE_PRINT:
                    {
                        if (_linkUrl != null)
                        {
                            switch (_linkUrl)
                            {
                                case LINKURL_MAINPAGE:
                                    result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("Print").UriStem;                                    
                                    break;
                                case LINKURL_ALLPAGES:
                                    result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("PrintAll").ToString();                                    
                                    
                                    break;
                            }
                        }
                        else
                        {
                            //result = ViewPage.CurrentView.GetCurrentPagePrintUrl();
                            result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("CurrentURL").ToString();                                    

                        }
                        break;
                    }
                case LINKTYPE_EMAIL:
                    {
                        if (_linkUrl != null)
                        {
                            switch (_linkUrl)
                            {
                                case LINKURL_MAINPAGE:
                                    //result = ViewPage.CurrentView.GetEmailUrl();
                                    result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("Email").ToString();                                    

                                    break;
                                case LINKURL_ALLPAGES:
                                    //result = ViewPage.CurrentView.GetAllPagesEmailUrl();  
                                    result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("Email").ToString();
                                    break;
                            }
                        }
                        else
                        {
                            //result = ViewPage.CurrentView.GetCurrentPageEmailUrl();
                            result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("Email").ToString();                                    
                            result = string.Empty;
                        }
                        break;
                    }
                case LINKTYPE_ALLPAGES:
                    {
                        result = PageAssemblyContext.Current.PageAssemblyInstruction.GetUrl("ViewAll").ToString();
                        break;
                    }
            }

            return result;
        }

    }
}
