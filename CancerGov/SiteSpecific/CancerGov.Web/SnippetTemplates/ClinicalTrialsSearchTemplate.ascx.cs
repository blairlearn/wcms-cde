using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CancerGov.CDR.ClinicalTrials.Search;
using System.Diagnostics;
using NCI.Util;

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsSearchTemplate : NCI.Web.CancerGov.Apps.AppsBaseUserControl
    {
        #region Constants

        // The only allowed values for marking a section as collapsed/expanded.
        protected const string COLLAPSED = "N";
        protected const string EXPANDED = "Y";

        #endregion


        public void SubmitButton_Click(object sender, ImageClickEventArgs e)
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
                    Response.Redirect(String.Format("ResultsClinicalTrials.aspx?protocolsearchid={0}", searchID), true);
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


        protected void cancerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cancerTypeID = 0;
            if (cancerType.SelectedIndex > 0)
                cancerTypeID = int.Parse(cancerType.SelectedValue);
            FillCancerStageSelectBox(cancerTypeID, null);
        }

        /// <summary>
        /// Click handler for the Institution list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InstutionListClearAll_ClickHandler(object sender, ImageClickEventArgs e)
        {
            institution.Items.Clear();
        }

        /// <summary>
        /// Click handler for the Drug list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DrugListClearAll_ClickHandler(object sender, ImageClickEventArgs e)
        {
            drug.Items.Clear();
        }

        /// <summary>
        /// Click handler for the Intervention list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InterventionListClearAll_ClickHandler(object sender, ImageClickEventArgs e)
        {
            intervention.Items.Clear();
        }

        /// <summary>
        /// Click handler for the Investigator list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void InvestigatorListClearAll_ClickHandler(object sender, ImageClickEventArgs e)
        {
            investigator.Items.Clear();
        }

        /// <summary>
        /// Click handler for the Lead Organization list's "Clear All" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LeadOrgClearAll_ClickHandler(object sender, ImageClickEventArgs e)
        {
            leadOrg.Items.Clear();
        }

        #region Page Life Cycle Events

        protected override void OnPreRender(EventArgs e)
        {
            /// Set up JavaScript resources. Order is important.  Because the page's script
            /// uses prototype, we need to register that one first.
            PrototypeManager.Load(this.Page);

            base.OnPreRender(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Force "Start Over" link to come back to whatever URL this page was invoked as.
            clear.HRef = Context.Request.FilePath;


            HelpPageLink = Strings.IfNull(Strings.Clean(ConfigurationSettings.AppSettings["ClinicalTrialsAdvancedSearchHelpPage"]), "#");
            CTCountOpen.Text = Strings.IfNull(Strings.Clean(ConfigurationSettings.AppSettings["ClinicalTrialsAdvancedSearchCountOpen"]), "5,000+");
            CTCountClosed.Text = Strings.IfNull(Strings.Clean(ConfigurationSettings.AppSettings["ClinicalTrialsAdvancedSearchCountClosed"]), "16,000+");

            SetLocationButtionsToStaticValues();

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
                        CancerGovError.LogError("", "CTSearchManager.LoadSavedCriteria", ErrorType.DbUnavailable, ex);
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
                FillTrialStatus(savedSearch);
                FillNewTrialBox(savedSearch);
                FillProtocolIDBox(savedSearch);

                FillSponsorsSelectBox(savedSearch);
                FillTrialInvestigatorBox(savedSearch);
                FillLeadOrganzationBox(savedSearch);

                FillSpecialCategorySelectBox(savedSearch);
                FillPhase(savedSearch);
                FillTrialType(savedSearch);

                SetLocationButtons(savedSearch);
            }


            this.WebAnalyticsPageLoad.SetChannelFromSectionNameAndUrl("Clinical Trial Search", this.Request.Url.OriginalString.ToString());
            this.WebAnalyticsPageLoad.AddProp(WebAnalyticsOptions.Props.RootPrettyURL, "/clinicaltrials/search"); // prop3
            this.WebAnalyticsPageLoad.AddProp(WebAnalyticsOptions.Props.ShortTitle, "Search for Clinical Trials"); // prop6
            this.WebAnalyticsPageLoad.AddProp(WebAnalyticsOptions.Props.PostedDate, ""); //prop25
            this.WebAnalyticsPageLoad.SetPageName(WebAnalyticsOptions.Hostname + "/clinicaltrials/search");
            litOmniturePageLoad.Text = this.WebAnalyticsPageLoad.Tag();

            string webAnalyticsParameters =
                String.Format(
                    "{{typeOfTrialControlID : '{0}',drugControlID : '{1}',treatnentInterventionControlID : '{2}',sponsorOfTrialControlID : '{3}',trialInvestigatorsControlID : '{4}',leadOrganizationCooperativeGroupControlID : '{5}', specialCategoryControlID : '{6}'}}",
                    trialType.ClientID,
                    drug.ClientID,
                    intervention.ClientID,
                    sponsor.ClientID,
                    investigator.ClientID,
                    leadOrg.ClientID,
                    specialCategory.ClientID
                );

            submit.OnClientClick = "doSubmit(" + webAnalyticsParameters + ");";
        }

        #endregion

        #region SelectListItem method and overloads

        private void SelectListItem(HtmlSelect selList, string selectedValue)
        {
            System.Web.UI.WebControls.ListItem selectedOption = null;

            if (selectedValue != null)
            {
                foreach (System.Web.UI.WebControls.ListItem li in selList.Items)
                {
                    if (selectedValue.IndexOf(";") == -1 && li.Value.IndexOf(";") != -1)
                    {
                        if (li.Value.Substring(li.Value.IndexOf(";") + 1).Trim().ToLower() == selectedValue.Trim().ToLower())
                        {
                            selectedOption = li;
                        }
                    }
                    else
                    {
                        if (li.Value.Trim().ToLower() == selectedValue.Trim().ToLower())
                        {
                            selectedOption = li;
                        }
                    }
                }

                if (selectedOption != null)
                {
                    if (!selList.Multiple && selList.Items[selList.SelectedIndex] != null)
                    {
                        selList.Items[selList.SelectedIndex].Selected = false;
                    }

                    selectedOption.Selected = true;
                }
            }
        }

        private void SelectListItem(HtmlSelect selList, string[] selectedValues)
        {
            System.Web.UI.WebControls.ListItem selectedOption = null;

            if (selectedValues[0] != null)
            {
                foreach (System.Web.UI.WebControls.ListItem li in selList.Items)
                {
                    foreach (string selectedValue in selectedValues)
                    {
                        if (selectedValue.IndexOf(";") == -1 && li.Value.IndexOf(";") != -1)
                        {
                            if (li.Value.Substring(li.Value.IndexOf(";") + 1) == selectedValue)
                            {
                                selectedOption = li;
                            }
                        }
                        else
                        {
                            if (li.Value.Trim().ToLower() == selectedValue.ToLower())
                            {
                                selectedOption = li;
                            }
                        }

                        if (selectedOption != null)
                        {
                            if (!selList.Multiple)
                            {
                                selList.Items[selList.SelectedIndex].Selected = false;
                            }

                            selectedOption.Selected = true;
                        }
                    }
                }
            }
        }

        #endregion

        private void FillCountryAndState(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<string> countryList;
            CTSearchFieldList<string> stateList;
            CTSearchManager.LoadStateAndCountryList(out countryList, out stateList);

            if (countryList != null && stateList != null)
            {
                state.AppendDataBoundItems = true;
                state.Items.Add(new System.Web.UI.WebControls.ListItem("All", ""));
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

        private void FillSponsorsSelectBox(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<string> sponsorsList = CTSearchManager.LoadSponsorList();

            sponsor.AppendDataBoundItems = true;
            sponsor.Items.Add(new System.Web.UI.WebControls.ListItem("All", "All"));
            sponsor.DataSource = sponsorsList;
            sponsor.DataValueField = "Value";
            sponsor.DataTextField = "Key";
            sponsor.DataBind();

            if (savedSearch == null)
            {
                // Default selection to "All"
                sponsor.SelectedIndex = 0;
            }
            else
            {
                bool matchFound = false;
                foreach (string id in savedSearch.SponsorIDList)
                {
                    ListItem item = sponsor.Items.FindByValue(id);
                    if (item != null)
                    {
                        item.Selected = true;
                        matchFound = true;
                    }
                }
                if (!matchFound)
                    sponsor.SelectedIndex = 0;
            }
        }

        private void FillSpecialCategorySelectBox(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<string> specialCategoryList = CTSearchManager.LoadSpecialCategoryList();

            specialCategory.AppendDataBoundItems = true;
            specialCategory.Items.Add(new System.Web.UI.WebControls.ListItem("All", "All"));
            specialCategory.DataSource = specialCategoryList;
            specialCategory.DataValueField = "Value";
            specialCategory.DataTextField = "Key";
            specialCategory.DataBind();

            if (savedSearch == null)
            {
                // Default selection to "All"
                specialCategory.SelectedIndex = 0;
            }
            else
            {
                bool matchFound = false;
                foreach (string value in savedSearch.SpecialCategoryList)
                {
                    ListItem item = specialCategory.Items.FindByValue(value);
                    if (item != null)
                    {
                        item.Selected = true;
                        matchFound = true;
                    }
                }
                if (!matchFound)
                    specialCategory.SelectedIndex = 0;
            }
        }

        private void FillCancerStageSelectBox(int cancerTypeID, CTSearchDefinition savedSearch)
        {
            emptySubType.Visible = false;

            if (cancerTypeID > 0)
            {
                CTSearchFieldList<int> cancerStageList = CTSearchManager.LoadCancerStageList(cancerTypeID);

                // We need to clear the list either way because of postbacks.
                cancerStage.Items.Clear();

                if (cancerStageList.Count < 1 && cancerType.SelectedIndex < 1)
                {
                    emptySubType.Visible = true;
                }
                else
                {
                    cancerStage.AppendDataBoundItems = true;
                    cancerStage.Items.Add(new System.Web.UI.WebControls.ListItem("All", ""));
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
                emptySubType.Visible = true;
            }

        }

        private void FillCancerTypeSelectBox(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<int> cancerTypeList = CTSearchManager.LoadCancerTypeList();

            cancerType.AppendDataBoundItems = true;
            cancerType.Items.Add(new System.Web.UI.WebControls.ListItem("All", ""));
            cancerType.DataSource = cancerTypeList;
            cancerType.DataValueField = "Value";
            cancerType.DataTextField = "Key";
            cancerType.DataBind();
            if (savedSearch == null)
            {
                // Default selection to "All"
                cancerType.SelectedIndex = 0;
            }
            else
            {
                // Because the list of cancer types includes synonyms, the cancer type values
                // cannot be trusted to be unique.  The cancer type text however is.
                ListItem item = cancerType.Items.FindByText(savedSearch.CancerType.Key);
                if (item != null)
                    item.Selected = true;
                else
                {
                    // If no match was found for the caner type text, then fail through
                    // and attempt with the first cancer type matching the value.
                    item = cancerType.Items.FindByValue(savedSearch.CancerType.Value.ToString());
                    if (item != null)
                        item.Selected = true;
                    else
                        cancerType.SelectedIndex = 0;
                }
            }
        }

        private void FillTrialType(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<int> trialTypeList = CTSearchManager.LoadTrialTypeList();
            trialType.DataSource = trialTypeList;
            trialType.DataValueField = "Value";
            trialType.DataTextField = "Key";
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

        private void FillPhase(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<int> trialPhaseList = CTSearchManager.LoadTrialPhaseList();
            trialPhase.Items.Clear();
            trialPhase.Items.Add(new ListItem("All", "0"));
            trialPhase.AppendDataBoundItems = true;
            trialPhase.DataSource = trialPhaseList;
            trialPhase.DataValueField = "Value";
            trialPhase.DataTextField = "Key";
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
                hospitalBox.Style.Add(HtmlTextWriterStyle.Display, "none");
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

        private void FillInterventionSelectionList(CTSearchDefinition savedSearch)
        {
            interventionListExpanded.Value = COLLAPSED;
            if (savedSearch != null)
            {
                // CTSearchDefinition guarantees that InterventionList will never be null.
                FillDeletableSelectionList(savedSearch.InterventionList, intervention, interventionListExpanded);
            }

            // If the list is still collapsed, hide it.
            if (interventionListExpanded.Value == COLLAPSED)
                interventionListArea.Style.Add(HtmlTextWriterStyle.Display, "none");
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

        private void FillKeywordText(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null && !string.IsNullOrEmpty(savedSearch.Keywords))
            {
                txtKeywords.Text = savedSearch.Keywords;
            }
        }

        private void FillTrialStatus(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null)
            {
                if (savedSearch.TrialStatusRestriction == TrialStatusType.OpenOnly)
                    trialStatus.SelectedIndex = 0;  // Open/Active
                else
                    trialStatus.SelectedIndex = 1;  // Closed/Inactive
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

        /// <summary>
        /// Helper method for populating and determining whether to display one of the collapsible lists.
        /// </summary>
        /// <param name="savedSearch"></param>
        /// <param name="list"></param>
        /// <param name="collapseState"></param>
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
        /// Sets the location buttons to their non-JavaScript (static page)
        /// equivalents.  This is necessary because we have two sets of
        /// buttons for the same functionality, and no way of knowing whether
        /// JavaScript was active.
        /// </summary>
        private void SetLocationButtionsToStaticValues()
        {
            if (hospitalLocationButton.Checked)
                hospitalLocationButtonStatic.Checked = true;
            else if (cityStateLocationButton.Checked)
                cityStateLocationButtonStatic.Checked = true;
            else if (atNihLocationButton.Checked)
                atNihLocationButtonStatic.Checked = true;
            else if (zipCodeLocationButton.Checked)
                zipCodeLocationButtonStatic.Checked = true;
        }

        private void SetLocationButtons(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null)
            {
                // Because we don't know whether JavaScript is enabled, the middle tier can
                // only set the location buttons for the static version of the page.
                // If JavaScript is active, the client-side UI code is responsible for
                // setting the dynamic location buttons appropriately.
                switch (savedSearch.LocationSearchType)
                {
                    case LocationSearchType.Institution:
                        hospitalLocationButtonStatic.Checked = true;
                        break;
                    case LocationSearchType.City:
                        cityStateLocationButtonStatic.Checked = true;
                        break;
                    case LocationSearchType.NIH:
                        atNihLocationButtonStatic.Checked = true;
                        break;
                    case LocationSearchType.Zip:
                    default:
                        zipCodeLocationButtonStatic.Checked = true;
                        break;
                }
            }
        }

        #region SelectIndex... method overloads

        private void SelectIndexByID(ref HtmlSelect select, int iID)
        {

            if (iID > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem liItem in select.Items)
                {
                    string strTmp = HttpUtility.UrlDecode(liItem.Value);

                    string[] starrKeyVal = strTmp.Split(';');

                    if (starrKeyVal.Length == 2)
                    {
                        if (starrKeyVal[1] == iID.ToString())
                        {
                            select.Items[0].Selected = false;
                            liItem.Selected = true;
                            continue;
                        }
                    }
                }
            }
        }

        private void SelectIndexByID(ref HtmlSelect select, ArrayList alList)
        {

            if (alList.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem liItem in select.Items)
                {
                    string strTmp = HttpUtility.UrlDecode(liItem.Value);

                    string[] starrKeyVal = strTmp.Split(';');

                    if (starrKeyVal.Length == 2)
                    {
                        if (alList.Contains(starrKeyVal[1]))
                        {
                            select.Items[0].Selected = false;
                            liItem.Selected = true;
                        }
                    }
                }
            }
        }

        private void SelectIndexByID(CheckBoxList list, ArrayList alList)
        {

            if (alList.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem liItem in list.Items)
                {
                    string strTmp = HttpUtility.UrlDecode(liItem.Value);

                    string[] starrKeyVal = strTmp.Split(';');

                    if (starrKeyVal.Length == 2)
                    {
                        if (alList.Contains(starrKeyVal[1]))
                        {
                            list.Items[0].Selected = false;
                            liItem.Selected = true;
                        }
                    }
                }
            }
        }

        private void SelectIndexByItem(CheckBoxList list, ArrayList alList)
        {

            if (alList.Count > 0)
            {
                foreach (string strItem in alList)
                {
                    System.Web.UI.WebControls.ListItem liItem = list.Items.FindByValue(strItem);

                    if (liItem != null)
                    {
                        list.Items[0].Selected = false;
                        liItem.Selected = true;
                    }
                }
            }
        }

        private void SelectIndexByItem(ref HtmlSelect select, ArrayList alList)
        {

            if (alList.Count > 0)
            {
                foreach (string strItem in alList)
                {

                    System.Web.UI.WebControls.ListItem liItem = select.Items.FindByValue(strItem);

                    if (liItem != null)
                    {
                        select.Items[0].Selected = false;
                        liItem.Selected = true;
                    }
                }
            }
        }

        private void SelectIndexByItem(ref HtmlSelect select, string strItem)
        {

            if (strItem != "")
            {

                System.Web.UI.WebControls.ListItem liItem = select.Items.FindByValue(strItem);

                if (liItem != null)
                {
                    select.Items[0].Selected = false;
                    liItem.Selected = true;
                }

            }
        }

        private void SelectIndexByItem(ref HtmlSelect select, int iItem)
        {

            if (iItem > 0)
            {

                System.Web.UI.WebControls.ListItem liItem = select.Items.FindByValue(iItem.ToString());

                if (liItem != null)
                {
                    select.Items[0].Selected = false;
                    liItem.Selected = true;
                }

            }
        }

        #endregion

        private CTSearchDefinition BuildSearchCriteria()
        {
            string value;
            CTSearchDefinition criteria = new CTSearchDefinition();

            // Mark how this search was launched (used for logging purposes).
            criteria.SearchInvocationType = SearchInvocationType.FromSearchForm;

            // Cancer type, if present.
            if (SelectionIsPresent(cancerType))
            {
                criteria.CancerType = new KeyValuePair<string, int>(cancerType.SelectedItem.Text,
                    int.Parse(cancerType.SelectedItem.Value));
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

            // Determine which location to use.  It isn't possible to have no buttons checked.
            if (zipCodeLocationButton.Checked || zipCodeLocationButtonStatic.Checked)
            {
                // Don't attempt to set a ZIP location unless ZIP is numeric and greater than 0.
                int zip = Strings.ToInt(zipCode.Text, 0);
                if (zip > 0)
                {
                    criteria.LocationSearchType = LocationSearchType.Zip;
                    criteria.SetLocationZipCriteria(zipCode.Text, zipCodeProximity.SelectedValue);
                }
            }
            else if (hospitalLocationButton.Checked || hospitalLocationButtonStatic.Checked)
            {
                criteria.LocationSearchType = LocationSearchType.Institution;
                foreach (ListItem item in institution.Items)
                {
                    // Check validity of item.Text via hash code contained in item.Value (CDRID+HashMaster hash and salt strings) - if valid, set value to CDRIF and add to criteria                    
                    if (HashMaster.SaltedHashCompareCompound(item.Text, item.Value, out value))
                        criteria.LocationInstitutions.Add(new KeyValuePair<string, int>(item.Text, int.Parse(value)));
                }
            }
            else if (cityStateLocationButton.Checked || cityStateLocationButtonStatic.Checked)
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
            else if (atNihLocationButton.Checked || atNihLocationButtonStatic.Checked)
            {
                criteria.LocationSearchType = LocationSearchType.NIH;
                criteria.LocationNihOnly = nihOnly.Checked;
            }

            // Keywords
            if (!string.IsNullOrEmpty(txtKeywords.Text))
                criteria.Keywords = txtKeywords.Text;

            // Trial Status
            if (trialStatus.Text == "1")
                criteria.TrialStatusRestriction = TrialStatusType.OpenOnly;
            else
                criteria.TrialStatusRestriction = TrialStatusType.ClosedOnly;

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

            // Sponsor List
            if (SelectionIsPresent(sponsor))
            {
                foreach (ListItem item in sponsor.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        criteria.SponsorIDList.Add(item.Value);
                    }
                }
            }

            // Special Categories
            if (SelectionIsPresent(specialCategory))
            {
                foreach (ListItem item in specialCategory.Items)
                {
                    if (item.Selected && !string.IsNullOrEmpty(item.Value))
                    {
                        criteria.SpecialCategoryList.Add(item.Value);
                    }
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
    }
}