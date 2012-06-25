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
    var ids = {
        radioStarts: "<%=radioStarts.ClientID %>"
    , AutoComplete1: "<%=AutoComplete1.ClientID %>"
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

                <asp:Label ID="lblAutoComplete1" style="position:absolute;left:-5000px" runat="server" Text="Search for" 
                    AssociatedControlID="AutoComplete1"></asp:Label>  
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

<asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
    <asp:View ID="ViewDefault" runat="server" EnableViewState="false">
        <img src="/images/spacer.gif" width="10" height="1" alt="" border="0" />
        <asp:Panel ID="pnlIntroEnglish" runat="server" EnableViewState="false">
            <p>
                Welcome to the NCI Dictionary of Cancer Terms, a resource with more than 6,000 terms
                related to cancer and medicine.</p>
            <p>
                <b>Tips on Looking Up a Word or Phrase</b></p>
            <ul>
                <li>In the search box, type the word or phrase you are looking for and click the "Go"
                    button.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>Click the radio button in front of the word "Contains" when you want to find all
                    terms in the dictionary that <b>include</b> a word or set of letters. For example,
                    if you type "lung" and select "Contains," the search will find terms such as "small
                    cell lung cancer" as well as "lung cancer."</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>You can also click on a letter of the alphabet to browse through the dictionary.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>The search box has an <b>autosuggest</b> feature. When you type three or more letters,
                    a list of up to 10 suggestions will pop up below the box. Click on a suggestion
                    with your mouse or use the arrow keys on your keyboard to move through the suggestions
                    and then hit the Enter key to choose one.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>Using the Escape key or clicking "close" within the autosuggest box closes the box
                    and turns off the feature until you start a new search.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>After you find your word or phrase, use the tabs under the search box to toggle
                    between definitions in English and Spanish.</li>
            </ul>
        </asp:Panel>
        <asp:Panel ID="pnlIntroSpanish" runat="server" Visible="false" EnableViewState="false">
            <p>
                Bienvenidos al Diccionario de cáncer del NCI, un recurso con más de 6.000 términos
                relacionados con el cáncer y la medicina.</p>
            <p>
                <b>Consejos para buscar una palabra o frase</b></p>
            <ul>
                <li>En la ventanilla para buscar, escriba la palabra o frase que busca y presione el
                    botón que dice "Buscar".</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>Presione el espacio que está frente a la palabra "Contiene" si quiere encontrar
                    todos los términos del diccionario que <b>incluyen</b> una palabra o conjunto de
                    palabras. Por ejemplo, si usted escribe "pulmón" y selecciona "Contiene", la búsqueda
                    encontrará términos tales como "cáncer de pulmón de células no pequeñas" al igual
                    que "cáncer de pulmón".</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>Usted también puede presionar en cualquier letra del abecedario para hojear todo
                    el diccionario.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>La ventanilla para buscar contiene una función llamada <b>sugerencia.</b> Cuando
                    usted escribe más de tres letras, le aparecerá una lista de hasta diez sugerencias
                    debajo de la ventanilla. Presione con el ratón sobre la sugerencia que le interesa
                    o utilice las teclas con flechas para ver todas las opciones y luego presione la
                    tecla Enter para elegir una.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>Cuando usted usa la tecla Esc o presiona "cerrar" dentro del espacio de sugerencia,
                    este espacio se cierra apagando dicha función hasta que se inicie una nueva búsqueda.</li><br />
                <!--[if IE]>
                        <br />
                    <![endif]-->
                <li>Una vez que encuentre la palabra o frase que le interesa, use los letreros debajo
                    de la ventanilla para buscar si quiere alternar entre definiciones en inglés o español.</li><br />
            </ul>
        </asp:Panel>
    </asp:View>
    <asp:View ID="ViewResultList" runat="server" EnableViewState="false">
        <!-- Number of results -->
        <asp:Panel ID="numResDiv" runat="server" CssClass="dictionary-search-results-header">
            <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
            <span class="page-title">
                <asp:Label ID="lblNumResults" CssClass="page-title" runat="server"></asp:Label>
                <asp:Label ID="lblResultsFor" CssClass="page-title" runat="server"></asp:Label>
            </span>&nbsp;&nbsp; &nbsp;&nbsp;
            <asp:Label ID="lblWord" CssClass="search-result" runat="server"></asp:Label><br />
            <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
            <img src="/images/gray_spacer.gif" width="571" height="1" alt="" border="0" /><br />
            <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
        </asp:Panel>
        <asp:ListView ID="resultListView" runat="server">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <a name="<%#DataBinder.Eval(Container.DataItem, "TermName")%>"></a><a href="<%# DictionaryURL %>?CdrID=<%#DataBinder.Eval(Container.DataItem, "GlossaryTermID")%><%=QueryStringLang%>"
                    <%# ResultListViewHrefOnclick(Container)%>>
                    <%# Eval("TermName")%></a> &nbsp;&nbsp;
                <%# AudioMediaHTML(DataBinder.Eval(Container.DataItem, "AudioMediaHTML")) %>&nbsp;&nbsp;
                <%#DataBinder.Eval(Container.DataItem, "TermPronunciation")%>
                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                    <tr>
                        <td width="100%" align="left">
                            <%#DataBinder.Eval(Container.DataItem, "DefinitionHTML")%>
                        </td>
                    </tr>
                </table>
                <br>
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
    </asp:View>
    <asp:View ID="ViewDefinition" runat="server" EnableViewState="false">
        <!-- Language buttons -->
        <CancerGovWww:LangSwitch ID="langSwitch" runat="server">
        </CancerGovWww:LangSwitch>
        <img src="/images/spacer.gif" width="10" height="19" alt="" border="0" /><br />
        <!-- Term and def -->
        <span class="header-A">
            <asp:Label ID="lblTermName" runat="server"></asp:Label></span>&nbsp;
        <asp:Literal ID="litAudioMediaHtml" runat="server"></asp:Literal>&nbsp;
        <asp:Label ID="lblTermPronun" runat="server"></asp:Label><br />
        <img src="/images/spacer.gif" width="10" height="5" alt="" border="0" /><br />
        <table width="" cellspacing="0" cellpadding="0" border="0">
            <tr>
                <td valign="top">
                    &nbsp;
                </td>
                <td valign="top">
                    <asp:Literal ID="litDefHtml" runat="server"></asp:Literal>
                    <asp:Panel runat="server" ID="pnlRelatedInfo">
                        <br />
                        <asp:Literal ID="litRelatedLinkInfo" runat="server"></asp:Literal>
                    </asp:Panel>
                    <asp:Literal ID="litImageHtml" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        <!-- <asp:Panel ID="pnlDefImages" runat="server">
                <img src="/images/spacer.gif" width="10" height="25" alt="" border="0"/><br/>
	            <img src="/images/gray_spacer.gif" width="571" height="1" alt="" border="0"/><br/>
	            <img src="/images/spacer.gif" width="1" height="25" alt="" border="0"/><br/>
            </asp:Panel> -->
        <asp:Panel ID="pnlDefPrint" runat="server" Visible="false">
            <p>
            </p>
        </asp:Panel>
        <!-- <asp:Panel ID="pnlPrevNext" runat="server" Visible="false">
                <table width="100%"  border="0" cellspacing="0" cellpadding="2">
                    <tr>
                        <td class="col1" valign="top"><b>
                            <asp:Label ID="lblPrevText" runat="server" Visible="false"></asp:Label></b>
                        </td>
                        <td class="col2" valign="top">
                            <asp:Literal ID="litPrevTerms" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="col1" valign="top"><b>
                            <asp:Label ID="lblNextText" runat="server" Visible="false"></asp:Label></b>
                        </td>
                        <td class="col2" valign="top">
                            <asp:Literal ID="litNextTerms" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </asp:Panel> -->
    </asp:View>
</asp:MultiView>
<!-- <img src="/images/spacer.gif" width="10" height="19" alt="" border="0"><br>-->
<asp:Literal ID="litBackToTop" runat="server" Visible="false"></asp:Literal>


</form>
<!-- Footer -->
<div id="footerzone" align="center">
    <asp:Literal ID="litPageFooter" runat="server"></asp:Literal>
</div>
<asp:Literal ID="litOmniturePageLoad" Mode="PassThrough" runat="server" />
