<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls" TagPrefix="NCI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link rel="icon" href="/PublishedContent/Files/favicon.ico" type="image/x-icon"> 
<link rel="shortcut icon" href="/PublishedContent/Files/favicon.ico" type="image/x-icon">
<meta content="text/html; charset=utf-8" http-equiv="Content-Type"/>
<title>The Cancer Genome Atlas</title>
<!--<link rel="stylesheet" href="/stylesheets/tcga-print.css" type="text/css" media="print" />
<link rel="stylesheet" href="/stylesheets/jquery/jquery-ui-1.8.5.custom.css" type="text/css" media="all" />
<link rel="stylesheet" href="/Stylesheets/slimbox2.css" type="text/css" media="screen" />
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
<script src="/scripts/slimbox2.js" type="text/javascript"></script>
<script src="/scripts/jquery-ui-1.8.5.custom.min.js" type="text/javascript"></script> 
<script src="/scripts/jquery-widgets.js" type="text/javascript"></script>
<script src="/scripts/508player.js" type="text/javascript"></script>
-->
<script src="/JS/popEvents.js" type="text/javascript"></script>
</head>

<body marginwidth="0" marginheight="0" leftmargin="0" topmargin="0"> 
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
<div id="container">
    <!-- Content -->
    <div id="content">
    <a name="skiptocontent"></a>
    <NCI:TemplateSlot ID="tcgaSlotBodyTop" runat="server" />
    <!-- <div class="content-sidebox"> -->
    <NCI:TemplateSlot ID="tcgaSlotImageCaption" runat="server" />   
	<NCI:TemplateSlot ID="tcgaSlotMultimedia" runat="server" /> 
    <!-- </div> -->
    <NCI:TemplateSlot ID="tcgaSlotBody" runat="server" />
    <NCI:TemplateSlot ID="tcgaSlotCDERelatedMultimedia" runat="server" />
    <NCI:TemplateSlot ID="tcgaSlotCitation" runat="server" />
    <NCI:TemplateSlot ID="tcgaSlotBodyBottom" runat="server" />
    </div>
    <!-- END Content -->
    <!-- Sidebar -->
	<div id="sidebar">
	<NCI:TemplateSlot ID="tcgaSlotRelatedPages" runat="server"  />
	<NCI:TemplateSlot ID="tcgaSlotRightNav" runat="server"  />
	</div>
	<!-- END Sidebar -->
</div>
<!-- END Container for Content and Sidebar -->
<!-- Footer -->
<div id="footer">
<NCI:TemplateSlot ID="tcgaSlotSiteFooter" runat="server"  /> 
</div>
<!-- END Footer -->
<script type="text/javascript">
// code courtesy of http://webdeveloperplus.com/jquery/featured-content-slider-using-jquery-ui/
$(document).ready(function(){
$("#news-slider").tabs({fx:{opacity: "toggle"}}).tabs("rotate", 15000, true);
$("#news-slider").hover(
function() {
$("#news-slider").tabs("rotate", 0, true);
},
function() {
$("#news-slider").tabs("rotate", 15000, true);
}
);
});
</script>
    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
</body>
</html>
