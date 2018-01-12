<%@ Control Language="C#" AutoEventWireup="true" Inherits="CancerGov.Dictionaries.SnippetControls.DictionaryHTMLSearchBlock" EnableViewState="false"%>
<%@ Register TagPrefix="CancerGovWww" TagName="AlphaListBox" Src="~/Common/UserControls/AlphaListBox.ascx" %>

<form id="aspnetForm" aria-label="Search the Dictionary of Cancer Terms" method="get" action="<%=this.FormAction%>">
    <div class="dictionary-search">
        <div id="englishHelpText" class="hidden">
            The search textbox has an autosuggest feature. When you enter three or more characters,
            a list of up to 10 suggestions will popup under the textbox. Use the arrow keys
            to move through the suggestions. To select a suggestion, hit the enter key. Using
            the escape key closes the listbox and puts you back at the textbox. The radio buttons
            allow you to toggle between having all search items start with or contain the text
            you entered in the search box.
        </div>
        <div id="dictionary_jPlayer">
        </div>
        <div class="row">
            <div class="small-12 columns">
                <span class="radio">
                    <input id="radioStarts" type="radio" name="contains" value="false" checked="checked" data-autosuggest="dict-radio-starts">
                    <label id="lblStartsWith" class="inline" for="radioStarts">Starts with</label>
                </span>
                <span class="radio">
                    <input id="radioContains" type="radio" value="true" name="contains" data-autosuggest="dict-radio-contains">
                    <label id="lblContains" class="inline" for="radioContains">Contains</label>
                </span>
            </div>
        </div>
        <div class="row">
            <div class="large-6 columns">
                <input type="text" class="dictionary-search-input" name="q" id="AutoComplete1" 
                    aria-autocomplete="list" autocomplete="off" aria-label="Enter keywords or phrases"
                    placeholder="Enter keywords or phrases" data-autosuggest="dict-autocomplete">
            </div>
            <div class="large-2 columns left">
                <input type="submit" class="submit button postfix" id="btnSearch" title="Search" value="Search">
            </div>

            <asp:Literal ID="showHelpButton" runat="server" Visible="false">
                <div class="medium-1 columns left" id="helpButton">
                    <a class="text-icon-help" aria-label="Help" href="javascript:dynPopWindow('/Common/PopUps/popHelp.aspx','popup','width=500,height=700,scrollbars=1,resizable=1,menubar=0,location=0,status=0,toolbar=0')">?
                    </a>
                </div>
            </asp:Literal>
        </div>
    </div>
    <div class="az-list">
        <CancerGovWww:AlphaListBox runat="server" ID="alphaListBox"
            NumericItems="true" ShowAll="false" />
    </div>
</form>