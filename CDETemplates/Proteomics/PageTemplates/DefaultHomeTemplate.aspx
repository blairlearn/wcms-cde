<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>

<!DOCTYPE html>
<html>
<head runat="server">
<link rel="icon" href="/publishedcontent/files/shareditems/favicon/favicon.ico" type="image/x-icon" />
<link rel="shortcut icon" href="/publishedcontent/files/shareditems/favicon/favicon.ico" type="image/x-icon" />
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /></head>
<body class="genHome">
	<div class="genSiteSkipToContent"><a href="#genSiteContent">Skip to Content</a></div>
	<!-- Branding Bar Slot (#genSlotBrandingBar) // TODO: Color class on slot determined by Content Type field value  -->
	<!-- <div class="clearFix red" id="genSlotBrandingBar"> -->
	<NCI:TemplateSlot ID="genSlotBrandingBar" runat="server" class="clearFix" />
	<!-- </div> -->
	<!-- END Branding Bar Slot (#genSlotBrandingBar) -->
	<div class="genSiteContainer">
		<!-- Site Banner Slot (#genSlotSiteBanner) -->

		<!-- <div id="genSlotSiteBanner" class="clearFix"> -->
			<!-- Site Banner Content // From Content Type in Slot -->
			<NCI:TemplateSlot ID="genSlotSiteBanner" runat="server"  class="clearFix"/>
			<!-- Site Banner Content -->
		<!-- </div> END Site Banner Slot (#genSlotSiteBanner) -->
		<!-- <div id="genSlotMainNav" class="clearFix"> -->
			<NCI:TemplateSlot ID="genSlotMainNav" runat="server"  class="clearFix"/>
		<!-- </div> -->
		<!-- END Main Navigation Slot (#genSlotMainNav) -->
		<div class="genSiteContentContainer clearFix"><a name="skiptocontent" id="skiptocontent "></a> 
		
				<div class="genSiteContentColumn">
					<!-- Timely Content Slot (#genSlotTC) -->
					<!-- <div id="genSlotTC">  -->
						<NCI:TemplateSlot ID="genSlotTC" runat="server"  />
					<!-- </div> --> <!-- END Timely Content Slot (#genSlotTC) -->
					<!-- Body Slot (#genSlotBody) -->
					<!-- <div id="genSlotBody"> -->
						<NCI:TemplateSlot ID="genSlotBody" runat="server"  />
					<!-- </div> -->
					<!-- END Body Slot (#genSlotBody) -->
					<div class="genSlotColumnContainer clearFix">
					<NCI:TemplateSlot ID="genSlotColumn1" runat="server" />
					<NCI:TemplateSlot ID="genSlotColumn2" runat="server" />
					</div>
					<div class="genSiteHighlightContainer clearFix">
					<NCI:TemplateSlot ID="genSlotHighlight1" runat="server" />
					<NCI:TemplateSlot ID="genSlotHighlight2" runat="server" />
					<NCI:TemplateSlot ID="genSlotHighlight3" runat="server" />
					</div>
				</div>

				<div class="genSiteRightColumn">
					<NCI:TemplateSlot ID="genSlotRightSidebar" runat="server"  />
				</div><!-- END Right Content Column (#genSiteRightColumn) -->
		</div>
		<!-- Site Footer Slot (#genSlotSiteFooter) -->
		<!-- <div id="genSlotSiteFooter"> -->
			<NCI:TemplateSlot ID="genSlotSiteFooter" runat="server"  />
		<!-- </div> --><!-- END Site Footer Slot (#genSlotSiteFooter) -->
	</div><!-- END Site Container (#genSiteContainer) -->
	<!-- Javscript Configuration Content // At Bottom -->
	<!-- END Javascript Configuration Content // At Bottom -->
	    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
</body>
</html>