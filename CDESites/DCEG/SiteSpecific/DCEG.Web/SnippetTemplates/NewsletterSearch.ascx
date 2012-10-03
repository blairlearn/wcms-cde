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
	<p> Or search between these dates: </p>
	<table width="100%" border="0" cellpadding="2" cellspacing="0">
	<tbody>
	  <tr>
		<td width="40" ><label for="startMonth" class="hidden"> select start Month</label>
		  <select name="startMonth" id="startMonth">
			<option value="1" selected="">Jan.</option>
			<option value="2">Feb.</option>
			<option value="3">Mar.</option>
			<option value="4">Apr.</option>
			<option value="5">May</option>
			<option value="6">Jun.</option>
			<option value="7">Jul.</option>
			<option value="8">Aug.</option>
			<option value="9">Sept.</option>
			<option value="10">Oct.</option>
			<option value="11">Nov.</option>
			<option value="12">Dec.</option>
		  </select></td>
		<td width="40"><label for="startYear" class="hidden"> select start Year</label>
		<select name="startYear" id="startYear">
			<% =GetYearListItems("startYear") %>
		</select></td>
		<td width="20">&nbsp;and&nbsp; </td>
		<td width="40"><label for="endMonth" class="hidden"> select end month</label>
		<select name="endMonth" id="endMonth">
			<% =GetMonthListItems("endMonth") %>
		</select></td>
		<td width="40"><label for="endYear" class="hidden"> select end Year</label>
		<select name="endYear" id="endYear">
			<% =GetYearListItems("endYear") %>
		</select></td>
		<td width="100%">&nbsp;&nbsp;
		<input type="submit" name="searchRangeButton" value="Search" onclick="searchType=1;" id="searchRangeButton"></td>
	  </tr>
	</tbody>
	</table>
</form>
</div>
