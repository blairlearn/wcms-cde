using System;
using System.Collections.Specialized;   // In order to reference Prototype.
using System.Configuration;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.CDR.DataManager;
using CancerGov.UI.CDR;
using CancerGov.UI.PageObjects;
using NCI.Logging;
using NCI.Util;
using NCI.Web;
using NCI.Web.CDE;
using NCI.Web.CDE.UI;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsView : SearchBaseUserControl, ISupportingSnippet
    {

        public string strContent = "";

        public string Content
        {
            get { return strContent; }
        }

        public bool IncludeSurvey()
        {
            if (Request.UrlReferrer != null)
            {
                String url = Request.UrlReferrer.AbsolutePath.ToString();
                //only do the survey for the advanced search page 
                if (url.Contains(SearchPageInfo.SearchResultsPrettyUrl))
                {
                    //only show this survey if the other survey was not shown
                    //look at the persistant cookie  for that survey  ascookie parameter in the tiggerParams file 
                    string no_results_survey = ConfigurationSettings.AppSettings["NoResCTSurveyShown"].ToString();
                    if (Request.Cookies.Get(no_results_survey) == null)
                        return true;
                }
            }
            return false;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int iProtocolID = -1;
            int iProtocolSearchID = 0;
            string strVersion = "";
            bool hasProtocolSearchid = false;



            ProtocolVersions pvVersion = ProtocolVersions.HealthProfessional;
            iProtocolID = Strings.ToInt(Request.Params["cdrid"]);

            if (iProtocolID < 1)
            {                
                //Send the user the PageNotFoundPage, do not log anything.  (One could argue we should return a 400 since there were invalid params...)
                throw new HttpException(404, "No Protocol ID Specified");
            }

            iProtocolSearchID = Strings.ToInt(Strings.IfNull(Strings.Clean(Request.Params["protocolsearchid"]), "0"));
            strVersion = Strings.IfNull(Strings.Clean(Request.Params["version"]), "healthprofessional");

            if (strVersion.ToUpper() == "PATIENT")
            {
                pvVersion = ProtocolVersions.Patient;
            }

            Protocol pProtocol = null;
            try
            {
                pProtocol = new Protocol(iProtocolID, ProtocolFunctions.GetSectionList(pvVersion, ProtocolDisplayFormats.SingleProtocol), iProtocolSearchID, pvVersion);
            }
            catch (CancerGov.Exceptions.ProtocolFetchFailureException fetchError)
            {

                NCI.Logging.Logger.LogError("ViewClinicalTrials", "ProtocolID = " + iProtocolID + " Error: " + fetchError.Message, NCIErrorLevel.Error, fetchError);
                //This is an error - maybe no DB connection, who knows what, but stuff broke.
                this.RaiseErrorPage();
            }
            catch (CancerGov.Exceptions.ProtocolTableEmptyException fetchError)
            {               
                //Do not log an error as no protocol was found.  If you really want to know why it was
                //not found, then just run the stored proc.  Instead, just send the user to the page 
                //not found page.  This is tricky since this can be thrown if the protocol was not 
                //retrieved, but also if NO tables came back, which would indicate
                //something wrong with the stored proc.  We will just treat them both as a protocol
                //was not found.
                throw new HttpException(404, "No Protocol Found with that ID");

            }
            catch (CancerGov.Exceptions.ProtocolTableMiscountException fetchError)
            {

                NCI.Logging.Logger.LogError("ViewClinicalTrials", "ProtocolID = " + iProtocolID + " Error: " + fetchError.Message, NCIErrorLevel.Error, fetchError);
                //This is an error - basically there are tables we are expecting that are not there
                //this is probably a stored proc issue.
                this.RaiseErrorPage();
            }
            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("ViewClinicalTrials", "ProtocolID = " + iProtocolID + " Error: " + ex.Message, NCIErrorLevel.Error, ex);
                this.RaiseErrorPage();
            }


            StringBuilder sbDate = new StringBuilder();

            if (pProtocol.ProtocolType == ProtocolTypes.Protocol)
            {
                if (pProtocol.DateLastModified != new DateTime(0))
                {
                    PageInstruction.AddFieldFilter("pvLastModified", (fieldName, data) =>
                    {
                        data.Value = pProtocol.DateLastModified.ToString("d");
                    });
                }
                else
                {
                    PageInstruction.AddFieldFilter("pvLastModified", (fieldName, data) =>
                    {
                        data.Value = String.Empty;
                    });

                }

                if (pProtocol.DateFirstPublished != new DateTime(0))
                {
                    PageInstruction.AddFieldFilter("pvFirstPublished", (fieldName, data) =>
                    {
                        data.Value = pProtocol.DateFirstPublished.ToString("d");
                    });
                }
                else
                {
                    PageInstruction.AddFieldFilter("pvFirstPublished", (fieldName, data) =>
                    {
                        data.Value = String.Empty;
                    });
                }

            }

            //set the page title as the protocol title
            PageInstruction.AddFieldFilter("long_title", (fieldName, data) =>
            {
                data.Value = pProtocol.ProtocolTitle;
            });
            
            StringBuilder sbPageUrl = new StringBuilder();
            sbPageUrl.Append(SearchPageInfo.DetailedViewSearchResultPagePrettyUrl);
            sbPageUrl.Append("?cdrid=");
            sbPageUrl.Append(iProtocolID.ToString());



            if (iProtocolSearchID > 0)
            {
                sbPageUrl.Append("&protocolsearchid=");
                sbPageUrl.Append(iProtocolSearchID.ToString());
            }


            ProtocolRendererOptions renderOptions;
            string protocolSearchID;
            if ((protocolSearchID = Request.QueryString.Get("protocolsearchid")) != null)
            {
                hasProtocolSearchid = true;
                CTSearchDefinition criteria = CTSearchManager.LoadSavedCriteria(Convert.ToInt32(protocolSearchID));

                if (criteria.LocationSearchType == LocationSearchType.None)
                    renderOptions = null;
                else
                {
                    NameValueCollection queryStringSansProtocolSearchid = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    queryStringSansProtocolSearchid.Remove("protocolsearchid");
                    renderOptions = new ProtocolRendererOptions();
                    renderOptions.TrialSitesSeeAllUrl = PrettyUrl + "?" + queryStringSansProtocolSearchid.ToString();
                    renderOptions.TrialSitesSeeAllText = "See All Trial Sites";
                }

            }
            else
                renderOptions = null;

            ProtocolRenderer prProtocol = new ProtocolRenderer(base.PageDisplayInformation, pProtocol, renderOptions, "");
            this.strContent += prProtocol.Render();

            if (this.PageDisplayInformation.Version == DisplayVersions.Print)
            {
                string printContent = "";

                //***Disable Glossary Terms and Footnote extraction*** 
                //string glossaryTableTitle = "Glossary Terms";
                //string linksTableTitle = "Table of Links";
                //if (this.PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.Spanish)
                //{
                //    glossaryTableTitle = "Glosario";
                //    linksTableTitle = "Lista de Enlaces";
                //}

                CancerGov.Common.Extraction.GlossaryTermExtractor gte = new CancerGov.Common.Extraction.GlossaryTermExtractor();
                CancerGov.Common.Extraction.FootnoteExtractor fe = new CancerGov.Common.Extraction.FootnoteExtractor();
                fe.ExcludeList = new string[] { "http://www.ncbi.nlm.nih.gov/entrez/query.fcgi?" };
                fe.RemoveReturnToTopBar = false;

                //***Disable Glossary Terms and Footnote extraction*** 
                printContent = strContent;
                //printContent = gte.ExtractGlossaryTerms(strContent);
                //printContent = fe.Extract(new Regex("<a\\s+?(?:class=\".*?\"\\s+?)*?href=\"(?<extractValue>.*?)\"(?:\\s+?\\w+?=\"(?:.*?)\")*?\\s*?>(?<linkText>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline), "extractValue", CancerGov.Common.Extraction.ExtractionTypes.URL, printContent);
                //printContent += gte.BuildGlossaryTable(glossaryTableTitle);
                //printContent += fe.GetFootnotes(linksTableTitle, 80);

                this.strContent = "<table cellspacing=\"0\" width=\"650\" cellpadding=\"0\" border=\"0\"><tr><td width=\"650\">" + printContent + "</td></tr></table>\n";
            }
            
            //// Web Analytics *************************************************
            //this.WebAnalyticsPageLoad.SetChannelFromSectionNameAndUrl("Clinicaltrials", this.Request.Url.OriginalString.ToString());
            this.PageInstruction.AddFieldFilter("channelName", (name, data) =>
            {
                data.Value = "Clinicaltrials";
            });

            // Add clinical trial view count to analytics 
            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.evar22, wbField =>
            {
                wbField.Value = "+1";
            });

            // Add clinical trial view event to analytics
            if (hasProtocolSearchid)
                // If viewed from a search
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event4, wbField =>
                {
                    wbField.Value = "";
                });
            else
                // If not viewed from a search
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.event13, wbField =>
                {
                    wbField.Value = "";
                });

            //// End Web Analytics *********************************************

            // Set the URL needed for PageOption Print
            this.PageInstruction.AddUrlFilter("Print", (name, url) =>
            {
                url.SetUrl(this.PageInstruction.GetUrl("CurrentURL").ToString() + "/print?" + Request.QueryString.ToString());
            });

            this.PageInstruction.AddUrlFilter("EmailUrl", (name, url) =>
            {
                foreach (string key in Request.QueryString)
                    url.QueryParameters.Add(key, Request.QueryString[key]);
            });

            this.PageInstruction.AddUrlFilter("BookMarkShareUrl", (name, url) =>
            {
                foreach (string key in Request.QueryString)
                    url.QueryParameters.Add(key, Request.QueryString[key]);
            });

            this.PageInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                string localUrl = url.ToString();

                if (iProtocolID > -1)
                    localUrl += "?cdrid=" + iProtocolID;

                if (pvVersion.ToString() != "")
                    localUrl += "?version=" + pvVersion;

                if (iProtocolSearchID > 0)
                    localUrl += "?protocolsearchid=" + iProtocolSearchID;

                url.SetUrl(localUrl);
            });


        }

        #region ISupportingSnippet Members

        public SnippetControl[] GetSupportingSnippets()
        {
            if (this.PageDisplayInformation.Version != DisplayVersions.Print)
            {
                // We need this snippet to help us render the version tab.
                SnippetControl snippetControl = (SnippetControl)Page.LoadControl("~/SnippetTemplates/ClinicalTrialsViewHeader.ascx");
                SnippetInfo snippetInfo = new SnippetInfo();
                snippetInfo.SlotName = "cgvContentHeader";
                snippetInfo.ContentID = "dynamicClinicalTrialsView1";
                snippetControl.SnippetInfo = snippetInfo;
                SnippetControl[] supportingControls = { snippetControl };
                return supportingControls;
            }
            else
                return null;
        }

        #endregion
    }
}

