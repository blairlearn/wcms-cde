<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteWideSearch.ascx.cs" Inherits="TCGA.Web.SnippetTemplates.SiteWideSearch" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>

<script type="text/javascript">
    function trackTextChange(tBox) {
        if (tBox.name == 'txtKeyword1') {
            document.getElementById('txtKeyword2').value = tBox.value;
        }
        else if (tBox.name == 'txtKeyword2') {
        document.getElementById('txtKeyword1').value = tBox.value;
        }
    }
</script>
<form id="frmResults" runat="server">

    <div>
        <input onchange="trackTextChange(this)" type="text" id="txtKeyword1" name="txtKeyword1" size="75"; value="<% =Keyword %>" />&nbsp;&nbsp;<input type="image" class="schImg" src="images/general/content-search.gif" alt="Search" />
        <% if (!string.IsNullOrEmpty(ResultsText))
           { %>
        <br />
        <p><% =ResultsText%></p>
        <% } %>
        <ul>
            <asp:Repeater ID="rptSearchResults" runat="server">
                <ItemTemplate>
                    <li>
                        <h1><%# DataBinder.Eval(Container.DataItem, "Title")%></h1>
                        <%# DataBinder.Eval(Container.DataItem, "Description") != null ? DataBinder.Eval(Container.DataItem, "Description") + "<br />" : "" %>
                        <a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>"><%# DataBinder.Eval(Container.DataItem, "Url")%></a>
                        <br />
                    </li>
                </ItemTemplate>     
            </asp:Repeater>
        </ul>
        <% if (ResultsFound){ %>
        <p><% =ResultsText%></p><%} %>
  <% if( ResultsFound ) {%>
  <br />
   <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="3" />
  <br />
   <input type="text" onchange="trackTextChange(this)" id="txtKeyword2" name="txtKeyword2" size="75"; value="<% =Keyword %>" />&nbsp;&nbsp;<input type="image" class="schImg" src="images/general/content-search.gif" alt="Search" />
    <%} %>
    </div>
    
<asp:HiddenField ID="itemsPerPage" Value="5" runat="server" />
</form>