<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="db_alpha.ascx.cs" Inherits="Www.Templates.DbAlpha" %>
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

        NCI.doAutocomplete("#" + ids.AutoComplete1, svcUrl, isContains, "searchTerm", { maxRows: 10 });
    }

    function IsContains() {
        var ret = false;

        if ($("#"+ids.radioContains).prop("checked"))
            ret = true;

        return ret;
    }
</script>

<!-- Content Header
	<div id="headerzone" style="margin-right: auto;margin-left: auto; width: 751px;">
	    <table width="751" cellspacing="0" cellpadding="0" border="0">
	        <tr>
	            <td>
	                <CancerGovWww:LangSwitch ID="gutterLangSwitch" EnableBothLinks="true" Visible="false" runat="server"></CancerGovWww:LangSwitch>        
	            </td>
	        </tr>
	    </table>
	    
	</div> 
-->

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="ViewDefault" runat="server" EnableViewState="false">
        <asp:Panel ID="pnlIntroEnglish" runat="server" EnableViewState="false">
            <p>
                Cancer terminology is often complicated. The NCI Dictionary of Cancer Terms features 
                <b><% =TotalCount %></b> terms related to cancer and medicine.
            </p>
            <p>
                Browse the dictionary by selecting a letter of the alphabet or by entering a cancer-related
                word or phrase in the search box.
            </p>
        </asp:Panel>
        <asp:Panel ID="pnlIntroSpanish" runat="server" Visible="false" EnableViewState="false">
            <p>
                El diccionario de cáncer del NCI contiene <b><% =TotalCount %></b> términos 
                relacionados con el cáncer y la medicina.
            </p>
            <p>
                Para buscar un término en el diccionario presione en cualquier letra del alfabeto o escriba 
                una palabra o frase relacionada con el cáncer en la casilla de búsqueda.
            </p>
        </asp:Panel>
    </asp:View>
</asp:MultiView>

<asp:Panel ID="pnlTermSearch" name="pnlTermSearch" runat="server">
    <div class="dictionary-search">
        <div class="hidden">
            The search textbox has an autosuggest feature. When you enter three or more characters,
            a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
            to move through the suggestions. To select a suggestion, hit the enter key. Using
            the escape key closes the listbox and puts you back at the textbox. The radio buttons
            allow you to toggle between having all search items start with or contain the text
            you entered in the search box.
        </div>
	    <form name="aspnetForm" method="post" action="/dictionary/" id="aspnetForm" onsubmit="NCIAnalytics.TermsDictionarySearch(this,false);"
            role="search" aria-label="Search the Dictionary of Cancer Terms" runat="server">
        
            <div id="dictionary_jPlayer"></div>
            
            <div class="row">
                <div class="small-2 columns">              
                    <span class="radio">
                        <asp:RadioButton ID="radioStarts" runat="server" Checked="true" GroupName="sgroup" />
                        <asp:Label ID="lblStartsWith" class="inline" runat="server" Text="Starts with"
                            AssociatedControlID="radioStarts"></asp:Label>
                    </span>
                </div>
                <div class="small-2 columns left">
                    <span class="radio">
                        <asp:RadioButton ID="radioContains" runat="server" GroupName="sgroup" />
                        <asp:Label ID="lblContains" runat="server" Text="Contains" class="inline" 
                            AssociatedControlID="radioContains"></asp:Label>
                    </span>
                </div>
            </div>
            <div class="row">
                <div class="medium-6 columns">
                    <asp:TextBox CssClass="dictionary-search-input" ID="AutoComplete1"
                        inputmode="latin" aria-autocomplete="list" runat="server" 
                        CallbackFunc="ACOnSubmit" autocomplete="off" />
                </div>
                <div class="medium-2 columns left">        
                    <asp:Button class="submit button postfix" Name="btnGo" ID="btnGo" runat="server"
                        ToolTip="Search" />
                </div>
        </form>
    </div>
	<div class="az-list">
	    <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/drugdictionary.aspx"
            NumericItems="true" ShowAll="false" />
	</div>
