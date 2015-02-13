<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmergencyBanner.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.EmergencyBanner" %>
<%@ Register tagPrefix="CGov" namespace="CancerGov.EmergencyAlert" assembly="CancerGov.EmergencyAlert" %>
<div class="site-notification">
    <div class="row">
        <CGov:EmergencyAlertBanner ID="EmergencyAlertBanner" runat="server" />
    </div>
</div>