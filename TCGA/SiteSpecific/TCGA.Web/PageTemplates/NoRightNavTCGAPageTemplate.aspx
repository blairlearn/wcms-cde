<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta content="text/html; charset=utf-8" http-equiv="Content-Type"/>
<title>The Cancer Genome Atlas</title>
<link rel="stylesheet" href="/stylesheets/tcga-print.css" type="text/css" media="print" />
<link rel="stylesheet" href="/stylesheets/jquery/jquery-ui-1.8.5.custom.css" type="text/css" media="all" />
<link rel="stylesheet" href="/Stylesheets/slimbox2.css" type="text/css" media="screen" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script src="/scripts/slimbox2.js" type="text/javascript"></script>
<script src="/scripts/508player.js" type="text/javascript"></script>
<script src="/JS/popEvents.js" type="text/javascript">
</head>
<body marginwidth="0" marginheight="0" leftmargin="0" topmargin="0"> 
<script src="/scripts/jquery-ui-1.8.5.custom.min.js" type="text/javascript"></script> 
<script src="/scripts/jquery-widgets.js" type="text/javascript"></script>
<div class="skip"><a title="Skip to content" href="#skiptocontent">Skip to content</a></div>
<!-- NCI Banner (please keep all code on one line for browsers spacing issue) -->
<div id="nci-banner">
<NCI:TemplateSlot ID="tcgaSlotBrandingBar" runat="server"  />
</div>
<!-- END NCI Banner -->

<!-- Masthead (Logo, utility links, search) -->
<div id="masthead">
<NCI:TemplateSlot ID="tcgaSlotSiteHeader" runat="server"  />
</div>
<!-- END Masthead (Logo, utility links, search) -->

<div id="mainnav">
<NCI:TemplateSlot ID="tcgaSlotMainNav" runat="server"  />
</div>
<!-- END Main Navigation -->
<!-- Container for Content and Sidebar -->
<div id="container-sans-sidebar">
    <!-- Content -->
    <div id="content">
    <NCI:TemplateSlot ID="tcgaSlotBodyTop" runat="server" />
    <NCI:TemplateSlot ID="tcgaSlotImageCaption" runat="server" CssClass="content-sidebox" />
    <NCI:TemplateSlot ID="tcgaSlotBody" runat="server" />
    <NCI:TemplateSlot ID="tcgaSlotCitation" runat="server" />
    <NCI:TemplateSlot ID="tcgaSlotBodyBottom" runat="server" />
    </div>
    <!-- END Content --> 
</div>
<!-- END Container for Content and Sidebar -->
<!-- Footer -->
<div id="footer">
<NCI:TemplateSlot ID="tcgaSlotSiteFooter" runat="server"  />
</div>
<!-- END Footer -->
</body>
</html>
