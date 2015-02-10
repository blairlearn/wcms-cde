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
    

    <a name="skiptocontent"></a>
    <form name="advResultForm" id="advResultForm" runat="server" method="post">
    <asp:Panel ID="CustomSectionsDisplay" runat="server" Visible="false">
        <cgov:customsectionselector id="customSections" runat="server" onselectionschanged="CustomSectionsChanged" />
    </asp:Panel>
    <asp:Panel ID="ResultsDisplay" runat="server" Visible="true">
        <!-- For use by JavaScript code -->
        <asp:HiddenField ID="pageSize" runat="server" />
        <asp:HiddenField ID="OffPageSelectionsExist" runat="server" EnableViewState="false" />
        <p>
        Initial search results include only NCI-sponsored clinical trials. To search all trials, click the "REFINE SEARCH" 
        button, scroll down to the Trial ID/Sponsor section and select the "All" check box in the Sponsor of Trial section.
        </p>
        <!-- Top View Content for box -->
        <div class="row">
            <asp:Panel runat="server" ID="ResultsFormatControl" CssClass="medium-11 columns">
                <div class="row view-content-for-container">
                  
                        <div class="medium-4 columns">
                            <strong>View Content for:</strong><br />
                            <div class="radio">
                                    <asp:RadioButton runat="server" ID="patientAudience" GroupName="AudienceType" Text="Patients"
                                        AutoPostBack="false" CssClass="black-text" />
                             </div><br />
                              <div class="radio"> 
                                    <asp:RadioButton runat="server" ID="healthProfAudience" GroupName="AudienceType"
                                        Text="Health Professionals" AutoPostBack="false" CssClass="black-text" />
                              </div>
                            
                        </div>
                        <div class="medium-8 columns">
                            <strong>Display:</strong><br />
                            
                                <div class="radio">
                                    <asp:RadioButton runat="server" ID="titleFormat" GroupName="DisplayFormat" Text="Title"
                                        CssClass="black-text" /></div><br />
                                  <div>
                                 <span class="radio">
                                    <asp:RadioButton runat="server" ID="descriptionFormat" GroupName="DisplayFormat"
                                        Text="Description with:" CssClass="black-text" /></span>
                                    <span class="checkbox"><asp:CheckBox runat="server" ID="includeLocations" Text="Locations"
                                        CssClass="black-text" /></span>
                                    <span class="checkbox"><asp:CheckBox runat="server" ID="includeEligibility" Text="Eligibility"
                                        CssClass="black-text" /></span>
                                      
                                </div>
                                <br />
                                     <div class="radio"><asp:RadioButton runat="server" ID="fullDescriptionFormat" GroupName="DisplayFormat"
                                        Text="Full Trial Description" CssClass="black-text" /></div><br />
                                
                                     <div class="radio"><asp:RadioButton runat="server" ID="customFormat" GroupName="DisplayFormat" Text="Custom"
                                        CssClass="black-text" /></div>
                            

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
                            <br />
                            <div style="float: right">
                            <asp:Button ID="UpdateAudienceAndDisplay" runat="server"
                                CssClass="submit button" Text="Go" AlternateText="Go" OnClick="UpdateAudienceAndDisplay_Click" />
                             </div>
                        </div>
                        
                  
                </div>
                       
            </asp:Panel>
             <div class="medium-1 columns">
                  <a class="text-icon-help" aria-label="Help" target="new" href="/clinicaltrials/search-form-help/page3#1">?</a>
                 </div> 
        </div>
        
        <div class="row search-criteria-box">
        <div class="medium-8 columns">
         <div id="hideCriteriaLink" class="hide-criteria-link">
            <a href="javascript:toggleSearchCriteria()">Hide
                Search Criteria</a></div>
        <div id="showCriteriaLink" class="show-criteria-link">
            <a href="javascript:toggleSearchCriteria()">Show
                Search Criteria</a></div>

        <div>
            <cgov:ctsearchcriteriadisplay runat="server" id="CriteriaDisplay" cssclass="clinicaltrials-results-criteria-display" />
        </div>
        <asp:HiddenField ID="DisplaySearchCriteriaCollapsed" runat="server" />
         <script type="text/javascript" language="javascript">             document.observe("dom:loaded", function() { SetSearchCriteriaDisplay(); });</script>
        </div>
       </div>
       <div>
                <h5>
                    <asp:Literal ID="ResultsCountText" runat="server" /></h5>
            </div>
        <!-- Top View Content For box -->
        <asp:Panel ID="NoResultsMessage" Visible="false" runat="server">
            <p>
                There were no clinical trials that matched the search criteria that you entered.
                Remember that the more criteria you enter, the fewer results you are likely to get.</p>
        </asp:Panel>
        <div class="row">
            <div class="medium-8 columns">
                <asp:Button ID="TopPrintButton" class="action button" runat="server" AlternateText="Print Selected" Text="Print Selected" OnClick="DisplayForPrint_ClickHandler" />
                <input id="refineSearch1" class="action button" runat="server" alt="Refine Search"
                    value="Refine Search" onserverclick="refineSearch_ServerClick"
                    type="submit" />

                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<% =TopPrintButton.ClientID %>") != null)
                            Event.observe($("<% =TopPrintButton.ClientID %>"), "click", submitPrint_ClickHandler.bindAsEventListener(this));
                    });
                </script>

                <a id="newSearch1" href="<% =SearchPageInfo.SearchPagePrettyUrl %>" class="reset startover button">
                    Start Over</a>
            </div>
           
            <div style="clear: both;">
            </div>
        </div>
        <asp:Panel ID="topControlArea" runat="server" class="row">
           
           <div class=\"medium-1 columns\">
            <div class="checkbox">
                <asp:CheckBox EnableViewState="false" runat="server" ID="checkAllTop" Text="<strong>Select All on Page</strong>" onclick="checkAll_ClickHandler(this)" />
               </div>

