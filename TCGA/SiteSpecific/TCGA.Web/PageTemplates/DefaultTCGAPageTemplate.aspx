<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta content="text/html; charset=utf-8" http-equiv="Content-Type">
<title>The Cancer Genome Atlas</title>
<script src="/scripts/javascript/jquery/jquery-1.4.2.min.js" type="text/javascript"></script> 
<script src="/scripts/javascript/jquery/jquery-ui-1.8.5.custom.min.js" type="text/javascript"></script> 
<link media="all" type="text/css" href="http://htmldev.cancer.gov/tcga/stylesheets/tcga.css" rel="stylesheet">
<link media="all" type="text/css" href="/stylesheets/jquery/jquery-ui-1.8.5.custom.css" rel="stylesheet">
<style charset="utf-8" type="text/css" id="" rel="stylesheet"> @media screen {span.skype_pnh_print_container{    display:none !important;}span.skype_pnh_container, span.skype_pnh_container *{  background-attachment: scroll !important;  background-color: transparent !important;  background-image: none !important;  background-position: 0px 0px !important;  background-repeat: no-repeat !important;  border: 0px none !important;  border-style: none !important;  color: #49535A !important;  cursor: pointer !important;  direction: ltr !important;  display: inline !important;  float: none !important;  font-family: Tahoma, Arial, Helvetica, sans-serif !important;  font-size: 11px !important;  font-style: normal !important;  font-weight: bold !important;  height: 14px !important;  letter-spacing: 0 !important;  line-height: 14px !important;  margin: 0px 0px 0px 0px !important;  padding: 0px 0px 0px 0px !important;  position:static !important;  text-decoration: none !important;  text-indent:0px !important;  text-transform: none !important;  vertical-align: baseline !important;  white-space:nowrap !important;  word-spacing: normal !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_common *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_inactive_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_common *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_active_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_fax *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_inactive_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_fax *{  background-image:url('chrome://skype_ff_extension/skin/numbers_common_active_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_fax span.skype_pnh_right_span,span.skype_pnh_container span.skype_pnh_highlighting_inactive_fax span.skype_pnh_right_span{  background-position: -71px 0px !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free *{  background-image:url('chrome://skype_ff_extension/skin/numbers_free_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free span.skype_pnh_dropart_flag_span{  background-position: 0px 1px !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free span.skype_pnh_textarea_span{  background-position: -71px 0px !important;}span.skype_pnh_container span.skype_pnh_highlighting_inactive_free span.skype_pnh_text_span{  background-image: none !important;  color: transparent !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free *{  background-image:url('chrome://skype_ff_extension/skin/numbers_free_icon_set.gif') !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free span.skype_pnh_dropart_flag_span{  background-position: 0px 1px !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free span.skype_pnh_textarea_span{  background-position: -135px 0px !important;}span.skype_pnh_container span.skype_pnh_highlighting_active_free span.skype_pnh_text_span{  background-position: -135px 0px !important;  color: #FFFFFF !important;}span.skype_pnh_container span.skype_pnh_left_span{  background-position: 0px 0px !important;  width: 6px !important;}span.skype_pnh_container span.skype_pnh_dropart_span{  background-position: -11px 0px !important;  width: 27px !important;}span.skype_pnh_container span.skype_pnh_dropart_wo_arrow_span{  background-position: -11px 0px !important;  width: 18px !important;}span.skype_pnh_container span.skype_pnh_dropart_flag_span{  background-image: url('chrome://skype_ff_extension/skin/flags.gif') !important;  background-position: 1px 1px !important;  width: 18px !important;}span.skype_pnh_container span.skype_pnh_textarea_span{  background-position: -90px 0px !important;}span.skype_pnh_container span.skype_pnh_text_span{  background-position: -90px 0px !important;}span.skype_pnh_container span.skype_pnh_right_span{  background-position: -52px 0px !important;  width: 15px !important;}} @media print {             span.skype_pnh_print_container{}span.skype_pnh_container{    display:none !important;} }                          span.skype_pnh_mark{  display:none !important;}</style></head>
<body marginwidth="0" marginheight="0" leftmargin="0" topmargin="0"> 
<div class="skip"><a title="Skip to content" href="#skiptocontent">Skip to content</a></div>
<!-- NCI Banner (please keep all code on one line for browsers spacing issue) -->
<div id="nci-banner">
<NCI:TemplateSlot ID="tcgaSlotBrandingBar" runat="server"  />
</div>
<!-- END NCI Banner -->

<!-- Masthead (Logo, utility links, search) -->
<div id="masthead">
<NCI:TemplateSlot ID="tcgaSlotHeader" runat="server"  />
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
    <NCI:TemplateSlot ID="tcgaBody" runat="server"  />
    </div>
    <!-- END Content -->
    <!-- Sidebar -->
	<div id="sidebar">
	<NCI:TemplateSlot ID="tcgaSlotRightNav" runat="server"  />
	</div>
	<!-- END Sidebar -->
</div>
<!-- END Container for Content and Sidebar -->
<!-- Footer -->
<div id="footer">
<NCI:TemplateSlot ID="tcgaSlotFooter" runat="server"  />
</div>
<!-- END Footer -->
</body>
</html>
