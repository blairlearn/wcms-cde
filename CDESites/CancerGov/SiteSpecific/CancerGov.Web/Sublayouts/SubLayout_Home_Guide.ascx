<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<!-- BEGIN CARDS ROW -->
<div id="accordion" class="row guide-card">
        <NCI:TemplateSlot
            id="nvcgSlLayoutGuideA"
            CssClass="equalheight medium-4 columns card featured-card gutter"
            runat="server"
            data-match-height="" />
        <NCI:TemplateSlot
            id="nvcgSlLayoutGuideB"
            CssClass="equalheight medium-4 columns card card gutter"
            runat="server"
            data-match-height="" />
</div>
<!-- END CARDS ROW -->