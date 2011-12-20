<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryHome.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileDictionaryHome" %>

<!-- Search box Start --> 
<form id="aspnetForm" onsubmit="NCIAnalytics.TermsDictionarySearch(this,false);" action="/dictionary" method="get" name="aspnetForm">
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
    <tbody>
    <tr>
        <td></td>
        <td>
            <h2>Dictionary of Cancer Terms</h2>
        </td>       
    </tr>
    <tr>
        <td></td>
        <td width="90%">
            <input name="searchString" id="searchString" type="text" runat="server"  />
        </td>
        <td></td>
        <td width="5%">
            <asp:ImageButton runat="server" src="/images/go-button.png" onclick="ImageButton1_Click" />
        </td>
        <td width="2px"></td>
        <td>
        </td>
        <td></td>
    </tr>
    <tr style="height:12px">
    </tr>
    </tbody>
    </table>
    <table border="0" cellpadding="5" cellspacing="0" width="100%">
    <tbody>
        <tr>
            <td><strong><% =AnchorTagCreator('#') %></strong></td>
            <td><strong><% =AnchorTagCreator('A') %></strong></td>
            <td><strong><% =AnchorTagCreator('B') %></strong></td>
            <td><strong><% =AnchorTagCreator('C') %></strong></td>
            <td><strong><% =AnchorTagCreator('D') %></strong></td>
            <td><strong><% =AnchorTagCreator('E') %></strong></td>
            <td><strong><% =AnchorTagCreator('F') %></strong></td>
            <td><strong><% =AnchorTagCreator('G') %></strong></td>
            <td><strong><% =AnchorTagCreator('H') %></strong></td></tr><tr>
            
            <td><strong><% =AnchorTagCreator('I') %></strong></td>
            <td><strong><% =AnchorTagCreator('J') %></strong></td>
            <td><strong><% =AnchorTagCreator('K') %></strong></td>
            <td><strong><% =AnchorTagCreator('L') %></strong></td>
            <td><strong><% =AnchorTagCreator('M') %></strong></td>
            <td><strong><% =AnchorTagCreator('N') %></strong></td>
            <td><strong><% =AnchorTagCreator('O') %></strong></td>
            <td><strong><% =AnchorTagCreator('P') %></strong></td>
            <td><strong><% =AnchorTagCreator('Q') %></strong></td></tr><tr>
            
            <td><strong><% =AnchorTagCreator('R') %></strong></td>
            <td><strong><% =AnchorTagCreator('S') %></strong></td>
            <td><strong><% =AnchorTagCreator('T') %></strong></td>
            <td><strong><% =AnchorTagCreator('U') %></strong></td>
            <td><strong><% =AnchorTagCreator('V') %></strong></td>
            <td><strong><% =AnchorTagCreator('W') %></strong></td>
            <td><strong><% =AnchorTagCreator('X') %></strong></td>
            <td><strong><% =AnchorTagCreator('Y') %></strong></td>
            <td><strong><% =AnchorTagCreator('Z') %></strong></td>
        </tr>
    </tbody>
    </table>  
</form>
<!-- Search box End --> 



 


