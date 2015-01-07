<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<!-- BEGIN MULTIMEDIA, SECONDARY FEATURE ROW -->
<div class="content-bottom-slot">
    <div class="row multimedia">
        <NCI:TemplateSlot
            id="TemplateSlot1"
            CssClass="equalheight large-8 columns card"
            runat="server"
            data-match-height="" />
        <NCI:TemplateSlot
            id="TemplateSlot2"
            CssClass="equalheight large-4 columns card"
            runat="server"
            data-match-height="" />
    </div>
</div>
<!-- BEGIN THUMBNAIL SECTION -->
<NCI:TemplateSlot
    id="nvcgSlLayoutThumbnailA"
    CssClass="row card-thumbnail"
    runat="server"
    data-match-height="" />
<!-- END THUMBNAIL SECTION -->