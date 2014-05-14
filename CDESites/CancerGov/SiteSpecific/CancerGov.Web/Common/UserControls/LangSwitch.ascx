<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LangSwitch.ascx.cs" Inherits="www.Common.UserControls.LangSwitch" %>
<asp:Panel ID="pnlLangSelect" runat="server" CssClass="dictionary-individual-search-results-header">
    <div class="language-link">
        <asp:HyperLink ID="hlEnglish" runat="server">
            <asp:Image ID="imgEnglish" runat="server" ImageUrl="/images/in-english-white.gif" AlternateText="In English" />
        </asp:HyperLink>
        <asp:Label ID="lblEnglish" runat="server" Text="In English" Visible="false"></asp:Label>
    </div>
    <div class="language-link">
        <asp:Image ID="imgLangTabDivider" runat="server" ImageUrl="/images/tab-divider-white-left.gif" AlternateText="" />
    </div>
    <div class="language-link">
        <asp:Literal ID="litTextLangDivider" runat="server"></asp:Literal>
    </div>
    <div class="language-link">
        <asp:HyperLink ID="hlSpanish" runat="server">
            <asp:Image ID="imgSpanish" runat="server" ImageUrl="/images/en-espanol-gray.gif" AlternateText="En espa&ntilde;ol" />
        </asp:HyperLink>
        <asp:Label ID="lblSpanish" runat="server" Text="En espa&ntilde;ol" Visible="false"></asp:Label>
    </div>
    <br/>
</asp:Panel>
