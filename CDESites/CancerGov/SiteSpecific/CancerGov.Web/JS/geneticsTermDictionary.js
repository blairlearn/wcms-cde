// JavaScript for Genetics Term Dictionary
 
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

function DoSearch()
{
   if($('#searchString').val() != "") {
      var localSearhString = htmlEscape($('#searchString').val());
      var isContains = IsContains();
      if(isContains) {
         var url = $('#litPageUrl').text() + "?search=" + localSearhString + "&contains=true";
         NCIAnalytics.GeneticsDictionarySearch(this,localSearhString,true);
      } else {
         var url = $('#litPageUrl').text() + "?search=" + localSearhString;
         NCIAnalytics.GeneticsDictionarySearch(this,localSearhString,false);
      }
      $(location).attr('href',url);
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
    var language = "English";

    if ($('html').attr("lang") === "es") {
        language = "Spanish"
    }

    var $keywordElem = $("#searchString")

    if ($keywordElem.length === 0)
        return;


    var isContains = IsContains();
    var svcUrl = "";
    if (isContains)  
        svcUrl = "/TermDictionary.svc/SuggestGeneticsContainsJSON/";
    else
        svcUrl = "/TermDictionary.svc/SuggestGeneticsStartsJSON/";

    svcUrl += language;
    NCI.doAutocomplete("#searchString", svcUrl, isContains);

    $keywordElem.keyup(function(event) {
       if ( event.which == 13 ) {
          event.preventDefault();
	      DoSearch();
       }
    });
}

function SelectIt()  {
    $("#btnGo").click()
}

function IsContains() {
    var ret = false;
    
    if($("#radioContains").prop("checked"))
         ret = true;

    return ret; 
}
