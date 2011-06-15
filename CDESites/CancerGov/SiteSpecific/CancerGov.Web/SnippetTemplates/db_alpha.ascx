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
	    
	</div> -->
<!-- Main Area -->
<form name="aspnetForm" method="post" action="/dictionary/" id="aspnetForm" onsubmit="NCIAnalytics.TermsDictionarySearch(this,false);"
runat="server">
<a name="top"></a>
<!--<div style="margin-right: auto;margin-left: auto; width: 571px;">-->
<table width="571" cellspacing="0" cellpadding="0" border="0">
    <tr>
        <!-- Left Nav Column -->
        <td id="leftzone" valign="top">
            <asp:Literal ID="litPageLeftColumn" runat="server"></asp:Literal>
        </td>
        <!-- Main content of page -->
        <td id="contentzone" valign="top" width="100%">
            <a name="skiptocontent"></a>
            <!-- ADMIN TOOL CONTENT GOES HERE -->
            <!-- Search area -->
            <asp:Panel ID="pnlTermSearch" runat="server" CssClass="pnlTermSearch">
                <div class="searchboxTopRow" class="searchboxTopRow">
                    <div id="searchboxContainer" runat="server" class="searchboxContainer">
                        <div class="hidden">
                            The search textbox has an autosuggest feature. When you enter three or more characters,
                            a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
                            to move through the suggestions. To select a suggestion, hit the enter key. Using
                            the escape key closes the listbox and puts you back at the textbox. The radio buttons
                            allow you to toggle between having all search items start with or contain the text
                            you entered in the search box.</div>
                        <asp:Label CssClass="lblStrSearch" ID="lblStrSearch" runat="server" Text="Search for"></asp:Label>
                        <CGov:AutoComplete CssClass="AutoComplete1" Name="AutoComplete1" ID="AutoComplete1"
                            runat="server" CallbackFunc="ACOnSubmit" autocomplete="off" MinWidth="333" />
                        <asp:Label ID="lblAccessSearch" CssClass="hidden" runat="server" Text="Search text box"
                            AssociatedControlID="AutoComplete1"></asp:Label>
                    </div>
                    <div id="searchboxBtn" runat="server" class="searchboxBtn">
                        <asp:ImageButton CssClass="btnGo" Name="btnGo" ID="btnGo" runat="server" ImageUrl="/images/red_go_button.gif"
                            AlternateText="Search" ToolTip="Search" />
                    </div>
                    <div id="searchboxStarts" runat="server" class="searchboxStarts">
                        <asp:RadioButton Name="radioStarts" ID="radioStarts" runat="server" Checked="True"
                            GroupName="sgroup" Text="Starts with" ToolTip="Search item starts with this" />
                    </div>
                    <div id="searchboxSeparator" class="searchboxSeparator">
                        <img alt="" src="/images/dictionary-search-radio.gif" />
                    </div>
                    <div id="searchboxContains" class="searchboxContains">
                        <asp:RadioButton Name="radioContains" ID="radioContains" runat="server" GroupName="sgroup"
                            Text="Contains" ToolTip="Search item contains this" />
                    </div>
                </div>
                <div class="search-control-div">
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/db_alpha.aspx"
                            NumericItems="true" />
                    </table>
                </div>
            </asp:Panel>
            <!-- end Search area -->
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
                    <asp:Panel ID="numResDiv" runat="server">
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
        </td>
        <td valign="top">
            <img src="/images/spacer.gif" width="10" height="1" alt="" border="0" />
        </td>
    </tr>
</table>
<!--</div>-->
</form>
<!-- Footer -->
<div id="footerzone" align="center">
    <asp:Literal ID="litPageFooter" runat="server"></asp:Literal>
</div>
<asp:Literal ID="litOmniturePageLoad" Mode="PassThrough" runat="server" />
