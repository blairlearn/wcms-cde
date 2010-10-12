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
        <NCI:TemplateSlot ID="cgvSiteBannerPrint" runat="server" />
    </div>
	
	<div align="center"></div>
 
	<!-- Content Header -->
    <!-- Content Header -->
    <div id="headerzone" align="center">
        <table cellspacing="0" cellpadding="0" border="0" width="751"><tbody><tr><td align="left"><table cellspacing="0" cellpadding="0" border="0" width="650"><tbody><tr><td><a class="navigation-dark-red-link" href="javascript:NCIAnalytics.SendToPrinterLink(this); window.print();">Send to Printer</a></td></tr></tbody></table></td></tr></tbody></table>
        <table cellspacing="0" cellpadding="0" border="0" width="1"><tbody><tr><td><img height="10" border="0" width="1" alt="" src="/images/spacer.gif"/></td></tr></tbody></table>
        <NCI:TemplateSlot ID="cgvContentHeader" runat="server" />
        <NCI:TemplateSlot ID="cgvLanguageDate" runat="server" />
    </div>
    <!-- Main Area -->
	<div align="center">
		<table width="771" cellspacing="0" cellpadding="0" border="0">
			<tr>		
			    <td valign="top"><img src="/images/spacer.gif" width="9" height="1" alt="" border="0"></td>
				
				<!-- Left Nav Column -->
				<td id="leftzone" valign="top"></td>
				<!----------------------->
				
				<!-- Red line -->
				
				<!-- Main Content Area -->
				<td id="contentzone" valign="top" width="100%"><a name="skiptocontent"></a><table width="650" cellspacing="0" cellpadding="0" border="0">
	<tr>
		<td valign="top">
            <NCI:TemplateSlot ID="cgvBodyHeader" runat="server"  />
                <NCI:TemplateSlot ID="cgvRightNav" runat="server">
                    <HeaderHtml>
                        <table cellspacing="0" cellpadding="0" border="0" align="right" width="167">
                            <tbody>
                                <tr>
                                    <td valign="top"><img height="1" border="0" width="8" alt="" src="/images/spacer.gif"/></td>
                                    <td width="159" valign="top">
                    </HeaderHtml>
                    <FooterHtml>
                                    </td>
                                </tr> 
                                <tr>
		                            <td valign="top" colspan="2"><img height="8" border="0" width="1" alt="" src="/images/spacer.gif"/></td>
	                            </tr>                                
                            </tbody>
                        </table>                        
                    </FooterHtml>
                </NCI:TemplateSlot>
            <NCI:TemplateSlot ID="cgvBody" runat="server"  />
			<!-- Glossary of Terms -->
			<!-- Link Extraction -->
		</td>
				<!----------------------->				
				<td valign="top"><img src="/images/spacer.gif" width="10" height="1" alt="" border="0"></td>
			</tr>
		</table>
	</div>
	<!-- End Main Area -->
 
	<!-- Footer -->
	<div id="footerzone" align="center"></div>
	
    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
  
  </body>
</html>

