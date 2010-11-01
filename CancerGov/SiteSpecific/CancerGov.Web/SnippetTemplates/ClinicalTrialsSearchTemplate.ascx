<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClinicalTrialsSearchTemplate.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ClinicalTrialsSearchTemplate" %>
<%@ Register assembly="CancerGovUIControls" namespace="NCI.Web.UI.WebControls.FormControls" tagprefix="CancerGov" %>
<script type="text/javascript">
    var ids = {
    drugListArea:               "<%=drugListArea.ClientID %>"
    , drugListExpanded:         "<%=drugListExpanded.ClientID %>"
    , interventionListArea:     "<%=interventionListArea.ClientID %>"
    , trialStatusExpanded:      "<%=trialStatusExpanded.ClientID %>"
    , hospitalLocationButton: "<%=hospitalLocationButton.ClientID %>"
    , zipCodeLocationButton: "<%=zipCodeLocationButton.ClientID %>"
    , cityStateLocationButton:"<%=cityStateLocationButton.ClientID %>"
    , atNihLocationButton:"<%=atNihLocationButton.ClientID %>"
    , LocationSelection:"<%=LocationSelection.ClientID %>"
    , zipCodeLocationButtonStatic:"<%=zipCodeLocationButtonStatic.ClientID %>"
    , cityStateLocationButtonStatic:"<%=cityStateLocationButtonStatic.ClientID %>"
    , hospitalLocationButtonStatic:"<%=hospitalLocationButtonStatic.ClientID %>"
    , atNihLocationButtonStatic:"<%=atNihLocationButtonStatic.ClientID %>"
    , country:"<%=country.ClientID %>"
    , city:"<%=city.ClientID %>"
    , state:"<%=state.ClientID %>"
    , hospitalBox:"<%=hospitalBox.ClientID %>"
    , showInstitutionListButton:"<%=showInstitutionListButton.ClientID %>"
    , treatmentTypeAreaExpanded:"<%=treatmentTypeAreaExpanded.ClientID %>"
    , interventionListArea:"<%=interventionListArea.ClientID %>"
    , interventionListExpanded:"<%=interventionListExpanded.ClientID %>"
    , trialSponsorExpanded:"<%=trialSponsorExpanded.ClientID %>"
    , trialInvestigatorsRow:"<%=trialInvestigatorsRow.ClientID %>"
    , investigatorListExpanded:"<%=investigatorListExpanded.ClientID %>"
    , investigatorid:"<%=investigatorid.ClientID %>"
    , trialLeadOrganizationRow:"<%=trialLeadOrganizationRow.ClientID %>"
    , leadOrgListExpanded: "<%=leadOrgListExpanded.ClientID %>"
    , institutionListExpanded: "<%=institutionListExpanded.ClientID %>"
    , institutionListSubBox: "<%=institutionListSubBox.ClientID %>"    
    , institution: "<%=institution.ClientID%>"
    , drug: "<%=drug.ClientID%>"
    , intervention:"<%=intervention.ClientID%>"
    , investigator:"<%=investigator.ClientID%>"
    , leadOrg:"<%=leadOrg.ClientID%>"
     }
</script>    
<style type="text/css">
/* IE doesn't include the drop down button in its width calculation,
   so for IE only we need to shrink the drop down. */
#country {
    width: 100%;
}
* html #country {
    width: 85%;
}

#trialPhase tr:first-child td
{
    height: 28px;
    vertical-align: top;
}

div.locationGroupbox
{
    margin: 10px 0 0 0;
    padding: 10px;
}
 
div.scrollingList
{
    background-color: #ffffff;
    border: solid 1px #bdbdbd;
    overflow: scroll;
    white-space: nowrap;
}
 
div.scrollingListDisabled
{
    background-color: #ffffff;
    border: solid 1px #bdbdbd;
    overflow: scroll;
    white-space: nowrap;
    color: #9e9e9e;
}


table.cttable td.column1-noline, table.cttable td.column2-noline, table.cttable td.column3-noline {
    border-top: none;
    padding-bottom: 10px;
}

