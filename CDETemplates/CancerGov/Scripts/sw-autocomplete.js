// JavaScript Document
jQuery(document).ready(function($) {
	
		//function log( message ) {
		//	$( "<div/>" ).text( message ).prependTo( "#log" );
		//	$( "#log" ).attr( "scrollTop", 0 );
		//}
		
		var language = "English";
		
		if ($( 'meta[name="content-language"]' ).attr("content") == "es") {
			language = "Spanish"
		}
		
		var keywordElem = $("#swKeyword")
		
		if(keywordElem !== null)	
			{
			keywordElem.autocomplete({
			source: "/AutoSuggestSearch.svc/SearchJSON/" + language,
			minLength: 3,
			focus: function( event, ui ) {
				$( "#swKeyword" ).val( ui.item.item );
				return false;
			},
			select: function( event, ui ) {
				$( "#swKeyword" ).val( ui.item.item );
				return false;
			}			
		}).data( "autocomplete" )._renderItem = function( ul, item ) {
			//Escape bad characters
			var lterm = this.term.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
			
			//This should find any words which begin with the text from item.			
			var regex = new RegExp("(^" + lterm + "|\\s+" + lterm + ")");
			
			var word = item.item.replace(regex, "<strong>$&</strong>");
			
			return $( "<li></li>" )
				.data( "item.autocomplete", item )
				.append( "<a>" + word + "</a>" )
				.appendTo( ul );
		};	
			}
	}
	);