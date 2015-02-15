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
using NCI.Web.UI.WebControls.JSLibraries;   // In order to reference Prototype.
using NCI.Logging;
using NCI.Web.CDE.Modules; 

namespace CancerGov.Web.SnippetTemplates
{
    public partial class ClinicalTrialsSearchTemplate : SearchBaseUserControl
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //Need to add ARIA tags to location controls - this has nothing to do with the state of the form,
            //so we are doing it on init.

            //Tell the radio which fieldset it controls
            zipCodeLocationButton.InputAttributes.Add("aria-controls", zipCodeLocationFieldset.ClientID);
            //Tell the fieldset which radio labels it
            zipCodeLocationFieldset.Attributes.Add("aria-labelledby", zipCodeLocationButton.ClientID);

            //Tell the radio which fieldset it controls
            cityStateLocationButton.InputAttributes.Add("aria-controls", cityStateLocationFieldset.ClientID);
            //Tell the fieldset which radio labels it
            cityStateLocationFieldset.Attributes.Add("aria-labelledby", cityStateLocationButton.ClientID);

            //Tell the radio which fieldset it controls
            hospitalLocationButton.InputAttributes.Add("aria-controls", hospitalLocationFieldset.ClientID);
            //Tell the fieldset which radio labels it
            hospitalLocationFieldset.Attributes.Add("aria-labelledby", hospitalLocationButton.ClientID);

            //Tell the radio which fieldset it controls
            atNihLocationButton.InputAttributes.Add("aria-controls", atNihLocationFieldset.ClientID);
            //Tell the fieldset which radio labels it
            atNihLocationFieldset.Attributes.Add("aria-labelledby", atNihLocationButton.ClientID);


            txtKeywords.Attributes.Add("placeholder", "Examples: PSA, HER-2, &quot;Paget disease&quot;");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CTSearchDefinition savedSearch = null;

                int cancerTypeID = 0;
                //Get Saved Search ID

                FillCancerTypeSelectBox(savedSearch);
                FillCancerStageSelectBox(cancerTypeID, savedSearch);

                FillZipLocationSelectBox(savedSearch);

                FillCountryAndState(savedSearch);
                FillNihLocationSelectBox(savedSearch);

                FillSponsorsSelectBox(savedSearch);

                FillKeywordText(savedSearch);
                FillSpecialCategorySelectBox(savedSearch);
                FillPhase(savedSearch);
                FillTrialType(savedSearch);
                FillNewTrialBox(savedSearch);

                SetLocationButtons(savedSearch);
            }

            



        }

        private void SetLocationButtons(CTSearchDefinition savedSearch)
        {
            if (savedSearch != null)
            {
                switch (savedSearch.LocationSearchType)
                {
                    case LocationSearchType.Institution:
                        hospitalLocationButton.Checked = true;
                        break;
                    case LocationSearchType.City:
                        cityStateLocationButton.Checked = true;
                        break;
                    case LocationSearchType.NIH:
                        atNihLocationButton.Checked = true;
                        break;
                    case LocationSearchType.Zip:
                    default:
                        zipCodeLocationButton.Checked = true;
                        break;
                }
            }
            else
            {
                zipCodeLocationButton.Checked = true;
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

        private void FillSpecialCategorySelectBox(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<string> specialCategoryList = CTSearchManager.LoadSpecialCategoryList();

            specialCategory.AppendDataBoundItems = true;
            specialCategory.Items.Add(new System.Web.UI.WebControls.ListItem("All", "All"));
            specialCategory.DefaultIndex = 0;
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

        private void FillSponsorsSelectBox(CTSearchDefinition savedSearch)
        {
            CTSearchFieldList<string> sponsorsList = CTSearchManager.LoadSponsorList();

            sponsor.AppendDataBoundItems = true;
            sponsor.Items.Add(new System.Web.UI.WebControls.ListItem("All", "All"));
            sponsor.DefaultIndex = 0;
            sponsor.DataSource = sponsorsList;
            sponsor.DataValueField = "Value";
            sponsor.DataTextField = "Key";
            sponsor.DataBind();

            if (savedSearch == null)
            {
                // if no search yet specified, search for "NCI" and select that item if found
                ListItem item = sponsor.Items.FindByText("NCI");
                if (item != null)
                {
                    item.Selected = true;
                }
                else
                {
                    // Default selection to "All"
                    sponsor.SelectedIndex = 0;
                }
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
                {
                    // if no search yet specified, search for "NCI" and select that item if found
                    ListItem item = sponsor.Items.FindByText("NCI");
                    if (item != null)
                    {
                        item.Selected = true;
                    }
                    else
                    {
                        // Default selection to "All"
                        sponsor.SelectedIndex = 0;
                    }
                }
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
    }
}