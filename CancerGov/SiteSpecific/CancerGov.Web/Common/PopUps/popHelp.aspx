<%@ Page language="c#" Codebehind="popHelp.aspx.cs" AutoEventWireup="True" Inherits="www.Common.PopUps.popHelp" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Frameset//EN">
<HTML>
  <HEAD  id="header" runat="server">
	<TITLE>NCI Drug Dictionary</TITLE>
	<link rel="stylesheet" href="/stylesheets/nci.css" type="text/css">

  </HEAD>
  <FRAMESET border=0 frameSpacing=0 rows=55,*,26 frameBorder=no>
	<FRAME name="Header" title="Header" src="/Common/PopUps/<%=Header%>" noResize scrolling=no>
	<FRAME name="Definition" title="Definition" src="/Common/PopUps/popHelp.html" noResize>
	<FRAME name="Footer" title="Footer" src="/Common/PopUps/<%=Footer%>" noResize scrolling=no>
  </FRAMESET>
</HTML>