<%--                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<%=checkAllTop.ClientID %>") != null)
                            Event.observe($("<%=checkAllTop.ClientID %>"), "click", checkAll_ClickHandler.bindAsEventListener(this));
                    });
                </script>
--%>
              </div> 
               <div class=\"medium-11 columns\">
                <label for="<%=sortOrder.ClientID%>">
                    <strong>Sort by:</strong></label>
                <asp:DropDownList ID="sortOrder" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                 <strong>Show</strong>
                <asp:DropDownList ID="resultsPerPage" runat="server" AutoPostBack="false">
                </asp:DropDownList>
                <label for="<%=resultsPerPage.ClientID%>">
                     <strong>Results per Page</strong></label>
           
                <asp:Button ID="Button1" runat="server" CssClass="submit button"
                    AlternateText="Go" Text="Go" OnClick="UpdateSortOrderAndPageSize_Click" />
               </div>
           
            
            <div style="clear: both;" />
 
        </asp:Panel>
        <asp:Literal runat="server" ID="ProtocolContent" EnableViewState="false" />
        <asp:Panel ID="lowerControlArea" runat="server">
            <div class="checkbox">
                <asp:CheckBox EnableViewState="false" runat="server" ID="checkAllBottom"
                    Text="<strong>Select All on Page</strong>" onclick="checkAll_ClickHandler(this)" />

 <%--               <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<% =checkAllBottom.ClientID %>") != null)
                            Event.observe($("<% =checkAllBottom.ClientID %>"), "click", checkAll_ClickHandler.bindAsEventListener(this));
                    });
                </script>
--%>
            </div>
            <div style="float: right;">
                <cgov:postbackbuttonpager id="pager" runat="server" cssclass="pagination"
                    onpagechanged="PageChanged" shownumpages="3">
                                            <PagerStyleSettings NextPageText="Next &gt;" PrevPageText="&lt; Prev" />
                                        </cgov:postbackbuttonpager>
            </div>
            <div style="clear: both;">
            </div>
        </asp:Panel>
        <asp:Panel ID="BottomActionControls" runat="server" CssClass="row">
            <div class="medium-8 columns">
                <asp:Button ID="BottomPrintButton" class="action button" runat="server" AlternateText="Print Selected" Text="Print Selected" OnClick="DisplayForPrint_ClickHandler" />
                <input id="refineSearch" class="action button" runat="server" alt="Refine Search"
                    value="Refine Search" onserverclick="refineSearch_ServerClick"
                    type="submit" />

                <script type="text/javascript">
                    document.observe("dom:loaded", function() {
                        if ($("<%=BottomPrintButton.ClientID%>") != null)
                            Event.observe($("<%=BottomPrintButton.ClientID%>"), "click", submitPrint_ClickHandler.bindAsEventListener(this));
                    });
                </script>


                <a id="newSearch" href="<% =SearchPageInfo.SearchPagePrettyUrl %>" class="reset startover button">
                    Start Over</a>
            </div>
           
            <div style="clear: both;">
            </div>
        </asp:Panel>
    </asp:Panel>
    <!--start new form footnote-->
    <!--end new form footnote-->
    </form>

