<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MobileTermDictionaryResultsList.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.MobileTermDictionaryResultsList" %>
<!-- Page Title Start --> 
<style type="text/css">
    .style4
    {
        height: 12px;
    }
</style>
<!-- Page Title End -->
<!-- Search box Start --> 
<form id="aspnetForm" onsubmit="NCIAnalytics.TermsDictionarySearch(this,false);" action="/dictionary" method="post" name="aspnetForm">
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
            <a id="azLink" name="asLink" runat="server">A-Z</a>
        </td>
        <td></td>
    </tr>
    <tr style="height:12px">
    </tr>
    <tr>
        <td></td>
        <td>
        <!-- Result List Begin -->
        <asp:ListView ID="resultListView" runat="server" Visible="true">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <a name="<%#DataBinder.Eval(Container.DataItem, "TermName")%>"></a>
                <a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%><%=QueryStringLang%>"
                <%# ResultListViewHrefOnclick(Container)%>>
                <%# Eval("TermName")%></a>
                &nbsp;&nbsp;
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="100%" align="left">
                        <div id="myDiv">
                            <%# LimitText(Container, 235)%>
                        </div>
                        </td>
                    </tr>
                </table>
                <br>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="pnlNoDataEnglish" runat="server" Visible="false">
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.</asp:Panel>
                <asp:Panel ID="pnlNoDataSpanish" runat="server" Visible="false">
                    No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                    correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                    la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                    lista de términos que empiezan con esa letra.</asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
        <!-- Result List End -->
        </td>
    </tr>
    </tbody>
    </table>
</form>
<!-- Search box End --> 


