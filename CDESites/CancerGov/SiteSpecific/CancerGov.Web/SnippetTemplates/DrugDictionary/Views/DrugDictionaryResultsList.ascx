﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Dictionaries.SnippetControls.DrugDictionary.DrugDictionaryResultsList" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>
<%@ Import Namespace="NCI.Web.Dictionary.BusinessObjects" %>

<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />

 <div class="results" data-dict-type="drug">
    <!-- Number of results -->
    <asp:Panel ID="numResDiv" runat="server" CssClass="dictionary-search-results-header">
        <span class="results-count">
            <asp:Label ID="lblNumResults" CssClass="results-num" runat="server"></asp:Label>
            <asp:Label ID="lblResultsFor" Text="result found for: " runat="server"></asp:Label>
            <asp:Label ID="lblWord" CssClass="term" runat="server"></asp:Label>
        </span>
    </asp:Panel>

    <asp:ListView ID="resultListView" runat="server" OnItemDataBound="resultListView_OnItemDataBound">
        <LayoutTemplate>
            <dl class="dictionary-list">
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </dl>
        </LayoutTemplate>
        <ItemTemplate>
            <dt>
               <dfn data-cdr-id="<%# ((DictionarySearchResult)(Container.DataItem)).ID%>">
                    <a href="<%# this.DictionaryRouter.GetDefinitionUrl() + GetFriendlyName(((DictionarySearchResult)(Container.DataItem)).ID.ToString())  %>" <%# ResultListViewHrefOnclick(Container)%>>
                        <%# HiLite(((DictionarySearchResult)(Container.DataItem)).MatchedTerm )%></a>
                    <span class="dictionary-partial-match-n">
                        <%# GetTermAliasList(Container.DataItem)%>
                    </span>
                </dfn>
            </dt>
            <asp:PlaceHolder ID="phPronunciation" runat="server">
                 <dd class="pronunciation">
                        <a id="pronunciationLink" runat="server" class="CDR_audiofile"><span class="hidden">listen</span></a>
                        <asp:Literal ID="pronunciationKey" runat="server" />
                  </dd>
            </asp:PlaceHolder>
            <dd class="definition">
                 <%# GetTermDefinition(Container.DataItem) %>
            </dd>
        </ItemTemplate>
        <EmptyDataTemplate>
            <asp:Label ID="lblNoDataMessage" runat="server" OnLoad="GetNoDataMessage" />
        </EmptyDataTemplate>
    </asp:ListView>
 </div>
<asp:Literal ID="litPager" runat="server"></asp:Literal>
 