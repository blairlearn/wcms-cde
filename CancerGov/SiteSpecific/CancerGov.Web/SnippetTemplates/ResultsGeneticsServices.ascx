<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsGeneticsServices.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ResultsGeneticsServices" %>
<!-- Main Content Area -->
<!--div align="center"-->
    <table width="771" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <!-- Main Content Area -->
            <td id="contentzone" valign="top" width="100%">
                <a name="skiptocontent"></a>
                <table cellpadding="1" width="100%" cellspacing="0" border="0" class="gray-border">
                    <tr>
                        <td>
                            <table cellpadding="7" cellspacing="0" border="0" width="100%" bgcolor="#ffffff">
                                <tr>
                                    <td>
                                        <span class="grey-text" a>This directory lists professionals who provide services related
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
                <form name="searchParamForm" method="post">
                <input type="hidden" name="selCancerType" value="<%=Request.Form["selCancerType"]%>">
                <input type="hidden" name="selCancerFamily" value="<%=Request.Form["selCancerFamily"]%>">
                <input type="hidden" name="txtCity" value="<%=Request.Form["txtCity"]%>">
                <input type="hidden" name="selState" value="<%=Request.Form["selState"]%>">
                <input type="hidden" name="selCountry" value="<%=Request.Form["selCountry"]%>">
                <input type="hidden" name="txtLastName" value="<%=Request.Form["txtLastName"]%>">
                <input type="hidden" name="selectedPage" value="">
                </form>
                <form id="resultForm" name="resultForm" method="post" action="/search/view_geneticspro.aspx">
                <!-- Search Result Summary Section -->
                <%=SearchSummary%>
                <p>
                    <!-- Result Search Display Section -->
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td>
                                <span class="header-A">
                                    <%=ResultLabel%></span>
                                <p>
                                    <asp:DataGrid runat="server" GridLines="None" ShowHeader="False" ItemStyle-BackColor="#f5f5f3"
                                        AlternatingItemStyle-BackColor="#ffffff" BorderWidth="0" ItemStyle-BorderStyle="None"
                                        AlternatingItemStyle-BorderStyle="None" Visible="False" ID="resultGrid" AutoGenerateColumns="False"
                                        CellPadding="3" BorderStyle="None" Width="100%">
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <ItemTemplate>
                                                    <input type="checkbox" name="personid" id="personid<%#DataBinder.Eval(Container.DataItem, "PersonID")%>"
                                                        height="2" value="<%#DataBinder.Eval(Container.DataItem, "PersonID")%>">
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn ItemStyle-VerticalAlign="Top" ItemStyle-Width="100%">
                                                <ItemTemplate>
                                                    <label for="personid<%#DataBinder.Eval(Container.DataItem, "PersonID")%>">
                                                        <a href="/search/view_geneticspro.aspx?personid=<%#DataBinder.Eval(Container.DataItem, "PersonID")%>">
                                                            <%#DataBinder.Eval(Container.DataItem, "FullName")%>
                                                            <%#DataBinder.Eval(Container.DataItem, "Degree")%></a></label>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                            </td>
                        </tr>
                    </table>
                    <p>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td valign="top">
                                    <span class="header-A">
                                        <%=ResultLabel%>
                                        shown.</span>
                                </td>
                                <td height="30" valign="top" align="right">
                                    <%=Pager%>
                                </td>
                            </tr>
                        </table>
                        <table>
                            <tr>
                                <td align="left" valign="bottom">
                                    <a href="javascript:doSubmit();" id="submit" runat="server">
                                        <img src="/images/form_checked_button.gif" alt="display checked results" 
                                        border="0" style="width: 61px; height: 18px"></a><input
                                            type="submit" id="textSubmit" value="Display Checked Results" 
                                        runat="server" style="margin-left: 0px">
                                    &nbsp;&nbsp;&nbsp;&nbsp; <a href="/search/search_geneticsservices.aspx" alt="New Genetics Services Search">
                                        <img src="/images/new_search_red.gif" border="0" alt="New Search"></a>
                                </td>
                            </tr>
                        </table>
                </form>
            </td>
        </tr>
    </table>
<!--/div>
<!------End Main Area------>
