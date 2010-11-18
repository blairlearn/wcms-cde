﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancerBulletinUnSubscribe.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.CancerBulletin.CancerBulletinUnSubscribe" %>
<table width="771" cellspacing="0" cellpadding="0" border="0" style="margin-top: 5px;">
    <tr>
        <td valign="top" height="100%">
            <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%" valign="top">
                <tr>
                    <td valign="top" height="61">
                        <table width="100%" cellspacing="0" cellpadding="20" border="0" bgcolor="#E1E0DE"
                            id="tblImgHeader" runat="server">
                            <tr>
                                <td valign="top" width="771" style="color: #b50000; height: 61px; vertical-align: baseline;
                                    font-family: Arial; font-weight: bold; font-size: 22px;">
                                    Unsubscribe from the <i>NCI Cancer Bulletin</i>
                                </td>
                            </tr>
                        </table>
                        <table width="100%" bgcolor="#cddeee" cellpadding="0" border="0" cellspacing="0"
                            runat="server" id="tblTextHeader">
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <!-- Start of questionable content cut -->
                    <td valign="top">
                        <a name="skiptocontent"></a>
                        <table cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td width="30">
                                </td>
                                <td valign="top">
                                    <br />
                                    <div class="HeaderText" style="color: #4d4d4d; font-family: Arial;">
                                        <%=strHeader%></div>
                                    <br>
                                    <form name="frmUnSubscribe" id="frmUnSubscribe" method="post" runat="server">
                                    <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#C3C2C2">
                                        <tr>
                                            <td valign="top">
                                                <table width="100%" cellspacing="0" cellpadding="20" border="0" bgcolor="#FFFFFF">
                                                    <tr>
                                                        <td valign="top" width="14">
                                                            <img src="/images/spacer.gif" width="14" height="1" alt="" border="0">
                                                        </td>
                                                        <td valign="top" width="100%">
                                                            <%=strMessageBody%>
                                                        </td>
                                                        <td valign="top" width="22">
                                                            <img src="/images/spacer.gif" width="22" height="1" alt="" border="0">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </form>
                                    <br>
                                    <!-- Send Feedback box -->
                                    <table id="tblFeedback" width="100%" cellspacing="0" cellpadding="0" border="0" runat="Server">
                                        <tr>
                                            <td valign="top">
                                                <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                    <tr>
                                                        <td valign="top" width="14">
                                                            <img src="/images/spacer.gif" width="14" height="23" alt="" border="0">
                                                        </td>
                                                        <td valign="top" width="443" align="left">
                                                            <img src="/images/spacer.gif" width="1" height="2" alt="" border="0"><br>
                                                            <span style="font-family: Arial; font-size: 18px;">Send us your feedback</span>
                                                        </td>
                                                        <td valign="top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF">
                                                    <tr>
                                                        <td valign="top" colspan="3">
                                                            <img src="/images/spacer.gif" width="1" height="20" alt="" border="0">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" width="14">
                                                            <img src="/images/spacer.gif" width="14" height="1" alt="" border="0">
                                                        </td>
                                                        <td valign="top" width="100%" style="font-family: Arial;">
                                                            We welcome your comments about the <i>NCI Cancer Bulletin</i>.
                                                            <p>
                                                                <form name="frmComment" method="post" action="/cbComments.aspx">
                                                                <span style="color: #333367; text-decoration: none; font-weight: bold;">Your comments:</span><br>
                                                                <img src="/images/spacer.gif" width="1" height="5" alt="" border="0"><br>
                                                                <label class="hidden" for="txtComment">
                                                                    Comment</label>
                                                                <textarea name="txtComment" rows="7" wrap="soft" cols="35" style="font-size: 12px;
                                                                    width: 350px;"></textarea><br>
                                                                <img src="/images/spacer.gif" width="1" height="8" alt="" border="0"><br>
                                                                <input type="submit" value="Submit" style="font-size: 12px;"><br>
                                                                <img src="/images/spacer.gif" width="1" height="14" alt="" border="0">
                                                                </form>
                                                        </td>
                                                        <td valign="top" width="22">
                                                            <img src="/images/spacer.gif" width="22" height="1" alt="" border="0">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- End Send Feedback box -->
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- End of questionable content cut -->
                <tr>
                    <td height="10">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </td>
        <!-- end of the content pane -->
    </tr>
</table>
