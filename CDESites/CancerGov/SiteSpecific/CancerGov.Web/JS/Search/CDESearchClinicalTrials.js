function country_onChange(countrySel) {
    var stateCheckboxes = $("locationStateList").select("input[type=checkbox]");

    if (countrySel.selectedIndex == 0 || countrySel.selectedIndex == 1) {
        $("locationStateList").removeClassName("scrollingListDisabled").addClassName("scrollingList");
        stateCheckboxes.invoke("enable");
        stateCheckboxes[0].checked = true;
    }
    else {
        $("locationStateList").removeClassName("scrollingList").addClassName("scrollingListDisabled");
        stateCheckboxes.invoke("disable");
        var len = stateCheckboxes.length;
        for (var i = 0; i < len; ++i)
            stateCheckboxes[i].checked = false;
    }
}

function doSubmit(webAnalyticsOptions) {
    var stateBox = $(ids.state);
    var stateFullName = $("stateFullName");

    if (stateBox.selectedIndex > 0 && document.advSearchForm.country.selectedIndex == 0) {
        $(ids.country).selectedIndex = 1;
    }

    if (stateBox != null && stateBox.selectedIndex > 0 && stateFullName != null) {
        stateFullName.value = "";

        for (i = 0; i < stateBox.options.length; i++) {
            if (stateBox.options[i].selected) {
                if (stateFullName.value.length > 0) {
                    stateFullName.value += ", ";
                }
                stateFullName.value += stateBox.options[i].text;
            }
        }
    }
    // CTAdvancedSearchSubmit($("advSearchForm"));
    //$("advSearchForm").submit();

    // if web analytics are in use, call the Clinical Trials search onclick function
    if (typeof (NCIAnalytics) != "undefined")
        NCIAnalytics.CTSearch(webAnalyticsOptions);

    return;
}

function InitializeLocationBox() {

    // Reveal the dynamic location radio buttons.
    // Hide the buttons for the static display.
    $("LocationChooser").show();
    $(ids.zipCodeLocationButtonStatic, ids.cityStateLocationButtonStatic, ids.hospitalLocationButtonStatic, ids.atNihLocationButtonStatic).invoke("hide");

    // Hidden field to contain location selection.  Updated when the selection changes.
    // This is needed when browser back-arrow events cause the page script to be
    // re-executed.
    var lastKnownLocation = $F(ids.LocationSelection);

    // Change from the buttons used for non-JavaScript to JavaScript enabled.
    if ($(ids.zipCodeLocationButtonStatic).checked || $(ids.zipCodeLocationButton).checked ||
            lastKnownLocation == "zip") {
        $(ids.zipCodeLocationButtonStatic).checked = false;
        $(ids.zipCodeLocationButton).checked = true;
    } else if ($(ids.cityStateLocationButtonStatic).checked || $(ids.cityStateLocationButton).checked ||
            lastKnownLocation == "city") {
        $(ids.cityStateLocationButtonStatic).checked = false;
        $(ids.cityStateLocationButton).checked = true;
    } else if ($(ids.hospitalLocationButtonStatic).checked || $(ids.hospitalLocationButton).checked ||
            lastKnownLocation == "hospital") {
        $(ids.hospitalLocationButtonStatic).checked = false;
        $(ids.hospitalLocationButton).checked = true;
    } else if ($(ids.atNihLocationButtonStatic).checked || $(ids.atNihLocationButton).checked ||
            lastKnownLocation == "nih") {
        $(ids.atNihLocationButtonStatic).checked = false;
        $(ids.atNihLocationButton).checked = true;
    } else {
        // Nothing was checked, so we'll force ZIP.
        $(ids.zipCodeLocationButton).checked = true;
        $(ids.LocationSelection).value = "zip";
    }

    // Tweak country/state display.
    var countrySel = $(ids.country);
    var stateCheckboxes = $("locationStateList").select("input[type=checkbox]");
    if (countrySel.selectedIndex == 0 || countrySel.selectedIndex == 1) {
        $("locationStateList").removeClassName("scrollingListDisabled").addClassName("scrollingList");
        stateCheckboxes.invoke("enable");
    }
    else {
        $("locationStateList").removeClassName("scrollingList").addClassName("scrollingListDisabled");
        stateCheckboxes.invoke("disable");
    }

    // Show only the current location box.
    $("zipCodeBox", "cityStateZipBox", ids.hospitalBox, "nihOnlyBox").invoke("hide");
    var chooser = $("LocationChooser");
    var button = chooser.select("input[type=radio]:checked");
    switch (button[0].getValue()) {
        case "zip":
        default:
            $("zipCodeBox").show();
            break;
        case "hospital":
            $(ids.hospitalBox).show();
            break;
        case "city":
            $("cityStateZipBox").show();
            break;
        case "nih":
            $("nihOnlyBox").show();
            break;
    }

    Event.observe(ids.zipCodeLocationButton, "click", UpdateLocationDisplay.bindAsEventListener($("zipCodeBox")));
    Event.observe(ids.hospitalLocationButton, "click", UpdateLocationDisplay.bindAsEventListener($(ids.hospitalBox)));
    Event.observe(ids.cityStateLocationButton, "click", UpdateLocationDisplay.bindAsEventListener($("cityStateZipBox")));
    Event.observe(ids.atNihLocationButton, "click", UpdateLocationDisplay.bindAsEventListener($("nihOnlyBox")));

    InitializeInstitutionListSubBox();
}

function UpdateLocationDisplay(event) {
    $("zipCodeBox", ids.hospitalBox, "cityStateZipBox", "nihOnlyBox").invoke("hide");

    // Preserve selection in case back-arrow causes the page's scripts to re-run.
    if (event.srcElement != null)
        $(ids.LocationSelection).value = event.srcElement.value;

    this.show();
}

function InitializeInstitutionListSubBox() {
    if ($(ids.institutionListExpanded).value == 'N') {
        $(ids.showInstitutionListButton).show();
        $(ids.institutionListSubBox).hide();
        Event.observe(ids.showInstitutionListButton, "click", InstitutionListButtonClickHandler.bindAsEventListener($(ids.showInstitutionListButton)));
    } else {
        showInstitutionList();
    }

    // Allow add button to become visible.
    $("institutionAddButton").show();
}

function showInstitutionList() {
    $(ids.showInstitutionListButton).hide();
    $(ids.institutionListSubBox).show();
    $(ids.institutionListExpanded).value = 'Y';
}

// When the institution list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function InstitutionListButtonClickHandler(event) {
    dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.institution + '&fld=institution&title=Find+Hospitals/Institutions', 'InstitutionLookup', 'width=621px,menubar=no,resizable=no,location=no,height=580px');
    Event.stop(event);
}

function InitializeDrugInterventionBox() {
    // The drug and intervention lists exist as subsections within the
    // overall drug/intevention options.
    InitializeDrugListSubBox();
    InitializeInterventionListSubBox();

    if ($(ids.treatmentTypeAreaExpanded).value == 'N') {
        $("showDrugSearchOptionsButton").show();
        $("interventionTable").hide();
    } else {
        showDrugInterventionOptions();
    }
}

function showDrugInterventionOptions() {
    $("showDrugSearchOptionsButton").hide();
    $("interventionTable").show();
    $(ids.treatmentTypeAreaExpanded).value = 'Y';
}

// By default, make the drug list section visible, but collapsed.
// Set up an event handler for button clicks.
function InitializeDrugListSubBox() {
    $(ids.drugListArea).show(); //Make visible.

    if ($(ids.drugListExpanded).value == 'N') {
        $("showDrugListButtonArea").show();
        $("drugListSubBox").hide();
        Event.observe("showDrugListButton", "click", DrugListButtonClickHandler.bindAsEventListener($("showDrugListButton")));
    } else {
        showDrugList();
    }

    // Allow add button to become visible.
    $("druglistAddButton").show();
}

// When the drug list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function DrugListButtonClickHandler(event) {
    dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.drug + '&fld=drug&title=Find+Drug', 'DrugLookup', 'width=621px,menubar=no,resizable=no,location=no,height=580px');
    Event.stop(event);
}

function showDrugList() {
    $("showDrugListButtonArea").hide();
    $("drugListSubBox").show();
    $(ids.drugListExpanded).value = 'Y';
}

