<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsletterSearch.ascx.cs" Inherits="DCEG.Web.SnippetTemplates.NewsletterSearch" %>

<script type="text/javascript"> 
var searchType = 0; /* 0 = Search All 1 = Search Date Range */

function NewsletterSearchSubmit()
{
	if(document.NewsletterSearchForm.cbkeyword.value.length == 0 || document.NewsletterSearchForm.cbkeyword.value =="Enter Keyword")
	{
		alert("You must enter a search value.");
		document.NewsletterSearchForm.cbkeyword.value="";
		document.NewsletterSearchForm.cbkeyword.focus();
		return false;		
	}
	else{
	    return true; 
	}
}

function CBSetSearchType(e)
{
        // if search is performed by hitting enter in the keyword textbox, set searchType 
        if (window.event) { e = window.event; }
        if (e.keyCode == 13)
        {
                searchType=0;
        }
}
</script>

<style type="text/css">

table.table-default {
    border: 1px solid #BDBDBD;
    border-collapse: collapse;
}
table.table-default th {
    background-color: #ECECEC;
    border: 1px solid #BDBDBD;
    padding: 2px 4px;
}
table.table-default td {
    border: 1px solid #BDBDBD;
    padding: 4px;
}
table.table-default-center-td td {
    text-align: center;
}
table.border {
    border: 1px solid #BDBDBD;
    border-collapse: collapse;
}
table.border th {
    border: 1px solid #BDBDBD;
    border-collapse: collapse;
}
table.border td {
    border: 1px solid #BDBDBD;
    border-collapse: collapse;
}
.hidden {
    height: 1px;
    left: 0;
    overflow: hidden;
    position: absolute;
    top: -500px;
    width: 1px;
}
</style>

<div class="newsletterSearchFormContainer">
<form action="/news-events/linkage-newsletter/search-archive/results" name="NewsletterSearchForm" method="post" onsubmit="<% =SubmitScript %>" >
	<h2>Search Linkage</h2>
	<p>Find specific newsletter articles</p><br />
	<label for="cbkeyword" id="Label1" >Enter keyword(s):</label><br />
	<input name="cbkeyword" size="28"onkeyup="CBSetSearchType(event);" id="cbkeyword" value="Enter Keyword" onfocus="value =''" />
	<input type="submit" value="Search" onclick="searchType=0;" id="ctl10_searchAllButton" name="ctl10$searchAllButton" />
</form>
</div>
