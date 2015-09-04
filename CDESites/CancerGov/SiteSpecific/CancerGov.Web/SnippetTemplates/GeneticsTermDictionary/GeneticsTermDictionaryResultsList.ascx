<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
<%@ Import Namespace="NCI.Web.Dictionary.BusinessObjects" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>

<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />

<div class="results">
    <!-- Number of results -->
    <asp:Panel ID="numResDiv" runat="server" CssClass="dictionary-search-results-header">
        <span class="results-count">
            <asp:Label ID="lblNumResults" CssClass="results-num" runat="server"></asp:Label>
            <asp:Label ID="lblResultsFor" runat="server" Text="results found for:"/>
            <asp:Label ID="lblWord" CssClass="term" runat="server"></asp:Label>
        </span>
    </asp:Panel>
    <dl class="dictionary-list">
        <asp:ListView ID="resultListView" runat="server" OnItemDataBound="resultListView_OnItemDataBound">
           <LayoutTemplate>
                    <dl class="dictionary-list">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </dl>
                </LayoutTemplate>
            <ItemTemplate>
                    <dt>
                        <dfn>
                        <a href="<%# DictionaryURL %>?CdrID=<%# ((DictionarySearchResult)(Container.DataItem)).ID  %>" <%# ResultListViewHrefOnclick(Container)%>>
                             <%# ((DictionarySearchResult)(Container.DataItem)).MatchedTerm%></a>
                        </dfn>
                    </dt>
                     <asp:PlaceHolder ID="phPronunciation" runat="server">
                         <dd class="pronunciation">
                                <a id="pronunciationLink" runat="server" class="CDR_audiofile"><span class="hidden">listen</span></a>
                                <asp:Literal ID="pronunciationKey" runat="server" />
                          </dd>
                    </asp:PlaceHolder>
                    <dd class="definition">
                         <%# ((DictionarySearchResult)(Container.DataItem)).Term.Definition.Text%>
                    </dd>
                </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="noMatched" runat="server">
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.
                </asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
    </dl>
</div>
