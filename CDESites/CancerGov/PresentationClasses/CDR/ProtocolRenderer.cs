using System;
using System.Data;
using System.Text;

using CancerGov.CDR.DataManager;
using CancerGov.UI.CDR;
using CancerGov.Text;
using NCI.Web.CDE;

namespace CancerGov.UI.CDR
{
    /// <summary>
	/// Summary description for ProtocolRenderer.
	/// </summary>
	public class ProtocolRenderer : IRenderer
	{
		//This needs to be fixed!!!
		//public DisplayVersions displayVersion = DisplayVersion.Image;
		protected Protocol pProtocol;
        protected DisplayInformation displayInfo;
        private ProtocolRendererOptions prOptions;
        protected string DetailedViewPage=String.Empty;

        public ProtocolRenderer(DisplayInformation displayInfo, Protocol protocol, ProtocolRendererOptions options, string detailedViewPage)
        {
			this.pProtocol = protocol;
            this.displayInfo = displayInfo;
            this.prOptions = options;
            this.DetailedViewPage = detailedViewPage;
		}

		public string Render() {

			StringBuilder sbContent = new StringBuilder();

			bool bWroteTrialDesc = false;
			bool bDrawnTCI = false;

            //add this div for accordion to work on mobile
            sbContent.Append("<div class=\"accordion\">");

			foreach (string strSectionType in pProtocol.SectionList.Split(',')) {
				
				int iSection = 0;

				iSection = Strings.ToInt(strSectionType);
				
				if (iSection > 0) {
                    
					switch ((ProtocolSectionTypes)iSection) {
						
						case ProtocolSectionTypes.Title : 
							sbContent.Append(RenderTitle());
							break;

						case ProtocolSectionTypes.TableOfContents :
							sbContent.Append(RenderTOC());
							break;

						case ProtocolSectionTypes.AlternateTitle :
							sbContent.Append(RenderAlternateTitle());
							break;

						case ProtocolSectionTypes.InfoBox :
                             
                            sbContent.Append("<h2 id=\"StudyIdInfo_");
                            sbContent.Append(pProtocol.FullCdrId);
                            sbContent.Append("\">Basic Trial Information</h2>\n");
							sbContent.Append(this.RenderInfoBox());
							break;
						
						case ProtocolSectionTypes.StudySites : 
							if ((pProtocol.Sites != null) && (pProtocol.Sites.SiteTable != null)) {
								if (!bDrawnTCI) {
                                    sbContent.Append("<h2 id=\"ContactInfo_");
                                    sbContent.Append(pProtocol.FullCdrId);
                                    sbContent.Append("\">Trial Contact Information</h2>\n");

									bDrawnTCI = true;
								}
								sbContent.Append(this.RenderStudySites());
							}
							break;

						case ProtocolSectionTypes.CTGovLeadOrgs : 
						case ProtocolSectionTypes.LeadOrgs : 
							if (!bDrawnTCI) {
                                sbContent.Append("<h2 id=\"ContactInfo_");
                                sbContent.Append(pProtocol.FullCdrId);
                                sbContent.Append("\">Trial Contact Information</h2>\n");

								bDrawnTCI = true;
							}
							sbContent.Append(pProtocol.GetSectionByID((ProtocolSectionTypes)iSection));

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
						
						default :	
							if (pProtocol.Sections != null) {
								sbContent.Append(pProtocol.GetSectionByID((ProtocolSectionTypes)iSection));
							}
							break;

					}
                   
				}
			}

            sbContent.Append("</div>");
			
			return sbContent.ToString();
		}


