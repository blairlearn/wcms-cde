<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsGeneticsServices.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ResultsGeneticsServices" %>
<!-- Main Content Area -->
<!--div align="center"-->

<script language="javascript">
			function page(first)
			{
				document.searchParamForm.selectedPage.value=first;
				document.searchParamForm.action='/genetic/resultgenetics';
				document.searchParamForm.submit();			
			}
			
			function doSubmit()
			{
				var blnInput = '';
				
				if(document.resultForm.personid != null)
				{
					if(<%=LastRec%> - <%=FirstRec%> == 0)
					{
						if(document.resultForm.personid.checked == true)
						{
							blnInput = document.resultForm.personid.value;								
						}
					}
					else
					{
						for(var i = 0; i <= (<%=LastRec%> - <%=FirstRec%>); i++)
						{
							if(document.resultForm.personid[i].checked == true)
							{
								if(blnInput.length > 0)
								{
									blnInput += ',';
								}
								blnInput += document.resultForm.personid[i].value;
							}
						}
					}
				}
											
				if(blnInput.length > 0)
				{
					window.location.href='/genetic/Sesultgenetics?personid=' + blnInput;								
				}
				else
				{
					alert("Please check the professionals you would like to view.");					
				}
			}
		</script>
		<script src="/Scripts/JSLoader/JSLoader.js" type="text/javascript"></script>	



    <table width="571" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <!-- Main Content Area --> 
            <td id="contentzone" valign="top" width="100%">

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
                                                        <a href="/genetic/Sesultgenetics?personid=<%#DataBinder.Eval(Container.DataItem, "PersonID")%>">
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
									<a href="javascript:doSubmit();" id="submit" runat="server"><img src="/images/form_checked_button.gif" alt="display checked results" border="0"></a><input type="submit" id="textSubmit" value="Display Checked Results" runat="server">
									&nbsp;&nbsp;&nbsp;&nbsp;
									<a href="/search/search_geneticsservices.aspx" alt="New Genetics Services Search"><img src="/images/new_search_red.gif" border="0" alt="New Search"></a>
								</td>
                            </tr>
                        </table>
                </form>
            </td>
        </tr>
    </table>
<!--/div>
<!------End Main Area------>
