using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

using CancerGov.CDR.DataManager;
using CancerGov.Common.ErrorHandling;
using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.UI.CDR;
using CancerGov.CDR.ClinicalTrials.Helpers;

using NCI.Util;
using NCI.Logging;
using NCI.Web.CDE.UI;

namespace CancerGov.Handlers
{
    public class ClinicalTrialsLinkHandler : IHttpHandler
    {
        HttpContext currentContext = null;

        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int termID;
            ClinicalTrialSearchLinkIdType termType;
            int diagnosisID;
            int trialType;
            int trialPhase;
            bool trialsAtNIHClinicalCenterOnly;
            bool closedTrialsOnly;
            bool newTrialsOnly;
            string country;
            ProtocolVersions audience = ProtocolVersions.None;
            CTSearchLinkPreferredNames preferredNames = null;
            int protocolSearchID = -1;

            currentContext = context;
            try
            {
                GetSearchCriteria(out termID, out termType, out diagnosisID, out trialType,
                    out trialPhase, out trialsAtNIHClinicalCenterOnly, out closedTrialsOnly,
                    out newTrialsOnly, out country, out audience);
                preferredNames = CTSearchManager.LoadPreferredNames(termID, termType, diagnosisID);
                string trialTypeText = LookupTrialTypeName(trialType);

                CTSearchDefinition searchDef = BuildSearchDefinition(preferredNames, trialTypeText, trialPhase, country,
                    trialsAtNIHClinicalCenterOnly, closedTrialsOnly, newTrialsOnly);
                protocolSearchID = CTSearchManager.SaveAndExecuteSearch(searchDef);
            }
            catch (ProtocolSearchExecutionFailureException eSearchFailure)
            {
                //Log Error
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", NCIErrorLevel.Error, eSearchFailure);
                this.RaiseErrorPage();
            }
            catch (NullProtocolSearchIDException eSearchFailure)
            {
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", "ProtocolSearchID that was returned is invalid", NCIErrorLevel.Error, eSearchFailure);
                this.RaiseErrorPage();
            }
            catch (CancerGov.Exceptions.SqlTimeoutException eSearchFailure)
            {
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", NCIErrorLevel.Error, eSearchFailure);
                this.RaiseErrorPage();
            }
            catch (ProtocolSearchParameterException eSearchFailure)
            {
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", NCIErrorLevel.Error, eSearchFailure);
                this.RaiseErrorPage();
            }
            catch (Exception eo)
            {
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", NCIErrorLevel.Error, eo);
                this.RaiseErrorPage();
            }

            //This should always happen
            if (protocolSearchID > 0)
            {
                if (audience != ProtocolVersions.None)
                {
                    context.Response.Redirect(string.Format("{2}?protocolsearchid={0}&vers={1}",
                        protocolSearchID.ToString("d"), (int)audience, SearchResultsPrettyUrl), true);
                }
                else
                {
                    context.Response.Redirect(string.Format("{1}?protocolsearchid={0}",
                        protocolSearchID.ToString("d"), SearchResultsPrettyUrl), true);
                }
            }
        }
        #endregion

