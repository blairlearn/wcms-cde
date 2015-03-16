<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="definition.aspx.cs" Inherits="TCGA.Web.Common.PopUps.Definition" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD id="header" runat="server">
	
  </HEAD>
	<body>
	    <div class="popup">
			<%=strHeading%>
			<%=this.Content.Render()%>
		</div>
        <div class="row">
    		<a href="Javascript:window.print();"><%=strSendPrinter%></a>
		</div>
	<asp:Literal ID="litOmniturePageLoad" mode="PassThrough" runat="server" />
  </body>
</HTML>
