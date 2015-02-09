<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<div id="welcomeDiv">
    <p>Welcome to the NCI Dictionary of Genetics Terms, which contains technical definitions for more than 150 terms related to genetics. These definitions were developed by the <a href="http://www.cancer.gov/cancertopics/pdq/cancer-genetics-board">PDQ® Cancer Genetics Editorial Board</a> to support the evidence-based, peer-reviewed <a href="http://www.cancer.gov/cancertopics/pdq/genetics">PDQ cancer genetics information summaries</a>.</p>
</div>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<div class="results">
    <span class="results-count">
        <span class="results-num" id="lblNumResults"><% =Results %></span> 
        results found for: 
        <span class="term" id="Span1"><% =SearchString %></span>
    </span>
    
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
                <dd class="pronunciation"><%#AudioPronounceLink(Container)%></dd>
            
                <% if (ShowDefinition){ %>
                <dd class="definition"><%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%></dd>
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