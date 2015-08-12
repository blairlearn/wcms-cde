<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DictionarySearchBlock.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.DictionarySearchBlock" %>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="~/Common/UserControls/AlphaListBox.ascx" %>

<%-- 8/12/2015 - Make sure the appropriate javascript file is loaded
--%>
<asp:PlaceHolder ID="phTermDictionarySearchBlock" runat="server">
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
        <asp:PlaceHolder ID="pnlTermSearch" name="pnlTermSearch" runat="server">
        
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
                    <div class="small-12 columns">              
                        <span class="radio">
                            <asp:RadioButton ID="radioStarts" runat="server" Checked="true" GroupName="sgroup" />
                            <asp:Label ID="lblStartsWith" class="inline" runat="server" Text="Starts with"
                                AssociatedControlID="radioStarts"></asp:Label>
                        </span>
                        <span class="radio">
                            <asp:RadioButton ID="radioContains" runat="server" GroupName="sgroup" />
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
                        <asp:Button class="submit button postfix" Name="btnGo" ID="btnGo" runat="server"
                            ToolTip="Search" />
                    </div>
                </div>
            </form>
        </div>
	    <div class="az-list">
	        <CancerGovWww:AlphaListBox runat="server" id="alphaListBox" BaseUrl="/templates/drugdictionary.aspx"
                NumericItems="true" ShowAll="false" />
	    </div>
        
</asp:PlaceHolder>

</asp:PlaceHolder>
<asp:PlaceHolder ID="phGeneticsTermDictionarySearchBlock" runat="server">

</asp:PlaceHolder>

<asp:PlaceHolder ID="phDrugDictionarySearchBlock" runat="server">
<%--

This search block contains the dictionary text at the top, the search text box and the a-z list.

Blair will add code here for the Drug Dictionary
--%>
</asp:PlaceHolder>

