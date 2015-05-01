function checkAll_ClickHandler(e) {
    topCheckbox = $("#" + ids.checkAllTop)[0];
    bottomCheckbox = $("#" +ids.checkAllBottom)[0];
    // var blnChecked = e.target.checked;
    var blnChecked = e.checked;

    if ( $('#' + ids.advResultForm + " input[name='cdrid']").length == 0) {
        alert("There are no trials to display.");
        topCheckbox.checked = false;
        bottomCheckbox.checked = false;
        return;
    }

    var protocolsPerPage = $("#" + ids.pageSize).val();
    topCheckbox.checked = blnChecked;
    bottomCheckbox.checked = blnChecked;

    for (i = 0; i <= protocolsPerPage; i++) {
        // Primary checkboxes
        if ($('#' + ids.advResultForm + " input[name='cdrid']")[i] != null) {
            $('#' + ids.advResultForm + " input[name='cdrid']")[i].checked = blnChecked;
        }
        else {
            if (i == 0 && $('#' + ids.advResultForm + " input[name='cdrid']") != null) {
                $('#' + ids.advResultForm + " input[name='cdrid']").checked = blnChecked;
            }
        }
        // Mirror checkboxes
        if ($('#' + ids.advResultForm + " input[name='cdrid_mirror']") != null) {
            if ($('#' + ids.advResultForm + " input[name='cdrid_mirror']")[i] != null) {
                $('#' + ids.advResultForm + " input[name='cdrid_mirror']")[i].checked = blnChecked;
            }
            else {
                if (i == 0 && $('#' + ids.advResultForm + " input[name='cdrid_mirror']") != null) {
                    $('#' + ids.advResultForm + " input[name='cdrid_mirror']").checked = blnChecked;
                }
            }
        }
    }
}

// Handler for the "Display for Print" button
function submitPrint_ClickHandler(event) {
    if ($("#" + ids.OffPageSelectionsExist).val() != "Y" && !SelectionsExistOnPage()) {
        alert("You must select one or more clinical trials to display for print.");
        Event.stop(event);
    }
}

// Scans the list of trial checkboxes to determine whether any are checked.
function SelectionsExistOnPage() {
    var selectionsExist = false;

    if ($('#' + ids.advResultForm + " input[name='cdrid']:checked").length > 0) {
        selectionsExist = true;
    }

    return selectionsExist;
}

// Sets up individual event handlers for clicks on BoxA to be reflected on BoxB and vice-versa.
// The two checkboxes are passed to the event handler as a source/mirror pair in an immediate object.
function CreateCheckboxMirror(boxA, boxB) {
    $(boxA).click(function(event) { 
        UpdateCheckboxReflection(boxA, boxB);
    });
    $(boxB).click(function(event) {
        UpdateCheckboxReflection(boxB, boxA);
    });
}

function UpdateCheckboxReflection(src, mirror) {
    var sourceBox = $(src);
    var mirrorBox = $(mirror);
    mirrorBox.prop('checked', sourceBox.prop('checked'));
}

// Click handler for change of audience.
function AudienceType_clickhandler(e) {
    var customButton = $('#' + ids.customFormat)[0];

    // Changing to health professional, all formats are available.
    if (e.target.value == "healthProfAudience") {
        customButton.disabled = false;
        $(customButton).parent().disabled = false;   // Required w/ IE for the ASP-generated SPAN tag
        $(customButton).parent().removeClass("gray-text").addClass("black-text");
    }
    else {
        // Changing to Patient, disable custom format.
        var cancelChange = false;

        // If the custom button was previously checked, confirm that it's OK to change
        // the selection type.
        if (customButton.checked == true) {
            cancelChange = confirm("The Custom Display option cannot be used to view patient-oriented clinical trial results.\nThe Custom Display uses information that is available only in clinical trial descriptions written for health professionals.");
            if (cancelChange) {
                $('#' + ids.titleFormat)[0].checked = true;
                customButton.disabled = true;
                $(customButton).parent().removeClass("black-text").addClass("gray-text");
            }
            else {
                $('#' + ids.healthProfAudience).prop('checked', true).focus();
            }
        }
        else {
            // The custom button wasn't previously checked, disable it.
            customButton.disabled = true;
            $(customButton).parent().removeClass("black-text").addClass("gray-text");
        }
    }
}

// Click handler for format buttons.  Clicking "Description with" should autocheck
// location and eligibility.  Clicking the other formats should clear them.
function FormatType_clickhandler(e) {
    var locations = $('#' + ids.includeLocations)[0];
    var eligibility = $('#' + ids.includeEligibility)[0];

    locations.checked = (e.target.value === "descriptionFormat");
    eligibility.checked = (e.target.value === "descriptionFormat");
}

// Handler for clicks on the Locations or Eligibility check boxes.
function DescriptionSubtype_clickhandler(e) {
    if (e.target.checked)
        $('#' + ids.descriptionFormat).prop('checked', true);
}

// Controls the collapse and expansion of the search criteria.
// SetSearchCriteriaDisplay() is called on page load, and again each
// time the display is toggled.
function SetSearchCriteriaDisplay() {
    if ($('#'+ids.DisplaySearchCriteriaCollapsed).prop('value') === "N") {
        $('#'+ids.CriteriaDisplay).hide();
        $("#hideCriteriaLink").hide();
        $("#showCriteriaLink").show();
    } else {
        $('#'+ids.CriteriaDisplay).show();
        $("#hideCriteriaLink").show();
        $("#showCriteriaLink").hide();
    }
}
function toggleSearchCriteria() {
    var settingField = $('#'+ids.DisplaySearchCriteriaCollapsed);
    settingField.prop('value', (settingField.prop('value') === "N") ? "Y" : "N");
    SetSearchCriteriaDisplay();
}

// Protocol titles are rendered wrapped in both an <a> and a <label> tag.  On some browsers
// (at the moment, Firefox), this results in the individual checkbox becoming selected when
// the user clicks the title to view the abstract.  This set of functions is used to
// prevent that from occuring.
function SetupTitleClickHandler() {
    // The protocol-abstract-link class is only applied to the protocol links.
    $(".protocol-abstract-link").click(function(event) {
        ProtocolTitleClickHandler(event);
    });
}

// Click handler for protocol title links.
// Cancel the event, change the window location directly.
function ProtocolTitleClickHandler(event) {
    event.stopImmediatePropagation();
    window.location = this.href;
}
