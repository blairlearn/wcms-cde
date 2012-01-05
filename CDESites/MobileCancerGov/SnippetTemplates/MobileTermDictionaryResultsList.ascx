<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryResultsList.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryResultsList" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<table border="0" cellpadding="2" cellspacing="0" width="100%">
<tbody>
    <tr>
        <td></td>
        <td>
        <!-- Result List w/o Definition Begin -->
        <asp:ListView ID="resultListViewNoDescription" runat="server" Visible="true">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <a name="<%#DataBinder.Eval(Container.DataItem, "TermName")%>"></a>
                <a href="<%# DictionaryURL %>?cdrid=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%>&language=<%=QueryStringLang%><% =SearchString %>"
                <%# ResultListViewHrefOnclick(Container)%>>
                <%# Eval("TermName")%></a>
                &nbsp;&nbsp;
                <br />
                &nbsp;&nbsp;
                <br />
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="pnlNoDataEnglishNoDef" runat="server" Visible="true" Width="275px">
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.</asp:Panel>
                <asp:Panel ID="pnlNoDataSpanishNoDef" runat="server" Visible="false" Width="275px">
                    No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                    correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                    la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                    lista de términos que empiezan con esa letra.</asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
        <!-- Result List w/o Definition End -->
        <!-- Result List w/Definition Begin -->
        <asp:ListView ID="resultListView" runat="server" Visible="false">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                <td class="mtd_spacer1"></td>
                <td width="100%" align="left">
                <a name="<%#DataBinder.Eval(Container.DataItem, "TermName")%>"></a>
                <a href="<%# DictionaryURL %>?cdrid=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%>&language=<%=QueryStringLang%><% =SearchString %>"
                <%# ResultListViewHrefOnclick(Container)%>>
                <%# Eval("TermName")%></a>
                &nbsp;&nbsp;
                </td>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td class="mtd_spacer1"></td>
                        <td width="100%" align="left">
                        <div>
                            <%# LimitText(Container, 235)%>
                        </div>
                        </td>
                    </tr>
                </table>
                <br>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="pnlNoDataEnglishDef" runat="server" Visible="true" Width="275px" >
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.</asp:Panel>
                <asp:Panel ID="pnlNoDataSpanishDef" runat="server" Visible="false" Width="275px">
                    No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                    correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                    la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                    lista de términos que empiezan con esa letra.</asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
        <!-- Result List w/Definition End -->
        <asp:Panel ID="pnlPager" runat="server" Visible="false" >
        <table border="0" cellpadding="0" cellspacing="0">
        <tbody>
        <tr>
            <td class="mtd_spacer1"></td>
            <td width="40px">
                <button class="ui-btn-hidden" 
                    data-theme="a" 
                    type="button" 
                    aria-disabled="false" 
                    onclick="window.location.href='<% =PreviousPagerOnclick %>'">Previous</button>
            </td>
            <td></td>
            <td >&nbsp;<% =PagerInfo %>&nbsp;</td>
            <td></td>
            <td width="40px">
                <button class="ui-btn-hidden" 
                    data-theme="a" 
                    type="button" 
                    aria-disabled="false" 
                    onclick="window.location.href='<% =NextPagerOnclick %>'">Next</button>
            </td>
        </tr>
        </tbody>
        </table>
        </asp:Panel>
        </td>
    </tr>
</tbody>
</table>