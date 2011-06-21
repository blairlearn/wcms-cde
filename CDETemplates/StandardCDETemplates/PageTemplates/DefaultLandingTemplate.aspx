<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>
<!DOCTYPE html>
<html>
<head runat="server"><meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /></head>

<body class="genLanding">
	<div class="genSiteSkipToContent"><a href="#genSiteContent">Skip to Content</a></div>
	<!-- Branding Bar Slot (#genSlotBrandingBar) // Color class on slot determined by Content Type field value  -->
	<NCI:TemplateSlot ID="genSlotBrandingBar" runat="server" class="clearFix" />
	<!-- END Branding Bar Slot (#genSlotBrandingBar) -->
	<div class="genSiteContainer">
		<!-- Site Banner Slot (#genSlotSiteBanner) -->

		<NCI:TemplateSlot ID="genSlotSiteBanner" runat="server"  class="clearFix"/>
		<!-- END Site Banner Slot (#genSlotSiteBanner) -->
		<!-- Main Navigation (#genSlotMainNav) -->
		<NCI:TemplateSlot ID="genSlotMainNav" runat="server"  class="clearFix"/>
		<!-- END Main Navigation Slot (#genSlotMainNav) -->
		<!-- Site Content Container (.genSiteContentContainer) -->

		<div class="genSiteContentContainer clearFix">
			<div class="genSiteLeftColumn">
				<!-- Section Navigation Slot (#genSlotSectionNav) -->
				<NCI:TemplateSlot ID="genSlotSectionNav" runat="server"/>
				<!-- END Section Navigation Slot (#genSlotSectionNav) -->

				<!-- Left Sidebar Slot (#genSlotLeftSidebar) -->
				<NCI:TemplateSlot ID="genSlotLeftSidebar" runat="server"/>
				<!-- END Left Sidebar Slot (#genSlotLeftSidebar) -->

			</div><!-- END Left Content Column (#genSiteLeftColumn) -->
			<!-- Main Column (.genSiteMainColumn) -->
			<div class="genSiteMainColumn clearFix">
				<!-- Section Banner Slot -->
				<NCI:TemplateSlot ID="genSlotSectionBanner" runat="server"/>
				<!-- END Section Banner Slot (#genSlotContentHeader) -->
				<div class="genSiteContentColumn">
					<!-- Content Title Slot // Includes Subtitle (#genSlotTitle) -->
					<NCI:TemplateSlot ID="genSlotTitle" runat="server"/>
					<!-- END Content Title Slot // Includes Subtitle (#genSlotTitle) -->
					<!-- Page Options (#genSlotPageOptions) -->
					<NCI:TemplateSlot ID="genSlotPageOptions" runat="server"/>
					<!-- END Page Options (#genSlotPageOptions) -->
					<!-- Timely Content Slot (#genSlotTC) -->
					<NCI:TemplateSlot ID="genSlotTC" runat="server"/>
					<!-- END Timely Content Slot (#genSlotTC) -->

					<!-- Body Slot (#genSlotBody) -->
					<NCI:TemplateSlot ID="genSlotBody" runat="server"  />
					<!-- END Body Slot (#genSlotBody) -->

					<!-- Column Slots Container (.genSlotColumnContainer) // If no content is put in to the Slots, then this div is not printed out. -->
					<div class="genSlotColumnContainer clearFix">
					<NCI:TemplateSlot ID="genSlotColumn1" runat="server" />
					<NCI:TemplateSlot ID="genSlotColumn2" runat="server" />
					</div>
					<div class="genSiteHighlightContainer clearFix">
					<NCI:TemplateSlot ID="genSlotHighlight1" runat="server" />
					<NCI:TemplateSlot ID="genSlotHighlight2" runat="server" />
					<NCI:TemplateSlot ID="genSlotHighlight3" runat="server" />
					</div>
					<!-- END Highlight Slots Container (.genSiteHighlightContainer) -->
					
					<NCI:TemplateSlot ID="genSlotBodyBottom" runat="server"  />
				</div>
				<div class="genSiteRightColumn">
				<NCI:TemplateSlot ID="genSlotRightSidebar" runat="server"  />
				</div><!-- END Right Content Column (#genSiteRightColumn) -->
			</div><!-- END Main Content Column (#genSiteMainColumn) -->
		</div>
		<!-- Site Footer Slot (#genSlotSiteFooter) -->
		<NCI:TemplateSlot ID="genSlotSiteFooter" runat="server"  />
		<!-- END Site Footer Slot (#genSlotSiteFooter) -->
	</div><!-- END Site Container (#genSiteContainer) -->
	<!-- Javscript Configuration Content // At Bottom -->
	<!-- END Javascript Configuration Content // At Bottom -->
	    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
</body>
</html>