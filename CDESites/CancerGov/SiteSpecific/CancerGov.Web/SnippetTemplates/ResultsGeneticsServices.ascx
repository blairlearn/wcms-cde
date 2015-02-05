<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ResultsGeneticsServices.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ResultsGeneticsServices" %>
<!-- Main Content Area -->
<!--div align="center"-->
 
<script language="javascript">
			function page(first)
			{
				document.searchParamForm.selectedPage.value=first;
				document.searchParamForm.action='<%=SearchPageInfo.SearchResultsPrettyUrl%>';
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
					window.location.href='<%=SearchPageInfo.DetailedViewSearchResultPagePrettyUrl%>' + '?personid=' + blnInput;								
				}
				else 
				{
					alert("Please check the professionals you would like to view.");					
				}
			}
		</script>
		<script src="/JS/JSLoader/JSLoader.js" type="text/javascript"></script>	




<form name="searchParamForm" method="post">
    <input type="hidden" name="selCancerType" value="<%=CancerType%>">
    <input type="hidden" name="selCancerFamily" value="<%=CancerFamily%>">
    <input type="hidden" name="txtCity" value="<%=Request.Form["txtCity"]%>">
    <input type="hidden" name="selState" value="<%=State%>">
    <input type="hidden" name="selCountry" value="<%=Country%>">
    <input type="hidden" name="txtLastName" value="<%=Request.Form["txtLastName"]%>">
    <input type="hidden" name="selectedPage" value="">
</form>
<div class="results">
    <form id="resultForm" name="resultForm" method="post" action="<%=SearchPageInfo.DetailedViewSearchResultPagePrettyUrl%>">
    <!-- Search Result Summary Section -->
    <%=SearchSummary%>
    
    <div class="results-listing">
        <span><%=ResultLabel%></span>
        <asp:Repeater runat="server" ID="resultGrid">
            <HeaderTemplate>
                <ul class="no-bullets">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <input type="checkbox" name="personid" id="personid<%#DataBinder.Eval(Container.DataItem, "PersonID")%>"
                        value="<%#DataBinder.Eval(Container.DataItem, "PersonID")%>">
                                            
                    <label for="personid<%#DataBinder.Eval(Container.DataItem, "PersonID")%>" class="inline">
                        <a href="<%=SearchPageInfo.DetailedViewSearchResultPagePrettyUrl%>?personid=<%#DataBinder.Eval(Container.DataItem, "PersonID")%>">
                            <%#DataBinder.Eval(Container.DataItem, "FullName")%>
                            <%#DataBinder.Eval(Container.DataItem, "Degree")%>
                        </a>
                    </label>
                </li>
            </ItemTemplate>
            <FooterTemplate>
                </ul>
            </FooterTemplate>
        </asp:Repeater>
                
        <span><%=ResultLabel%> shown.</span>
        
        <asp:Literal ID="ulPager" runat="server" />
        
        </div>
        
        <button id="submit" type="button" onclick="doSubmit();" class="button" runat="server">
            Display checked results</button>
        		
        <button id="reset" type="button" onclick="window.location.href='<%=SearchPageInfo.SearchPagePrettyUrl%>'" class="button">
            New search</button>
  </form>
</div>
		