<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClinicalTrialsSearchTemplate.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.ClinicalTrialsSearchTemplate" %>
<%@ Register assembly="NCILibrary.Web.UI.WebControls" namespace="NCI.Web.UI.WebControls.FormControls" tagprefix="CancerGov" %>

<style>    
    .cts-location 
    {
        padding: 0;
    }
    
    .cts-location>div 
    {
        border-top: 3px solid #00B5BC;
        padding: 12px;
    }   

    fieldset fieldset 
    {
        background: #E2F5F9;                
        margin: 0;
        padding: 18px;
    }
    
    fieldset fieldset .legend 
    {   
        font-size: 1.2em;
        color: #606060;
        background: transparent;
    }
        
</style>

<%-- Grouped Check Box List --%>
<script type="text/javascript">
/*
 This jQuery plugin is for making a checkbox list that can be a single input (i.e. name attr is the same) but
 the checkboxes can be mutually exclusive of each other.
 */
(function ( $ ) {

    if ($.fn.groupedCheckBoxList === undefined) {

        $.groupedCheckBoxList = {
            "default": { }
        };

        $.fn.groupedCheckBoxList = function () {
            //Loop through each element we are trying to make a checkbox list.  This element should contain checkboxes.
            return this.each(function() {
                var cbl = $(this);
                var checkboxes = cbl.find('input[type=checkbox]');
                //Find all checkboxes within the current checkbox list
                checkboxes.each(function() {
                    //Initial setup function
                    var el = $(this); //This is the checkbox

                    el.click(function () {
                        console.log("Clicked: " + this.id);
                        if (this.checked == true) {
                            //Only do something if this item becomes checked.
                            var target = $(this);
                            var target_group = target.data("cbgroup");

                            //Loop through checkboxes to uncheck if needed
                            checkboxes.each(function () {
                                var curr_cb = $(this);
                                var curr_group = curr_cb.data("cbgroup");

                                //Only uncheck if not in current group and it is checked
                                if ((target_group != curr_group) && curr_cb.prop("checked")) {
                                    console.log("Unchecking " + curr_cb.attr("id"));
                                    curr_cb.prop("checked", false);
                                }
                            });
                        }
                    });
                });
            });
        }

    }


    return $.fn.groupedCheckBoxList;
})(jQuery);
</script>

