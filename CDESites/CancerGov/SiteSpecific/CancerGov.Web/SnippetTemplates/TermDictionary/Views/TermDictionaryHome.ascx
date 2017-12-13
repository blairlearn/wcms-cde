<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Dictionaries.SnippetControls.TermDictionary.TermDictionaryHome" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>


        <asp:PlaceHolder ID="pnlIntroEnglish" runat="server" EnableViewState="false">
            <p>
                The NCI Dictionary of Cancer Terms features <b><%= TotalCount %></b> terms related 
                to cancer and medicine.
            </p>
            <p>
                We offer a widget that you can add to your website to let users look up cancer-related terms. <a href="/syndication/widgets">Get NCI’s Dictionary of Cancer Terms Widget</a>.
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pnlIntroSpanish" runat="server" Visible="false" EnableViewState="false">
            <p>
                El diccionario de cáncer del NCI contiene <b><%= TotalCount %></b> términos relacionados 
                con el cáncer y la medicina.
            </p>
            <p>
                Ofrecemos un widget que usted puede añadir a su sitio web para que sus usuarios puedan buscar términos de cáncer. <a href="/espanol/sindicacion/widgets">Obtenga el widget de términos de cáncer del Diccionario del NCI</a>.
            </p>
        </asp:PlaceHolder>

 
<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />
