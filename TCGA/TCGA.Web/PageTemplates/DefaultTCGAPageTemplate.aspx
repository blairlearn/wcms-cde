<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="header" runat="server">
    <title></title>
</head>
<body  runat="server">
<div class="skip"><a href="#skiptocontent" title="Skip to content">Skip to content</a></div>
<!-- NCI Banner (please keep all code on one line for browsers spacing issue) -->
<NCI:TemplateSlot ID="tcgaSlotBrandingBar" runat="server" />
<!-- END NCI Banner --> 
 
<!-- Masthead (Logo, utility links, search) -->
<NCI:TemplateSlot ID="tcgaSlotSiteHeader" runat="server" />
<!-- END Masthead (Logo, utility links, search) --> 
 
<!-- Container for Content and Sidebar -->
<NCI:TemplateSlot ID="tcgaSlotBody" runat="server" />
  <!-- Sidebar -->
  <NCI:TemplateSlot ID="tcgaSlotRightSidebar" runat="server" />
  <!-- End Sidebar -->
<!-- END Container for Content and Sidebar -->
 
<!-- Footer -->
<NCI:TemplateSlot ID="tcgaSlotSiteFooter" runat="server" />
<!-- END Footer -->
</body>
</html>
