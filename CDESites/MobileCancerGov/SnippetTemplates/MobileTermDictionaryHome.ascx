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
            <asp:ImageButton ID="goButton" name="goButton" runat="server" src="/images/go-button.png" onclick="ImageButton1_Click" />
        </td>
        <td width="2px"></td>
        <td>
            <a id="azLink" name="asLink" runat="server" href="">A-Z</a>
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
            <td><strong><a href="#">#</a></strong></td>
            <td><strong><a href="http://localhost:7069/TermDictionaryTest-O-Matic.aspx?expand=A">A</a></strong></td>
            <td><strong><a href="b.html">B</a></strong></td>
            <td><strong><a href="c.html">C</a></strong></td>

            <td><strong><a href="#">D</a></strong></td>
            <td><strong><a href="#">E</a></strong></td>
            <td><strong>F</strong></td>
            <td><strong><a href="#">G</a></strong></td>
            <td><strong><a href="#">H</a></strong></td></tr><tr>
            <td><strong><a href="#">I</a></strong></td>

            <td><strong>J</strong></td>
            <td><strong><a href="#" data-ajax="false">K</a></strong></td>
            <td><strong><a href="l.html">L</a></strong></td>
            <td><strong><a href="#" data-ajax="false">M</a></strong></td>
            <td><strong><a href="#">N</a></strong></td>
            <td><strong><a href="#" data-ajax="false">O</a></strong></td>

            <td><strong><a href="p.html">P</a></strong></td>
            <td><strong>Q</strong></td></tr><tr>
            <td><strong><a href="#" data-ajax="false">R</a></strong></td>
            <td><strong><a href="s.html">S</a></strong></td>
            <td><strong><a href="#">T</a></strong></td>
            <td><strong><a href="u.html">U</a></strong></td>

            <td><strong><a href="#">V</a></strong></td>
            <td><strong><a href="#">W</a></strong></td>
            <td><strong>X</strong></td>
            <td><strong><a href="#">Y</a></strong></td>
            <td><strong>Z</strong></td>
        </tr>
    </tbody>
    </table>  
</form>
<!-- Search box End --> 



 


