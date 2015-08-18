// function used by AutoComplete to submit to server when user
// selects an item
function ACOnSubmit() {
    $("#btnGo").click();
}

//Hookup JPlayer for Audio
if (jQuery.jPlayer && !Modernizr.touch) {
    jQuery(document).ready(function($) {
        var my_jPlayer = $("#dictionary_jPlayer");

        my_jPlayer.jPlayer({
            swfPath: "/PublishedContent/files/global/flash/", //Path to SWF File Used by jPlayer
            //errorAlerts: true,
            supplied: "mp3" //The types of files which will be used.
        });

        //Attach a click event to the audio link

        $(".CDR_audiofile").click(function() {
            my_jPlayer.jPlayer("setMedia", {
                mp3: $(this).attr("href") // Defines the m4v url
            }).jPlayer("play");

            return false;
        });
    });
}


$(document).ready(function() {
    autoFunc();
});

function autoFunc() {
    var language = "English";
    if ($("html").attr("lang") === "es")
        language = "Spanish";

    var $keywordElem = document.getElementById('AutoComplete1'); //$("#AutoComplete1").val();

    alert("test:" + $keywordElem);
    
    alert($('#'+ AutoComplete1 ).val());
    
    var isContains = IsContains();
    var svcUrl = "";
    if (isContains)
        svcUrl = "/TermDictionary.svc/SearchJSON/" + language + "?contains=true";
    else
        svcUrl = "/TermDictionary.svc/SearchJSON/" + language;

    NCI.doAutocomplete("#" + $("#AutoComplete1"), svcUrl, isContains, "searchTerm", { maxRows: 10 });
}

function IsContains() {
    var ret = false;

    if ($("#radioContains").prop("checked"))
        ret = true;

    return ret;
}
