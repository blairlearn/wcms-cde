<%@ Page language="c#" Codebehind="CTLookupResults.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupResults" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>CTLookupResults</title>
    <link rel="stylesheet" href="/PublishedContent/Styles/nci.css" />
</head>
<body style="background:none">
    <form id="resultsForm" runat="server" method="post" > 
        <h1 style="font-size: 14px;  padding-left: 4px"><%=Caption%></h1> 
        <asp:DataGrid CssClass="cts-az-results" ID="results" Runat="server" ShowHeader="False" 
            ItemStyle-BackColor="#F5F5F3" AlternatingItemStyle-BackColor="#ffffff" 
            Visible="False" AutoGenerateColumns="False">
		    <Columns>
			    <asp:TemplateColumn ItemStyle-CssClass="cts-az-results-checkbox">
				    <ItemTemplate>
					    <input type="checkbox" name="chkItem" 
					        value="<%#DataBinder.Eval(Container.DataItem, "Name")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}" 
					        id="<%#DataBinder.Eval(Container.DataItem, "DisplayName")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}"> 
				    </ItemTemplate>
			    </asp:TemplateColumn>
			    <asp:TemplateColumn ItemStyle-CssClass="cts-az-results-label">
				    <ItemTemplate>
					    <label for="<%#DataBinder.Eval(Container.DataItem, "DisplayName")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}"><%#DataBinder.Eval(Container.DataItem, "DisplayName")%></label>
				    </ItemTemplate>
			    </asp:TemplateColumn>
		    </Columns>
	    </asp:DataGrid>		
    </form>
</body>
</html>
