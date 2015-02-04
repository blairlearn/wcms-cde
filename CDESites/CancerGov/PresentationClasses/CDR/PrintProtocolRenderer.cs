using System;
using System.Text;

using CancerGov.CDR.DataManager;
using CancerGov.UI.CDR;
using CancerGov.Text;
using NCI.Web.CDE;

namespace CancerGov.UI.CDR {
	/// <summary>
	/// The clase renders a single protocol for printing
	/// </summary>
	public class PrintProtocolRenderer : ProtocolRenderer, IRenderer {
		//Need to separate title from content.

        ProtocolDisplayFormats _displayFormat;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="basePage">A BasePage object</param>
		/// <param name="protocol">A protocol</param>
        public PrintProtocolRenderer(DisplayInformation displayInfo, Protocol protocol, ProtocolDisplayFormats displayFormat, string detailedViewPage)
            : base(displayInfo, protocol, null, detailedViewPage)
        {
            _displayFormat = displayFormat;
		}

		/// <summary>
		/// Renders a title with date
		/// </summary>
		/// <returns>String of HTML</returns>
        protected new string RenderTitle()
        {

            StringBuilder sbContent = new StringBuilder();

            sbContent.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n");

            //Draw the title, everything has to have a title
            sbContent.AppendFormat("<td valign=\"top\"><label for=\"cdrid{1}\"><a href=\"{0}?cdrid={1}",
                DetailedViewPage, pProtocol.CdrId);

            sbContent.Append("&version=");
            sbContent.Append(pProtocol.ProtocolVersion.ToString());

            if (pProtocol.ProtocolSearchID != -1)
            {
                sbContent.Append("&protocolsearchid=");
                sbContent.Append(pProtocol.ProtocolSearchID);
            }

            sbContent.Append("\">");
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

                if (pProtocol.DateFirstPublished != DateTime.MinValue)
                {
                    sbContent.Append("<span class=\"protocol-date-label\">yFirst Published: </span>");
                    sbContent.Append("<span class=\"protocol-dates\">");
                    sbContent.Append(pProtocol.DateFirstPublished.ToString("d"));
                    sbContent.Append("</span>");
                    if (pProtocol.DateLastModified != new DateTime(0))
                    {
                        sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
                    }
                    else
                    {
                        sbContent.Append("&nbsp;&nbsp;");
                    }                    
                }

                if (pProtocol.DateLastModified != DateTime.MinValue)
                {
                    sbContent.Append("<span class=\"protocol-date-label\">Last Modified: </span>");
                    sbContent.Append("<span class=\"protocol-dates\">");
                    sbContent.Append(pProtocol.DateLastModified.ToString("d"));
                    sbContent.Append("</span>");
                    sbContent.Append("&nbsp;&nbsp;");
 
                }

            }

            sbContent.Append("</td>");
            sbContent.Append("</tr>");

            sbContent.Append("</table>\n");

            return sbContent.ToString();
        }


		/// <summary>
		/// Renders the protocol out to html for print
		/// </summary>
		/// <param name="iResultNumber">The index of the protocol</param>
		/// <returns>A string of html</returns>
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
                                    
                                    sbContent.Append("<h2 id=\"ContactInfo_");
                                    sbContent.Append(pProtocol.FullCdrId);
                                    sbContent.Append("\">Trial Contact Information</h2>\n");

									bDrawnTCI = true;
								}

								sbContent.Append(sbTmp.ToString());
							}


							break;

						case ProtocolSectionTypes.CTGovBriefSummary :
						case ProtocolSectionTypes.CTGovDetailedDescription :
						case ProtocolSectionTypes.CTGovEntryCriteria :

							if (pProtocol.Sections.Contains(iSection)) {

								if (!bWroteTrialDesc) {
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
