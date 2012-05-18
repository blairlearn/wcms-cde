<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryHome" %>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="/Common/UserControls/AlphaListBox.ascx" %>
<%@ Register TagPrefix="CancerGovWww" TagName="LangSwitch" Src="/Common/UserControls/LangSwitch.ascx" %>
<%@ Register TagPrefix="CGov" Assembly="NCILibrary.Web.UI.WebControls" Namespace="NCI.Web.UI.WebControls.FormControls" %>



<script type="text/javascript">
    // function used by AutoComplete to submit to server when user
    // selects an item
    function ACOnSubmit() {
        document.getElementById('<%=btnGo.ClientID%>').click();
    }

    //Hookup JPlayer for Audio
    if (jQuery.jPlayer) {
        jQuery(document).ready(function($) {
            var my_jPlayer = $("#dictionary_jPlayer");

            my_jPlayer.jPlayer({
                swfPath: "/PublishedContent/files/global/flash/", //Path to SWF File Used by jPlayer
                //errorAlerts: true,
                supplied: "mp3" //The types of files which will be used.
            });

            //Attach a click event to the audio link
            $(".CDR_audiofile").click(function() {
                my_jPlayer.jPlayer("setMedia", {
                    mp3: $(this).attr("href") // Defines the m4v url
                }).jPlayer("play");

                return false;
            });
        });
    }
       
</script>

<script type="text/javascript">
    var ids = {
        radioStarts: "<%=radioStarts.ClientID %>"
    , AutoComplete1: "<%=AutoComplete1.ClientID %>"
    }
</script>

<div class="hidden">
    The search textbox has an autosuggest feature. When you enter three or more characters,
    a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
    to move through the suggestions. To select a suggestion, hit the enter key. Using
    the escape key closes the listbox and puts you back at the textbox. The radio buttons
    allow you to toggle between having all search items start with or contain the text
    you entered in the search box.
</div>

<asp:Panel ID="pnlTermSearch" runat="server">
<div class="dictionary-box">
	<div class="row1">
        <!-- Table needed for proper functing of asp:AutoComplete control -->
        <table width="100%">
        <tr>
        <td>
            <form name="aspnetForm" method="post" action="/dictionary/" id="aspnetForm" onsubmit="NCIAnalytics.TermsDictionarySearch(this,false);"
                runat="server">
                <div id="dictionary_jPlayer"></div>


                 <CGov:AutoComplete CssClass="dictionary" Name="AutoComplete1" ID="AutoComplete1"
                    runat="server" CallbackFunc="ACOnSubmit" autocomplete="off" MinWidth="333" />

                <asp:ImageButton class="go-button" Name="btnGo" ID="btnGo" runat="server" ImageUrl="/PublishedContent/Images/Images/red_search_button.gif"
                    AlternateText="Search" ToolTip="Search" />

                    
                <asp:RadioButton ID="radioStarts" Name="radioStarts" CssClass="starts-with-radio" runat="server" Checked="True" GroupName="sgroup" />
                <asp:Label ID="lblStartsWith" CssClass="starts-with-label" runat="server" Text="Starts with"
                    AssociatedControlID="radioStarts"></asp:Label>
               
                <asp:RadioButton  ID="radioContains" Name="radioContains" CssClass="contains-radio" runat="server" GroupName="sgroup" />
                <asp:Label ID="lblContains" CssClass="contains-label" runat="server" Text="Contains" 
                    AssociatedControlID="radioContains"></asp:Label>  

            </form>
        </td>
        </tr>
        </table>
	</div>
	<div class="row2">
	    <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/drugdictionary.aspx"
            NumericItems="true" ShowAll="true" />
	</div>
</div>
</asp:Panel>