        private void GetSearchCriteria(out int termID, out ClinicalTrialSearchLinkIdType termType,
            out int diagnosisID, out int trialType, out int trialPhase, out bool nihClinicalCenterOnly,
            out bool closedTrialsOnly, out bool newTrialsOnly, out string country, out ProtocolVersions audience)
        {
            // Assign defaults.
            #region Default values;
            termID = -1;
            termType = ClinicalTrialSearchLinkIdType.None;
            diagnosisID = -1;
            trialType = -1;
            trialPhase = -1;
            nihClinicalCenterOnly = false;
            closedTrialsOnly = false;
            newTrialsOnly = false;
            country = String.Empty;
            audience = ProtocolVersions.None;
            #endregion

            // If ID is null, IDType must be null.  If ID is not null, IDType must not be null
            if ((currentContext.Request["id"] != null || currentContext.Request["idtype"] != null) &&  // One of them is not null
                (currentContext.Request["id"] == null || currentContext.Request["idtype"] == null))   // One of them is null
            {
                throw new ProtocolSearchParameterException("Please specify both id and idtype parameters.");
            }
            else
            {
                termID = Strings.ToInt(currentContext.Request["id"], -1);
                termType = (ClinicalTrialSearchLinkIdType)Strings.ToInt(currentContext.Request["idtype"], 0);
                if (!Enum.IsDefined(typeof(ClinicalTrialSearchLinkIdType), termType))
                    throw new ProtocolSearchParameterException("The idtype is invalid");
            }

            // The format argument has been repurposed to contain audience.
            // format == 1 >> Patient
            // format == 2 >> Health Proffesional
            if (!string.IsNullOrEmpty(currentContext.Request["format"]))
            {
                try
                {
                    switch (Strings.ToInt(currentContext.Request["format"], true))
                    {
                        case 1:
                            audience = ProtocolVersions.Patient;
                            break;

                        case 2:
                            audience = ProtocolVersions.HealthProfessional;
                            break;

                        default:
                            throw new Exception();
                    }
                }
                catch (Exception)
                {
                    NCI.Logging.Logger.LogError("ClinicalTrialsLink", "Invalid parameters provided.  Search type is not valid", NCIErrorLevel.Error);
                    throw new ProtocolSearchParameterException("Specified search format is not valid. Specify 1 for the patient and 2 for the health professional version.");
                    throw;
                }
            }

            // Generate an error if anything is non-parsable.
            try
            {
                int temp;

                if (currentContext.Request["diagnosis"] != null)
                    diagnosisID = Strings.ToInt(currentContext.Request["diagnosis"], true);
                if (currentContext.Request["tt"] != null)
                    trialType = Strings.ToInt(currentContext.Request["tt"], true);
                if (currentContext.Request["phase"] != null)
                    trialPhase = Strings.ToInt(currentContext.Request["phase"], true);
                if (currentContext.Request["ncc"] != null)
                {
                    temp = Strings.ToInt(currentContext.Request["ncc"], true);
                    nihClinicalCenterOnly = (temp == 0) ? false : true;
                }
                if (currentContext.Request["closed"] != null)
                {
                    temp = Strings.ToInt(currentContext.Request["closed"], true);
                    closedTrialsOnly = (temp == 0) ? false : true;
                }
                if (currentContext.Request["new"] != null)
                {
                    temp = Strings.ToInt(currentContext.Request["new"], true);
                    newTrialsOnly = (temp == 0) ? false : true;
                }
            }
            catch (Exception ex)
            {
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", "Invalid parameters provided. Conversion failed.", NCIErrorLevel.Error, ex);
                throw new ProtocolSearchParameterException("Please check your URL query paramters for the clinical trials search.");
            }


            // If present, Country must be an integer, so we try parsing it.  But then we override
            // with the value from AppSettings.
            if (currentContext.Request["cn"] != null)
            {
                try
                {
                    int.Parse(Strings.Clean(currentContext.Request["cn"]));
                }
                catch (Exception ex)
                {
                    NCI.Logging.Logger.LogError("ClinicalTrialsLink", "Invalid parameters provided. Conversion failed.", NCIErrorLevel.Error, ex);
                    throw new ProtocolSearchParameterException("Please make sure the country in your URL query paramters for the clinical trials search should be integer.");
                }
                try
                {
                    country = ConfigurationSettings.AppSettings["ClinicalTrialsSearchLinkCountry"].ToString();
                }
                catch (Exception ex)
                {
                    NCI.Logging.Logger.LogError("ClinicalTrialsLink", "ClinicalTrialsSearchLinkCountry AppSetting missing. Conversion failed. ", NCIErrorLevel.Error, ex);
                    throw new ProtocolSearchParameterException("Please make sure the ClinicalTrialsSearchLinkCountry AppSetting is set in web.config.");
                }
            }
        }

