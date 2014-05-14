using System;
using System.Collections.Generic;

using CancerGov.UI.CDR;
using CancerGov.CDR.DataManager;

namespace CancerGov.UI.CDR {
	/// <summary>
	/// This class contains static helper functions to map between the web site enumerations for display
	/// type and the section ids in the database
	/// </summary>
	public class ProtocolFunctions {

		/// <summary>
		/// This is used to translate a version enum and a display format enum into a section list string
		/// that the database can handle.
		/// </summary>
		/// <param name="version">Version of protocol</param>
		/// <param name="display">Display format</param>
		/// <returns>Comma separated string of sectionids</returns>
		public static string GetSectionList(ProtocolVersions version, ProtocolDisplayFormats display) {

			string strSectionList = "";

			switch (display) {
			
				case ProtocolDisplayFormats.Short : { //P & HP are the same
					strSectionList = 
						((int)ProtocolSectionTypes.Title).ToString() + "," + 
						((int)ProtocolSectionTypes.InfoBox).ToString(); 
					break;
				}

				case ProtocolDisplayFormats.Medium : {

					switch (version) {
						
						case ProtocolVersions.Patient : {
							strSectionList =  
								((int)ProtocolSectionTypes.Title).ToString() + "," + 
								((int)ProtocolSectionTypes.InfoBox).ToString() + "," +
								((int)ProtocolSectionTypes.SpecialCategory).ToString() + "," +
								((int)ProtocolSectionTypes.AlternateTitle).ToString() + "," + 
								//((int)ProtocolSectionTypes.LastMod).ToString() + "," + 
								((int)ProtocolSectionTypes.PatientAbstract).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovBriefSummary).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDetailedDescription).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovEntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.LeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovLeadOrgs).ToString();
								
								
							break;
						}

						case ProtocolVersions.HealthProfessional : {
							strSectionList = 
								((int)ProtocolSectionTypes.Title).ToString() + "," + 
								((int)ProtocolSectionTypes.InfoBox).ToString() + "," +
								((int)ProtocolSectionTypes.SpecialCategory).ToString() + "," +
								((int)ProtocolSectionTypes.AlternateTitle).ToString() + "," + 
								//((int)ProtocolSectionTypes.LastMod).ToString() + "," + 
								((int)ProtocolSectionTypes.Objectives).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovBriefSummary).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDetailedDescription).ToString() + "," +
								((int)ProtocolSectionTypes.EntryCriteria).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovEntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.LeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovLeadOrgs).ToString();					

