<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewsletterSearchResults.ascx.cs" Inherits="DCEG.Web.SnippetTemplates.NewsletterSearchResults"  %>
    <!-- Main Content Area -->
				   
				 
<div>
    <p><b>Search results for keyword(s)</b> "<%=Keyword%>"</p>
    
   <p> <b>Showing results:</b> <%=FirstRecord%>-<%=LastRecord%> of <%=TotalItems%></p>

</div>
					
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
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem, "Description")) %>
									</td>
								</tr>
								<tr><td class="display-url">
										<%# Convert.ToString(DataBinder.Eval(Container.DataItem,"Url")) %>
								</td></tr>
								<tr><td>&nbsp;</td></tr>
							</ItemTemplate>
							
							<FooterTemplate>
								</table>
							</FooterTemplate>
						
						</asp:Repeater>
						<p>
						<!---- end repeater -->
				   
						<%=Pager%>
					</p>
							
	<!-- End Main Area -->
