<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="definition.aspx.cs" Inherits="TCGA.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
   <head id="header" runat="server">
      <script type="text/javascript" src="/PublishedContent/js/popEvents.js"></script>
      <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js" language="JavaScript"></script>
      <script src="/PublishedContent/js/jquery.jplayer.min.js" type="text/javascript"></script>
      <link rel="stylesheet" href="/PublishedContent/Styles/tcga.css" type="text/css" />	
   </head>
   <body class="popup-pages">
      <div class="popup">
         <%=strHeading%>
         <%=this.Content.Render()%>
      </div>
      <div class="row">
         <a href="Javascript:window.print();"><%=strSendPrinter%></a>
      </div>
   </body>
</html>