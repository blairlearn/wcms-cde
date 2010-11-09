<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClinicalTrialsResults.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ClinicalTrialsResults" %>
   
<%@ Register TagPrefix="CGov" assembly="CancerGovUIControls" namespace="NCI.Web.UI.WebControls.FormControls" %>    
<%@ Register TagPrefix="CGov" TagName="CustomSectionSelector" Src="~/UserControls/CTCustomSelection.ascx" %>
<%@ Register TagPrefix="CGov" assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls" %>    


<script type="text/javascript">
    var ids = {
    checkAllTop:"<%=checkAllTop.ClientID %>"
    ,checkAllBottom:"<%=checkAllBottom.ClientID %>"
    ,pageSize:"<%=pageSize.ClientID %>"
    ,OffPageSelectionsExist:"<%=OffPageSelectionsExist.ClientID%>"
    ,customFormat:"<%=customFormat.ClientID%>"
    ,titleFormat:"<%=titleFormat.ClientID%>"
    ,healthProfAudience:"<%=healthProfAudience.ClientID%>"
    ,includeLocations:"<%=includeLocations.ClientID%>"
    ,includeEligibility:"<%=includeEligibility.ClientID%>"
    ,descriptionFormat:"<%=descriptionFormat.ClientID%>"
    ,DisplaySearchCriteriaCollapsed:"<%=DisplaySearchCriteriaCollapsed.ClientID%>"
    , CriteriaDisplay: "<%=CriteriaDisplay.ClientID%>"
    , advResultForm: "<%=advResultForm.ClientID%>"
        }
</script>
    
