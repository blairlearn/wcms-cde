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
                    <span><%# ((DictionaryTerm)(Container.DataItem)).Term%></span>
                </dfn>
            </dt>
            <dd class="info-summary" id="ddPatientInfo" runat="server" Visible="false">
                <asp:HyperLink ID="hlPatientInfo" runat="server" Visible="true">
                    <img src="/images/btn-patient-info.gif" alt="Patient Information" title="Patient Information" width="139" height="20" hspace="12" border="0"  align="absmiddle" />
                </asp:HyperLink>
            </dd>
            <dd class="definition">

                <%# ((DictionaryTerm)(Container.DataItem)).Definition.Html%>
                
                <%-- Location to display the list of aliases.  Code behind will make it visible if
                there's anything to show. --%>
                <asp:PlaceHolder id="phAliasList" runat="server" Visible="false">
                    <%# GenerateTermAliasList(Container.DataItem)%>
                </asp:PlaceHolder>
                
            </dd>
        </dl>
        
</ItemTemplate> 
</asp:Repeater>
