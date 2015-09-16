<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DrugDictionaryDefinitionView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.DrugDictionaryDefinitionView" %>
<%@ Register TagPrefix="DrugDictionaryHome" TagName="SearchBlock" Src="~/SnippetTemplates/DrugDictionary/Views/DrugDictionaryHome.ascx" %>
<%@ Import Namespace="NCI.Web.Dictionary.BusinessObjects" %>
 
<DrugDictionaryHome:SearchBlock id="dictionarySearchBlock" runat="server" />
    
<asp:Repeater ID="drugDictionaryDefinitionView" runat="server" OnItemDataBound="drugDictionaryDefinitionView_OnItemDataBound">
<ItemTemplate> 
        <!-- Term and def -->
        <dl>
            <dt>
                <dfn>
                    <%# ((DictionaryTerm)(Container.DataItem)).Term%>
                </dfn>
            </dt>
            <!-- Drug definitions don't have pronunciations, so this won't actually render. -->
            <asp:PlaceHolder ID="phPronunciation" runat="server">
                <dd class="pronunciation">
                    <a id="pronunciationLink" runat="server" class="CDR_audiofile"><span class="hidden">listen</span></a>
                    <asp:Literal ID="pronunciationKey" runat="server" />
                </dd>
            </asp:PlaceHolder>
            <dd class="definition">
                <%# ((DictionaryTerm)(Container.DataItem)).Definition.Text%>
                
                <!-- Location to display the list of aliases.  Code behind will make it visible if
                there's anything to show. -->
                <asp:PlaceHolder id="phAliasList" runat="server" Visible="false">
                    <%# GenerateTermAliasList(Container.DataItem)%>
                </asp:PlaceHolder>
                
            </dd>
        </dl>
        
</ItemTemplate> 
</asp:Repeater>
