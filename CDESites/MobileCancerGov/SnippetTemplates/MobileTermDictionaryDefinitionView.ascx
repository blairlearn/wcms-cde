<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryDefinitionView.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryDefinitionView" %>
<form id="aspnetForm" onsubmit="NCIAnalytics.TermsDictionarySearch(this,false);" action="/dictionary" method="post" name="aspnetForm">
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
    <tbody>
    <tr>
        <td></td>
        <td><h2>Dictionary of Cancer Terms</h2></td>
               
    </tr>
    <tr>
        <td></td>
        <td width="90%">
            <input name="searchString" id="searchString" type="text" runat="server"  />
        </td>
        <td></td>
        <td width="5%">
            <asp:ImageButton ID="goButton" name="goButton" runat="server" 
                src="/images/go-button.png" onclick="ImageButton1_Click" />
        </td>
        <td width="2px"></td>
        <td>
            <a id="azLink" name="asLink" runat="server">A-Z</a>
        </td>
        <td></td>
    </tr>
    </tbody>
    </table>
</form>
<% =TermName %>
<div class="addthis_toolbox addthis_container addthis_default_style addthis_32x32_style" id="AddThisButtonList1">
    <a class="addthis_button_email"></a>
    <a class="addthis_button_facebook"></a>
    <a class="addthis_button_twitter"></a>
    <a class="addthis_button_plus.google.com"></a>
    <a class="addthis_button_compact"></a>
</div>
<script src="http://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-4ed7cc9f006efaac" type="text/javascript"></script>
<p />   
<% =DefinitionHTML %>




