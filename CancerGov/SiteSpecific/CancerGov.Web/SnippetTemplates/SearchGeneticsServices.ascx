<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchGeneticsServices.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.SearchGeneticsServices" %>

<script type="text/javascript">
    var ids = {
    txtCity: "txtCity"
    ,txtLastName: "txtLastName"
    ,selCancerType: "<%=selCancerType.ClientID%>"
    ,selCancerFamily: "<%=selCancerFamily.ClientID%>"
    ,selState: "<%=selState.ClientID%>"
    , selCountry: "<%=selCountry.ClientID%>"
}

function doWebAnalyticsStuff() {
    NCIAnalytics.GeneticServicesDirectorySearch(null);
    return true;
}

</script>   
    
<table width="571" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <!-- Main Content Area -->
        <td id="contentzone" valign="top" width="100%">
<br />
            <form id="searchForm" name="searchForm" action="<%=SearchPageInfo.SearchResultsPrettyUrl%>"  method="post" onsubmit="return doWebAnalyticsStuff();">
            <table cellpadding="1" cellspacing="0" border="0" width="100%" class="gray-border">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                            <tr>
                                <td align="left">
                                    <table cellpadding="5" cellspacing="0" border="0" width="100%" class="gray-border">
                                        <tr>
                                            <td class="box-title">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<span class="header-A">Specialty</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="10" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <img src="/images/spacer.gif" border="0" alt="" width="140" height="1">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top">
                                                            <label for="<%=selCancerType.ClientID%>">
                                                                Type of Cancer:</label>&nbsp;&nbsp;&nbsp;&nbsp;<br>
                                                            <i>(choose 1 or more)</i>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <select id="selCancerType" runat="server" size="3" class="size50x3_field" multiple
                                                                name="selCancerType" onkeypress="javascript:checkKeyPress(event);">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td align="left">
                                                            <b>OR</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top">
                                                            <label for="<%=selCancerFamily.ClientID%>">
                                                                Family Cancer&nbsp;&nbsp;&nbsp;&nbsp;<br>
                                                                Syndrome:</label>&nbsp;&nbsp;&nbsp;&nbsp;<br>
                                                            <i>(choose 1 or more)</i>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <select id="selCancerFamily" runat="server" size="3" class="size50x3_field" multiple
                                                                name="selCancerFamily" onkeypress="javascript:checkKeyPress(event);">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="5" cellspacing="0" border="0" width="100%" class="gray-border">
                                        <tr>
                                            <td class="box-title">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<span class="header-A">Location</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="5" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                                        <tr>
                                            <td>
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <img src="/images/spacer.gif" border="0" alt="" width="140" height="1">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top">
                                                            <label for="txtCity">
                                                                City:</label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td valign="top">
                                                            <input type="text" name="txtCity" id="txtCity" maxlength="40" size="30" class="size30_field">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" valign="top" nowrap>
                                                            <label for="<%=selState.ClientID%>">
                                                                State:</label>&nbsp;&nbsp;&nbsp;&nbsp;<br>
                                                            <i>(choose 1 or more)</i>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <select id="selState" runat="server" class="size30x3_field" size="3" multiple name="selState"
                                                                onkeypress="javascript:checkKeyPress(event);">
                                                            </select>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <label for="<%=selCountry.ClientID%>">
                                                                Country:</label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <select id="selCountry" runat="server" class="size30x3_field" size="3" multiple name="selCountry"
                                                                onkeypress="javascript:checkKeyPress(event);">
                                                            </select>
                                                            <!--		<SELECT class="size30_field" name="selCountry" onchange="javascript:defunct();" onfocus="javascript:formFocus();" onblur="javascript:formBlur();">
																			<option selected value="all countries;default">all countries</option>
																			<option value="United States">U.S.A.</option>
																			<option value="Canada">Canada</option>
																		</SELECT>
																-->
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="5" cellspacing="0" border="0" width="100%" class="gray-border">
                                        <tr>
                                            <td class="box-title">
                                                &nbsp;&nbsp;&nbsp;&nbsp;<span class="header-A">Name of genetics professional, if known</span>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="5" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <img src="/images/spacer.gif" border="0" alt="" width="140" height="1">
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="right">
                                                            <label for="txtLastName">
                                                                Last Name:</label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                        <td valign="top">
                                                            <input type="text" name="txtLastName" id="txtLastName" maxlength="40" class="size30_field"
                                                                size="30">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table cellpadding="5" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                                        <tr>
                                            <td>
                                                <table cellspacing="0" cellpadding="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <img src="/images/spacer.gif" border="0" alt="" width="140" height="1">
                                                        </td>
                                                        <td align="left">
                                                            <input id="searchBtn" type="image" alt="Search" src="/images/search_genetics.gif"
                                                                runat="server" /><input type="submit" id="textSubmit" value="Search " runat="server">
                                                            &nbsp;&nbsp;&nbsp;
                                                            <input id="clearBtn" type="image" alt="Clear" src="/images/clear_red.gif" onclick="document.searchForm.reset(); return false;"
                                                                runat="server" /><input type="reset" id="textClear" value="Clear " runat="server">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            </form>
            <asp:Literal Mode="PassThrough" ID="litOnSubmitHandlerAdder" runat="server">
					    <script type="text/javascript">
//					        var the_form = document.searchForm;
//					        if (the_form) {
//					            if (the_form.addEventListener) {
//					                the_form.addEventListener('submit',function() {NCIAnalytics.GeneticServicesDirectorySearch(null)},false);
//					            } else {
//					                the_form.attachEvent('onsubmit', function() { NCIAnalytics.GeneticServicesDirectorySearch(null)} );
//					            }
//					        }					    
//                        </script>
            </asp:Literal>
        </td>
    </tr>
</table>
