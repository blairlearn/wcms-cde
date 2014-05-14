<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainPageHeader.ascx.cs"
    Inherits="CancerGov.Web.UserControls.Headers.MainPageHeader" %>
<%@ Register Src="~/UserControls/Menu/TopLevelNavigationMenu.ascx" TagPrefix="NCI"
    TagName="TopLevelNav" %>
<table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#A90101">
    <tr>
        <td>
            <div class="skip">
                <a href="#skiptocontent" title="Skip to content">Skip to content</a></div>
        </td>
    </tr>
</table>
<table width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#A90101">
    <tr>
        <td valign="top" align="center">
            <table width="771" cellspacing="0" cellpadding="0" border="0">
                <tr>
                    <td valign="top">
                        <img src="/images/spacer.gif" width="10" height="1" alt="" border="0">
                    </td>
                    <!-- Start get banner links -->
                    <!-- When the length of the banner links is less than 4 we return string.empty, which is weird.
						<!-- Banner Links here -->
                    <td valign="top" width="500" align="left" nowrap>
                        <table width="433" cellspacing="0" cellpadding="0" border="0" align="left">
                            <tr>
                                <td valign="top" rowspan="2">
                                    <a href="/">
                                        <img src="/images/banner_nci_logo_1.gif" alt="National Cancer Institute" width="118"
                                            height="81" border="0" /></a>
                                </td>
                                <td valign="top" colspan="2">
                                    <a href="/">
                                        <img src="/images/banner_nci_logo_2.gif" alt="National Cancer Institute" width="315"
                                            height="41" border="0" /></a>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <a href="http://www.nih.gov">
                                        <img src="/images/banner_nci_logo_3.gif" alt="U.S. National Institutes of Health"
                                            width="209" height="40" border="0" /></a>
                                </td>
                                <td valign="top">
                                    <a href="/">
                                        <img src="/images/banner_nci_logo_4.gif" alt="National Cancer Institute" width="106"
                                            height="40" border="0" /></a>
                                </td>
                            </tr>
                        </table>
                        <!-- End get banner links -->
                    </td>
                    <td valign="top" align="left" width="251">
                    </td>
                    <td valign="top">
                        <img src="/images/spacer.gif" width="10" height="1" alt="" border="0" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <img src="/images/spacer.gif" width="10" height="1" alt="" border="0">
                    </td>
                    <td valign="bottom" colspan="2">
                        <!-- Main Navigation menu - also called the Top level nav -->
                        <NCI:TopLevelNav id="TopLevelNav" runat="server" />
                    </td>
                    <td valign="top">
                        <img src="/images/spacer.gif" width="10" height="1" alt="" border="0" />
                    </td>
                </tr>
                <!-- end nav bar -->
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top" bgcolor="#FFFFFF">
            <img src="/images/spacer.gif" width="10" height="15" alt="" border="0" />
        </td>
    </tr>
</table>
<a name="top"></a>