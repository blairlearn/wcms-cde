<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionaryResultsList.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.TermDictionaryResultsList" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="DictionarySearchBlock.ascx" %>
 
<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />

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
            <asp:ListView ID="resultListView" runat="server">
                <LayoutTemplate>
                    
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                   
                </LayoutTemplate>
                <ItemTemplate>
                    <dt>
                        <dfn>
                        <a href="<%# DictionaryURL %>?CdrID=<%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryExpansion)(Container.DataItem)).ID  %>" <%# ResultListViewHrefOnclick(Container)%>>
                             <%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryExpansion)(Container.DataItem)).MatchedTerm%></a>
                             
     
                        </dfn>
                    </dt>
                     <dd class="pronunciation">
                            <a href="<%# ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]%>/<%#((NCI.Web.Dictionary.BusinessObjects.DictionaryExpansion)(Container.DataItem)).Term.Pronunciation.Audio  %>" class="CDR_audiofile"><span class="hidden">listen</span></a>
                            <%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryExpansion)(Container.DataItem)).Term.Pronunciation.Key%>
                      </dd>
                    <dd class="definition">
                         <%# ((NCI.Web.Dictionary.BusinessObjects.DictionaryExpansion)(Container.DataItem)).Term.Definition.Text%>
                    </dd>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <asp:Label ID="lblNoDataMessage" runat="server" OnLoad="GetNoDataMessage" />
                   
                </EmptyDataTemplate>
            </asp:ListView>
             </dl>
 </div>