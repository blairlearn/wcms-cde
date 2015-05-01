<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<!-- BEGIN FEATURE PRIMARY CARDS ROW -->
<div class="feature-primary-title">
    <h3>
    <%= CurrentLanguage().Equals("en") ? "Featured News" : "Noticias destacadas"%>
    </h3>
</div>
<NCI:TemplateSlot
    id="nvcgSlLayoutFeatureA"
    CssClass="row feature-primary"
    AdditionalSnippetClasses="equalheight large-4 columns card gutter"
    runat="server"
    data-match-height="" />
<!-- END FEATURE PRIMARY CARDS CARDS ROW -->
<!-- BEGIN GENERAL ROW -->
<div class="row news" data-match-height="">
	<NCI:TemplateSlot
		id="nvcgSlLayoutGeneralA"
		CssClass="large-8 columns card gutter"
		runat="server" />
	<NCI:TemplateSlot
		id="nvcgSlLayoutGeneralB"
		CssClass="large-4 columns gutter accordion"
		runat="server" />
</div>
<!-- END GENERAL ROW -->