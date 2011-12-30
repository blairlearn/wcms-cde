<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryDefinitionView.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryDefinitionView" %>
<style type="text/css">
    .style1
    {
        height: 8px;
    }
</style>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>  
<table border="0" cellpadding="2" cellspacing="0" width="100%">
<tbody>
<tr >
    <td></td>
    <td ><span class="term"><% =TermName %></span></td>    
</tr>
<tr>
    <td></td>
    <td><!-- addthis block -->
        <div class="addthis_toolbox addthis_container addthis_default_style addthis_32x32_style" id="AddThisButtonList1">
            <a class="addthis_button_email"></a>
            <a class="addthis_button_facebook"></a>
            <a class="addthis_button_twitter"></a>
            <a class="addthis_button_plus.google.com"></a>
            <a class="addthis_button_compact"></a>
        </div>
        <script src="http://s7.addthis.com/js/250/addthis_widget.js#pubid=xa-4ed7cc9f006efaac" type="text/javascript"></script>
        <!-- end addthis block -->
    </td>   
</tr>
<tr>
    <td class="style1"></td>
    <td class="style1"></td>
</tr>
<tr>
    <td></td>
    <td>
        <% =AudioPronounceLink %>
    </td> 
</tr>
<tr>
    <td class="style1"></td>
    <td class="style1"></td>
</tr>
<tr>
    <td></td>
    <td>
        <span class="definition"><% =DefinitionHTML %></span>
    </td>    
</tr>
<tr>
    <td class="style1"></td>
    <td class="style1"></td>
</tr>
<tr>
    <td></td>
    <td>
        <table border="0" cellpadding="2" cellspacing="0" 
            style="width: 40%;" >
            <tr>
                <td><% =ImageLink %></td>
            </tr>
            <tr>
                <td><% =ImageCaption %></td>
            </tr>
        </table>
    </td>    
</tr>
</tbody>
</table>







