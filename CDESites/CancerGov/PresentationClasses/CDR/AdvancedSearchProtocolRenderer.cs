using System;
using System.Collections;
using System.Text;

using CancerGov.CDR.DataManager;
using CancerGov.UI.CDR;
using CancerGov.Text;
using NCI.Web.CDE;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.UI.CDR
{
	/// <summary>
	/// Summary description for AdvancedSearchProtocolRenderer.
	/// Because of the layout this might have nothing to do with the protocolrenderer
	/// </summary>
	public class AdvancedSearchProtocolRenderer : ProtocolRenderer, IRenderer {
		//Need to separate title from content.

		private ArrayList alCDRIDs;
        private ProtocolDisplayFormats _displayFormat;

        public AdvancedSearchProtocolRenderer(DisplayInformation displayInfo    , Protocol protocol,
            ArrayList cdrIDs, ProtocolDisplayFormats displayFormat,string detailedViewPage)
            : base(displayInfo, protocol, null,detailedViewPage)
        {
            this.alCDRIDs = cdrIDs;
            _displayFormat = displayFormat;
        }



        protected new string RenderTitle()
        {

            StringBuilder sbContent = new StringBuilder();

            sbContent.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n");

            //Draw the title, everything has to have a title
            sbContent.AppendFormat("<tr>\n<td valign=\"top\"><label for=\"cdrid{1}\"><a href=\"{0}?cdrid={1}",
                DetailedViewPage, pProtocol.CdrId);

            sbContent.Append("&version=");
            sbContent.Append(pProtocol.ProtocolVersion.ToString());

            if (pProtocol.ProtocolSearchID != -1)
            {
                sbContent.Append("&protocolsearchid=");
                sbContent.Append(pProtocol.ProtocolSearchID);
            }

            sbContent.Append("\" class=\"protocol-abstract-link\" ");
            // Web Analytics *************************************************
            if (WebAnalyticsOptions.IsEnabled)
                sbContent.Append("onclick=\"NCIAnalytics.CTSearchResults(this,'" + this.pProtocol.ResultNumber.ToString() + "');\"");
            // End Web Analytics **********************************************
            sbContent.Append(" >");
    
            sbContent.Append(pProtocol.ProtocolTitle);
            sbContent.Append("</a></label></td>\n");
            sbContent.Append("</tr>\n");

            // The row is always rendered for spacing purposes.  The contents are dependent on the data.
            sbContent.Append("<tr>\n");
            sbContent.Append("<td valign=\"top\" align=\"right\"><br>\n");

            // Don't show dates if the display format is ProtocolDisplayFormats.Short
            // Don't show dates for CTGov protocols since they are usually wrong.
            // Do require at least one date to be present.
            if ((_displayFormat != ProtocolDisplayFormats.Short) &&
                (pProtocol.ProtocolType == ProtocolTypes.Protocol))
            {

                if (pProtocol.DateLastModified != DateTime.MinValue)
                {
                    sbContent.Append("<span class=\"protocol-date-label\">Last Modified: </span>");
                    sbContent.Append("<span class=\"protocol-dates\">");
                    sbContent.Append(pProtocol.DateLastModified.ToString("d"));
                    sbContent.Append("</span>");

                    if (pProtocol.DateFirstPublished != DateTime.MinValue)
                    {
                        sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
                    }
                    else
                    {
                        sbContent.Append("&nbsp;&nbsp;");
                    }
                }

                if (pProtocol.DateFirstPublished != DateTime.MinValue)
                {
                    sbContent.Append("<span class=\"protocol-date-label\">xFirst Published: </span>");
                    sbContent.Append("<span class=\"protocol-dates\">");
                    sbContent.Append(pProtocol.DateFirstPublished.ToString("d"));
                    sbContent.Append("</span>");
                    sbContent.Append("&nbsp;&nbsp;");
                }
            }

            sbContent.Append("</td>");
            sbContent.Append("</tr>");

            sbContent.Append("</table>\n");

            return sbContent.ToString();
        }



		public new string Render() {

			StringBuilder sbContent = new StringBuilder();

			bool bDrawnLeadOrg = false;
			bool bDrawnStudySites = false;
			bool bWroteTrialDesc = false;
			bool bDrawnTCI = false;
			StringBuilder sbTmp = null;


			foreach (string strSectionType in pProtocol.SectionList.Split(',')) {
				
				int iSection = 0;

				iSection = Strings.ToInt(strSectionType);
				
				if (iSection > 0) {
					switch ((ProtocolSectionTypes)iSection) {
						
						case ProtocolSectionTypes.Title : 
							sbContent.Append(RenderTitle());
							break;

						case ProtocolSectionTypes.AlternateTitle :
							sbContent.Append(RenderAlternateTitle());
							break;

						case ProtocolSectionTypes.InfoBox : 
							sbContent.Append(RenderInfoBox(_displayFormat != ProtocolDisplayFormats.Short));
							break;

						case ProtocolSectionTypes.CTGovLeadOrgs :
						case ProtocolSectionTypes.LeadOrgs :
							sbTmp = new StringBuilder();

							sbTmp.Append(pProtocol.GetSectionByID((ProtocolSectionTypes)iSection));

							if (sbTmp.Length >= 1) {
								bDrawnLeadOrg = true; //For stupid Trial Contact Information
								
								if (!bDrawnTCI) { //For stupid Trial Contact Information
									bDrawnTCI = true;
                                    //sbContent.Append("<p>\n");
                                    //sbContent.Append("<a name=\"ContactInfo_");
                                    //sbContent.Append(pProtocol.FullCdrId);
                                    //sbContent.Append("\"></a>");
                                    //sbContent.Append("<span class=\"Protocol-Section-Heading\">Trial Contact Information</span>\n");
                                    //sbContent.Append("<p>\n");
                                    sbContent.Append("<h2 id=\"ContactInfo_");
                                    sbContent.Append(pProtocol.FullCdrId);
                                    sbContent.Append("\">Trial Contact Information</h2>\n");
								}

								sbContent.Append(sbTmp.ToString());
							}


							break;

						case ProtocolSectionTypes.CTGovBriefSummary :
						case ProtocolSectionTypes.CTGovDetailedDescription :
						case ProtocolSectionTypes.CTGovEntryCriteria :

							if (pProtocol.Sections.Contains(iSection)) {

								if (!bWroteTrialDesc) {
                                    //sbContent.Append("<p>\n");
                                    //sbContent.Append("<a name=\"TrialDescription_");
                                    //sbContent.Append(pProtocol.FullCdrId);
                                    //sbContent.Append("\"></a>");
                                    //sbContent.Append("<span class=\"Protocol-Section-Heading\">Trial Description</span>\n");
                                    //sbContent.Append("<p>\n");	
                                    sbContent.Append("<h2 id=\"TrialDescription_");
                                    sbContent.Append(pProtocol.FullCdrId);
                                    sbContent.Append("\">Trial Description</h2>\n");
						
									bWroteTrialDesc = true;
								}
							
								sbContent.Append(pProtocol.GetSectionByID((ProtocolSectionTypes)iSection));

							}

							break;

						case ProtocolSectionTypes.StudySites : 

							if ((pProtocol.Sites != null) && (pProtocol.Sites.SiteTable != null)) {
								bDrawnStudySites = true;
								if (!bDrawnTCI) { //For stupid Trial Contact Information
                                    //sbContent.Append("<p>\n");
                                    //sbContent.Append("<a name=\"ContactInfo_");
                                    //sbContent.Append(pProtocol.FullCdrId);
                                    //sbContent.Append("\"></a>");
                                    //sbContent.Append("<span class=\"Protocol-Section-Heading\">Trial Contact Information</span>\n");
                                    //sbContent.Append("<p>\n");

                                    sbContent.Append("<h2 id=\"ContactInfo_");
                                    sbContent.Append(pProtocol.FullCdrId);
                                    sbContent.Append("\">Trial Contact Information</h2>\n");

									bDrawnTCI = true;
								}

								sbContent.Append(this.RenderStudySites());
							}
							break;

						default :	
							//These need a table wrapped around them, but only if they exist. Soo
							sbTmp = new StringBuilder();

							sbTmp.Append(pProtocol.GetSectionByID((ProtocolSectionTypes)iSection));

							if (sbTmp.Length >= 1) {
								sbContent.Append(sbTmp.ToString());
							}

							break;

					}
				}
			}

			return sbContent.ToString();
		}


	}

}
