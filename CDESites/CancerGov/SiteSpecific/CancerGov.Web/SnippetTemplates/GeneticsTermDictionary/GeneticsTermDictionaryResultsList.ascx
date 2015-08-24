<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryResultsList.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.GenerticsTermDictionaryResultsList" %>
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
        <asp:ListView ID="resultListView" runat="server" Visible="true">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <dt><dfn><a href="<%# DictionaryURL %>?CdrID=<%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryTerm)(Container.DataItem)).ID  %>"
                    <%# ResultListViewHrefOnclick(Container)%>>
                    <%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryTerm)(Container.DataItem)).Term%></a>
                </dfn></dt>
                <dd class="pronunciation">
                    <a href="<%# ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]%><%#((NCI.Web.Dictionary.BusinessObjects.DictionaryTerm)(Container.DataItem)).Pronunciation.Audio  %>"
                        class="CDR_audiofile"><span class="hidden">listen</span></a>
                    <%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryTerm)(Container.DataItem)).Pronunciation.Key%>
                </dd>
                <dd class="definition">
                    <%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryTerm)(Container.DataItem)).Definition.Text%>
                </dd>
            </ItemTemplate>
            <EmptyDataTemplate>
                <asp:Panel ID="noMatched" runat="server" Visible="false">
                    No matches were found for the word or phrase you entered. Please check your spelling,
                    and try searching again. You can also type the first few letters of your word or
                    phrase, or click a letter in the alphabet and browse through the list of terms that
                    begin with that letter.
                </asp:Panel>
            </EmptyDataTemplate>
        </asp:ListView>
    </dl>
</div>
