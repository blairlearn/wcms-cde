<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CDE.UI.SnippetControls.SiteWideSearch" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>

<form id="frmResults" runat="server">

    <div>
        <input type="text" id="txtKeyword1" name="txtKeyword1"  size="50"; value="<% =System.Web.HttpContext.Current.Server.HtmlEncode(Keyword) %>" />&nbsp;<input id="swSearchButton" type="submit" value="Search"/>
        <% if (!string.IsNullOrEmpty(ResultsText))
           { %>
        <br />
        <p class="genSiteSearchResultsCount"><% =ResultsText%></p>
        <% } %>
         <ol>
            <asp:Repeater ID="rptSearchResults" runat="server" OnItemDataBound="rptSearchResults_OnItemDataBound">
                <ItemTemplate>
                    <li class="genSearchItem">
                        <h4><a id="titleLink" runat="server"/></h4>
                        <p class="genListItemDesc"><%# DataBinder.Eval(Container.DataItem, "Description") != null ? DataBinder.Eval(Container.DataItem, "Description") : "" %></p>
                        <p class="genListItemLink"><%# DataBinder.Eval(Container.DataItem, "Url")%></p>
                    </li>
                </ItemTemplate>     
            </asp:Repeater>
        </ol>
  <% if( ResultsFound ) {%>
  <br />
   <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="3" />
  <br />
    <%} %>
    </div>
    
<asp:HiddenField ID="itemsPerPage" Value="5" runat="server" />
</form>