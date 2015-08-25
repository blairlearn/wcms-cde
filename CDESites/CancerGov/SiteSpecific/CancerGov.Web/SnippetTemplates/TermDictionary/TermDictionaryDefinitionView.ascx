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
                        <%# ((NCI.Web.Dictionary.BusinessObjects.TermReturn)(Container.DataItem)).Term.Term  %>
                       
                    </dfn>
                </dt>
                <dd class="pronunciation">
                    <a href="<%# ConfigurationSettings.AppSettings["CDRAudioMediaLocation"]%><%#((NCI.Web.Dictionary.BusinessObjects.TermReturn)(Container.DataItem)).Term.Pronunciation.Audio  %>" class="CDR_audiofile"><span class="hidden">listen</span></a>
                    <%# ((NCI.Web.Dictionary.BusinessObjects.TermReturn)(Container.DataItem)).Term.Pronunciation.Key  %>
                </dd>
                <dd class="definition">
                    <%# ((NCI.Web.Dictionary.BusinessObjects.TermReturn)(Container.DataItem)).Term.Definition.Text  %>
                    
                    <asp:Panel runat="server" ID="pnlRelatedInfo">
                        <div class="related-resources">
                            <h6><asp:Literal ID="litMoreInformation" runat="server" /></h6>
                            <asp:Repeater ID="relatedExternalRefs" runat="server" Visible="false">
                                <HeaderTemplate> 
                                    <ul class="no-bullets">
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <li><a href="<%# ((NCI.Web.Dictionary.BusinessObjects.RelatedExternalLink)(Container.DataItem)).Url  %>"><%# ((NCI.Web.Dictionary.BusinessObjects.RelatedExternalLink)(Container.DataItem)).Text  %></a></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="relatedSummaryRefs" runat="server" Visible="false">
                                 <HeaderTemplate> 
                                    <ul class="no-bullets">
                                </HeaderTemplate>
                                <ItemTemplate>
                                       <li><a href="<%# ((NCI.Web.Dictionary.BusinessObjects.RelatedSummary)(Container.DataItem)).url  %>"><%# ((NCI.Web.Dictionary.BusinessObjects.RelatedSummary)(Container.DataItem)).Text  %></a></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="relatedDrugInfoSummaries" runat="server" Visible="false">
                                 <HeaderTemplate> 
                                    <ul class="no-bullets">
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <li><a href="<%# ((NCI.Web.Dictionary.BusinessObjects.RelatedDrugSummary)(Container.DataItem)).url  %>"><%# ((NCI.Web.Dictionary.BusinessObjects.RelatedDrugSummary)(Container.DataItem)).Text  %></a></li>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:PlaceHolder ID="phRelatedTerms" runat="server" Visible="false">
                                <p><asp:Label ID="labelDefintion" runat="server" class="related-definition-label"/>
                                 <asp:Repeater ID="relatedTerms" runat="server" OnItemDataBound="relatedTerms_OnItemDataBound">
                                   
                                    <ItemTemplate>
                                         <asp:HyperLink ID="relatedTermLink" runat="server" />
                                         <asp:Literal ID="relatedTermSeparator" runat="server" Text="," Visible="false" />
                                    </ItemTemplate>
                                    
                                </asp:Repeater>
                                </p>
                            </asp:PlaceHolder>
                         
                            <asp:Repeater ID="relatedImages" runat="server" Visible="false" OnItemDataBound="relatedImages_OnItemDataBound">
                              <ItemTemplate>
                                <figure class="image-left-medium">
                                    <a id="termEnlargeImage" runat="server" target="_blank" class="article-image-enlarge no-resize">Enlarge</a>
                                    <img id="termImage" runat="server" src="" alt="" />
                                        <figcaption>
                                            <div class="caption-container no-resize">
                                            <p><%# ((NCI.Web.Dictionary.BusinessObjects.ImageReference)(Container.DataItem)).Caption  %></p>
                                            </div>
                                        </figcaption>
                                 </figure>
                              </ItemTemplate>
                         </asp:Repeater>
                      
                         </div>
                         
                    </asp:Panel>
                    
                </dd>
            </dl>
        </div>
        
</ItemTemplate> 
</asp:Repeater>
