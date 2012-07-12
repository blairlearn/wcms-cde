// JavaScript for Genetics Term Dictionary

//Hookup JPlayer for Audio
if (jQuery.jPlayer) {
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
      var isContains=false;
      if($("#radioContains").attr("checked")!= "undefined")
         if($("#radioContains").attr("checked"))
           isContains=true;
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

    if ($('meta[name="content-language"]').attr("content") == "es") {
        language = "Spanish"
    }

    var $keywordElem = $("#searchString")

    if ($keywordElem.length == 0)
        return;
        

    var isContains = IsContains() 
    var svcUrl = "";
    if (IsContains())  
        svcUrl = "/TermDictionary.svc/SuggestGeneticsContainsJSON/";
    else
        svcUrl = "/TermDictionary.svc/SuggestGeneticsStartsJSON/";

	$keywordElem.autocomplete({
        
        // Set AJAX service source 
	    source: svcUrl + language,

        // Start autocomplete only after three characters are typed 
	    minLength: 3,
	    
	    focus: function(event, ui) {
		$("#searchString").val(ui.item.item);
		return false;
	    },
	    select: function(event, ui) {
		$("#searchString").val(ui.item.item);
		return false;
	    }
	}).data("autocomplete")._renderItem = function(ul, item) {
	    //Escape bad characters
	    var lterm = this.term.replace(/[-[\]{}()*+?.,\^$|#\s]/g, "\$&");
	    
	    if(isContains)
	        // highlight autocomplete item if it appears anywhere 
	        var regexBold = new RegExp("(" + lterm + "|\s+" + lterm + "i)","i");
	    else
	        // hightlight autocomplete item if it appears at the beginning
            var regexBold = new RegExp("(^" + lterm + "|\\s+" + lterm + ")");

	    var word = item.item.replace(regexBold, "<strong>$&</strong>");

	    return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a onclick=\"SelectIt();\">" + word + "</a>")
				.appendTo(ul);
	};

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
    
    if($("#radioContains").attr("checked")!= "undefined")
      if($("#radioContains").attr("checked"))
         ret = true;

    return ret; 
}
