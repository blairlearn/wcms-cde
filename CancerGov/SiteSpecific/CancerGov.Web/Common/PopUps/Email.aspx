<%@ Page language="c#" trace="false" Codebehind="Email.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.Email" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
  <HEAD id="header" runat="server">
  </HEAD>
	<body leftmargin="0" topmargin="0" marginwidth="0" marginheight="0">
	<div align="center">
		<p>
		<table border="0" cellpadding="10" cellspacing="0" width="100%"><TBODY>
			<tr>
				<td align="center" valign="top">
				<div id="formDiv" runat="server">
					<form id="emailForm" method="post" runat="server">
						<input type="hidden" id="Url" runat="server">
						<input type="hidden" id="Document" runat="server" NAME="Document">
						
						<asp:Label ID="Title" Runat="server" CssClass="black-text-b"></asp:Label>
						<p>
						<table cellpadding="0" cellspacing="0" border="0">
							<tr valign="middle">
								<td align="right" valign="middle" nowrap><label for="To"><%=strSendtoEmail%></label></td>
								<td align="left" valign="middle"><input type="text" id="To" runat="server"></td><td><asp:RequiredFieldValidator ID="toValid" ControlToValidate="To" ErrorMessage=" (required valid e-mail)" Runat="server" EnableClientScript="True"></asp:RequiredFieldValidator></td>
							</tr>
							<tr><td>&nbsp;</td></tr>
							<tr valign="middle">
								<td align="right" valign="middle" nowrap><label for="From"><%=strFromEmail%></label></td>
								<td align="left" valign="middle"><input type="text" id="From" runat="server"></td><td><asp:RequiredFieldValidator ID="fromValid" ControlToValidate="From" ErrorMessage=" (required valid e-mail)" Runat="server" EnableClientScript="True"></asp:RequiredFieldValidator></td>
							</tr>
							<tr><td>&nbsp;</td></tr>
							<tr valign="middle">
								<td align="right" valign="middle" nowrap><label for="FromName"><%=strName%></label></td>
								<td align="left" valign="middle"><input type="text" id="FromName" runat="server"></td><td>&nbsp;</td>
							</tr>
						</table>
						<p>
						<input type="submit" value="<%=strSend%>">
					</form>
				</div>
				<div id="confirmDiv" runat="server">
				</div>
					<%=strConfirm%>
				</div>
				</TD>
			</TR>
		</TABLE>
		<p>
	</div>
  </body>
</HTML>
