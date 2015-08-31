<%@ Page language="c#" debug="true" Codebehind="PopEmail.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.PopEmail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Frameset//EN">
<HTML>
  <HEAD id="header" runat="server">
	<TITLE><%=BrowserTitle%></TITLE>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	<script type="text/javascript">
	    var callingUrl = opener.location.href;
	</script>
	<link rel="stylesheet" href="/PublishedContent/Styles/nci.css" type="text/css">
    <!--[if lt IE 9]>
        <script src="/PublishedContent/js/respond.js"></script>
    <![endif]-->
  </HEAD>
  <FRAMESET border=0 frameSpacing=0 rows=80,*,26 frameBorder=no>
	<FRAME name="Header" title="Header" src="/Common/PopUps/<%=Header%>" noResize scrolling=no>
	<FRAME name="Email" title="Email" src="/Common/PopUps/Email.aspx<%=Request.Url.Query%>" noResize>
	<FRAME name="Footer" title="Footer" src="/Common/PopUps/<%=Footer%>" noResize scrolling=no>
  </FRAMESET>
</HTML>