<%-- Radio Toggle Blocks --%>
<script type="text/javascript">
(function ( $ ) {

    if ($.fn.radioToggleBlocks === undefined) {

        $.radioToggleBlocks = {
            "default": { }
        };

        $.fn.radioToggleBlocks = function () {
            //Loop through each element we are trying to make a radioToggleBlock.  Each element should contain controllers.
            return this.each(function() {
                var cbl = $(this);
                var radios = cbl.find('input[type=radio]');

                var regions = [];

                //Find all checkboxes within the current checkbox list
                radios.each(function() {
                    //Initial setup function
                    var el = $(this); //This is the radio


                    var ctrl_tocontrol = el.attr("aria-controls");
                    var region = $("#" + ctrl_tocontrol);
                    regions.push(region); //Add region to our list of regions

                    region.prop("tabindex", "-1");

                    if (this.checked !== true) {
                        region.hide().attr('aria-expanded', 'false');
                    } else {
                        region.attr('aria-expanded', 'true');
                    }


                    el.on('click', regions , function () {

                        //Only do this if we are selecting the radio
                        if (this.checked === true) {

                            var curr_el = $(this);

                            var selected_region = false;
                            for(var i=0; i< regions.length; i++) {
                                if (regions[i].attr('id') === curr_el.attr('aria-controls')) {
                                    regions[i].show().attr("aria-expanded", "true");
                                    selected_region = regions[i];
                                } else {
                                    regions[i].hide().attr("aria-expanded", "false");
                                }

                            }

                            if (selected_region) {
                                selected_region.focus();
                            }
                        }
                    });

                });
            });
        }

    }


    return $.fn.radioToggleBlocks;
})(jQuery);
</script>

    <script type="text/javascript">
        var ids = {
              intervention:"<%=intervention.ClientID%>"
            , investigator: "<%=investigator.ClientID%>"
            , leadOrg: "<%=leadOrg.ClientID%>"
            , drug: "<%=drug.ClientID%>"
            , institution: "<%=institution.ClientID%>"
            
            , hospitalLocationButton: "<%=hospitalLocationButton.ClientID %>"
            , zipCodeLocationButton: "<%=zipCodeLocationButton.ClientID %>"
            , cityStateLocationButton:"<%=cityStateLocationButton.ClientID %>"
            , atNihLocationButton:"<%=atNihLocationButton.ClientID %>"
            , country:"<%=country.ClientID %>"
            , city:"<%=city.ClientID %>"
            , state:"<%=state.ClientID %>"
            , investigatorid:"<%=investigatorid.ClientID %>"
            , institutionListSubBox: "<%=institutionListSubBox.ClientID %>"
            , cancerType: "<%=ddlCancerType.ClientID%>"
            , newOnly: "<%=newOnly.ClientID%>"
            , txtKeywords: "<%=txtKeywords.ClientID%>"
            , txtKeywords_state: "<%=txtKeywords.ClientID%>_state"
            , trialStatus: "<%=trialStatus.ClientID%>"
            , trialStatus_0: "<%=trialStatus.ClientID%>_0"
            , trialStatus_1: "<%=trialStatus.ClientID%>_1"
            , trialPhase: "<%=trialPhase.ClientID%>"
            , trialPhase_0: "<%=trialPhase.ClientID%>_0"
            , trialPhase_1: "<%=trialPhase.ClientID%>_1"
            , trialPhase_2: "<%=trialPhase.ClientID%>_2"
            , trialPhase_3: "<%=trialPhase.ClientID%>_3"
            , trialPhase_4: "<%=trialPhase.ClientID%>_4"
            , trialPhase_5: "<%=trialPhase.ClientID%>_5"
            , protocolID: "<%=protocolID.ClientID%>"
            , nihOnly: "<%=nihOnly.ClientID%>"
        }
    
        $(document).ready(function() {
            $("select").selectmenu({
                change: function(event, ui) {
                    //This calls the parent change event so that .NET dropdowns can
                    //autopostback.
                    ui.item.element.parent().change();
                }
            });

            $(".groupedCheckBoxList").groupedCheckBoxList();
            $("#locationFieldset").radioToggleBlocks();
        });
    </script>


