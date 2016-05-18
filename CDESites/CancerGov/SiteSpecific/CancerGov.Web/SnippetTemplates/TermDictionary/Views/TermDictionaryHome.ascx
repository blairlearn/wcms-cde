<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.TermDictionaryHome" %>
<%@ Register TagPrefix="DictionarySearchBlock" TagName="SearchBlock" Src="~/SnippetTemplates/TermDictionary/DictionarySearchBlock.ascx" %>


        <asp:PlaceHolder ID="pnlIntroEnglish" runat="server" EnableViewState="false">
            <p>
                The NCI Dictionary of Cancer Terms features <b><%= TotalCount %></b> terms related 
                to cancer and medicine.
            </p>
        </asp:PlaceHolder>
        <asp:PlaceHolder ID="pnlIntroSpanish" runat="server" Visible="false" EnableViewState="false">
            <p>
                El diccionario de cáncer del NCI contiene <b><%= TotalCount %></b> términos relacionados 
                con el cáncer y la medicina.
            </p>
        </asp:PlaceHolder>

 
<DictionarySearchBlock:SearchBlock id="dictionarySearchBlock" runat="server" />
