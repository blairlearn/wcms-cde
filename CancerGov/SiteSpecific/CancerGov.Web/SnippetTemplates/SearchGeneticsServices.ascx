<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchGeneticsServices.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.SearchGeneticsServices" %>
<table width="771" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <td valign="top">
            <img src="/images/spacer.gif" width="10" height="1" alt="" border="0">
        </td>
        <!-- Left Nav Column -->
        <td id="leftzone" valign="top">
            <%=this.PageLeftColumn.Render()%>
        </td>
        <!-- Main Content Area -->
        <td id="contentzone" valign="top" width="100%">
            <a name="skiptocontent"></a>
            <table cellpadding="1" width="100%" cellspacing="0" border="0" class="gray-border">
                <tr>
                    <td>
                        <table cellpadding="7" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                            <tr>
                                <td>
                                    <span class="grey-text">This directory lists professionals who provide services related
                                        to cancer genetics (cancer risk assessment, genetic counseling, genetic susceptibility
                                        testing, and others). These professionals have applied to be listed in this directory.
                                        Inclusion in this directory does not imply an endorsement by the National Cancer
                                        Institute. For information on inclusion criteria and applying to the directory,
                                        see the <a href="/cancertopics/genetics/directory/applicationform">application form</a>.</span>
                                    <p>
                                        <span class="grey-text">For more information please send an e-mail to </span><a href="mailto:GeneticsDirectory@cancer.gov">
                                            GeneticsDirectory@cancer.gov</a>
                                    <p>
                                        <a href="/cancertopics/genetics/directory/description" class="navigation-dark-red-link">
                                            NCI Cancer Genetics Services Directory: Description</a><br>
                                        <a href="/cancertopics/genetics/directory/applicationform" class="navigation-dark-red-link">
                                            Join the NCI Cancer Genetics Services Directory</a><br>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <p />
            <form id="searchForm" name="searchForm" action="/search/results_geneticsservices.aspx"
            method="post">
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
                                                            <label for="selCancerType">
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
                                                            <label for="selCancerFamily">
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
                                                            <label for="selState">
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
                                                            <label for="selCountry">
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
					        var the_form = document.getElementById('searchForm');
    					    
					        if (the_form) 
					        {
					            if (the_form.addEventListener) {
					                the_form.addEventListener('submit',function() {NCIAnalytics.GeneticServicesDirectorySearch(null)},false);
					            } else {
					                the_form.attachEvent('onsubmit', function() { NCIAnalytics.GeneticServicesDirectorySearch(null)} );
					            }
					        }					    
                        </script>
            </asp:Literal>
        </td>
        <!----------------------->
        <td valign="top">
            <img src="/images/spacer.gif" width="10" height="1" alt="" border="0">
        </td>
    </tr>
</table>
