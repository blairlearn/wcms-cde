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
							sbContent.Append("<p>\n");
							sbContent.Append("<a name=\"StudyIdInfo_");
							sbContent.Append(pProtocol.FullCdrId);
							sbContent.Append("\"></a>");
							sbContent.Append("<span class=\"Protocol-Section-Heading\">Basic Trial Information</span>\n");
							sbContent.Append("<p>\n");

							sbContent.Append(this.RenderInfoBox());
							break;
						
						case ProtocolSectionTypes.StudySites : 
							if ((pProtocol.Sites != null) && (pProtocol.Sites.SiteTable != null)) {
								if (!bDrawnTCI) {
									sbContent.Append("<p>\n");
									sbContent.Append("<a name=\"ContactInfo_");
									sbContent.Append(pProtocol.FullCdrId);
									sbContent.Append("\"></a>");
									sbContent.Append("<span class=\"Protocol-Section-Heading\">Trial Contact Information</span>\n");
									sbContent.Append("<p>\n");

									bDrawnTCI = true;
								}
								sbContent.Append(this.RenderStudySites());
							}
							break;

						case ProtocolSectionTypes.CTGovLeadOrgs : 
						case ProtocolSectionTypes.LeadOrgs : 
							if (!bDrawnTCI) {
								sbContent.Append("<p>\n");
								sbContent.Append("<a name=\"ContactInfo_");
								sbContent.Append(pProtocol.FullCdrId);
								sbContent.Append("\"></a>");
								sbContent.Append("<span class=\"Protocol-Section-Heading\">Trial Contact Information</span>\n");
								sbContent.Append("<p>\n");

								bDrawnTCI = true;
							}
							sbContent.Append(pProtocol.GetSectionByID((ProtocolSectionTypes)iSection));

							break;
							
						case ProtocolSectionTypes.CTGovBriefSummary :
						case ProtocolSectionTypes.CTGovDetailedDescription :
						case ProtocolSectionTypes.CTGovEntryCriteria :

							if (pProtocol.Sections.Contains(iSection)) {

								if (!bWroteTrialDesc) {
									sbContent.Append("<p>\n");
									sbContent.Append("<a name=\"TrialDescription_");
									sbContent.Append(pProtocol.FullCdrId);
									sbContent.Append("\"></a>");
									sbContent.Append("<span class=\"Protocol-Section-Heading\">Trial Description</span>\n");
									sbContent.Append("<p>\n");	
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
			
			return sbContent.ToString();
		}


		protected virtual string RenderTitle() {

			StringBuilder sbContent = new StringBuilder();

			//Draw the title, Everything has to have some title
		
			sbContent.Append("<span class=\"Protocol-Title\">");
			sbContent.Append(pProtocol.ProtocolTitle);
			sbContent.Append("</span>\n");

			// for some reason this is drawn in a separate place when it's not a printable version
			// also, don't show the dates for CTGov Protocols, since they are mostly wrong
            if ((this.displayInfo.Version == DisplayVersions.Print) && (this.pProtocol.ProtocolType == ProtocolTypes.Protocol))
			{
				sbContent.Append("<BR>");
				
				if (pProtocol.DateLastModified != new DateTime(0)) 
				{
					sbContent.Append("<span class=\"protocol-date-label\">Last Modified: </span>");
					sbContent.Append("<span class=\"protocol-dates\">");
					sbContent.Append(pProtocol.DateLastModified.ToString("d"));
					sbContent.Append("</span>");
			
					if (pProtocol.DateFirstPublished != new DateTime(0)) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp");
					} 
					else 
					{
						sbContent.Append("&nbsp;&nbsp;");
					}
				}

				if (pProtocol.DateFirstPublished != new DateTime(0)) 
				{
					sbContent.Append("<span class=\"protocol-date-label\">First Published: </span>");
					sbContent.Append("<span class=\"protocol-dates\">");
					sbContent.Append(pProtocol.DateFirstPublished.ToString("d"));
					sbContent.Append("</span>");
					sbContent.Append("&nbsp;&nbsp;");
				}
			}

			sbContent.Append("<p>\n"); //<---- note we draw <p> tags after a section has been drawn

			return sbContent.ToString();
		}


		protected string RenderAlternateTitle(){

			StringBuilder sbContent = new StringBuilder();

			//Now for the alternate title... We only show  if we have one.
			if ((pProtocol.AlternateTitle != null) && (pProtocol.AlternateTitle != "") && (pProtocol.AlternateTitle != pProtocol.ProtocolTitle)) {

				sbContent.Append("<a name=\"AlternateTitle_");
				sbContent.Append(pProtocol.FullCdrId);
				sbContent.Append("\"></a><span class=\"Protocol-Section-Heading\">Alternate Title</span>\n");
				sbContent.Append("<p>\n");
				sbContent.Append(pProtocol.AlternateTitle);
				sbContent.Append("<p>\n");


			} //else the titles are the same so don't show them

			return sbContent.ToString();
		}

		protected string RenderTOC () 
		{

			StringBuilder sbContent = new StringBuilder();

			//sbContent.Append("<p><h1>Table of Contents</h1>");
			if ((pProtocol.AlternateTitle != null) && (pProtocol.AlternateTitle != "") && (pProtocol.AlternateTitle != pProtocol.ProtocolTitle)) 
			{
				sbContent.Append("<a href=\"#AlternateTitle_");
				sbContent.Append(pProtocol.FullCdrId);
				sbContent.Append("\" class=\"protocol-toc-link\">Alternate Title</a><br />");
			}

			sbContent.Append("<a href=\"#StudyIdInfo_");
			sbContent.Append(pProtocol.FullCdrId);
			sbContent.Append("\" class=\"protocol-toc-link\">Basic Trial Information</a><br />");

			if (pProtocol.ProtocolVersion == ProtocolVersions.HealthProfessional) 
			{
				 
				if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.Objectives)) 
				{
					sbContent.Append("<a href=\"#Objectives_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Objectives</a><br />");
				}

				if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
				{


					sbContent.Append("<a href=\"#TrialDescription_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Trial Description</a><br />");

					if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						sbContent.Append("<a href=\"#Objectives_");
						sbContent.Append(pProtocol.FullCdrId);
						sbContent.Append("\" class=\"protocol-toc-link\">Summary</a><br />");	
					}
					
					if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription))) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						sbContent.Append("<a href=\"#Outline_");
						sbContent.Append(pProtocol.FullCdrId);
						sbContent.Append("\" class=\"protocol-toc-link\">Further Trial Information</a><br />");	
					}

					if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria))) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						sbContent.Append("<a href=\"#EntryCriteria_");
						sbContent.Append(pProtocol.FullCdrId);
						sbContent.Append("\" class=\"protocol-toc-link\">Eligibility Criteria</a><br />");	
					}


				}

				if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.EntryCriteria)) 
				{
					sbContent.Append("<a href=\"#EntryCriteria_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Entry Criteria</a><br />");
				}

				if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.ExpectedEnrollment)) 
				{
					sbContent.Append("<a href=\"#ExpectedEnrollment_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Expected Enrollment</a><br />");
				}

				if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.Outcomes)) 
				{
					sbContent.Append("<a href=\"#Outcomes_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Outcomes</a><br />");
				}

				if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.Outline)) 
				{
					sbContent.Append("<a href=\"#Outline_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Outline</a><br />");
				}

				if ((pProtocol.Sections != null) && (pProtocol.Sections.Contains((int)ProtocolSectionTypes.PublishedResults))) 
				{
					sbContent.Append("<a href=\"#PublishedResults_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Published Results</a><br />");
				}

				if ((pProtocol.Sections != null) && (pProtocol.Sections.Contains((int)ProtocolSectionTypes.RelatedPublications))) 
				{
					sbContent.Append("<a href=\"#RelatedPublications_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Related Publications</a><br />");
				}

			} 
			else 
			{


				if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.PatientAbstract)) 
				{
					sbContent.Append("<a href=\"#TrialDescription_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Trial Description</a><br />");


					//Sub links, ah, tab in 5 places?
					sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
					sbContent.Append("<a href=\"#Purpose_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Purpose</a><br />");

					sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
					sbContent.Append("<a href=\"#Eligibility_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Eligibility</a><br />");

					sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
					sbContent.Append("<a href=\"#TreatmentIntervention_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Treatment/Intervention</a><br />");
				}

				if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
				{


					sbContent.Append("<a href=\"#TrialDescription_");
					sbContent.Append(pProtocol.FullCdrId);
					sbContent.Append("\" class=\"protocol-toc-link\">Trial Description</a><br />");

					if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						sbContent.Append("<a href=\"#Objectives_");
						sbContent.Append(pProtocol.FullCdrId);
						sbContent.Append("\" class=\"protocol-toc-link\">Summary</a><br />");	
					}
					
					if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription))) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						sbContent.Append("<a href=\"#Outline_");
						sbContent.Append(pProtocol.FullCdrId);
						sbContent.Append("\" class=\"protocol-toc-link\">Further Trial Information</a><br />");	
					}

					if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria))) 
					{
						sbContent.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
						sbContent.Append("<a href=\"#EntryCriteria_");
						sbContent.Append(pProtocol.FullCdrId);
						sbContent.Append("\" class=\"protocol-toc-link\">Eligibility Criteria</a><br />");	
					}


				}

			}

			//New Requirements, Draw Trial Contact Information to link above lead orgs and study sites
			sbContent.Append("<a href=\"#ContactInfo_");
			sbContent.Append(pProtocol.FullCdrId);
			sbContent.Append("\" class=\"protocol-toc-link\">Trial Contact Information</a><br />");
			
			//SCR 850
			if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.PatientRelatedInformation)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.HPRelatedInformation))) 
			{
				sbContent.Append("<a href=\"#ProtocolRelatedLinks_");
				sbContent.Append(pProtocol.FullCdrId);
				sbContent.Append("\" class=\"protocol-toc-link\">Related Information</a><br />");
			}

			//registry info 
			if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.RegistryInformation)){ 
				sbContent.Append("<a href=\"#RegistryInfo_");
				sbContent.Append(pProtocol.FullCdrId);
				sbContent.Append("\" class=\"protocol-toc-link\">Registry Information</a><br />");
			}

			sbContent.Append("<p>");

			return sbContent.ToString();
		}

		#region Study Sites
		protected string RenderStudySites() {
			StringBuilder sbContent = new StringBuilder();
			StringBuilder sbUSA = new StringBuilder();
			StringBuilder sbWorld = new StringBuilder();

			DataView dvStudySites = pProtocol.Sites.SiteTable;

			if (dvStudySites.Count > 0) {

				sbContent.Append("<a name=\"SitesAndContacts_");
				sbContent.Append(pProtocol.FullCdrId);
				sbContent.Append("\"></a>");
				sbContent.Append("<span class=\"Protocol-Section-SubHeading\">Trial Sites</span>\n");
                sbContent.Append("<p>\n");

				sbContent.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\n");
				sbContent.Append("<tr>\n");
                sbContent.Append("<td valign=\"top\" width=\"7%\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" border=\"0\"></td>\n");
                sbContent.Append("<td valign=\"top\" width=\"7%\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" border=\"0\"></td>\n");
                sbContent.Append("<td valign=\"top\" width=\"33%\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" border=\"0\"></td>\n");
                sbContent.Append("<td valign=\"top\" width=\"53%\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"1\" alt=\"\" border=\"0\"></td>\n");
                sbContent.Append("</tr>\n");

	
				string strPrevCountry = "";
				string strPrevState = "";
				string strPrevCity = "";
				
				foreach (DataRowView drvSite in dvStudySites) {

					if (drvSite.Row["Country"].ToString() == "U.S.A.") {

						if (strPrevCountry == (string)drvSite.Row["Country"]) { //do not draw country
						
							if (strPrevState == (string)drvSite.Row["State"]) { //do not draw state

								if (strPrevCity == (string)drvSite.Row["City"]) { //do not draw city
					
									sbUSA.Append(drvSite.Row["HTML"].ToString());
					
								} else {

									sbUSA.Append("<tr>\n");
									sbUSA.Append("<td valign=\"top\">&nbsp;</td>\n");
									sbUSA.Append("<td valign=\"top\" colspan=\"3\" class=\"protocol-city\">");
									sbUSA.Append(drvSite.Row["City"].ToString());
									sbUSA.Append("</td>\n");
									sbUSA.Append("</tr>\n");
									sbUSA.Append("<tr>\n");
									sbUSA.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
									sbUSA.Append("</tr>\n");
									sbUSA.Append(drvSite.Row["HTML"].ToString());

									strPrevCity = drvSite.Row["City"].ToString();

								}

							} else {
								if (drvSite.Row["State"].ToString() != "") {
									sbUSA.Append("<tr>\n");
									sbUSA.Append("<td valign=\"top\" colspan=\"4\" class=\"protocol-state\">");
									sbUSA.Append(drvSite.Row["State"].ToString());
									sbUSA.Append("</td>\n");
									sbUSA.Append("</tr>\n");
									sbUSA.Append("<tr>\n");
									sbUSA.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
									sbUSA.Append("</tr>\n");
								}
								sbUSA.Append("<tr>\n");
								sbUSA.Append("<td valign=\"top\">&nbsp;</td>\n");
								sbUSA.Append("<td valign=\"top\" colspan=\"3\" class=\"protocol-city\">");
								sbUSA.Append(drvSite.Row["City"].ToString());
								sbUSA.Append("</td>\n");
								sbUSA.Append("</tr>\n");
								sbUSA.Append("<tr>\n");
								sbUSA.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
								sbUSA.Append("</tr>\n");
								
								sbUSA.Append(drvSite.Row["HTML"].ToString());

								strPrevState = drvSite.Row["State"].ToString();
								strPrevCity = drvSite.Row["City"].ToString();

							}

						} else {
							sbUSA.Append("<tr>\n");
							sbUSA.Append("<td valign=\"top\" colspan=\"4\" class=\"protocol-country\">");
							sbUSA.Append(drvSite.Row["Country"].ToString());
							sbUSA.Append("</td>\n");
							sbUSA.Append("</tr>\n");

							sbUSA.Append("<tr>\n");
							sbUSA.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
							sbUSA.Append("</tr>\n");

							if (drvSite.Row["State"].ToString() != "") {
								sbUSA.Append("<tr>\n");
								sbUSA.Append("<td valign=\"top\" colspan=\"4\" class=\"protocol-state\">");
								sbUSA.Append(drvSite.Row["State"].ToString());
								sbUSA.Append("</td>\n");
								sbUSA.Append("</tr>\n");
								sbUSA.Append("<tr>\n");
								sbUSA.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
								sbUSA.Append("</tr>\n");
							}

							sbUSA.Append("<tr>\n");
							sbUSA.Append("<td valign=\"top\">&nbsp;</td>\n");
							sbUSA.Append("<td valign=\"top\" colspan=\"3\" class=\"protocol-city\">");
							sbUSA.Append(drvSite.Row["City"].ToString());
							sbUSA.Append("</td>\n");
							sbUSA.Append("</tr>\n");
							sbUSA.Append("<tr>\n");
							sbUSA.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
							sbUSA.Append("</tr>\n");
							
							sbUSA.Append(drvSite.Row["HTML"].ToString());
						
							strPrevCountry = drvSite.Row["Country"].ToString();
							strPrevState = drvSite.Row["State"].ToString();
							strPrevCity = drvSite.Row["City"].ToString();

						}
					} else {
						if (strPrevCountry == (string)drvSite.Row["Country"]) { //do not draw country
						
							if (strPrevState == (string)drvSite.Row["State"]) { //do not draw state

								if (strPrevCity == (string)drvSite.Row["City"]) { //do not draw city
					
									sbWorld.Append(drvSite.Row["HTML"].ToString());
					
								} else {

									sbWorld.Append("<tr>\n");
									sbWorld.Append("<td valign=\"top\">&nbsp;</td>\n");
									sbWorld.Append("<td valign=\"top\" colspan=\"3\" class=\"protocol-city\">");
									sbWorld.Append(drvSite.Row["City"].ToString());
									sbWorld.Append("</td>\n");
									sbWorld.Append("</tr>\n");
									sbWorld.Append("<tr>\n");
									sbWorld.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
									sbWorld.Append("</tr>\n");
									sbWorld.Append(drvSite.Row["HTML"].ToString());

									strPrevCity = drvSite.Row["City"].ToString();

								}

							} else {
								if (drvSite.Row["State"].ToString() != "") {
									sbWorld.Append("<tr>\n");
									sbWorld.Append("<td valign=\"top\" colspan=\"4\" class=\"protocol-state\">");
									sbWorld.Append(drvSite.Row["State"].ToString());
									sbWorld.Append("</td>\n");
									sbWorld.Append("</tr>\n");
									sbWorld.Append("<tr>\n");
									sbWorld.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
									sbWorld.Append("</tr>\n");
								}
								sbWorld.Append("<tr>\n");
								sbWorld.Append("<td valign=\"top\">&nbsp;</td>\n");
								sbWorld.Append("<td valign=\"top\" colspan=\"3\" class=\"protocol-city\">");
								sbWorld.Append(drvSite.Row["City"].ToString());
								sbWorld.Append("</td>\n");
								sbWorld.Append("</tr>\n");
								sbWorld.Append("<tr>\n");
								sbWorld.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
								sbWorld.Append("</tr>\n");
								sbWorld.Append(drvSite.Row["HTML"].ToString());

								strPrevState = drvSite.Row["State"].ToString();
								strPrevCity = drvSite.Row["City"].ToString();

							}

						} else {
							sbWorld.Append("<tr>\n");
							sbWorld.Append("<td valign=\"top\" colspan=\"4\" class=\"protocol-country\">");
							sbWorld.Append(drvSite.Row["Country"].ToString());
							sbWorld.Append("</td>\n");
							sbWorld.Append("</tr>\n");

							sbWorld.Append("<tr>\n");
							sbWorld.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
							sbWorld.Append("</tr>\n");

							if (drvSite.Row["State"].ToString() != "") {
								sbWorld.Append("<tr>\n");
								sbWorld.Append("<td valign=\"top\" colspan=\"4\" class=\"protocol-state\">");
								sbWorld.Append(drvSite.Row["State"].ToString());
								sbWorld.Append("</td>\n");
								sbWorld.Append("</tr>\n");
								sbWorld.Append("<tr>\n");
								sbWorld.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
								sbWorld.Append("</tr>\n");
							}

							sbWorld.Append("<tr>\n");
							sbWorld.Append("<td valign=\"top\">&nbsp;</td>\n");
							sbWorld.Append("<td valign=\"top\" colspan=\"3\" class=\"protocol-city\">");
							sbWorld.Append(drvSite.Row["City"].ToString());
							sbWorld.Append("</td>\n");
							sbWorld.Append("</tr>\n");
							sbWorld.Append("<tr>\n");
							sbWorld.Append("<td valign=\"top\" colspan=\"4\"><img src=\"/images/spacer.gif\" width=\"1\" height=\"5\" alt=\"\" border=\"0\"></td>\n");
							sbWorld.Append("</tr>\n");
							sbWorld.Append(drvSite.Row["HTML"].ToString());
						
							strPrevCountry = drvSite.Row["Country"].ToString();
							strPrevState = drvSite.Row["State"].ToString();
							strPrevCity = drvSite.Row["City"].ToString();

						}

					}
				
				}
				sbContent.Append(sbUSA.ToString());
				sbContent.Append(sbWorld.ToString());
				sbContent.Append("</table>\n");

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
                sbContent.Append("<table width=\"100%\" cellpadding=\"5\" cellspacing=\"0\" border=\"0\" class=\"Protocol-info-box\">");
                sbContent.Append("<tr style=\"background: #e7e6e2\">");
                sbContent.Append("<th valign=\"top\" class=\"phaseCol label\">Phase</th>");
                sbContent.Append("<th valign=\"top\" class=\"typeCol label\">Type</th>");
                sbContent.Append("<th valign=\"top\" class=\"statusCol label\">Status</th>");
                sbContent.Append("<th valign=\"top\" class=\"ageCol label\">Age</th>");
                sbContent.Append("<th valign=\"top\" class=\"sponsorCol label\">Sponsor</th>");
                sbContent.Append("<th valign=\"top\" class=\"protocolIDCol label\">Protocol IDs</th>");
                sbContent.Append("</tr>");
                sbContent.AppendFormat("<tr>");
                sbContent.AppendFormat("<td valign=\"top\" class=\"phaseCol\">{0}</td>", pProtocol.Phase);
                sbContent.AppendFormat("<td valign=\"top\" class=\"typeCol\">{0}</td>", pProtocol.TrialType);
                sbContent.AppendFormat("<td valign=\"top\" class=\"statusCol\">{0}</td>", pProtocol.CurrentStatus);
                sbContent.AppendFormat("<td valign=\"top\" class=\"ageCol\">{0}</td>", pProtocol.AgeRange);
                sbContent.AppendFormat("<td valign=\"top\" class=\"sponsorCol\">{0}</td>", pProtocol.TrialSponsor.Replace("/", " / "));
                sbContent.AppendFormat("<td valign=\"top\" class=\"protocolIDCol\"><span class=\"protocol-primaryprotocolid\">{0}</span><br>{1}</td>", pProtocol.PrimaryProtocolID, pProtocol.AlternateProtocolIDs);
                sbContent.AppendFormat("</tr>");
                sbContent.Append("</table>");
            }
            else
            {
                string InfoBoxFormat = "<span class=\"label\">{0}: </span>{1}<br>";

                // build protocol list with a comma separation instead of the linebreak used in
                // the tabular layout.
                string protocolList;
                if( string.IsNullOrEmpty(pProtocol.AlternateProtocolIDs))
                    protocolList = string.Format("<span class=\"protocol-primaryprotocolid\">{0}</span>",
                        pProtocol.PrimaryProtocolID);
                else
                    protocolList = string.Format("<span class=\"protocol-primaryprotocolid\">{0}</span>, {1}",
                        pProtocol.PrimaryProtocolID, pProtocol.AlternateProtocolIDs);

                sbContent.Append("<div class=\"Protocol-info-box-list\">\n");
                sbContent.AppendFormat(InfoBoxFormat, "Phase", pProtocol.Phase);
                sbContent.AppendFormat(InfoBoxFormat, "Type", pProtocol.TrialType);
                sbContent.AppendFormat(InfoBoxFormat, "Status", pProtocol.CurrentStatus);
                sbContent.AppendFormat(InfoBoxFormat, "Age", pProtocol.AgeRange);
                sbContent.AppendFormat(InfoBoxFormat, "Sponsor", pProtocol.TrialSponsor.Replace("/", " / "));
                sbContent.AppendFormat(InfoBoxFormat, "Protocol IDs", protocolList);
                sbContent.Append("</div>\n");
            }

            return sbContent.ToString();
        }

        #endregion

	}
}