		protected virtual string RenderTitle() {

			StringBuilder sbContent = new StringBuilder();

			//Title is being set in the ClinicalTrialsView.ascx.cs control 
            //and should not being rendered here.

            //sbContent.Append("<h1>");
            //sbContent.Append(pProtocol.ProtocolTitle);
            //sbContent.Append("</h1>\n");

			// for some reason this is drawn in a separate place when it's not a printable version
			// also, don't show the dates for CTGov Protocols, since they are mostly wrong
            if ((this.displayInfo.Version == DisplayVersions.Print) && (this.pProtocol.ProtocolType == ProtocolTypes.Protocol))
			{
				sbContent.Append("<BR>");

                if (pProtocol.DateFirstPublished != new DateTime(0)) 
				{
                    sbContent.Append("<span class=\"protocol-date-label\">First Published: </span>");
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

                if (pProtocol.DateLastModified != new DateTime(0)) 
				{
                    sbContent.Append("<span class=\"protocol-date-label\">Last Modified: </span>");
                    sbContent.Append("<span class=\"protocol-dates\">");
                    sbContent.Append(pProtocol.DateLastModified.ToString("d"));
                    sbContent.Append("</span>");
					sbContent.Append("&nbsp;&nbsp;");
				}
			}

			//sbContent.Append("<p>\n"); //<---- note we draw <p> tags after a section has been drawn

			return sbContent.ToString();
		}


		protected string RenderAlternateTitle(){

			StringBuilder sbContent = new StringBuilder();

			//Now for the alternate title... We only show  if we have one.
            if ((pProtocol.AlternateTitle != null) && (pProtocol.AlternateTitle != "") && (pProtocol.AlternateTitle != pProtocol.ProtocolTitle))
            {
                sbContent.Append("<h2 id=\"AlternateTitle_");
                sbContent.Append(pProtocol.FullCdrId);
                sbContent.Append("\">Alternate Title</h2>\n");
                sbContent.Append("<p>" + pProtocol.AlternateTitle + "</p>");
                sbContent.Append("\n");


            } //else the titles are the same so don't show them

			return sbContent.ToString();
		}

		protected string RenderTOC () 
		{

			StringBuilder sbContent = new StringBuilder();

            //create an empty div to add the CTGovProtocol table of contents using Javascript
            sbContent.Append("<div id=\"pdq-toc-protocol\"></div>");

			return sbContent.ToString();
		}

		#region Study Sites
		protected string RenderStudySites() {
			StringBuilder sbContent = new StringBuilder();
			StringBuilder sbUSA = new StringBuilder();
			StringBuilder sbWorld = new StringBuilder();

			DataView dvStudySites = pProtocol.Sites.SiteTable;

			if (dvStudySites.Count > 0) {

                sbContent.Append("<h3 do-not-show=\"toc\">Trial Sites</h3>\n");

                string strPrevOrgname = string.Empty;
                string strPrevCountry = string.Empty;
                string strPrevState = string.Empty;
                string strPrevCity = string.Empty;
				
				foreach (DataRowView drvSite in dvStudySites) {

					if (drvSite.Row["Country"].ToString() == "U.S.A.") {

						if (strPrevCountry == (string)drvSite.Row["Country"]) { //do not draw country
						
							if (strPrevState == (string)drvSite.Row["State"]) { //do not draw state

								if (strPrevCity == (string)drvSite.Row["City"]) { //do not draw city

                                    if (strPrevOrgname != (string)drvSite.Row["OrganizationName"])
                                    {
                                        sbUSA.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                                        strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                                    }
									sbUSA.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());
					
								} else { // new city

                                    if(!string.IsNullOrEmpty(strPrevCity) && sbUSA.Length > 0)
                                        sbUSA.Append("</div>"); // Close previous city.

                                    sbUSA.Append("<div class=\"study-site-city\">");
									sbUSA.AppendFormat("<h6>{0}</h6>\n", drvSite.Row["City"].ToString());
                                    sbUSA.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                                    sbUSA.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());

                                    strPrevCity = drvSite.Row["City"].ToString();
                                    strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
								}

							} else {  // new state

                                if (!string.IsNullOrEmpty(strPrevCity) && sbUSA.Length > 0)
                                    sbUSA.Append("</div>"); // Close previous city.
                                if (!string.IsNullOrEmpty(strPrevState) && sbUSA.Length > 0)
                                    sbUSA.Append("</div>"); // Close previous state.

                                sbUSA.Append("<div class=\"study-site-state\">");
                                if (drvSite.Row["State"].ToString() != "")
                                {
                                    sbUSA.AppendFormat("<h5>{0}</h5>\n", drvSite.Row["State"].ToString());
								}
                                sbUSA.Append("<div class=\"study-site-city\">");
                                sbUSA.AppendFormat("<h6>{0}</h6>\n", drvSite.Row["City"].ToString());
                                sbUSA.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                                sbUSA.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());

								strPrevState = drvSite.Row["State"].ToString();
                                strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                                strPrevCity = drvSite.Row["City"].ToString();
							}

						} else { // Change country.
                            if (!string.IsNullOrEmpty(strPrevCity) && sbUSA.Length > 0)
                                sbUSA.Append("</div>"); // Close previous city.
                            if (!string.IsNullOrEmpty(strPrevState) && sbUSA.Length > 0)
                                sbUSA.Append("</div>"); // Close previous state.

                            sbUSA.AppendFormat("<h4 do-not-show=\"toc\">{0}</h4>\n", drvSite.Row["Country"].ToString());

                            sbUSA.Append("<div class=\"study-site-state\">");
                            if (drvSite.Row["State"].ToString() != "")
                            {
                                sbUSA.AppendFormat("<h5>{0}</h5>\n", drvSite.Row["State"].ToString());
							}

                            sbUSA.Append("<div class=\"study-site-city\">");
                            sbUSA.AppendFormat("<h6>{0}</h6>\n", drvSite.Row["City"].ToString());
                            sbUSA.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                            sbUSA.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());
						
							strPrevCountry = drvSite.Row["Country"].ToString();
							strPrevState = drvSite.Row["State"].ToString();
							strPrevCity = drvSite.Row["City"].ToString();
                            strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                        }
					} else { // NON-USA
						if (strPrevCountry == (string)drvSite.Row["Country"]) { //do not draw country
							if (strPrevState == (string)drvSite.Row["State"]) { //do not draw state
								if (strPrevCity == (string)drvSite.Row["City"]) { //do not draw city
                                    if (strPrevOrgname != (string)drvSite.Row["OrganizationName"])
                                    {
                                        sbWorld.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                                        strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                                    }
                                    sbWorld.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());
								} else { // new city

                                    if (!string.IsNullOrEmpty(strPrevCity) && sbWorld.Length > 0)
                                        sbWorld.Append("</div>"); // Close previous city.

                                    sbWorld.Append("<div class=\"study-site-city\">");

                                    sbWorld.AppendFormat("<h6>{0}</h6>\n", drvSite.Row["City"].ToString());
                                    sbWorld.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                                    sbWorld.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());

									strPrevCity = drvSite.Row["City"].ToString();
                                    strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                                }

							} else { // new state

                                if (!string.IsNullOrEmpty(strPrevCity) && sbWorld.Length > 0)
                                    sbWorld.Append("</div>"); // Close previous city.
                                if (!string.IsNullOrEmpty(strPrevState) && sbWorld.Length > 0)
                                    sbWorld.Append("</div>"); // Close previous state.

                                sbWorld.Append("<div class=\"study-site-state\">");
                                if (drvSite.Row["State"].ToString() != "")
                                {
                                    sbWorld.AppendFormat("<h5>{0}</h5>\n", drvSite.Row["State"].ToString());
								}
                                sbWorld.Append("<div class=\"study-site-city\">");
                                sbWorld.AppendFormat("<h6>{0}</h6>\n", drvSite.Row["City"].ToString());
                                sbWorld.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                                sbWorld.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());

