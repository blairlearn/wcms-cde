<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>
<%@ Register tagPrefix="CGov" namespace="CancerGov.EmergencyAlert" assembly="CancerGov.EmergencyAlert" %>    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script type="text/javascript" language="JavaScript" src="/scripts/imgEvents.js"></script>
    <script type="text/javascript" language="JavaScript" src="/JS/NetTracker/ntpagetag.js"></script>
    <script src="/JS/popEvents.js" type="text/javascript"></script>
    <!--[if IE]>
	    <link rel="Stylesheet" type="text/css" href="/stylesheets/emergency_IE.css" />
    <![endif]-->
</head>
<body runat="server">
	<!-- CGov Container -->
    <div id="cgovContainer">
    <!-- Site Banner -->
    <NCI:TemplateSlot ID="cgvSiteBanner" runat="server" />
    <CGov:EmergencyAlertBanner ID="EmergencyAlertBanner" runat="server" />    
    <!-- End Site Banner -->
    <!-- Content Header -->
    <div id="headerzone">
        <NCI:TemplateSlot ID="cgvContentHeader" runat="server" />
        <NCI:TemplateSlot ID="cgvLanguageDate" runat="server" />
    </div>
    <!-- Main Area -->
    <!-- Left Navigation and Content Area -->
    <div id="mainContainer">
        <table width="751" cellspacing="0" cellpadding="0" border="0">
            <tr>                            
                <!-- Left Nav Column -->
                <td id="leftzone" valign="top">
                    <table border="0" cellspacing="0" cellpadding="0" width="164">
                         <NCI:TemplateSlot ID="cgvSectionNav" runat="server" CssClass="LeftNavSlot">
                            <HeaderHtml>
                                <tr>
                                    <td valign="top" align="left">                            
                            </HeaderHtml>
                            <FooterHtml>
                                    </td>
                                    <!--<td valign="top">
                                        <img src="/images/spacer.gif" border="0" alt="" width="16" height="1" />
                                    </td> -->
                                </tr>                            
                            </FooterHtml>
                         </NCI:TemplateSlot>
                        <tr>
                            <td valign="top" align="left" width="164">
                                <NCI:TemplateSlot ID="cgvLeftNav" runat="server" CssClass="LeftNavSlot" />
                            </td>
                            <!-- So... there should be 6px rendered after each item. 
                            <td valign="top">
                                <img src="/images/spacer.gif" border="0" alt="" width="16" height="1" />
                            </td> -->
                        </tr>
                    </table>
                </td>
                <!-- End Left Nav -->                
                <!-- Main Content Area -->
                <td id="contentzone" valign="top" width="100%">
                    <a name="skiptocontent"></a>                 
                    <!-- Parent container for content and timely content zone column -->
                    <div id="portalPageContentContainer">                       
                    <NCI:TemplateSlot ID="cgvBodyHeader" runat="server" CssClass="BodyHeaderSlot"/>  
                    <NCI:TemplateSlot ID="cgvSlotTimelyContentItem" runat="server" CssClass="TimelyContentSlot" />        
                     <!-- Tile zone column -->
                    <NCI:TemplateSlot ID="cgvTileSlot" runat="server" CssClass="TileSlot" />
                    <!-- End Tile zone column -->
                    <NCI:TemplateSlot ID="cgvBody" CssClass="BodySlotPortal" runat="server"  />                                     
                    </div>
                    <!-- End Parent container for content and timely content zone column -->
                </td>
                <!-- End Content Area -->               
            </tr>
        </table>
        </div>
    <!-- End Left Navigation and Content Area -->
    <!-- End Main Area -->
    <!-- Footer -->
    <div id="footerzone">
        <NCI:TemplateSlot ID="cgvFooter" runat="server" RemoveIfEmpty="false" />
    </div>    
    <!-- End Foooter-->
     </div>
    <!-- End CGovContainer--> 
    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
</body>
</html>
