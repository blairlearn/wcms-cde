<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancerTypeHomeTemplate.aspx.cs" Inherits="CancerGov.Web.PageTemplates.CancerTypeHomeTemplate" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
    <title></title>
    <script type="text/javascript" language="JavaScript" src="/scripts/imgEvents.js"></script>
    <!--[if IE]>
	    <link rel="Stylesheet" type="text/css" href="/stylesheets/emergency_IE.css" />
    <![endif]-->
</head>
<body id="Body1" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" runat="server">
    <!-- Site Banner -->
    <div id="bannerDiv" align="center">
        <NCI:TemplateSlot ID="cgvSiteBanner" runat="server" />
    </div>
    <div align="center">
    </div>
    <!-- Content Header -->
    <div id="headerzone" align="center">
        <NCI:TemplateSlot ID="cgvContentHeader" runat="server" />
        <NCI:TemplateSlot ID="cgvLanguageDate" runat="server" />
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
                         <NCI:TemplateSlot ID="cgvSectionNav" runat="server" CssClass="LeftNavSlot">
                            <HeaderHtml>
                                <tr>
                                    <td valign="top" align="left">                            
                            </HeaderHtml>
                            <FooterHtml>
                                    </td>
                                    <td valign="top">
                                        <img src="/images/spacer.gif" border="0" alt="" width="16" height="1" />
                                    </td>
                                </tr>                            
                            </FooterHtml>
                         </NCI:TemplateSlot>
                        <tr>
                            <td valign="top" align="left" width="164">
                                <NCI:TemplateSlot ID="cgvLeftNav" runat="server" CssClass="LeftNavSlot" />
                            </td>
                            <!-- So... there should be 6px rendered after each item. -->
                            <td valign="top">
                                <img src="/images/spacer.gif" border="0" alt="" width="16" height="1" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</body>
</html>
