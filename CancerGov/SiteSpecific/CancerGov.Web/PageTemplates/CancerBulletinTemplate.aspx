<%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>

<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
    <title></title>
    <style>
        .GoodText
        {
            font-weight: bold;
            color: #484bd0;
            font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
        }
        .BadText
        {
            font-weight: bold;
            color: #a8364d;
            font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
        }
        .InfoText
        {
            font-weight: bold;
            font-size: 19px;
            color: #333367;
            font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
        }
        .HeaderText
        {
            font-family: Times New Roman, Serif;
            font-size: 19px;
            color: #C5080B;
        }
    </style>

    <script type="text/javascript" language="JavaScript" src="/scripts/imgEvents.js"></script>

    <script src="/JS/popEvents.js" type="text/javascript"></script>

    <!--[if IE]>
	    <link rel="Stylesheet" type="text/css" href="/stylesheets/emergency_IE.css" />
    <![endif]-->
</head>
<body id="Body1" leftmargin="0" topmargin="0" marginwidth="0" marginheight="0" runat="server">
    <!-- Site Banner -->
    <div class="skip">
        <a title="Skip to content" href="#skiptocontent">Skip to content</a></div>
    <div align="center">
        <div class="bulletin-header">
            <a href="/">
                <img src="/images/cancerbulletin/cancerbulletin-header-nci.gif" width="112" height="85"
                    alt="National Cancer Institute" border="0" style="float: left;"></a> <a href="/ncicancerbulletin/cancerbulletin">
                        <img src="/images/cancerbulletin/cancerbulletin-header-bulletin.gif" width="306"
                            height="85" alt="NCI Cancer Bulletin: A Trusted Source for Cancer Research News"
                            border="0" style="float: right;"></a>
        </div>
        <NCI:TemplateSlot ID="cgvBody" runat="server" />
    </div>
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
