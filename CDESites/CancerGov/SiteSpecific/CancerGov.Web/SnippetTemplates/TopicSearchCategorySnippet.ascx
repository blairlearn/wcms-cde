<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Modules.TopicSearch.UI.TopicSearchCategorySnippet" %>
<form id="Form1" runat="server">
<table cellspacing="0" cellpadding="1" border="0" width="100%" class="gray-border">
    <tbody>
        <tr>
            <td valign="top">
                <table  width="100%" cellpadding="0" cellspacing="0"  border="0" bgcolor="#ffffff">
                    <tbody>
                        <tr>
                            <td valign="top" class="box-title">
                                <img height="25" border="0" width="7" alt="" src="/images/spacer.gif">
                            </td>
                            <td valign="middle" class="box-title" colspan="2">
                                <asp:Label ID="strCategoryName" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" class="gray-border" colspan="3" >
                                <img height="1" border="0" width="1" alt="" src="/images/spacer.gif">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <img height="7" border="0" width="8" alt="" src="/images/spacer.gif">
                            </td>
                            <td valign="top">
                                <img height="1" border="0" width="553" alt="" src="/images/spacer.gif">
                            </td>
                            <td valign="top">
                                <img height="1" border="0" width="8" alt="" src="/images/spacer.gif">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <img height="1" border="0" width="7" alt="" src="/images/spacer.gif">
                            </td>
                            <td valign="top">
                                <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                    <tbody>
                                        <tr>
                                            <td height="10" colspan="4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20">
                                            </td>
                                            <td valign="top">
                                                <b>Select Subcategory:</b>
                                                <p>
                                                    <asp:RadioButtonList ID="rblTopicSearchList" runat="server" RepeatLayout="Flow" />
                                                    <br>
                                                </p>
                                            </td>
                                            <td width="150" valign="top">
                                                <b>Select Timeframe:</b><p>
                                                    <b></b>
                                                    <asp:RadioButtonList ID="rblTimeframeList" runat="server" RepeatLayout="Flow">
                                                        <asp:ListItem id="all" runat="server" Value="All (No Date Limit)" />
                                                        <asp:ListItem id="thirty" runat="server" Value="Last 30 Days" />
                                                        <asp:ListItem id="sixty" runat="server" Value="Last 60 Days" />
                                                        <asp:ListItem id="ninety" runat="server" Value="Last 90 Days" />
                                                    </asp:RadioButtonList>
                                                    <br>
                                                </p>
                                            </td>
                                            <td width="100">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="3" colspan="4">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="20">
                                            </td>
                                            <td colspan="3">
                                                <br>
                                                <br>
                                                <asp:ImageButton alt AlternateText="search" ID="btnSearch" runat="server" ImageUrl="/images/search_red.gif"
                                                    OnClick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="10" colspan="4">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                            <td valign="top">
                                <img height="1" border="0" width="7" alt="" src="/images/spacer.gif">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" colspan="3">
                                <img height="7" border="0" width="1" alt="" src="/images/spacer.gif">
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
</form>
