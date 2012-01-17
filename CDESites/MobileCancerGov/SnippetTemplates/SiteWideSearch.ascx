<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteWideSearch.ascx.cs" Inherits="MobileCancerGov.Web.SnippetTemplates.SiteWideSearch" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>

<form id="frmResults" runat="server">
    <div class="searchResults">
        <ul>
            <asp:Repeater ID="rptSearchResults" runat="server">
                <ItemTemplate>
                    <li>
                        <a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" onclick='<%# ResultsHyperlinkOnclick(Container) %>'><%# DataBinder.Eval(Container.DataItem, "Title")%></a><br />
                        <%# LimitText(Container) %>
                    </li>
                </ItemTemplate>     
            </asp:Repeater>
        </ul>
  <% if (ResultsFound)
     {%>
   <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="3" />
  <br />
    <%}%>
      <p><asp:HyperLink runat="server" ID="lnkSearchInDeskTop" >Search The Full Site</asp:HyperLink></p>
    <% if (!ResultsFound){%>
        <p><strong><% =ResultsText %></strong></p>
    <% } %>
    </div>
<asp:HiddenField ID="itemsPerPage" Value="5" runat="server" />
</form>