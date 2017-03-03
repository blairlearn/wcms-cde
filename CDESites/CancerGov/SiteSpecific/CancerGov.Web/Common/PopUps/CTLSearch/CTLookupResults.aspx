<%@ Page language="c#" Codebehind="CTLookupResults.aspx.cs" AutoEventWireup="True" Inherits="CancerGov.Web.CTLookupResults" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
    <title>CTLookupResults</title>
    <link rel="stylesheet" href="/PublishedContent/Styles/nvcg.css" />
    <script src="/PublishedContent/js/Common.js" type="text/javascript"></script>
    <script src="/PublishedContent/js/Popups.js" type="text/javascript"></script>
    <meta name="robots" content="noindex,nofollow">
</head>
<body class="popup">
    <form id="resultsForm" runat="server" method="post" > 
        <div class="row" id="resultsCaption" runat="server" visible="false">
            <div class="small-12 columns">
                <h6><%=Caption%></h6> 
            </div>
        </div>
        <asp:Repeater ID="results" Runat="server" Visible="False">
            <HeaderTemplate>
                <div class="scrolling-list tall groupedCheckBoxList">
            </HeaderTemplate>
		    <ItemTemplate>
		        <div class="checkbox row">
		            <div class="small-12 columns">
			            <input type="checkbox" name="chkItem" 
			                value="<%#DataBinder.Eval(Container.DataItem, "Name")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}" 
			                id="<%#DataBinder.Eval(Container.DataItem, "DisplayName")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}"> 
			            <label for="<%#DataBinder.Eval(Container.DataItem, "DisplayName")%>{<%#DataBinder.Eval(Container.DataItem, "HashedCDRID")%>}"><%#DataBinder.Eval(Container.DataItem, "DisplayName")%></label>
			        </div>
			    </div>
		    </ItemTemplate>
		    <FooterTemplate>
		        </div>
		    </FooterTemplate>
	    </asp:Repeater>
    </form>
</body>
</html>
