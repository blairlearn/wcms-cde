<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<div class="results">
    <h3>
        
        <span class="results-count" id="lblNumResults"><% =Results %></span> 
        <span class="results-count" id="lblResultsFor">results found for:</span> 
        <span class="term" id="Span1"><% =SearchString %></span>
    </h3>
    
    <dl class="dictionary-list">
        <asp:ListView ID="resultListView" runat="server" Visible="true">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <dt style="list-style-type: none;">
                <a href="<%# DictionaryURL %>?cdrid=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%>"
                <%# ResultListViewHrefOnclick(Container)%>>
                <dfn><%# Eval("TermName")%></dfn></a>
                <dd><%#AudioPronounceLink(Container)%></dd>
            
                <% if (ShowDefinition){ %>
                <dd><%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%></dd>
                <% } %>
                </dt>
                
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="noMatched" runat="server" Visible="true" >
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.
                </asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
       
        <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="3" class="simplePager" />
    </dl>
</div>