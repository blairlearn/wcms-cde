<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintClinicalTrialsView.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.PrintClinicalTrialsView" %>


    <div class="clinical-trials-print">

          <!-- Version tab and Date -->

  <div class="clinical-trial-version-date"> <a href="javascript:window.print();">Print</a> |

    <asp:HyperLink id="EmailResults" runat="server">E-Mail These Results</asp:HyperLink>

    | <a id="newSearch" href="<% =SearchPageInfo.SearchPagePrettyUrl %>">New Search</a> </div>

  <!-- end Version tab and Date -->

  <p><span>Please Note:</span><br />

    The search results shown are the set of trials that you

    selected for printing in the display format you chose. The URL for this page will be stored

    for 90 days, and the same display of trials will be available if you view the page during

    that time frame. The title of each trial is linked to a “live” version of the trial that will

    show the most current information about the trial, including any updates made since the day

    the search was performed.</p>

  <p class="date-search">Date of Search:&nbsp;

    <asp:Literal ID="cacheDate" runat="server" />

  </p>

  <asp:Literal ID="litPageContent" runat="server" />
    </div>
