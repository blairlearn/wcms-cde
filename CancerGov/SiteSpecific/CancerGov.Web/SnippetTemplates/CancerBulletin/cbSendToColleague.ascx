<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cbSendToColleague.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.CancerBulletin.cbSendToColleague" %>
<!-- Styles needed for this page -->
<style>
    .GoodText
    {
        font-weight: bold;
        color: #484bd0;
        font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
    }
    .BadText
    {
        font-weight: bold;
        color: #a8364d;
        font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
    }
    .InfoText
    {
        font-weight: bold;
        font-size: 19px;
        color: #333367;
        font-family: Trebuchet MS, Tahoma, Verdana, Arial, sans-serif;
    }
    .HeaderText
    {
        font-family: Times New Roman, Serif;
        font-size: 19px;
        color: #C5080B;
    }
</style>
<table cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td width="30">
        </td>
        <td valign="top">
            <a name="skiptocontent"></a>
            <br>
            <form name="frmUnSubscribe" id="frmUnSubscribe" method="post" runat="server">
            <table width="450" cellspacing="0" cellpadding="1" border="0">
                <tr>
                    <td valign="top">
                        <div runat="server" id="ErrorMsg" visible="false">
                        </div>
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
                            <tr>
                                <td valign="top" width="14">
                                    <img src="/images/spacer.gif" width="14" height="1" alt="" border="0">
                                </td>
                                <td valign="top" width="100%">
                                    <div class="HeaderText" style="font-family: Arial; color: #4d4d4d;">
                                        <%=strHeader%></div>
                                    <br />
                                    <%=strMessageBody%><br />
                                    <form name="frmSendToFriend" action="/cbSendToFriend.aspx" method="post">
                                    <table runat="server" id="tableSend" style="font-family: Arial;">
                                        <tr>
                                            <td>
                                                <label for="toemail" style="text-decoration: none;">
                                                    Send the <i>NCI Cancer Bulletin</i> to:</label><br>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" id="toemail" name="toemail" class="" size="28"><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label for="fromemail" style="text-decoration: none;">
                                                    Your e-mail address:</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" id="fromemail" name="fromemail" class="" size="28"><br />
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label for="fromname" style="text-decoration: none;">
                                                    Your name:</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="text" id="fromname" name="fromname" class="" size="28" id="Text1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="issuelink" name="issuelink" value=" strIssueLink + "">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right">
                                                <input type="submit" value="Submit">
                                            </td>
                                        </tr>
                                    </table>
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
            </form>
        </td>
    </tr>
</table>
