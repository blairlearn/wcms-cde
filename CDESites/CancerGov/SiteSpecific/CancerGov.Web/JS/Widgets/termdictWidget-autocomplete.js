// JavaScript Document

jQuery(document).ready(function($) {

    var language = "English";

    if ($('meta[name="content-language"]').attr("content") == "es") {
        language = "Spanish"
    }

    var $keywordElem = $("#search")

    if ($keywordElem.length == 0)
        return;

	$keywordElem.autocomplete({
	    //source: "/AutoSuggestSearch.svc/SearchJSON/" + language,
	    source: "/TermDictionary.svc/SuggestJSON/" + language,
		
		//source: "/PublishedContent/js/data.html" ,
		
	    minLength: 3,
	    focus: function(event, ui) {
		$("#search").val(ui.item.item);
		return false;
	    },
	    select: function(event, ui) {
		$("#search").val(ui.item.item);
		return false;
	    }
	}).data("autocomplete")._renderItem = function(ul, item) {
	    //Escape bad characters
	    var lterm = this.term.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
	    

	    //This should find any words which begin with the text from item.
	    var regex = new RegExp("(^" + lterm + "|\\s+" + lterm + "i)","i");

	    var word = item.item.replace(regex, "<strong>$&</strong>");

	    return $("<li></li>")
				.data("item.autocomplete", item)
				.append("<a>" + word + "</a>")
				.appendTo(ul);
	};
	
    $keywordElem.keyup(function(event) {	
       if ( event.which == 13 ) {
          event.preventDefault();
          $(".ui-autocomplete").css({"display":"none"});
          $('#Go').focus();
	      doSearch(null);
       }       
    });


}
	);

