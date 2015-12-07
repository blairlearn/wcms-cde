<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.TermDictionaryHome" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>


        <asp:PlaceHolder ID="pnlIntroEnglish" runat="server" EnableViewState="false">
            <p>
                The NCI Dictionary of Cancer Terms features <b><%= TotalCount %></b> terms related 
                to cancer and medicine.
            </p>
            <p>
                Browse the dictionary by selecting a letter of the alphabet or by entering a cancer-related
                word or phrase in the search box.
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pnlIntroSpanish" runat="server" Visible="false" EnableViewState="false">
            <p>
                El diccionario de cáncer del NCI contiene <b><%= TotalCount %></b> términos relacionados 
                con el cáncer y la medicina.
            </p>
            <p>
                Para buscar un término en el diccionario presione en cualquier letra del alfabeto o escriba 
                una palabra o frase relacionada con el cáncer en la casilla de búsqueda.
            </p>
        </asp:PlaceHolder>

 
<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />
