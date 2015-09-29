function country_onChange(countrySel) {
	var $stateList = $('#' + ids.state);
	var $stateCheckboxes = $stateList.find('input[type=checkbox]');

	if (countrySel.selectedIndex === 0 || countrySel.selectedIndex === 1) {
		$stateList.removeClass("scrollingListDisabled").addClass('scrollingList');
		$stateCheckboxes.prop('disabled', false);
		$stateCheckboxes.eq(0).prop('checked', true);
	} else {
		$stateList.removeClass('scrollingList').addClass('scrollingListDisabled');
		$stateCheckboxes.prop('checked', false);
		$stateCheckboxes.prop('disabled', true);
	}
}

function doSubmit(webAnalyticsOptions) {
	var $stateBox = $('#' + ids.state);
	var $countryBox = $('#' + ids.country);
	var $stateFullName = $('#' + ids.stateFullName);

	if ($stateBox.prop('selectedIndex') > 0 && $countryBox.prop('selectedIndex') === 0) {
		$countryBox.prop('selectedIndex', 1);
		$countryBox.selectmenu('refresh');
	}

	if ($stateBox.length > 0 && $stateBox.prop('selectedIndex') > 0 && $stateFullName.length > 0) {
		$stateFullName.value = "";

		$stateBox.find(':checked').each(function() {
			if ($stateFullName.value.length > 0) {
				$stateFullName.value += ", ";
			}
			$stateFullName.value += $stateBox.find('label[for="' + this.id + '"]').text();
		});
	}
	// CTAdvancedSearchSubmit($("advSearchForm"));
	//$("advSearchForm").submit();

	// if web analytics are in use, call the Clinical Trials search onclick function
	if (typeof NCIAnalytics !== "undefined")
		NCIAnalytics.CTSearch(webAnalyticsOptions);

	return;
}

function InitializeInstitutionListSubBox() {
	if ($('#' + ids.institutionListExpanded).prop('value') === 'N') {
		$('#' + ids.showInstitutionListButton).show();
		$('#' + ids.institutionListSubBox).hide();
		$('#' + ids.showInstitutionListButton).on('click', function(e) { InstitutionListButtonClickHandler(e); });
	} else {
		showInstitutionList();
	}

	// Allow add button to become visible.
	$('#institutionAddButton').show();
}

function showInstitutionList() {
	$('#'+ids.showInstitutionListButton).hide();
	$('#'+ids.institutionListSubBox).show();
	$('#'+ids.institutionListExpanded).prop('value', 'Y');
}

// When the institution list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function InstitutionListButtonClickHandler(event) {
	dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.institution + '&fld=institution&title=Find+Hospitals/Institutions', 'InstitutionLookup', 'width=750px,menubar=no,resizable=no,location=no,height=650px');
	event.preventDefault();
	event.stopPropagation();
}

function InitializeDrugInterventionBox() {
	// The drug and intervention lists exist as subsections within the
	// overall drug/intevention options.
	InitializeDrugListSubBox();
	InitializeInterventionListSubBox();

	if ($('#'+ids.treatmentTypeAreaExpanded).prop('value') === 'N') {
		$('#showDrugSearchOptionsButton').show();
		$('#interventionArea').hide();
	} else {
		showDrugInterventionOptions();
	}
}

function showDrugInterventionOptions() {
	$('#showDrugSearchOptionsButton').hide();
	$('#interventionArea').show();
	$('#'+ids.treatmentTypeAreaExpanded).prop('value', 'Y');
}

// By default, make the drug list section visible, but collapsed.
// Set up an event handler for button clicks.
function InitializeDrugListSubBox() {
	$('#'+ids.drugListArea).show(); //Make visible.

	if ($('#'+ids.drugListExpanded).prop('value') === 'N') {
		$('#showDrugListButtonArea').show();
		$('#drugListSubBox').hide();
		$('#showDrugListButton').on('click', function(e) { DrugListButtonClickHandler(e); });
	} else {
		showDrugList();
	}

	// Allow add button to become visible.
	$('#druglistAddButton').show();
}

// When the drug list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function DrugListButtonClickHandler(event) {
	dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.drug + '&fld=drug&title=Find+Drug', 'DrugLookup', 'width=750px,menubar=no,resizable=no,location=no,height=650px');
	event.preventDefault();
	event.stopPropagation();
}

function showDrugList() {
	$('#showDrugListButtonArea').hide();
	$('#drugListSubBox').show();
	$('#'+ids.drugListExpanded).prop('value', 'Y');
}

// By default, make the intervention list section visible, but collapsed.
// Set up an event handler for button clicks.
function InitializeInterventionListSubBox() {
	$('#'+ids.interventionListArea).show(); // Make visible

	if ($('#'+ids.interventionListExpanded).prop('value') === 'N') {
		$('#showInterventionListButtonArea').show();
		$('#interventionListSubBox').hide();
		$('#showInterventionListButton').on('click', function(e) { InterventionListButtonClickHandler(e); });
	} else {
		showInterventionList();
	}

	// Allow add button to become visible.
	$('#interventionlistAddButton').show();
}

// When the intervention list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function InterventionListButtonClickHandler(event) {
	dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.intervention + '&fld=intervention&title=Treatment/Intervention', 'InterventionLookup', 'width=750px,menubar=no,resizable=no,location=no,height=650px');
	event.preventDefault();
	event.stopPropagation();
}

function showInterventionList() {
	$('#showInterventionListButtonArea').hide();
	$('#interventionListSubBox').show();
	//$('#'+ids.interventionListExpanded).prop('value', 'Y');
}

function InitializeTrialStatusBox() {
	if ($('#'+ids.trialStatusExpanded).prop('value') === 'N') {
		$('#showTrialStatusSearchOptionsButton').show();
		$('#trialStatusArea').hide();
	} else {
		showTrialStatusSearchOptions();
	}
}

function showTrialStatusSearchOptions() {
	$('#showTrialStatusSearchOptionsButton').hide();
	$('#trialStatusArea').show();
	$('#'+ids.trialStatusExpanded).prop('value', 'Y');
}

function InitializeTrialSponsorBox() {
	// The Investigator and Leading Organzation lists exist as subsections within the
	// overall Trial ID/Sponsor section.
	InitializeInvestigatorListSubBox();
	InitializeLeadOrgListSubBox();

	if ($('#'+ids.trialSponsorExpanded).prop('value') === 'N') {
		$('#showTrialSponsorSearchOptionsButton').show();
		$('#trialSponsorArea, #' +ids.trialInvestigatorsRow + ', #' + ids.trialLeadOrganizationRow).hide();
	} else {
		showTrialSponsorSearchOptions();
	}
}

function InitializeInvestigatorListSubBox() {
	if ($('#'+ids.investigatorListExpanded).prop('value') == 'N') {
		$('#showInvestigatorListButtonArea').show();
		$('#investigatorListSubBox').hide();
		$('#showInvestigatorListButton').on('click', function(e) { InvestigatorListButtonClickHandler(e); });
	} else {
		showInvestigatorList();
	}

	// Allow add button to become visible.
	$('#investigatorListAddButton').show();
}

// When the investigator list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function InvestigatorListButtonClickHandler(event) {
	dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.investigator + '&fld=investigator&title=Find+Trial+Investigators', 'InvestigatorLookup', 'width=750px,menubar=no,resizable=no,location=no,height=650px');
	event.preventDefault();
	event.stopPropagation();
}

function showInvestigatorList() {
	$('#showInvestigatorListButtonArea').hide();
	$('#investigatorListSubBox').show();
	$('#'+ids.investigatorListExpanded).prop('value', 'Y');
}

function InitializeLeadOrgListSubBox() {
	if ($('#'+ids.leadOrgListExpanded).prop('value') === 'N') {
		$('#showLeadOrgListButtonArea').show();
		$('#leadOrgListSubBox').hide();
		$('#showLeadOrgListButton').on('click', function(e) { LeadOrgListButtonClickHandler(e); });
	} else {
		showLeadOrgList();
	}

	// Allow add button to become visible.
	$('#leadOrgAddButton').show();
}

// When the leading organization list button is clicked, pop up the selection dialog.
// The list is only expanded if values are selected.
function LeadOrgListButtonClickHandler(event) {
	dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=' + ids.leadOrg + '&fld=leadOrg&title=Find+Lead+Organizations', 'LeadOrgLookup', 'width=700px,menubar=no,resizable=no,location=no,height=650px');
	event.preventDefault();
	event.stopPropagation();
}

function showLeadOrgList() {
	$('#showLeadOrgListButtonArea').hide();
	$('#leadOrgListSubBox').show();
	$('#'+ids.leadOrgListExpanded).prop('value', 'Y');
}

function showTrialSponsorSearchOptions() {
	$('#showTrialSponsorSearchOptionsButton').hide();
	$('#trialSponsorArea, #' + ids.trialInvestigatorsRow + ', #' + ids.trialLeadOrganizationRow).show();
	$('#'+ids.trialSponsorExpanded).prop('value', 'Y');
}
