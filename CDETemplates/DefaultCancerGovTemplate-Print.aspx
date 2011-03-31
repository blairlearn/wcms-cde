 <%@ Page Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.WebPageAssembler" %>
<%@ Register Assembly="NCILibrary.Web.ContentDeliveryEngine.UI" Namespace="NCI.Web.CDE.UI.WebControls"
    TagPrefix="NCI" %>
	
<%@ Register src="/SnippetTemplates/TableofLinks.ascx" tagname="TableofLinks" tagprefix="uc1" %>
	
<%@ Register src="/SnippetTemplates/GlossaryTerms.ascx" tagname="GlossaryTerms" tagprefix="uc2" %>
	
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="header" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <!--[if IE]>
	    <link rel="Stylesheet" type="text/css" href="/stylesheets/emergency_IE.css" />
    <![endif]-->
    
</head>
<body runat="server">
	<!-- CGov Container -->
    <div id="cgovContainer">
	<!-- Site Banner -->
<div class="skip">
        <a title="Skip to content" href="#skiptocontent">Skip to content</a></div>  
	
    <div id="bannerDiv" align="center">
        <NCI:TemplateSlot ID="cgvSiteBannerPrint" runat="server" />
    </div>
	
	<%--<div align="center"></div>--%>
 
	<!-- Content Header --> 
    <!-- Content Header -->
    <div id="headerzone" align="center">
<%--        <table cellspacing="0" cellpadding="0" border="0" width="1">
            <tbody>
                <tr>
                    <td>
                        <img height="10" border="0" width="1" alt="" src="/images/spacer.gif" />
                    </td>
                </tr>
            </tbody>
        </table>
        <table cellspacing="0" cellpadding="0" border="0" width="751">
            <tbody>
                <tr>
                    <td align="left">
                        <table cellspacing="0" cellpadding="0" border="0" width="650">
                            <tbody>
                                <tr>
                                    <td>
                                        <a class="navigation-dark-red-link" href="javascript:NCIAnalytics.SendToPrinterLink(this); window.print();">
                                            Send to Printer</a>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </tbody>
        </table>
--%>        <table cellspacing="0" cellpadding="0" border="0" width="1">
            <tbody>
                <tr>
                    <td>
                        <img height="10" border="0" width="1" alt="" src="/images/spacer.gif" />
                    </td>
                </tr>
            </tbody>
        </table>
        <NCI:TemplateSlot ID="cgvContentHeader" runat="server" />
        <NCI:TemplateSlot ID="cgvLanguageDate" runat="server" />
    </div>
    <!-- Main Area -->
    <!-- Left Navigation and Content Area -->
    <div id="mainContainer">
		<table width="751" cellspacing="0" cellpadding="0" border="0">
			<tr>		
			    
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
            <NCI:TemplateSlot ID="cgvMpToc" runat="server"  />
                    <NCI:TemplateSlot ID="cgvBody" runat="server"  />
			<!-- Glossary of Terms -->
 		    <uc2:GlossaryTerms ID="GlossaryTerms1" runat="server" />
                   
			<!-- Link Extraction -->
		    <uc1:TableofLinks ID="TableofLinks1" runat="server" />
		</td>
				<!----------------------->				

			</tr>
		</table>
	<!-- End Main Area -->
         </div>
    <!-- End Left Navigation and Content Area -->
	<!-- Footer -->
	<div id="footerzone" align="center"></div>
	     </div>
    <!-- End CGovContainer--> 
    <!-- TO INSERT WEB ANALYTICS CODE. Every template should have this 
    control else Web analytics scripts will not show up in the HTML-->
    <NCI:WebAnalyticsControl ID="WebAnalyticsControl1" runat="server" />
  
  </body>
</html>

