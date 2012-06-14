<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<div class="searchResults">
    <span class="page-title">
        <br />
        <span class="page-title" id="lblNumResults"><% =Results %></span> 
        <span class="page-title" id="lblResultsFor">results found for:</span> 
        <span class="page-title" id="Span1"><% =SearchString %></span>
        <br />
        <img width="10" height="19" border="0" alt="" src="/images/spacer.gif">
        <br>
        <img width="571" height="1" border="0" alt="" src="/images/gray_spacer.gif">
         <br />
        <img width="10" height="19" border="0" alt="" src="/images/spacer.gif">
        <br />
    </span>
    <ul class="no-bullets">
        <asp:ListView ID="resultListView" runat="server" Visible="true">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <li style="list-style-type: none;">
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