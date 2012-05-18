<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<div class="searchResults">
<div class="resultsFound"><% =Results %> results found for: <% =SearchString %></div>
    <ul>
        <asp:ListView ID="resultListView" runat="server" Visible="true">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                <a href="<%# DictionaryURL %>?cdrid=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%>"
                <%# ResultListViewHrefOnclick(Container)%>>
                <%# Eval("TermName")%></a>&nbsp;
                <%#AudioPronounceLink(Container)%>
                <br />
                <% if (ShowDefinition){ %>
                <%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%>
                <% } %>
                </li>
                <br />
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="noMatched" runat="server" Visible="true" Width="275px" >
                    <% if (IsSpanish)
                       { %>
                    No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                    correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                    la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                    lista de términos que empiezan con esa letra.
                    <% }
                       else
                       { %>
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.
                    <% } %>
                </asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
        <br />
        <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="3" class="simplePager" />
    </ul>
</div>