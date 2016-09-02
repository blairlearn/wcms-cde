<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DictionarySearchBlock.ascx.cs"
    Inherits="CancerGov.Web.SnippetTemplates.DictionarySearchBlock"%>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="~/Common/UserControls/AlphaListBox.ascx" %>

<script type="text/javascript">
    (function (factory) {
        if (typeof define === 'function' && define.amd && typeof require === 'function') {
            // AMD
            require(['jquery', 'jquery/jplayer'], factory);
        } else {
            // Browser globals
            factory(jQuery);
        }
    }(function (jQuery) {
        //Hookup JPlayer for Audio
        if (jQuery.jPlayer && !Modernizr.touch) {
            jQuery(document).ready(function ($) {
                var my_jPlayer = $("#dictionary_jPlayer");

                my_jPlayer.jPlayer({
                    swfPath: "/PublishedContent/files/global/flash/", //Path to SWF File Used by jPlayer
                    //errorAlerts: true,
                    supplied: "mp3" //The types of files which will be used.
                });

                //Attach a click event to the audio link

                $(".CDR_audiofile").click(function () {
                    my_jPlayer.jPlayer("setMedia", {
                        mp3: $(this).attr("href") // Defines the m4v url
                    }).jPlayer("play");

                    return false;
                });
            });
        }
    }));

    // Autocomplete functionality
    var ids = {
        radioStarts: "<%=radioStarts.ClientID %>",
        radioContains: "<%=radioContains.ClientID %>",
        AutoComplete1: "<%=AutoComplete1.ClientID %>"
    }

    $(document).ready(function () {
        autoFunc();
    });

    function autoFunc() {
        var dictionary = "<%=Dictionary.ToString() %>";
        var language = 'English';
        if ($('html').attr('lang') === 'es') {
            language = 'Spanish';
        }
        var isContains = IsContains();

        (function (factory) {
            if (typeof define === 'function' && define.amd && typeof require === 'function') {
                // AMD
                require(['Common/Enhancements/NCI', 'Data/DictionaryService'], factory);
            } else {
                // Browser globals
                factory(NCI, NCI.dictionary);
            }
        }(function (NCI, DictionaryService) {
            NCI.doAutocomplete('#' + ids.AutoComplete1, function (term) { return DictionaryService.searchSuggest(dictionary, term, language, isContains ? 'contains' : 'begins'); }, isContains);
        }));
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
            <asp:Panel ID="englishHelpText" Visible="false" runat="server" CssClass="hidden">
                The search textbox has an autosuggest feature. When you enter three or more characters,
                a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
                to move through the suggestions. To select a suggestion, hit the enter key. Using
                the escape key closes the listbox and puts you back at the textbox. The radio buttons
                allow you to toggle between having all search items start with or contain the text
                you entered in the search box.
            </asp:Panel>
            <asp:Panel ID="espanolHelpText" Visible="false" runat="server" CssClass="hidden">
                La casilla de texto para búsquedas ofrece sugerencias. Al poner tres o más caracteres, 
                aparecerán en la casilla hasta 10 sugerencias. Use las teclas de flechas para moverse 
                por las sugerencias. Para seleccionar una, oprima la tecla de Enter. Al oprimir la 
                tecla de Escape, se cerrará la lista de la casilla y le regresará a la casilla de texto. 
                Los botones de radio le permiten que todos los términos de la búsqueda empiecen o que 
                contengan el texto que usted puso en la casilla de búsqueda. 
            </asp:Panel>
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
                        aria-autocomplete="list" runat="server" autocomplete="off" />
                </div>
                <div class="large-2 columns left">
                    <asp:Button class="submit button postfix" ID="btnSearch" runat="server" OnClick="btnSearch_OnClick"
                        ToolTip="Search" />
                </div>

                <asp:Panel runat="server" CssClass="medium-1 columns left" ID="helpButton" Visible="false">
                    <a class="text-icon-help" aria-label="Help" href="javascript:dynPopWindow('/Common/PopUps/popHelp.aspx','popup','width=500,height=700,scrollbars=1,resizable=1,menubar=0,location=0,status=0,toolbar=0')">?
                    </a>
                </asp:Panel>
            </div>
        </div>
        <div class="az-list">
            <CancerGovWww:AlphaListBox runat="server" ID="alphaListBox"
                NumericItems="true" ShowAll="false" />
        </div>
    </form>
</asp:PlaceHolder>
