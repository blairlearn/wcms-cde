<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancerBulletinSearch.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.CancerBulletin.CancerBulletinSearch" %>
<script type="text/javascript">    var searchType = 0; /* 0 = Search All 1 = Search Date Range */ </script>
<table border="0" cellspacing="0" cellpadding="0" width="571">
    <tbody>
        <tr>
            <td valign="top">
                <form onsubmit="<% =SubmitScript %>" method="post" name="CBSearchForm" action="<% =PostBackUrl %>">
                <table border="0" cellspacing="0" cellpadding="0" bgcolor="#e9e9e9">
                    <tbody>
                        <tr>
                            <td bgcolor="#cccccc" width="571" colspan="2">
                                <img alt="" src="/images/spacer.gif" width="1" height="1">
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table border="0" cellspacing="0" cellpadding="2" width="571" bgcolor="#e9e9e9">
                    <tbody>
                        <tr>
                            <td>
                                <img alt="" src="/images/spacer.gif" width="5" height="1">
                            </td>
                            <td width="571">
                                <img alt="" src="/images/spacer.gif" width="1" height="5">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img alt="" src="/images/spacer.gif" width="5" height="1">
                            </td>
                            <td>
                                <label style="color: #333333; font-size: 13px; font-weight: bold" id="searchAllIssues"
                                    for="cbkeyword">
                                    Search All Issues for:</label>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table border="0" cellspacing="0" cellpadding="2" width="571" bgcolor="#e9e9e9">
                    <tbody>
                        <tr>
                            <td>
                                <img alt="" src="/images/spacer.gif" width="5" height="1">
                            </td>
                            <td>
                                <input id="cbkeyword" onkeyup="CBSetSearchType(event);" value="Enter Keyword" size="28"
                                    name="cbkeyword">
                            </td>
                            <td>
                                <img alt="" src="/images/spacer.gif" width="3" height="1">
                            </td>
                            <td width="100%">
                                <input id="searchAllButton" onclick="searchType=0;" alt="search all" src="/images/searchAll.gif"
                                    type="image" name="searchAllButton" runat="server">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <img alt="" src="/images/spacer.gif" width="1" height="1">
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table border="0" cellspacing="0" cellpadding="2" width="571" bgcolor="#e9e9e9">
                    <tbody>
                        <tr>
                            <td width="1">
                                <img alt="" src="/images/spacer.gif" width="5" height="1">
                            </td>
                            <td colspan="6">
                                <span style="color: #333333; font-size: 11px; font-weight: normal">Or search between
                                    these dates:</span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <img alt="" src="/images/spacer.gif" width="5" height="1">
                            </td>
                            <td width="40">
                                <label class="hidden" for="startMonth">
                                    select start Month</label>
                                <select id="startMonth" name="startMonth">
                                    <option selected value="1">Jan.</option>
                                    <option value="2">Feb.</option>
                                    <option value="3">Mar.</option>
                                    <option value="4">Apr.</option>
                                    <option value="5">May</option>
                                    <option value="6">Jun.</option>
                                    <option value="7">Jul.</option>
                                    <option value="8">Aug.</option>
                                    <option value="9">Sept.</option>
                                    <option value="10">Oct.</option>
                                    <option value="11">Nov.</option>
                                    <option value="12">Dec.</option>
                                </select>
                            </td>
                            <td width="40">
                                <label class="hidden" for="startYear">
                                    select start Year</label>
                                <select id="startYear" name="startYear">
                                <% =GetYearListItems("startYear") %>
                                </select>
                            </td>
                            <td width="20">
                                &nbsp;and&nbsp;</STRONG>
                            </td>
                            <td width="40">
                                <label class="hidden" for="endMonth">
                                    select end month</label>
                                <select id="endMonth" name="endMonth">
                                    <% =GetMonthListItems("endMonth") %>
                                </select>
                            </td>
                            <td width="40">
                                <label class="hidden" for="endYear">
                                    select end Year</label>
                                <select id="endYear" name="endYear">
                                    <% =GetYearListItems("endYear") %>
                                </select>
                            </td>
                            <td width="100%">
                                &nbsp;&nbsp;<input id="searchRangeButton" onclick="searchType=1;" alt="search" src="/images/search_red.gif"
                                    type="image" name="searchRangeButton">
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table border="0" cellspacing="0" cellpadding="0" width="571">
                    <tbody>
                        <tr>
                            <td bgcolor="#e9e9e9" colspan="7">
                                <img alt="" src="/images/spacer.gif" width="1" height="5">
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor="#ffffff" colspan="7">
                                <img alt="" src="/images/spacer.gif" width="1" height="1">
                            </td>
                        </tr>
                    </tbody>
                </table>
                </form>
            </td>
        </tr>
    </tbody>
</table>
