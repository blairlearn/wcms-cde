<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<div class="feature-primary-title">
    <h3>
    <%= CurrentLanguage().Equals("en") ? "Featured News" : "Noticias destacadas"%>
    </h3>
</div>
<!-- BEGIN FEATURE PRIMARY CARDS ROW -->
<NCI:TemplateSlot
    id="nvcgSlLayoutFeatureA"
    CssClass="row feature-primary"
    AdditionalSnippetClasses="equalheight large-4 columns card gutter"
    runat="server"
    data-match-height="" />
<!-- END FEATURE PRIMARY CARDS CARDS ROW -->