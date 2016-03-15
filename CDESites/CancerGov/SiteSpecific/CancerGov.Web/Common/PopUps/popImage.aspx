<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popImage.aspx.cs" Inherits="Www.Common.PopUps.popImage" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
	<META http-equiv="content-type" content="text/html;charset=ISO-8859-1">
    <meta name="robots" content="noindex, nofollow">
    <link rel="stylesheet" href="/PublishedContent/Styles/nci.css" type="text/css">
    <!--[if lt IE 9]>
        <script src="/PublishedContent/js/respond.js"></script>
    <![endif]-->
    <title>National Cancer Institute</title>
  </head>
  <body style="margin:0 0 0 0; padding:0 0 0 0"> 
  <table border="0" cellpadding="0" cellspacing="0" width="750" height="600" align="center">
  <% if (Caption!="") { %>
  <tr><td valign="middle" align="center">
    <table border="0" cellpadding="0" cellspacing="0">
    
    <tr><td align="center">
		<div class="caption-image">
		<table border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td ><img border="0" src="<%=ImageName%>" alt="" /></td>
			</tr>
			<tr><td valign="top">
				<img src="/images/spacer.gif" border="0" height="10" width="12" alt="">
			</td></tr>
		</table>
		</div>
    </td></tr>
    <tr><td valign="top">
		<img src="/images/spacer.gif" border="0" height="3" width="12"  alt="">
	</td></tr>
	
	<tr><td class="caption" align="left" valign="top"><p><%=Caption%></p></td></tr>	
	
	</td></tr>
	</table>
	
 <% }else{ %>
		<tr>
			<td valign="middle" align="center"><img border="0" src="<%=ImageName%>"  alt="" /></td>
		</tr>
 <% } %>
  </table>	
  </body>
</html>
