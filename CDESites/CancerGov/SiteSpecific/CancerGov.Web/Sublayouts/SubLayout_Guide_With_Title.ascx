<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<!-- BEGIN Row Title -->
<h2>
    <NCI:CDEField
        Scope="Snippet"
        FieldName="sublayout_title"
        id="CDEField1"
        CssClass="row collapse"
        AdditionalSnippetClasses="large-12 columns"
        runat="server" />
</h2>
<!-- END Row Title -->
<!-- BEGIN GUIDE CARDS ROW -->
<div id="accordion">
    <NCI:TemplateSlot
        id="nvcgSlLayoutGuideB"
        CssClass="row guide-card"
        AdditionalSnippetClasses="equalheight medium-4 columns card gutter"
        runat="server"
        data-match-height="" />
</div>
<!-- END GUIDE CARDS ROW -->
<!-- BEGIN FEATURE SECONDARY CARDS ROW -->
<NCI:TemplateSlot
    id="nvcgSlLayoutFeatureB"
    CssClass="row feature-secondary"
    AdditionalSnippetClasses="equalheight large-4 columns card gutter"
    runat="server"
    data-match-height="" />
<!-- END FEATURE SECONDARY CARDS ROW -->