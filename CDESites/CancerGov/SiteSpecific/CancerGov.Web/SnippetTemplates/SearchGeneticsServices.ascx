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
    <form class="genetics-directory-form" id="searchForm" name="searchForm" action="<%=SearchPageInfo.SearchResultsPrettyUrl%>"  
        method="post" onsubmit="return doWebAnalyticsStuff();" 
        aria-label="Search the Cancer Genetics Services Directory">
        <fieldset aria-labelledby="legend-specialty">
            <div class="row">
                <div id="legend-specialty" class="large-4 columns legend">Specialty</div>
            </div>
            <div class="row">
                <div class="large-4 columns"> 
                    <label for="selCancerType" class="field inline">Type of Cancer:<br />(choose 1 or more)
                    </label>
                </div>
                <div class="large-8 columns">
                    <select id="selCancerType" class="full-width" multiple name="selCancerType" runat="server" size="3">
                    </select>
                 </div>
            </div>
            <div class="row"><div class="large-8 columns right">OR</div></div>
            <div class="row">
                <div class="large-4 columns">
                    <label for="selCancerFamily" class="field inline">Family Cancer Syndrome:<br />(choose 1 or more)
                    </label>
                </div>
                <div class="large-8 columns">
                    <select id="selCancerFamily" class="full-width" multiple name="selCancerFamily" runat="server" size="3">
                    </select>
                </div>
            </div>
        </fieldset>
        <fieldset aria-labelledby="legend-location">
            <div class="row">
                <div id="legend-location" class="large-4 columns legend">Location</div>
            </div>
            <div class="row">
                <div class="large-4 columns">
                    <label for="txtCity" class="field inline">City:</label>
                </div>
                <div class="large-8 columns">                                      
                    <input type="text" name="txtCity" id="txtCity" maxlength="40">
                </div>
            </div>
            <div class="row">
                <div class="large-4 columns">
                    <label for="selState" class="field inline">State:<br />(choose one or more)
                    </label>
                </div>
                <div class="large-8 columns">
                    <select id="selState" class="full-width" multiple name="selState" runat="server" size="3">
                    </select>
                </div>
            </div>
            <div class="row">
                <div class="large-4 columns">
                    <label for="selCountry" class="field inline">Country:<br />(choose one or more)
                    </label>
                </div>
                <div class="large-8 columns">
                    <select id="selCountry" class="full-width" multiple name="selCountry" runat="server" size="3">
                    </select>
                </div>
            </div>
        </fieldset>
        <fieldset aria-labelledby="legend-prof-name">
            <div class="row">
                <div id="legend-prof-name" class="large-4 columns legend">Name of genetics professional, if known</div>
            </div>
            <div class="row">
                <div class="large-4 columns">
                    <label for="txtLastName" class="field inline">Last name:</label>
                </div>
                <div class="large-8 columns">
                    <input type="text" name="txtLastName" id="txtLastName" maxlength="40">
                </div>
            </div>
        </fieldset>
        <div class="row">
            <div class="large-8 columns large-offset-4">
                <button type="submit" name="searchBtn" class="submit button">Search</button>
                <button type="reset" name="clearBtn" class="reset button">Clear</button>
            </div>
        </div>
    </form>
</div>