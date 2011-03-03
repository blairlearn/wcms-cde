<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintClinicalTrialsView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.PrintClinicalTrialsView" %>


    <div align="center"; style="width: 771px; margin: 5px auto 0px auto; text-align: left;">

          <!-- Version tab and Date --> 
          <table width="100%" cellspacing="0" cellpadding="0" border="0">
             <tr>
                <td valign="top"><img src="/images/spacer.gif" width="1" height="1" alt="" border="0"></td>
             </tr>
             <tr>
                <td valign="top" align="right" class="red-text"><img src="/images/spacer.gif" width="400" height="4" alt="" border="0"><br />
                    <a href="javascript:window.print();">Print</a> | <asp:HyperLink id="EmailResults" runat="server">E-Mail These Results</asp:HyperLink> | <a id="newSearch" href="<% =SearchPageInfo.SearchPagePrettyUrl %>">New Search</a>                    
                <td valign="top"><img src="/images/spacer.gif" width="10" height="1" alt="" border="0" /></td>
             </tr>
          </table>
          <!-- end Version tab and Date -->
             <table cellspacing="0" cellpadding="0" border="0">
             <tr>
                <td valign="top">
			        <a name="skiptocontent"></a>
		        </td>
	        </tr>
	        </table>
	        <p style="border: solid 1px #bdbdbd; padding: 5px;"><span class="red-text" style="font-weight: bold; font-size: 14px;">Please Note:</span><br />The search results shown are the set of trials that you
	        selected for printing in the display format you chose. The URL for this page will be stored
	        for 90 days, and the same display of trials will be available if you view the page during
	        that time frame. The title of each trial is linked to a “live” version of the trial that will
	        show the most current information about the trial, including any updates made since the day
	        the search was performed.</p>
	        <p style="text-align: right;">Date of Search:&nbsp;<asp:Literal ID="cacheDate" runat="server" /></p>
	        <asp:Literal ID="litPageContent" runat="server" />
    </div>
