using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CancerGov.CDR.DataManager;
using CancerGov.Common.ErrorHandling;
using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.UI.CDR;
using CancerGov.CDR.ClinicalTrials.Helpers;

using NCI.Web.UI.WebControls;
using NCI.Util;
using NCI.Web.UI.WebControls.JSLibraries;   // In order to reference Prototype.
using NCI.Web.UI.WebControls.FormControls;  // For the CTSearchCriteriaDisplay object.
using NCI.Web.CancerGov.Apps;
using NCI.Logging;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsLink : ClinicalTrialsBaseUserControl
    {
        #region Page Life Cycle Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

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
                NCI.Logging.Logger.LogError("ClinicalTrialsLink", "ProtocolSearchID that was returned is invalid", NCIErrorLevel.Error, eSearchFailure );
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
                    Response.Redirect(string.Format("{2}?protocolsearchid={0}&vers={1}",
                        protocolSearchID.ToString("d"), (int)audience, SearchPageInfo.SearchResultsPrettyUrl), true);
                }
                else
                {
                    Response.Redirect(string.Format("{1}?protocolsearchid={0}",
                        protocolSearchID.ToString("d"), SearchPageInfo.SearchResultsPrettyUrl), true);
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
            if ((Request["id"] != null || Request["idtype"] != null) &&  // One of them is not null
                (Request["id"] == null || Request["idtype"] == null))   // One of them is null
            {
                throw new ProtocolSearchParameterException("Please specify both id and idtype parameters.");
            }
            else
            {
                termID = Strings.ToInt(Request["id"], -1);
                termType = (ClinicalTrialSearchLinkIdType)Strings.ToInt(Request["idtype"], 0);
                if (!Enum.IsDefined(typeof(ClinicalTrialSearchLinkIdType), termType))
                    throw new ProtocolSearchParameterException("The idtype is invalid");
            }

            // The format argument has been repurposed to contain audience.
            // format == 1 >> Patient
            // format == 2 >> Health Proffesional
            if (!string.IsNullOrEmpty(Request["format"]))
            {
                try
                {
                    switch (Strings.ToInt(Request["format"], true))
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
                    NCI.Logging.Logger.LogError("ClinicalTrialsLink", "Invalid parameters provided.  Search type is not valid", NCIErrorLevel.Error );
                    throw new ProtocolSearchParameterException("Specified search format is not valid. Specify 1 for the patient and 2 for the health professional version.");
                    throw;
                }
            }

            // Generate an error if anything is non-parsable.
            try
            {
                int temp;

                if (Request["diagnosis"] != null)
                    diagnosisID = Strings.ToInt(Request["diagnosis"], true);
                if (Request["tt"] != null)
                    trialType = Strings.ToInt(Request["tt"], true);
                if (Request["phase"] != null)
                    trialPhase = Strings.ToInt(Request["phase"], true);
                if (Request["ncc"] != null)
                {
                    temp = Strings.ToInt(Request["ncc"], true);
                    nihClinicalCenterOnly = (temp == 0) ? false : true;
                }
                if (Request["closed"] != null)
                {
                    temp = Strings.ToInt(Request["closed"], true);
                    closedTrialsOnly = (temp == 0) ? false : true;
                }
                if (Request["new"] != null)
                {
                    temp = Strings.ToInt(Request["new"], true);
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
            if (Request["cn"] != null)
            {
                try
                {
                    int.Parse(Strings.Clean(Request["cn"]));
                }
                catch (Exception ex)
                {
                    NCI.Logging.Logger.LogError("ClinicalTrialsLink", "Invalid parameters provided. Conversion failed.", NCIErrorLevel.Error,  ex);
                    throw new ProtocolSearchParameterException("Please make sure the country in your URL query paramters for the clinical trials search should be integer.");
                }
                try
                {
                    country = System.Configuration.ConfigurationSettings.AppSettings["ClinicalTrialsSearchLinkCountry"].ToString();
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

            definition.TrialStatusRestriction =
                closedTrialsOnly ? TrialStatusType.ClosedOnly : TrialStatusType.OpenOnly;

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

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }
        #endregion
    }
}