</asp:Panel>	

<asp:MultiView ID="MultiView2" runat="server" ActiveViewIndex="0">
    <asp:View ID="ViewResultList" runat="server" EnableViewState="false">
        <div class="results">
            <!-- Number of results -->
            <asp:Panel ID="numResDiv" runat="server" CssClass="dictionary-search-results-header">
                <span class="results-count">
                    <asp:Label ID="lblNumResults" CssClass="results-num" runat="server"></asp:Label>
                    <asp:Label ID="lblResultsFor" runat="server"></asp:Label>
                    <asp:Label ID="lblWord" CssClass="term" runat="server"></asp:Label>
                </span>
            </asp:Panel>
                
            <asp:ListView ID="resultListView" runat="server">
                <LayoutTemplate>
                    <dl class="dictionary-list">
                        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                    </dl>
                </LayoutTemplate>
                <ItemTemplate>
                    <dt>
                        <dfn>
                             <a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%><%=QueryStringLang%>" <%# ResultListViewHrefOnclick(Container)%>>
                             <%# Eval("TermName")%></a>
                        </dfn>
                    </dt>
                    <dd class="pronunciation">
                        <%# AudioMediaHTML(DataBinder.Eval(Container.DataItem, "AudioMediaHTML")) %>
                        <span><%#DataBinder.Eval(Container.DataItem, "TermPronunciation")%></span>
                    </dd>
                    <dd class="definition">
                        <%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%>
                    </dd>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <asp:Panel ID="pnlNoDataEnglish" runat="server" Visible="false">
                        No matches were found for the word or phrase you entered. Please check your spelling,
                        and try searching again. You can also type the first few letters of your word or
                        phrase, or click a letter in the alphabet and browse through the list of terms that
                        begin with that letter.
                    </asp:Panel>
                    <asp:Panel ID="pnlNoDataSpanish" runat="server" Visible="false">
                        No se encontraron resultados para lo que usted busca. Revise si escribi&oacute;
                        correctamente e inténtelo de nuevo. También puede escribir las primeras letras de
                        la palabra o frase que busca o hacer clic en la letra del alfabeto y revisar la
                        lista de términos que empiezan con esa letra.
                    </asp:Panel>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </asp:View>
    <asp:View ID="ViewDefinition" runat="server" EnableViewState="false">
        <!-- Language buttons -->
        <CancerGovWww:LangSwitch ID="langSwitch" runat="server">
        </CancerGovWww:LangSwitch>
        <!-- Term and def -->
        <div class="results">
            <dl class="dictionary-list">
                <dt>
                    <dfn>
                        <asp:Label ID="lblTermName" runat="server"></asp:Label>
                    </dfn>
                </dt>
                <dd class="pronunciation">
                    <asp:Literal ID="litAudioMediaHtml" runat="server"></asp:Literal>
                    <asp:Label ID="lblTermPronun" runat="server"></asp:Label>
                </dd>
                <dd class="definition">
                    <asp:Literal ID="litDefHtml" runat="server"></asp:Literal>
                    <asp:Panel runat="server" ID="pnlRelatedInfo">
                        <asp:Literal ID="litRelatedLinkInfo" runat="server"></asp:Literal>
                    </asp:Panel>
                    <asp:Literal ID="litImageHtml" runat="server"></asp:Literal>
                </dd>
            </dl>
        </div>
        
        <asp:Panel ID="pnlDefPrint" runat="server" Visible="false">
        </asp:Panel>
    </asp:View>
</asp:MultiView>

<!-- Footer -->
<div id="footerzone" align="center">
    <asp:Literal ID="litPageFooter" runat="server"></asp:Literal>
</div>
<asp:Literal ID="litOmniturePageLoad" Mode="PassThrough" runat="server" />
