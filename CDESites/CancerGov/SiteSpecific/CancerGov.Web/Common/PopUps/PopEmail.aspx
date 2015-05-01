<%@ Page language="c#" debug="true" Codebehind="PopEmail.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.PopEmail" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Frameset//EN">
<HTML>
  <HEAD id="header" runat="server">
	<TITLE><%=BrowserTitle%></TITLE>
		<script type="text/javascript">
		    var callingUrl = opener.location.href;
		</script>
		<link rel="stylesheet" href="/PublishedContent/Styles/nci.css" type="text/css">

  </HEAD>
  <FRAMESET border=0 frameSpacing=0 rows=55,*,26 frameBorder=no>
	<FRAME name="Header" title="Header" src="/Common/PopUps/<%=Header%>" noResize scrolling=no>
	<FRAME name="Email" title="Email" src="/Common/PopUps/Email.aspx<%=Request.Url.Query%>" noResize>
	<FRAME name="Footer" title="Footer" src="/Common/PopUps/<%=Footer%>" noResize scrolling=no>
  </FRAMESET>
</HTML>