							break;
						}
						
					}

					break;
				}

				case ProtocolDisplayFormats.Long : {

					switch (version) {
						
						case ProtocolVersions.Patient : {
					
							//todo add the new sections
							strSectionList =
								((int)ProtocolSectionTypes.Title).ToString() + "," + 
								((int)ProtocolSectionTypes.InfoBox).ToString() + "," +
								((int)ProtocolSectionTypes.SpecialCategory).ToString() + "," + 
								((int)ProtocolSectionTypes.AlternateTitle).ToString() + "," + 
								//((int)ProtocolSectionTypes.LastMod).ToString() + "," + 
								((int)ProtocolSectionTypes.PatientAbstract).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovBriefSummary).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDetailedDescription).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovEntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.PDisclaimer).ToString() + "," + 
								((int)ProtocolSectionTypes.LeadOrgs).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovLeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.StudySites).ToString() + "," +
								ProtocolSectionTypes.PatientRelatedInformation.ToString("d") + "," +
								((int)ProtocolSectionTypes.CTGovFooter).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDisclaimer).ToString();

							//bDrawStudySites = true;

							break;
						}

						case ProtocolVersions.HealthProfessional : {
							strSectionList = 
								((int)ProtocolSectionTypes.Title).ToString() + "," + 
								((int)ProtocolSectionTypes.InfoBox).ToString() + "," +
								((int)ProtocolSectionTypes.SpecialCategory).ToString() + "," +
								((int)ProtocolSectionTypes.AlternateTitle).ToString() + "," + 
								//((int)ProtocolSectionTypes.LastMod).ToString() + "," + 
								((int)ProtocolSectionTypes.Objectives).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovBriefSummary).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDetailedDescription).ToString() + "," +
								((int)ProtocolSectionTypes.EntryCriteria).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovEntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.ExpectedEnrollment).ToString() + "," +
								((int)ProtocolSectionTypes.Outcomes).ToString() + "," +
								((int)ProtocolSectionTypes.Outline).ToString() + "," + 
								((int)ProtocolSectionTypes.PublishedResults).ToString() + "," + 
								((int)ProtocolSectionTypes.RelatedPublications).ToString() + "," +
								((int)ProtocolSectionTypes.LeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovLeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.StudySites).ToString() + "," +
								ProtocolSectionTypes.HPRelatedInformation.ToString("d") + "," +
								((int)ProtocolSectionTypes.RegistryInformation).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovFooter).ToString() + "," +
								((int)ProtocolSectionTypes.HPDisclaimer).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDisclaimer).ToString();

							break;
						}
					}
					
					break;
				}					
					//todo add the new sections 
				case ProtocolDisplayFormats.SingleProtocol : {

					switch (version) {

						case ProtocolVersions.Patient : {

							strSectionList = 
								((int)ProtocolSectionTypes.Title).ToString() + "," + 
								((int)ProtocolSectionTypes.TableOfContents).ToString() + "," + 
								((int)ProtocolSectionTypes.AlternateTitle).ToString() + "," + 
								//((int)ProtocolSectionTypes.LastMod).ToString() + "," + 
								((int)ProtocolSectionTypes.InfoBox).ToString() + "," + 
								((int)ProtocolSectionTypes.SpecialCategory).ToString() + "," +
								((int)ProtocolSectionTypes.PatientAbstract).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovBriefSummary).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDetailedDescription).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovEntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.PDisclaimer).ToString() + "," +
								((int)ProtocolSectionTypes.LeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovLeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.StudySites).ToString() + "," +
								ProtocolSectionTypes.PatientRelatedInformation.ToString("d") + "," +
								((int)ProtocolSectionTypes.RegistryInformation).ToString() + "," +								
								((int)ProtocolSectionTypes.CTGovFooter).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDisclaimer).ToString();

							break;
						}

						case ProtocolVersions.HealthProfessional : {
							strSectionList = 
								((int)ProtocolSectionTypes.Title).ToString() + "," + 
								((int)ProtocolSectionTypes.TableOfContents).ToString() + "," + 
								((int)ProtocolSectionTypes.AlternateTitle).ToString() + "," + 
								//((int)ProtocolSectionTypes.LastMod).ToString() + "," + 
								((int)ProtocolSectionTypes.InfoBox).ToString() + "," + 
								((int)ProtocolSectionTypes.SpecialCategory).ToString() + "," +
								((int)ProtocolSectionTypes.Objectives).ToString() + "," + 
								((int)ProtocolSectionTypes.CTGovBriefSummary).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDetailedDescription).ToString() + "," +
								((int)ProtocolSectionTypes.EntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovEntryCriteria).ToString() + "," +
								((int)ProtocolSectionTypes.ExpectedEnrollment).ToString() + "," +
								((int)ProtocolSectionTypes.Outcomes).ToString() + "," +
								((int)ProtocolSectionTypes.Outline).ToString() + "," +
								((int)ProtocolSectionTypes.PublishedResults).ToString() + "," +
								((int)ProtocolSectionTypes.RelatedPublications).ToString() + "," +
								((int)ProtocolSectionTypes.LeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovLeadOrgs).ToString() + "," +
								((int)ProtocolSectionTypes.StudySites).ToString() + "," +
								ProtocolSectionTypes.HPRelatedInformation.ToString("d") + "," +
								((int)ProtocolSectionTypes.RegistryInformation).ToString() + "," +
								((int)ProtocolSectionTypes.HPDisclaimer).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovFooter).ToString() + "," +
								((int)ProtocolSectionTypes.CTGovDisclaimer).ToString() ; 
						


							break;
						}
					}

					break;
				}
			}
			

			return strSectionList;

		}

        /// <summary>
        /// Translates the combination of ProtocolVersions (audience) and ProtocolDisplayFormats (format)
        /// values into a list of sections.
        /// </summary>
        /// <param name="version">Either ProtocolVersions.Patient or ProtocolVersions.HealthProfessional</param>
        /// <param name="display">A ProtocolDisplayFormats value</param>
        /// <returns>Array of section ids</returns>
        public static ProtocolSectionTypes[] BuildSectionList(ProtocolVersions version, ProtocolDisplayFormats display,
            bool includeEligibility, bool includeLocations)
        {
            List<ProtocolSectionTypes> sectionList = new List<ProtocolSectionTypes>();

            switch (display)
            {
                case ProtocolDisplayFormats.Short:
                    { //Patient & Health Professional are the same
                        sectionList.Add(ProtocolSectionTypes.Title);
                        sectionList.Add(ProtocolSectionTypes.InfoBox);
                        break;
                    }

                case ProtocolDisplayFormats.Medium:
                    {
                        switch (version)
                        {

                            case ProtocolVersions.Patient:
                                {
                                    sectionList.Add(ProtocolSectionTypes.Title);
                                    sectionList.Add(ProtocolSectionTypes.AlternateTitle);
                                    sectionList.Add(ProtocolSectionTypes.InfoBox);
                                    sectionList.Add(ProtocolSectionTypes.SpecialCategory);
                                    sectionList.Add(ProtocolSectionTypes.PatientAbstract);
                                    sectionList.Add(ProtocolSectionTypes.CTGovBriefSummary);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                                    if(includeEligibility)
                                        sectionList.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.LeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                                    if (includeLocations)
                                        sectionList.Add(ProtocolSectionTypes.StudySites);
                                    break;
                                }

                            case ProtocolVersions.HealthProfessional:
                                {
                                    sectionList.Add(ProtocolSectionTypes.Title);
                                    sectionList.Add(ProtocolSectionTypes.AlternateTitle);
                                    sectionList.Add(ProtocolSectionTypes.InfoBox);
                                    sectionList.Add(ProtocolSectionTypes.SpecialCategory);
                                    sectionList.Add(ProtocolSectionTypes.Objectives);
                                    sectionList.Add(ProtocolSectionTypes.CTGovBriefSummary);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                                    if (includeEligibility)
                                    {
                                        sectionList.Add(ProtocolSectionTypes.EntryCriteria);
                                        sectionList.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                                    }
                                    sectionList.Add(ProtocolSectionTypes.LeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                                    if (includeLocations)
                                        sectionList.Add(ProtocolSectionTypes.StudySites);
                                    break;
                                }
                        }

                        break;
                    }

                case ProtocolDisplayFormats.Long:
                    {

                        switch (version)
                        {

                            case ProtocolVersions.Patient:
                                {
                                    sectionList.Add(ProtocolSectionTypes.Title);
                                    sectionList.Add(ProtocolSectionTypes.AlternateTitle);
                                    sectionList.Add(ProtocolSectionTypes.InfoBox);
                                    sectionList.Add(ProtocolSectionTypes.SpecialCategory);
                                    sectionList.Add(ProtocolSectionTypes.PatientAbstract);
                                    sectionList.Add(ProtocolSectionTypes.CTGovBriefSummary);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                                    sectionList.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.PDisclaimer);
                                    sectionList.Add(ProtocolSectionTypes.LeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.StudySites);
                                    sectionList.Add(ProtocolSectionTypes.PatientRelatedInformation);
                                    sectionList.Add(ProtocolSectionTypes.RegistryInformation);
                                    sectionList.Add(ProtocolSectionTypes.CTGovFooter);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDisclaimer);
                                    break;
                                }

                            case ProtocolVersions.HealthProfessional:
                                {
                                    sectionList.Add(ProtocolSectionTypes.Title);
                                    sectionList.Add(ProtocolSectionTypes.AlternateTitle);
                                    sectionList.Add(ProtocolSectionTypes.InfoBox);
                                    sectionList.Add(ProtocolSectionTypes.SpecialCategory);
                                    sectionList.Add(ProtocolSectionTypes.Objectives);
                                    sectionList.Add(ProtocolSectionTypes.CTGovBriefSummary);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                                    sectionList.Add(ProtocolSectionTypes.EntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.ExpectedEnrollment);
                                    sectionList.Add(ProtocolSectionTypes.Outcomes);
                                    sectionList.Add(ProtocolSectionTypes.Outline);
                                    sectionList.Add(ProtocolSectionTypes.PublishedResults);
                                    sectionList.Add(ProtocolSectionTypes.RelatedPublications);
                                    sectionList.Add(ProtocolSectionTypes.LeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.StudySites);
                                    sectionList.Add(ProtocolSectionTypes.HPRelatedInformation);
                                    sectionList.Add(ProtocolSectionTypes.RegistryInformation);
                                    sectionList.Add(ProtocolSectionTypes.CTGovFooter);
                                    sectionList.Add(ProtocolSectionTypes.HPDisclaimer);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDisclaimer);
                                    break;
                                }
                        }
                        break;
                    }

                case ProtocolDisplayFormats.SingleProtocol:
                    {
                        switch (version)
                        {
                            case ProtocolVersions.Patient:
                                {
                                    sectionList.Add(ProtocolSectionTypes.Title);
                                    sectionList.Add(ProtocolSectionTypes.TableOfContents);
                                    sectionList.Add(ProtocolSectionTypes.AlternateTitle);
                                    sectionList.Add(ProtocolSectionTypes.InfoBox);
                                    sectionList.Add(ProtocolSectionTypes.SpecialCategory);
                                    sectionList.Add(ProtocolSectionTypes.PatientAbstract);
                                    sectionList.Add(ProtocolSectionTypes.CTGovBriefSummary);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                                    sectionList.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.PDisclaimer);
                                    sectionList.Add(ProtocolSectionTypes.LeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.StudySites);
                                    sectionList.Add(ProtocolSectionTypes.PatientRelatedInformation);
                                    sectionList.Add(ProtocolSectionTypes.RegistryInformation);
                                    sectionList.Add(ProtocolSectionTypes.CTGovFooter);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDisclaimer);
                                    break;
                                }

                            case ProtocolVersions.HealthProfessional:
                                {
                                    sectionList.Add(ProtocolSectionTypes.Title);
                                    sectionList.Add(ProtocolSectionTypes.TableOfContents);
                                    sectionList.Add(ProtocolSectionTypes.AlternateTitle);
                                    sectionList.Add(ProtocolSectionTypes.InfoBox);
                                    sectionList.Add(ProtocolSectionTypes.SpecialCategory);
                                    sectionList.Add(ProtocolSectionTypes.Objectives);
                                    sectionList.Add(ProtocolSectionTypes.CTGovBriefSummary);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDetailedDescription);
                                    sectionList.Add(ProtocolSectionTypes.EntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.CTGovEntryCriteria);
                                    sectionList.Add(ProtocolSectionTypes.ExpectedEnrollment);
                                    sectionList.Add(ProtocolSectionTypes.Outcomes);
                                    sectionList.Add(ProtocolSectionTypes.Outline);
                                    sectionList.Add(ProtocolSectionTypes.PublishedResults);
                                    sectionList.Add(ProtocolSectionTypes.RelatedPublications);
                                    sectionList.Add(ProtocolSectionTypes.LeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.CTGovLeadOrgs);
                                    sectionList.Add(ProtocolSectionTypes.StudySites);
                                    sectionList.Add(ProtocolSectionTypes.HPRelatedInformation);
                                    sectionList.Add(ProtocolSectionTypes.RegistryInformation);
                                    sectionList.Add(ProtocolSectionTypes.HPDisclaimer);
                                    sectionList.Add(ProtocolSectionTypes.CTGovFooter);
                                    sectionList.Add(ProtocolSectionTypes.CTGovDisclaimer);
                                    break;
                                }
                        }

                        break;
                    }
            }
            
            return sectionList.ToArray();
        }
	}
}

