<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneticsTermDictionaryHome.ascx.cs" Inherits="CancerGov.Web.SnippetTemplates.GeneticsTermDictionaryHome" %>
<asp:Literal runat="server" ID="litPageUrl" Visible="false"></asp:Literal>
<div id="welcomeDiv">
    <p>Welcome to the NCI Dictionary of Genetics Terms, which contains technical definitions for more than 150 terms related to genetics. These definitions were developed by the <a href="http://www.cancer.gov/cancertopics/pdq/cancer-genetics-board">PDQ® Cancer Genetics Editorial Board</a> to support the evidence-based, peer-reviewed <a href="http://www.cancer.gov/cancertopics/pdq/genetics">PDQ cancer genetics information summaries</a>.</p>
    <h3>Tips on Looking Up a Word or Phrase</h3>
    <ul>
        <li>In the search box, type the word or part of the word you are looking for and click the "Go" button.</li>
        <li>Change the search from “Starts with” to “Contains” to find all terms in the dictionary that include a word or set of letters.  For example, type “mutation” to find “de novo mutation” and “deleterious mutation”.</li>
        <li>You can also click on a letter of the alphabet to browse through the dictionary or “All” to view it in its entirety.</li>
        <li>The search box has an <strong>autosuggest</strong> feature. When you type three or more letters, a list of up to 10 suggestions will pop up below the box.  Choose one of the suggestions or type more letters to find the term you want.</li>
        <li>Additional genetics terms and definitions are available in the <a href="http://www.genome.gov/Glossary/">Talking Glossary of Genetics Terms</a> on the <a href="http://www.genome.gov/">National Human Genome Research Institute’s website</a>. Many terms in the Talking Glossary of Genetics Terms are accompanied by illustrations, animations, and descriptions by specialists in the field of genetics.</li>
    </ul>
</div>
<asp:Literal runat="server" ID="litSearchBlock"></asp:Literal>