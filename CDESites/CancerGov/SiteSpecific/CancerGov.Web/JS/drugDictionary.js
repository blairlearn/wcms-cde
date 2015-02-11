// JavaScript for Drug Dictionary

function DoSearch() {
    if ($('#AutoComplete1').val() != "") {
        var localSearhString = htmlEscape($('#AutoComplete1').val());
        var isContains = IsContains();
        if (isContains) {
            var url = $('#litPageUrl').text() + "?search=" + localSearhString + "&contains=true";
            NCIAnalytics.GeneticsDictionarySearch(this, localSearhString, true);
        } else {
            var url = $('#litPageUrl').text() + "?search=" + localSearhString;
            NCIAnalytics.GeneticsDictionarySearch(this, localSearhString, false);
        }
        $(location).attr('href', url);
    }
}

function htmlEscape(str) {
    return String(str)
    .replace(/&/g, '&amp;')
    .replace(/"/g, '&quot;')
    .replace(/'/g, '&#39;')
    .replace(/[(]/g, '&#28;')
    .replace(/[)]/g, '&#29;')
    .replace(/[?]/g, '&#3f;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;');
}

// AutoComplete stuff
jQuery(document).ready(function($) {

    autoFunc();

});

//AutoComplete function 
function autoFunc() {

    var $keywordElem = $("#"+ids.AutoComplete1);

    if ($keywordElem.length == 0)
        return;


    var isContains = IsContains();
    var svcUrl = "";
    if (isContains)
        svcUrl = "/DrugDictionary.svc/SearchJSON?contains=true";
    else
        svcUrl = "/DrugDictionary.svc/SearchJSON";

    NCI.doAutocomplete("#" + ids.AutoComplete1, svcUrl, isContains, "searchTerm", { maxRows: 10 });

    $keywordElem.keyup(function(event) {
        if (event.which == 13) {
            event.preventDefault();
            DoSearch();
        }
    });
}

function SelectIt() {
    $("#btnGo").click()
}

function IsContains() {
    var ret = false;

    if ($("#"+ids.radioContains).prop("checked"))
        ret = true;

    return ret;
}
