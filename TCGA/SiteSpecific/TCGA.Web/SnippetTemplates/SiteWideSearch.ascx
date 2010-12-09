<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteWideSearch.ascx.cs" Inherits="TCGA.Web.SnippetTemplates.SiteWideSearch" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="NCI" %>

<form id="frmResults" runat="server">    

    <div class="searchresults">
    
        <img src="images/spacer.gif" width="566" height="1" alt="" border="0"><br>
        <asp:PlaceHolder ID="phResultsLabel" runat="server">
            <h1>Search Results for: <span class="term"><asp:Label ID="lblSearchTerm" runat="server" /></span></h1>         
        </asp:PlaceHolder>
    
    
        <span class="searchresults-top"><asp:Label ID="lblResults" runat="server" /></span>
     
        <ul>
            <asp:Repeater ID="rptSearchResults" runat="server">
                <ItemTemplate>
                    <li>
                        <strong>
                            <a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>">
                            <%# DataBinder.Eval(Container.DataItem, "Title")%></a>
                        </strong><br />
                     
                        <%# DataBinder.Eval(Container.DataItem, "Description") != null ? DataBinder.Eval(Container.DataItem, "Description") + "<br />" : "" %>                               
                        <a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>"><%# DataBinder.Eval(Container.DataItem, "Url")%></a>
                    </li>
                </ItemTemplate>     
            </asp:Repeater>
        </ul>
    
  
  <!-- Begin Page Result Per Page -->
    <table width="100%" cellspacing="0" cellpadding="0" border="0" class="searchresults-bottom" runat="server" id="tblPager">
        <tr>
            <td valign="top">
                Results <asp:Label ID="lblResultsBottom" runat="server" /> | Show
                <asp:DropDownList ID="ddlPageUnit" runat="server" OnSelectedIndexChanged="ddlPageUnit_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem value="10">10</asp:ListItem>
                    <asp:ListItem value="25">25</asp:ListItem>
                    <asp:ListItem value="50">50</asp:ListItem>
                    <asp:ListItem value="100">100</asp:ListItem> 
                </asp:DropDownList> 
                results per page.</td>
            <td valign="top" align="right">
                <NCI:SimplePager ID="spPager" runat="server" ShowNumPages="2" />
            </td>                
        </tr>
    </table>  
    <!-- End Page Result Per Page -->
    </div>

</form>