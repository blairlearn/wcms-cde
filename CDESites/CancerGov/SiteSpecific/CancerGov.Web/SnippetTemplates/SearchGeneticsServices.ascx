<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchGeneticsServices.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.SearchGeneticsServices" %>

<script type="text/javascript"> 
    var ids = {
    txtCity: "txtCity"
    ,txtLastName: "txtLastName"
    ,selCancerType: "<%=selCancerType.ClientID%>"
    ,selCancerFamily: "<%=selCancerFamily.ClientID%>" 
    ,selState: "<%=selState.ClientID%>"
    , selCountry: "<%=selCountry.ClientID%>"
}

function doWebAnalyticsStuff() {
    NCIAnalytics.GeneticServicesDirectorySearch(null);
    return true;
}

</script>   

<!-- Main Content Area -->
<div class="directory-search">
    <form id="searchForm" name="searchForm" action="<%=SearchPageInfo.SearchResultsPrettyUrl%>"  
        method="post" onsubmit="return doWebAnalyticsStuff();" 
        aria-label="Search the Cancer Genetics Services Directory">
        <fieldset>
            <legend>Specialty</legend>
            <div class="row">
                <div class="medium-4 columns"> 
                    <label for="selCancerType" class="right inline">Type of Cancer:<br />(choose 1 or more)
                    </label>
                </div>
                <div class="medium-8 columns">
                    <select id="selCancerType" multiple name="selCancerType" runat="server" size="3">
                    </select>
                 </div>
            </div>
            <div class="row"><div class="medium-8 columns right">OR</div></div>
            <div class="row">
                <div class="medium-4 columns">
                    <label for="selCancerFamily" class="right inline">Family Cancer Syndrome:<br />(choose 1 or more)
                    </label>
                </div>
                <div class="medium-8 columns">
                    <select id="selCancerFamily" multiple name="selCancerFamily" runat="server" size="3">
                    </select>
                </div>
            </div>
        </fieldset>
            
        <fieldset>
            <legend>Location</legend>
            <div class="row">
                <div class="medium-4 columns">
                    <label for="txtCity" class="right inline">City:</label>
                </div>
                <div class="medium-8 columns">                                      
                    <input type="text" name="txtCity" id="txtCity" maxlength="40" size="30">
                </div>
            </div>
            <div class="row">
                <div class="medium-4 columns">
                    <label for="selState" class="right inline">State:<br />(choose one or more)
                    </label>
                </div>
                <div class="medium-8 columns">
                    <select id="selState" multiple name="selState" runat="server" size="3">
                    </select>
                </div>
            </div>
            <div class="row">
                <div class="medium-4 columns">
                    <label for="selCountry" class="right inline">Country:<br />(choose one or more)
                    </label>
                </div>
                <div class="medium-8 columns">
                    <select id="selCountry" multiple name="selCountry" runat="server" size="3">
                    </select>
                </div>
            </div>
        </fieldset>
                                  
        <fieldset>
            <legend>Name of genetics professional, if known</legend>
            <div class="row">
                <div class="medium-4 columns">
                    <label for="txtLastName" class="right inline">Last name:</label>
                </div>
                <div class="medium-8 columns">
                    <input type="text" name="txtLastName" id="txtLastName" maxlength="40" 
                        size="30">
                </div>
            </div>
        </fieldset>
        
        <button type="submit" name="searchBtn" class="button">Search</button>
        <button type="reset" name="clearBtn" class="button">Clear</button>
    </form>
</div>