        private CTSearchDefinition BuildSearchDefinition(CTSearchLinkPreferredNames preferredNames, string trialType,
            int trialPhaseIndex, string country, bool trialsAtNIHOnly, bool closedTrialsOnly, bool newTrialsOnly)
        {
            string[] trialPhaseNames = { "Phase I", "Phase II", "Phase III", "Phase IV" };

            CTSearchDefinition definition = new CTSearchDefinition();

            // For logging information.
            definition.SearchInvocationType = SearchInvocationType.FromSearchLink;

            if (!string.IsNullOrEmpty(trialType))
                definition.TrialTypeList.Add(trialType);

            // Use trialPhaseIndex to look up the string used by the middle tier.
            if (trialPhaseIndex > 0 && trialPhaseIndex < 5)
            {
                definition.TrialPhase.Add(trialPhaseNames[trialPhaseIndex - 1]);
            }

            // Apply preferred name data.
            AssignPreferredNames(definition, preferredNames);

            // This is a little less than obvious, but as of the January 2009 redesign, clinical trial search
            // only allows one location at a time.  Clinical Trial search links allow three different
            // search types to be specified:
            // 
            //     Institution  (idtype = 2 or ClinicalTrialSearchLinkIdType.Institution)
            //     Trials at NIH Only
            //     Trials with a country
            //     
            // We therefore have to keep these mutually exclusive so we don't inadvertently change the
            // search's location criteria.  (This makes sense since a specific institution is going to
            // imply a certain country.  Likewise, the clinical center implies US.)
            if (preferredNames.NameType == ClinicalTrialSearchLinkIdType.Institution)
            {
                // Institution name is added during the call to AssignPreferredNames (above).
                // All we need to do here is set the location type.
                definition.LocationSearchType = LocationSearchType.Institution;
            }
            else if (trialsAtNIHOnly)
            {
                definition.LocationSearchType = LocationSearchType.NIH;
                definition.LocationNihOnly = true;
            }
            else
            {
                definition.LocationSearchType = LocationSearchType.City;
                definition.LocationCountry = country;
            }

            // The system only contains open trials.
            definition.TrialStatusRestriction = TrialStatusType.OpenOnly;

            definition.RestrictToRecent = newTrialsOnly;

            return definition;
        }

        private string LookupTrialTypeName(int trialTypeID)
        {
            string result = null;

            if (trialTypeID > 0)
            {
                CTSearchFieldList<int> trialTypeList = CTSearchManager.LoadTrialTypeList();
                foreach (KeyValuePair<string, int> pair in trialTypeList)
                {
                    if (pair.Value == trialTypeID)
                    {
                        result = pair.Key;
                        break;
                    }
                }
            }

            return result;
        }

        private void AssignPreferredNames(CTSearchDefinition definition, CTSearchLinkPreferredNames preferredNames)
        {
            if (preferredNames != null)
            {
                string name;
                int id;

                // Cancer Type.
                name = preferredNames.CancerTypeName;
                id = preferredNames.CancerTypeID;
                if (id > 0)
                {
                    definition.CancerType = new KeyValuePair<string, int>(name, id);
                }

                // Cancer Subtype
                name = preferredNames.CancerSubtypeName;
                id = preferredNames.CancerSubtypeID;
                if (id > 0)
                {
                    definition.CancerSubtypeIDList.Add(id);
                    if (!string.IsNullOrEmpty(name))
                        definition.CancerSubtypeNameList.Add(name);
                }

                // Assign preferred name depending on its type.
                if (preferredNames.Identifier > 0)
                {
                    KeyValuePair<string, int> kvp = new KeyValuePair<string, int>(preferredNames.PreferredName, preferredNames.Identifier);
                    switch (preferredNames.NameType)
                    {
                        case ClinicalTrialSearchLinkIdType.Drug:
                            definition.DrugList.Add(kvp);
                            break;
                        case ClinicalTrialSearchLinkIdType.Institution:
                            definition.LocationInstitutions.Add(kvp);
                            break;
                        case ClinicalTrialSearchLinkIdType.LeadOrganization:
                            definition.LeadOrganizationList.Add(kvp);
                            break;
                        case ClinicalTrialSearchLinkIdType.Investigator:
                            definition.InvestigatorList.Add(kvp);
                            break;
                        case ClinicalTrialSearchLinkIdType.Intervention:
                            definition.InterventionList.Add(kvp);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Raise the error page
        /// </summary>
        private void RaiseErrorPage()
        {
            ErrorPageDisplayer.RaisePageError(this.GetType().ToString());
        }

        private string SearchResultsPrettyUrl
        {
            get { return ConfigurationSettings.AppSettings["ClinicalTrailsResultsPage"]; }
        }
    }
}
