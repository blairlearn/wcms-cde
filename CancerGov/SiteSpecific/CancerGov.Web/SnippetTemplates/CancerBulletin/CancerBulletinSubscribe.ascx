<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancerBulletinSubscribe.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.CancerBulletin.CancerBulletinSubscribe" %>
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

<script src="/Scripts/JSLoader/JSLoader.js" type="text/javascript"></script>

<!--div align="center"-->
    <table width="571" align="center" cellspacing="0" cellpadding="0" border="0" style="margin-top: 5px;">
        <tr>
            <td valign="top" height="100%">
                <table border="0" cellpadding="0" cellspacing="0" width="100%" height="100%" valign="top">
                    <%--<tr>
                    <td valign="top" height="61">
                        <table width="100%" cellspacing="0" cellpadding="20" border="0" bgcolor="#E1E0DE"
                            id="tblImgHeader" runat="server">
                            <tr>
                                <td valign="top" width="771" style="color: #b50000; height: 61px; vertical-align: baseline;
                                    font-family: Arial; font-weight: bold; font-size: 22px;">
                                    Subscribe to the <i>NCI Cancer Bulletin</i>
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
                </tr>--%>
                    <tr>
                        <!-- Start of questionable content cut -->
                        <td valign="top">
                            <!-- Main Content -->
                            <a name="skiptocontent"></a>
                            <table cellspacing="0" cellpadding="0" border="0">
                                <tr>
                                    <td width="30">
                                    </td>
                                    <td valign="top">
                                        <br>
                                        <table width="100%" cellspacing="0" cellpadding="1" border="0">
                                            <tr>
                                                <td valign="top">
                                                    <asp:Label ID="lblInfo" runat="server" Visible="false" />
                                                    <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                                        <tr>
                                                            <td valign="top" width="14">
                                                                <img src="/images/spacer.gif" width="14" height="1" alt="" border="0">
                                                            </td>
                                                            <td valign="top" width="100%">
                                                                <!-- Different Messages -->
                                                                <div class="HeaderText" style="font-family: Arial; color: #4d4d4d;">
                                                                    <asp:Label ID="lblHeader" runat="server" /></div>
                                                                <br />
                                                                <div id="divMessageBox" runat="server">
                                                                    <asp:Label ID="lblMessage" runat="server" Width="100%" />
                                                                    <div id="divSubscribe" runat="server">
                                                                        <form name="frmSubscribe" action="/cbsubscribe.aspx" method="post">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <label for="email">
                                                                                        E-mail address:</label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <input type="text" id="email" name="email" style="width: 200px;" />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="text-align: right">
                                                                                    <input type="submit" value="Submit" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        </form>
                                                                    </div>
                                                                </div>
                                                                <!-- end Different Messages -->
                                                            </td>
                                                            <td valign="top" width="22">
                                                                <img src="/images/spacer.gif" width="22" height="1" alt="" border="0">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" colspan="3">
                                                                <img src="/images/spacer.gif" width="1" height="20" alt="" border="0">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <div id="divSurvey" runat="server" style="padding-left: 10px;">
                                            <form name="frmSurvey" action="/cbsubscribe.aspx" method="post">
                                            <input type="hidden" name="hdnSurvey" value="1" />
                                            <asp:Label ID="lblSurveyMessage" runat="server">
	                                       In an effort to better 
											understand our audience and improve the newsletter, we invite you to submit 
											answers to this brief questionnaire. Please check at least one box in each 
											category.</asp:Label><br />
                                            <br />
                                            <!-- Probably need the user ID? -->
                                            <strong>I learned about the <i>NCI Cancer Bulletin</i> through:</strong><br />
                                            <asp:PlaceHolder ID="phLearnedQuestions" runat="server" />
                                            <br />
                                            <strong>I am a:</strong><br />
                                            <asp:PlaceHolder ID="phProfQuestions" runat="server" />
                                            <br />
                                            <input type="submit" value="Submit" />
                                            </form>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <!-- End Main Content -->
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
<!--/div-->
