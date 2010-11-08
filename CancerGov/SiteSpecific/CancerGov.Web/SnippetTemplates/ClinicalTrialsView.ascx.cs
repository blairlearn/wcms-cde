using System;
using System.Collections;
using System.Collections.Specialized;   // In order to reference Prototype.
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CancerGov.CDR.ClinicalTrials.Helpers;
using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.CDR.DataManager;
using CancerGov.Common.HashMaster;
using CancerGov.UI.CDR;
using CancerGov.UI.HTML;
using CancerGov.UI.PageObjects;
using NCI.Logging;
using NCI.Util;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.UI;
using NCI.Web.UI.WebControls;
using NCI.Web.UI.WebControls.FormControls;
using NCI.Web.UI.WebControls.JSLibraries;
using System.Text.RegularExpressions;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsView : NCI.Web.CancerGov.Apps.AppsBaseUserControl
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
                if (url.Equals("/search/ClinicalTrialsResults.aspx") || url.Equals("/search/Results_ClinicalTrials.aspx"))
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

            //this.PageHtmlHead.Title = "National Cancer Institute - Clinical Trials (PDQ&#174;)";
            //this.PageHtmlHead.Title = "Clinical Trials (PDQ&#174;) - National Cancer Institute";
            //for protocal title as page title (SCR: 30153), see Line: ~156
            // -------------------------------------------
            // 6-22-09 This code was used to overcome the issue related to CTSearchPage loading 
            // generic navigation because it is also used by SearchCancerTopics.  Ultimately the solution 
            // will be to have CTSearchPage use "nav_clinical_trials" and to have SearchCancerTopic not 
            // inherit from CTSearchPage.  For now the problem is an error when displaying the print version
            // that does not have a NavigationBar - to fix this problem we will check to see if the 
            // NavigationBar is null before we set anything.  
            // ------------------------------------------
            //if ( !(this.pageBanner.NavigationBar == null))
            //    this.pageBanner.NavigationBar.SelectedTabImg = "nav_clinical_trials";

            ProtocolVersions pvVersion = ProtocolVersions.HealthProfessional;
            iProtocolID = Strings.ToInt(Request.Params["cdrid"]);

            if (iProtocolID < 1)
            {

                NCI.Logging.Logger.LogError("ViewClinicalTrials.OnLoad", "No ProtocalID Specified", NCIErrorLevel.Error);
                this.RaiseErrorPage("No ProtocalID Specified");
            }

            iProtocolSearchID = Strings.ToInt(Strings.IfNull(Strings.Clean(Request.Params["protocolsearchid"]), "0"));
            strVersion = Strings.IfNull(Strings.Clean(Request.Params["version"]), "healthprofessional");

            if (strVersion.ToUpper() == "PATIENT")
            {
                pvVersion = ProtocolVersions.Patient;
            }


            //Get Related Links Info Box
            #region TODO RELATED LINKS INFO BOX
            //if (pvVersion == ProtocolVersions.Patient)
            //{
            //    Guid gLinksList = Strings.ToGuid(ConfigurationSettings.AppSettings["ClinicalTrialsPatientRelatedLinksList"]);

            //    if (gLinksList != Guid.Empty)
            //    {
            //        List lLinksList = new List(gLinksList);

            //        if ((lLinksList != null) && (lLinksList.Count > 0))
            //        {
            //            InfoBoxListStyle iblsStyle = new InfoBoxListStyle();
            //            BaseList blLinksList = new BaseList(lLinksList, iblsStyle);

            //            if (blLinksList != null)
            //            {
            //                InfoBoxStyle ibs = new InfoBoxStyle(159);
            //                ibs.ShowHeader = true;
            //                InfoBox ibxListbox = new InfoBox(blLinksList, "Related Links", ibs, (BasePage)this);
            //                StringBuilder sbContent = new StringBuilder();
            //                sbContent.Append("<table border=\"0\" width=\"165\" cellpadding=\"0\" cellspacing=\"0\" align=\"right\">\n");
            //                sbContent.Append("<tr>\n");
            //                sbContent.Append("<td valign=\"top\" width=\"6\"><img src=\"/images/spacer.gif\" width=\"3\" height=\"1\" alt=\"\"></td>\n");
            //                sbContent.Append("<td valign=\"top\" width=\"159\">\n");
            //                sbContent.Append(ibxListbox.Render());
            //                sbContent.Append("</td>\n");
            //                sbContent.Append("</tr>\n");
            //                sbContent.Append("<tr>\n");
            //                sbContent.Append("<td valign=\"top\" colspan=\"2\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"3\" alt=\"\"></td>\n");
            //                sbContent.Append("</tr>\n");
            //                sbContent.Append("</table>\n");
            //                strContent = sbContent.ToString();
            //            }
            //        }
            //    }
            //}
            #endregion


            Protocol pProtocol = null;
            try
            {
                pProtocol = new Protocol(iProtocolID, ProtocolFunctions.GetSectionList(pvVersion, ProtocolDisplayFormats.SingleProtocol), iProtocolSearchID, pvVersion);
            }
            catch (ProtocolFetchFailureException fetchError)
            {

                NCI.Logging.Logger.LogError("ViewClinicalTrials", "ProtocolID = " + iProtocolID + " Error: " + fetchError.Message, NCIErrorLevel.Error, fetchError);
                this.RaiseErrorPage("Error:" + fetchError.Message);
            }
            catch (ProtocolTableEmptyException fetchError)
            {

                NCI.Logging.Logger.LogError("ViewClinicalTrials", "ProtocolID = " + iProtocolID + " Error: " + fetchError.Message, NCIErrorLevel.Error, fetchError);
                this.RaiseErrorPage("Error:" + fetchError.Message);
            }
            catch (ProtocolTableMiscountException fetchError)
            {

                NCI.Logging.Logger.LogError("ViewClinicalTrials", "ProtocolID = " + iProtocolID + " Error: " + fetchError.Message, NCIErrorLevel.Error, fetchError);
                this.RaiseErrorPage("Error:" + fetchError.Message);
            }

            //Fix related to SCR: 30153 - client decided to impliment 
            //this.PageHtmlHead.Title = pProtocol.ProtocolTitle + " - National Cancer Institute";

            StringBuilder sbDate = new StringBuilder();

            if (pProtocol.ProtocolType == ProtocolTypes.Protocol)
            {
                if (pProtocol.DateLastModified != new DateTime(0))
                {
                    sbDate.Append("<span class=\"protocol-date-label\">Last Modified: </span>");
                    sbDate.Append("<span class=\"protocol-dates\">");
                    sbDate.Append(pProtocol.DateLastModified.ToString("d"));
                    sbDate.Append("</span>");

                    if (pProtocol.DateFirstPublished != new DateTime(0))
                    {
                        sbDate.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
                    }
                    else
                    {
                        sbDate.Append("&nbsp;&nbsp;");
                    }
                }

                if (pProtocol.DateFirstPublished != new DateTime(0))
                {
                    sbDate.Append("<span class=\"protocol-date-label\">First Published: </span>");
                    sbDate.Append("<span class=\"protocol-dates\">");
                    sbDate.Append(pProtocol.DateFirstPublished.ToString("d"));
                    sbDate.Append("</span>");
                    sbDate.Append("&nbsp;&nbsp;");
                }
            }
            //SCR30153
            //this.PageHtmlHead.Title = pProtocol.ProtocolTitle;

            StringBuilder sbPageUrl = new StringBuilder();
            sbPageUrl.Append("/search/ViewClinicalTrials.aspx");
            sbPageUrl.Append("?cdrid=");
            sbPageUrl.Append(iProtocolID.ToString());



            if (iProtocolSearchID > 0)
            {
                sbPageUrl.Append("&protocolsearchid=");
                sbPageUrl.Append(iProtocolSearchID.ToString());
            }

            if (this.PageDisplayInformation.Version != DisplayVersions.Print)
            {
                //CancerGov.UI.HTML.HtmlImage headerImage = new CancerGov.UI.HTML.HtmlImage("/images/title_clinical_trials.jpg", "", "160", "60");
                //headerImage.DisplayInfo = this.PageDisplayInformation;
                //TitleBlock tbTitle = new TitleBlock("Clinical Trials (PDQ<sup class=\"header\">&#174;</sup>)", headerImage, this.PageDisplayInformation);

                //tbTitle.Gutter.Add(new CDRVersionBar((BasePage)this, pvVersion, sbDate.ToString(), sbPageUrl.ToString()));
                //this.PageHeaders.Add(tbTitle);
                //this.PageLeftColumn = new LeftNavColumn(this, Strings.ToGuid(ConfigurationSettings.AppSettings["ClinicalTrialSearchLeftViewID"]));
                //this.PageLeftColumn.Insert(0, new ProtocolContentVersionBox((BasePage)this, pProtocol));
            }


            // Add "See all Trial Sites" link if protocolsearchid is in the QueryString
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
                    renderOptions.TrialSitesSeeAllUrl = Request.FilePath + "?" + queryStringSansProtocolSearchid.ToString();
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
                string glossaryTableTitle = "Glossary Terms";
                string linksTableTitle = "Table of Links";

                if (this.PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.Spanish)
                {
                    glossaryTableTitle = "Glosario";
                    linksTableTitle = "Lista de Enlaces";
                }

                CancerGov.Common.Extraction.GlossaryTermExtractor gte = new CancerGov.Common.Extraction.GlossaryTermExtractor();
                CancerGov.Common.Extraction.FootnoteExtractor fe = new CancerGov.Common.Extraction.FootnoteExtractor();
                fe.ExcludeList = new string[] { "http://www.ncbi.nlm.nih.gov/entrez/query.fcgi?" };
                fe.RemoveReturnToTopBar = false;

                printContent = gte.ExtractGlossaryTerms(strContent);
                printContent = fe.Extract(new Regex("<a\\s+?(?:class=\".*?\"\\s+?)*?href=\"(?<extractValue>.*?)\"(?:\\s+?\\w+?=\"(?:.*?)\")*?\\s*?>(?<linkText>.*?)</a>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline), "extractValue", CancerGov.Common.Extraction.ExtractionTypes.URL, printContent);
                printContent += gte.BuildGlossaryTable(glossaryTableTitle);
                printContent += fe.GetFootnotes(linksTableTitle, 80);

                this.strContent = "<table cellspacing=\"0\" width=\"650\" cellpadding=\"0\" border=\"0\"><tr><td width=\"650\">" + printContent + "</td></tr></table>\n";
            }
            else
            {
                this.strContent += new ReturnToTopAnchor(this.PageDisplayInformation).Render();
            }

            //// Web Analytics *************************************************
            //this.WebAnalyticsPageLoad.SetChannelFromSectionNameAndUrl("Clinicaltrials", this.Request.Url.OriginalString.ToString());
            this.PageInstruction.AddFieldFilter("channelName", (name,data) =>
            {
                data.Value = "Clinicaltrials";
            });

            this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.eVars.ClinicalTrialViewCount, wbField =>
            {
                wbField.Value = "+1";
            });

            if (hasProtocolSearchid)
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.ClinicalTrialViewFromSearch, wbField =>
                {
                    wbField.Value = "";
                });
            else
                this.PageInstruction.SetWebAnalytics(WebAnalyticsOptions.Events.ClinicalTrialViewNotSearch, wbField =>
                {
                    wbField.Value = "";
                });

            //// End Web Analytics *********************************************

        }

    }
}

