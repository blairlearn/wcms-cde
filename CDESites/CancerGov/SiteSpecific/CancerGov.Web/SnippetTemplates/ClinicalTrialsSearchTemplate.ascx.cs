using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Collections;
using System.Configuration;

using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.CDR.ClinicalTrials.Helpers;
using CancerGov.Common.HashMaster;
using NCI.Util;
using NCI.Web.UI.WebControls;
using NCI.Web.UI.WebControls.FormControls;
using NCI.Logging;
using NCI.Web.CDE.Modules; 

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsSearchTemplate : SearchBaseUserControl
    {
        #region Constants

        // The only allowed values for marking a section as collapsed/expanded.
        protected const string COLLAPSED = "N";
        protected const string EXPANDED = "Y";

        protected const string LOCATION_ALL = "all";
        protected const string LOCATION_ZIP = "zip";
        protected const string LOCATION_CITY = "city";
        protected const string LOCATION_HOSPITAL = "hospital";
        protected const string LOCATION_NIH = "nih";

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            //Need to add ARIA tags to location controls - this has nothing to do with the state of the form,
            //so we are doing it on init.


            ListItem selector;

            // Associate each location selector value with the fieldset it controls
            // We deliberately do *NOT* associate the "all" selector with the all/blank input area.


            // Associate the "zip" selector with the zip location fields.
            selector = LocationTypeSelector.Items.FindByValue(LOCATION_ZIP);
            if (selector != null)
                selector.Attributes.Add("aria-controls", zipCodeLocationFieldset.ClientID);

            ////Tell the radio which fieldset it controls
            //zipCodeLocationButton.InputAttributes.Add("aria-controls", zipCodeLocationFieldset.ClientID);
            ////Tell the fieldset which radio labels it
            //zipCodeLocationFieldset.Attributes.Add("aria-labelledby", zipCodeLocationButton.ClientID);


            // Associate the "city/state/country" selector with the zip location fields.
            selector = LocationTypeSelector.Items.FindByValue(LOCATION_CITY);
            if (selector != null)
                selector.Attributes.Add("aria-controls", cityStateLocationFieldset.ClientID);

            ////Tell the radio which fieldset it controls
            //cityStateLocationButton.InputAttributes.Add("aria-controls", cityStateLocationFieldset.ClientID);
            ////Tell the fieldset which radio labels it
            //cityStateLocationFieldset.Attributes.Add("aria-labelledby", cityStateLocationButton.ClientID);


            // Associate the "city/state/country" selector with the zip location fields.
            selector = LocationTypeSelector.Items.FindByValue(LOCATION_HOSPITAL);
            if (selector != null)
                selector.Attributes.Add("aria-controls", hospitalLocationFieldset.ClientID);

            ////Tell the radio which fieldset it controls
            //hospitalLocationButton.InputAttributes.Add("aria-controls", hospitalLocationFieldset.ClientID);
            ////Tell the fieldset which radio labels it
            //hospitalLocationFieldset.Attributes.Add("aria-labelledby", hospitalLocationButton.ClientID);


            // Associate the "city/state/country" selector with the zip location fields.
            selector = LocationTypeSelector.Items.FindByValue(LOCATION_NIH);
            if (selector != null)
                selector.Attributes.Add("aria-controls", atNihLocationFieldset.ClientID);

            ////Tell the radio which fieldset it controls
            //atNihLocationButton.InputAttributes.Add("aria-controls", atNihLocationFieldset.ClientID);
            ////Tell the fieldset which radio labels it
            //atNihLocationFieldset.Attributes.Add("aria-labelledby", atNihLocationButton.ClientID);


            txtKeywords.Attributes.Add("placeholder", "Examples: PSA, HER-2, \"Paget disease\"");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Force "Start Over" link to come back to whatever URL this page was invoked as.
            clear.HRef = PageInstruction.GetUrl(NCI.Web.CDE.PageAssemblyInstructionUrls.PrettyUrl).ToString();

            if (!IsPostBack)
            {
                // On initial page load, check for a protocol search ID. If one is found, load the
                // saved criteria and initialize the page's controls.
                CTSearchDefinition savedSearch = null;

                string rawSearchID = Strings.Clean(Request.Params["protocolsearchid"]);
                int iProtocolSearchID = Strings.ToInt(rawSearchID);

                if (string.IsNullOrEmpty(rawSearchID))
                {
                    // No protocol search was specified, so collapse all display areas
                    treatmentTypeAreaExpanded.Value = COLLAPSED;
                    trialStatusExpanded.Value = COLLAPSED;
                    trialSponsorExpanded.Value = COLLAPSED;
                }
                else if (iProtocolSearchID > 0)
                {
                    try
                    {
                        savedSearch = CTSearchManager.LoadSavedCriteria(iProtocolSearchID);
                    }
                    catch (Exception ex)
                    {
                        NCI.Logging.Logger.LogError("", "CTSearchManager.LoadSavedCriteria", NCIErrorLevel.Error, ex);
                        this.RaiseErrorPage("InvalidSearchID");
                    }

                    // A protocol search was specified, so expand all display areas
                    treatmentTypeAreaExpanded.Value = EXPANDED;
                    trialStatusExpanded.Value = EXPANDED;
                    trialSponsorExpanded.Value = EXPANDED;
                }
                else
                {
                    // An invalid search ID was specified.  Redirect to error.
                }

                // Determine what cancer type to use.  Needed for the cancer stage list.
                int cancerTypeID = 0;
                if (savedSearch != null)
                    cancerTypeID = savedSearch.CancerType.Value;

                FillCancerTypeSelectBox(savedSearch);
                FillCancerStageSelectBox(cancerTypeID, savedSearch);

                FillZipLocationSelectBox(savedSearch);
                FillInstitutionSelectBox(savedSearch);
                FillCountryAndState(savedSearch);
                FillNihLocationSelectBox(savedSearch);

                FillDrugSelectionList(savedSearch);
                FillInterventionSelectionList(savedSearch);

                FillKeywordText(savedSearch);
                FillNewTrialBox(savedSearch);
                FillProtocolIDBox(savedSearch);

                FillTrialInvestigatorBox(savedSearch);
                FillLeadOrganzationBox(savedSearch);

                FillPhase(savedSearch);
                FillTrialType(savedSearch);
                

                SetLocationButtons(savedSearch);
            }

            string webAnalyticsParameters =
                String.Format(
                    "{{typeOfTrialControlID : '{0}',drugControlID : '{1}',treatnentInterventionControlID : '{2}',trialInvestigatorsControlID : '{3}',leadOrganizationCooperativeGroupControlID : '{4}'}}",
                    trialType.ClientID,
                    drug.ClientID,
                    intervention.ClientID,
                    investigator.ClientID,
                    leadOrg.ClientID
                );

            submit.OnClientClick = "doSubmit(" + webAnalyticsParameters + ");";

            JSManager.AddExternalScript(this.Page, "/JS/Search/CDESearchClinicalTrials.js");
        }

        private void SetLocationButtons(CTSearchDefinition savedSearch)
        {
            string searchLocation = LOCATION_ALL;
            if (savedSearch != null)
            {
                switch (savedSearch.LocationSearchType)
                {
                    case LocationSearchType.Institution:
                        searchLocation = LOCATION_HOSPITAL;
                        break;
                    case LocationSearchType.City:
                        searchLocation = LOCATION_CITY;
                        break;
                    case LocationSearchType.NIH:
                        searchLocation = LOCATION_NIH;
                        break;
                    case LocationSearchType.Zip:
                        searchLocation = LOCATION_ZIP;
                        break;
                    default:
                    // Do nothing -- already set to LOCATION_ALL.
                        break;
                }
            }

            LocationTypeSelector.SelectedValue = searchLocation;
        }

        private void FillNihLocationSelectBox(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null && savedSearch.LocationSearchType == LocationSearchType.NIH)
            {
                if (savedSearch.LocationNihOnly)
                    nihOnly.Checked = true;
                else
                    nihOnly.Checked = false;
            }
        }

        private void FillKeywordText(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null && !string.IsNullOrEmpty(savedSearch.Keywords))
            {
                txtKeywords.Text = savedSearch.Keywords;
            }
        }

        private void FillNewTrialBox(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null)
            {
                newOnly.Checked = savedSearch.RestrictToRecent;
            }
        }

        private void FillProtocolIDBox(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null)
            {
                foreach (string id in savedSearch.SpecificProtocolIDList)
                {
                    if (!(string.IsNullOrEmpty(protocolID.Text)))
                    {
                        protocolID.Text += ", ";
                    }
                    protocolID.Text += id;
                }
            }
        }

        private void FillCancerTypeSelectBox(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<int> cancerTypeList = CTSearchManager.LoadCancerTypeList();

            ddlCancerType.AppendDataBoundItems = true;
            ddlCancerType.Items.Add(new System.Web.UI.WebControls.ListItem("All", ""));

            ddlCancerType.DataSource = cancerTypeList;
            ddlCancerType.DataValueField = "Value";
            ddlCancerType.DataTextField = "Key";
            ddlCancerType.DataBind();
            if (savedSearch == null)
            {
                // Default selection to "All"
                ddlCancerType.SelectedIndex = 0;
            }
            else
            {
                // Because the list of cancer types includes synonyms, the cancer type values
                // cannot be trusted to be unique.  The cancer type text however is.
                ListItem item = ddlCancerType.Items.FindByText(savedSearch.CancerType.Key);
                if (item != null)
                    item.Selected = true;
                else
                {
                    // If no match was found for the caner type text, then fail through
                    // and attempt with the first cancer type matching the value.
                    item = ddlCancerType.Items.FindByValue(savedSearch.CancerType.Value.ToString());
                    if (item != null)
                        item.Selected = true;
                    else
                        ddlCancerType.SelectedIndex = 0;
                }
            }
        }

        private void FillPhase(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<int> trialPhaseList = CTSearchManager.LoadTrialPhaseList();
            trialPhase.Items.Clear();
            trialPhase.Items.Add(new ListItem("All", "0"));
            trialPhase.AppendDataBoundItems = true;
            trialPhase.DataSource = trialPhaseList;
            trialPhase.DataValueField = "Value";
            trialPhase.DataTextField = "Key";
            trialPhase.DefaultIndex = 0;
            trialPhase.DataBind();

            if (savedSearch == null)
            {
                // Default selection to "All"
                trialPhase.SelectedIndex = 0;
            }
            else
            {
                bool matchFound = false;
                foreach (string phase in savedSearch.TrialPhase)
                {
                    ListItem item = trialPhase.Items.FindByText(phase);
                    if (item != null)
                    {
                        item.Selected = true;
                        matchFound = true;
                    }
                }
                if (!matchFound)
                    trialPhase.SelectedIndex = 0;
            }
        }


        private void FillTrialType(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<int> trialTypeList = CTSearchManager.LoadTrialTypeList();
            trialType.DataSource = trialTypeList;
            trialType.DataValueField = "Value";
            trialType.DataTextField = "Key";
            trialType.DefaultIndex = 0;
            trialType.DataBind();

            if (savedSearch == null)
            {
                // Default selection to "All"
                trialType.SelectedIndex = 0;
            }
            else
            {
                if (savedSearch.TrialTypeList.Count > 0)
                {
                    foreach (string text in savedSearch.TrialTypeList)
                    {
                        ListItem item = trialType.Items.FindByText(text);
                        if (item != null)
                            item.Selected = true;
                    }
                }
                else
                    trialType.SelectedIndex = 0;
            }
        }


        private void FillCancerStageSelectBox(int cancerTypeID, CTSearchDefinition savedSearch)
        {

            if (cancerTypeID > 0)
            {
                CTSearchFieldList<int> cancerStageList = CTSearchManager.LoadCancerStageList(cancerTypeID);

                // We need to clear the list either way because of postbacks.
                cancerStage.Items.Clear();

                //Only populate stages if a Cancer Type is selected.
                if (ddlCancerType.SelectedIndex >= 1)
                {
                    cancerStage.AppendDataBoundItems = true;
                    cancerStage.Items.Add(new System.Web.UI.WebControls.ListItem("All", ""));
                    cancerStage.DefaultIndex = 0;
                    cancerStage.DataSource = cancerStageList;
                    cancerStage.DataValueField = "Value";
                    cancerStage.DataTextField = "Key";
                    cancerStage.DataBind();
                    cancerStage.SelectedIndex = 0;
                }

                if (savedSearch != null)
                {
                    // It is possible for a saved search (particularly one via a search link)
                    // to contain a cancer type, or subtype which is not available through
                    // the user interface.
                    bool subTypesPresent = (cancerStageList.Count > 0);

                    // If any cancer subtypes were selected, then don't select "All."
                    if (subTypesPresent && savedSearch.CancerSubtypeIDList.Count > 0)
                        cancerStage.Items[0].Selected = false;

                    bool matchFound = false;
                    foreach (int subType in savedSearch.CancerSubtypeIDList)
                    {
                        ListItem item = cancerStage.Items.FindByValue(subType.ToString("d"));
                        if (item != null)
                        {
                            item.Selected = true;
                            matchFound = true;
                        }
                    }

                    // If no matches were found, select "All."
                    if (!matchFound)
                        cancerStage.SelectedIndex = 0;
                }
            }
            else
            {
                cancerStage.Items.Clear();
            }

        }

        /// <summary>
        /// Initializes the "Near ZIP Code" location search.
        /// </summary>
        /// <param name="savedSearch">Saved search parameters</param>
        private void FillZipLocationSelectBox(CTSearchDefinition savedSearch)
        {
            // The ZIP Proximity list uses hardcoded values provided in the .ASPX.

            if (savedSearch != null &&
                savedSearch.LocationSearchType == LocationSearchType.Zip)
            {
                int proximity = savedSearch.LocationZipProximity;
                int zip = savedSearch.LocationZipCode;

                if (zip > 0 && proximity > 0)
                {

                    zipCode.Text = string.Format("{0:D5}", zip);

                    // ZIP Proximity
                    try
                    {
                        zipCodeProximity.SelectedValue = proximity.ToString();
                    }
                    catch (Exception)
                    {
                        // Couldn't find the selected value?  Leave the default in place.
                    }
                }
            }
        }

        private void FillCountryAndState(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<string> countryList;
            CTSearchFieldList<string> stateList;
            CTSearchManager.LoadStateAndCountryList(out countryList, out stateList);

            if (countryList != null && stateList != null)
            {
                state.AppendDataBoundItems = true;
                state.Items.Add(new System.Web.UI.WebControls.ListItem("All", ""));
                state.DefaultIndex = 0;
                state.DataSource = stateList;
                state.DataValueField = "Value";
                state.DataTextField = "Key";
                state.DataBind();

                // country (Sytem.Web.UI.HtmlControls.HtmlSelect) does not support AppendDataBoundItems so All and U.S.A are added after binding
                country.DataSource = countryList;
                country.DataValueField = "Value";
                country.DataTextField = "Key";
                country.DataBind();
                country.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", ""));
                country.Items.Insert(1, new System.Web.UI.WebControls.ListItem("U.S.A.", "U.S.A.|"));

                if (savedSearch == null || savedSearch.LocationSearchType != LocationSearchType.City)
                {
                    country.SelectedIndex = 1;  // Default country is "U.S.A."
                    state.SelectedIndex = 0;    // Default selection to "All"
                }
                else
                {
                    // Restore City.
                    city.Value = savedSearch.LocationCity;

                    // Restore country
                    if (!string.IsNullOrEmpty(savedSearch.LocationCountry))
                    {
                        ListItem item = country.Items.FindByValue(savedSearch.LocationCountry);
                        if (item != null)
                            item.Selected = true;
                        else // Country not found, default to "U.S.A."
                            country.SelectedIndex = 1;
                    }
                    else
                        country.SelectedIndex = 1;  // Default country is "U.S.A."

                    if (savedSearch.LocationStateIDList.Count > 0)
                    {
                        bool found = false;
                        foreach (string stateID in savedSearch.LocationStateIDList)
                        {
                            ListItem item = state.Items.FindByValue(stateID);
                            if (item != null)
                            {
                                item.Selected = true;
                                found = true;
                            }
                        }
                        if (!found) // none of the state codes matched
                            state.SelectedIndex = 0;
                    }
                    else // No states were specified.
                        state.SelectedIndex = 0;
                }
            }
        }





        /// <summary>
        /// Handler for Cancer Type Selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cancerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cancerTypeID = 0;
            if (ddlCancerType.SelectedIndex > 0)
                cancerTypeID = int.Parse(ddlCancerType.SelectedValue);
            FillCancerStageSelectBox(cancerTypeID, null);
        }

        /// <summary>
        /// Click handler for the Intervention list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InterventionListClearAll_ClickHandler(object sender, EventArgs e)
        {
            intervention.Items.Clear();
        }

        private void FillInterventionSelectionList(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null)
            {
                // CTSearchDefinition guarantees that InterventionList will never be null.
                FillDeletableSelectionList(savedSearch.InterventionList, intervention, interventionListExpanded);
            }
        }

        /// <summary>
        /// Helper method for populating and determining whether to display one of the collapsible lists.
        /// </summary>
        /// <param name="savedSearch"></param>
        /// <param name="list"></param>
        private void FillDeletableSelectionList(IList<KeyValuePair<string, int>> selections, DeleteList list, HiddenField expandedState)
        {
            // If there are values, draw the list in an expanded condition.
            if (selections.Count > 0)
            {
                foreach (KeyValuePair<string, int> item in selections)
                {
                    list.Items.Add(new ListItem(item.Key.Trim(), HashMaster.SaltedHashCompoundString(item.Key.Trim(), item.Value.ToString())));
                }
                expandedState.Value = EXPANDED;
            }
            else
            {
                expandedState.Value = COLLAPSED;
            }
        }

        /// <summary>
        /// Click handler for the Investigator list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InvestigatorListClearAll_ClickHandler(object sender, EventArgs e)
        {
            investigator.Items.Clear();
        }

        private void FillTrialInvestigatorBox(CTSearchDefinition savedSearch)
        {
            investigatorListExpanded.Value = COLLAPSED;
            if (savedSearch != null)
            {
                // CTSearchDefinition guarantees that InvestigatorList will never be null.
                FillDeletableSelectionList(savedSearch.InvestigatorList, investigator, investigatorListExpanded);
            }

            // If the list is still collapsed, hide it.
            if (investigatorListExpanded.Value == COLLAPSED)
                trialInvestigatorsRow.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        /// <summary>
        /// Click handler for the Lead Organization list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LeadOrgClearAll_ClickHandler(object sender, EventArgs e)
        {
            leadOrg.Items.Clear();
        }

        private void FillLeadOrganzationBox(CTSearchDefinition savedSearch)
        {
            leadOrgListExpanded.Value = COLLAPSED;
            if (savedSearch != null)
            {
                // CTSearchDefinition guarantees that LeadOrganizationList will never be null.
                FillDeletableSelectionList(savedSearch.LeadOrganizationList, leadOrg, leadOrgListExpanded);
            }

            // If the list is still collapsed, hide it.
            if (leadOrgListExpanded.Value == COLLAPSED)
                trialLeadOrganizationRow.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        /// <summary>
        /// Click handler for the Drug list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DrugListClearAll_ClickHandler(object sender, EventArgs e)
        {
            drug.Items.Clear();
        }

        private void FillDrugSelectionList(CTSearchDefinition savedSearch)
        {
            drugListExpanded.Value = COLLAPSED;
            if (savedSearch != null)
            {
                // CTSearchDefinition guarantees that DrugList will never be null.
                FillDeletableSelectionList(savedSearch.DrugList, drug, drugListExpanded);
                drugListAllOrAny.SelectedValue = savedSearch.RequireAllDrugsMatch ? "all" : "any";
            }

            // If the list is still collapsed, hide it.
            if (drugListExpanded.Value == COLLAPSED)
                drugListArea.Style.Add(HtmlTextWriterStyle.Display, "none");
        }

        /// <summary>
        /// Click handler for the Institution list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InstutionListClearAll_ClickHandler(object sender, EventArgs e)
        {
            institution.Items.Clear();
        }

        private void FillInstitutionSelectBox(CTSearchDefinition savedSearch)
        {
            bool showAsCollapsed = true;

            /// We only initialize the hospital list to an expanded state if
            ///     a) This is an existing search.
            ///     b) The location type is Hospital.
            ///     c) The list of institutions is non-empty.
            if (savedSearch != null &&
                savedSearch.LocationSearchType == LocationSearchType.Institution &&
                savedSearch.LocationInstitutions.Count > 0)
            {
                showAsCollapsed = false;
            }

            if (showAsCollapsed)
            {
                institutionListExpanded.Value = COLLAPSED;
                hospitalLocationFieldset.Style.Add(HtmlTextWriterStyle.Display, "none");
                institutionListSubBox.Style.Add(HtmlTextWriterStyle.Display, "none");
            }
            else
            {
                showInstitutionListButton.Style.Add(HtmlTextWriterStyle.Display, "none");
                institutionListExpanded.Value = EXPANDED;

                // Create a checksum for each name and append it to the values to assure
                // strings aren't altered.  This allows us to use the same codepath for building
                // criteria sets via popups and values which were already in the list.
                CTLookupListItem encodedItem;
                foreach (KeyValuePair<string, int> item in savedSearch.LocationInstitutions)
                {
                    encodedItem = new CTLookupListItem(item.Value.ToString("d"), item.Key, "");
                    institution.Items.Add(new ListItem(encodedItem.Name, encodedItem.HashedCDRID));
                }
            }
        }

        public void SubmitButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                int searchID = 0;

                // Gather all search criteria.
                CTSearchDefinition criteria = BuildSearchCriteria();

                // Save and Execute the search, yielding a protocol search ID.
                searchID = CTSearchManager.SaveAndExecuteSearch(criteria);

                // If a valid search ID is returned.
                if (searchID > 0)
                {
                    // Redirect to the search results page.
                    Response.Redirect(String.Format("{0}?protocolsearchid={1}", SearchPageInfo.SearchResultsPrettyUrl, searchID), true);
                }
                else
                {
                    // Otherwise, log an error.
                    EventLog eLog = new EventLog("CancerGov");
                    eLog.Source = "CancerGov";
                    eLog.WriteEntry("Unable to perform SaveAndExecuteSearch().", EventLogEntryType.Error);
                    eLog.Close();
                }
            }
        }

        private CTSearchDefinition BuildSearchCriteria()
        {
            string value;
            CTSearchDefinition criteria = new CTSearchDefinition();

            // Mark how this search was launched (used for logging purposes).
            criteria.SearchInvocationType = SearchInvocationType.FromSearchForm;

            // Cancer type, if present.
            if (SelectionIsPresent(ddlCancerType))
            {
                criteria.CancerType = new KeyValuePair<string, int>(ddlCancerType.SelectedItem.Text,
                    int.Parse(ddlCancerType.SelectedItem.Value));
            }
            else
                criteria.CancerType = new KeyValuePair<string, int>(string.Empty, 0);

            // Cancer subtype, if present
            if (SelectionIsPresent(cancerStage))
            {
                foreach (System.Web.UI.WebControls.ListItem item in cancerStage.Items)
                {
                    if (item.Selected)
                    {
                        criteria.CancerSubtypeIDList.Add(Strings.ToInt(item.Value));
                        criteria.CancerSubtypeNameList.Add(Strings.Clean(item.Text));
                    }
                }
            }

            // Trial type list, if present
            if (SelectionIsPresent(trialType))
            {
                // This is very, very ugly.  But the data and stored procs
                // all deal with treatment types as strings.  But by retrieving the
                // display text from the control instead of using values returned from
                // the form, we're guaranteed to get text which hasn't been tampered with.
                foreach (System.Web.UI.WebControls.ListItem item in trialType.Items)
                {
                    if (item.Selected)
                    {
                        criteria.TrialTypeList.Add(item.Text);
                    }
                }
            }

            // Drug List
            if (drug.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem item in drug.Items)
                {
                    // Check validity of item.Text via hash code contained in item.Value (CDRID+HashMaster hash and salt strings) - if valid, set value to CDRIF and add to criteria                    
                    if (HashMaster.SaltedHashCompareCompound(item.Text, item.Value, out value))
                        criteria.DrugList.Add(new KeyValuePair<string, int>(item.Text, int.Parse(value)));
                }
            }

            // Should drugs use AND or OR?
            if (drugListAllOrAny.Text.ToLower() == "all")
                criteria.RequireAllDrugsMatch = true;
            else
                criteria.RequireAllDrugsMatch = false;

            // Treatment/intervention list
            if (intervention.Items.Count > 0)
            {
                foreach (ListItem item in intervention.Items)
                {
                    // Check validity of item.Text via hash code contained in item.Value (CDRID+HashMaster hash and salt strings) - if valid, set value to CDRIF and add to criteria                    
                    if (HashMaster.SaltedHashCompareCompound(item.Text, item.Value, out value))
                        criteria.InterventionList.Add(new KeyValuePair<string, int>(item.Text, int.Parse(value)));
                }
            }

            // List of Trial Investigators
            if (investigator.Items.Count > 0)
            {
                foreach (ListItem item in investigator.Items)
                {
                    // Check validity of item.Text via hash code contained in item.Value (CDRID+HashMaster hash and salt strings) - if valid, set value to CDRIF and add to criteria                    
                    if (HashMaster.SaltedHashCompareCompound(item.Text, item.Value, out value))
                        criteria.InvestigatorList.Add(new KeyValuePair<string, int>(item.Text, int.Parse(value)));
                }
            }

            // Lead organization list
            if (leadOrg.Items.Count > 0)
            {
                foreach (ListItem item in leadOrg.Items)
                {
                    // Check validity of item.Text via hash code contained in item.Value (CDRID+HashMaster hash and salt strings) - if valid, set value to CDRIF and add to criteria                    
                    if (HashMaster.SaltedHashCompareCompound(item.Text, item.Value, out value))
                        criteria.LeadOrganizationList.Add(new KeyValuePair<string, int>(item.Text, int.Parse(value)));
                }
            }

            // Determine which location to use.
            switch (LocationTypeSelector.SelectedValue)
            {
                case LOCATION_ZIP:
                    {
                        // Don't attempt to set a ZIP location unless ZIP is numeric and greater than 0.
                        int zip = Strings.ToInt(zipCode.Text, 0);
                        if (zip > 0)
                        {
                            criteria.LocationSearchType = LocationSearchType.Zip;
                            criteria.SetLocationZipCriteria(zipCode.Text, zipCodeProximity.SelectedValue);
                        }
                    }
                    break;

                case LOCATION_HOSPITAL:
                    {
                        criteria.LocationSearchType = LocationSearchType.Institution;
                        foreach (ListItem item in institution.Items)
                        {
                            // Check validity of item.Text via hash code contained in item.Value (CDRID+HashMaster hash and salt strings) - if valid, set value to CDRIF and add to criteria                    
                            if (HashMaster.SaltedHashCompareCompound(item.Text, item.Value, out value))
                                criteria.LocationInstitutions.Add(new KeyValuePair<string, int>(item.Text, int.Parse(value)));
                        }
                    }
                    break;

                case LOCATION_CITY:
                    {
                        criteria.LocationSearchType = LocationSearchType.City;
                        criteria.LocationCity = city.Value;

                        // If the selected country is anything other than "All" or "U.S.A.",
                        // then the state list is ignored.
                        // Index 0 == "All"; Index 1 == "U.S.A."
                        if (country.SelectedIndex == 0 || country.SelectedIndex == 1)
                        {
                            if (SelectionIsPresent(state))
                            {
                                foreach (ListItem item in state.Items)
                                {
                                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                                    {
                                        criteria.LocationStateIDList.Add(item.Value);
                                        criteria.LocationStateNameList.Add(item.Text);
                                    }
                                }
                            }
                        }

                        // If "All" was selected (selected country was null or empty) and
                        // one or more states were selected, then force the country selection to
                        // be U.S.A.
                        if (country.SelectedIndex == 0 && criteria.LocationStateIDList.Count > 0)
                        {
                            criteria.LocationCountry = country.Items[1].Value;
                        }
                        else
                        {
                            // Otherwise, use whatever country was selected.
                            int index = country.SelectedIndex;
                            if (index >= 0)
                                criteria.LocationCountry = country.Items[index].Value;
                        }
                    }
                    break;

                case LOCATION_NIH:
                    {
                        criteria.LocationSearchType = LocationSearchType.NIH;
                        criteria.LocationNihOnly = nihOnly.Checked;
                    }
                    break;

                case LOCATION_ALL:
                default:
                    // Do Nothing.
                    break;
            }


            // Keywords
            if (!string.IsNullOrEmpty(txtKeywords.Text))
                criteria.Keywords = txtKeywords.Text;

            // Trial Status
            // Trial Status is no longer available in the search UI.
            // All trials are assumed to be open.  This is just forcing
            // the search.
            criteria.TrialStatusRestriction = TrialStatusType.OpenOnly;

            // Trial Phase
            if (SelectionIsPresent(trialPhase))
            {
                foreach (ListItem item in trialPhase.Items)
                {
                    // This is very, very ugly.  But the data and stored procs
                    // all deal with strings of the "Phase I", "Phase II", etc.
                    // variety.  But by retrieving the display text from the control
                    // instead of using values returned from the form, we're
                    // guaranteed to get text which hasn't been tampered with.
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        criteria.TrialPhase.Add(item.Text.Trim());
                    }
                }
            }

            // Last 30 days.
            if (newOnly.Checked)
                criteria.RestrictToRecent = true;
            else
                criteria.RestrictToRecent = false;

            // Specific Protocol ID
            if (!string.IsNullOrEmpty(protocolID.Text))
            {
                string[] idList = protocolID.Text.Split(',', ';');
                foreach (string id in idList)
                {
                    criteria.SpecificProtocolIDList.Add(id);
                }
            }

            return criteria;
        }

        /// <summary>
        /// Helper function to encapsulate the logic for verifying that a list has a selection.
        /// In addition to having a selected value, the selection must be at an index other
        /// than 0.  (Index zero is used throughout the page to mean "All.")
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        private bool SelectionIsPresent(ListControl selection)
        {
            bool selectionIsPresent = false;

            if (selection != null &&
                selection.SelectedIndex > 0)
            {
                selectionIsPresent = true;
            }

            return selectionIsPresent;
        }

        /// <summary>
        /// Helper function to encapsulate the logic for verifying that a list has a selection.
        /// In addition to having a selected value, the selection must be at an index other
        /// than 0.  (Index zero is used throughout the page to mean "All.")
        /// </summary>
        /// <param name="selection"></param>
        /// <returns></returns>
        private bool SelectionIsPresent(AccessibleCheckBoxList selection)
        {
            bool selectionIsPresent = false;

            if (selection != null &&
                selection.SelectedIndex > 0)
            {
                selectionIsPresent = true;
            }

            return selectionIsPresent;
        }
    }
}