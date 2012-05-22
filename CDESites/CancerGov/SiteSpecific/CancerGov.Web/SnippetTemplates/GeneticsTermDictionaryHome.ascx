<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryHome" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>
<div class="searchHome">
    <p>
        Welcome to the NCI Dictionary of Genetics Terms
    </p>
    <p>
        <b>Tips on Looking Up a Word or Phrase</b></p>
    <ul>
        <li>In the search box, type the word or phrase you are looking for and click the "Search"
            button.</li><br />

        <li>Click the radio button in front of the word "Contains" when you want to find all
            terms in the dictionary that <b>include</b> a word or set of letters. For example,
            if you type "lung" and select "Contains," the search will find terms such as "small
            cell lung cancer" as well as "lung cancer."</li><br />
        <li>You can also click on a letter of the alphabet to browse through the dictionary.</li><br />
        <li>The search box has an <b>autosuggest</b> feature. When you type three or more letters,
            a list of up to 10 suggestions will pop up below the box. Click on a suggestion
            with your mouse or use the arrow keys on your keyboard to move through the suggestions
            and then hit the Enter key to choose one.</li><br />
        <li>Using the Escape closes the box 
            and turns off the feature until you start a new search.</li><br />
    </ul>
</div>