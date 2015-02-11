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
    if (IsContains())
        svcUrl = "/DrugDictionary.svc/SearchJSON?contains=true&maxRows=10";
    else
        svcUrl = "/DrugDictionary.svc/SearchJSON?maxRows=10";

    $keywordElem.autocomplete({

        // Set AJAX service source
    source: function(request, response) {
            $.getJSON(svcUrl, { searchTerm: request.term },response);
        },



        // Start autocomplete only after three characters are typed 
        minLength: 3,

        focus: function(event, ui) {
            $("#" + ids.AutoComplete1).val(ui.item.item);
            return false;
        },
        select: function(event, ui) {
            $("#" + ids.AutoComplete1).val(ui.item.item);
            return false;
        }
    }).data("ui-autocomplete")._renderItem = function(ul, item) {
        //Escape bad characters
        var lterm = this.term.replace(/[-[\]{}()*+?.,\^$|#\s]/g, "\$&");
        console.log(lterm);
        if (isContains)
        // highlight autocomplete item if it appears anywhere 
            var regexBold = new RegExp("(" + lterm + "|\s+" + lterm + "i)", "i");
        else
        // hightlight autocomplete item if it appears at the beginning
            var regexBold = new RegExp("(^" + lterm + "|\\s+" + lterm + ")");
        console.log(regexBold);
        var word = item.item.replace(regexBold, "<strong>$&</strong>");
        console.log(word);
        return $("<li></li>")
				.data("ui-autocomplete-item", item)
				.append("<a onclick=\"SelectIt();\">" + word + "</a>")
				.appendTo(ul);
    };

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