// By default, make the intervention list section visible, but collapsed.
// Set up an event handler for button clicks.
function InitializeInterventionListSubBox() {
    $(ids.interventionListArea).show();  // Make visible

    if ($F(ids.interventionListExpanded) == 'N') {
        $("showInterventionListButtonArea").show();
        $("interventionListSubBox").hide();
        Event.observe("showInterventionListButton", "click", InterventionListButtonClickHandler.bindAsEventListener($("showInterventionListButton")));
    } else {
        showInterventionList();
    }

    // Allow add button to become visible.
    $("interventionlistAddButton").show();
}

// When the intervention list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function InterventionListButtonClickHandler(event) {
    dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.intervention + '&fld=intervention&title=Treatment/Intervention', 'InterventionLookup', 'width=621px,menubar=no,resizable=no,location=no,height=580px');
    Event.stop(event);
}

function showInterventionList() {
    $("showInterventionListButtonArea").hide();
    $("interventionListSubBox").show();
    $(ids.interventionListExpanded).value = 'Y';
}

function InitializeTrialStatusBox() {
    if ($(ids.trialStatusExpanded).value == 'N') {
        $("showTrialStatusSearchOptionsButton").show();
        $("trialStatusTable").hide();
    } else {
        showTrialStatusSearchOptions();
    }
}

function showTrialStatusSearchOptions() {
    $("showTrialStatusSearchOptionsButton").hide();
    $("trialStatusTable").show();
    $(ids.trialStatusExpanded).value = 'Y';
}

function InitializeTrialSponsorBox() {
    // The Investigator and Leading Organzation lists exist as subsections within the
    // overall Trial ID/Sponsor section.
    InitializeInvestigatorListSubBox();
    InitializeLeadOrgListSubBox();

    if ($F(ids.trialSponsorExpanded) == 'N') {
        $("showTrialSponsorSearchOptionsButton").show();
        $("trialSponsorTable", ids.trialInvestigatorsRow, ids.trialLeadOrganizationRow).invoke("hide");
    } else {
        showTrialSponsorSearchOptions();
    }
}

function InitializeInvestigatorListSubBox() {
    if ($F(ids.investigatorListExpanded) == 'N') {
        $("showInvestigatorListButtonArea").show();
        $("investigatorListSubBox").hide();
        Event.observe("showInvestigatorListButton", "click", InvestigatorListButtonClickHandler.bindAsEventListener($("showInvestigatorListButton")));
    } else {
        showInvestigatorList();
    }

    // Allow add button to become visible.
    $("investigatorListAddButton").show();
}

// When the investigator list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function InvestigatorListButtonClickHandler(event) {
    dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.investigator + '&fld=investigator&title=Find+Trial+Investigators', 'InvestigatorLookup', 'width=621px,menubar=no,resizable=no,location=no,height=580px');
    Event.stop(event);
}

function showInvestigatorList() {
    $("showInvestigatorListButtonArea").hide();
    $("investigatorListSubBox").show();
    $(ids.investigatorListExpanded).value = 'Y';
}

function InitializeLeadOrgListSubBox() {
    if ($F(ids.leadOrgListExpanded) == 'N') {
        $("showLeadOrgListButtonArea").show();
        $("leadOrgListSubBox").hide();
        Event.observe("showLeadOrgListButton", "click", LeadOrgListButtonClickHandler.bindAsEventListener($("showLeadOrgListButton")));
    } else {
        showLeadOrgList();
    }

    // Allow add button to become visible.
    $("leadOrgAddButton").show();
}

// When the leading organization list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function LeadOrgListButtonClickHandler(event) {
    dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.leadOrg + '&fld=leadOrg&title=Find+Lead+Organizations', 'LeadOrgLookup', 'width=650px,menubar=no,resizable=no,location=no,height=580px');
    Event.stop(event);
}

function showLeadOrgList() {
    $("showLeadOrgListButtonArea").hide();
    $("leadOrgListSubBox").show();
    $(ids.leadOrgListExpanded).value = 'Y';
}

function showTrialSponsorSearchOptions() {
    $("showTrialSponsorSearchOptionsButton").hide();
    $("trialSponsorTable", ids.trialInvestigatorsRow, ids.trialLeadOrganizationRow).invoke("show");
    $(ids.trialSponsorExpanded).value = 'Y';
}
