using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using CancerGov.CDR.DataManager;
using CancerGov.Common.ErrorHandling;
using CancerGov.CDR.ClinicalTrials.Search;
using CancerGov.UI.CDR;

using NCI.Web.UI.WebControls;
using NCI.Util;
using NCI.Web.UI.WebControls.FormControls;  // For the CTSearchCriteriaDisplay object.
using NCI.Web.CancerGov.Apps;
using NCI.Logging;
using NCI.Web.CDE;
using NCI.Web.CDE.Modules;
using NCI.Web.CDE.WebAnalytics;

namespace CancerGov.Web.SnippetTemplates
{
    /// <summary>
    /// Summary description for ClinicalTrialsResults.
    /// </summary>
    public partial class ClinicalTrialsResults : SearchBaseUserControl
    {
        private AdvancedSearchResultRenderer _pageRenderer = null;

        private const int DEFAULT_RESULTS_PER_PAGE = 25;

        enum PageRenderingState
        {
            None = 0,
            Results = 1,
            CustomSelections = 2
        };
        PageRenderingState _pageRenderingState = PageRenderingState.Results;


        #region Properties

        /// <summary>
        /// Master list of selected trials.
        /// </summary>
        protected List<int> SelectedTrialMasterList
        {
            get
            {
                if (ViewState["SelectedTrialMasterList"] == null)
                    ViewState["SelectedTrialMasterList"] = new List<int>();
                return (List<int>)ViewState["SelectedTrialMasterList"];
            }
        }

        /// <summary>
        /// List of trials which were rendered when the page was displayed.
        /// </summary>
        protected List<int> RenderedTrials
        {
            get
            {
                if (ViewState["RenderedTrials"] == null)
                    ViewState["RenderedTrials"] = new List<int>();
                return (List<int>)ViewState["RenderedTrials"];
            }
        }

        /// <summary>
        /// Preserves the currently active audience selection across page transitions.
        /// </summary>
        protected ProtocolVersions CurrentAudience
        {
            //As part of CTRP data feed phase 1. there will only be one version, so we will
            //force to HP always
            get { return ProtocolVersions.HealthProfessional; }
            set { return; }
        }

        /// <summary>
        /// Preserves the currently active display format selection across page transitions.
        /// </summary>
        protected ProtocolDisplayFormats CurrentDisplayFormat
        {
            get { return (ProtocolDisplayFormats)(ViewState["CurrentDisplayFormat"] ?? ProtocolDisplayFormats.Short); }
            set { ViewState["CurrentDisplayFormat"] = value; }
        }

        /// <summary>
        /// Preserves the current state of the Description format's "Location" checkbox.
        /// </summary>
        protected bool DescriptionIncludesLocation
        {
            get { return (bool)(ViewState["DescriptionIncludesLocation"] ?? false); }
            set { ViewState["DescriptionIncludesLocation"] = value; }
        }

        /// <summary>
        /// Preserves the current state of the Description format's "Eligibility" checkbox.
        /// </summary>
        protected bool DescriptionIncludesEligibility
        {
            get { return (bool)(ViewState["DescriptionIncludesEligibility"] ?? false); }
            set { ViewState["DescriptionIncludesEligibility"] = value; }
        }

        /// <summary>
        /// Preserves the current result sort order across page transitions.
        /// </summary>
        protected CTSSortFilters PersistentSortOrder
        {
            get { return (CTSSortFilters)(ViewState["PersistentSortOrder"] ?? CTSSortFilters.PhaseDesc); }
            set { ViewState["PersistentSortOrder"] = value; }
        }

        /// <summary>
        /// Preserves the currently effective number of records per page across page transitions.
        /// </summary>
        protected int PersistentRecordsPerPage
        {
            get { return (int)(ViewState["PersistentRecordsPerPage"] ?? DEFAULT_RESULTS_PER_PAGE); }
            set { ViewState["PersistentRecordsPerPage"] = value; }
        }

        #endregion


        #region Page Lifecycle Events

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //This will probably go away.  At least I hope it will.
            //this.PageLeftColumn = new LeftNavColumn((BasePage)this);

