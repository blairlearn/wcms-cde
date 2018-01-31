<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Dictionaries.SnippetControls.DictionaryHTMLSearchBlock" EnableViewState="false"%>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="~/Common/UserControls/AlphaListBox.ascx" %>

<form id="aspnetForm" aria-label="Search the Dictionary of Cancer Terms" method="get" action="<%=this.FormAction%>">
    <div class="dictionary-search">
        <div id="espanolHelpText" class="hidden">
            La casilla de texto para búsquedas ofrece sugerencias. Al poner tres o más caracteres, 
            aparecerán en la casilla hasta 10 sugerencias. Use las teclas de flechas para moverse 
            por las sugerencias. Para seleccionar una, oprima la tecla de Enter. Al oprimir la 
            tecla de Escape, se cerrará la lista de la casilla y le regresará a la casilla de texto. 
            Los botones de radio le permiten que todos los términos de la búsqueda empiecen o que 
            contengan el texto que usted puso en la casilla de búsqueda. 
        </div>
        <div id="dictionary_jPlayer">
        </div>
        <div class="row">
            <div class="small-12 columns">
                <span class="radio">
                    <input id="radioStarts" type="radio" name="contains" value="false" <%=CheckRadioStarts%> data-autosuggest="dict-radio-starts">
                    <label id="lblStartsWith" class="inline" for="radioStarts">Empieza con</label>
                </span>
                <span class="radio">
                    <input id="radioContains" type="radio" name="contains" value="true" <%=CheckRadioContains%> data-autosuggest="dict-radio-contains">
                    <label id="lblContains" class="inline" for="radioContains">Contiene</label>
                </span>
            </div>
        </div>
        <div class="row">
            <div class="large-6 columns">
                <input type="text" class="dictionary-search-input" name="q" id="AutoComplete1" 
                    aria-autocomplete="list" autocomplete="off" aria-label="Escriba frase o palabra clave"
                    placeholder="Escriba frase o palabra clave" data-autosuggest="dict-autocomplete"
                    value="<%=SearchBoxInputVal%>">
            </div>
            <div class="large-2 columns left">
                <input type="submit" class="submit button postfix" id="btnSearch" title="Buscar" value="Buscar">
            </div>
        </div>
    </div>
    <div class="az-list">
        <CancerGovWww:AlphaListBox runat="server" ID="alphaListBox"
            NumericItems="true" ShowAll="false" />
    </div>
</form>