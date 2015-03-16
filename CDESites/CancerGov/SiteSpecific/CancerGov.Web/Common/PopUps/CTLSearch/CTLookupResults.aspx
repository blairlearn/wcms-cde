<%@ Page language="c#" Codebehind="CTLookupResults.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupResults" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>CTLookupResults</title>
    <link rel="stylesheet" href="/PublishedContent/Styles/nvcg.css" />
    <script src="/PublishedContent/js/modernizr.custom.2.7.1.js" type="text/javascript"></script>
</head>
<body style="background:none">
    <form id="resultsForm" runat="server" method="post" > 
        <h6 style="font-size: 14px;  padding-left: 4px"><%=Caption%></h6> 
        <asp:Repeater ID="results" Runat="server" Visible="False">
            <HeaderTemplate>
                <div class="scrolling-list tall groupedCheckBoxList">
            </HeaderTemplate>
		    <ItemTemplate>
		        <div class="checkbox">
			        <input type="checkbox" name="chkItem" 
			            value="<%#DataBinder.Eval(Container.DataItem, "Name")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}" 
			            id="<%#DataBinder.Eval(Container.DataItem, "DisplayName")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}"> 
			        <label for="<%#DataBinder.Eval(Container.DataItem, "DisplayName")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}"><%#DataBinder.Eval(Container.DataItem, "DisplayName")%></label>
			    </div>
		    </ItemTemplate>
		    <FooterTemplate>
		        </div>
		    </FooterTemplate>
	    </asp:Repeater>
    </form>
</body>
</html>
