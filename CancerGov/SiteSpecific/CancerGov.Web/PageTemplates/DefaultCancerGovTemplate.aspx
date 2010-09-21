<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>
    
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
    <title></title>
    <script type="text/javascript" language="JavaScript" src="/scripts/imgEvents.js"></script>
</head>
<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" runat="server">
    <!-- Site Banner -->
    <div id="bannerDiv" align="center">
        <NCI:TemplateSlot ID="cgvSiteBanner" runat="server" />
    </div>
    <div align="center">
    </div>
    <!-- Content Header -->
    <div id="headerzone" align="center">
        <NCI:TemplateSlot ID="cgvContentHeader" runat="server" />
    </div>
    <!-- Main Area -->
    <!-- Main Area -->
    <div align="center">
        <table width="771" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td valign="top">
                    <img src="/images/spacer.gif" width="9" height="1" alt="" border="0" />
                </td>                
                <!-- Left Nav Column -->
                <td id="leftzone" valign="top">
                    <table border="0" cellspacing="0" cellpadding="0" width="164">
                         <tr>
                            <td valign="top" align="left">
                                <NCI:TemplateSlot ID="cgvSectionNav" runat="server" CssClass="LeftNavSlot" />
                            </td>
                            <td valign="top">
                                <img src="/images/spacer.gif" border="0" alt="" width="16" height="1" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" align="left">
                                <NCI:TemplateSlot ID="cgvLeftNav" runat="server" CssClass="LeftNavSlot" />
                            </td>
                            <!-- So... there should be 6px rendered after each item. -->
                            <td valign="top">
                                <img src="/images/spacer.gif" border="0" alt="" width="16" height="1" />
                            </td>
                        </tr>
                    </table>
                </td>
                <!-- End Left Nav -->
                <!-- Main Content Area -->
                <td id="contentzone" valign="top" width="571">
                    <a name="skiptocontent"></a>
                    <NCI:TemplateSlot ID="cgvBody" runat="server"  />
                </td>
                <!-- End Content Area -->
                <td valign="top">
                    <img src="/images/spacer.gif" width="10" height="1" alt="" border="0" />
                </td>
            </tr>
        </table>
    </div>
    <!-- End Main Area -->
    <!-- Footer -->
    <div id="footerzone" align="center">
        <NCI:TemplateSlot ID="cgvFooter" runat="server" RemoveIfEmpty="false" />
    </div>
    <!-- End Foooter-->
    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
</body>
</html>