            int protocolSearchID = Strings.ToInt(Strings.Clean(Request.Params["protocolsearchid"]));

            // If there's no valid protocol search ID, redirect to the UI page.
            if (protocolSearchID == -1)
                this.RaiseErrorPage("InvalidSearchID");

            if (!IsPostBack)
            {
                // This block of code is only executed on the initial page load.
                // Search results will not be loaded in this block.  Most (all?)
                // of the reasons for loading this page via a PostBack involve
                // changes to how the search results are displayed, so we don't want
                // to do anything that would involve maintaining search results
                // in the page's ViewState.

                DisplayCriteria criteria = null;
                try
                {
                    // Load search criteria.  Collapse/Expand of display is handled further down.
                    criteria = CTSearchManager.LoadDisplayCriteria(protocolSearchID);

                    CriteriaDisplay.Criteria = criteria;

                    // Set up the sort order list.
                    // Normally, PersistentSortOrder is only updated in UpdateSortOrderAndPageSize_Click().
                    // But on the initial page load, we also need to determine whether keywords are
                    // present in the search criteria because that requires a different sort order.
                    bool includesKeyword = !string.IsNullOrEmpty(criteria.Keywords);
                    if (includesKeyword)
                        PersistentSortOrder = CTSSortFilters.Relevance;
                    else
                        PersistentSortOrder = CTSSortFilters.PhaseDesc;
                    InitializeSortOrderList(includesKeyword, PersistentSortOrder);

                    // Set up page size
                    InitializeResultsPerPage();

                    // Set up the pager
                    pager.RecordCount = 0;
                    pager.RecordsPerPage = PersistentRecordsPerPage;
                    pager.CurrentPage = 1;

                    // Make page size selection visible to JavaScript.
                    pageSize.Value = PersistentRecordsPerPage.ToString("d");
                }
                catch (Exception ex)
                {
                    CancerGovError.LogError("", "CTSearchManager.LoadDisplayCriteria", CancerGov.Common.ErrorHandling.ErrorType.DbUnavailable, ex);
                    this.RaiseErrorPage("InvalidSearchID");
                }
            }

            //WebAnalytics
            //this.WebAnalyticsPageLoad.SetChannelFromSectionNameAndUrl("Clinical Trial Search", this.Request.Url.OriginalString.ToString());
            this.PageInstruction.AddFieldFilter("channelName", (fieldName, data) =>
                {
                    data.Value = "Clinical Trial Search";
                });
            //End Web Analytics
            
            this.PageInstruction.AddFieldFilter("invokedFrom", (name, field) =>
            {
                field.Value = EmailPopupInvokedBy.ClinicalTrialSearchResults.ToString("d");
            });

            this.PageInstruction.AddUrlFilter("EmailUrl", (name, url) =>
            {
                foreach (string key in Request.QueryString)
                    url.QueryParameters.Add(key, Request.QueryString[key]);
            });

            this.PageInstruction.AddUrlFilter("BookMarkShareUrl", (name, url) =>
            {
                foreach (string key in Request.QueryString)
                    url.QueryParameters.Add(key, Request.QueryString[key]);
            });
            
            this.PageInstruction.AddUrlFilter(PageAssemblyInstructionUrls.CanonicalUrl, (name, url) =>
            {
                string localUrl = url.ToString();

                if (protocolSearchID > -1)
                    localUrl += "?protocolsearchid=" + protocolSearchID;

                url.SetUrl(localUrl);
            });

            JSManager.AddExternalScript(this.Page, "/JS/Search/CDEResultsClinicalTrials.js");
            JSManager.AddExternalScript(this.Page, "/JS/popEvents.js");
            //JSManager.AddExternalScript(this.Page, "/scripts/JSLoader/JSLoader.js");
           // CssManager.AddStyleSheet(this.Page, "/StyleSheets/jquery.css", String.Empty);
            
        }

