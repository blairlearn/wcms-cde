<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>
<%--<%@ Register tagPrefix="CGov" namespace="CancerGov.EmergencyAlert" assembly="CancerGov.EmergencyAlert" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
<title></title>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<!-- This is to make content width follow different rules for IE7 and below -->
<!--[if lt IE 8]>
<style type="text/css">
.contentzone-defaultTemplateContentContainer{
	width: 650px !important;
	padding-right: 50px !important;
}
.cgvBody-defaultTemplateContentContainer {
	padding-right: 0 !important;
  width: auto !important;
}
ul.ctpListPageList {
	width: 400px !important;
}
</style>
<![endif]-->
</head>
<body id="Body1" runat="server">
<!-- CGov Container -->
<div id="cgovContainer"> 
  <!-- Site Banner -->
  <div class="skip"><a title="Skip to content" href="#skiptocontent">Skip to content</a></div>
  <NCI:TemplateSlot ID="cgvSiteBanner" runat="server" />
  <NCI:TemplateSlot ID="cgvMainNav" runat="server" />
  <%--<CGov:EmergencyAlertBanner ID="EmergencyAlertBanner" runat="server" />--%>
  <!-- Content Header -->
  <div id="headerzone">
    <NCI:TemplateSlot ID="cgvContentHeader" runat="server" />
    <NCI:TemplateSlot ID="cgvLanguageDate" runat="server" />
  </div>
  <!-- Main Area --> 
  <!-- Left Navigation and Content Area -->
  <div id="mainContainer"> 
    <!-- Left Nav Column -->
    <div class="leftzone">
      <NCI:TemplateSlot ID="cgvFindACancerTypeSlot" runat="server" CssClass="LeftNavSlot" />
      <NCI:TemplateSlot ID="cgvSectionNav" runat="server" CssClass="LeftNavSlot" />      
      <NCI:TemplateSlot ID="cgvLeftNav" runat="server" CssClass="LeftNavSlot" />
    </div>
    <!-- End Left Nav --> 
    <!-- Main Content Area -->
    <div class="contentzone contentzone-defaultTemplateContentContainer"> 
    	<a name="skiptocontent"></a>
      <NCI:TemplateSlot ID="cgvPublicArchiveBannerSl" runat="server" />
      <NCI:TemplateSlot ID="cgvBodyHeader" runat="server"  />
      <NCI:TemplateSlot ID="cgvRightNav" runat="server" CssClass="RightNavSlot" />
      <NCI:TemplateSlot ID="cgvMpToc" runat="server"  />
      <NCI:TemplateSlot ID="cgvBody" CssClass="cgvBody-defaultTemplateContentContainer" runat="server"  />
      <NCI:TemplateSlot ID="cgvBodyNav" runat="server" />
      <!-- Begin Public Use Slot -->
      <NCI:TemplateSlot ID="cgvPublicUseNoticeSl" runat="server" />
      <!-- End Public Use Slot -->
    </div>
    <!-- End Content Area --> 
  </div>
  <!-- End Left Navigation and Content Area --> 
  <!-- End Main Area --> 
  <!-- Footer -->
  <NCI:TemplateSlot ID="cgvFooter" runat="server" RemoveIfEmpty="false" />
  <!-- End Foooter--> 
</div>
<!-- End CGovContainer--> 
<!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
<NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
</body>
</html>