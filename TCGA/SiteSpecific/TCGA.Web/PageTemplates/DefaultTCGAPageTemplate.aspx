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
<style charset="utf-8" type="text/css" id="" rel="stylesheet"> @media screen {span.skype_pnh_print_container{    display:none !important;}span.skype_pnh_container, span.skype_pnh_container *{  background-attachment: scroll !important;  background-color: transparent !important;  background-image: none !important;  background-position: 0px 0px !important;  background-repeat: no-repeat !important;  border: 0px none !important;  border-style: none !important;  color: #49535A !important;  cursor: pointer !important;  direction: ltr !important;  display: inline !important;  float: none !important;  font-family: Tahoma, Arial, Helvetica, sans-serif !important;  font-size: 11px !important;  font-style: normal !important;  font-weight: bold !important;  height: 14px !important;  letter-spacing: 0 !important;  line-height: 14px !important;  margin: 0px 0px 0px 0px !important;  padding: 0px 0px 0px 0px !important;  position:static !important;  text-decoration: none !important;  text-indent:0px !important;  text-transform: none !important;  vertical-align: baseline !important;  white-space:nowrap !important;  word-spacing: normal !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_common *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_inactive_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_common *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_active_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_fax *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_inactive_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_fax *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_active_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_fax span.skype_pnh_right_span,span.skype_pnh_container span.skype_pnh_highlighting_inactive_fax span.skype_pnh_right_span{  background-position: -71px 0px !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free *{  background-image:url('chrome://skype_ff_extension/skin/numbers_free_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free span.skype_pnh_dropart_flag_span{  background-position: 0px 1px !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free span.skype_pnh_textarea_span{  background-position: -71px 0px !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free span.skype_pnh_text_span{  background-image: none !important;  color: transparent !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free *{  background-image:url('chrome://skype_ff_extension/skin/numbers_free_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free span.skype_pnh_dropart_flag_span{  background-position: 0px 1px !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free span.skype_pnh_textarea_span{  background-position: -135px 0px !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free span.skype_pnh_text_span{  background-position: -135px 0px !important;  color: #FFFFFF !important;}span.skype_pnh_container span.skype_pnh_left_span{  background-position: 0px 0px !important;  width: 6px !important;}span.skype_pnh_container span.skype_pnh_dropart_span{  background-position: -11px 0px !important;  width: 27px !important;}span.skype_pnh_container span.skype_pnh_dropart_wo_arrow_span{  background-position: -11px 0px !important;  width: 18px !important;}span.skype_pnh_container span.skype_pnh_dropart_flag_span{  background-image: url('chrome://skype_ff_extension/skin/flags.gif') !important;  background-position: 1px 1px !important;  width: 18px !important;}span.skype_pnh_container span.skype_pnh_textarea_span{  background-position: -90px 0px !important;}span.skype_pnh_container span.skype_pnh_text_span{  background-position: -90px 0px !important;}span.skype_pnh_container span.skype_pnh_right_span{  background-position: -52px 0px !important;  width: 15px !important;}} @media print {             span.skype_pnh_print_container{}span.skype_pnh_container{    display:none !important;} }                          span.skype_pnh_mark{  display:none !important;}</style></head>
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
<div id="container">
    <!-- Content -->
    <div id="content">
    <NCI:TemplateSlot ID="tcgaSlotBodyTop" runat="server" />
    <div class="content-sidebox">
    <NCI:TemplateSlot ID="tcgaSlotImageCaption" runat="server" />   
	<NCI:TemplateSlot ID="tcgaSlotMultimedia" runat="server" CssClass="multimedia-sidebox" /> 
    </div>
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
</body>
</html>