								strPrevState = drvSite.Row["State"].ToString();
								strPrevCity = drvSite.Row["City"].ToString();
                                strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                            }

						} else { // Change country.
                            if (!string.IsNullOrEmpty(strPrevCity) && sbWorld.Length > 0)
                                sbWorld.Append("</div>"); // Close previous city.
                            if (!string.IsNullOrEmpty(strPrevState) && sbWorld.Length > 0)
                                sbWorld.Append("</div>"); // Close previous state.

                            sbWorld.AppendFormat("<h4 do-not-show=\"toc\">{0}</h4>\n", drvSite.Row["Country"].ToString());

                            sbWorld.Append("<div class=\"study-site-state\">");
							if (drvSite.Row["State"].ToString() != "") {
                                sbWorld.AppendFormat("<h5>{0}</h5>\n", drvSite.Row["State"].ToString());
							}

                            sbWorld.Append("<div class=\"study-site-city\">");
                            sbWorld.AppendFormat("<h6>{0}</h6>\n", drvSite.Row["City"].ToString());
                            sbWorld.AppendFormat("<p class=\"study-site-name\">{0}</p>\n", drvSite.Row["OrganizationName"].ToString());
                            sbWorld.AppendFormat("<p>{0}</p>\n", drvSite.Row["HTML"].ToString());
						
							strPrevCountry = drvSite.Row["Country"].ToString();
							strPrevState = drvSite.Row["State"].ToString();
							strPrevCity = drvSite.Row["City"].ToString();
                            strPrevOrgname = drvSite.Row["OrganizationName"].ToString();
                        }
					}
				}

                // Close out city and state.
                if(sbUSA.Length > 0)
                    sbUSA.Append("</div></div>\n");
                if(sbWorld.Length > 0)
                    sbWorld.Append("</div></div>\n");

				sbContent.Append(sbUSA.ToString());
				sbContent.Append(sbWorld.ToString());

                // if prOptions is not null, check to see if TrialSitesSeeAllUrl and Text are set - if so, render link with anchor tag navigation
                if (this.prOptions != null)
                {
                    if (this.prOptions.TrialSitesSeeAllText != "" || this.prOptions.TrialSitesSeeAllUrl != "")
                    {
                        sbContent.Append("<p class=\"note Protocol-AllTrialSites\">\n");
                        sbContent.Append("<a href=\"");
                        sbContent.Append(this.prOptions.TrialSitesSeeAllUrl + "#SitesAndContacts_" + pProtocol.FullCdrId);
                        sbContent.Append("\">");
                        sbContent.Append(this.prOptions.TrialSitesSeeAllText);
                        sbContent.Append("</a>\n");
                        sbContent.Append("</p>\n");
                    }
                }

			}

			return sbContent.ToString();

		}
		#endregion

		#region Info Box
        protected string RenderInfoBox()
        {
            return RenderInfoBox(true);
        }

        protected string RenderInfoBox(bool drawAsTable)
        {
            StringBuilder sbContent = new StringBuilder();

            if (drawAsTable)
            {
                //Draw Study info box
                sbContent.Append("<table class=\"table-default\">");
                sbContent.Append("<tr>");
                sbContent.Append("<th>Phase</th>");
                sbContent.Append("<th>Type</th>");
                sbContent.Append("<th>Status</th>");
                sbContent.Append("<th>Age</th>");
                sbContent.Append("<th>Trial IDs</th>");
                sbContent.Append("</tr>");
                sbContent.AppendFormat("<tr>");
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.Phase);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.TrialType);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.CurrentStatus);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.AgeRange);
                sbContent.AppendFormat("<td><strong>{0}</strong><br>{1}</td>", pProtocol.PrimaryProtocolID, pProtocol.AlternateProtocolIDs);
                sbContent.AppendFormat("</tr>");
                sbContent.Append("</table>");
            }
            else
            {
                string InfoBoxFormat = "<strong>{0}: </strong>{1}<br/>";

                // build protocol list with a comma separation instead of the linebreak used in
                // the tabular layout.
                string protocolList;
                if( string.IsNullOrEmpty(pProtocol.AlternateProtocolIDs))
                    protocolList = string.Format("<strong>{0}</strong>",
                        pProtocol.PrimaryProtocolID);
                else
                    protocolList = string.Format("<strong>{0}</strong>, {1}",
                        pProtocol.PrimaryProtocolID, pProtocol.AlternateProtocolIDs);

                sbContent.Append("<div>\n");
                sbContent.AppendFormat(InfoBoxFormat, "Status", pProtocol.CurrentStatus);
                sbContent.AppendFormat(InfoBoxFormat, "Phase", pProtocol.Phase);
                sbContent.AppendFormat(InfoBoxFormat, "Type", pProtocol.TrialType);
                sbContent.AppendFormat(InfoBoxFormat, "Age", pProtocol.AgeRange);
                sbContent.AppendFormat(InfoBoxFormat, "Trial IDs", protocolList);
                sbContent.Append("</div>\n");
            }

            return sbContent.ToString();
        }

        #endregion

	}
}