<form name="advSearchForm" id="advSearchForm" method="post" runat="server">
    
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
        CssClass="clinicaltrials-errorMessage" ValidationGroup="v1" />
        
    <%-- --------------------- Begin Cancer Types ------------------------ --%>
    <fieldset aria-labelledby="legend-condition">
        <div class="row">
            <div class="medium-4 columns">
                <asp:Label CssClass="right" ID="lblCancerType" AssociatedControlID="ddlCancerType" runat="server">Cancer Type/Condition</asp:Label>
            </div>
            <div class="medium-7 left columns">
                 <asp:DropDownList 
                    ID="ddlCancerType" 
                    runat="server" 
                    CssClass="fullwidth" 
                    AutoPostBack="true"
                    OnSelectedIndexChanged="cancerType_SelectedIndexChanged" 
                    CausesValidation="false"
                    ValidationGroup="v1">
                </asp:DropDownList>
            </div>
            <div class="medium-1 columns"><a href="" class="text-icon-help" target="_blank" aria-label="Help">?</a></div>
        </div>
                
        <div class="row">
            <div class="medium-4 columns">
                <label class="right">Stage/Subtype</label>
            </div>
            <div class="medium-7 left columns">
                <CancerGov:AccessibleCheckBoxList 
                    ID="cancerStage"
                    runat="server"
                    EmptyText="Select cancer type/condition first."
                    CssClass="scrolling-list roundy-box groupedCheckBoxList"
                    />            
            </div>            
        </div>
    </fieldset>
    <%-- ------------------------ END Cancer Types -------------------- --%>

    <%-- ************************ Location **************************** --%>
    <fieldset id="locationFieldset" aria-labelledby="legend-location">
        <div class="row">
            <div id="legend-location" class="medium-4 columns legend">Location</div>
            <div class="medium-7 columns cts-location roundy-box">
                <div class="row">
                    <div class="column">
                        <div class="row">
                            <div class="large-6 columns">
                                <div class="radio"><asp:RadioButton ID="zipCodeLocationButton" value="zip" GroupName="LocationChooser" runat="server" Text="Near ZIP Code" /></div>
                                <div class="radio"><asp:RadioButton ID="cityStateLocationButton" value="city" GroupName="LocationChooser" runat="server" Text="In City/State/Country" /></div>
                            </div>                       
                            <div class="large-6 columns">         
                                <div class="radio"><asp:RadioButton ID="hospitalLocationButton" value="hospital" GroupName="LocationChooser" runat="server" Text="At Hospital/Institution" /></div>
                                <div class="radio"><asp:RadioButton ID="atNihLocationButton" value="nih" GroupName="LocationChooser" runat="server" Text="At NIH" /></div>
                            </div>                                
                        </div>
                        <fieldset ID="zipCodeLocationFieldset" runat="server" class="roundy-box row" role="region">
                            <div class="column">
                                <div class="legend" id="legend-location-zip">Near ZIP Code</div>
                                <div>
                                    <asp:Label ID="lblzipCodeProximity" AssociatedControlID="zipCodeProximity" runat="server">Show trials located within:</asp:Label>
                                    <div class="row">
                                        <asp:DropDownList ID="zipCodeProximity" runat="server">
                                            <asp:ListItem Value="20">20 miles</asp:ListItem>
                                            <asp:ListItem Value="50">50 miles</asp:ListItem>
                                            <asp:ListItem Value="100" Selected="True">100 miles</asp:ListItem>
                                            <asp:ListItem Value="200">200 miles</asp:ListItem>
                                            <asp:ListItem Value="500">500 miles</asp:ListItem>
                                        </asp:DropDownList>
                                        of
                                        <asp:Label ID="lblzipCode" AssociatedControlID="zipCode" runat="server">ZIP Code</asp:Label>
                                        <asp:TextBox ID="zipCode" MaxLength="5" Columns="8" runat="server" ValidationGroup="v1"></asp:TextBox>
                                    </div>
                                    <!-- Add validator -->
                                </div>
                            </div>
                        </fieldset>
                        <fieldset ID="hospitalLocationFieldset" runat="server" class="roundy-box row" role="region">
                            <div class="column">
                                <div class="legend" id="legend-location-hospital">At Hospital/Institution</div>
                                <div>
                                    <input id="institutionid" type="hidden" size="18" name="institutionid" runat="server" />
                                    <div id="institutionListSubBox" runat="server">
                                        <cancergov:deletelist id="institution" runat="server" 
                                            emptylisttext="Select &quot;Add More&quot; to see hospital names." />
                                        <span id="institutionAddButton">
                                            <button class="button action" type="button"
                                                onclick="dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=<% =institution.ClientID %>&amp;fld=institution&amp;title=Find+Hospitals/Institutions', 'InstitutionLookup', 'width=725px,menubar=no,location=no,height=675px');">
                                                Add More
                                            </button>
                                        </span>
                                        <asp:Button ID="institutionClearAll" runat="server" Text="Clear All"
                                            OnClick="InstutionListClearAll_ClickHandler" 
                                            OnClientClick="$('#' + ids.institution)deletelist('clearAll');return false;"
                                            CssClass="button reset" />
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                        <fieldset  ID="cityStateLocationFieldset" runat="server" class="roundy-box row" role="region">
                            <div class="column">
                                <div class="legend" id="legend-location-citystate">In City/State/Country</div>
                                <div>                                    
                                    <label for="<%=country.ClientID%>">Country:</label>
                                    <select id="country" onchange="country_onChange(this);" name="country" runat="server" />                                
                                    <label for="<%=city.ClientID%>">City:</label>
                                    <input id="city" type="text" size="14" name="city" style="width: 100%;" runat="server" />
                        
                                    <label>State</label>
                                    <CancerGov:AccessibleCheckBoxList
                                        id="state"
                                        runat="server"
                                        CssClass="scrolling-list roundy-box groupedCheckBoxList" />                        
                                </div>
                            </div>
                        </fieldset>
                        <fieldset ID="atNihLocationFieldset" runat="server" class="roundy-box row" role="region">
                            <div class="column">
                                <div class="legend" id="legend-location-NIH">At NIH</div>
                                <div><asp:CheckBox ID="nihOnly" runat="server" value="1" Text="Only show trials at the NIH Clinical Center (Bethesda, Md.)" Checked="true"></asp:CheckBox></div>
                            </div>
                        </fieldset>
                    </div>                       
                </div>
            </div>
            <div class="medium-1 columns"><a href="" class="text-icon-help" target="_blank" aria-label="Help">?</a></div>
        </div>
    </fieldset>
    <%-- ************************* End Location ******************************** --%>



    <%-- ------------------------- Trial/Treatment Type ------------------------ --%>
    <fieldset aria-labelledby="legend-trialtreatment">
        <div class="row">
            <div id="legend-trialtreatment" class="medium-4 columns legend">Trial/Treatment Type</div>
            <div class="medium-7 columns">
                Search by trial type, drug, or treatment/intervention
            </div>
            <div class="medium-1 columns"><a href="" class="text-icon-help" target="_blank" aria-label="Help">?</a></div>
        </div>        
        <div class="collapsible">
            <div class="row">
                <div class="medium-4 columns">
                    <label class="right">Type of Trial</label>
                </div>
                <div class="medium-7 left columns">
                    <CancerGov:AccessibleCheckBoxList 
                        ID="trialType" 
                        runat="server" 
                        CssClass="scrolling-list roundy-box groupedCheckBoxList"
                     />
                </div>
            </div>

            <div class="row">
                <div class="medium-4 columns">
                    <label class="right">Drug</label>
                </div>
                <div class="medium-7 left columns">
                    <div id="drugListSubBox">
                        Find trials that include<br />
                        <asp:RadioButtonList CssClass="radio" ID="drugListAllOrAny" RepeatDirection="Vertical" 
                            RepeatLayout="Flow" runat="server">
                            <asp:ListItem Selected="True" Value="any" Text="Any drugs shown" />
                            <asp:ListItem Selected="False" Value="all" Text="All drugs shown" />
                        </asp:RadioButtonList>
                        <input id="drugid" type="hidden" size="18" name="drugid" runat="server" />
                        <cancergov:deletelist id="drug" runat="server" 
                            emptylisttext="Select &quot;Add More&quot; to see drug names." />
                        <span id="druglistAddButton" >
                            <button class="button action" type="button"
                                onclick="dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=<% =drug.ClientID %>&amp;fld=drug&amp;title=Find+Drug', 'DrugLookup', 'width=725px,menubar=no,location=no,height=675px');">
                                Add More
                            </button>
                        </span>
                        <asp:Button ID="druglistClearAll" runat="server" Text="Clear All" 
                            OnClick="DrugListClearAll_ClickHandler"
                            OnClientClick="$('#' + ids.drug).deletelist('clearAll');return false;"
                            CssClass="button reset" />
                    </div>
                </div>
            </div>
            
            <div class="row">
                <div class="medium-4 columns">
                    <label class="right">Treatment/Intervention</label>
                </div>
                <div class="medium-7 left columns">
                    <em>Examples: chemotherapy, adjuvant therapy, colonoscopy</em><br />
                    <div id="interventionListSubBox">
                        <input id="interventionid" type="hidden" size="18" name="interventionid" runat="server" />
                        <cancergov:deletelist id="intervention" 
                            runat="server" 
                            emptylisttext="Select &quot;Add More&quot; to see treatment/intervention names." />
                        <span id="interventionlistAddButton">
                        <button class="button action" type="button"
                            onclick="dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=<% =intervention.ClientID %>&amp;fld=intervention&amp;title=Treatment/Intervention', 'InterventionLookup', 'width=725px,menubar=no,location=no,height=675px');">
                            Add More
                        </button>
                        </span>
                        <asp:Button ID="interventionlistClearAll" runat="server" Text="Clear All"
                            OnClick="InterventionListClearAll_ClickHandler" 
                            OnClientClick="$('#' + ids.intervention).deletelist('clearAll');return false;"
                            CssClass="button reset" />
                    </div>
                </div>
            </div>
        </div>    
    </fieldset>
    <%-- ------------------- End Trial/Treatment Type ----------------- --%>


    <%-- *******************  Keywords/Phrase ****************** --%>
    <fieldset aria-labelledby="legend-keyword">
        <div class="row">
            <div id="legend-keyword" class="medium-4 columns legend">Keywords/Phrases</div>
            <div class="medium-7 columns">
                <div class="row">Search by word or phrase (use quotation marks with phrases)</div>
                <div class="row">
                <asp:TextBox id="txtKeywords" size="50" maxlength="100"
                    name="txtKeywords" runat="server" />
                </div>
            </div>
            <div class="medium-1 columns"><a href="" class="text-icon-help" target="_blank" aria-label="Help">?</a></div>
        </div>
    </fieldset>
    <%-- *******************  End Keywords/Phrase ****************** --%>


    <%-- =================== Trial Status/Phase ==================== --%>
    <fieldset aria-labelledby="legend-trialstatus">
        <div class="row">
            <div id="legend-trialstatus" class="medium-4 columns legend">Trial Status/Phase</div>
            <div class="medium-7 columns">Search by trial status, phase, or trials added in the last 30 days</div>
            <div class="medium-1 columns"><a href="" class="text-icon-help" target="_blank" aria-label="Help">?</a></div>
        </div>
        <div class="collapsible">
            <div class="row">
                <div class="medium-4 columns"><label class="right">Trial Status</label></div>
                <div class="medium-7 left columns">
                    <asp:RadioButtonList CssClass="radio" runat="server" ID="trialStatus" RepeatDirection="Vertical" RepeatLayout="Flow">
                        <asp:ListItem Value="1" Selected="True">Active (currently accepting patients)</asp:ListItem>
                        <asp:ListItem Value="0">Closed (not accepting patients)</asp:ListItem>
                    </asp:RadioButtonList>
                </div>                
            </div>
            <div class="row">
                <div class="medium-4 columns"><label class="right">Trial Phase</label></div>
                <div class="medium-7 left columns">
                    <CancerGov:AccessibleCheckBoxList
                        ID="trialPhase"
                        runat="server" 
                        CssClass="scrolling-list roundy-box groupedCheckBoxList"
                        />
                </div>                
            </div>
            <div class="row">
                <div class="medium-4 columns"><label class="right">New Trials?</label></div>
                <div class="medium-7 left columns"><div class="checkbox"><asp:CheckBox ID="newOnly" runat="server" Text="Added in last 30 days" /></div></div>
            </div>        
        </div>
    </fieldset>
    <%-- =================== End Trial Status/Phase ================= --%>

    <%-- ................... Trial ID/Sponsor ....................... --%>
    <fieldset aria-labelledby="legend-trialsponsor">
        <div class="row">
            <div id="legend-trialsponsor" class="medium-4 columns legend">Trial ID/Sponsor</div>
            <div class="medium-7 columns">Search by protocol ID, sponsor, investigators, lead organization/cooperative group, or special category</div>
            <div class="medium-1 columns"><a href="" class="text-icon-help" target="_blank" aria-label="Help">?</a></div>
        </div>        
        <div class="collapsible">
            <div>
                <div class="row">
                    <div class="medium-4 columns"><asp:Label ID="lblProtocolID" runat="server" AssociatedControlID="protocolID" CssClass="right" >Protocol ID</asp:Label></div>
                    <div class="medium-7 left columns">
                        <div class="row">Separate multiple IDs with commas or semicolon</div>
                        <div class="row"><asp:TextBox ID="protocolID" Width="100%" MaxLength="50" runat="server" /></div>
                    </div>
                </div>                       
            </div>
            <div class="row">
                <div class="medium-4 columns"><label class="right">Sponsor of Trial</label></div>
                <div class="medium-7 left columns"><CancerGov:AccessibleCheckBoxList 
                        ID="sponsor" 
                        runat="server" 
                        CssClass="scrolling-list roundy-box groupedCheckBoxList"
                     /></div>
            </div>
            <div class="row">
                <div class="medium-4 columns"><label class="right">Trial Investigators</label></div>
                <div class="medium-7 left columns">
                    <div id="investigatorListSubBox">
                        Trial Investigators Selected:<br />
                        <input id="investigatorid" type="hidden" size="18" name="investigatorid" runat="server" />
                        <cancergov:deletelist id="investigator"
                            runat="server" emptylisttext="Select &quot;Add More&quot; to see investigator names." />
                        <span id="investigatorListAddButton">
                            <button class="button action" type="button"
                                onclick="dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=<% =investigator.ClientID %>&amp;fld=investigator&amp;title=Find+Trial+Investigators', 'InvestigatorLookup', 'width=725px,menubar=no,location=no,height=675px');">
                                Add More
                            </button>
                        </span>
                        <asp:Button ID="investigatorListAddButtonClearAll" runat="server" Text="Clear All"
                            OnClick="InvestigatorListClearAll_ClickHandler" 
                            OnClientClick="$('#' + ids.investigator).deletelist('clearAll);return false;"
                            CssClass="button reset" />
                    </div>
                </div>
            </div>        
            <div class="row">
                <div class="medium-4 columns"><label class="right">Lead Organization/ Cooperative Group</label></div>
                <div class="medium-7 left columns">
                    <div id="leadOrgListSubBox">
                        Lead Organizations or Cooperative Groups Selected:<br />
                        <input id="leadOrgid" type="hidden" size="18" name="leadOrgid" runat="server" />
                        <cancergov:deletelist id="leadOrg" 
                            runat="server" emptylisttext="Select &quot;Add More&quot; to see lead organization names." />
                        <span id="leadOrgAddButton">
                            <button class="button action" type="button"
                                onclick="dynPopWindow('/Common/PopUps/CTLSearch/CTLookup.aspx?type=<% =leadOrg.ClientID %>&amp;fld=leadOrg&amp;title=Find+Lead+Organizations', 'LeadOrgLookup', 'width=725px,menubar=no,location=no,height=675px');">
                                Add More
                            </button>
                        </span>
                        <asp:Button ID="leadOrgClearAll" runat="server" Text="Clear All" OnClick="LeadOrgClearAll_ClickHandler"
                            OnClientClick="$('#' + ids.leadOrg).deletelist('clearAll');return false;"
                            CssClass="button reset" />
                    </div>
                </div>
            </div>        
            <div class="row">
                <div class="medium-4 columns"><label class="right">Special Category</label></div>
                <div class="medium-7 left columns"><CancerGov:AccessibleCheckBoxList 
                        ID="specialCategory" 
                        runat="server" 
                        CssClass="scrolling-list roundy-box groupedCheckBoxList"
                     /></div>
            </div>        
        </div>
    </fieldset>
    <%-- ................... END Trial ID/Sponsor ................... --%>

    <div class="row">
        <div class="medium-8 columns right">
            <asp:button id="submit" CssClass="submit button" Text="Search" runat="server"
                OnClick="SubmitButton_Click" />
            <a id="clear" class="reset startover button" href="#" runat="server">Start Over</a>
        </div>
    </div>

</form>


