<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionaryDefinitionView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.TermDictionaryDefinitionView" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="DictionarySearchBlock.ascx" %>


 
<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />

    
<asp:Repeater ID="termDictionaryDefinitionView" runat="server" OnItemDataBound="termDictionaryDefinitionView_OnItemDataBound">
<ItemTemplate> 
        <!-- Term and def -->
        <div class="results">
            <dl class="dictionary-list">
                <dt>
                    <dfn>
                       <%# Eval("TermName") %>
                    </dfn>
                </dt>
                <dd class="pronunciation">
                    <asp:Literal ID="litAudioMediaHtml" runat="server"></asp:Literal>
                    <%# Eval("TermPronunciation") %>
                </dd>
                <dd class="definition">
                    <%# Eval("DefinitionHTML")%>
                    <asp:Panel runat="server" ID="pnlRelatedInfo">
                        <div class="related-resources">
                            <h6><asp:Literal ID="litMoreInformation" runat="server" /></h6>
                            <ul class="no-bullets">
                                <li><a href="http://www.genome.gov/glossary/index.cfm?id=70">Gene</a></li>
                                <li><a href="http://www.cancer.gov/about-cancer/causes-prevention/genetics">The Genetics of Cancer</a></li>
                             </ul>
                         </div>
                        <asp:Literal ID="litRelatedLinkInfo" runat="server"></asp:Literal>
                    </asp:Panel>
                    <asp:Literal ID="litImageHtml" runat="server"></asp:Literal>
                </dd>
            </dl>
        </div>
        
</ItemTemplate> 
</asp:Repeater>
