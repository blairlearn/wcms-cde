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
                            //add this div for accordion to work on mobile
                            sbContent.Append("<div class=\"accordion\">");
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
                sbContent.Append(pProtocol.AlternateTitle);
                sbContent.Append("\n");


            } //else the titles are the same so don't show them

			return sbContent.ToString();
		}

		protected string RenderTOC () 
		{

			StringBuilder sbContent = new StringBuilder();

            //create an empty div to add the CTGovProtocol table of contents using Javascript
            sbContent.Append("<div id=\"pdq-toc-protocol\"></div>");

            //Keeping this commented out code for now until we decide for sure that we are using 
            //javascript to build table of contents

            //sbContent.Append("<div class=\"on-this-page\">");
            //sbContent.Append("<h6>ON THIS PAGE</h6>");
            //sbContent.Append("<ul>");
            			
            //if ((pProtocol.AlternateTitle != null) && (pProtocol.AlternateTitle != "") && (pProtocol.AlternateTitle != pProtocol.ProtocolTitle)) 
            //{
            //    sbContent.Append("<li>");
            //    sbContent.Append("<a href=\"#AlternateTitle_");
            //    sbContent.Append(pProtocol.FullCdrId);
            //    sbContent.Append("\">Alternate Title</a>");
            //    sbContent.Append("</li>");
            //}

            //sbContent.Append("<li>");
            //sbContent.Append("<a href=\"#StudyIdInfo_");
            //sbContent.Append(pProtocol.FullCdrId);
            //sbContent.Append("\">Basic Trial Information</a>");
            //sbContent.Append("</li>");

            //if (pProtocol.ProtocolVersion == ProtocolVersions.HealthProfessional) 
            //{
				 
            //    if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.Objectives)) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#Objectives_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Objectives</a>");
            //        sbContent.Append("</li>");
            //    }

            //    if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
            //    {

            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#TrialDescription_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Trial Description</a>");


            //        if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary)))
            //        {
            //            sbContent.Append("<ul>");
            //            sbContent.Append("<li>");
            //            sbContent.Append("<a href=\"#Objectives_");
            //            sbContent.Append(pProtocol.FullCdrId);
            //            sbContent.Append("\">Summary</a>");
            //            sbContent.Append("</li>");
            //            sbContent.Append("</ul>");
            //        }
					
            //        if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription))) 
            //        {
            //            sbContent.Append("<ul>");
            //            sbContent.Append("<li>");
            //            sbContent.Append("<a href=\"#Outline_");
            //            sbContent.Append(pProtocol.FullCdrId);
            //            sbContent.Append("\">Further Trial Information</a>");
            //            sbContent.Append("</li>");
            //            sbContent.Append("</ul>");
            //        }

            //        if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria))) 
            //        {
            //            sbContent.Append("<ul>");
            //            sbContent.Append("<li>");
            //            sbContent.Append("<a href=\"#EntryCriteria_");
            //            sbContent.Append(pProtocol.FullCdrId);
            //            sbContent.Append("\">Eligibility Criteria</a>");
            //            sbContent.Append("</li>");
            //            sbContent.Append("</ul>");
            //        }

            //        sbContent.Append("</li>");


            //    }

            //    if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.EntryCriteria)) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#EntryCriteria_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Entry Criteria</a>");
            //        sbContent.Append("</li>");
            //    }

            //    if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.ExpectedEnrollment)) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#ExpectedEnrollment_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Expected Enrollment</a>");
            //        sbContent.Append("</li>");
            //    }

            //    if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.Outcomes)) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#Outcomes_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Outcomes</a>");
            //        sbContent.Append("</li>");
            //    }

            //    if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.Outline)) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#Outline_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Outline</a>");
            //        sbContent.Append("</li>");
            //    }

            //    if ((pProtocol.Sections != null) && (pProtocol.Sections.Contains((int)ProtocolSectionTypes.PublishedResults))) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#PublishedResults_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Published Results</a>");
            //        sbContent.Append("</li>");
            //    }

            //    if ((pProtocol.Sections != null) && (pProtocol.Sections.Contains((int)ProtocolSectionTypes.RelatedPublications))) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#RelatedPublications_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Related Publications</a>");
            //        sbContent.Append("</li>");
            //    }

            //} 
            //else 
            //{


            //    if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.PatientAbstract)) 
            //    {
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#TrialDescription_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Trial Description</a>");


            //        sbContent.Append("<ul>");
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#Purpose_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Purpose</a>");
            //        sbContent.Append("</li>");
            //        sbContent.Append("</ul>");

            //        sbContent.Append("<ul>");
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#Eligibility_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Eligibility</a>");
            //        sbContent.Append("</li>");
            //        sbContent.Append("</ul>");

            //        sbContent.Append("<ul>");
            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#TreatmentIntervention_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Treatment/Intervention</a>");
            //        sbContent.Append("</li>");
            //        sbContent.Append("</ul>");

            //        sbContent.Append("</li>");
            //    }

            //    if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
            //    {

            //        sbContent.Append("<li>");
            //        sbContent.Append("<a href=\"#TrialDescription_");
            //        sbContent.Append(pProtocol.FullCdrId);
            //        sbContent.Append("\">Trial Description</a>");
                    
            //        if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovBriefSummary))) 
            //        {
            //            sbContent.Append("<ul>");
            //            sbContent.Append("<li>");
            //            sbContent.Append("<a href=\"#Objectives_");
            //            sbContent.Append(pProtocol.FullCdrId);
            //            sbContent.Append("\">Summary</a>");
            //            sbContent.Append("</li>");
            //            sbContent.Append("</ul>");
            //        }
					
            //        if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovDetailedDescription))) 
            //        {
            //            sbContent.Append("<ul>");
            //            sbContent.Append("<li>");
            //            sbContent.Append("<a href=\"#Outline_");
            //            sbContent.Append(pProtocol.FullCdrId);
            //            sbContent.Append("\">Further Trial Information</a>");
            //            sbContent.Append("</li>");
            //            sbContent.Append("</ul>");
            //        }

            //        if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.CTGovEntryCriteria))) 
            //        {
            //            sbContent.Append("<ul>");
            //            sbContent.Append("<li>");
            //            sbContent.Append("<a href=\"#EntryCriteria_");
            //            sbContent.Append(pProtocol.FullCdrId);
            //            sbContent.Append("\">Eligibility Criteria</a>");
            //            sbContent.Append("</li>");
            //            sbContent.Append("</ul>");
            //        }

            //        sbContent.Append("</li>");


            //    }

            //}

            ////New Requirements, Draw Trial Contact Information to link above lead orgs and study sites
            //sbContent.Append("<li>");
            //sbContent.Append("<a href=\"#ContactInfo_");
            //sbContent.Append(pProtocol.FullCdrId);
            //sbContent.Append("\">Trial Contact Information</a>");
            //sbContent.Append("</li>");
			
            ////SCR 850
            //if ((pProtocol.Sections.Contains((int)ProtocolSectionTypes.PatientRelatedInformation)) || (pProtocol.Sections.Contains((int)ProtocolSectionTypes.HPRelatedInformation))) 
            //{
            //    sbContent.Append("<li>");
            //    sbContent.Append("<a href=\"#ProtocolRelatedLinks_");
            //    sbContent.Append(pProtocol.FullCdrId);
            //    sbContent.Append("\">Related Information</a>");
            //    sbContent.Append("</li>");
            //}

            ////registry info 
            //if (pProtocol.Sections.Contains((int)ProtocolSectionTypes.RegistryInformation)){
            //    sbContent.Append("<li>");
            //    sbContent.Append("<a href=\"#RegistryInfo_");
            //    sbContent.Append(pProtocol.FullCdrId);
            //    sbContent.Append("\">Registry Information</a>");
            //    sbContent.Append("</li>");
            //}

            //sbContent.Append("</ul>");
            //sbContent.Append("</div>");

			//sbContent.Append("<p>");

			return sbContent.ToString();
		}

		#region Study Sites
		protected string RenderStudySites() {
			StringBuilder sbContent = new StringBuilder();
			StringBuilder sbUSA = new StringBuilder();
			StringBuilder sbWorld = new StringBuilder();

			DataView dvStudySites = pProtocol.Sites.SiteTable;

			if (dvStudySites.Count > 0) {

                sbContent.Append("<h3 id=\"SitesAndContacts_");
                sbContent.Append(pProtocol.FullCdrId);
                sbContent.Append("\" do-not-show=\"toc\">Trial Sites</h3>\n");

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
                sbContent.Append("<table class=\"table-default\">");
                sbContent.Append("<tr>");
                sbContent.Append("<th>Phase</th>");
                sbContent.Append("<th>Type</th>");
                sbContent.Append("<th>Status</th>");
                sbContent.Append("<th>Age</th>");
                sbContent.Append("<th>Sponsor</th>");
                sbContent.Append("<th>Protocol IDs</th>");
                sbContent.Append("</tr>");
                sbContent.AppendFormat("<tr>");
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.Phase);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.TrialType);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.CurrentStatus);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.AgeRange);
                sbContent.AppendFormat("<td>{0}</td>", pProtocol.TrialSponsor.Replace("/", " / "));
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
