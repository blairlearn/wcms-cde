<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.EmergencyBanner" %>
<%@ Register tagPrefix="CGov" namespace="CancerGov.EmergencyAlert" assembly="CancerGov.EmergencyAlert" %>
<div class="site-notification">
    <div class="row">
        <CGov:EmergencyAlertBanner ID="EmergencyAlertBanner" runat="server" />
    </div>
</div>