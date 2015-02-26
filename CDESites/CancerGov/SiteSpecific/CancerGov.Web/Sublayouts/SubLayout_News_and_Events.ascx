<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<NCI:CDEField Scope="Snippet" FieldName="sublayout_title" id="CDEField1" runat="server"/>
<!-- BEGIN FEATURE PRIMARY CARDS ROW -->
<div class="row">
<NCI:TemplateSlot
    id="nvcgSlLayoutFeatureA"
    CssClass="row feature-primary"
    AdditionalSnippetClasses="equalheight large-4 columns card gutter"
    runat="server"
    data-match-height="" />
</div>
<!-- END FEATURE PRIMARY CARDS CARDS ROW -->
<!-- BEGIN GENERAL ROW -->
<div class="row news" data-match-height="">
	<NCI:TemplateSlot
		id="nvcgSlLayoutGeneralA"
		CssClass="equalheight large-8 columns card"
		runat="server" />
	<NCI:TemplateSlot
		id="nvcgSlLayoutGeneralB"
		CssClass="equalheight large-4 columns card"
        runat="server" />
</div>
<!-- END GENERAL ROW -->