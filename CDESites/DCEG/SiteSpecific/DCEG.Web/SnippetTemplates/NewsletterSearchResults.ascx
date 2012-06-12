<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsletterSearchResults.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.CancerBulletin.ResultsCancerBulletin" %>
    <!-- Main Content Area -->
				   
				   <!-- Nam's html -->
				   <table width="100%" cellspacing="0" cellpadding="0" border="0">
							<tr>
							<td valign="top">

							<table border="0" cellpadding="2" cellspacing="0" width="100%">
							<tr>
							<td><span class="page-title"><i>NCI Cancer Bulletin</i> Archive Results</span></td>
							</tr>
							<tr>
							<td><img src="/images/spacer.gif" alt="" width="1" height="10"></td>
							</tr>
							<tr>
							<td><b>Search results for keyword(s)</b> "<%=Keyword%>"  <%=DateLabel%></td>
							</tr>
							<tr>
							<td><b>Showing results:</b> <%=FirstRecord%>-<%=LastRecord%> of <%=TotalItems%></td>
							</tr>
							<tr>
							<td><img src="/images/spacer.gif" alt="" width="1" height="10"></td>
							</tr>
								<tr>
							<td><img src="/images/gray_spacer.gif" width="100%" height="1" alt="" border="0"></td></tr>
							<tr>
							<td><img src="/images/spacer.gif" width="1" height="10" alt="" border="0"></td></tr>
							</table>
							
				    <!-- Nam's html -->
						
					</td></tr>
					</table>
					
						<!-- Results Table -->
						<%=ResultsHtml%>
						<asp:Repeater ID="ResultRepeater" Runat="server">
						
							<HeaderTemplate>
									<table border="0" cellspacing="0" cellpadding="0" width="100%">  
							</HeaderTemplate>

							<ItemTemplate>
								<tr>
									<td>
										<a href="<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Url")) %>" onclick="<%=WebAnalytics%>">
										  <%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Title")) %>
										</a>
									</td>
							    </tr>
							    <tr>
									<td>
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "VolumeNumber")) %>
									</td>
								</tr>
							    <tr>
									<td>
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description")) %>
									</td>
								</tr>
								<tr><td class="display-url">
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"DisplayUrl")) %>
								</td></tr>
								<tr><td>&nbsp;</td></tr>
							</ItemTemplate>
							
							<FooterTemplate>
								</table>
							</FooterTemplate>
						
						</asp:Repeater>
						<p>
						<!---- end repeater -->
				   
					<!---------------- cancer bulletin content -->
						<%=Pager%>
					<!---------------- end cancer bulletin content -->
					</p>
							
	<!-- End Main Area -->
