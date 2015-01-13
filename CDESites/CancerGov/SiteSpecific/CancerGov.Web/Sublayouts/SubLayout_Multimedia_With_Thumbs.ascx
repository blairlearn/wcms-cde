<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SubLayoutControl" %>
<!-- BEGIN MULTIMEDIA, SECONDARY FEATURE ROW -->
<div class="multimedia-slot">
    <div class="row multimedia" data-match-height="">
        <NCI:TemplateSlot
            id="nvcgSlLayoutMultimediaA"
            CssClass="equalheight large-8 columns card"
            runat="server" />
        <NCI:TemplateSlot
            id="nvcgSlLayoutFeatureB"
            CssClass="equalheight large-4 columns card"
            runat="server" />
    </div>
</div>
<!-- BEGIN THUMBNAIL SECTION -->
<NCI:TemplateSlot
    id="nvcgSlLayoutThumbnailA"
    CssClass="row card-thumbnail"
    runat="server"
    data-match-height="" />
<!-- END THUMBNAIL SECTION -->