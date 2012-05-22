<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
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
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.
                </asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
        <br />
        <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="3" class="simplePager" />
    </ul>
</div>