<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DictionarySearchBlock.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.DictionarySearchBlock" %>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="~/Common/UserControls/AlphaListBox.ascx" %>


<script type="text/javascript">
    // function used by AutoComplete to submit to server when user
    // selects an item
    function ACOnSubmit() {
        document.getElementById('<%=btnGo.ClientID%>').click();
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
        var language = "English";
        if ($("html").attr("lang") === "es")
            language = "Spanish";

        var isContains = IsContains();
        var svcUrl = "";
        if (isContains)
            svcUrl = "/TermDictionary.svc/SearchJSON/" + language + "?contains=true";
        else
            svcUrl = "/TermDictionary.svc/SearchJSON/" + language;


        //alert(svcUrl);

        NCI.doAutocomplete("#" + ids.AutoComplete1, svcUrl, isContains, "searchTerm", { maxRows: 10 });
    }

    function IsContains() {
        var ret = false;

        if ($("#" + ids.radioContains).prop("checked"))
            ret = true;

        return ret;
    }
</script>

<asp:PlaceHolder ID="phTermDictionarySearchBlockText" runat="server" Visible="false">
        <asp:PlaceHolder ID="pnlIntroEnglish" runat="server" EnableViewState="false">
            <p>
                The NCI Dictionary of Cancer Terms features <b><% =TotalCount %></b> terms related 
                to cancer and medicine.
            </p>
            <p>
                Browse the dictionary by selecting a letter of the alphabet or by entering a cancer-related
                word or phrase in the search box.
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pnlIntroSpanish" runat="server" Visible="false" EnableViewState="false">
            <p>
                El diccionario de cáncer del NCI contiene <b><% =TotalCount %></b> términos relacionados 
                con el cáncer y la medicina.
            </p>
            <p>
                Para buscar un término en el diccionario presione en cualquier letra del alfabeto o escriba 
                una palabra o frase relacionada con el cáncer en la casilla de búsqueda.
            </p>
        </asp:PlaceHolder>
   </asp:PlaceHolder>     
   
   <asp:PlaceHolder ID="phGeneticsTermDictionarySearchBlockText" runat="server" Visible="false">
<div id="welcomeDiv">
    <p>Welcome to the NCI Dictionary of Genetics Terms, which contains technical definitions for more than 150 terms related to genetics. These definitions were developed by the <a href="/cancertopics/pdq/cancer-genetics-board">PDQ® Cancer Genetics Editorial Board</a> to support the evidence-based, peer-reviewed <a href="/cancertopics/pdq/genetics">PDQ cancer genetics information summaries</a>.</p>
</div>

</asp:PlaceHolder>
<asp:PlaceHolder ID="phDrugDictionarySearchBlockText" runat="server" Visible="false">
<%--

This search block contains the dictionary text at the top

Blair will add code here for the Drug Dictionary
--%>
</asp:PlaceHolder>

        <asp:PlaceHolder ID="pnlTermSearch" runat="server">
        <form id="searchForm" aria-label="Search the Dictionary of Cancer Terms" runat="server">
        <div class="dictionary-search">
            <div class="hidden">
                The search textbox has an autosuggest feature. When you enter three or more characters,
                a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
                to move through the suggestions. To select a suggestion, hit the enter key. Using
                the escape key closes the listbox and puts you back at the textbox. The radio buttons
                allow you to toggle between having all search items start with or contain the text
                you entered in the search box.
            </div>
	                   
                <div id="dictionary_jPlayer"></div>
                
                <div class="row">
                    <div class="small-12 columns">              
                        <span class="radio">
                        <asp:RadioButton ID="radioStarts" runat="server" GroupName="sgroup" Checked="true"    />
                            <asp:Label ID="lblStartsWith" class="inline" runat="server" Text="Starts with"
                                AssociatedControlID="radioStarts"></asp:Label>
                        </span>
                        <span class="radio">
                            <asp:RadioButton ID="radioContains" runat="server" GroupName="sgroup"  />
                            <asp:Label ID="lblContains" runat="server" Text="Contains" class="inline" 
                                AssociatedControlID="radioContains"></asp:Label>
                        </span>
                    </div>
                </div>
                <div class="row">
                    <div class="large-6 columns">
                        <asp:TextBox CssClass="dictionary-search-input" ID="AutoComplete1"
                            inputmode="latin" aria-autocomplete="list" runat="server" 
                            CallbackFunc="ACOnSubmit" autocomplete="off" />
                    </div>
                    <div class="large-2 columns left">        
                        <asp:Button class="submit button postfix" Name="btnGo" ID="btnGo" runat="server" OnClick="btnGo_OnClick"
                            ToolTip="Search" />
                    </div>
                </div>
    
        </div>
	    <div class="az-list">
	        <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/drugdictionary.aspx"
                NumericItems="true" ShowAll="false" />
	    </div>
        </form>
</asp:PlaceHolder>





