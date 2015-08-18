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
                
            <asp:ListView ID="resultListView" runat="server">
                <LayoutTemplate>
                    <dl class="dictionary-list">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </dl>
                </LayoutTemplate>
                <ItemTemplate>
                    <dt>
                        <dfn>
                        <a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%>" <%# ResultListViewHrefOnclick(Container)%>>
                             <%# Eval("TermName")%></a>
                             
     
                        </dfn>
                    </dt>
                    <dd class="pronunciation">
                        <%# AudioMediaHTML(DataBinder.Eval(Container.DataItem, "AudioMediaHTML")) %>
                        <span><%#DataBinder.Eval(Container.DataItem, "TermPronunciation")%></span>
                    </dd>
                    <dd class="definition">
                        <%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%>
                    </dd>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <asp:Panel ID="pnlNoDataEnglish" runat="server" Visible="false">
                        No matches were found for the word or phrase you entered. Please check your spelling,
                        and try searching again. You can also type the first few letters of your word or
                        phrase, or click a letter in the alphabet and browse through the list of terms that
                        begin with that letter.
                    </asp:Panel>
                    <asp:Panel ID="pnlNoDataSpanish" runat="server" Visible="false">
                        No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                        correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                        la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                        lista de términos que empiezan con esa letra.
                    </asp:Panel>
                </EmptyDataTemplate>
            </asp:ListView>
 </div>