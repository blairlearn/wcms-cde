<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Dictionaries.SnippetControls.TermDictionary.TermDictionaryResultsList"%>
<%@ Register TagPrefix="TermDictionaryHome" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/Views/TermDictionaryHome.ascx" %>
<%@ Import Namespace="NCI.Web.Dictionary.BusinessObjects" %>

<TermDictionaryHome:SearchBlock id="dictionarySearchBlock" runat="server" />

 <div class="results">
            <!-- Number of results -->
            <asp:Panel ID="numResDiv" runat="server" CssClass="dictionary-search-results-header">
                <span class="results-count">
                    <asp:Label ID="lblNumResults" CssClass="results-num" runat="server"></asp:Label>
                    <asp:Label ID="lblResultsFor" runat="server"></asp:Label>
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
                        <a href="<%# DictionaryPrettyURL %>/def/<%# GetFriendlyName(((DictionarySearchResult)(Container.DataItem)).ID)  %>" <%# ResultListViewHrefOnclick(Container)%>>
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
                         <%# ((DictionarySearchResult)(Container.DataItem)).Term.Definition.Html%>
                    </dd>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNoDataMessage" runat="server" OnLoad="GetNoDataMessage" />
                   
                </EmptyDataTemplate>
            </asp:ListView>
             </dl>
 </div>