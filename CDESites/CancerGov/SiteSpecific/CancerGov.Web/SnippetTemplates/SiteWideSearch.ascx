<%@ Control Language="C#" AutoEventWireup="true" Inherits="NCI.Web.CancerGov.Apps.SiteWideSearch"  %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" tagprefix="cc1" %>

<script type="text/javascript">
    var ids = {
    txtSWRKeyword: "<%=txtSWRKeyword.ClientID %>"
}
</script>


	<!-- Main Area -->
	<div class="sw-search-results">
		<table width="100%" cellspacing="0" cellpadding="0" border="0">
			<tr>		
				<td valign="top">
				<!-- a href="#Content"><img src="/images/spacer.gif" width="1" height="1" alt="skip to content" border="0"></a -->
				<img src="/images/spacer.gif" width="9" height="1" alt="" border="0"></td>
				<!-- Main Content Area -->
				<td id="contentzone" valign="top" width="100%">
				   <a name="skiptocontent"></a>
                    <form id="resultForm" runat="server">
                        <cc1:JavascriptProbeControl ID="jsProbe" runat="server" />
                        <asp:Label CssClass="page-title" id="lblResultsForText" Text="Results for:" runat="server"/>&nbsp;&nbsp;<asp:Label CssClass="search-result" ID="lblResultsForKeyword" runat="server" />
                        <p></p>
                        
                        <!-- Best Bets Here -->
                        <asp:Repeater ID="rptBestBets" EnableViewState="false" runat="server">
                            <ItemTemplate>
                                <div style="background:#e7e7e7;padding: 4px 8px 4px 8px; border: 1px solid #bdbdbc;">
                                    <asp:Label CssClass="header-A" 
                                        ID="lblBBCatName" 
                                        Text='<%# (PageDisplayInformation.Language == NCI.Web.CDE.DisplayLanguage.Spanish ? "Mejores resultados para " : "Best Bets for ") + Eval("CategoryName")%>'
                                        runat="server" />
                                </div>    
                                <asp:Literal ID="Literal1" runat="server" Text='<%# BestBetResultsHyperlinkOnclick(Container) %>'>
                                </asp:Literal>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <!-- DYM -->
                        <asp:PlaceHolder ID="phDYM" runat="server">
                            <b><asp:Literal ID="litDidYouMeanText" runat="server">Did you mean</asp:Literal>:  
                            </b><asp:HyperLink id="lnkDym" NavigateUrl="#" runat="server"><i><b> <asp:Literal ID="litDYMString" runat="server" /> </b></i></asp:HyperLink><p></p>
                        </asp:PlaceHolder>
                        
                        <!-- Results x-y of z for: Keyword -->
                        <div style="background:#e7e7e7;padding: 4px 8px 4px 8px; border: 1px solid #bdbdbc;">
                            <asp:Label CssClass="header-A" ID="lblTopResultsXofY" runat="server" /> <asp:Label ID="lblTopResultsXofYKeyword" runat="server" />
                        </div>
                        
                        <!-- Error message, used to be ResultsView -->
                        <asp:PlaceHolder ID="phError" runat="server">
                            <div style="margin: 24px 0 24px 0;">
                                <asp:Literal ID="litError" runat="server" Text="Please enter a search phrase." />
                            </div>
                        </asp:PlaceHolder>
                        
                        <asp:Repeater ID="rptResults" EnableViewState="false" runat="server">
                            <HeaderTemplate>
                                <div style="padding: 12px 0 10px 8px;">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HyperLink
                                    id="lnkResultLink"
                                    runat="server"
                                    onclick='<%# ResultsHyperlinkOnclick(Container) %>'                                   
                                    NavigateUrl='<%# Eval("Url") %>'
                                    Text='<%# Eval("Title") %>' />
   								<br/>
								<%# Eval("Description") %>
								<br/>
								<%# Eval("DisplayUrl") %>
								<br><br>
                            </ItemTemplate>    
                            <FooterTemplate>
                                </div>
                            </FooterTemplate>
                        </asp:Repeater>
                        <p></p>
                        
                        <!-- pager and selection -->
                        <div class="sw-search-results-pager">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
							    <tr>
								    <td colspan="5"><asp:Label CssClass="header-A" ID="lblBottomResultsXofY" runat="server" /></td>
							    </tr>
				                <tr>
		                            <td align="left" nowrap="nowrap" valign="middle" colspan="5"><img src="/images/spacer.gif" width="1" height="5" alt="" border="0" /></td>
					            </tr>
				                <tr>
								    <td valign="middle" style="color:#4d4d4d;"><asp:Label id="lblDDLPageUnitShowText" Text="Show" runat="server" style="margin-right:5px"/></td>
								    <td nowrap="nowrap" valign="middle" style="color:#4d4d4d;">
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
								    </td>
								    <td nowrap="nowrap" valign="middle" width="100%"><asp:Label id="lblDDLPageUnitResultsPPText" Text="results per page." runat="server" style="margin-left:5px"/></td>
								    <td nowrap="nowrap">&nbsp;&nbsp;</td>
								    <td valign="middle" align="right">
                                        <cc1:simplepager 
								    ID="spPager" runat="server">								    
								    </cc1:simplepager></td>
							    </tr>						
						    </table>
						</div>
						    <div style="border: 1px solid #bdbdbd; padding: 5px 5px 18px 5px; margin: 28px 0 12px 0;">
                                <asp:Label CssClass="header-A" ID="lblSearchWithinResultsFound" runat="server" />
                                <asp:Label ID="lblSearchWithinResultKeyword" runat="server" />
	                            <p></p>
           						<asp:Panel ID="pnlSWR" runat="server">
	                                <table border="0" cellpadding="0" cellspacing="0" bgcolor="#ffffff">
		                                <tr>
			                                <td width="10%">&nbsp;</td>
			                                <td align="center">
				                                <table cellpadding="0" cellspacing="0" border="0">
					                                <tr>
						                                <td valign="bottom" align="right" style="padding-bottom: 2px;">
							                                <asp:RadioButtonList ID="rblSWRSearchType" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
							                                    <asp:ListItem Selected="True" Text="New Search&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Value="1" />
							                                    <asp:ListItem Text="Search Within Results" Value="2" />
							                                </asp:RadioButtonList>
						                                </td>
						                                <td><img src="/images/spacer.gif" border="0" width="1" height="20" alt=""></td>
					                                </tr>
					                                <tr>
						                                <td align="right">
						                                <asp:Label AssociatedControlID="txtSWRKeyword" ID="lblSWRKeywordLabel" runat="server" CssClass="hidden">Keyword</asp:Label>
						                                <asp:TextBox ID="txtSWRKeyword" Rows="40" MaxLength="40" style="WIDTH:420px" runat="server" /></td>
						                                <td align="left" valign="middle" nowrap>&nbsp;&nbsp;&nbsp;&nbsp;
						                                    <asp:ImageButton id="btnSWRImgSearch" ImageUrl="/images/search_site.gif" AlternateText="Search" runat="server" OnClick="SearchWithinResults" />
						                                    <asp:Button id="btnSWRTxtSearch" text="Search >" runat="server" OnClick="SearchWithinResults" /></td>
					                                </tr>
				                                </table>
			                                </td>
			                                <td width="10%">&nbsp;</td>
		                                </tr>
	                                </table>
                   				</asp:Panel>
           				    </div>
                    </form>                    				
				</td>
				
				<td valign="top"><img src="/images/spacer.gif" width="10" height="1" alt="" border="0"></td>
			</tr>
		</table>
	</div>
	<!-- End Main Area -->