table.cttable td.column1 {
    padding: 10px 6px;
    text-align: right;
    color: #000000;
    font-weight: bold;
    width: 23%;
}

table.cttable td.column2 {
    padding: 10px 0 10px 10px;
}	

table.cttable td.column3 {
    padding: 13px 0 13px 3px;
    text-align: right;
    width: 5%;
}

table.cttable td.hr {
    padding: 0;
}

table.cttable td.sectionBreak {
    border-top: 1px solid #bdbdbd;
}

table.subtable td.subSectionBreak
{
    border-top: dashed 1px #bdbdbd;
}

table.cttable td.sectionBottom
{
	padding-bottom: 25px;
}

span.gray-text
{
	color: #6e6e6e;
}

#updateSubTypeList
{
	border: none;
	margin-top: 3px;
}

#LocationChooser input[type='radio']
{
	margin-top: 0px;
	padding-top:0px;
}

</style>
<td id="contentzone" valign="top" width="*">
    <a name="skiptocontent"></a>
    <form name="advSearchForm" id="advSearchForm" method="post" runat="server">
    <input type="hidden" value="2" name="searchtype" />
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
        CssClass="clinicaltrials-errorMessage" ValidationGroup="v1" />
    <noscript>
        <div class="clinicaltrials-javascriptOffMessage">
            <p>
                <img src="/images/CTSearch/alert-yellow-icon.gif" width="26" height="25" border="0"
                    alt="Warning" style="vertical-align: middle;" />
                <span>JavaScript is turned off in your browser. Please turn on JavaScript to enable
                    more <a href="/clinicaltrials/search-form-help/page1#1">search options</a>.</span></p>
        </div>
    </noscript>
    <p style="margin-top: 0px;">
        Search NCI's list of
        <asp:Literal ID="CTCountOpen" runat="server"></asp:Literal>
        clinical trials now accepting participants, or use more search options to search
        the set of
        <asp:Literal ID="CTCountClosed" runat="server"></asp:Literal>
        clinical trials that are no longer recruiting.</p>
    <p>
        <strong>Search Tip:</strong> Skip any items that are unknown or not applicable.</p>
    <table width="100%" cellpadding="0" cellspacing="0" border="0" class="cttable">
        <tr valign="top">
            <td class="column1 sectionBreak">
                <label for="cancerType" style="white-space: nowrap;">
                    Cancer Type/Condition</label>
            </td>
            <td class="column2 sectionBreak">
                <asp:DropDownList ID="cancerType" runat="server" Width="100%" AutoPostBack="true"
                    OnSelectedIndexChanged="cancerType_SelectedIndexChanged" CausesValidation="True"
                    ValidationGroup="v1">
                </asp:DropDownList>
                <%-- We don't actually need to specify an event handler for updateSubTypeList.  Clicking it
                                        causes a postback event which allows the change in cancerType to be picked up and throw
                                        its own update event. --%>
                <asp:ImageButton ID="updateSubTypeList" runat="server" ImageUrl="/images/CTSearch/btn-stage-subtype.gif"
                    AlternateText="Show Stage/Subtype" />

                <script language="javascript" type="text/javascript">                    document.observe("dom:loaded", function() { $("<% =updateSubTypeList.ClientID %>").hide(); });</script>

            </td>
            <td class="column3 sectionBreak">
                <a href="/clinicaltrials/search-form-help/page2#1" target="new">
                    <img src="/images/ctsearch/gray-question.gif" width="15" height="15" border="0" alt="Help!" /></a>
            </td>
        </tr>
        <tr id="cancerStageArea" runat="server">
            <td valign="top" class="column1 sectionBottom">
                <span class="gray-text">Stage/Subtype</span>
            </td>
            <td valign="top" class="column2 sectionBottom">
                <div id="Div1" class="scrollingList" style="width: 400px; height: 136px;" runat="server">
                    <p id="emptySubType" style="margin-left: 5px; margin-top: 5px;" runat="server" visible="true">
                        Select cancer type/condition first.</p>
                    <cancergov:smartcheckboxlist id="cancerStage" runat="server" repeatcolumns="1" repeatlayout="Flow" />
                </div>
            </td>
            <td valign="middle" class="column3 sectionBottom">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td valign="top" class="column1 sectionBreak sectionBottom" style="padding-top: 14px;">
                Location
            </td>
            <td valign="top" class="column2 sectionBreak sectionBottom">
                <table id="LocationChooser" style="display: none;" width="300" cellpadding="4" cellspacing="0"
                    border="0">
                    <tr>
                        <td valign="top" style="vertical-align: top;">
                            <input type="radio" id="zipCodeLocationButton" name="LocationChooser" value="zip"
                                runat="server" style="" /><label for="zipCodeLocationButton" style="margin-top: 0px;
                                    padding-top: 0px;">Near ZIP Code</label>
                        </td>
                        <td valign="top">
                            <input type="radio" id="hospitalLocationButton" name="LocationChooser" value="hospital"
                                runat="server" /><label for="hospitalLocationButton">At&nbsp;Hospital/Institution</label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <input type="radio" id="cityStateLocationButton" name="LocationChooser" value="city"
                                runat="server" /><label for="cityStateLocationButton">In&nbsp;City/State/Country</label>
                        </td>
                        <td valign="top">
                            <input type="radio" id="atNihLocationButton" name="LocationChooser" value="nih" runat="server" /><label
                                for="atNihLocationButton">At NIH</label>
                        </td>
                    </tr>
                </table>
                <%--Hidden value for client-side use in keeping track of the selected location
                                        for use in handling browser back-arrow navigation.--%>
                <asp:HiddenField ID="LocationSelection" runat="server" EnableViewState="false" />
                <div id="zipCodeBox">
                    <div class="locationGroupbox clinicaltrials-filledbox">
                        <input type="radio" id="zipCodeLocationButtonStatic" name="LocationChooser" value="zip-static"
                            runat="server" checked />
                        <label for="zipCodeLocationButtonStatic">
                            <span style="font-weight: bold; color: #000000;">Near ZIP Code</span></label><br />
                        <div style="padding: 5px 0 0 40px; line-height: 25px;">
                            <label for="zipCodeProximity">
                                Show trials located within:</label><br />
                            <asp:DropDownList ID="zipCodeProximity" runat="server">
                                <asp:ListItem Value="20">20 miles</asp:ListItem>
                                <asp:ListItem Value="50">50 miles</asp:ListItem>
                                <asp:ListItem Value="100" Selected="True">100 miles</asp:ListItem>
                                <asp:ListItem Value="200">200 miles</asp:ListItem>
                                <asp:ListItem Value="500">500 miles</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;&nbsp;of
                            <asp:Label ID="Label1" AssociatedControlID="zipCode" runat="server">ZIP Code:</asp:Label>&nbsp;&nbsp;<asp:TextBox
                                ID="zipCode" MaxLength="5" Columns="8" runat="server" ValidationGroup="v1"></asp:TextBox>
                            <br />
                            <span style="padding-left: 165px;"><a onclick="javascript:dynPopWindow('http://zip4.usps.com/zip4/citytown.jsp', '', 'width=740px,menubar=no,location=no,height=465px,scrollbar=yes'); return(false);"
                                href="http://zip4.usps.com/zip4/citytown.jsp">ZIP Code Lookup</a></span></div>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="zipCode"
                            ErrorMessage="&lt;p&gt;&lt;img src=&quot;/images/CTSearch/error-red-icon.gif&quot; width=&quot;26&quot; height=&quot;25&quot; border=&quot;0&quot; alt=&quot;Error&quot; style=&quot;vertical-align:middle;&quot; /&gt;  &lt;span&gt;&nbsp;Please enter a zip code with 5 numeric characters.&lt;/span&gt;&lt;/p&gt;"
                            ValidationExpression="\d{5}" Display="Dynamic" ValidationGroup="v1"><div 
                                                    class="clinicaltrials-errorMessageSmall"><p><img src="/images/CTSearch/error-red-icon.gif" width="26" height="25" border="0" alt="Error" style="vertical-align:middle;" /><span>Please enter a zip code with 5 numeric characters.</span></p></div></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div id="cityStateZipBox">
                    <noscript>
                        <p>
                            <b>OR</b></p>
                    </noscript>
                    <div class="locationGroupbox clinicaltrials-filledbox">
                        <input type="radio" id="cityStateLocationButtonStatic" name="LocationChooser" value="city-static"
                            runat="server" />
                        <label for="cityStateLocationButtonStatic">
                            <span style="font-weight: bold; color: #000000;">In City/State/Country</span></label><br />
                        <div style="padding: 5px 0 0 40px; line-height: 25px;">
                            <label for="country">
                                Country:</label><br />
                            <!-- Width set in style section -->
                            <select id="country" onchange="country_onChange(this);" name="country" runat="server">
                            </select>
                        </div>
                        <div style="padding: 5px 0 0 40px; line-height: 25px;">
                            <label for="city">
                                City:</label><br />
                            <input id="city" type="text" size="14" name="city" style="width: 100%;" runat="server" />
                        </div>
                        <div style="padding: 5px 0 0 40px; line-height: 25px;">
                            State:<br />
                            <div id="locationStateList" class="scrollingList" style="width: 50%; height: 170px;">
                                <cancergov:smartcheckboxlist id="state" runat="server" repeatcolumns="1" repeatlayout="Flow" />
                            </div>
                        </div>
                    </div>
                </div>
                <div id="hospitalBox" runat="server">
                    <noscript>
                        <p>
                            <b>OR</b></p>
                    </noscript>
                    <div class="locationGroupbox  clinicaltrials-filledbox">
                        <input type="radio" id="hospitalLocationButtonStatic" name="LocationChooser" value="hospital-static"
                            runat="server" />
                        <span style="font-weight: bold; color: #000000;">
                            <label for="hospitalLocationButtonStatic">
                                At Hospital/Institution:</label></span>
                        <input type="image" id="showInstitutionListButton" src="/images/choose_from_list_btn.gif"
                            alt="Choose From List" style="margin-left: 15px; vertical-align: middle;" runat="server" /><br />
                        <input type="hidden" id="institutionListExpanded" value="N" runat="server" />
                        <input id="institutionid" type="hidden" size="18" name="institutionid" runat="server" />
                        <div id="institutionListSubBox" style="padding: 5px 0 5px 40px;" runat="server">
                            <cancergov:deletelist id="institution" deleteiconurl="~/Images/delete_item.gif" width="350"
                                height="110" runat="server" emptylisttext="Select &quot;Add More&quot; to see hospital names.">
											        </cancergov:deletelist>
                            <span id="institutionAddButton" style="display: none;"><a href="javascript:dynPopWindow('/common/popups/CTLSearch/CTLookup.aspx?type=<% =institution.ClientID %>&amp;fld=institution&amp;title=Find+Hospitals/Institutions', 'InstitutionLookup', 'width=681px,menubar=no,location=no,height=580px');">
                                <img height="15" alt="Add More" src="/images/add_more_btn.gif" width="65" border="0" /></a>
                            </span>
                            <asp:ImageButton ID="institutionClearAll" runat="server" AlternateText="Clear All"
                                OnClick="InstutionListClearAll_ClickHandler" OnClientClick="DeleteList.ClearAll('institution');Event.stop(event)"
                                ImageUrl="/images/clear_all_btn.gif" Width="62" Height="15" />
                        </div>
                    </div>
                </div>
                <div id="nihOnlyBox">
                    <noscript>
                        <p>
                            <b>OR</b></p>
                    </noscript>
                    <div class="locationGroupbox clinicaltrials-filledbox">
                        <input type="radio" id="atNihLocationButtonStatic" name="LocationChooser" value="nih-static"
                            runat="server" />
                        <label for="atNihLocationButtonStatic">
                            <span style="font-weight: bold; color: #000000;">At NIH</span></label><br />
                        <div style="padding: 5px 0 0 40px; line-height: 25px;">
                            <table cellpadding="0" cellspacing="3">
                                <tr>
                                    <td valign="top">
                                        <input id="nihOnly" type="checkbox" value="1" checked="checked" name="nihOnly" runat="server" />
                                    </td>
                                    <td>
                                        <label for="nihOnly">
                                            Only show trials at the NIH Clinical Center<br />
                                            (Bethesda, Md.)</label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <script language="javascript" type="text/javascript">document.observe("dom:loaded", function(){InitializeLocationBox();});</script>

            </td>
            <td valign="top" class="column3 sectionBreak">
                <a href="/clinicaltrials/search-form-help/page2#2" target="new">
                    <img src="/images/ctsearch/gray-question.gif" width="15" height="15" border="0" alt="Help!" /></a>
            </td>
        </tr>
        <tr>
            <td valign="top" class="column1 sectionBreak">
                Trial/Treatment<br />
                Type
            </td>
            <td valign="top" class="column2 sectionBreak">
                Search by trial type, drug, or treatment/intervention
                <div id="showDrugSearchOptionsButton" style="display: none;">
                    <a class="clinicaltrials-expansionLink" href="javascript:showDrugInterventionOptions()">
                        Show Search Options</a></div>
                <input type="hidden" id="treatmentTypeAreaExpanded" runat="server" />
            </td>
            <td valign="top" class="column3 sectionBreak">
                <a href="/clinicaltrials/search-form-help/page2#3" target="new">
                    <img src="/images/ctsearch/gray-question.gif" width="15" height="15" border="0" alt="Help!" /></a>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="sectionBottom">
                <table id="interventionTable" class="subtable" width="100%" cellpadding="0" cellspacing="0"
                    border="0">
                    <tr>
                        <td valign="top" class="column1">
                            <span class="gray-text">Type of Trial</span>
                        </td>
                        <td valign="top" class="column2">
                            <div class="scrollingList" style="width: 380px; height: 170px;">
                                <cancergov:smartcheckboxlist id="trialType" runat="server" repeatcolumns="1" repeatlayout="Flow" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="drugListArea" runat="server">
                        <td valign="top" class="column1">
                            <span class="gray-text">Drug</span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <div id="showDrugListButtonArea" style="display: none; margin: 5px 0px;">
                                <input type="image" id="showDrugListButton" src="/images/choose_from_list_btn.gif"
                                    alt="Choose From List" /></div>
                            <asp:HiddenField runat="server" ID="drugListExpanded" />
                            <div id="drugListSubBox">
                                Find trials that include<br />
                                <asp:RadioButtonList ID="drugListAllOrAny" RepeatDirection="Horizontal" runat="server">
                                    <asp:ListItem Selected="True" Value="any" Text="Any drugs shown" />
                                    <asp:ListItem Selected="False" Value="all" Text="All drugs shown" />
                                </asp:RadioButtonList>
                                <input id="drugid" type="hidden" size="18" name="drugid" runat="server" />
                                <cancergov:deletelist id="drug" deleteiconurl="~/Images/delete_item.gif" height="110"
                                    width="350" runat="server" emptylisttext="Select &quot;Add More&quot; to see drug names.">
											            </cancergov:deletelist>
                                <span id="druglistAddButton" style="display: none;"><a class="black-text" href="javascript:dynPopWindow('/common/popups/CTLSearch/CTLookup.aspx?type=<% =drug.ClientID %>&amp;fld=drug&amp;title=Find+Drug', 'DrugLookup', 'width=681px,menubar=no,location=no,height=580px');">
                                    <img height="15" alt="Add More" src="/images/add_more_btn.gif" width="65" border="0" /></a>
                                </span>
                                <asp:ImageButton ID="druglistClearAll" runat="server" AlternateText="Clear All" OnClick="DrugListClearAll_ClickHandler"
                                    OnClientClick="DeleteList.ClearAll('drug');Event.stop(event)" ImageUrl="/images/clear_all_btn.gif"
                                    Width="62" Height="15" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="interventionListArea" runat="server">
                        <td valign="top" class="column1">
                            <span class="gray-text">Treatment/ Intervention</span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            Examples: chemotherapy, adjuvant therapy, colonoscopy
                            <div id="showInterventionListButtonArea" style="display: none; margin: 5px 0px;">
                                <input type="image" id="showInterventionListButton" src="/images/choose_from_list_btn.gif"
                                    alt="Choose From List" /></div>
                            <asp:HiddenField runat="server" ID="interventionListExpanded" />
                            <div id="interventionListSubBox">
                                <input id="interventionid" type="hidden" size="18" name="interventionid" runat="server" />
                                <cancergov:deletelist id="intervention" deleteiconurl="~/Images/delete_item.gif"
                                    height="110" width="350" runat="server" emptylisttext="Select &quot;Add More&quot; to see treatment/intervention names.">
										                </cancergov:deletelist>
                                <span id="interventionlistAddButton" style="display: none;"><a href="javascript:dynPopWindow('/common/popups/CTLSearch/CTLookup.aspx?type=<% =intervention.ClientID %>&amp;fld=intervention&amp;title=Treatment/Intervention', 'InterventionLookup', 'width=681px,menubar=no,location=no,height=580px');">
                                    <img height="15" alt="Add More" src="/images/add_more_btn.gif" width="65" border="0" /></a>
                                </span>
                                <asp:ImageButton ID="interventionlistClearAll" runat="server" AlternateText="Clear All"
                                    OnClick="InterventionListClearAll_ClickHandler" OnClientClick="DeleteList.ClearAll('intervention');Event.stop(event)"
                                    ImageUrl="/images/clear_all_btn.gif" Width="62" Height="15" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                </table>

                <script language="javascript" type="text/javascript">document.observe("dom:loaded", function(){InitializeDrugInterventionBox();});</script>

            </td>
        </tr>
        <tr>
            <td valign="top" class="column1 sectionBreak sectionBottom">
                Keywords/<br />
                Phrases
            </td>
            <td valign="top" class="column2 sectionBreak sectionBottom">
                <label for="txtKeywords">
                    Search by word or phrase (use quote marks with phrases)</label><br />
                <cancergov:helptextinput id="txtKeywords" helpertext="Examples: PSA, HER-2, \&quot;Paget disease\&quot;"
                    helpertextcolor="#6e6e6e" style="width: 100%; margin: 5px 0px;" size="50" maxlength="100"
                    name="txtKeywords" runat="server" />
            </td>
            <td valign="top" class="column3 sectionBreak sectionBottom">
                <a href="/clinicaltrials/search-form-help/page2#4" target="new">
                    <img src="/images/ctsearch/gray-question.gif" width="15" height="15" border="0" alt="Help!" /></a>
            </td>
        </tr>
        <tr>
            <td valign="top" class="column1 sectionBreak">
                Trial Status/Phase
            </td>
            <td valign="top" class="column2 sectionBreak">
                Search by trial status, phase, or trials added in the last 30 days
                <div id="showTrialStatusSearchOptionsButton" style="display: none;">
                    <a class="clinicaltrials-expansionLink" href="javascript:showTrialStatusSearchOptions()">
                        Show Search Options</a></div>
                <input type="hidden" id="trialStatusExpanded" runat="server" />
            </td>
            <td valign="top" class="column3 sectionBreak">
                <a href="/clinicaltrials/search-form-help/page2#5" target="new">
                    <img src="/images/ctsearch/gray-question.gif" width="15" height="15" border="0" alt="Help!" /></a>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="sectionBottom">
                <table id="trialStatusTable" class="subtable" border="0" cellpadding="0" cellspacing="0"
                    width="100%">
                    <tr>
                        <td valign="top" class="column1">
                            <span class="gray-text">Trial Status</span>
                        </td>
                        <td valign="top" class="column2">
                            <asp:RadioButtonList runat="server" ID="trialStatus" RepeatDirection="Vertical" RepeatLayout="Flow">
                                <asp:ListItem Value="1" Selected="True">Active (currently accepting patients)</asp:ListItem>
                                <asp:ListItem Value="0">Closed (not accepting patients)</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="column1" style="padding-top: 15px;">
                            <span class="gray-text">Trial Phase</span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <cancergov:smartcheckboxlist id="trialPhase" repeatdirection="Vertical" repeatlayout="Table"
                                repeatcolumns="1" runat="server" />
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="column1" style="padding-top: 12px;">
                            <span class="gray-text">New Trials?</span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <asp:CheckBox ID="newOnly" runat="server" Text="Added in last 30 days" />
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                </table>

                <script language="javascript" type="text/javascript">document.observe("dom:loaded", function(){InitializeTrialStatusBox();});</script>

            </td>
        </tr>
        <tr>
            <td valign="top" class="column1 sectionBreak">
                Trial ID/Sponsor
            </td>
            <td valign="top" class="column2 sectionBreak">
                Search by protocol ID, sponsor, investigators, lead organization/cooperative group,
                or special category
                <div id="showTrialSponsorSearchOptionsButton" style="display: none;">
                    <a class="clinicaltrials-expansionLink" href="javascript:showTrialSponsorSearchOptions()">
                        Show Search Options</a></div>
                <input type="hidden" id="trialSponsorExpanded" runat="server" />
            </td>
            <td valign="top" class="column3 sectionBreak">
                <a href="/clinicaltrials/search-form-help/page2#6" target="new">
                    <img src="/images/ctsearch/gray-question.gif" width="15" height="15" border="0" alt="Help!" /></a>
            </td>
        </tr>
        <tr>
            <td colspan="3" class="sectionBottom">
                <table id="trialSponsorTable" class="subtable" border="0" cellpadding="0" cellspacing="0"
                    width="100%">
                    <tr>
                        <td valign="top" class="column1">
                            <label for="protocolID">
                                <span class="gray-text"><b>Protocol ID</b></span></label>
                        </td>
                        <td valign="top" class="column2">
                            Separate multiple IDs with commas or semicolons
                            <asp:TextBox ID="protocolID" Width="100%" MaxLength="50" runat="server"></asp:TextBox>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="column1">
                            <span class="gray-text">Sponsor of Trial</span><br />
                            <a onclick="javascript:dynPopWindow('http://www.nih.gov/icd/', '', 'width=740px,menubar=no,location=no,height=600px,scrollbar=yes'); return(false);"
                                href="http://www.nih.gov/icd/" style="font-weight: normal;">NIH Acronym Lookup</a>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <div class="scrollingList" style="width: 240px; height: 170px;">
                                <cancergov:smartcheckboxlist id="sponsor" runat="server" repeatcolumns="1" repeatlayout="Flow" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trialInvestigatorsRow" runat="server">
                        <td valign="top" class="column1">
                            <span class="gray-text"><b>Trial Investigators</b></span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <div id="showInvestigatorListButtonArea" style="display: none;">
                                <input type="image" id="showInvestigatorListButton" src="/images/choose_from_list_btn.gif"
                                    alt="Choose From List" /></div>
                            <asp:HiddenField runat="server" ID="investigatorListExpanded" />
                            <div id="investigatorListSubBox">
                                Trial Investigators Selected:<br />
                                <input id="investigatorid" type="hidden" size="18" name="investigatorid" runat="server" />
                                <cancergov:deletelist id="investigator" deleteiconurl="~/Images/delete_item.gif"
                                    height="110" width="350" runat="server" emptylisttext="Select &quot;Add More&quot; to see investigator names.">
										            </cancergov:deletelist>
                                <span id="investigatorListAddButton" style="display: none;"><a href="javascript:dynPopWindow('/common/popups/CTLSearch/CTLookup.aspx?type=<% =investigator.ClientID %>&amp;fld=investigator&amp;title=Find+Trial+Investigators', 'InvestigatorLookup', 'width=681px,menubar=no,location=no,height=580px');">
                                    <img height="15" alt="Add More" src="/images/add_more_btn.gif" width="65" border="0" /></a>
                                </span>
                                <asp:ImageButton ID="investigatorListAddButtonClearAll" runat="server" AlternateText="Clear All"
                                    OnClick="InvestigatorListClearAll_ClickHandler" OnClientClick="DeleteList.ClearAll('investigator');Event.stop(event)"
                                    ImageUrl="/images/clear_all_btn.gif" Width="62" Height="15" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trialLeadOrganizationRow" runat="server">
                        <td valign="top" class="column1">
                            <span class="gray-text"><b>Lead&nbsp;Organization/<br />
                                Cooperative Group</b></span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <div id="showLeadOrgListButtonArea" style="display: none; margin: 5px 0px;">
                                <input type="image" id="showLeadOrgListButton" src="/images/choose_from_list_btn.gif"
                                    alt="Choose From List" /></div>
                            <asp:HiddenField runat="server" ID="leadOrgListExpanded" />
                            <div id="leadOrgListSubBox">
                                Lead Organizations or Cooperative Groups Selected:<br />
                                <input id="leadOrgid" type="hidden" size="18" name="leadOrgid" runat="server" />
                                <cancergov:deletelist id="leadOrg" deleteiconurl="~/Images/delete_item.gif" height="110"
                                    width="350" runat="server" emptylisttext="Select &quot;Add More&quot; to see lead organization names.">
										            </cancergov:deletelist>
                                <span id="leadOrgAddButton" style="display: none;"><a class="black-text" href="javascript:dynPopWindow('/common/popups/CTLSearch/CTLookup.aspx?type=<% =leadOrg.ClientID %>&amp;fld=leadOrg&amp;title=Find+Lead+Organizations', 'LeadOrgLookup', 'width=681px,menubar=no,location=no,height=580px');">
                                    <img height="15" alt="Add More" src="/images/add_more_btn.gif" width="65" border="0" /></a>
                                </span>
                                <asp:ImageButton ID="leadOrgClearAll" runat="server" AlternateText="Clear All" OnClick="LeadOrgClearAll_ClickHandler"
                                    OnClientClick="DeleteList.ClearAll('leadOrg');Event.stop(event)" ImageUrl="/images/clear_all_btn.gif"
                                    Width="62" Height="15" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="column1">
                            <span class="gray-text"><b>Special Category</b></span>
                        </td>
                        <td valign="top" class="column2 subSectionBreak">
                            <div class="scrollingList" style="width: 240px; height: 170px;">
                                <cancergov:smartcheckboxlist id="specialCategory" runat="server" repeatcolumns="1"
                                    repeatlayout="Flow" />
                            </div>
                        </td>
                        <td class="column3">
                            &nbsp;
                        </td>
                    </tr>
                </table>

                <script language="javascript" type="text/javascript">document.observe("dom:loaded", function(){InitializeTrialSponsorBox();});</script>

            </td>
        </tr>
        <tr>
            <td valign="top" class="column1 sectionBreak">
                &nbsp;
            </td>
            <td valign="top" class="column2 sectionBreak">
                <asp:ImageButton ID="submit" OnClientClick="doSubmit();" OnClick="SubmitButton_Click"
                    Height="15" Width="48" AlternateText="Search" ToolTip="Search" ImageUrl="/images/ct-search.gif"
                    runat="server" ValidationGroup="v1" />
                &nbsp;&nbsp;&nbsp;&nbsp; <a id="clear" href="#" runat="server">
                    <img height="15" width="70" alt="Start Over" src="/images/CTSearch/grey_start_over_btn.gif"
                        border="0" /></a>
            </td>
            <td valign="top" class="column3 sectionBreak">
                &nbsp;
            </td>
        </tr>
    </table>
    <!--start new form footnote-->
    <p>
    </p>
    <asp:ValidationSummary ID="ValidationSummary2" runat="server" DisplayMode="List"
        CssClass="clinicaltrials-errorMessage" ValidationGroup="v1" />
    <p>
        <a onclick="dynPopWindow('https://cissecure.nci.nih.gov/livehelp/welcome.asp', '', 'width=620px,menubar=no,location=no,height=465px,scrollbar=yes'); return false;"
            href="https://cissecure.nci.nih.gov/livehelp/welcome.asp">
            <img src="/images/ctsearch/livehelp.gif" alt="Having trouble with this form? Check the help page or contact an NCI information specialist through LiveHelp online text chat or by calling 1-800-4-CANCER." /></a></p>
    <!--end new form footnote-->
    </form>
</td>
