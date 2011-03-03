<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopLevelNavigationMenu.ascx.cs"
    Inherits="CancerGov.Web.UserControls.TopLevelNavigationMenu" %>
<table width="751" cellspacing="0" cellpadding="0" border="0" bgcolor="#696559" align="left">
    <tr>
        <asp:Repeater runat="server" ID="rptTopLevelNavigation">
            <ItemTemplate>
                <td valign="top">
                    <a href="<%# DataBinder.Eval(Container.DataItem, "sectionPath") %>" onmouseover="MM_swapImage('<%# DataBinder.Eval(Container.DataItem, "sectionID") %>','/images/<%# DataBinder.Eval(Container.DataItem, "sectionID") %>_over.gif');window.status=window.location.protocol + '//' + window.location.host + '/'; return true;"
                        onmouseout="MM_swapImgRestore();window.status=''; return true;">
                        <img border="0" alt="<%# DataBinder.Eval(Container.DataItem, "altText") %>" name="<%# DataBinder.Eval(Container.DataItem, "sectionID") %>"
                            src="/images/<%# isInSection(Container) %>" /></a>
                </td>
            </ItemTemplate>
        </asp:Repeater>
    </tr>
</table>