<td id="contentzone" valign="top" width="*">
    <a name="skiptocontent"></a>
    <form name="advResultForm" id="advResultForm" runat="server" method="post">
    <asp:Panel ID="CustomSectionsDisplay" runat="server" Visible="false">
        <cgov:customsectionselector id="customSections" runat="server" onselectionschanged="CustomSectionsChanged" />
    </asp:Panel>
    <asp:Panel ID="ResultsDisplay" runat="server" Visible="true">
        <!-- For use by JavaScript code -->
        <asp:HiddenField ID="pageSize" runat="server" />
        <asp:HiddenField ID="OffPageSelectionsExist" runat="server" EnableViewState="false" />
        <!-- Top View Content for box -->
        <div class="clinicaltrials-filledbox" style="width: 568px;">
            <asp:Panel runat="server" ID="ResultsFormatControl" Style="border-bottom: 1px #bdbdbd solid;">
                <table width="554" cellpadding="0" cellspacing="8" border="0" style="background: #ffffff;
                    margin: 6px;" align="center">
                    <tr>
                        <td valign="top" width="32%" style="border-right: 1px dotted #bdbdbd;">
                            <span style="color: #cb060d; font-size: 14px; font-weight: bold;">View Content for:</span>
                            <ul class="clinicaltrials-results-formatcontrols">
                                <li>
                                    <asp:RadioButton runat="server" ID="patientAudience" GroupName="AudienceType" Text="Patients"
                                        AutoPostBack="false" CssClass="black-text" /></li>
                                <li>
                                    <asp:RadioButton runat="server" ID="healthProfAudience" GroupName="AudienceType"
                                        Text="Health Professionals" AutoPostBack="false" CssClass="black-text" /></li>
                            </ul>
                        </td>
                        <td valign="top" width="62%" style="padding-left: 15px;">
                            <span style="color: #cb060d; font-size: 14px; font-weight: bold;">Display:</span>
                            <ul class="clinicaltrials-results-formatcontrols">
                                <li>
                                    <asp:RadioButton runat="server" ID="titleFormat" GroupName="DisplayFormat" Text="Title"
                                        CssClass="black-text" /></li>
                                <li>
                                    <asp:RadioButton runat="server" ID="descriptionFormat" GroupName="DisplayFormat"
                                        Text="Description with:" CssClass="black-text" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="includeLocations" Text="Locations"
                                        CssClass="black-text" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox runat="server" ID="includeEligibility" Text="Eligibility"
                                        CssClass="black-text" />
                                </li>
                                <li>
                                    <asp:RadioButton runat="server" ID="fullDescriptionFormat" GroupName="DisplayFormat"
                                        Text="Full Trial Description" CssClass="black-text" /></li>
                                <li>
                                    <asp:RadioButton runat="server" ID="customFormat" GroupName="DisplayFormat" Text="Custom"
                                        CssClass="black-text" /></li>
                            </ul>

                            <script type="text/javascript">
                                document.observe("dom:loaded", function() {
                                    Event.observe($("<% =patientAudience.ClientID%>"), "click", AudienceType_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =healthProfAudience.ClientID%>"), "click", AudienceType_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =titleFormat.ClientID%>"), "click", FormatType_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =descriptionFormat.ClientID%>"), "click", FormatType_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =fullDescriptionFormat.ClientID%>"), "click", FormatType_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =customFormat.ClientID%>"), "click", FormatType_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =includeLocations.ClientID%>"), "click", DescriptionSubtype_clickhandler.bindAsEventListener(this));
                                    Event.observe($("<% =includeEligibility.ClientID%>"), "click", DescriptionSubtype_clickhandler.bindAsEventListener(this));
                                });
                            </script>

                        </td>
                        <td valign="top" width="6%" align="right">
                            <a target="new" href="<% =SearchHelpPrettyUrl %>/page3#1">
                                <img src="/images/ctsearch/gray-question.gif" alt="Help with display options." width="15"
                                    height="15" style="margin-bottom: 70px;" /></a>
                            <asp:ImageButton ID="UpdateAudienceAndDisplay" runat="server" ImageUrl="/images/ctsearch/go-btn-red.gif"
                                Width="24" Height="15" AlternateText="Go" OnClick="UpdateAudienceAndDisplay_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div style="padding: 6px;">
                <strong>
                    <asp:Literal ID="ResultsCountText" runat="server" /></strong>
            </div>
        </div>
        <!-- Top View Content For box -->
        <asp:Panel ID="NoResultsMessage" Visible="false" runat="server">
            <p>
                There were no clinical trials that matched the search criteria that you entered.
                Remember that the more criteria you enter, the fewer results you are likely to get.</p>
        </asp:Panel>
        <div style="width: 100%;">
            <cgov:ctsearchcriteriadisplay runat="server" id="CriteriaDisplay" cssclass="clinicaltrials-results-criteria-display" />
        </div>
        <asp:HiddenField ID="DisplaySearchCriteriaCollapsed" runat="server" />
        <div id="hideCriteriaLink" style="display: none; padding: 0px 8px 0px;">
            <a class="clinicaltrials-collapseLink" href="javascript:toggleSearchCriteria()">Hide
                Search Criteria</a></div>
        <div id="showCriteriaLink" style="display: none; padding: 8px 8px 0px;">
            <a class="clinicaltrials-expansionLink" href="javascript:toggleSearchCriteria()">Show
                Search Criteria</a></div>

        <script type="text/javascript" language="javascript">            document.observe("dom:loaded", function() { SetSearchCriteriaDisplay(); });</script>

        <div class="clinicaltrials-results-action-controls">
            <div style="float: left;">
                <asp:ImageButton ID="TopPrintButton" class="action-button" runat="server" AlternateText="Print Selected"
                    Width="89" Height="15" ImageUrl="/images/ctsearch/btn-print-selected-red.gif"
                    OnClick="DisplayForPrint_ClickHandler" />
                <input id="refineSearch1" class="action-button" runat="server" alt="Refine Search"
                    border="0" height="15" width="85" onserverclick="refineSearch_ServerClick" src="/images/ctsearch/btn-refine-search-gray.gif"
                    type="image" />

                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<% =TopPrintButton.ClientID %>") != null)
                            Event.observe($("<% =TopPrintButton.ClientID %>"), "click", submitPrint_ClickHandler.bindAsEventListener(this));
                    });
                </script>

                <a id="newSearch1" href="<% =SearchPageInfo.SearchPagePrettyUrl %>">
                    <img alt="Start Over" border="0" height="15" width="70" src="/images/CTSearch/grey_start_over_btn.gif" /></a>
            </div>
            <div style="float: right;">
                <a target="new" href="<% =SearchHelpPrettyUrl %>/page3">Help with Results</a>
            </div>
            <div style="clear: both;">
            </div>
        </div>
        <asp:Panel ID="topControlArea" runat="server" CssClass="clinicaltrials-filledbox"
            Style="color: #000000; font-weight: bold; padding: 8px 8px 8px 8px;">
            <span style="float: left;">
                <asp:CheckBox EnableViewState="false" runat="server" ID="checkAllTop" Text="Select All on Page" />

                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<%=checkAllTop.ClientID %>") != null)
                            Event.observe($("<%=checkAllTop.ClientID %>"), "click", checkAll_ClickHandler.bindAsEventListener(this));
                    });
                </script>

                &nbsp;&nbsp;
                <label for="sortOrder">
                    Sort by:</label>&nbsp;
                <asp:DropDownList ID="sortOrder" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                &nbsp;&nbsp; Show
                <asp:DropDownList ID="resultsPerPage" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                <label for="resultsPerPage">
                    Results per Page</label>
            </span><span style="float: right; margin-top: 4px;">
                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="/images/ctsearch/go-btn-red.gif"
                    Width="24" Height="15" AlternateText="Go" OnClick="UpdateSortOrderAndPageSize_Click" />
            </span>
            <div style="clear: both;">
            </div>
        </asp:Panel>
        <asp:Literal runat="server" ID="ProtocolContent" EnableViewState="false" />
        <asp:Panel ID="lowerControlArea" runat="server" CssClass="clinicaltrials-filledbox"
            Style="padding: 8px;">
            <div style="float: left;">
                <asp:CheckBox EnableViewState="false" runat="server" ID="checkAllBottom" CssClass="black-text-b"
                    Text="Select All on Page" />

                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<% =checkAllBottom.ClientID %>") != null)
                            Event.observe($("<% =checkAllBottom.ClientID %>"), "click", checkAll_ClickHandler.bindAsEventListener(this));
                    });
                </script>

            </div>
            <div style="float: right;">
                <cgov:postbackbuttonpager id="pager" runat="server" cssclass="clinicaltrials-pager"
                    onpagechanged="PageChanged" shownumpages="3">
                                            <PagerStyleSettings NextPageText="Next &gt;" PrevPageText="&lt; Prev" />
                                        </cgov:postbackbuttonpager>
            </div>
            <div style="clear: both;">
            </div>
        </asp:Panel>
        <asp:Panel ID="BottomActionControls" runat="server" CssClass="clinicaltrials-results-action-controls">
            <div style="float: left;">
                <asp:ImageButton ID="BottomPrintButton" class="action-button" runat="server" AlternateText="Print Selected"
                    Width="89" Height="15" ImageUrl="/images/ctsearch/btn-print-selected-red.gif"
                    OnClick="DisplayForPrint_ClickHandler" />
                <input id="refineSearch" class="action-button" runat="server" alt="Refine Search"
                    border="0" height="15" width="85" onserverclick="refineSearch_ServerClick" src="/images/ctsearch/btn-refine-search-gray.gif"
                    type="image" />

                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<%=BottomPrintButton.ClientID%>") != null)
                            Event.observe($("<%=BottomPrintButton.ClientID%>"), "click", submitPrint_ClickHandler.bindAsEventListener(this));
                    });
                </script>

                <a id="newSearch" href="<% =SearchPageInfo.SearchPagePrettyUrl %>">
                    <img alt="New Search" border="0" height="15" width="70" src="/images/grey_start_over_btn.gif" /></a>
            </div>
            <div style="float: right;">
                <a target="new" href="<% =SearchHelpPrettyUrl %>/page3">Help with Results</a>
            </div>
            <div style="clear: both;">
            </div>
        </asp:Panel>
    </asp:Panel>
    <!--start new form footnote-->
    <p>
        <a onclick="dynPopWindow('https://cissecure.nci.nih.gov/livehelp/welcome.asp', '', 'width=620px,menubar=no,location=no,height=465px,scrollbar=yes'); return false;"
            href="https://cissecure.nci.nih.gov/livehelp/welcome.asp">
            <img src="/images/ctsearch/livehelp.gif" alt="Having trouble with this form? Check the help page or contact an NCI information specialist through LiveHelp online text chat or by calling 1-800-4-CANCER." /></a></p>
    <!--end new form footnote-->
    <input type="hidden" value=<% =GetProtocolSearchID().ToString() %> name="protocolsearchid" id="protocolsearchid" />
    </form>
</td>
