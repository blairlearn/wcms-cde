<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DictionarySearchBlock.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.DictionarySearchBlock" %>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="~/Common/UserControls/AlphaListBox.ascx" %>

<script type="text/javascript">
    // function used by AutoComplete to submit to server when user
    // selects an item
    function ACOnSubmit() {
        document.getElementById('<%=btnSearch.ClientID%>').click();
    }

    //Hookup JPlayer for Audio
    if (jQuery.jPlayer && !Modernizr.touch) {
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

    // Autocomplete functionality
    var ids = {
        radioStarts: "<%=radioStarts.ClientID %>",
        radioContains: "<%=radioContains.ClientID %>",
        AutoComplete1: "<%=AutoComplete1.ClientID %>"
    }

    $(document).ready(function() {
        autoFunc();
    });

    function autoFunc() {

        var dictionary = "<%=Dictionary.ToString() %>";



        var language = 'English';

        if ($('html').attr('lang') === 'es') {

            language = 'Spanish';

        }



        var isContains = IsContains();



        NCI.doAutocomplete('#' + ids.AutoComplete1, function(term) { return NCI.dictionary.searchSuggest(dictionary, term, language, isContains ? 'contains' : 'begins'); }, isContains);

    }

 
    function IsContains() {
        var ret = false;

        if ($("#" + ids.radioContains).prop("checked"))
            ret = true;

        return ret;
    }
</script>

<asp:PlaceHolder ID="pnlTermSearch" runat="server">
    <form id="aspnetForm" aria-label="Search the Dictionary of Cancer Terms" runat="server">
    <div class="dictionary-search">
        <div class="hidden">
            The search textbox has an autosuggest feature. When you enter three or more characters,
            a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
            to move through the suggestions. To select a suggestion, hit the enter key. Using
            the escape key closes the listbox and puts you back at the textbox. The radio buttons
            allow you to toggle between having all search items start with or contain the text
            you entered in the search box.
        </div>
        <div id="dictionary_jPlayer">
        </div>
        <div class="row">
            <div class="small-12 columns">
                <span class="radio">
                    <asp:RadioButton ID="radioStarts" runat="server" GroupName="sgroup" Checked="true" />
                    <asp:Label ID="lblStartsWith" class="inline" runat="server" Text="Starts with" AssociatedControlID="radioStarts"></asp:Label>
                </span><span class="radio">
                    <asp:RadioButton ID="radioContains" runat="server" GroupName="sgroup" />
                    <asp:Label ID="lblContains" runat="server" Text="Contains" class="inline" AssociatedControlID="radioContains"></asp:Label>
                </span>
            </div>
        </div>
        <div class="row">
            <div class="large-6 columns">
                <asp:TextBox CssClass="dictionary-search-input" ID="AutoComplete1" inputmode="latin"
                    aria-autocomplete="list" runat="server" CallbackFunc="ACOnSubmit" autocomplete="off" />
            </div>
            <div class="large-2 columns left">
                <asp:Button class="submit button postfix" ID="btnSearch" runat="server" OnClick="btnSearch_OnClick"
                    ToolTip="Search" />
            </div>

            <div class="medium-1 columns left">
                <a  class="text-icon-help" aria-label="Help"  href="javascript:dynPopWindow('/Common/PopUps/popHelp.aspx','popup','width=500,height=700,scrollbars=1,resizable=1,menubar=0,location=0,status=0,toolbar=0')">
                    ?
                </a> 
            </div>
        </div>
    </div>
    <div class="az-list">
        <CancerGovWww:AlphaListBox runat="server" ID="alphaListBox"
            NumericItems="true" ShowAll="false" />
    </div>
    </form>
</asp:PlaceHolder>
