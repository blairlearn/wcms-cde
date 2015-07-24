<%@ Page language="c#" debug="true" Codebehind="popDefinition.aspx.cs" AutoEventWireup="True" Inherits="Www.Common.PopUps.PopDefinition" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Frameset//EN">
<HTML>
  <HEAD id="header" runat="server">
	<TITLE>Dictionary of Cancer Terms</TITLE>
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
	<!--[if lt IE 9]>
        <script src="/PublishedContent/js/respond.js"></script>
    <![endif]-->
  </HEAD>
  <FRAMESET border=0 frameSpacing=0 rows="80,*" frameBorder=no>
	<FRAME name="Header" title="Header" src="/Common/PopUps/<%=Header%>" noResize scrolling=no>
	<FRAME name="Definition" title="Definition" src="/Common/PopUps/definition.aspx?<%=UrlArgs%>" noResize>
	<!--
	<FRAME name="Footer" title="Footer" src="/Common/PopUps/<%=Footer%>" noResize scrolling=no> 
	-->
  </FRAMESET>
</HTML>
