// JavaScript for Drug Dictionary



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
