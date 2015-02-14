﻿<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CancerGov.Apps.SiteWideSearch"  %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="cc1" %>

<script type="text/javascript">
    var ids = {
    txtSWRKeyword: "<%=txtSWRKeyword.ClientID %>"
}
</script>


	<!-- Main Area -->
	<div class="results">
		<!-- Main Content Area -->

        <cc1:JavascriptProbeControl ID="jsProbe" runat="server" />
        <h3><asp:Label id="lblResultsForText" Text="Results for:" runat="server"/> <asp:Label CssClass="term" ID="lblResultsForKeyword" runat="server" /></h3>
     
        <!-- Best Bets Here -->
        <asp:Repeater ID="rptBestBets" EnableViewState="false" runat="server">
            <ItemTemplate>
                <div class="featured sitewide-results">
                    <h2>
                        <asp:Label
                            ID="lblBBCatName" 
                            Text='<%# (PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.Spanish ? "Mejores resultados para " : "Best Bets for ") + Eval("CategoryName")%>'
                            runat="server" />
                    </h2>
                    <asp:Literal ID="Literal1" runat="server" Text='<%# BestBetResultsHyperlinkOnclick(Container) %>'>
                    </asp:Literal>
                </div>    
            </ItemTemplate>
        </asp:Repeater>
        
        <!-- DYM -->
        <asp:PlaceHolder ID="phDYM" runat="server">
            <h2><asp:Literal ID="litDidYouMeanText" runat="server">Did you mean</asp:Literal>:</h2>
            <asp:HyperLink id="lnkDym" NavigateUrl="#" runat="server"><span class="term"><asp:Literal ID="litDYMString" runat="server" /></span></asp:HyperLink><p></p>
        </asp:PlaceHolder>
        
        <!-- Results x-y of z for: Keyword -->
        <div class="sitewide-results">
            <h4><asp:Label ID="lblTopResultsXofY" runat="server" /> <span class="term"><asp:Label ID="lblTopResultsXofYKeyword" runat="server" /></span></h4>
        
            <!-- Error message, used to be ResultsView -->
            <asp:PlaceHolder ID="phError" runat="server">
                <div>
                    <asp:Literal ID="litError" runat="server" Text="Please enter a search phrase." />
                </div>
            </asp:PlaceHolder>
            
            <asp:Repeater ID="rptResults" EnableViewState="false" runat="server">
                <HeaderTemplate>
                    <div class="sitewide-list" >
                        <ul class="no-bullets">
                </HeaderTemplate>
                <ItemTemplate>
                            <li>
                                <div>
                                <asp:HyperLink
                                    id="lnkResultLink"
                                    runat="server"
                                    onclick='<%# ResultsHyperlinkOnclick(Container) %>'                                   
                                    NavigateUrl='<%# Eval("Url") %>'
                                    Text='<%# Eval("Title") %>' />
                                </div>
                                <div>
			                        <cite class="url">
			                        <%# Eval("DisplayUrl") %>
			                        </cite>
			                    </div>
			                    <div class="description">
			                        <%# Eval("Description") %>
			                    </div>
			                </li>
                </ItemTemplate>    
                <FooterTemplate>
                        </ul>
                    </div>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        
        <form id="resultForm" runat="server" class="sitewide-search-results">
            <!-- pager and selection -->
            <div class="results-pager">
                <h2 class="results-count"><asp:Label ID="lblBottomResultsXofY" runat="server" /></h2>
                <span>
                    <asp:Label id="lblDDLPageUnitShowText" Text="Show" runat="server"/>
                    <asp:Label ID="lblPageUnit" runat="server" CssClass="hidden" Text="Number of results to show on a page" AssociatedControlID="ddlPageUnit" />
                    <asp:DropDownList
                        id="ddlPageUnit"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ChangePageUnit"
                        runat="server">
                        <asp:ListItem Text="10" Value="10" />
                        <asp:ListItem Text="20" Value="20" />
                        <asp:ListItem Text="50" Value="50" />
                    </asp:DropDownList>
                    <noscript>
                        <asp:Button 
                            ID="btnTextChangePageUnit" 
                            Text="Go"
                            OnClick="ChangePageUnitBtnClick"
                            runat="server" />
                    </noscript>
			        <asp:Label id="lblDDLPageUnitResultsPPText" Text="results per page." runat="server"/>
			    </span>
                <cc1:simpleulpager ID="spPager" runat="server" CssClass="pagination"></cc1:simpleulpager>
	        </div>
	    
	        <h4>
                <asp:Label ID="lblSearchWithinResultsFound" runat="server" />
                <asp:Label CssClass="term" ID="lblSearchWithinResultKeyword" runat="server" />
            </h4>
		    <asp:Panel ID="pnlSWR" runat="server">

                <asp:RadioButtonList CssClass="radio" ID="rblSWRSearchType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Text="New Search" Value="1" />
                    <asp:ListItem Text="Search Within Results" Value="2" />
                </asp:RadioButtonList>
          
                <div class="row">
                    <div class="small-6 columns">
                        <asp:Label AssociatedControlID="txtSWRKeyword" ID="lblSWRKeywordLabel" runat="server" CssClass="hidden">Keyword</asp:Label>
                        <asp:TextBox CssClass="input" ID="txtSWRKeyword" Rows="40" MaxLength="40" runat="server" />
                    </div>
                    <div class="small-2 columns left">
                        <asp:Button CssClass="button postfix" id="btnSWRTxtSearch" text="Search" runat="server" OnClick="SearchWithinResults" />
                    </div>
                </div>

		    </asp:Panel>
        </form>                    				
	</div>
	<!-- End Main Area -->
