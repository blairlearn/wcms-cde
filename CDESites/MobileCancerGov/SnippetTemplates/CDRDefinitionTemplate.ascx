<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CDRDefinitionTemplate.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.CDRDefinitionTemplate" %>
<div data-role="collapsible" data-collapsed="true" data-iconpos="right" id="section_definition">
<h2 class="section_heading">Definition</h2>
<div id="spDefinitionText" style="display: block" runat="server">
<asp:Literal runat="server" ID="ltDefinitionText"></asp:Literal>&nbsp;<a visible="false" name="moreLink" id="moreLink" href="javascript:toggle();" runat="server"></a>
</div>
</div>



