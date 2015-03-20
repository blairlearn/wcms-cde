function checkAll_ClickHandler(e) {
    topCheckbox = $(ids.checkAllTop);
    bottomCheckbox = $(ids.checkAllBottom);
    // var blnChecked = e.target.checked;
    var blnChecked = e.checked;

    if ( $(ids.advResultForm).cdrid == null) {
        alert("There are no trials to display.");
        topCheckbox.checked = false;
        bottomCheckbox.checked = false;
        return;
    }

    var protocolsPerPage = $F(ids.pageSize);
    topCheckbox.checked = blnChecked;
    bottomCheckbox.checked = blnChecked;

    for (i = 0; i <= protocolsPerPage; i++) {
        // Primary checkboxes
        if ($(ids.advResultForm).cdrid[i] != null) {
            $(ids.advResultForm).cdrid[i].checked = blnChecked;
        }
        else {
            if (i == 0 && $(ids.advResultForm).cdrid != null) {
                $(ids.advResultForm).cdrid.checked = blnChecked;
            }
        }
        // Mirror checkboxes
        if ($(ids.advResultForm).cdrid_mirror != null) {
            if ($(ids.advResultForm).cdrid_mirror[i] != null) {
                $(ids.advResultForm).cdrid_mirror[i].checked = blnChecked;
            }
            else {
                if (i == 0 && $(ids.advResultForm).cdrid_mirror != null) {
                    $(ids.advResultForm).cdrid_mirror.checked = blnChecked;
                }
            }
        }
    }
}

// Handler for the "Display for Print" button
function submitPrint_ClickHandler(event) {
    if ($F(ids.OffPageSelectionsExist) != "Y" && !SelectionsExistOnPage()) {
        alert("You must select one or more clinical trials to display for print.");
        Event.stop(event);
    }
}

// Scans the list of trial checkboxes to determine whether any are checked.
function SelectionsExistOnPage() {
    var selectionsExist = false;

    if ($(ids.advResultForm).cdrid != null) {

        if ($(ids.advResultForm).cdrid[0] == null)	// single checkbox, not an array
        {
            if ($(ids.advResultForm).cdrid.checked == true) {
                selectionsExist = true;
            }
        }
        else	//case: checkbox array
        {
            var protocolsPerPage = $F(ids.pageSize);
            for (i = 0; i <= protocolsPerPage; i++) {
                if ($(ids.advResultForm).cdrid[i] != null) {
                    if ($(ids.advResultForm).cdrid[i].checked == true) {
                        selectionsExist = true;
                        break;
                    }
                }
            }
        }
    }

    return selectionsExist;
}

// Sets up individual event handlers for clicks on BoxA to be reflected on BoxB and vice-versa.
// The two checkboxes are passed to the event handler as a source/mirror pair in an immediate object.
function CreateCheckboxMirror(boxA, boxB) {
    Event.observe(boxA, "click", UpdateCheckboxReflection.bindAsEventListener({ src: $(boxA), mirror: $(boxB) }));
    Event.observe(boxB, "click", UpdateCheckboxReflection.bindAsEventListener({ src: $(boxB), mirror: $(boxA) }));
}

function UpdateCheckboxReflection(event) {
    var src = this.src;
    var mirror = this.mirror;
    mirror.checked = src.checked;
}

// Click handler for change of audience.
function AudienceType_clickhandler(e) {
    var customButton = $(ids.customFormat);

    // Changing to health professional, all formats are available.
    if (e.target.value == "healthProfAudience") {
        customButton.disabled = false;
        customButton.up().disabled = false;   // Required w/ IE for the ASP-generated SPAN tag
        customButton.up().removeClassName("gray-text");
        customButton.up().addClassName("black-text");
    }
    else {
        // Changing to Patient, disable custom format.
        var cancelChange = false;

        // If the custom button was previously checked, confirm that it's OK to change
        // the selection type.
        if (customButton.checked == true) {
            cancelChange = confirm("The Custom Display option cannot be used to view patient-oriented clinical trial results.\nThe Custom Display uses information that is available only in clinical trial descriptions written for health professionals.");
            if (cancelChange) {
                $(ids.titleFormat).checked = true;
                customButton.disabled = true;
                customButton.parent().removeClass("black-text").addClass("gray-text");
            }
            else {
                $(ids.healthProfAudience).prop('checked', true).focus();
            }
        }
        else {
            // The custom button wasn't previously checked, disable it.
            customButton.disabled = true;
            customButton.parent().removeClassName("black-text").addClassName("gray-text");
        }
    }
}

// Click handler for format buttons.  Clicking "Description with" should autocheck
// location and eligibility.  Clicking the other formats should clear them.
function FormatType_clickhandler(e) {
    var locations = $(ids.includeLocations);
    var eligibility = $(ids.includeEligibility);

    locations.checked = (e.target.value === "descriptionFormat");
    eligibility.checked = (e.target.value === "descriptionFormat");
}

// Handler for clicks on the Locations or Eligibility check boxes.
function DescriptionSubtype_clickhandler(e) {
    if (e.target.checked)
        $(ids.descriptionFormat).prop('checked', true);
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
    $(".protocol-abstract-link").each(function(item) {
        Event.observe(item, "click", ProtocolTitleClickHandler.bindAsEventListener(item));
    });
}

// Click handler for protocol title links.
// Cancel the event, change the window location directly.
function ProtocolTitleClickHandler(event) {
    Event.stop(event);
    window.location = this.href;
}
