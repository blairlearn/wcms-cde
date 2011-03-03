<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DocTitleBlock.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.DocTitleBlock" %>
<asp:PlaceHolder ID="phWeb" runat="server" Visible="false">
<table width="751" cellspacing="0" cellpadding="0" border="0" bgcolor="#d4d9d9">
	<tbody>
		<tr>
			<td width="100%" valign="top" align="left"><table cellspacing="0" cellpadding="0" border="0">
					<tbody>
						<tr>
							<td colspan="2"><img width="1" height="4" border="0" alt="" src="/images/spacer.gif" /></td>
						</tr>
						<tr>
							<td><img width="8" height="1" border="0" alt="" src="/images/spacer.gif" /></td>
							<td><span class="document-title"><asp:Literal ID="litTitle" runat="server" /></span></td>
						</tr>
						<tr>
							<td colspan="2"><img width="1" height="4" border="0" alt="" src="/images/spacer.gif" /></td>
						</tr>
					</tbody>
			</table></td>
			<td valign="top" align="right"><img width="10" height="1" border="0" alt="" src="/images/spacer.gif" /></td>
			<td valign="top" bgcolor="#ffffff"><img width="1" height="1" border="0" alt="" src="/images/spacer.gif" /></td>
			<td valign="top" bgcolor="#d4d9d9" align="right"><asp:Image ID="imgImage" runat="server" AlternateText="" GenerateEmptyAlternateText="true" /></td>			
		</tr>
	</tbody>
</table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phPrint" runat="server" Visible="false">
    <table cellspacing="0" cellpadding="0" border="0" bgcolor="#ffffff" width="751">
        <tbody>
            <tr>
                <td align="left">
                    <table cellspacing="0" cellpadding="0" border="0" width="650">
                        <tbody>
                            <tr>
                                <td width="100%" valign="top">
                                    <span class="document-title">
                                        <table cellspacing="0" cellpadding="0" border="0" width="650">
                                            <tbody>
                                                <tr>
                                                    <td width="100%" valign="bottom">
                                                        <span class="document-title">
                                                            <asp:Literal ID="litPrintTitle" runat="server" />
                                                        </span>
                                                    </td>
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                                    </td>
                                                    <td nowrap="" valign="top">
                                                        <img height="5" border="0" width="1" alt="" src="/images/spacer.gif"><br />
                                                        <asp:Literal ID="litPrintDate" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td width="100%" valign="bottom" colspan="3">
                                                        <span class="document-title">
                                                            <asp:Literal ID="litAudienceTitle" runat="server" />
                                                        </span>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </span>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <img height="4" border="0" width="1" alt="" src="/images/spacer.gif">
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
        </tbody>
    </table>
    <table cellspacing="0" cellpadding="0" border="0" width="1">
        <tbody>
            <tr>
                <td>
                    <img height="16" border="0" width="1" alt="" src="/images/spacer.gif">
                </td>
            </tr>
        </tbody>
    </table>
</asp:PlaceHolder>
<asp:PlaceHolder ID="phPrintNoAudience" runat="server" Visible="false">
<div align="center">
		<table width="771" cellspacing="0" cellpadding="0" border="0">
			<tr>
<td valign="top"><img src="/images/spacer.gif" width="3" height="1" alt="" border="0"></td>
		
<td valign="top">

    <span class="page-title"><asp:Literal ID="litNoAudiencePrintTitle" runat="server" /></span><br />
    </td>
    </tr>
    </table>
    </div>
</asp:PlaceHolder>