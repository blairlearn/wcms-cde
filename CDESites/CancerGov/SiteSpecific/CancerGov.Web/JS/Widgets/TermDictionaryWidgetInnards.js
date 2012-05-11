
function loadResults(searchTerm) {
    var language;
    var xmlhttp;
    
    searchTerm = searchTerm.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, "\\$&");
    
    if (window.XMLHttpRequest)  {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp=new XMLHttpRequest();
    }
    else  {
        // code for IE6, IE5
        xmlhttp=new ActiveXObject('Microsoft.XMLHTTP');
    }

    xmlhttp.onreadystatechange=function() {
        if (xmlhttp.readyState==4 && xmlhttp.status==200)  {
            var AJAXReturned = xmlhttp.responseText;
            var someJSON = eval(AJAXReturned);
            if(someJSON.length == 1) 
            {
                loadDefinition(someJSON[0].id);
            }
            else
            {   
                var terms = '<ul>';
                if ($('meta[name="content-language"]').attr("content") == "es")
                    terms += '<li>' + someJSON.length + ' (es)results found for: <b>' + searchTerm + '</b></li>';
                else
                    terms += '<li>' + someJSON.length + ' results found for: <b>' + searchTerm + '</b></li>';
          
                for (x in someJSON) {
                
                    terms += '<li><a href="javascript:;" onclick="loadDefinition(' + 
                    someJSON[x].id + ',\''+ language + '\');">' +  someJSON[x].item + '</a></li>';
                    
                }
                terms += '</ul>';    
                document.getElementById('output').innerHTML=terms;
            }
        }
    }
    if ($('meta[name="content-language"]').attr("content") == "es")
    {
        language = "Spanish";
        xmlhttp.open("GET","/TermDictionary.svc/SearchJSON/Spanish?searchTerm=" + searchTerm ,true);
    }
    else
    {
        language = "English";
        xmlhttp.open("GET","/TermDictionary.svc/SearchJSON/English?searchTerm=" + searchTerm ,true);
    }
    //document.getElementById('search').value = "";
    xmlhttp.send();
}

function loadDefinition(id) {
    var xmlhttp2;
    if (window.XMLHttpRequest)  {
        // code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp2=new XMLHttpRequest();
    }
    else  {
        // code for IE6, IE5
        xmlhttp2=new ActiveXObject('Microsoft.XMLHTTP');
    }

    xmlhttp2.onreadystatechange=function() {
        if (xmlhttp2.readyState==4 && xmlhttp2.status==200)  {
            // The added square bracket are to accomidate some 
            // funkiness in the JSON returned - needs to be addresed
            // in the service 
            var AJAXReturned = "[" + xmlhttp2.responseText +"]";
            var someJSON = eval(AJAXReturned);
            
            if ($('meta[name="content-language"]').attr("content") == "es")
            {
                var definition = '<p><b>' + someJSON[0].item + '</b> ' + someJSON[0].TermDictionaryDetail.TermPronunciation + 
                    '</p><p>' + someJSON[0].TermDictionaryDetail.DefinitionHTML + '</p><p>' +
                    '<a href="http://www.cancer.gov/diccionario?CdrID=' + someJSON[0].id + '" target="_blank">' +
                    '(es) Read this definition on the National Cancer Institute&#39;s website</a></p>';
             }
             else
             {
                var definition = '<p><b>' + someJSON[0].item + '</b> ' + someJSON[0].TermDictionaryDetail.TermPronunciation + 
                    '</p><p>' + someJSON[0].TermDictionaryDetail.DefinitionHTML + '</p><p>' +
                    '<a href="http://www.cancer.gov/dictionary?CdrID=' + someJSON[0].id + '" target="_blank">' +
                    'Read this definition on the National Cancer Institute&#39;s website</a></p>';
             }
            
            $('#output').scrollTop(0);    
            document.getElementById('output').innerHTML=definition;
        }
    }
    if ($('meta[name="content-language"]').attr("content") == "es")
        xmlhttp2.open("GET","/TermDictionary.svc/GetTermDictionaryByIdJSON/Spanish?TermId=" + id + "&Audience=Patient", true);
    else
        xmlhttp2.open("GET","/TermDictionary.svc/GetTermDictionaryByIdJSON/English?TermId=" + id + "&Audience=Patient", true);
       
    xmlhttp2.send();
}

function doSearch(e){
    var doit = false;

    if(e!=null)  {
        if (e.keyCode ==13)
            doit = true;
    }
    else
    doit = true;

    if(doit)
    {
        if ($('#search').val() != "")
            loadResults($('#search').val());
    }
}

// AutoComplete Stuff 
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
          $('#output').focus();
          doSearch(null);

       }       
    });
    
});


