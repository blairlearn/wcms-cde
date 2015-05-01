<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popDefinition.aspx.cs" Inherits="TCGA.Web.Common.PopUps.PopDefinition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Frameset//EN">
<HTML>
  <HEAD id="header" runat="server">
	<TITLE>Dictionary of Cancer Terms</TITLE>
  </HEAD>
  <FRAMESET border=0 frameSpacing=0 rows="60,*" frameBorder=no>
	<FRAME name="Header" title="Header" src="/Common/PopUps/<%=Header%>" noResize scrolling=no>
	<FRAME name="Definition" title="Definition" src="/Common/PopUps/definition.aspx?<%=UrlArgs%>" noResize>
	<!--
	<FRAME name="Footer" title="Footer" src="/Common/PopUps/<%=Footer%>" noResize scrolling=no>
	-->
  </FRAMESET>
</HTML>