        /// <summary>
        /// The LoadComplete event fires after all controls on the page have loaded.  After the
        /// controls have been loaded, now we can examine their values and decide what to do
        /// with them.
        /// </summary>
        /// <param name="e"></param>
        protected void OnLoadComplete()
        {
            // Synchronize the various radio button sets with their assigned values.
            SynchronizeDisplayFormatSelection();
            SynchronizeSortOrder();
            SynchronizeResultsPerPage();

            // Retrieve search results.
            CTSearchResultOptions displayOptions = null;
            int totalResultCount = 0;

            int protocolSearchID = GetProtocolSearchID();

            ReconcileSelectedProtocolList();

            if (_pageRenderingState == PageRenderingState.Results)
            {

                ProtocolCollection pcProtocols = new ProtocolCollection();

                bool bDrawStudySites = false;

                ProtocolSectionTypes[] sectionList;

                if (CurrentDisplayFormat == ProtocolDisplayFormats.Custom)
                {
                    sectionList = customSections.SelectedIDs;
                    foreach (ProtocolSectionTypes section in sectionList)
                    {
                        if (section == ProtocolSectionTypes.StudySites)
                        {
                            bDrawStudySites = true;
                            break;
                        }
                    }
                }
                else
                {
                    sectionList = ProtocolFunctions.BuildSectionList(CurrentAudience, CurrentDisplayFormat,
                        DescriptionIncludesEligibility, DescriptionIncludesLocation);
                    bDrawStudySites = DecideToDrawStudySites();
                }

                displayOptions = new CTSearchResultOptions(CurrentAudience, PersistentSortOrder, CurrentDisplayFormat, sectionList, bDrawStudySites);


                try
                {
                    // Retrieve protocols.
                    pcProtocols = CTSearchManager.LoadCachedSearchResults(protocolSearchID, displayOptions,
                        pager.CurrentPage, PersistentRecordsPerPage, out totalResultCount);
                }
                catch (ProtocolFetchFailureException fetchError)
                {
                    CancerGovError.LogError("ClinicalTrialsResultsAdvanced.aspx", 1, "Failed to fetch Results for PSID : " + protocolSearchID.ToString() + " Error : " + fetchError.Message);
                    this.RaiseErrorPage("InvalidSearchID");
                }
                catch (ProtocolTableEmptyException fetchError)
                {
                    CancerGovError.LogError("ClinicalTrialsResultsAdvanced", 1, "ProtocolSearchID = " + protocolSearchID.ToString() + " Error: " + fetchError.Message);
                    this.RaiseErrorPage("InvalidSearchID");
                }
                catch (ProtocolTableMiscountException fetchError)
                {
                    CancerGovError.LogError("ClinicalTrialsResultsAdvanced", 1, "ProtocolSearchID = " + protocolSearchID.ToString() + " Error: " + fetchError.Message);
                    this.RaiseErrorPage("InvalidSearchID");
                }

                pager.RecordCount = totalResultCount;

                _pageRenderer = new AdvancedSearchResultRenderer(PageDisplayInformation, pcProtocols, SelectedTrialMasterList.ToArray(), CurrentDisplayFormat, SearchPageInfo.DetailedViewSearchResultPagePrettyUrl);
                RecordTrialsOnPage(pcProtocols);

                if (pcProtocols.TotalResults > 0)
                {
                    int firstResult, lastResult;
                    PostBackButtonPager.GetFirstItemLastItem(pager.CurrentPage, pager.RecordsPerPage, pager.RecordCount,
                        out firstResult, out lastResult);

                    ResultsCountText.Text = string.Format("Results {0}-{1} of {2} for your search:", firstResult, lastResult, pager.RecordCount);
                }
                else
                {
                    // If there are no results, we hide a bunch of things.  There is no need to switch
                    // any of them back on anywhere since the user will have to exit the page to choose
                    // different search criteria.
                    ResultsFormatControl.Visible = false;
                    topControlArea.Visible = false;
                    lowerControlArea.Visible = false;
                    TopPrintButton.Visible = false;
                    ProtocolContent.Visible = false;
                    BottomActionControls.Visible = false;
                    ResultsCountText.Text = "No results found.";
                    NoResultsMessage.Visible = true;
                }

                // Clear "Select All" check boxes.
                checkAllTop.Checked = false;
                checkAllBottom.Checked = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            OnLoadComplete();
            /// Set up JavaScript resources. Order is important.  Because the page's script
            /// uses prototype, we need to register that one first.
            //PrototypeManager.Load(this.Page);

            if (_pageRenderingState == PageRenderingState.Results && _pageRenderer != null)
            {
                ProtocolContent.Text = _pageRenderer.Render();
            }

            base.OnPreRender(e);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Helper method to encapsulate the logic for retrieving the requested
        /// protocol search ID.
        /// </summary>
        /// <returns>Integer value identifiying the protocol search to retrieve.</returns>
        protected int GetProtocolSearchID()
        {
            int protocolSearchID = Strings.ToInt(Strings.Clean(Request.Params["protocolsearchid"]));
            if (protocolSearchID == -1)
            {
                CancerGovError.LogError("ClinicalTrialsResultsAdvanced.aspx", 1, "Invalid Protocol Search ID");
                this.RaiseErrorPage("InvalidSearchID");
            }

            return protocolSearchID;
        }

        /// <summary>
        /// Helper method to initialize the sort order dropdown list.
        /// </summary>
        /// <param name="includeRelevance">boolean value noting whether to include
        /// relevance ordering.</param>
        /// <remarks>InitializeSortOrderList() inserts values into the Sort Order
        /// drop down list.  If the search criteria include keyword text,
        /// includeRelevance should be set true, causing "Relevance" to be included
        /// as an available sort order and marked as the default selection.
        /// 
        /// InitializeSortOrderList() should only be called on the initial page load.</remarks>
        private void InitializeSortOrderList(bool includeRelevance, CTSSortFilters defaultSortOrder)
        {
            sortOrder.Items.Clear();
            if (includeRelevance)
                sortOrder.Items.Add(new ListItem("Relevance", CTSSortFilters.Relevance.ToString("d")));
            sortOrder.Items.Add(new ListItem("Phase of Trial", CTSSortFilters.PhaseDesc.ToString("d")));
            sortOrder.Items.Add(new ListItem("Title", CTSSortFilters.TitleAsc.ToString("d")));
            sortOrder.Items.Add(new ListItem("Type of Trial", CTSSortFilters.TrialTypeAsc.ToString("d")));
            sortOrder.Items.Add(new ListItem("Status of Trial", CTSSortFilters.StatusAsc.ToString("d")));
            sortOrder.Items.Add(new ListItem("Age Range", CTSSortFilters.AgeRangeAsc.ToString("d")));
            sortOrder.Items.Add(new ListItem("Trial ID", CTSSortFilters.ProtocolIDAsc.ToString("d")));

            // Set the default sort order.
            ListItem item = sortOrder.Items.FindByValue(defaultSortOrder.ToString("d"));
            if (item != null)
                item.Selected = true;
        }

        /// <summary>
        /// Helper method to initialize the "results per page" dropdown list.
        /// 
        /// This method should only be called on the initial page load.
        /// </summary>
        private void InitializeResultsPerPage()
        {
            resultsPerPage.Items.Clear();
            resultsPerPage.Items.Add(new ListItem("10", "10"));
            resultsPerPage.Items.Add(new ListItem("25", "25"));
            resultsPerPage.Items.Add(new ListItem("50", "50"));
            resultsPerPage.Items.Add(new ListItem("100", "100"));
            resultsPerPage.Items.Add(new ListItem("200", "200"));

            resultsPerPage.SelectedValue = DEFAULT_RESULTS_PER_PAGE.ToString("d");
        }

        /// <summary>
        /// Encapsulates the logic for determining which elements are visible when
        /// switching between rendering results or the custom sections view.
        /// </summary>
        /// <param name="state"></param>
        private void SetPageRenderingState(PageRenderingState state)
        {
            switch (state)
            {
                case PageRenderingState.Results:
                    CustomSectionsDisplay.Visible = false;
                    ResultsDisplay.Visible = true;
                    _pageRenderingState = state;
                    break;
                case PageRenderingState.CustomSelections:
                    CustomSectionsDisplay.Visible = true;
                    ResultsDisplay.Visible = false;
                    _pageRenderingState = state;
                    break;
                default:
                    // If this is an unknown state, don't change anything.
                    break;
            }
        }

        /// <summary>
        /// Determines whether study sites should be displayed based on the current display format.
        /// </summary>
        /// <returns>True if sites should be displayed, false if not</returns>
        private bool DecideToDrawStudySites()
        {
            bool shouldBeDrawn;

            if ((CurrentDisplayFormat == ProtocolDisplayFormats.Long) ||
                (CurrentDisplayFormat == ProtocolDisplayFormats.SingleProtocol) ||
                (CurrentDisplayFormat == ProtocolDisplayFormats.Medium && DescriptionIncludesLocation))
            {
                shouldBeDrawn = true;
            }
            else
            {
                shouldBeDrawn = false;
            }

            return shouldBeDrawn;
        }

        #endregion

        #region Manage List of Selected Trials
        // So, what's going on here?  Well, first you have to keep in mind that although we know trials
        // appear in the search results when we render them, when a post-back event occurs, the only
        // information sent back is the list that was marked as selected.  There's no "memory" of what
        // was sent out to be selected from, much less what was selected on a previous cycle. So we need
        // to track this information in the code-behind.
        // 
        // Every time a set of search results is rendered to the output, we need to record the list of
        // protocols involved.
        // 
        // Next, when the user posts back for any reason, we reconcile the list of protocols which
        // are marked as *selected* against the list which was rendered onto the page. If a trial
        // appears in the list which was marked as selected, then we add it to the master list of
        // selected trials.  If a trial appears in the list that was previously rendered, but does
        // not appear in the selected list, then we make sure it doesn't appear in the master list
        // and if it does, remove it.


        /// <summary>
        /// Records the list of trials being rendered on the page.
        /// </summary>
        /// <param name="protocols">A ProtocolCollection containing the set of protocols
        /// which are being rendered onto the page.</param>
        private void RecordTrialsOnPage(ProtocolCollection protocols)
        {
            RenderedTrials.Clear();
            RenderedTrials.Capacity = protocols.Count;  // Eliminate any need to reallocate.

            foreach (Protocol protocol in protocols)
            {
                RenderedTrials.Add(protocol.CdrId);
            }

            // Are there any trials selected which don't appear on this page?
            bool offPageTrialsExist = SelectedTrialMasterList.Exists(delegate(int trialID)
            { return !RenderedTrials.Contains(trialID); });
            if (offPageTrialsExist)
                OffPageSelectionsExist.Value = "Y";
            else
                OffPageSelectionsExist.Value = "N";
        }

        private void ReconcileSelectedProtocolList()
        {
            string strCDRIDs = Strings.Clean(Request.Form["cdrid"]);
            string[] splitIDs;
            List<int> selectedIDs = null;

            // If there are selections, it is logically impossible for RenderedTrials to be empty.
            if (!string.IsNullOrEmpty(strCDRIDs))
            {
                // Convert list of selected IDs into an array of ints.
                splitIDs = strCDRIDs.Split(',');
                selectedIDs = new List<int>(splitIDs.Length);
                for (int i = 0; i < splitIDs.Length; i++)
                {
                    selectedIDs.Add(Strings.ToInt(splitIDs[i]));
                }

                RenderedTrials.ForEach(delegate(int renderedID)
                {
                    bool isSelected = selectedIDs.Contains(renderedID);
                    bool isInMasterList = SelectedTrialMasterList.Contains(renderedID);

                    // If the rendered ID is selected, and not already in the master list,
                    // add it to the master list.
                    if (isSelected && !isInMasterList)
                    {
                        SelectedTrialMasterList.Add(renderedID);
                    }
                    // If the rendered ID was not selected, but exists in the master list, remove it.
                    else if (!isSelected && isInMasterList)
                    {
                        SelectedTrialMasterList.RemoveAll(delegate(int trialID)
                        {
                            return trialID == renderedID;
                        });
                    }
                });
            }
        }

        #endregion

        #region Event Handlers

        protected void refineSearch_ServerClick(object sender, EventArgs e)
        {
            Response.Redirect(SearchPageInfo.SearchPagePrettyUrl + "?protocolsearchid=" + GetProtocolSearchID());
        }

        /// <summary>
        /// Event handler for save of the custom sections control.
        /// Hides the custom sections control; makes the results section visible.
        /// Sets the page rendering state back to rendering results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CustomSectionsChanged(object sender, EventArgs e)
        {
            SetPageRenderingState(PageRenderingState.Results);
        }

        protected void PageChanged(object sender, EventArgs e)
        {
            // By default, search criteria are displayed when switching to page 1, and collapsed
            // when switching to any other page.  Remove this block of code to maintain the
            // user-selected collapse/expand state. [FR9255-010 & FR9255-020]
            if (pager.CurrentPage == 1)
                DisplaySearchCriteriaCollapsed.Value = "Y";
            else
                DisplaySearchCriteriaCollapsed.Value = "N";
        }

        /// <summary>
        /// Print handler for the image button version of "Display for Print".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DisplayForPrint_ClickHandler(object sender, EventArgs e)
        {
            // Make certain the master list is up to date.
            ReconcileSelectedProtocolList();

            // Don't try to generate a print page if there's nothing to print.
            if (SelectedTrialMasterList.Count > 0)
            {
                ProtocolCollection protocols = LoadProtocolsForPrinting();
                string pageHtml = RenderPrintableResults(protocols);

                int cacheID = CTSearchManager.CachePageHtml(pageHtml);

                Response.Redirect(string.Format("{2}?cid={0}&protocolsearchid={1}", cacheID, GetProtocolSearchID(), SearchPageInfo.PrintSearchResultPagePrettyUrl));
            }
        }

        /// <summary>
        /// Encapsulates the logic for loading a printable version of only those
        /// protocols which have been stored in the master selection list.
        /// </summary>
        /// <returns></returns>
        private ProtocolCollection LoadProtocolsForPrinting()
        {
            ProtocolCollection protocols;

            // Build up display options
            bool bDrawStudySites = false;
            ProtocolSectionTypes[] sectionList;
            ProtocolVersions protocolVersion = CurrentAudience;
            ProtocolDisplayFormats pdfFormat = CurrentDisplayFormat;
            CTSearchResultOptions displayOptions = null;

            if (pdfFormat == ProtocolDisplayFormats.Custom)
            {
                sectionList = customSections.SelectedIDs;
                bDrawStudySites = Array.Exists(sectionList,
                    delegate(ProtocolSectionTypes section) { return section == ProtocolSectionTypes.StudySites; });
            }
            else
            {
                sectionList = ProtocolFunctions.BuildSectionList(protocolVersion, pdfFormat,
                    DescriptionIncludesEligibility, DescriptionIncludesLocation);
                bDrawStudySites = DecideToDrawStudySites();
            }

            displayOptions = new CTSearchResultOptions(protocolVersion, PersistentSortOrder, pdfFormat, sectionList, bDrawStudySites);

            // Retrieve only the protocols the user has selected.
            protocols = CTSearchManager.LoadSelectedProtocols(SelectedTrialMasterList.ToArray(),
                displayOptions, GetProtocolSearchID());

            return protocols;
        }

        private string RenderPrintableResults(ProtocolCollection protocols)
        {
            string renderedResults;

            ProtocolDisplayFormats displayFormat = CurrentDisplayFormat;

            // Load search criteria and create a renderer.
            DisplayCriteria criteria = CTSearchManager.LoadDisplayCriteria(protocols.ProtocolSearchID);
            CTSearchCriteriaDisplay criteriaRenderer = new CTSearchCriteriaDisplay();
            criteriaRenderer.Criteria = criteria;
            criteriaRenderer.CssClass = "clinicaltrials-results-criteria-display";

            // After the first protocol is "printed", page breaks should occur at the beginning of each protocol.
            // For the Short/Title-only format, there are no page breaks.
            bool bRenderPageBreaks = (displayFormat != ProtocolDisplayFormats.Short);
            PrintSearchResultsRenderer psrrResults =
                new PrintSearchResultsRenderer(PageDisplayInformation, protocols, displayFormat, bRenderPageBreaks, SearchPageInfo.DetailedViewSearchResultPagePrettyUrl);

            // Now that we have our two renderers, we can generate the string.
            renderedResults = "<p class=\"clinicaltrials-results-criteria-label\">Search Criteria:</p>" + criteriaRenderer.Render() + "" + psrrResults.Render();

            return renderedResults;
        }

        #endregion

        #region Audience and Display Handling.

        // The display options selected by the audience and display format controls don't go into effect
        // until the user commits to them by clicking the "GO" button.
        // 
        // So instead of evaluating the values contained in the controls directly, the code needs to
        // evaluate the corresponding state properties instead:
        // 
        //     CurrentAudience -- This is being forced to HP as part of CTRP Data Feed Phase 1
        //     CurrentDisplayFormat -- Which display formatio should be used?
        //     DescriptionIncludesLocation -- If the Description format is in use, should locations be included?
        //     DescriptionIncludesEligibility -- If the Description format is in use, should eligibility be included?
        //     
        // These properties are updated by UpdateAudienceAndDisplay_Click().
        // The selections are restored on page load by calling SynchronizeAudienceSelection() and
        // SynchronizeDisplayFormatSelection().
        // 
        // This block of code is intended to be the only place where the page updates or
        // evaluates the on-screen controls, and the only place which updates the state properties.

        /// <summary>
        /// Forces the display format controls to correspond to their preserved state
        /// from previous page loads.
        /// </summary>
        private void SynchronizeDisplayFormatSelection()
        {
            // In order to restore the buttons to their previous state, we first
            // have to clear any that are already checked.  (This wouldn't be a
            // problem if we could use a RadioButtonGroup, but the page layout
            // won't allow it.)
            titleFormat.Checked = false;
            descriptionFormat.Checked = false;
            fullDescriptionFormat.Checked = false;
            customFormat.Checked = false;

            includeLocations.Checked = false;
            includeEligibility.Checked = false;

            // Set the button for the current selection.
            switch (CurrentDisplayFormat)
            {
                case ProtocolDisplayFormats.Short:
                    titleFormat.Checked = true;
                    break;
                case ProtocolDisplayFormats.Medium:
                    descriptionFormat.Checked = true;
                    break;
                case ProtocolDisplayFormats.Long:
                    fullDescriptionFormat.Checked = true;
                    break;
                case ProtocolDisplayFormats.Custom:
                    customFormat.Checked = true;
                    break;
            }

            // Set the description format's sub-options.
            includeLocations.Checked = DescriptionIncludesLocation;
            includeEligibility.Checked = DescriptionIncludesEligibility;
        }

        /// <summary>
        /// Click handler for the "GO" button in the Audience and Display section of the results page.
        /// 
        /// This method determines which buttons are active, updates the page's CurrentAudience and 
        /// CurrentDisplayFormat properties, enables/disables the "Custom" format button as necessary,
        /// and (if needed) switches the page's rendering mode to display the custom section selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateAudienceAndDisplay_Click(object sender, EventArgs e)
        {
            // Set the current display format according to the display format radio button
            // This is the one and only place where the display format radio buttons
            // are evaluated.
            DescriptionIncludesLocation = false;     // These will be restored if descriptionFormat is checked.
            DescriptionIncludesEligibility = false;
            if (descriptionFormat.Checked)
            {
                CurrentDisplayFormat = ProtocolDisplayFormats.Medium;
                DescriptionIncludesLocation = includeLocations.Checked;
                DescriptionIncludesEligibility = includeEligibility.Checked;
            }
            else if (fullDescriptionFormat.Checked)
            {
                CurrentDisplayFormat = ProtocolDisplayFormats.Long;
            }
            // If customFormat was disabled when the page was generated, ASP.Net assumes that
            // that it still is and ignores client-side changes and thus changes.
            // To get around this, we also have to check for the customFormat button in
            // directly in the parameters collection.
            else if (customFormat.Checked || (Request.Params["DisplayFormat"] == "customFormat"))
            {
                CurrentDisplayFormat = ProtocolDisplayFormats.Custom;
            }
            else
            {
                // Default to Title format.
                CurrentDisplayFormat = ProtocolDisplayFormats.Short;
            }

            // If format is now Custom, show the custom format control
            // and hide everything else. The button handler for the
            // custom format control will hide that control and
            // re-enable everything else.
            if (CurrentDisplayFormat == ProtocolDisplayFormats.Custom)
            {
                SetPageRenderingState(PageRenderingState.CustomSelections);
            }
        }

        #endregion // Audience and Display Handling.


        #region Sort Order and Page Size Handling.

        // The sort order and page size options don't go into effect until the user commits to them by clicking
        // the "GO" button.
        // 
        // So instead of evaluating the values contained in the controls directly, the code needs to
        // evaluate the corresponding state properties instead:
        // 
        //     PersistentSortOrder -- The current sort order.
        //     PersistentRecordsPerPage -- The number of records to show on each page.
        //     
        // These properties are updated by UpdateSortOrderAndPageSize_Click().
        // The selections are restored on page load by calling SynchronizeSortOrder() and
        // SynchronizeResultsPerPage().
        // 
        // In addition to this block of code, the sort order is also set on the non-postback
        // path through the OnLoad() method. This accomodates different default sort orders
        // based on search criteria.
        //
        // This code block is the only place where the on-screen controls are updated.


        /// <summary>
        /// Forces the displayed sort order to correspond to its preserved state from previous
        /// page loads.  This value is only updated via UpdateSortOrderAndPageSize_Click (when
        /// the user clicks "Go").
        /// </summary>
        private void SynchronizeSortOrder()
        {
            int integralSortOrder = (int)PersistentSortOrder;
            ListItem item = sortOrder.Items.FindByValue(integralSortOrder.ToString("d"));
            if (item != null)
            {
                sortOrder.ClearSelection();
                item.Selected = true;
            }
        }

        /// <summary>
        /// Forces the displayed results per page to correspond to its preserved state from previous
        /// page loads.  This value is only updated via UpdateSortOrderAndPageSize_Click (when
        /// the user clicks "Go").
        /// </summary>
        private void SynchronizeResultsPerPage()
        {
            ListItem item = resultsPerPage.Items.FindByValue(PersistentRecordsPerPage.ToString("d"));
            if (item != null)
            {
                resultsPerPage.ClearSelection();
                item.Selected = true;
            }
        }

        /// <summary>
        /// Click handler for the "GO" button for the "Sort by" and "results per page" controls.
        /// 
        /// This method is responsible for updating the stateful PersistentSortOrder and
        /// PersistentRecordsPerPage properties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UpdateSortOrderAndPageSize_Click(object sender, EventArgs e)
        {
            // Update Sort Order.
            string selectedValue = sortOrder.SelectedValue;
            PersistentSortOrder = ConvertEnum<CTSSortFilters>.Convert(selectedValue, CTSSortFilters.PhaseDesc);


            // Update Page Size
            // Reset the page number to keep the current set of records visible.
            int newResultsPerPage = Strings.ToInt(resultsPerPage.SelectedValue);
            if (newResultsPerPage < 1)
                newResultsPerPage = 10;

            PersistentRecordsPerPage = newResultsPerPage;

            /// The pager uses page numbers starting with 1, subtracting 1 gives us the offset from
            /// the first page. So Page #1 is 0 pages away from #1; Page #5 is 4 pages away from #1.
            /// and so on.
            /// 
            /// multiplying the *OLD* page offset by the number of records on the page tells us how
            /// far the first record on the page is from the beginning of the entire set of records.
            /// e.g. At 25 records per page, the first record on page 2 has an offset of 25, the
            /// first record on page 3 has an offset of 50.  (This seems strange at first because
            /// the UI says page 3 has records 51 to 75, but that's because the first record in the
            /// UI is #1 instead of #0.)
            /// 
            /// Dividing the offset by the *NEW* number of records yields the page offset of the
            /// page containing the first record of the old page.  Adding 1 turns it back into
            /// the 1-based numbering system used by the pager.

            int oldResultsPerPage = pager.RecordsPerPage;
            int newPageNumber = ((oldResultsPerPage * (pager.CurrentPage - 1)) / newResultsPerPage) + 1;

            pager.RecordsPerPage = newResultsPerPage;
            pager.CurrentPage = newPageNumber;

            // Update page size selection visible to JavaScript.
            pageSize.Value = PersistentRecordsPerPage.ToString("d");
        }

        #endregion
    }
}