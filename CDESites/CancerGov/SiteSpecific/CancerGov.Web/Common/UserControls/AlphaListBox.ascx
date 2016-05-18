<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="AlphaListBox.ascx.cs" Inherits="Www.Common.UserControls.AlphaListBox" %>

<asp:PlaceHolder ID="phBrowseEnglish" runat="server" EnableViewState="false">
    <span class="browse">Browse:</span>
</asp:PlaceHolder><asp:PlaceHolder ID="phBrowseSpanish" runat="server" Visible="false" EnableViewState="false">
    <span class="browse">Ojear:</span>
</asp:PlaceHolder><%=AlphaListItems%>

