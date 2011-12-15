<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionary.ascx.cs" Inherits="MobileCancerGov.Web.UserControls.TermDictionary" %>

<asp:Panel id="TermSearch" runat="server" Visible="true"> 
<div class="pageTitle">
    <h2>Dictionary of Cancer Terms</h2>
</div> 
<table border="0" cellpadding="5" cellspacing="0" width="100%">
<tbody>
    <tr>
        <td width="85%">
            <input name="" type="text" placeholder="Search Dictionary" style="width:90%" />
        </td>
    <td>
    <div>
        <div style="width:35px; float:left;">
            <img src="/images/icon-question-mark-red.png" /> 
        </div>
        <div class="callBtn">
            <a href="tel:18004226237"><img src="/images/go-button.png"></a>
        </div>
    </div>
    </td>
    </tr>
</tbody>
</table>
<table border="0" cellpadding="5" cellspacing="0" width="100%">
    <tbody>
        <tr>
            <td><strong><a href="#">#</a></strong></td>
            <td><strong><a href="#">A</a></strong></td>
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
</asp:Panel>    
<asp:Panel id="ViewResultsList" runat="server" Visible="false"> 
</asp:Panel> 
<asp:Panel id="ViewDefinition" runat="server" Visible="false"> 
</asp:Panel> 

</div>
<asp:Panel id="basic" runat="server" Visible="false">
<p>This is Basic</p>
</asp:Panel>
<asp:Panel id="advanced" runat="server" Visible="true">
<p>This is advanced</p>
</asp:Panel>







